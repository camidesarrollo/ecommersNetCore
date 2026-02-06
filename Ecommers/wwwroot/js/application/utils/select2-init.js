// select2-init.js
import $ from "../../bundle/vendors_select2.js";

// Verificar que Select2 esté disponible
if (typeof $.fn.select2 === 'undefined') {
  console.error('Select2 no está cargado correctamente');
} else {
  console.log('Select2 cargado correctamente');
}

export function initSelect2(context = document) {
  console.log('Inicializando Select2...');

  if (!$.fn.select2) {
    console.error('Select2 no está cargado');
    return;
  }

  const $selects = $(context).find("select.select2").filter(function () {
    // ❌ Evitar reinicializar
    return !$(this).hasClass("select2-hidden-accessible");
  });

  console.log(`Selects nuevos a inicializar: ${$selects.length}`);

  $selects.each(function () {
    const $select = $(this);

    try {
      const isMulti =
        $select.attr("data-multiselect") === "true" ||
        $select.data("multiselect") === true ||
        $select.prop("multiple") === true;

      const allowTags =
        $select.attr("data-tags") === "true" ||
        $select.data("tags") === true ||
        $select.hasClass("select2-tags");

      if (isMulti && !$select.prop("multiple")) {
        $select.prop("multiple", true);
      }

      const options = {
        placeholder: $select.attr("data-placeholder") || "Selecciona una opción",
        tags: allowTags,
        tokenSeparators: allowTags && isMulti ? [',', ';'] : [],
        allowClear: !isMulti,
        width: "100%",
        closeOnSelect: !isMulti,
        maximumSelectionLength: $select.data("max-selection") || 0,
        language: {
          noResults: () => "No se encontraron resultados",
          searching: () => "Buscando...",
          inputTooShort: args => `Ingrese al menos ${args.minimum} caracteres`,
          maximumSelected: args => `Solo puedes seleccionar ${args.maximum} opciones`
        },
        createTag: function (params) {
          if (!allowTags) return null;

          const term = $.trim(params.term);
          if (!term) return null;

          const termLower = term.toLowerCase();

          const exists = $select.find("option").filter(function () {
            return (
              $(this).val()?.toLowerCase() === termLower ||
              $(this).text()?.toLowerCase() === termLower
            );
          }).length > 0;

          if (exists) return null;

          return {
            id: term,
            text: term,
            newTag: true
          };
        },
        templateResult: function (data) {
          if (data.loading) return data.text;

          if (data.newTag) {
            return $(`
              <span style="color:#84cc16">
                <i class="fa fa-plus-circle"></i>
                Crear: <strong>${data.text}</strong>
              </span>
            `);
          }

          return data.text;
        },
        insertTag: function (data, tag) {
          const exists = data.some(
            item => item.text?.toLowerCase() === tag.text.toLowerCase()
          );
          if (!exists) data.unshift(tag);
        }
      };

      console.log(
        "Select2 →",
        $select.attr("id") || $select.attr("name"),
        "| Multi:", isMulti,
        "| Tags:", allowTags
      );

      $select.select2(options);

      // ===== data-selected =====
      const dataSelected = $select.attr("data-selected") || $select.data("selected");

      if (dataSelected) {
        if (isMulti) {
          const values = dataSelected.split(',').map(v => v.trim());
          $select.val(values).trigger("change");
        } else {
          $select.val(dataSelected.trim()).trigger("change");
        }
      }

      // ===== Eventos =====
      $select.on("select2:select", function (e) {
        if (e.params.data.newTag) {
          const val = e.params.data.id;
          if (!$select.find(`option[value="${val}"]`).length) {
            $select.append(
              $("<option>", {
                value: val,
                text: e.params.data.text,
                selected: true
              })
            );
          }
        }
      });

    } catch (err) {
      console.error("Error inicializando Select2:", err);
    }
  });

  // ===== Estilos =====
  setTimeout(() => {
    $(".select2-container--default")
      .not(".select2-styled")
      .addClass(
        "select2-styled w-full px-4 py-3 text-lg border-2 border-gray-300 rounded-lg transition"
      );

    $(".select2-selection--single").css("border", "none");
  }, 50);
}

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

// Función helper para obtener todas las opciones seleccionadas
export function getSelectedOptions(selector) {
  const $select = $(selector);
  const selected = [];
  
  $select.find('option:selected').each(function () {
    selected.push({
      id: $(this).val(),
      text: $(this).text()
    });
  });
  
  return selected;
}

// Función helper para obtener todas las opciones disponibles
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

// Función helper para limpiar todas las selecciones
export function clearSelection(selector) {
  const $select = $(selector);
  $select.val(null).trigger('change');
}

// Función helper para seleccionar opciones programáticamente
export function setSelectedOptions(selector, values) {
  const $select = $(selector);
  $select.val(values).trigger('change');
}

export default $;