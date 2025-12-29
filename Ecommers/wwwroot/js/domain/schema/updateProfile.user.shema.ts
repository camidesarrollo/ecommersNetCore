/*import { z } from "zod"

export const updateProfileSchema = z.object({
    name: z
        .string()
        .min(2, "El nombre debe tener al menos 2 caracteres")
        .max(50, "El nombre no puede exceder 50 caracteres")
        .regex(/^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/, "El nombre solo puede contener letras"),

    apellido: z
        .string()
        .min(2, "El apellido debe tener al menos 2 caracteres")
        .max(50, "El apellido no puede exceder 50 caracteres")
        .regex(/^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/, "El apellido solo puede contener letras"),

    phone: z.string().optional(),
})
*/