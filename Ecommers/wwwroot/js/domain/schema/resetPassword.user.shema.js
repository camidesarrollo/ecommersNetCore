/*import { z } from "zod";

export const resetPasswordSchema = z.object({
    email: z
        .string()
        .email("Debe ser un correo electrónico válido")
        .transform((val) => val.toLowerCase()),

    code: z
        .string()
        .regex(/^\d{6}$/, "El código debe tener exactamente 6 dígitos"),

    newPassword: z
        .string()
        .min(8, "La contraseña debe tener al menos 8 caracteres")
        .regex(/[a-z]/, "Debe contener al menos una minúscula")
        .regex(/[A-Z]/, "Debe contener al menos una mayúscula")
        .regex(/\d/, "Debe contener al menos un número")
        .regex(/^[^\s]+$/, "No puede contener espacios")
        .regex(/^[^\\¡¿"ºª·`´çñÑ]+$/, "Contiene caracteres no permitidos"),

    confirmNewPassword: z
        .string()
        .refine((val, ctx) => val === ctx.parent.newPassword, {
            message: "Las contraseñas no coinciden",
        }),
})
*/ 
