/* --------------------------- Categorías --------------------------- */
const categorias = [
    { id: 1, name: 'Frutos Secos', icon: '🥜' },
    { id: 2, name: 'Semillas', icon: '🌰' },
    { id: 3, name: 'Frutas Deshidratadas', icon: '🍇' },
    { id: 4, name: 'Mix Saludables', icon: '🥗' },
    { id: 5, name: 'Snacks', icon: '🍿' },
    { id: 6, name: 'Superfoods', icon: '✨' }
];

// Insertar categorías desktop
if (document.getElementById("desktop-categorias")) {
    document.getElementById("desktop-categorias").innerHTML =
        categorias.map(c => `
    <a href="/productos?categoria=${c.id}" class="item_categoria block px-4 py-2.5 text-gray-700 hover:bg-yellow-50 hover:text-yellow-600">
        <span class="mr-2">${c.icon}</span> ${c.name}
    </a>
    `).join("");
}


// Insertar categorías mobile
if (document.getElementById("mobile-categorias")) {
    document.getElementById("mobile-categorias").innerHTML =
        categorias.map(c => `
    <a href="/productos?categoria=${c.id}" class="flex items-center gap-3 px-6 py-3 text-gray-600 hover:bg-yellow-50">
        <span class="text-lg ml-8">${c.icon}</span> <span class="text-sm">${c.name}</span>
    </a>
    `).join("");
}


/* --------------------------- Desktop dropdown --------------------------- */
const productsBtnDesktop = document.getElementById("products-desktop");
const productsDropdown = document.getElementById("products-dropdown");

let dropdownTimeout = null;

// Mostrar con transición
productsBtnDesktop.addEventListener("mouseenter", () => {
    clearTimeout(dropdownTimeout);
    productsDropdown.classList.remove("hidden");
    productsDropdown.classList.remove("opacity-0");
    productsDropdown.classList.add("opacity-100", "translate-y-0");
});

// Ocultar con retardo y verificación si el mouse está sobre categorías
productsBtnDesktop.addEventListener("mouseleave", () => {

    dropdownTimeout = setTimeout(() => {
        const isHoveringCategoria = document.querySelector(".item_categoria:hover");

        if (!isHoveringCategoria) {
            productsDropdown.classList.remove("opacity-100", "translate-y-0");
            productsDropdown.classList.add("opacity-0", "hidden");
        }

    }, 200);
});

/* Cuando el mouse sale de una categoría */
document.addEventListener("mouseover", (e) => {
    const isHoveringCategoria = document.querySelector(".item_categoria:hover");

    if (!e.target.closest("#products-desktop") && isHoveringCategoria) {
        productsDropdown.classList.add("opacity-0", "hidden");
    }
});

/* --------------------------- Mobile menu --------------------------- */
const btnMobile = document.getElementById("btn-mobile-menu");
const mobileMenu = document.getElementById("mobile-menu");
const mobileOverlay = document.getElementById("mobile-overlay");
const closeMobile = document.getElementById("close-mobile-menu");
const btnMobileProducts = document.getElementById("btn-mobile-products");
const mobileProductsSubmenu = document.getElementById("mobile-products-submenu");
const chevronMobile = btnMobileProducts.querySelector("i");

/* Abrir con transición */
btnMobile.addEventListener("click", () => {
    mobileMenu.classList.remove("hidden");
    mobileOverlay.classList.remove("hidden");

    // Animación slide-in
    requestAnimationFrame(() => {
        mobileMenu.classList.remove("-translate-x-full");
    });
});

/* Cerrar con transición */
const close = () => {
    mobileMenu.classList.add("-translate-x-full");
    mobileOverlay.classList.add("hidden");

    setTimeout(() => {
        mobileMenu.classList.add("hidden");
        mobileProductsSubmenu.classList.add("hidden");
        chevronMobile.classList.remove("rotate-180");
    }, 300);
};

closeMobile.addEventListener("click", close);
mobileOverlay.addEventListener("click", close);

/* Submenu productos */
btnMobileProducts.addEventListener("click", () => {
    mobileProductsSubmenu.classList.toggle("hidden");
    chevronMobile.classList.toggle("rotate-180");
});
