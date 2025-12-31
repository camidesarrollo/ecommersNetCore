const plugin = require('tailwindcss/plugin');

module.exports = plugin(function ({ addBase, addComponents, addUtilities, addVariant, theme }) {

    // ==========================================
    // AGREGAR VARIANTE CHECKED
    // ==========================================
    addVariant('checked', '&:checked');

    // ==========================================
    // BASE STYLES (Variables CSS)
    // ==========================================
    addBase({
        ':root': {
            // Colores primarios
            '--color-olive-green': '#475E1D',
            '--color-olive-green-dark': '#31430E',
            '--color-nut-brown': '#8B5E3C',
            '--color-nut-brown-dark': '#6B4529',
            '--color-golden-yellow': '#F4C430',
            '--color-golden-yellow-dark': '#D4A017',
            '--color-burgundy-red': '#8B0000',
            '--color-burgundy-red-dark': '#650000',

            // Colores secundarios
            '--color-beige': '#F5F5DC',
            '--color-beige-dark': '#E6E6C7',
            '--color-mint-green': '#98FF98',
            '--color-mint-green-dark': '#7DE67D',
            '--color-dark-chocolate': '#3B2F2F',
            '--color-orange-warm': '#FF8C00',
            '--color-orange-warm-dark': '#CC7000',

            // Colores neutrales
            '--color-gray-light': '#E5E5E5',
            '--color-gray-light-dark': '#D0D0D0',
            '--color-gray-dark': '#4B4B4B',
            '--color-white': '#FFFFFF',

            // Colores de resalte
            '--color-lime-yellow': '#DFFF00',
            '--color-magenta-strong': '#FF00FF',

            // Escalas OKLCH - Olive Green
            '--color-olive-green-100': 'oklch(67.0% 0.100 120.0)',
            '--color-olive-green-200': 'oklch(62.0% 0.120 120.0)',
            '--color-olive-green-300': 'oklch(57.0% 0.140 120.0)',
            '--color-olive-green-400': 'oklch(53.0% 0.160 120.0)',
            '--color-olive-green-500': 'oklch(48.5% 0.173 120.0)',
            '--color-olive-green-600': 'oklch(41.5% 0.168 120.0)',
            '--color-olive-green-700': 'oklch(35.0% 0.160 120.0)',
            '--color-olive-green-800': 'oklch(28.0% 0.155 120.0)',
            '--color-olive-green-900': 'oklch(22.0% 0.150 120.0)',
            '--color-olive-green-1000': 'oklch(15.0% 0.145 120.0)',

            // Escalas OKLCH - Nut Brown
            '--color-nut-brown-100': 'oklch(59.0% 0.130 29.0)',
            '--color-nut-brown-200': 'oklch(54.0% 0.145 29.0)',
            '--color-nut-brown-300': 'oklch(49.0% 0.160 29.0)',
            '--color-nut-brown-400': 'oklch(44.0% 0.170 29.0)',
            '--color-nut-brown-500': 'oklch(39.5% 0.185 29.0)',
            '--color-nut-brown-600': 'oklch(33.0% 0.180 29.0)',
            '--color-nut-brown-700': 'oklch(28.0% 0.175 29.0)',
            '--color-nut-brown-800': 'oklch(23.0% 0.170 29.0)',
            '--color-nut-brown-900': 'oklch(18.0% 0.165 29.0)',
            '--color-nut-brown-1000': 'oklch(13.0% 0.160 29.0)',

            // Escalas OKLCH - Golden Yellow
            '--color-golden-yellow-100': 'oklch(96.0% 0.095 102.3)',
            '--color-golden-yellow-200': 'oklch(94.0% 0.105 102.3)',
            '--color-golden-yellow-300': 'oklch(92.0% 0.115 102.3)',
            '--color-golden-yellow-400': 'oklch(90.0% 0.125 102.3)',
            '--color-golden-yellow-500': 'oklch(87.2% 0.134 102.3)',
            '--color-golden-yellow-600': 'oklch(78.0% 0.147 102.3)',
            '--color-golden-yellow-700': 'oklch(68.0% 0.130 102.3)',
            '--color-golden-yellow-800': 'oklch(60.0% 0.120 102.3)',
            '--color-golden-yellow-900': 'oklch(50.0% 0.110 102.3)',
            '--color-golden-yellow-1000': 'oklch(40.0% 0.100 102.3)',

            // Escalas OKLCH - Burgundy Red
            '--color-burgundy-red-100': 'oklch(40.0% 0.180 29.0)',
            '--color-burgundy-red-200': 'oklch(36.0% 0.185 29.0)',
            '--color-burgundy-red-300': 'oklch(34.0% 0.190 29.0)',
            '--color-burgundy-red-400': 'oklch(30.0% 0.195 29.0)',
            '--color-burgundy-red-500': 'oklch(32.0% 0.197 29.0)',
            '--color-burgundy-red-600': 'oklch(25.0% 0.200 29.0)',
            '--color-burgundy-red-700': 'oklch(18.0% 0.190 29.0)',
            '--color-burgundy-red-800': 'oklch(12.0% 0.180 29.0)',
            '--color-burgundy-red-900': 'oklch(8.0% 0.170 29.0)',
            '--color-burgundy-red-1000': 'oklch(4.0% 0.160 29.0)',

            // Escalas OKLCH - Beige
            '--color-beige-100': 'oklch(97.0% 0.035 99.0)',
            '--color-beige-200': 'oklch(96.0% 0.037 99.0)',
            '--color-beige-300': 'oklch(95.0% 0.038 99.0)',
            '--color-beige-400': 'oklch(92.0% 0.039 99.0)',
            '--color-beige-500': 'oklch(94.0% 0.039 99.0)',
            '--color-beige-600': 'oklch(90.0% 0.045 99.0)',
            '--color-beige-700': 'oklch(85.0% 0.040 99.0)',
            '--color-beige-800': 'oklch(80.0% 0.038 99.0)',
            '--color-beige-900': 'oklch(75.0% 0.035 99.0)',
            '--color-beige-1000': 'oklch(70.0% 0.030 99.0)',

            // Escalas OKLCH - Mint Green
            '--color-mint-green-100': 'oklch(98.0% 0.100 136.0)',
            '--color-mint-green-200': 'oklch(96.0% 0.105 136.0)',
            '--color-mint-green-300': 'oklch(94.0% 0.110 136.0)',
            '--color-mint-green-400': 'oklch(90.0% 0.115 136.0)',
            '--color-mint-green-500': 'oklch(93.0% 0.120 136.0)',
            '--color-mint-green-600': 'oklch(87.0% 0.115 136.0)',
            '--color-mint-green-700': 'oklch(80.0% 0.110 136.0)',
            '--color-mint-green-800': 'oklch(73.0% 0.105 136.0)',
            '--color-mint-green-900': 'oklch(65.0% 0.100 136.0)',
            '--color-mint-green-1000': 'oklch(55.0% 0.095 136.0)',

            // Escalas OKLCH - Dark Chocolate
            '--color-dark-chocolate-100': 'oklch(25.0% 0.090 29.0)',
            '--color-dark-chocolate-200': 'oklch(22.0% 0.085 29.0)',
            '--color-dark-chocolate-300': 'oklch(20.0% 0.100 29.0)',
            '--color-dark-chocolate-400': 'oklch(18.0% 0.095 29.0)',
            '--color-dark-chocolate-500': 'oklch(20.0% 0.100 29.0)',
            '--color-dark-chocolate-600': 'oklch(15.0% 0.090 29.0)',
            '--color-dark-chocolate-700': 'oklch(10.0% 0.080 29.0)',
            '--color-dark-chocolate-800': 'oklch(7.0% 0.075 29.0)',
            '--color-dark-chocolate-900': 'oklch(4.0% 0.070 29.0)',
            '--color-dark-chocolate-1000': 'oklch(2.0% 0.065 29.0)',

            // Escalas OKLCH - Orange Warm
            '--color-orange-warm-100': 'oklch(78.0% 0.210 74.0)',
            '--color-orange-warm-200': 'oklch(75.0% 0.205 74.0)',
            '--color-orange-warm-300': 'oklch(72.0% 0.200 74.0)',
            '--color-orange-warm-400': 'oklch(70.0% 0.225 74.0)',
            '--color-orange-warm-500': 'oklch(70.0% 0.225 74.0)',
            '--color-orange-warm-600': 'oklch(61.0% 0.210 74.0)',
            '--color-orange-warm-700': 'oklch(52.0% 0.195 74.0)',
            '--color-orange-warm-800': 'oklch(45.0% 0.180 74.0)',
            '--color-orange-warm-900': 'oklch(38.0% 0.165 74.0)',
            '--color-orange-warm-1000': 'oklch(32.0% 0.150 74.0)',

            // Escalas OKLCH - Lime Yellow
            '--color-lime-yellow-100': 'oklch(99.0% 0.080 100.0)',
            '--color-lime-yellow-200': 'oklch(97.0% 0.100 100.0)',
            '--color-lime-yellow-300': 'oklch(95.0% 0.120 100.0)',
            '--color-lime-yellow-400': 'oklch(93.0% 0.140 100.0)',
            '--color-lime-yellow-500': 'oklch(95.0% 0.180 100.0)',
            '--color-lime-yellow-600': 'oklch(85.0% 0.160 100.0)',
            '--color-lime-yellow-700': 'oklch(75.0% 0.140 100.0)',
            '--color-lime-yellow-800': 'oklch(65.0% 0.120 100.0)',
            '--color-lime-yellow-900': 'oklch(55.0% 0.100 100.0)',
            '--color-lime-yellow-1000': 'oklch(45.0% 0.080 100.0)',

            // Escalas OKLCH - Magenta Strong
            '--color-magenta-strong-100': 'oklch(95.0% 0.270 330.0)',
            '--color-magenta-strong-200': 'oklch(65.0% 0.280 330.0)',
            '--color-magenta-strong-300': 'oklch(63.0% 0.290 330.0)',
            '--color-magenta-strong-400': 'oklch(61.0% 0.310 330.0)',
            '--color-magenta-strong-500': 'oklch(60.0% 0.350 330.0)',
            '--color-magenta-strong-600': 'oklch(55.0% 0.330 330.0)',
            '--color-magenta-strong-700': 'oklch(50.0% 0.310 330.0)',
            '--color-magenta-strong-800': 'oklch(45.0% 0.290 330.0)',
            '--color-magenta-strong-900': 'oklch(40.0% 0.270 330.0)',
            '--color-magenta-strong-1000': 'oklch(35.0% 0.250 330.0)',

            // Ring variables
            '--ring-color': 'transparent',
            '--ring-offset-width': '0px',
            '--ring-offset-color': 'transparent',
            '--ring-width': '3px',
        },

        // Estilos base para imágenes
        'img': {
            imageRendering: 'pixelated',
        },

        // Estilos para botones
        'button:focus': {
            outline: 'none',
            boxShadow: '0 0 0 3px rgba(107, 142, 35, 0.3)',
        },
        'button:disabled': {
            opacity: '0.6',
            cursor: 'not-allowed',
            transform: 'none !important',
        },
        'button:disabled:hover': {
            transform: 'none !important',
            boxShadow: 'none !important',
        },

        // Scrollbar personalizado
        '::-webkit-scrollbar': {
            width: '8px',
        },
        '::-webkit-scrollbar-track': {
            background: '#f1f1f1',
        },
        '::-webkit-scrollbar-thumb': {
            background: '#8b7765',
            borderRadius: '4px',
        },
        '::-webkit-scrollbar-thumb:hover': {
            background: '#6b5d4f',
        },

        // Scrollbar para elementos con overflow
        '.overflow-y-auto::-webkit-scrollbar': {
            width: '6px',
        },
        '.overflow-y-auto::-webkit-scrollbar-track': {
            background: 'var(--color-gray-light)',
        },
        '.overflow-y-auto::-webkit-scrollbar-thumb': {
            background: 'var(--color-olive-green)',
            borderRadius: '3px',
        },
        '.overflow-y-auto::-webkit-scrollbar-thumb:hover': {
            background: 'var(--color-olive-green-dark)',
        },
    });

    // ==========================================
    // COMPONENTES (Botones y estilos reutilizables)
    // ==========================================
    addComponents({
        // Botones
        '.btn-add': {
            backgroundColor: 'var(--color-olive-green)',
            color: 'var(--color-white)',
            padding: '0.5rem 1rem',
            borderRadius: '0.5rem',
            fontWeight: '600',
            transition: 'background 0.3s',
            '&:hover': {
                backgroundColor: '#556B2F',
            },
        },
        '.btn-delete': {
            backgroundColor: 'var(--color-burgundy-red)',
            color: 'var(--color-white)',
            padding: '0.5rem 1rem',
            borderRadius: '0.5rem',
            fontWeight: '600',
            transition: 'background 0.3s',
            '&:hover': {
                backgroundColor: '#5C0000',
            },
        },
        '.btn-edit': {
            backgroundColor: 'var(--color-golden-yellow)',
            color: 'var(--color-dark-chocolate)',
            padding: '0.5rem 1rem',
            borderRadius: '0.5rem',
            fontWeight: '600',
            transition: 'background 0.3s',
            '&:hover': {
                backgroundColor: '#D4A017',
            },
        },
        '.btn-primary': {
            background: 'linear-gradient(135deg, var(--color-olive-green) 0%, var(--color-olive-green-dark) 100%)',
            transition: 'all 0.3s ease',
            '&:hover:not(:disabled)': {
                background: 'linear-gradient(135deg, var(--color-olive-green-dark) 0%, var(--color-nut-brown-dark) 100%)',
                transform: 'translateY(-2px)',
                boxShadow: '0 8px 25px rgba(107, 142, 35, 0.3)',
            },
        },

        // Inputs
        '.input-focus': {
            transition: 'all 0.3s ease',
            border: '2px solid var(--color-gray-light)',
            '&:focus': {
                borderColor: 'var(--color-olive-green)',
                boxShadow: '0 0 0 3px rgba(107, 142, 35, 0.1)',
                outline: 'none',
            },
        },

        // Efectos especiales
        '.glass-effect': {
            background: 'rgba(255, 255, 255, 0.95)',
            backdropFilter: 'blur(10px)',
            border: '1px solid rgba(255, 255, 255, 0.3)',
        },
        '.parallax-effect': {
            backgroundAttachment: 'fixed',
            backgroundPosition: 'center',
            backgroundRepeat: 'no-repeat',
            backgroundSize: 'cover',
        },
        '.text-shadow': {
            textShadow: '2px 2px 4px rgba(0, 0, 0, 0.5)',
        },
        '.drop-shadow-lg': {
            textShadow: '0 4px 8px rgba(0, 0, 0, 0.4)',
        },
        '.overlay-gradient': {
            background: 'linear-gradient(135deg, rgba(139, 69, 19, 0.8) 0%, rgba(210, 105, 30, 0.7) 100%)',
        },

        // Errores de validación
        '.field-validation-error': {
            color: 'red',
        },

        // Fondos con patrón
        '.bg-pattern': {
            backgroundColor: '#f8f5f0',
            backgroundImage: 'radial-gradient(circle at 25px 25px, rgba(139, 119, 101, 0.05) 2%, transparent 0%), radial-gradient(circle at 75px 75px, rgba(139, 119, 101, 0.05) 2%, transparent 0%)',
            backgroundSize: '100px 100px',
        },

        // Botón de ayuda flotante
        '.help-button-float': {
            position: 'fixed',
            bottom: '30px',
            right: '30px',
            width: '60px',
            height: '60px',
            borderRadius: '50%',
            background: 'linear-gradient(135deg, var(--color-olive-green), var(--color-olive-green-dark))',
            color: 'white',
            border: 'none',
            boxShadow: '0 8px 20px rgba(71, 94, 29, 0.3)',
            cursor: 'pointer',
            zIndex: '999',
            transition: 'all 0.3s ease',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            fontSize: '24px',
            animation: 'pulse-help 2s infinite',
            '&:hover': {
                transform: 'translateY(-5px) scale(1.1)',
                boxShadow: '0 12px 30px rgba(71, 94, 29, 0.4)',
                animation: 'none',
            },
            '&:active': {
                transform: 'translateY(-2px) scale(1.05)',
            },
            '&::before': {
                content: "'¿Necesitas ayuda?'",
                position: 'absolute',
                right: '70px',
                background: 'var(--color-dark-chocolate)',
                color: 'white',
                padding: '8px 12px',
                borderRadius: '8px',
                fontSize: '14px',
                whiteSpace: 'nowrap',
                opacity: '0',
                visibility: 'hidden',
                transition: 'all 0.3s ease',
                fontWeight: '500',
            },
            '&:hover::before': {
                opacity: '1',
                visibility: 'visible',
                right: '75px',
            },
        },

        // Botón de ayuda en header
        '.help-button-header': {
            display: 'inline-flex',
            alignItems: 'center',
            gap: '8px',
            padding: '10px 20px',
            background: 'linear-gradient(135deg, var(--color-mint-green-dark), var(--color-olive-green))',
            color: 'white',
            border: 'none',
            borderRadius: '10px',
            fontWeight: '600',
            cursor: 'pointer',
            transition: 'all 0.3s ease',
            boxShadow: '0 4px 12px rgba(71, 94, 29, 0.2)',
            '&:hover': {
                transform: 'translateY(-2px)',
                boxShadow: '0 6px 16px rgba(71, 94, 29, 0.3)',
            },
            '& i': {
                fontSize: '18px',
            },
        },

        // Swiper customization
        '.swiper': {
            zIndex: '0',
        },
        '.swiper-button-next-custom, .swiper-button-prev-custom': {
            color: '#1D4ED8',
            backgroundColor: '#E0F2FE',
            padding: '0.5rem 1rem',
            borderRadius: '0.5rem',
            top: '50%',
            transform: 'translateY(-50%)',
        },
        '.swiper-button-prev-custom': {
            left: '-10px',
        },
        '.swiper-button-next-custom': {
            right: '-10px',
        },

        // Transiciones para toast
        '.toast-enter-active, .toast-leave-active': {
            transition: 'all 0.3s ease',
        },
        '.toast-enter-from': {
            opacity: '0',
            transform: 'translateX(100%)',
        },
        '.toast-leave-to': {
            opacity: '0',
            transform: 'translateX(100%)',
        },

        // Transiciones fade
        '.fade-enter-active, .fade-leave-active': {
            transition: 'all 0.3s ease',
        },
        '.fade-enter-from': {
            opacity: '0',
            transform: 'translateY(-10px)',
        },
        '.fade-leave-to': {
            opacity: '0',
            transform: 'translateY(-10px)',
        },
    });

    // ==========================================
    // UTILIDADES (Clases de fondo, texto, etc.)
    // ==========================================
    addUtilities({
        // Colores de fondo - Principales
        '.bg-olive-green': { backgroundColor: 'var(--color-olive-green)' },
        '.bg-olive-green-dark': { backgroundColor: 'var(--color-olive-green-dark)' },
        '.bg-nut-brown': { backgroundColor: 'var(--color-nut-brown)' },
        '.bg-nut-brown-dark': { backgroundColor: 'var(--color-nut-brown-dark)' },
        '.bg-golden-yellow': { backgroundColor: 'var(--color-golden-yellow)' },
        '.bg-golden-yellow-dark': { backgroundColor: 'var(--color-golden-yellow-dark)' },
        '.bg-burgundy-red': { backgroundColor: 'var(--color-burgundy-red)' },
        '.bg-burgundy-red-dark': { backgroundColor: 'var(--color-burgundy-red-dark)' },
        '.bg-beige': { backgroundColor: 'var(--color-beige)' },
        '.bg-beige-dark': { backgroundColor: 'var(--color-beige-dark)' },
        '.bg-mint-green': { backgroundColor: 'var(--color-mint-green)' },
        '.bg-mint-green-dark': { backgroundColor: 'var(--color-mint-green-dark)' },
        '.bg-dark-chocolate': { backgroundColor: 'var(--color-dark-chocolate)' },
        '.bg-orange-warm': { backgroundColor: 'var(--color-orange-warm)' },
        '.bg-orange-warm-dark': { backgroundColor: 'var(--color-orange-warm-dark)' },
        '.bg-gray-light': { backgroundColor: 'var(--color-gray-light)' },
        '.bg-gray-light-dark': { backgroundColor: 'var(--color-gray-light-dark)' },

        // Colores hover de fondo
        '.hover\\:bg-olive-green-dark:hover': { backgroundColor: 'var(--color-olive-green-dark)' },
        '.hover\\:bg-nut-brown-dark:hover': { backgroundColor: 'var(--color-nut-brown-dark)' },
        '.hover\\:bg-golden-yellow-dark:hover': { backgroundColor: 'var(--color-golden-yellow-dark)' },
        '.hover\\:bg-burgundy-red-dark:hover': { backgroundColor: 'var(--color-burgundy-red-dark)' },
        '.hover\\:bg-beige-dark:hover': { backgroundColor: 'var(--color-beige-dark)' },
        '.hover\\:bg-mint-green-dark:hover': { backgroundColor: 'var(--color-mint-green-dark)' },
        '.hover\\:bg-orange-warm-dark:hover': { backgroundColor: 'var(--color-orange-warm-dark)' },
        '.hover\\:bg-gray-light-dark:hover': { backgroundColor: 'var(--color-gray-light-dark)' },

        // Fondos personalizados adicionales
        '.bg-green-olive': { backgroundColor: '#6B8E23' },
        '.bg-red-burgundy': { backgroundColor: '#8B0000' },

        // Hovers adicionales
        '.hover\\:bg-green-700:hover': { backgroundColor: '#556B2F' },
        '.hover\\:bg-red-800:hover': { backgroundColor: '#5C0000' },
        '.hover\\:bg-d4a017:hover': { backgroundColor: '#D4A017' },
        '.hover\\:bg-green-400:hover': { backgroundColor: '#32CD32' },
        '.hover\\:bg-orange-600:hover': { backgroundColor: '#FF7F00' },
        '.hover\\:bg-gray-300:hover': { backgroundColor: '#D1D1D1' },

        // Colores de texto
        '.text-dark-chocolate': { color: 'var(--color-dark-chocolate)' },
        '.text-gray-dark': { color: 'var(--color-gray-dark)' },
        '.text-primary': { color: 'var(--color-nut-brown)' },
        '.text-secondary': { color: 'var(--color-gray-dark)' },
        '.text-accent': { color: 'var(--color-golden-yellow)' },
        '.text-success': { color: 'var(--color-olive-green)' },
        '.text-warning': { color: 'var(--color-orange-warm)' },
        '.text-error': { color: 'var(--color-burgundy-red)' },

        // Colores de borde
        '.border-nut-brown': { borderColor: 'var(--color-nut-brown)' },
        '.border-primary': { borderColor: 'var(--color-nut-brown)' },
        '.border-secondary': { borderColor: 'var(--color-gray-light)' },
        '.border-accent': { borderColor: 'var(--color-golden-yellow)' },

        // Bordes OKLCH Olive Green
        ...Object.fromEntries(
            [100, 200, 300, 400, 500, 600, 700, 800, 900, 1000].map(scale => [
                `.border-olive-green-${scale}`,
                { borderColor: `var(--color-olive-green-${scale})` }
            ])
        ),

        // Ring colors
        ...Object.fromEntries(
            [100, 200, 300, 400, 500, 600, 700, 800, 900, 1000].map(scale => [
                `.ring-olive-green-${scale}`,
                { '--ring-color': `var(--color-olive-green-${scale})` }
            ])
        ),
        '.ring': {
            boxShadow: '0 0 0 var(--ring-offset-width) var(--ring-offset-color), 0 0 0 calc(var(--ring-offset-width) + var(--ring-width)) var(--ring-color)',
        },
        '.ring-1': { '--ring-width': '1px' },
        '.ring-2': { '--ring-width': '2px' },
        '.ring-4': { '--ring-width': '4px' },
        '.ring-offset-1': { '--ring-offset-width': '1px' },
        '.ring-offset-2': { '--ring-offset-width': '2px' },
        '.ring-offset-white': { '--ring-offset-color': 'white' },
        '.ring-offset-gray': { '--ring-offset-color': '#e5e7eb' },

        // Sombras personalizadas
        '.shadow-olive-green\\/30': { boxShadow: '0 10px 25px rgba(107, 142, 35, 0.3)' },
        '.hover\\:shadow-olive-green\\/30:hover': { boxShadow: '0 15px 35px rgba(107, 142, 35, 0.3)' },
        '.shadow-burgundy-red\\/30': { boxShadow: '0 10px 25px rgba(139, 0, 0, 0.3)' },
        '.hover\\:shadow-burgundy-red\\/30:hover': { boxShadow: '0 15px 35px rgba(139, 0, 0, 0.3)' },
        '.shadow-golden-yellow\\/30': { boxShadow: '0 10px 25px rgba(244, 196, 48, 0.3)' },
        '.hover\\:shadow-golden-yellow\\/30:hover': { boxShadow: '0 15px 35px rgba(244, 196, 48, 0.3)' },
        '.shadow-mint-green\\/30': { boxShadow: '0 10px 25px rgba(152, 255, 152, 0.3)' },
        '.hover\\:shadow-mint-green\\/30:hover': { boxShadow: '0 15px 35px rgba(152, 255, 152, 0.3)' },
        '.shadow-orange-warm\\/30': { boxShadow: '0 10px 25px rgba(255, 140, 0, 0.3)' },
        '.hover\\:shadow-orange-warm\\/30:hover': { boxShadow: '0 15px 35px rgba(255, 140, 0, 0.3)' },
        '.shadow-nut-brown\\/30': { boxShadow: '0 10px 25px rgba(139, 94, 60, 0.3)' },
        '.hover\\:shadow-nut-brown\\/30:hover': { boxShadow: '0 15px 35px rgba(139, 94, 60, 0.3)' },
        '.shadow-beige\\/30': { boxShadow: '0 10px 25px rgba(245, 245, 220, 0.3)' },
        '.hover\\:shadow-beige\\/30:hover': { boxShadow: '0 15px 35px rgba(245, 245, 220, 0.3)' },
        '.shadow-gray-light\\/30': { boxShadow: '0 10px 25px rgba(229, 229, 229, 0.3)' },
        '.hover\\:shadow-gray-light\\/30:hover': { boxShadow: '0 15px 35px rgba(229, 229, 229, 0.3)' },

        // Gradientes personalizados
        '.bg-gradient-nuts': {
            background: 'linear-gradient(135deg, var(--color-nut-brown) 0%, var(--color-golden-yellow) 100%)',
        },
        '.bg-gradient-natural': {
            background: 'linear-gradient(135deg, var(--color-olive-green) 0%, var(--color-mint-green) 100%)',
        },
        '.to-mint-green': {
            '--tw-gradient-to': '#e8f5e9',
        },
        '.from-beige': {
            '--tw-gradient-from': '#f5f5dc',
        },

        // Gradientes específicos con clases combinadas
        '.bg-gradient-to-br.from-olive-green.to-mint-green': {
            background: 'linear-gradient(to bottom right, var(--color-olive-green), var(--color-mint-green))',
        },
        '.bg-gradient-to-br.from-dark-chocolate.to-nut-brown': {
            background: 'linear-gradient(to bottom right, var(--color-dark-chocolate), var(--color-nut-brown))',
        },
        '.bg-gradient-to-br.from-golden-yellow.to-beige': {
            background: 'linear-gradient(to bottom right, var(--color-golden-yellow), var(--color-beige))',
        },
        '.bg-gradient-to-br.from-nut-brown.to-dark-chocolate': {
            background: 'linear-gradient(to bottom right, var(--color-nut-brown), var(--color-dark-chocolate))',
        },
        '.bg-gradient-to-br.from-burgundy-red.to-orange-warm': {
            background: 'linear-gradient(to bottom right, var(--color-burgundy-red), var(--color-orange-warm))',
        },
        '.bg-gradient-to-br.from-nut-brown.to-golden-yellow': {
            background: 'linear-gradient(to bottom right, var(--color-nut-brown), var(--color-golden-yellow))',
        },
        '.bg-gradient-to-br.from-beige.to-golden-yellow': {
            background: 'linear-gradient(to bottom right, var(--color-beige), var(--color-golden-yellow))',
        },
        '.bg-gradient-to-br.from-orange-warm.to-burgundy-red': {
            background: 'linear-gradient(to bottom right, var(--color-orange-warm), var(--color-burgundy-red))',
        },
        '.bg-gradient-to-br.from-golden-yellow.to-nut-brown': {
            background: 'linear-gradient(to bottom right, var(--color-golden-yellow), var(--color-nut-brown))',
        },
        '.bg-gradient-to-br.from-burgundy-red.to-olive-green': {
            background: 'linear-gradient(to bottom right, var(--color-burgundy-red), var(--color-olive-green))',
        },
        '.bg-gradient-to-br.from-mint-green.to-olive-green': {
            background: 'linear-gradient(to bottom right, var(--color-mint-green), var(--color-olive-green))',
        },
        '.bg-gradient-to-br.from-mint-green.to-beige': {
            background: 'linear-gradient(to bottom right, var(--color-mint-green), var(--color-beige))',
        },
        '.bg-gradient-to-br.from-burgundy-red.to-golden-yellow': {
            background: 'linear-gradient(to bottom right, var(--color-burgundy-red), var(--color-golden-yellow))',
        },
        '.bg-gradient-to-br.from-beige.to-mint-green': {
            background: 'linear-gradient(to bottom right, var(--color-beige), var(--color-mint-green))',
        },
        '.bg-gradient-to-br.from-golden-yellow.to-orange-warm': {
            background: 'linear-gradient(to bottom right, var(--color-golden-yellow), var(--color-orange-warm))',
        },
        '.bg-gradient-to-br.from-olive-green.to-golden-yellow': {
            background: 'linear-gradient(to bottom right, var(--color-olive-green), var(--color-golden-yellow))',
        },
        '.bg-gradient-to-br.from-gray-light.to-gray-dark': {
            background: 'linear-gradient(to bottom right, var(--color-gray-light), var(--color-gray-dark))',
        },
        '.bg-gradient-to-br.from-lime-yellow.to-magenta-strong': {
            background: 'linear-gradient(to bottom right, var(--color-lime-yellow), var(--color-magenta-strong))',
        },
        '.bg-gradient-to-br.from-lime-yellow.to-olive-green': {
            background: 'linear-gradient(to bottom right, var(--color-lime-yellow), var(--color-olive-green))',
        },
        '.bg-gradient-to-br.from-lime-yellow.to-nut-brown': {
            background: 'linear-gradient(to bottom right, var(--color-lime-yellow), var(--color-nut-brown))',
        },
        '.bg-gradient-to-br.from-magenta-strong.to-golden-yellow': {
            background: 'linear-gradient(to bottom right, var(--color-magenta-strong), var(--color-golden-yellow))',
        },
        '.bg-gradient-to-br.from-magenta-strong.to-burgundy-red': {
            background: 'linear-gradient(to bottom right, var(--color-magenta-strong), var(--color-burgundy-red))',
        },
        '.bg-gradient-to-br.from-magenta-strong.to-mint-green': {
            background: 'linear-gradient(to bottom right, var(--color-magenta-strong), var(--color-mint-green))',
        },
        '.bg-gradient-to-br.from-lime-yellow.to-beige': {
            background: 'linear-gradient(to bottom right, var(--color-lime-yellow), var(--color-beige))',
        },
        '.bg-gradient-to-br.from-magenta-strong.to-beige': {
            background: 'linear-gradient(to bottom right, var(--color-magenta-strong), var(--color-beige))',
        },

        // Fondos semánticos
        '.bg-primary': { backgroundColor: 'var(--color-beige)' },
        '.bg-secondary': { backgroundColor: 'var(--color-gray-light)' },
        '.bg-accent': { backgroundColor: 'var(--color-golden-yellow)' },
        '.bg-success': { backgroundColor: 'var(--color-olive-green)' },
        '.bg-warning': { backgroundColor: 'var(--color-orange-warm)' },
        '.bg-error': { backgroundColor: 'var(--color-burgundy-red)' },

        // Animaciones
        '.floating': {
            animation: 'floating 6s ease-in-out infinite',
        },
        '.floating-animation': {
            animation: 'float 6s ease-in-out infinite',
        },
        '.pulse-glow': {
            animation: 'pulseGlow 2s infinite',
        },
        '.pulse-warm': {
            animation: 'pulseWarm 2s infinite',
        },
        '.fade-in': {
            animation: 'fadeIn 1.5s ease-out',
        },
        '.slide-in': {
            animation: 'slideIn 0.8s ease-out forwards',
        },
        '.slide-in-left': {
            animation: 'slideInLeft 1s ease-out 0.3s both',
        },
        '.slide-in-right': {
            animation: 'slideInRight 1s ease-out 0.6s both',
        },
        '.animate-pulse-slow': {
            animation: 'pulse 3s infinite',
        },
        '.delay-200': { animationDelay: '0.2s' },
        '.delay-400': { animationDelay: '0.4s' },
        '.animate-delay-1000': { animationDelay: '1s' },
        '.animate-delay-2000': { animationDelay: '2s' },

        /* ===============================
             SWITCH / TOGGLE INPUT
          =============================== */
        '.appearance-none': { appearance: 'none' },
        '.before\\:content-\\[\'\'\\]::before': {
            content: "''",
        },
        '.before\\:absolute::before': {
            position: 'absolute',
        },
        '.before\\:top-1::before': {
            top: '0.25rem',
        },
        '.before\\:left-1::before': {
            left: '0.25rem',
        },
        '.before\\:w-4::before': {
            width: '1rem',
        },
        '.before\\:h-4::before': {
            height: '1rem',
        },
        '.before\\:bg-white::before': {
            backgroundColor: '#ffffff',
        },
        '.before\\:rounded-full::before': {
            borderRadius: '9999px',
        },
        '.before\\:transition-transform::before': {
            transitionProperty: 'transform',
        },
        '.before\\:duration-300::before': {
            transitionDuration: '300ms',
        },
        '.checked\\:before\\:translate-x-6:checked::before': {
            transform: 'translateX(1.5rem)',
        },
    });

    // ==========================================
    // ANIMACIONES (Keyframes)
    // ==========================================
    addBase({
        '@keyframes floating': {
            '0%, 100%': { transform: 'translateY(0px) rotate(0deg)' },
            '33%': { transform: 'translateY(-15px) rotate(1deg)' },
            '66%': { transform: 'translateY(-5px) rotate(-1deg)' },
        },
        '@keyframes float': {
            '0%, 100%': { transform: 'translateY(0px)' },
            '50%': { transform: 'translateY(-20px)' },
        },
        '@keyframes bounce': {
            '0%, 100%': { transform: 'translateY(0)' },
            '50%': { transform: 'translateY(-10px)' },
        },
        '@keyframes pulseGlow': {
            '0%, 100%': { boxShadow: '0 0 20px rgba(255, 255, 255, 0.3)' },
            '50%': { boxShadow: '0 0 40px rgba(255, 255, 255, 0.6)' },
        },
        '@keyframes pulseWarm': {
            '0%, 100%': {
                boxShadow: '0 0 20px rgba(244, 196, 48, 0.3)',
                transform: 'scale(1)'
            },
            '50%': {
                boxShadow: '0 0 30px rgba(244, 196, 48, 0.5)',
                transform: 'scale(1.02)'
            },
        },
        '@keyframes pulse-help': {
            '0%, 100%': {
                boxShadow: '0 8px 20px rgba(71, 94, 29, 0.3)',
            },
            '50%': {
                boxShadow: '0 8px 30px rgba(71, 94, 29, 0.5)',
            },
        },
        '@keyframes fadeIn': {
            from: { opacity: '0', transform: 'translateY(30px)' },
            to: { opacity: '1', transform: 'translateY(0)' },
        },
        '@keyframes slideIn': {
            from: { opacity: '0', transform: 'translateY(40px) scale(0.95)' },
            to: { opacity: '1', transform: 'translateY(0) scale(1)' },
        },
        '@keyframes slideInLeft': {
            from: { opacity: '0', transform: 'translateX(-50px)' },
            to: { opacity: '1', transform: 'translateX(0)' },
        },
        '@keyframes slideInRight': {
            from: { opacity: '0', transform: 'translateX(50px)' },
            to: { opacity: '1', transform: 'translateX(0)' },
        },
    });
}, {
    theme: {
        extend: {
            colors: {
                'olive-green': {
                    DEFAULT: '#475E1D',
                    dark: '#31430E',
                    100: 'var(--color-olive-green-100)',
                    200: 'var(--color-olive-green-200)',
                    300: 'var(--color-olive-green-300)',
                    400: 'var(--color-olive-green-400)',
                    500: 'var(--color-olive-green-500)',
                    600: 'var(--color-olive-green-600)',
                    700: 'var(--color-olive-green-700)',
                    800: 'var(--color-olive-green-800)',
                    900: 'var(--color-olive-green-900)',
                    1000: 'var(--color-olive-green-1000)',
                },
                'nut-brown': {
                    DEFAULT: '#8B5E3C',
                    dark: '#6B4529',
                    100: 'var(--color-nut-brown-100)',
                    200: 'var(--color-nut-brown-200)',
                    300: 'var(--color-nut-brown-300)',
                    400: 'var(--color-nut-brown-400)',
                    500: 'var(--color-nut-brown-500)',
                    600: 'var(--color-nut-brown-600)',
                    700: 'var(--color-nut-brown-700)',
                    800: 'var(--color-nut-brown-800)',
                    900: 'var(--color-nut-brown-900)',
                    1000: 'var(--color-nut-brown-1000)',
                },
                'golden-yellow': {
                    DEFAULT: '#F4C430',
                    dark: '#D4A017',
                    100: 'var(--color-golden-yellow-100)',
                    200: 'var(--color-golden-yellow-200)',
                    300: 'var(--color-golden-yellow-300)',
                    400: 'var(--color-golden-yellow-400)',
                    500: 'var(--color-golden-yellow-500)',
                    600: 'var(--color-golden-yellow-600)',
                    700: 'var(--color-golden-yellow-700)',
                    800: 'var(--color-golden-yellow-800)',
                    900: 'var(--color-golden-yellow-900)',
                    1000: 'var(--color-golden-yellow-1000)',
                },
                'burgundy-red': {
                    DEFAULT: '#8B0000',
                    dark: '#650000',
                    100: 'var(--color-burgundy-red-100)',
                    200: 'var(--color-burgundy-red-200)',
                    300: 'var(--color-burgundy-red-300)',
                    400: 'var(--color-burgundy-red-400)',
                    500: 'var(--color-burgundy-red-500)',
                    600: 'var(--color-burgundy-red-600)',
                    700: 'var(--color-burgundy-red-700)',
                    800: 'var(--color-burgundy-red-800)',
                    900: 'var(--color-burgundy-red-900)',
                    1000: 'var(--color-burgundy-red-1000)',
                },
                'beige': {
                    DEFAULT: '#F5F5DC',
                    dark: '#E6E6C7',
                    100: 'var(--color-beige-100)',
                    200: 'var(--color-beige-200)',
                    300: 'var(--color-beige-300)',
                    400: 'var(--color-beige-400)',
                    500: 'var(--color-beige-500)',
                    600: 'var(--color-beige-600)',
                    700: 'var(--color-beige-700)',
                    800: 'var(--color-beige-800)',
                    900: 'var(--color-beige-900)',
                    1000: 'var(--color-beige-1000)',
                },
                'mint-green': {
                    DEFAULT: '#98FF98',
                    dark: '#7DE67D',
                    100: 'var(--color-mint-green-100)',
                    200: 'var(--color-mint-green-200)',
                    300: 'var(--color-mint-green-300)',
                    400: 'var(--color-mint-green-400)',
                    500: 'var(--color-mint-green-500)',
                    600: 'var(--color-mint-green-600)',
                    700: 'var(--color-mint-green-700)',
                    800: 'var(--color-mint-green-800)',
                    900: 'var(--color-mint-green-900)',
                    1000: 'var(--color-mint-green-1000)',
                },
                'dark-chocolate': {
                    DEFAULT: '#3B2F2F',
                    100: 'var(--color-dark-chocolate-100)',
                    200: 'var(--color-dark-chocolate-200)',
                    300: 'var(--color-dark-chocolate-300)',
                    400: 'var(--color-dark-chocolate-400)',
                    500: 'var(--color-dark-chocolate-500)',
                    600: 'var(--color-dark-chocolate-600)',
                    700: 'var(--color-dark-chocolate-700)',
                    800: 'var(--color-dark-chocolate-800)',
                    900: 'var(--color-dark-chocolate-900)',
                    1000: 'var(--color-dark-chocolate-1000)',
                },
                'orange-warm': {
                    DEFAULT: '#FF8C00',
                    dark: '#CC7000',
                    100: 'var(--color-orange-warm-100)',
                    200: 'var(--color-orange-warm-200)',
                    300: 'var(--color-orange-warm-300)',
                    400: 'var(--color-orange-warm-400)',
                    500: 'var(--color-orange-warm-500)',
                    600: 'var(--color-orange-warm-600)',
                    700: 'var(--color-orange-warm-700)',
                    800: 'var(--color-orange-warm-800)',
                    900: 'var(--color-orange-warm-900)',
                    1000: 'var(--color-orange-warm-1000)',
                },
                'lime-yellow': {
                    DEFAULT: '#DFFF00',
                    100: 'var(--color-lime-yellow-100)',
                    200: 'var(--color-lime-yellow-200)',
                    300: 'var(--color-lime-yellow-300)',
                    400: 'var(--color-lime-yellow-400)',
                    500: 'var(--color-lime-yellow-500)',
                    600: 'var(--color-lime-yellow-600)',
                    700: 'var(--color-lime-yellow-700)',
                    800: 'var(--color-lime-yellow-800)',
                    900: 'var(--color-lime-yellow-900)',
                    1000: 'var(--color-lime-yellow-1000)',
                },
                'magenta-strong': {
                    DEFAULT: '#FF00FF',
                    100: 'var(--color-magenta-strong-100)',
                    200: 'var(--color-magenta-strong-200)',
                    300: 'var(--color-magenta-strong-300)',
                    400: 'var(--color-magenta-strong-400)',
                    500: 'var(--color-magenta-strong-500)',
                    600: 'var(--color-magenta-strong-600)',
                    700: 'var(--color-magenta-strong-700)',
                    800: 'var(--color-magenta-strong-800)',
                    900: 'var(--color-magenta-strong-900)',
                    1000: 'var(--color-magenta-strong-1000)',
                },
            },
        },
    },
});