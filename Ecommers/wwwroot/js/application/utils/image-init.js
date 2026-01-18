function addImageBase(config) {
    const {
        containerElement,
        indexRef,
        indexName,
        namePrefix,
        previewFn,
        updatePrimaryFn,
        removeFn,
        radioName,
        isPrimaryParam = false
    } = config;


    console.log(config);
    const container = containerElement;
    if (!container) {
        console.error(`Contenedor ${container} no encontrado`);
        return;
    }

    const currentIndex = window[indexRef]++;
    const isFirstImage = container.querySelectorAll(`[data-${indexName}-index]`).length === 0;
    const shouldBePrimary = isPrimaryParam || isFirstImage;

    const imageDiv = document.createElement('div');
    imageDiv.className =
        'border-2 border-olive-green-300 rounded-lg p-5 bg-white hover:border-olive-green-500 transition-all duration-300 shadow-sm hover:shadow-md';
    imageDiv.setAttribute(`data-${indexName}-index`, currentIndex);

    imageDiv.innerHTML = `
        <div class="flex items-start gap-4">
            <!-- Preview -->
            <div class="flex-shrink-0">
                <div class="w-28 h-28 border-2 border-gray-300 rounded-lg overflow-hidden bg-gray-50 flex items-center justify-center">
                    <img id="preview_${currentIndex}" class="w-full h-full object-cover hidden">
                    <i id="icon_${currentIndex}" class="fas fa-image text-4xl text-gray-400"></i>
                </div>
            </div>

            <!-- Controles -->
            <div class="flex-1 space-y-4">
                <div>
                    <label class="block font-semibold mb-2">
                        Archivo de imagen <span class="text-red-700">*</span>
                    </label>
                    <input type="file"
                        name="${namePrefix}[${currentIndex}].ImageFile"
                        accept="image/*"
                        required
                        onchange="${previewFn}(this, ${currentIndex})"
                        class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg focus:border-olive-green-500 outline-none transition
                               file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0
                               file:bg-olive-green-500 file:text-white file:font-semibold hover:file:bg-olive-green-600
                               file:cursor-pointer cursor-pointer">
                </div>

                <div class="grid grid-cols-2 gap-4">
                    <div>
                        <label class="block font-semibold mb-2 text-sm">
                            Orden <span class="text-red-700">*</span>
                        </label>
                        <input type="number"
                            name="${namePrefix}[${currentIndex}].SortOrder"
                            value="${currentIndex + 1}"
                            min="0"
                            required
                            class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg focus:border-olive-green-500 outline-none transition">
                    </div>

                    <div class="flex items-end">
                        <label class="flex items-center gap-2 cursor-pointer p-3 rounded-lg hover:bg-olive-green-50 transition-colors">
                            <input type="radio"
                                name="${radioName}"
                                value="${currentIndex}"
                                ${shouldBePrimary ? 'checked' : ''}
                                onchange="${updatePrimaryFn}(${currentIndex})"
                                class="w-5 h-5 text-olive-green-600 cursor-pointer">
                            <span class="text-sm font-semibold text-dark-chocolate">Imagen principal</span>
                        </label>
                    </div>
                </div>

                <input type="hidden"
                    name="${namePrefix}[${currentIndex}].IsPrimary"
                    id="isPrimary_${currentIndex}"
                    value="${shouldBePrimary}">
            </div>

            <button type="button"
                onclick="${removeFn}(${currentIndex})"
                class="flex-shrink-0 text-red-600 hover:text-white hover:bg-red-600 transition-all duration-200 p-3 rounded-lg">
                <i class="fas fa-trash-alt text-lg"></i>
            </button>
        </div>
    `;

    container.appendChild(imageDiv);

    setTimeout(() => {
        imageDiv.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }, 100);
}

window.addImageBase = addImageBase;