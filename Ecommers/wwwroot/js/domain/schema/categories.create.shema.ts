import { z } from "zod";

/**
 * Zod schema para CategoriesD
 * Nombres iguales a la entidad C# (PascalCase)
 */
export const categoriesCreateSchema = z.object({
    Name: z
        .string({
            required_error: "Debe ingresar el nombre de la categoría.",
        })
        .min(1, "Debe ingresar el nombre de la categoría.")
        .max(255, "El nombre no puede superar los 255 caracteres."),

    Slug: z
        .string({
            required_error: "Debe ingresar el slug de la categoría.",
        })
        .min(1, "Debe ingresar el slug de la categoría.")
        .max(255, "El slug no puede superar los 255 caracteres."),

    ShortDescription: z
        .string()
        .min(1, "Debe ingresar una descripción corta.")
        .max(150, "La descripción corta no puede superar los 150 caracteres."),

    Description: z
        .string()
        .min(1, "Debe ingresar la descripción completa de la categoría."),

    Image: z
        .string()
        .max(255, "La ruta de la imagen no puede superar los 255 caracteres.")
        .nullable()
        .optional(),

    BgClass: z
        .string()
        .min(1, "Debe seleccionar un color o gradiente de fondo.")
        .max(255, "La clase de fondo no puede superar los 255 caracteres.")
        .nullable()
        .optional(),

    SortOrder: z
        .number({
            invalid_type_error: "El orden de visualización debe ser un número.",
        })
        .int("El orden de visualización debe ser un número entero.")
        .min(0, "El orden de visualización debe ser mayor o igual a 0."),

    IsActive: z.boolean({
        required_error: "Debe indicar si la categoría está activa.",
    }),

    ParentId: z
        .number({
            invalid_type_error: "La categoría padre no es válida.",
        })
        .int("La categoría padre debe ser un número entero.")
        .nullable()
        .optional(),
});

