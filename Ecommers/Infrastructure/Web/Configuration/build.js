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
const entryPoints = glob.sync(`${normalizedBasePath}/**/*.ts`);

console.log("\n📦 Archivos TypeScript encontrados:", entryPoints.length);

if (entryPoints.length === 0) {
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
    // Separar el nombre del paquete de la ruta interna
    let packageName, subPath;
    
    if (moduleName.startsWith('@')) {
        // Paquetes con scope (@org/package)
        const parts = moduleName.split('/');
        packageName = `${parts[0]}/${parts[1]}`;
        subPath = parts.slice(2).join('/');
    } else {
        // Paquetes normales
        const parts = moduleName.split('/');
        packageName = parts[0];
        subPath = parts.slice(1).join('/');
    }

    const packagePath = path.resolve(nodeModulesDir, packageName);

    // Si no existe el paquete, retornar null
    if (!fs.existsSync(packagePath)) {
        return null;
    }

    // Si hay una ruta interna (subPath), intentar resolverla
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

    // Si no hay subPath, buscar el entry point del paquete
    const packageJsonPath = path.join(packagePath, "package.json");
    if (fs.existsSync(packageJsonPath)) {
        try {
            const packageJson = JSON.parse(fs.readFileSync(packageJsonPath, "utf8"));

            // Intentar con diferentes campos de entrada
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

    // Intentar archivos por defecto
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
// BUILD DOMAIN
// ========================================
async function buildDomain() {
    console.log("\n🔨 Compilando Domain...");

    await esbuild.build({
        entryPoints,
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
        plugins: [
            {
                name: "resolve-node-modules",
                setup(build) {
                    // Resolver imports desde node_modules
                    build.onResolve({ filter: /^[^.]/ }, args => {
                        // Solo procesar imports que no sean relativos
                        if (args.path.startsWith('.') || args.path.startsWith('/')) {
                            return null;
                        }

                        // Resolver el módulo
                        const resolved = resolveNodeModule(args.path);

                        if (resolved) {
                            return { path: resolved };
                        }

                        // Si no se resolvió, dejar que esbuild lo maneje
                        return null;
                    });
                }
            }
        ]
    });

    console.log("\n✅ Compilación finalizada");
    console.log("📍 Output:", outputDir);
}

// ========================================
// EJECUTAR
// ========================================
(async () => {
    try {
        await buildDomain();
        console.log("\n🎉 Build completado correctamente");
    } catch (err) {
        console.error("\n❌ Error en el build");
        console.error(err);
        process.exit(1);
    }
})();