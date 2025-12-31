// ==============================
// ELEMENTOS
// ==============================
const btnSearch = document.getElementById("btnSearch");
const searchInput = document.getElementById("searchInput");
const searchMobile = document.getElementById("searchMobile");

const btnUser = document.getElementById("btnUser");
const userMenu = document.getElementById("userMenu");

const btnCart = document.getElementById("btnCart");
const cartMenu = document.getElementById("cartMenu");
const cartModalMobile = document.getElementById("cartModalMobile");
const cartOverlay = document.getElementById("cartOverlay");
const btnCloseCartMobile = document.getElementById("closeCartMobile");

const cartCounter = document.getElementById("cartCounter");
const cartList = document.getElementById("cartList");
const cartListMobile = document.getElementById("cartListMobile");
const cartFooter = document.getElementById("cartFooter");
const cartFooterMobile = document.getElementById("cartFooterMobile");

// ==============================
// ESTADO
// ==============================
let searchOpen = false;
let userOpen = false;
let cartOpen = false;
let cart = []; // Array del carrito

// ==============================
// FUNCIONES AUXILIARES
// ==============================

// Abrir menú (dropdown)
function openMenu(menu) {
    menu.classList.remove("hidden", "opacity-0", "scale-95", "max-h-0");
    void menu.offsetWidth;
    menu.classList.add("opacity-100", "scale-100", "max-h-96");
}

// Cerrar menú (dropdown)
function closeMenu(menu, callback) {
    menu.classList.remove("opacity-100", "scale-100", "max-h-96");
    menu.classList.add("opacity-0", "scale-95", "max-h-0");
    setTimeout(() => {
        menu.classList.add("hidden");
        if (callback) callback();
    }, 200);
}

// ==============================
// CARRITO MÓVIL
// ==============================
const closeCartMobileMenu = () => {
    cartModalMobile.classList.add("translate-y-full");
    cartOverlay.classList.add("opacity-0");
    setTimeout(() => {
        cartModalMobile.classList.add("hidden");
        cartOverlay.classList.add("hidden");
        document.body.style.overflow = "";
    }, 300);
};

const openCartMobileMenu = () => {
    cartModalMobile.classList.remove("hidden");
    cartOverlay.classList.remove("hidden");
    document.body.style.overflow = "hidden";
    setTimeout(() => {
        cartModalMobile.classList.remove("translate-y-full");
        cartOverlay.classList.remove("opacity-0");
    }, 10);
};

// ==============================
// BÚSQUEDA
// ==============================
function openSearchInput(input) {
    input.classList.remove("h-0", "py-0", "opacity-0", "scale-95");
    void input.offsetWidth;
    input.classList.add("h-12", "py-2", "opacity-100", "scale-100");
    setTimeout(() => input.focus(), 200);
}

function closeSearchInput(input) {
    input.classList.remove("h-12", "py-2", "opacity-100", "scale-100");
    input.classList.add("h-0", "py-0", "opacity-0", "scale-95");
}

function toggleMobileSearch() {
    if (searchOpen) {
        searchMobile.classList.remove("hidden", "max-h-0", "opacity-0");
        void searchMobile.offsetWidth;
        searchMobile.classList.add("max-h-20", "opacity-100");
    } else {
        searchMobile.classList.remove("max-h-20", "opacity-100");
        searchMobile.classList.add("max-h-0", "opacity-0");
        setTimeout(() => searchMobile.classList.add("hidden"), 200);
    }
}

// ==============================
// EVENTOS
// ==============================

// Toggle búsqueda
btnSearch.addEventListener("click", () => {
    searchOpen = !searchOpen;

    if (userOpen) { closeMenu(userMenu); userOpen = false; }
    if (cartOpen) {
        if (window.innerWidth < 640) closeCartMobileMenu();
        else closeMenu(cartMenu);
        cartOpen = false;
    }

    if (window.innerWidth >= 768) {
        searchOpen ? openSearchInput(searchInput) : closeSearchInput(searchInput);
    } else {
        toggleMobileSearch();
    }
});

// Toggle usuario
btnUser.addEventListener("click", () => {
    userOpen = !userOpen;

    if (searchOpen) {
        closeSearchInput(searchInput);
        if (window.innerWidth < 768) {
            searchMobile.classList.add("hidden", "max-h-0", "opacity-0");
        }
        searchOpen = false;
    }
    if (cartOpen) {
        if (window.innerWidth < 640) closeCartMobileMenu();
        else closeMenu(cartMenu);
        cartOpen = false;
    }

    userOpen ? openMenu(userMenu) : closeMenu(userMenu);
});

// Toggle carrito
btnCart.addEventListener("click", () => {
    cartOpen = !cartOpen;

    if (userOpen) { closeMenu(userMenu); userOpen = false; }
    if (searchOpen) {
        closeSearchInput(searchInput);
        if (window.innerWidth < 768) searchMobile.classList.add("hidden", "max-h-0", "opacity-0");
        searchOpen = false;
    }

    if (cartOpen) {
        window.innerWidth < 640 ? openCartMobileMenu() : openMenu(cartMenu);
    } else {
        window.innerWidth < 640 ? closeCartMobileMenu() : closeMenu(cartMenu);
    }
});

// Cerrar carrito móvil
btnCloseCartMobile.addEventListener("click", () => {
    cartOpen = false;
    closeCartMobileMenu();
});
cartOverlay.addEventListener("click", () => {
    cartOpen = false;
    closeCartMobileMenu();
});

// Cerrar menús al hacer clic fuera (solo desktop)
document.addEventListener("click", (e) => {
    if (window.innerWidth >= 640) {
        const isClickInsideSearch = searchInput.contains(e.target) || searchMobile.contains(e.target) || btnSearch.contains(e.target);
        const isClickInsideUser = userMenu.contains(e.target) || btnUser.contains(e.target);
        const isClickInsideCart = cartMenu.contains(e.target) || btnCart.contains(e.target);

        if (!isClickInsideSearch && searchOpen) {
            closeSearchInput(searchInput);
            searchMobile.classList.remove("max-h-20", "opacity-100");
            searchMobile.classList.add("max-h-0", "opacity-0");
            setTimeout(() => searchMobile.classList.add("hidden"), 200);
            searchOpen = false;
        }
        if (!isClickInsideUser && userOpen) { closeMenu(userMenu); userOpen = false; }
        if (!isClickInsideCart && cartOpen) { closeMenu(cartMenu); cartOpen = false; }
    }
});

// ==============================
// CARRITO
// ==============================
function updateCart() {
    if (cart.length === 0) {
        cartList.innerHTML = `<p class="py-10 text-gray-600 text-center">Tu carrito está vacío</p>`;
        cartListMobile.innerHTML = `<p class="py-10 text-gray-600 text-center">Tu carrito está vacío</p>`;
        cartFooter.classList.add("hidden");
        cartFooterMobile.classList.add("hidden");
        cartCounter.classList.add("hidden");
        return;
    }

    cartCounter.classList.remove("hidden");
    cartCounter.textContent = cart.reduce((a, b) => a + b.quantity, 0);

    const total = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    document.getElementById("cartTotal").textContent = `$${total.toLocaleString()}`;
    document.getElementById("cartTotalMobile").textContent = `$${total.toLocaleString()}`;

    const cartHTML = cart.map(item => `
        <div class="flex justify-between border-b py-3 hover:bg-gray-50 rounded-lg px-2">
            <p class="font-medium">${item.name} <span class="text-gray-500">x${item.quantity}</span></p>
            <p class="font-semibold text-yellow-600">$${(item.quantity * item.price).toLocaleString()}</p>
        </div>
    `).join("");

    cartList.innerHTML = cartHTML;
    cartListMobile.innerHTML = cartHTML;
    cartFooter.classList.remove("hidden");
    cartFooterMobile.classList.remove("hidden");
}

// ==============================
// EJEMPLO DE PRODUCTOS
// ==============================
cart = [
    { name: "Almendras Premium", price: 5000, quantity: 2 },
    { name: "Nueces Naturales", price: 4000, quantity: 1 }
];

updateCart();
