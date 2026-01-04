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
      // Determinar si es multiselect
      const isMulti = 
        $select.attr("data-multiselect") === "true" || 
        $select.data("multiselect") === true ||
        $select.prop("multiple") === true;
      
      // Determinar si permite crear nuevas opciones
      const allowTags = 
        $select.attr("data-tags") === "true" || 
        $select.data("tags") === true || 
        $select.hasClass("select2-tags");
      
      // Si es multiselect, asegurar atributo multiple
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
          if (term === '') return null;
          
          const termLower = term.toLowerCase();
          
          // Verificar duplicados en opciones existentes (case-insensitive)
          const existsInOptions = $select.find('option').filter(function () {
            const optVal = $(this).val();
            const optText = $(this).text();
            return (
              (optVal && optVal.toLowerCase() === termLower) ||
              (optText && optText.toLowerCase() === termLower)
            );
          }).length > 0;
          
          if (existsInOptions) {
            console.log('Tag ya existe como opción:', term);
            return null;
          }
          
          // Verificar duplicados en valores seleccionados (case-insensitive)
          const selectedValues = $select.val();
          let alreadySelected = false;
          
          if (Array.isArray(selectedValues)) {
            alreadySelected = selectedValues.some(v => 
              v && v.toLowerCase() === termLower
            );
          } else if (selectedValues) {
            alreadySelected = selectedValues.toLowerCase() === termLower;
          }
          
          if (alreadySelected) {
            console.log('Tag ya está seleccionado:', term);
            return null;
          }
          
          // Usar el término original (con su capitalización)
          return {
            id: term,
            text: term,
            newTag: true
          };
        },
        templateResult: function (data) {
          if (data.loading) return data.text;
          
          // Mostrar opción "Crear" solo si es nueva
          if (data.newTag) {
            return $(`
              <span style="color: #84cc16;">
                <i class="fa fa-plus-circle"></i>
                Crear: <strong>${$.fn.select2.amd.require('select2/utils').escapeMarkup(data.text)}</strong>
              </span>
            `);
          }
          
          return data.text;
        },
        insertTag: function (data, tag) {
          // Verificar una última vez antes de insertar
          const tagLower = tag.text.toLowerCase();
          
          const exists = data.some(item => 
            item.text && item.text.toLowerCase() === tagLower
          );
          
          if (!exists) {
            data.unshift(tag);
          }
        }
      };
      
      console.log('Inicializando Select2 en:', $select.attr('id') || $select.attr('name'), 
                  '| Multiselect:', isMulti, '| Tags:', allowTags);
      
      $select.select2(options);
      
      // Evento cuando se selecciona una opción
      $select.on("select2:select", function (e) {
        const data = e.params.data;
        
        // Si es una nueva opción creada con tags
        if (data.newTag) {
          console.log("Nueva opción creada:", data.text);
          
          // Verificar si la opción ya existe en el DOM
          const optionExists = $select.find(`option[value="${data.id}"]`).length > 0;
          
          if (!optionExists) {
            // Agregar la opción al select de forma permanente
            const $newOption = $('<option>')
              .val(data.id)
              .text(data.text)
              .prop('selected', true);
            
            $select.append($newOption);
            
            // Disparar evento personalizado
            $select.trigger('new-option-created', [data]);
          }
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
      
    } catch (error) {
      console.error('Error al inicializar Select2:', error);
    }
  });
  
  // Aplicar estilos después de inicializar todos los Select2
  setTimeout(() => {
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
    console.log('Estilos aplicados correctamente');
  }, 100);
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