(() => {
    /* --------------------------- Categorías --------------------------- */
    const categorias = [
        { id: 1, name: 'Frutos Secos', icon: '🥜' },
        { id: 2, name: 'Semillas', icon: '🌰' },
        { id: 3, name: 'Frutas Deshidratadas', icon: '🍇' },
        { id: 4, name: 'Mix Saludables', icon: '🥗' },
        { id: 5, name: 'Snacks', icon: '🍿' },
        { id: 6, name: 'Superfoods', icon: '✨' }
    ];

    /* Desktop categorías */
    const desktopCategorias = document.getElementById("desktop-categorias");
    if (desktopCategorias) {
        desktopCategorias.innerHTML = categorias.map(c => `
            <a href="/productos?categoria=${c.id}"
               class="item_categoria block px-4 py-2.5 text-gray-700 hover:bg-yellow-50 hover:text-yellow-600">
                <span class="mr-2">${c.icon}</span>${c.name}
            </a>
        `).join("");
    }

    /* Mobile categorías */
    const mobileCategorias = document.getElementById("mobile-categorias");
    if (mobileCategorias) {
        mobileCategorias.innerHTML = categorias.map(c => `
            <a href="/productos?categoria=${c.id}"
               class="flex items-center gap-3 px-6 py-3 text-gray-600 hover:bg-yellow-50">
                <span class="text-lg ml-8">${c.icon}</span>
                <span class="text-sm">${c.name}</span>
            </a>
        `).join("");
    }

    /* --------------------------- Desktop dropdown --------------------------- */
    const productsBtnDesktop = document.getElementById("products-desktop");
    const productsDropdown = document.getElementById("products-dropdown");

    if (productsBtnDesktop && productsDropdown) {
        let dropdownTimeout = null;

        productsBtnDesktop.addEventListener("mouseenter", () => {
            clearTimeout(dropdownTimeout);
            productsDropdown.classList.remove("hidden", "opacity-0");
            productsDropdown.classList.add("opacity-100", "translate-y-0");
        });

        productsBtnDesktop.addEventListener("mouseleave", () => {
            dropdownTimeout = setTimeout(() => {
                const isHoveringCategoria =
                    document.querySelector(".item_categoria:hover");

                if (!isHoveringCategoria) {
                    productsDropdown.classList.remove(
                        "opacity-100",
                        "translate-y-0"
                    );
                    productsDropdown.classList.add("opacity-0", "hidden");
                }
            }, 200);
        });

        /* Cuando el mouse sale completamente */
        productsDropdown.addEventListener("mouseleave", () => {
            productsDropdown.classList.add("opacity-0", "hidden");
        });
    }

    /* --------------------------- Mobile menu --------------------------- */
    const btnMobile = document.getElementById("btn-mobile-menu");
    const mobileMenu = document.getElementById("mobile-menu");
    const mobileOverlay = document.getElementById("mobile-overlay");
    const closeMobile = document.getElementById("close-mobile-menu");
    const btnMobileProducts = document.getElementById("btn-mobile-products");
    const mobileProductsSubmenu = document.getElementById("mobile-products-submenu");

    if (
        btnMobile &&
        mobileMenu &&
        mobileOverlay &&
        closeMobile &&
        btnMobileProducts &&
        mobileProductsSubmenu
    ) {
        const chevronMobile = btnMobileProducts.querySelector("i");

        /* Abrir */
        btnMobile.addEventListener("click", () => {
            mobileMenu.classList.remove("hidden");
            mobileOverlay.classList.remove("hidden");

            requestAnimationFrame(() => {
                mobileMenu.classList.remove("-translate-x-full");
            });
        });

        /* Cerrar */
        const close = () => {
            mobileMenu.classList.add("-translate-x-full");
            mobileOverlay.classList.add("hidden");

            setTimeout(() => {
                mobileMenu.classList.add("hidden");
                mobileProductsSubmenu.classList.add("hidden");
                chevronMobile?.classList.remove("rotate-180");
            }, 300);
        };

        closeMobile.addEventListener("click", close);
        mobileOverlay.addEventListener("click", close);

        /* Submenu productos */
        btnMobileProducts.addEventListener("click", () => {
            mobileProductsSubmenu.classList.toggle("hidden");
            chevronMobile?.classList.toggle("rotate-180");
        });
    }
})();
