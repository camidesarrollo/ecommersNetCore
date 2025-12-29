// src/domain/utils/constants.js


// ========================================
// API Y CONFIGURACIÓN
// ========================================

export const API_BASE_URL = 'http://localhost:8000/api';
export const API_TIMEOUT = 30000 // 30 segundos
export const MAX_UPLOAD_SIZE = 5 * 1024 * 1024 // 5MB
export const ALLOWED_IMAGE_TYPES = ['image/jpeg', 'image/png', 'image/webp', 'image/gif']
export const ALLOWED_DOCUMENT_TYPES = ['application/pdf', 'application/msword']

// ========================================
// PAGINACIÓN
// ========================================

export const DEFAULT_PAGE_SIZE = 20
export const PAGE_SIZE_OPTIONS = [10, 20, 50, 100]
export const MAX_PAGE_SIZE = 100

// ========================================
// REGIONES DE CHILE
// ========================================

export const REGIONES_CHILE = [
    { id: 1, nombre: 'Arica y Parinacota', codigo: 'AP' },
    { id: 2, nombre: 'Tarapacá', codigo: 'TA' },
    { id: 3, nombre: 'Antofagasta', codigo: 'AN' },
    { id: 4, nombre: 'Atacama', codigo: 'AT' },
    { id: 5, nombre: 'Coquimbo', codigo: 'CO' },
    { id: 6, nombre: 'Valparaíso', codigo: 'VA' },
    { id: 7, nombre: 'Metropolitana de Santiago', codigo: 'RM' },
    { id: 8, nombre: "O'Higgins", codigo: 'LI' },
    { id: 9, nombre: 'Maule', codigo: 'ML' },
    { id: 10, nombre: 'Ñuble', codigo: 'NB' },
    { id: 11, nombre: 'Biobío', codigo: 'BI' },
    { id: 12, nombre: 'La Araucanía', codigo: 'AR' },
    { id: 13, nombre: 'Los Ríos', codigo: 'LR' },
    { id: 14, nombre: 'Los Lagos', codigo: 'LL' },
    { id: 15, nombre: 'Aysén', codigo: 'AI' },
    { id: 16, nombre: 'Magallanes', codigo: 'MA' }
]

// ========================================
// ROLES DE USUARIO
// ========================================

export const USER_ROLES = {
    ADMIN: 'admin',
    SELLER: 'vendedor',
    BUYER: 'comprador',
    GUEST: 'invitado'
}

export const ROLE_NAMES = {
    [USER_ROLES.ADMIN]: 'Administrador',
    [USER_ROLES.SELLER]: 'Vendedor',
    [USER_ROLES.BUYER]: 'Comprador',
    [USER_ROLES.GUEST]: 'Invitado'
}

export const ROLE_PERMISSIONS = {
    [USER_ROLES.ADMIN]: ['*'], // Todos los permisos
    [USER_ROLES.SELLER]: ['products.create', 'products.edit', 'orders.view', 'orders.manage'],
    [USER_ROLES.BUYER]: ['orders.create', 'orders.view', 'profile.edit'],
    [USER_ROLES.GUEST]: ['products.view']
}

// ========================================
// ESTADOS DE PEDIDO
// ========================================

export const ORDER_STATUS = {
    PENDING: 'pendiente',
    CONFIRMED: 'confirmado',
    PREPARING: 'preparando',
    SHIPPED: 'enviado',
    DELIVERED: 'entregado',
    CANCELLED: 'cancelado',
    RETURNED: 'devuelto'
}

export const ORDER_STATUS_LABELS = {
    [ORDER_STATUS.PENDING]: 'Pendiente',
    [ORDER_STATUS.CONFIRMED]: 'Confirmado',
    [ORDER_STATUS.PREPARING]: 'En preparación',
    [ORDER_STATUS.SHIPPED]: 'Enviado',
    [ORDER_STATUS.DELIVERED]: 'Entregado',
    [ORDER_STATUS.CANCELLED]: 'Cancelado',
    [ORDER_STATUS.RETURNED]: 'Devuelto'
}

export const ORDER_STATUS_COLORS = {
    [ORDER_STATUS.PENDING]: 'bg-yellow-100 text-yellow-800 border-yellow-300',
    [ORDER_STATUS.CONFIRMED]: 'bg-blue-100 text-blue-800 border-blue-300',
    [ORDER_STATUS.PREPARING]: 'bg-purple-100 text-purple-800 border-purple-300',
    [ORDER_STATUS.SHIPPED]: 'bg-indigo-100 text-indigo-800 border-indigo-300',
    [ORDER_STATUS.DELIVERED]: 'bg-green-100 text-green-800 border-green-300',
    [ORDER_STATUS.CANCELLED]: 'bg-red-100 text-red-800 border-red-300',
    [ORDER_STATUS.RETURNED]: 'bg-gray-100 text-gray-800 border-gray-300'
}

// ========================================
// MÉTODOS DE PAGO
// ========================================

export const PAYMENT_METHODS = {
    CREDIT_CARD: 'tarjeta_credito',
    DEBIT_CARD: 'tarjeta_debito',
    TRANSFER: 'transferencia',
    WEBPAY: 'webpay',
    MERCADOPAGO: 'mercadopago',
    CASH: 'efectivo'
}

export const PAYMENT_METHOD_LABELS = {
    [PAYMENT_METHODS.CREDIT_CARD]: 'Tarjeta de Crédito',
    [PAYMENT_METHODS.DEBIT_CARD]: 'Tarjeta de Débito',
    [PAYMENT_METHODS.TRANSFER]: 'Transferencia Bancaria',
    [PAYMENT_METHODS.WEBPAY]: 'Webpay',
    [PAYMENT_METHODS.MERCADOPAGO]: 'Mercado Pago',
    [PAYMENT_METHODS.CASH]: 'Efectivo'
}

export const PAYMENT_METHOD_ICONS = {
    [PAYMENT_METHODS.CREDIT_CARD]: 'credit-card',
    [PAYMENT_METHODS.DEBIT_CARD]: 'credit-card',
    [PAYMENT_METHODS.TRANSFER]: 'exchange-alt',
    [PAYMENT_METHODS.WEBPAY]: 'shopping-cart',
    [PAYMENT_METHODS.MERCADOPAGO]: 'money-check-alt',
    [PAYMENT_METHODS.CASH]: 'money-bill-wave'
}

// ========================================
// ESTADOS DE PAGO
// ========================================

export const PAYMENT_STATUS = {
    PENDING: 'pendiente',
    PROCESSING: 'procesando',
    APPROVED: 'aprobado',
    REJECTED: 'rechazado',
    REFUNDED: 'reembolsado'
}

export const PAYMENT_STATUS_LABELS = {
    [PAYMENT_STATUS.PENDING]: 'Pendiente',
    [PAYMENT_STATUS.PROCESSING]: 'Procesando',
    [PAYMENT_STATUS.APPROVED]: 'Aprobado',
    [PAYMENT_STATUS.REJECTED]: 'Rechazado',
    [PAYMENT_STATUS.REFUNDED]: 'Reembolsado'
}

// ========================================
// MÉTODOS DE ENVÍO
// ========================================

export const SHIPPING_METHODS = {
    STANDARD: 'estandar',
    EXPRESS: 'express',
    PICKUP: 'retiro_tienda',
    FREE: 'gratis'
}

export const SHIPPING_METHOD_LABELS = {
    [SHIPPING_METHODS.STANDARD]: 'Envío Estándar (3-5 días)',
    [SHIPPING_METHODS.EXPRESS]: 'Envío Express (1-2 días)',
    [SHIPPING_METHODS.PICKUP]: 'Retiro en Tienda',
    [SHIPPING_METHODS.FREE]: 'Envío Gratis'
}

export const SHIPPING_METHOD_PRICES = {
    [SHIPPING_METHODS.STANDARD]: 3000,
    [SHIPPING_METHODS.EXPRESS]: 5000,
    [SHIPPING_METHODS.PICKUP]: 0,
    [SHIPPING_METHODS.FREE]: 0
}

// ========================================
// CATEGORÍAS DE PRODUCTOS
// ========================================

export const PRODUCT_CATEGORIES = {
    NUTS: 'frutos_secos',
    DRIED_FRUITS: 'frutas_deshidratadas',
    SEEDS: 'semillas',
    MIXES: 'mezclas',
    SNACKS: 'snacks_saludables',
    SUPERFOODS: 'superalimentos',
    ORGANIC: 'organicos'
}

export const CATEGORY_LABELS = {
    [PRODUCT_CATEGORIES.NUTS]: 'Frutos Secos',
    [PRODUCT_CATEGORIES.DRIED_FRUITS]: 'Frutas Deshidratadas',
    [PRODUCT_CATEGORIES.SEEDS]: 'Semillas',
    [PRODUCT_CATEGORIES.MIXES]: 'Mezclas',
    [PRODUCT_CATEGORIES.SNACKS]: 'Snacks Saludables',
    [PRODUCT_CATEGORIES.SUPERFOODS]: 'Superalimentos',
    [PRODUCT_CATEGORIES.ORGANIC]: 'Orgánicos'
}

// ========================================
// UNIDADES DE MEDIDA
// ========================================

export const UNITS = {
    GRAM: 'g',
    KILOGRAM: 'kg',
    UNIT: 'unidad',
    PACK: 'paquete',
    BOX: 'caja'
}

export const UNIT_LABELS = {
    [UNITS.GRAM]: 'Gramos',
    [UNITS.KILOGRAM]: 'Kilogramos',
    [UNITS.UNIT]: 'Unidad',
    [UNITS.PACK]: 'Paquete',
    [UNITS.BOX]: 'Caja'
}

// ========================================
// VALIDACIÓN
// ========================================

export const VALIDATION_RULES = {
    MIN_PASSWORD_LENGTH: 8,
    MAX_PASSWORD_LENGTH: 50,
    MIN_NAME_LENGTH: 2,
    MAX_NAME_LENGTH: 100,
    MIN_PHONE_LENGTH: 11,
    MAX_PHONE_LENGTH: 15,
    RUT_MIN_LENGTH: 8,
    RUT_MAX_LENGTH: 12,
    MIN_PRODUCT_PRICE: 100,
    MAX_PRODUCT_PRICE: 10000000,
    MIN_QUANTITY: 1,
    MAX_QUANTITY: 999,
    MIN_RATING: 1,
    MAX_RATING: 5
}

// ========================================
// MENSAJES DE ERROR
// ========================================

export const ERROR_MESSAGES = {
    NETWORK_ERROR: 'Error de conexión. Verifica tu internet',
    SERVER_ERROR: 'Error del servidor. Intenta nuevamente',
    UNAUTHORIZED: 'No tienes permisos para realizar esta acción',
    SESSION_EXPIRED: 'Tu sesión ha expirado. Por favor, inicia sesión nuevamente',
    VALIDATION_ERROR: 'Por favor, corrige los errores en el formulario',
    NOT_FOUND: 'No se encontró el recurso solicitado',
    TIMEOUT: 'La solicitud tardó demasiado tiempo',
    UNKNOWN_ERROR: 'Ocurrió un error inesperado'
}

// ========================================
// MENSAJES DE ÉXITO
// ========================================

export const SUCCESS_MESSAGES = {
    SAVE_SUCCESS: 'Guardado exitosamente',
    UPDATE_SUCCESS: 'Actualizado exitosamente',
    DELETE_SUCCESS: 'Eliminado exitosamente',
    LOGIN_SUCCESS: 'Inicio de sesión exitoso',
    LOGOUT_SUCCESS: 'Sesión cerrada exitosamente',
    REGISTER_SUCCESS: 'Registro exitoso',
    PASSWORD_RESET: 'Contraseña restablecida exitosamente',
    EMAIL_SENT: 'Correo enviado exitosamente',
    ORDER_CREATED: 'Pedido creado exitosamente',
    PAYMENT_SUCCESS: 'Pago realizado exitosamente'
}

// ========================================
// TIEMPOS (en milisegundos)
// ========================================

export const TIMEOUTS = {
    TOAST_DURATION: 5000,
    SHORT_TOAST: 3000,
    LONG_TOAST: 7000,
    DEBOUNCE_DELAY: 300,
    THROTTLE_DELAY: 1000,
    AUTO_LOGOUT: 1800000, // 30 minutos
    SESSION_WARNING: 1500000 // 25 minutos
}

// ========================================
// STORAGE KEYS
// ========================================

export const STORAGE_KEYS = {
    AUTH_TOKEN: 'auth_token',
    USER_ID: 'user_id',
    USER_NAME: 'user_name',
    USER_EMAIL: 'user_email',
    USER_ROLE: 'user_role',
    CART: 'shopping_cart',
    THEME: 'theme_preference',
    LANGUAGE: 'language_preference',
    REMEMBER_ME: 'remember_me',
    LAST_VISIT: 'last_visit'
}

// ========================================
// RUTAS DE NAVEGACIÓN
// ========================================

export const ROUTES = {
    HOME: '/',
    LOGIN: '/login',
    REGISTER: '/register',
    FORGOT_PASSWORD: '/forgot-password',
    RESET_PASSWORD: '/reset-password',
    DASHBOARD: '/dashboard',
    PROFILE: '/profile',
    PRODUCTS: '/products',
    PRODUCT_DETAIL: '/products/:id',
    CART: '/cart',
    CHECKOUT: '/checkout',
    ORDERS: '/orders',
    ORDER_DETAIL: '/orders/:id',
    ADMIN: '/Dashboard',
    SETTINGS: '/settings',
    TERMS: '/terminos',
    PRIVACY: '/privacidad'
}

// ========================================
// COLORES DEL TEMA (Tailwind classes)
// ========================================

export const THEME_COLORS = {
    PRIMARY: 'olive-green',
    SECONDARY: 'nut-brown',
    ACCENT: 'golden-yellow',
    SUCCESS: 'olive-green',
    ERROR: 'burgundy-red',
    WARNING: 'orange-warm',
    INFO: 'blue-500',
    DARK: 'dark-chocolate',
    LIGHT: 'beige',
    NEUTRAL: 'gray-dark'
}

// ========================================
// ICONOS FONTAWESOME
// ========================================

export const ICONS = {
    USER: ['fas', 'user'],
    EMAIL: ['fas', 'envelope'],
    PHONE: ['fas', 'phone'],
    LOCK: ['fas', 'lock'],
    CART: ['fas', 'shopping-cart'],
    HEART: ['fas', 'heart'],
    STAR: ['fas', 'star'],
    SEARCH: ['fas', 'search'],
    FILTER: ['fas', 'filter'],
    TRASH: ['fas', 'trash'],
    EDIT: ['fas', 'edit'],
    SAVE: ['fas', 'save'],
    CANCEL: ['fas', 'times'],
    CHECK: ['fas', 'check'],
    ARROW_LEFT: ['fas', 'arrow-left'],
    ARROW_RIGHT: ['fas', 'arrow-right'],
    HOME: ['fas', 'home'],
    DASHBOARD: ['fas', 'tachometer-alt'],
    PRODUCTS: ['fas', 'box'],
    ORDERS: ['fas', 'shopping-bag'],
    SETTINGS: ['fas', 'cog'],
    LOGOUT: ['fas', 'sign-out-alt']
}

// ========================================
// EXPRESIONES REGULARES
// ========================================

export const REGEX_PATTERNS = {
    EMAIL: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
    PHONE_CL: /^(\+?56)?9\d{8}$/,
    RUT: /^\d{1,2}\.\d{3}\.\d{3}-[\dkK]$/,
    NUMBERS_ONLY: /^\d+$/,
    LETTERS_ONLY: /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/,
    ALPHANUMERIC: /^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s]+$/,
    PASSWORD_STRENGTH: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d@$!%*?&]{8,}$/,
    URL: /^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$/
}

// ========================================
// CÓDIGOS HTTP
// ========================================

export const HTTP_STATUS = {
    OK: 200,
    CREATED: 201,
    NO_CONTENT: 204,
    BAD_REQUEST: 400,
    UNAUTHORIZED: 401,
    FORBIDDEN: 403,
    NOT_FOUND: 404,
    UNPROCESSABLE_ENTITY: 422,
    INTERNAL_SERVER_ERROR: 500,
    SERVICE_UNAVAILABLE: 503
}

// ========================================
// LÍMITES DE PRODUCTOS
// ========================================

export const PRODUCT_LIMITS = {
    MIN_STOCK: 0,
    LOW_STOCK_THRESHOLD: 10,
    MAX_CART_QUANTITY: 99,
    MIN_PRICE: 100,
    MAX_IMAGES: 5,
    MIN_DESCRIPTION_LENGTH: 20,
    MAX_DESCRIPTION_LENGTH: 1000
}

// ========================================
// CONFIGURACIÓN DE CARRITO
// ========================================

export const CART_CONFIG = {
    FREE_SHIPPING_THRESHOLD: 30000, // Envío gratis sobre $30.000
    MAX_ITEMS: 50,
    CART_EXPIRY_DAYS: 7,
    SESSION_TIMEOUT: 30 * 60 * 1000 // 30 minutos
}

// ========================================
// NOTIFICACIONES
// ========================================

export const NOTIFICATION_TYPES = {
    SUCCESS: 'success',
    ERROR: 'error',
    WARNING: 'warning',
    INFO: 'info'
}

export const NOTIFICATION_POSITIONS = {
    TOP_RIGHT: 'top-right',
    TOP_LEFT: 'top-left',
    TOP_CENTER: 'top-center',
    BOTTOM_RIGHT: 'bottom-right',
    BOTTOM_LEFT: 'bottom-left',
    BOTTOM_CENTER: 'bottom-center'
}

// ========================================
// ORDENAMIENTO
// ========================================

export const SORT_OPTIONS = {
    NEWEST: 'newest',
    OLDEST: 'oldest',
    PRICE_ASC: 'price_asc',
    PRICE_DESC: 'price_desc',
    NAME_ASC: 'name_asc',
    NAME_DESC: 'name_desc',
    POPULAR: 'popular',
    RATING: 'rating'
}

export const SORT_LABELS = {
    [SORT_OPTIONS.NEWEST]: 'Más recientes',
    [SORT_OPTIONS.OLDEST]: 'Más antiguos',
    [SORT_OPTIONS.PRICE_ASC]: 'Precio: Menor a mayor',
    [SORT_OPTIONS.PRICE_DESC]: 'Precio: Mayor a menor',
    [SORT_OPTIONS.NAME_ASC]: 'Nombre: A-Z',
    [SORT_OPTIONS.NAME_DESC]: 'Nombre: Z-A',
    [SORT_OPTIONS.POPULAR]: 'Más populares',
    [SORT_OPTIONS.RATING]: 'Mejor valorados'
}