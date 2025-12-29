import { z } from "zod";
/**
 * Zod schema para CategoriesD
 * Nombres iguales a la entidad C# (PascalCase)
 */
export const serviciosUpdateSchema = z.object({
    Id: z.number().int(), // BaseEntity<long>
    Name: z
        .string()
        .min(1, "El nombre del servicio es obligatorio.")
        .max(255, "El nombre del servicio no puede superar los 255 caracteres."),
    Description: z
        .string()
        .max(1000, "La descripción no puede superar los 1000 caracteres.")
        .nullable()
        .optional(),
    Image: z
        .string()
        .max(255, "La imagen no puede superar los 255 caracteres.")
        .nullable()
        .optional(),
    SortOrder: z
        .number()
        .int("El orden debe ser un número entero.")
        .nullable()
        .optional(),
    IsActive: z.boolean(),
});
