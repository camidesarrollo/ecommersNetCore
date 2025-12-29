// src/domain/utils/validators.js

/**
 * Utilidades de validación puras (sin estado ni reactividad)
 * Estas funciones pueden ser usadas tanto en composables como en lógica pura
 */

// ========================================
// VALIDADORES DE IDENTIFICACIÓN
// ========================================

/**
 * Validar formato y dígito verificador de RUT chileno
 * @param {string} rut - RUT a validar
 * @returns {boolean} True si es válido
 */
export const validateRut = (rut) => {
    if (!rut) return false

    // Limpiar RUT
    const cleanRut = rut.replace(/\./g, '').replace(/-/g, '').toUpperCase()

    if (cleanRut.length < 2) return false

    const body = cleanRut.slice(0, -1)
    const dv = cleanRut.slice(-1)

    // Verificar que el cuerpo sea numérico
    if (!/^\d+$/.test(body)) return false

    // Calcular dígito verificador
    let suma = 0
    let multiplicador = 2

    for (let i = body.length - 1; i >= 0; i--) {
        suma += parseInt(body[i]) * multiplicador
        multiplicador = multiplicador === 7 ? 2 : multiplicador + 1
    }

    const dvEsperado = 11 - (suma % 11)
    const dvCalculado = dvEsperado === 11 ? '0' : dvEsperado === 10 ? 'K' : dvEsperado.toString()

    return dv === dvCalculado
}

/**
 * Obtener dígito verificador de RUT
 * @param {string} rut - RUT sin dígito verificador
 * @returns {string} Dígito verificador
 */
export const getRutDigit = (rut) => {
    if (!rut) return ''

    const cleanRut = rut.replace(/\./g, '').replace(/-/g, '')

    let suma = 0
    let multiplicador = 2

    for (let i = cleanRut.length - 1; i >= 0; i--) {
        suma += parseInt(cleanRut[i]) * multiplicador
        multiplicador = multiplicador === 7 ? 2 : multiplicador + 1
    }

    const dv = 11 - (suma % 11)
    return dv === 11 ? '0' : dv === 10 ? 'K' : dv.toString()
}

// ========================================
// VALIDADORES DE CONTACTO
// ========================================

/**
 * Validar formato de email
 * @param {string} email - Email a validar
 * @returns {boolean} True si es válido
 */
export const validateEmail = (email) => {
    if (!email) return false

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return emailRegex.test(email)
}

/**
 * Validar formato de email más estricto
 * @param {string} email - Email a validar
 * @returns {boolean} True si es válido
 */
export const validateEmailStrict = (email) => {
    if (!email) return false

    // RFC 5322 simplificado
    const emailRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/
    return emailRegex.test(email)
}

/**
 * Validar teléfono chileno móvil
 * @param {string} phone - Teléfono a validar
 * @returns {boolean} True si es válido
 */
export const validateChileanMobilePhone = (phone) => {
    if (!phone) return false

    // Limpiar espacios y caracteres especiales
    const cleanPhone = phone.replace(/\s/g, '').replace(/[+()-]/g, '')

    // Formato: 569XXXXXXXX (11 dígitos) o 9XXXXXXXX (9 dígitos)
    const mobileRegex = /^(56)?9\d{8}$/
    return mobileRegex.test(cleanPhone)
}

/**
 * Validar teléfono chileno fijo
 * @param {string} phone - Teléfono a validar
 * @returns {boolean} True si es válido
 */
export const validateChileanLandlinePhone = (phone) => {
    if (!phone) return false

    const cleanPhone = phone.replace(/\s/g, '').replace(/[+()-]/g, '')

    // Formato: 56[2-9]XXXXXXXX o [2-9]XXXXXXXX
    const landlineRegex = /^(56)?[2-9]\d{8}$/
    return landlineRegex.test(cleanPhone)
}

/**
 * Validar cualquier teléfono chileno
 * @param {string} phone - Teléfono a validar
 * @returns {boolean} True si es válido
 */
export const validateChileanPhone = (phone) => {
    return validateChileanMobilePhone(phone) || validateChileanLandlinePhone(phone)
}

// ========================================
// VALIDADORES DE CONTRASEÑA
// ========================================

/**
 * Validar longitud mínima de contraseña
 * @param {string} password - Contraseña
 * @param {number} minLength - Longitud mínima
 * @returns {boolean} True si cumple
 */
export const validatePasswordLength = (password, minLength = 8) => {
    return password && password.length >= minLength
}

/**
 * Validar que contraseña tiene al menos un número
 * @param {string} password - Contraseña
 * @returns {boolean} True si cumple
 */
export const validatePasswordHasNumber = (password) => {
    return /\d/.test(password)
}

/**
 * Validar que contraseña tiene al menos una mayúscula
 * @param {string} password - Contraseña
 * @returns {boolean} True si cumple
 */
export const validatePasswordHasUppercase = (password) => {
    return /[A-Z]/.test(password)
}

/**
 * Validar que contraseña tiene al menos una minúscula
 * @param {string} password - Contraseña
 * @returns {boolean} True si cumple
 */
export const validatePasswordHasLowercase = (password) => {
    return /[a-z]/.test(password)
}

/**
 * Validar que contraseña tiene al menos un carácter especial
 * @param {string} password - Contraseña
 * @returns {boolean} True si cumple
 */
export const validatePasswordHasSpecialChar = (password) => {
    return /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(password)
}

/**
 * Validar que contraseña no tiene espacios
 * @param {string} password - Contraseña
 * @returns {boolean} True si cumple
 */
export const validatePasswordNoSpaces = (password) => {
    return !/\s/.test(password)
}

/**
 * Validar que contraseña no tiene caracteres inválidos
 * @param {string} password - Contraseña
 * @returns {boolean} True si cumple
 */
export const validatePasswordNoInvalidChars = (password) => {
    return !/[\\¡¿"ºª·`´çñÑ]/.test(password)
}

/**
 * Validar fortaleza de contraseña completa
 * @param {string} password - Contraseña
 * @returns {Object} Objeto con validaciones individuales
 */
export const validatePasswordStrength = (password) => {
    return {
        minLength: validatePasswordLength(password),
        hasNumber: validatePasswordHasNumber(password),
        hasUppercase: validatePasswordHasUppercase(password),
        hasLowercase: validatePasswordHasLowercase(password),
        hasSpecialChar: validatePasswordHasSpecialChar(password),
        noSpaces: validatePasswordNoSpaces(password),
        noInvalidChars: validatePasswordNoInvalidChars(password),
        isValid: validatePasswordLength(password) &&
            validatePasswordHasNumber(password) &&
            validatePasswordHasUppercase(password) &&
            validatePasswordHasLowercase(password) &&
            validatePasswordNoSpaces(password) &&
            validatePasswordNoInvalidChars(password)
    }
}

// ========================================
// VALIDADORES DE TEXTO
// ========================================

/**
 * Validar que un texto solo contiene letras
 * @param {string} text - Texto a validar
 * @returns {boolean} True si es válido
 */
export const isAlphabetic = (text) => {
    if (!text) return false
    return /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/.test(text)
}

/**
 * Validar que un texto solo contiene números
 * @param {string} text - Texto a validar
 * @returns {boolean} True si es válido
 */
export const isNumeric = (text) => {
    if (!text) return false
    return /^\d+$/.test(text)
}

/**
 * Validar que un texto es alfanumérico
 * @param {string} text - Texto a validar
 * @returns {boolean} True si es válido
 */
export const isAlphanumeric = (text) => {
    if (!text) return false
    return /^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s]+$/.test(text)
}

/**
 * Validar longitud de texto
 * @param {string} text - Texto a validar
 * @param {number} min - Longitud mínima
 * @param {number} max - Longitud máxima
 * @returns {boolean} True si es válido
 */
export const validateTextLength = (text, min = 0, max = Infinity) => {
    if (!text) return min === 0
    return text.length >= min && text.length <= max
}

// ========================================
// VALIDADORES DE URL
// ========================================

/**
 * Validar formato de URL
 * @param {string} url - URL a validar
 * @returns {boolean} True si es válido
 */
export const validateUrl = (url) => {
    if (!url) return false

    try {
        new URL(url)
        return true
    } catch {
        return false
    }
}

/**
 * Validar que URL es HTTPS
 * @param {string} url - URL a validar
 * @returns {boolean} True si es HTTPS
 */
export const isHttpsUrl = (url) => {
    if (!validateUrl(url)) return false
    return url.startsWith('https://')
}

// ========================================
// VALIDADORES DE EDAD
// ========================================

/**
 * Calcular edad desde fecha de nacimiento
 * @param {Date|string} birthDate - Fecha de nacimiento
 * @returns {number} Edad en años
 */
export const calculateAge = (birthDate) => {
    const birth = birthDate instanceof Date ? birthDate : new Date(birthDate)
    const today = new Date()

    let age = today.getFullYear() - birth.getFullYear()
    const monthDiff = today.getMonth() - birth.getMonth()

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
        age--
    }

    return age
}

/**
 * Validar edad mínima
 * @param {Date|string} birthDate - Fecha de nacimiento
 * @param {number} minAge - Edad mínima requerida
 * @returns {boolean} True si cumple edad mínima
 */
export const validateMinimumAge = (birthDate, minAge = 18) => {
    const age = calculateAge(birthDate)
    return age >= minAge
}

// ========================================
// VALIDADORES DE ARCHIVOS
// ========================================

/**
 * Validar tipo de archivo por extensión
 * @param {string} filename - Nombre del archivo
 * @param {Array<string>} allowedExtensions - Extensiones permitidas
 * @returns {boolean} True si es válido
 */
export const validateFileExtension = (filename, allowedExtensions) => {
    if (!filename || !allowedExtensions || allowedExtensions.length === 0) {
        return false
    }

    const extension = filename.split('.').pop().toLowerCase()
    return allowedExtensions.map(ext => ext.toLowerCase()).includes(extension)
}

/**
 * Validar tamaño de archivo
 * @param {number} fileSize - Tamaño en bytes
 * @param {number} maxSizeMB - Tamaño máximo en MB
 * @returns {boolean} True si es válido
 */
export const validateFileSize = (fileSize, maxSizeMB) => {
    if (!fileSize || !maxSizeMB) return false

    const maxSizeBytes = maxSizeMB * 1024 * 1024
    return fileSize <= maxSizeBytes
}

/**
 * Validar que es archivo de imagen
 * @param {string} filename - Nombre del archivo
 * @returns {boolean} True si es imagen
 */
export const isImageFile = (filename) => {
    const imageExtensions = ['jpg', 'jpeg', 'png', 'gif', 'webp', 'svg', 'bmp']
    return validateFileExtension(filename, imageExtensions)
}

/**
 * Validar que es archivo de documento
 * @param {string} filename - Nombre del archivo
 * @returns {boolean} True si es documento
 */
export const isDocumentFile = (filename) => {
    const docExtensions = ['pdf', 'doc', 'docx', 'xls', 'xlsx', 'ppt', 'pptx', 'txt']
    return validateFileExtension(filename, docExtensions)
}

// ========================================
// VALIDADORES DE TARJETAS DE CRÉDITO
// ========================================

/**
 * Validar número de tarjeta con algoritmo de Luhn
 * @param {string} cardNumber - Número de tarjeta
 * @returns {boolean} True si es válido
 */
export const validateCreditCard = (cardNumber) => {
    if (!cardNumber) return false

    const cleaned = cardNumber.replace(/\s/g, '').replace(/-/g, '')

    if (!/^\d+$/.test(cleaned)) return false
    if (cleaned.length < 13 || cleaned.length > 19) return false

    // Algoritmo de Luhn
    let sum = 0
    let isEven = false

    for (let i = cleaned.length - 1; i >= 0; i--) {
        let digit = parseInt(cleaned[i])

        if (isEven) {
            digit *= 2
            if (digit > 9) {
                digit -= 9
            }
        }

        sum += digit
        isEven = !isEven
    }

    return sum % 10 === 0
}

/**
 * Detectar tipo de tarjeta de crédito
 * @param {string} cardNumber - Número de tarjeta
 * @returns {string|null} Tipo de tarjeta o null
 */
export const detectCardType = (cardNumber) => {
    if (!cardNumber) return null

    const cleaned = cardNumber.replace(/\s/g, '').replace(/-/g, '')

    const patterns = {
        visa: /^4/,
        mastercard: /^5[1-5]/,
        amex: /^3[47]/,
        discover: /^6(?:011|5)/,
        diners: /^3(?:0[0-5]|[68])/,
        jcb: /^35/
    }

    for (const [type, pattern] of Object.entries(patterns)) {
        if (pattern.test(cleaned)) {
            return type
        }
    }

    return null
}