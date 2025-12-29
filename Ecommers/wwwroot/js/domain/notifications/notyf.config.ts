import { Notyf } from 'notyf';

// Crear instancia de Notyf con configuración personalizada
const notyf = new Notyf({
    duration: 3000,
    position: {
        x: 'right',
        y: 'top',
    },
    types: [
        {
            type: 'success',
            background: '#10b981',
            icon: false
        },
        {
            type: 'error',
            background: '#ef4444',
            icon: false
        },
        {
            type: 'warning',
            background: '#f59e0b',
            icon: false
        },
        {
            type: 'info',
            background: '#3b82f6',
            icon: false
        }
    ]
});

export const showSuccess = (message: string) => {
    notyf.success(message);
};

export const showError = (message: string) => {
    notyf.error(message);
};

export const showWarning = (message: string) => {
    notyf.open({
        type: 'warning',
        message: message
    });
};

export const showInfo = (message: string) => {
    notyf.open({
        type: 'info',
        message: message
    });
};

// Exportar la instancia si necesitas usarla directamente
export { notyf };