"use strict";
/*import { z } from "zod";
import { validateRut } from "../utils/validators.js";

export const registerUserSchema = z
    .object({
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

        email: z
            .string()
            .email("Debe ser un correo electrónico válido")
            .transform((val) => val.toLowerCase()),

        phone: z.string(),

        rut: z
            .string()
            .refine((val) => validateRut(val), { message: "RUT inválido" }),

        password: z
            .string()
            .min(8, "La contraseña debe tener al menos 8 caracteres")
            .regex(/[a-z]/, "Debe contener al menos una minúscula")
            .regex(/[A-Z]/, "Debe contener al menos una mayúscula")
            .regex(/\d/, "Debe contener al menos un número")
            .regex(/^[^\s]+$/, "No puede contener espacios")
            .regex(/^[^\\¡¿"ºª·`´ç]+$/, "Contiene caracteres no permitidos"),

        confirmPassword: z.string(),

        acceptTerms: z.literal(true, {
            errorMap: () => ({
                message: "Debes aceptar los términos y condiciones",
            }),
        }),

        roles: z.array(z.string()).optional(),
        permissions: z.array(z.string()).optional(),
    })
    .superRefine((data, ctx) => {
        if (data.password !== data.confirmPassword) {
            ctx.addIssue({
                code: "custom",
                path: ["confirmPassword"],
                message: "Las contraseñas no coinciden",
            });
        }
    });
    */ 
