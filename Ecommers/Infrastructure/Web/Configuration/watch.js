const glob = require("glob");
const path = require("path");
const esbuild = require("esbuild");
const fs = require("fs");

// Rutas base
const baseDir = path.resolve(__dirname, "../../../wwwroot/js/domain");
const outputDir = path.resolve(__dirname, "../../../wwwroot/js/bundle");
const nodeModulesDir = path.resolve(__dirname, "./node_modules");
const cssOutputDir = path.resolve(__dirname, "../../../wwwroot/css");

// Verificar que el directorio existe
console.log("📂 Verificando directorio base:", baseDir);
console.log("   ¿Existe?", fs.existsSync(baseDir));

// Convertir backslashes a forward slashes para glob
const pattern = baseDir.replace(/\\/g, '/') + '/**/*.ts';
console.log("🔍 Patrón de búsqueda:", pattern);

// Buscar archivos con glob
const inputFiles = glob.sync(pattern);

console.log(`📦 Encontrados ${inputFiles.length} archivos TypeScript`);
if (inputFiles.length > 0) {
    console.log("📄 Archivos encontrados:");
    inputFiles.forEach(file => console.log("   -", file));
}

console.log("📁 Directorio de salida:", outputDir);
console.log("📦 node_modules en:", nodeModulesDir);

// Si no hay archivos, salir
if (inputFiles.length === 0) {
    console.error("\n❌ No se encontraron archivos TypeScript");
    console.log("💡 Verifica que existan archivos .ts en:", baseDir);
    process.exit(1);
}

// Asegurar que los directorios de salida existen
if (!fs.existsSync(outputDir)) {
    fs.mkdirSync(outputDir, { recursive: true });
    console.log("✅ Directorio de salida JS creado");
}

if (!fs.existsSync(cssOutputDir)) {
    fs.mkdirSync(cssOutputDir, { recursive: true });
    console.log("✅ Directorio de salida CSS creado");
}

// Copiar CSS de Notyf
const notyfCssSource = path.resolve(nodeModulesDir, "notyf/notyf.min.css");
const notyfCssDestination = path.resolve(cssOutputDir, "notyf.min.css");

if (fs.existsSync(notyfCssSource)) {
    try {
        fs.copyFileSync(notyfCssSource, notyfCssDestination);
        console.log("✅ CSS de Notyf copiado exitosamente");
    } catch (error) {
        console.error("⚠️  Error al copiar CSS de Notyf:", error.message);
    }
} else {
    console.log("⚠️  No se encontró notyf.min.css - ¿Está instalado notyf?");
}


// --- Copiar archivos de Jquery ---
console.log("\n📦 Copiando archivos de Jquery...");
const jquerysSources = [
    { src: "jquery/dist", dest: path.join(outputDir, "jquery") },
];


jquerysSources.forEach(pkg => {
    const sourceDir = path.join(nodeModulesDir, pkg.src);
    if (fs.existsSync(sourceDir)) {
        console.log("   ✔ Copiando", pkg.src);
        copyFolderRecursive(sourceDir, pkg.dest);
    } else {
        console.log("   ⚠ No encontrado:", pkg.src);
    }
});


// --- Copiar archivos de DataTables ---
console.log("\n📦 Copiando archivos de DataTables...");

const dataTablesSources = [
    { src: "datatables.net/js", dest: path.join(outputDir, "datatables") },
    { src: "datatables.net-dt/js", dest: path.join(outputDir, "datatables-dt") },
    { src: "datatables.net-dt/css", dest: path.join(cssOutputDir, "datatables-dt") },
];



dataTablesSources.forEach(pkg => {
    const sourceDir = path.join(nodeModulesDir, pkg.src);
    if (fs.existsSync(sourceDir)) {
        console.log("   ✔ Copiando", pkg.src);
        copyFolderRecursive(sourceDir, pkg.dest);
    } else {
        console.log("   ⚠ No encontrado:", pkg.src);
    }
});

console.log("✅ DataTables copiado correctamente.");

// Crea carpeta y copia archivos recursivamente
function copyFolderRecursive(srcDir, destDir) {
    if (!fs.existsSync(destDir)) {
        fs.mkdirSync(destDir, { recursive: true });
    }

    const files = fs.readdirSync(srcDir);
    files.forEach(file => {
        const srcPath = path.join(srcDir, file);
        const destPath = path.join(destDir, file);

        if (fs.lstatSync(srcPath).isDirectory()) {
            copyFolderRecursive(srcPath, destPath);
        } else {
            fs.copyFileSync(srcPath, destPath);
        }
    });
}


// Compilar con esbuild
esbuild.build({
    entryPoints: inputFiles,
    outdir: outputDir,
    bundle: true,
    format: "esm",
    minify: true,
    sourcemap: true,
    target: ["es2020"],
    outExtension: { '.js': '.js' },
    nodePaths: [nodeModulesDir],
    absWorkingDir: path.resolve(__dirname),
})
    .then(() => console.log("\n✅ JavaScript compilado exitosamente"))
    .catch((error) => {
        console.error("\n❌ Error al compilar JavaScript:", error);
        process.exit(1);
    });

