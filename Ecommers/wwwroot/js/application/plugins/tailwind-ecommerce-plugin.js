const plugin = require('tailwindcss/plugin');

module.exports = plugin(function ({ addBase, addComponents, addUtilities, theme }) {
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
    });

    // ==========================================
    // UTILIDADES (Clases de fondo, texto, etc.)
    // ==========================================
    addUtilities({
        // Colores de fondo
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
        '.shadow-burgundy-red\\/30': { boxShadow: '0 10px 25px rgba(139, 0, 0, 0.3)' },
        '.shadow-golden-yellow\\/30': { boxShadow: '0 10px 25px rgba(244, 196, 48, 0.3)' },
        '.shadow-mint-green\\/30': { boxShadow: '0 10px 25px rgba(152, 255, 152, 0.3)' },
        '.shadow-orange-warm\\/30': { boxShadow: '0 10px 25px rgba(255, 140, 0, 0.3)' },
        '.shadow-nut-brown\\/30': { boxShadow: '0 10px 25px rgba(139, 94, 60, 0.3)' },

        // Gradientes
        '.bg-gradient-nuts': {
            background: 'linear-gradient(135deg, var(--color-nut-brown) 0%, var(--color-golden-yellow) 100%)',
        },
        '.bg-gradient-natural': {
            background: 'linear-gradient(135deg, var(--color-olive-green) 0%, var(--color-mint-green) 100%)',
        },

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
                },
                'burgundy-red': {
                    DEFAULT: '#8B0000',
                    dark: '#650000',
                },
                'beige': {
                    DEFAULT: '#F5F5DC',
                    dark: '#E6E6C7',
                },
                'mint-green': {
                    DEFAULT: '#98FF98',
                    dark: '#7DE67D',
                },
                'dark-chocolate': '#3B2F2F',
                'orange-warm': {
                    DEFAULT: '#FF8C00',
                    dark: '#CC7000',
                },
            },
        },
    },
});