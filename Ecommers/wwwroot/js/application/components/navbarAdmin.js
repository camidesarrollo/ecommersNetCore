document.addEventListener('DOMContentLoaded', () => {

    // --- Menú lateral (mobile) ---
    const menuToggleBtn = document.getElementById('menu-toggle-btn');
    const menuIconOpen = document.getElementById('menu-icon-open');
    const menuIconClose = document.getElementById('menu-icon-close');

    menuToggleBtn.addEventListener('click', () => {
        menuIconOpen.classList.toggle('hidden');
        menuIconClose.classList.toggle('hidden');
        // Aquí puedes abrir/cerrar tu menú lateral si lo tienes
        // document.getElementById('sidebar').classList.toggle('hidden');
    });

    // --- Dropdown de notificaciones ---
    const notificationsBtn = document.getElementById('notifications-btn');
    const notificationsDropdown = document.getElementById('notifications-dropdown');

    notificationsBtn.addEventListener('click', (e) => {
        e.stopPropagation(); // Evita que se cierre al hacer click en el botón
        notificationsDropdown.classList.toggle('hidden');
        userMenuDropdown.classList.add('hidden'); // Cierra menú de usuario si está abierto
    });

    // --- Dropdown de usuario ---
    const userMenuBtn = document.getElementById('user-menu-btn');
    const userMenuDropdown = document.getElementById('user-menu-dropdown');

    userMenuBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        userMenuDropdown.classList.toggle('hidden');
        notificationsDropdown.classList.add('hidden'); // Cierra notificaciones si está abierto
    });

    // --- Cerrar todos los dropdowns al hacer click fuera ---
    document.addEventListener('click', () => {
        notificationsDropdown.classList.add('hidden');
        userMenuDropdown.classList.add('hidden');
    });

});
