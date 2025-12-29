import { z } from "zod";
export const loginUserSchema = z.object({
    email: z
        .string()
        .email({ message: "Debe ser un correo electrónico válido" }) // ✅ usar objeto
        .transform(val => val.toLowerCase()),
    password: z
        .string()
        .min(8, { message: "La contraseña debe tener al menos 8 caracteres" }), // ✅ usar objeto
});
