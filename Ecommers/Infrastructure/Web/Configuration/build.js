const glob = require("glob");
const path = require("path");
const esbuild = require("esbuild");
const fs = require("fs");

// ========================================
// CONFIGURACIÓN DE RUTAS
// ========================================
const baseDirDomain = path.resolve(__dirname, "../../../wwwroot/js/domain");
const outputDir = path.resolve(__dirname, "../../../wwwroot/js/bundle");
const nodeModulesDir = path.resolve(__dirname, "./node_modules");
const cssOutputDir = path.resolve(__dirname, "../../../wwwroot/css");

// ========================================
// LIMPIAR DIRECTORIO DE SALIDA
// ========================================
if (fs.existsSync(outputDir)) {
    fs.rmSync(outputDir, { recursive: true, force: true });
}
fs.mkdirSync(outputDir, { recursive: true });

// ========================================
// FUNCIÓN PARA COPIAR CSS
// ========================================
function copyCssFile(source, destination, libraryName) {
    if (!fs.existsSync(source)) {
        console.warn(`⚠️ No se encontró CSS de ${libraryName}`);
        return;
    }
    fs.mkdirSync(path.dirname(destination), { recursive: true });
    fs.copyFileSync(source, destination);
    console.log(`✅ CSS de ${libraryName} copiado`);
}

// ========================================
// BUSCAR ARCHIVOS TYPESCRIPT
// ========================================
const normalizedBasePath = baseDirDomain.replace(/\\/g, "/");
const allEntryPoints = glob.sync(`${normalizedBasePath}/**/*.ts`);

// SEPARAR vendors de otros archivos
const vendorsSwiper = allEntryPoints.find(f => f.includes("vendors_swiper.ts"));
const regularEntryPoints = allEntryPoints.filter(f => !f.includes("vendors_swiper.ts"));

console.log("\n📦 Archivos TypeScript encontrados:");
console.log("  - Regular:", regularEntryPoints.length);
console.log("  - Vendors:", vendorsSwiper ? 1 : 0);

if (allEntryPoints.length === 0) {
    console.error("❌ No se encontraron archivos .ts en domain");
    process.exit(1);
}

// ========================================
// COPIAR CSS DE DEPENDENCIAS
// ========================================
console.log("\n📋 Copiando CSS de dependencias...");

[
    {
        src: "notyf/notyf.min.css",
        dest: "notyf.min.css",
        name: "Notyf"
    },
    {
        src: "datatables.net-dt/css/dataTables.dataTables.min.css",
        dest: "dataTables.dataTables.min.css",
        name: "DataTables"
    },
    {
        src: "datatables.net-responsive-dt/css/responsive.dataTables.min.css",
        dest: "responsive.dataTables.min.css",
        name: "DataTables Responsive"
    },
    {
        src: "sweetalert2/dist/sweetalert2.min.css",
        dest: "sweetalert2.min.css",
        name: "SweetAlert2"
    },
    {
        src: "shepherd.js/dist/css/shepherd.css",
        dest: "shepherd.css",
        name: "Shepherd.js"
    },
    {
        src: "swiper/swiper-bundle.min.css",
        dest: "swiper-bundle.min.css",
        name: "Swiper"
    },
    {
        src: "select2/dist/css/select2.min.css",
        dest: "select2.min.css",
        name: "Select2"
    }
].forEach(lib => {
    copyCssFile(
        path.resolve(nodeModulesDir, lib.src),
        path.resolve(cssOutputDir, lib.dest),
        lib.name
    );
});

// ========================================
// HELPER: RESOLVER MÓDULOS
// ========================================
function resolveNodeModule(moduleName) {
    let packageName, subPath;

    if (moduleName.startsWith('@')) {
        const parts = moduleName.split('/');
        packageName = `${parts[0]}/${parts[1]}`;
        subPath = parts.slice(2).join('/');
    } else {
        const parts = moduleName.split('/');
        packageName = parts[0];
        subPath = parts.slice(1).join('/');
    }

    const packagePath = path.resolve(nodeModulesDir, packageName);

    if (!fs.existsSync(packagePath)) {
        return null;
    }

    if (subPath) {
        const possiblePaths = [
            path.join(packagePath, subPath),
            path.join(packagePath, `${subPath}.js`),
            path.join(packagePath, `${subPath}.mjs`),
            path.join(packagePath, `${subPath}.ts`),
            path.join(packagePath, subPath, 'index.js'),
            path.join(packagePath, subPath, 'index.mjs'),
            path.join(packagePath, subPath, 'index.ts')
        ];

        for (const p of possiblePaths) {
            if (fs.existsSync(p)) {
                return p;
            }
        }
    }

    const packageJsonPath = path.join(packagePath, "package.json");
    if (fs.existsSync(packageJsonPath)) {
        try {
            const packageJson = JSON.parse(fs.readFileSync(packageJsonPath, "utf8"));

            const entryPoints = [
                packageJson.module,
                packageJson.main,
                packageJson.browser,
                "index.js",
                "index.mjs",
                "index.ts"
            ];

            for (const entry of entryPoints) {
                if (!entry) continue;

                const entryPath = path.join(packagePath, entry);
                if (fs.existsSync(entryPath)) {
                    return entryPath;
                }
            }
        } catch (err) {
            console.warn(`⚠️ Error leyendo package.json de ${packageName}`);
        }
    }

    const defaultFiles = ["index.js", "index.mjs", "index.ts", "dist/index.js"];
    for (const file of defaultFiles) {
        const fullPath = path.join(packagePath, file);
        if (fs.existsSync(fullPath)) {
            return fullPath;
        }
    }

    return null;
}

// ========================================
// PLUGIN COMPARTIDO
// ========================================
const resolvePlugin = {
    name: "resolve-node-modules",
    setup(build) {
        build.onResolve({ filter: /^[^.]/ }, args => {
            if (args.path.startsWith('.') || args.path.startsWith('/')) {
                return null;
            }

            const resolved = resolveNodeModule(args.path);

            if (resolved) {
                return { path: resolved };
            }

            return null;
        });
    }
};

// ========================================
// BUILD VENDORS SWIPER (IIFE - GLOBAL)
// ========================================
async function buildVendorsSwiper() {
    console.log("\n🔨 Preparando vendors_swiper...");

    // Buscar el bundle IIFE/UMD de Swiper que ya está compilado
    const swiperBundlePath = path.resolve(nodeModulesDir, 'swiper/swiper-bundle.js');
    
    if (!fs.existsSync(swiperBundlePath)) {
        console.error('❌ No se encontró swiper-bundle.js');
        console.log('Intentando con swiper-bundle.min.js...');
        
        const swiperMinPath = path.resolve(nodeModulesDir, 'swiper/swiper-bundle.min.js');
        if (fs.existsSync(swiperMinPath)) {
            // Copiar directamente el minificado
            fs.copyFileSync(
                swiperMinPath,
                path.join(outputDir, "vendors_swiper.js")
            );
            console.log("✅ Swiper bundle copiado (minificado)");
            return;
        }
        
        console.error('❌ No se encontró ningún bundle de Swiper');
        return;
    }

    // Leer el bundle original
    let swiperCode = fs.readFileSync(swiperBundlePath, 'utf8');
    
    // El bundle de Swiper ya expone window.Swiper, solo necesitamos asegurarnos
    const wrappedCode = `${swiperCode}

// Verificación y evento de carga
if (typeof window.Swiper === 'function') {
    console.log('✅ Swiper cargado correctamente:', typeof window.Swiper);
    window.dispatchEvent(new Event('swiperLoaded'));
} else {
    console.error('❌ Swiper no se cargó correctamente:', typeof window.Swiper);
}
`;

    // Guardar
    fs.writeFileSync(
        path.join(outputDir, "vendors_swiper.js"),
        wrappedCode
    );
    
    console.log("✅ vendors_swiper preparado");
}

// ========================================
// BUILD DOMAIN (ESM - RESTO DE ARCHIVOS)
// ========================================
async function buildDomain() {
    if (regularEntryPoints.length === 0) {
        console.log("\n⚠️ No hay archivos regulares para compilar");
        return;
    }

    console.log("\n🔨 Compilando archivos regulares...");

    await esbuild.build({
        entryPoints: regularEntryPoints,
        outdir: outputDir,
        bundle: true,
        format: "esm",
        target: ["es2020"],
        minify: true,
        sourcemap: true,
        outbase: baseDirDomain,
        platform: "browser",
        resolveExtensions: [".ts", ".js", ".mjs"],
        loader: {
            ".ts": "ts"
        },
        absWorkingDir: path.resolve(__dirname),
        logLevel: "info",
        plugins: [resolvePlugin]
    });

    console.log("✅ Archivos regulares compilados");
}

// ========================================
// EJECUTAR
// ========================================
(async () => {
    try {
        await buildVendorsSwiper(); // Primero vendors como IIFE
        await buildDomain();         // Luego el resto como ESM
        console.log("\n🎉 Build completado correctamente");
        console.log("📍 Output:", outputDir);
    } catch (err) {
        console.error("\n❌ Error en el build");
        console.error(err);
        process.exit(1);
    }
})();