"use strict";
/*import { z } from "zod";

export const changePasswordSchema = z
    .object({
        currentPassword: z.string(),

        newPassword: z
            .string()
            .min(8, "La contraseña debe tener al menos 8 caracteres")
            .regex(/[a-z]/, "Debe contener al menos una minúscula")
            .regex(/[A-Z]/, "Debe contener al menos una mayúscula")
            .regex(/\d/, "Debe contener al menos un número")
            .regex(/^[^\s]+$/, "No puede contener espacios")
            // Aquí elimina el regex de ñ y Ñ si usas español; solo ejemplo:
            .regex(/^[^\\¡¿"ºª·`´ç]+$/, "Contiene caracteres no permitidos"),

        confirmNewPassword: z.string(),
    })
    .superRefine((data, ctx) => {
        // Contraseña distinta a la actual
        if (data.newPassword === data.currentPassword) {
            ctx.addIssue({
                code: "custom",
                path: ["newPassword"],
                message: "La nueva contraseña debe ser diferente a la actual",
            });
        }

        // Confirmación de contraseña
        if (data.newPassword !== data.confirmNewPassword) {
            ctx.addIssue({
                code: "custom",
                path: ["confirmNewPassword"],
                message: "Las contraseñas no coinciden",
            });
        }
    });
    */ 
