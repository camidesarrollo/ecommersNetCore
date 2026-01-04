// select2-init.js
import $ from "../../bundle/vendors_select2.js";

// Verificar que Select2 esté disponible
if (typeof $.fn.select2 === 'undefined') {
    console.error('Select2 no está cargado correctamente');
} else {
    console.log('Select2 cargado correctamente');
}

// Esperar a que el DOM esté listo
$(document).ready(() => {
    console.log('DOM listo, inicializando Select2...');

    // Buscar todos los select con clase .select2
    const $selects = $("select.select2");
    console.log(`Encontrados ${$selects.length} elementos select.select2`);

    $selects.each(function () {
        const $select = $(this);

        // Verificar que el elemento existe y que select2 está disponible
        if (!$.fn.select2) {
            console.error('$.fn.select2 no está definido');
            return;
        }

        try {
            // Determinar si permite crear nuevas opciones
            const allowTags = $select.attr("data-tags") === "true" ||
                $select.data("tags") === true ||
                $select.hasClass("select2-tags");

            const options = {
                placeholder: $select.attr("data-placeholder") || "Selecciona una opción",
                tags: allowTags, // Permite crear nuevas opciones
                tokenSeparators: allowTags ? [',', ';'] : [], // Separadores para crear múltiples tags
                allowClear: true,
                width: "100%",
                language: {
                    noResults: function () {
                        return "No se encontraron resultados";
                    },
                    searching: function () {
                        return "Buscando...";
                    },
                    inputTooShort: function (args) {
                        return `Ingrese al menos ${args.minimum} caracteres`;
                    }
                },
                // Permitir crear opciones personalizadas
                createTag: function (params) {
                    const term = $.trim(params.term);

                    if (term === '') {
                        return null;
                    }

                    // Verificar si ya existe
                    const existingOption = $(this).find('option').filter(function () {
                        return $(this).text().toLowerCase() === term.toLowerCase();
                    });

                    if (existingOption.length > 0) {
                        return null;
                    }

                    return {
                        id: term,
                        text: term,
                        newTag: true // Marca para identificar nuevas opciones
                    };
                },
                // Personalizar cómo se muestra la nueva opción
                templateResult: function (data) {
                    if (data.loading) {
                        return data.text;
                    }

                    // Si es una nueva opción, mostrar indicador
                    if (data.newTag) {
                        return $('<span><i class="fa fa-plus-circle"></i> Crear: <strong>' + data.text + '</strong></span>');
                    }

                    return data.text;
                },
                // Insertar las nuevas opciones al inicio de la lista
                insertTag: function (data, tag) {
                    data.unshift(tag);
                }
            };

            console.log('Inicializando Select2 en:', $select.attr('id') || $select.attr('name'),
                '| Tags:', allowTags);

            $select.select2(options);

            // Evento cuando se selecciona una opción
            $select.on("select2:select", function (e) {
                const data = e.params.data;

                // Si es una nueva opción creada
                if (data.newTag || data.id === data.text) {
                    console.log("Nueva opción creada:", data.text);

                    // Agregar la opción al select de forma permanente
                    const $newOption = $('<option>')
                        .val(data.id)
                        .text(data.text)
                        .attr('selected', true);

                    $select.append($newOption);

                    // Disparar evento personalizado para que puedas manejarlo
                    $select.trigger('new-option-created', [data]);
                }
            });

            // Evento para limpiar la selección
            $select.on("select2:clear", function () {
                console.log("Selección limpiada en:", $select.attr('id') || $select.attr('name'));
            });

            // Evento para remover una opción (en modo múltiple)
            $select.on("select2:unselect", function (e) {
                console.log("Opción removida:", e.params.data.text);
            });

            $(".select2.select2-container.select2-container--default").each(function () {
                $(this).addClass(
                    "w-full px-4 py-3 text-lg border-2 border-gray-300 rounded-lg " +
                    "focus:border-olive-green-500 focus:ring-olive-green-500 outline-none transition"
                );
            });

            $(".select2-selection.select2-selection--single").each(function () {
                $(this).attr("style", "border: none !important");
            });
            $(".select2-container--default .select2-selection--single .select2-selection__arrow b").each(function () {
                $(this).attr("style", "top: 100% !important");
            });
            
        } catch (error) {
            console.error('Error al inicializar Select2:', error);
        }
    });
});

// Función helper para escuchar cuando se crean nuevas opciones
export function onNewOptionCreated(selector, callback) {
    $(selector).on('new-option-created', function (e, data) {
        callback(data);
    });
}

// Función helper para agregar una opción programáticamente
export function addOption(selector, id, text, selected = false) {
    const $select = $(selector);

    if ($select.length === 0) {
        console.warn('Elemento no encontrado:', selector);
        return;
    }

    // Verificar si la opción ya existe
    if ($select.find(`option[value="${id}"]`).length > 0) {
        console.warn('La opción ya existe:', id);
        return;
    }

    const $option = $('<option>')
        .val(id)
        .text(text)
        .prop('selected', selected);

    $select.append($option).trigger('change');

    console.log('Opción agregada:', text);
}

// Función helper para obtener todas las opciones
export function getOptions(selector) {
    const $select = $(selector);
    const options = [];

    $select.find('option').each(function () {
        options.push({
            id: $(this).val(),
            text: $(this).text()
        });
    });

    return options;
}

export default $;