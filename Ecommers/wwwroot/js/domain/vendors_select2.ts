// vendors_select2.js - Bundle de jQuery + Select2
import $ from "jquery";

// CRÍTICO: Exponer jQuery globalmente ANTES de importar Select2
window.$ = $;
window.jQuery = $;

// Importar Select2 CSS
import "select2/dist/css/select2.min.css";

// Importar Select2 JS - Esto debe ejecutarse DESPUÉS de exponer jQuery
// Select2 busca jQuery en window.jQuery
import select2 from "select2";

// Inicializar Select2 manualmente si no se cargó automáticamente
if (typeof $.fn.select2 === 'undefined' && typeof select2 === 'function') {
    select2($);
}

// Verificar carga
if (typeof $.fn.select2 !== 'undefined') {
    console.log('✓ Select2 cargado correctamente');

    // Configuración global en español
    try {
        $.fn.select2.defaults.set("language", {
            errorLoading: () => "No se pudieron cargar los resultados.",
            inputTooShort: (args) => {
                const remainingChars = args.minimum - args.input.length;
                return `Por favor, introduzca ${remainingChars} o más caracteres`;
            },
            loadingMore: () => "Cargando más resultados…",
            maximumSelected: (args) => {
                return `Sólo puede seleccionar ${args.maximum} elemento${args.maximum !== 1 ? 's' : ''}`;
            },
            noResults: () => "No se encontraron resultados",
            searching: () => "Buscando…",
            removeAllItems: () => "Eliminar todos los elementos"
        });
    } catch (e) {
        console.warn('No se pudo configurar el idioma de Select2:', e);
    }
} else {
    console.error('❌ Select2 NO se cargó correctamente');
    console.log('jQuery disponible:', typeof $ !== 'undefined');
    console.log('jQuery global:', typeof window.jQuery !== 'undefined');
}

export default $;