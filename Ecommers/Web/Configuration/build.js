const glob = require("glob");
const path = require("path");
const esbuild = require("esbuild");
const fs = require("fs");

// ========================================
// CONFIGURACIÓN DE RUTAS
// ========================================
const baseDirDomain = path.resolve(__dirname, "../../wwwroot/js/domain");
const outputDir = path.resolve(__dirname, "../../wwwroot/js/bundle");
const nodeModulesDir = path.resolve(__dirname, "./node_modules");
const cssOutputDir = path.resolve(__dirname, "../../wwwroot/css");

// ========================================
// FUNCIÓN PARA COPIAR CSS
// ========================================
function copyCssFile(source, destination, libraryName) {
    if (fs.existsSync(source)) {
        try {
            fs.copyFileSync(source, destination);
            console.log(`✅ CSS de ${libraryName} copiado`);
            return true;
        } catch (error) {
            console.error(`⚠️ Error al copiar ${libraryName}:`, error.message);
            return false;
        }
    } else {
        console.warn(`⚠️ No se encontró CSS de ${libraryName}`);
        return false;
    }
}

// ========================================
// VERIFICACIÓN Y CREACIÓN DE DIRECTORIOS
// ========================================
console.log("📂 Verificando directorios:");
console.log("   Domain:", baseDirDomain, "- Existe:", fs.existsSync(baseDirDomain));

// Crear directorios de salida si no existen
[outputDir, cssOutputDir].forEach(dir => {
    if (!fs.existsSync(dir)) {
        fs.mkdirSync(dir, { recursive: true });
        console.log(`✅ Directorio creado: ${dir}`);
    }
});

// ========================================
// BUSCAR ARCHIVOS A PROCESAR
// ========================================
// Convertir rutas para glob (normalizar separadores)
const normalizedBasePath = baseDirDomain.replace(/\\/g, "/");

// Buscar archivos .ts y .js recursivamente
const tsFiles = glob.sync(`${normalizedBasePath}/**/*.ts`);

// Combinar todos los archivos
const allInputFiles = [...tsFiles];

console.log(`\n📦 Archivos encontrados:`);
console.log(`   TypeScript: ${tsFiles.length}`);
console.log(`   Total: ${allInputFiles.length}`);

if (allInputFiles.length === 0) {
    console.error("\n❌ No se encontraron archivos en domain");
    console.log("💡 Verifica que existan archivos .ts o .js en:", baseDirDomain);
    process.exit(1);
}

// Mostrar algunos archivos encontrados (para debug)
console.log("\n📄 Primeros archivos detectados:");
allInputFiles.slice(0, 5).forEach(file => {
    console.log(`   - ${path.basename(file)}`);
});
if (allInputFiles.length > 5) {
    console.log(`   ... y ${allInputFiles.length - 5} más`);
}

// ========================================
// COPIAR ARCHIVOS CSS DE DEPENDENCIAS
// ========================================
console.log("\n📋 Copiando archivos CSS:");

const cssFiles = [
    {
        source: path.resolve(nodeModulesDir, "notyf/notyf.min.css"),
        dest: path.resolve(cssOutputDir, "notyf.min.css"),
        name: "Notyf"
    },
    {
        source: path.resolve(nodeModulesDir, "datatables.net-dt/css/dataTables.dataTables.min.css"),
        dest: path.resolve(cssOutputDir, "dataTables.dataTables.min.css"),
        name: "DataTables"
    },
    {
        source: path.resolve(nodeModulesDir, "datatables.net-responsive-dt/css/responsive.dataTables.min.css"),
        dest: path.resolve(cssOutputDir, "responsive.dataTables.min.css"),
        name: "DataTables Responsive"
    },
    {
        source: path.resolve(nodeModulesDir, "sweetalert2/dist/sweetalert2.min.css"),
        dest: path.resolve(cssOutputDir, "sweetalert2.min.css"),
        name: "SweetAlert2"
    },
    {
        source: path.resolve(nodeModulesDir, "shepherd.js/dist/css/shepherd.css"),
        dest: path.resolve(cssOutputDir, "shepherd.css"),
        name: "Shepherd.js"
    },
    {
        source: path.resolve(nodeModulesDir, "swiper/swiper-bundle.min.css"),
        dest: path.resolve(cssOutputDir, "swiper-bundle.min.css"),
        name: "Swiper"
    }
];

cssFiles.forEach(({ source, dest, name }) => {
    copyCssFile(source, dest, name);
});

// ========================================
// BUILD: COMPILAR ARCHIVOS
// ========================================
async function buildDomain() {
    console.log("\n🔨 Compilando archivos Domain...");

    try {
        await esbuild.build({
            entryPoints: allInputFiles,
            outdir: outputDir,
            bundle: true,
            format: "esm",
            minify: true,
            sourcemap: true,
            target: ["es2020"],
            outExtension: { ".js": ".js" },
            nodePaths: [nodeModulesDir],
            absWorkingDir: path.resolve(__dirname),
            // Mantener estructura de carpetas (opcional)
            outbase: baseDirDomain,
            // Configuración adicional para resolver módulos
            resolveExtensions: [".ts", ".js", ".tsx", ".jsx"],
        });

        console.log("✅ Domain compilado exitosamente");
        console.log(`📍 Archivos de salida en: ${outputDir}`);

        // Listar archivos generados
        const outputFiles = fs.readdirSync(outputDir);
        console.log(`📦 Archivos generados: ${outputFiles.length}`);

    } catch (error) {
        throw error;
    }
}

// ========================================
// EJECUTAR BUILD
// ========================================
(async () => {
    try {
        await buildDomain();
        console.log("\n🎉 ¡Compilación completada con éxito!");
    } catch (err) {
        console.error("\n❌ Error al compilar:");
        console.error(err);
        process.exit(1);
    }
})();