import { z } from "zod";

/**
 * Zod schema para CategoriesD
 * Nombres iguales a la entidad C# (PascalCase)
 */
export const categoriesCreateSchema = z.object({
  Name: z
    .string()
    .min(1, "El nombre de la categoría es obligatorio.")
    .max(255, "El nombre de la categoría no puede superar los 255 caracteres."),

  Slug: z
    .string()
    .min(1, "El slug de la categoría es obligatorio.")
    .max(255, "El slug no puede superar los 255 caracteres."),

  ShortDescription: z
    .string()
    .max(500, "La descripción corta no puede superar los 500 caracteres.")
    .nullable()
    .optional(),

  Description: z
    .string()
    .nullable()
    .optional(),

  Image: z
    .string()
    .max(255, "La imagen no puede superar los 255 caracteres.")
    .nullable()
    .optional(),

  BgClass: z
    .string()
    .max(100, "La clase de fondo no puede superar los 100 caracteres.")
    .nullable()
    .optional(),

  SortOrder: z
    .number()
    .int("El orden debe ser un número entero.")
    .nullable()
    .optional(),

  IsActive: z.boolean(),

  ParentId: z
    .number()
    .int("La categoría padre debe ser un número entero.")
    .nullable()
    .optional(),

  CantidadProductos: z
    .number()
    .int("La cantidad de productos debe ser un número entero.")
    .nullable()
    .optional(),
});
