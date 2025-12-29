const glob = require("glob");
const path = require("path");
const esbuild = require("esbuild");
const fs = require("fs");

// Rutas base
const baseDirDomain = path.resolve(__dirname, "../../wwwroot/js/domain");
const outputDir = path.resolve(__dirname, "../../wwwroot/js/bundle");
const nodeModulesDir = path.resolve(__dirname, "./node_modules");
const cssOutputDir = path.resolve(__dirname, "../../wwwroot/css");

// Verificar que los directorios existen
console.log("📂 Verificando directorios:");
console.log("   Domain:", baseDirDomain, "- Existe:", fs.existsSync(baseDirDomain));

// Convertir rutas para glob
const patternDomain = baseDirDomain.replace(/\\/g, "/") + "/**/*.ts";

console.log("🔍 Patrón de búsqueda Domain:", patternDomain);

// Buscar archivos TS en domain
const inputFilesDomain = glob.sync(patternDomain);

console.log(`\n📦 Domain: ${inputFilesDomain.length} archivos TypeScript`);
if (inputFilesDomain.length === 0) {
    console.error("\n❌ No se encontraron archivos TypeScript en domain");
    console.log("💡 Verifica que existan archivos .ts en:", baseDirDomain);
    process.exit(1);
}

console.log("📁 Directorio de salida:", outputDir);
console.log("📦 node_modules:", nodeModulesDir);

// Crear directorios de salida
if (!fs.existsSync(outputDir)) {
    fs.mkdirSync(outputDir, { recursive: true });
    console.log("✅ Directorio JS creado");
}

if (!fs.existsSync(cssOutputDir)) {
    fs.mkdirSync(cssOutputDir, { recursive: true });
    console.log("✅ Directorio CSS creado");
}

// Copiar CSS de Notyf
const notyfCssSource = path.resolve(nodeModulesDir, "notyf/notyf.min.css");
const notyfCssDestination = path.resolve(cssOutputDir, "notyf.min.css");

if (fs.existsSync(notyfCssSource)) {
    try {
        fs.copyFileSync(notyfCssSource, notyfCssDestination);
        console.log("✅ CSS de Notyf copiado");
    } catch (error) {
        console.error("⚠️ Error al copiar Notyf:", error.message);
    }
} else {
    console.log("⚠️ No se encontró notyf.min.css");
}

// Ruta de node_modules
const dataTablesCssSource = path.resolve(nodeModulesDir, "datatables.net-dt/css/dataTables.dataTables.min.css");

// Ruta de salida en wwwroot
const dataTablesCssDestination = path.resolve(cssOutputDir, "dataTables.dataTables.min.css");

// Ruta de node_modules
const responsivedataTablesCssSource = path.resolve(nodeModulesDir, "datatables.net-responsive-dt/css/responsive.dataTables.min.css");

// Ruta de salida en wwwroot
const responsivedataTablesCssDestination = path.resolve(cssOutputDir, "responsive.dataTables.min.css");

if (fs.existsSync(dataTablesCssSource)) {
    fs.copyFileSync(dataTablesCssSource, dataTablesCssDestination);
    console.log("✅ CSS de DataTables copiado");
} else {
    console.warn("⚠️ No se encontró CSS de DataTables");
}

if (fs.existsSync(responsivedataTablesCssSource)) {
    fs.copyFileSync(responsivedataTablesCssSource, responsivedataTablesCssDestination);
    console.log("✅ CSS de DataTables copiado");
} else {
    console.warn("⚠️ No se encontró CSS de DataTables");
}

const sweetAlertCssSource = path.resolve(nodeModulesDir, "sweetalert2/dist/sweetalert2.min.css");
const sweetAlertCssDestination = path.resolve(cssOutputDir, "sweetalert2.min.css");

if (fs.existsSync(sweetAlertCssSource)) {
    fs.copyFileSync(sweetAlertCssSource, sweetAlertCssDestination);
    console.log("✅ CSS de SweetAlert2 copiado");
}

// ========================================
// BUILD 1: Domain (sin jQuery)
// ========================================
async function buildDomain() {
    console.log("\n🔨 Compilando DOMAIN...");

    // En la sección BUILD 1: Domain
    await esbuild.build({
        entryPoints: [
            ...inputFilesDomain,
            //path.resolve(__dirname, "../../wwwroot/js/domain/utils/zod-generic.ts"),
            path.resolve(__dirname, "../../wwwroot/js/domain/vendors_datatables.js"),
            path.resolve(__dirname, "../../wwwroot/js/domain/vendors_dayjs.js"),
            path.resolve(__dirname, "../../wwwroot/js/domain/vendors_sweetalert.ts"),
        ],
        outdir: outputDir,
        bundle: true,
        format: "esm",
        minify: true,
        sourcemap: true,
        target: ["es2020"],
        outExtension: { ".js": ".js" },
        nodePaths: [nodeModulesDir],
        absWorkingDir: path.resolve(__dirname),
    });

    console.log("✅ Domain compilado");
}

// ========================================
// EJECUTAR BUILDS
// ========================================
(async () => {
    try {
        await buildDomain();
        console.log("\n🎉 ¡Compilación completada!");
        console.log("📍 Archivos en:", outputDir);
    } catch (err) {
        console.error("\n❌ Error al compilar:", err);
        process.exit(1);
    }
})();