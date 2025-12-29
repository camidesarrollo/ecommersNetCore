// ===================================
// FUNCIONALIDAD DEL SIDEBAR
// ===================================

document.addEventListener('DOMContentLoaded', function () {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebar-overlay');
    const closeSidebarBtn = document.getElementById('close-sidebar-btn');
    const menuToggleBtn = document.getElementById('menu-toggle-btn'); // Botón hamburguesa (debes agregarlo en tu layout)

    // Función para abrir el sidebar
    function openSidebar() {
        sidebar.classList.add('active');
        overlay.classList.remove('hidden');
        overlay.classList.add('active');
        document.body.style.overflow = 'hidden'; // Prevenir scroll en el body
    }

    // Función para cerrar el sidebar
    function closeSidebar() {
        sidebar.classList.remove('active');
        overlay.classList.remove('active');
        setTimeout(() => {
            overlay.classList.add('hidden');
        }, 300);
        document.body.style.overflow = ''; // Restaurar scroll
    }

    // Event listeners
    if (closeSidebarBtn) {
        closeSidebarBtn.addEventListener('click', closeSidebar);
    }

    if (overlay) {
        overlay.addEventListener('click', closeSidebar);
    }

    // Si tienes un botón hamburguesa para abrir el menú
    if (menuToggleBtn) {
        menuToggleBtn.addEventListener('click', openSidebar);
    }

    // Marcar el item activo según la URL actual
    const currentPath = window.location.pathname;
    const menuItems = document.querySelectorAll('.sidebar-menu-item');

    menuItems.forEach(item => {
        const itemPath = item.getAttribute('href');
        if (itemPath === currentPath) {
            item.classList.add('active');
        }

        // Remover clase active de otros items al hacer click
        item.addEventListener('click', function () {
            menuItems.forEach(i => i.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // Cerrar sidebar en desktop al cambiar tamaño de ventana
    window.addEventListener('resize', function () {
        if (window.innerWidth >= 1024) {
            closeSidebar();
        }
    });

    // Prevenir cierre accidental en mobile al hacer scroll
    let startY = 0;
    if (sidebar) {
        sidebar.addEventListener('touchstart', function (e) {
            startY = e.touches[0].clientY;
        });

        sidebar.addEventListener('touchmove', function (e) {
            e.stopPropagation();
        });
    }
});