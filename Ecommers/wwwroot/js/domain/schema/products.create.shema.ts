import { z } from "zod";

export const productsCreateSchema = z.object({
    name: z
        .string({
            required_error: "Debes ingresar el nombre del producto.",
            invalid_type_error: "El nombre del producto debe ser un texto."
        })
        .min(1, "Debes ingresar el nombre del producto.")
        .max(255, "El nombre del producto no puede superar los 255 caracteres."),

    slug: z
        .string({
            required_error: "Debes ingresar el slug del producto.",
            invalid_type_error: "El slug del producto debe ser un texto."
        })
        .min(1, "Debes ingresar el slug del producto.")
        .max(255, "El slug no puede superar los 255 caracteres."),

    description: z
        .string({
            required_error: "Debes ingresar la descripción del producto.",
            invalid_type_error: "La descripción debe ser un texto."
        })
        .min(1, "Debes ingresar la descripción del producto."),

    shortDescription: z
        .string({
            required_error: "Debes ingresar una descripción corta del producto.",
            invalid_type_error: "La descripción corta debe ser un texto."
        })
        .min(1, "Debes ingresar una descripción corta del producto.")
        .max(150, "La descripción corta no puede superar los 150 caracteres."),

    isActive: z.boolean().optional(),

    categoryId: z
        .number({
            required_error: "Debes seleccionar una categoría para el producto.",
            invalid_type_error: "La categoría seleccionada no es válida."
        })
        .int("La categoría seleccionada no es válida.")
        .positive("Debes seleccionar una categoría para el producto."),
    sku: z
        .string({
            required_error: "Debes ingresar el SKU de la variante.",
            invalid_type_error: "El SKU debe ser un texto."
        })
        .min(1, "Debes ingresar el SKU de la variante.")
        .max(100, "El SKU no puede superar los 100 caracteres."),

    stockQuantity: z
        .number({
            invalid_type_error: "La cantidad de stock debe ser un número."
        })
        .int("La cantidad de stock debe ser un número entero.")
        .min(0, "La cantidad de stock no puede ser negativa.")
        .nullable()
        .optional(),

    manageStock: z
        .boolean({
            invalid_type_error: "El valor de gestionar stock no es válido."
        })
        .optional()
        .nullable(),

    stockStatus: z
        .string({
            required_error: "Debes indicar el estado de stock.",
            invalid_type_error: "El estado de stock debe ser un texto."
        })
        .min(1, "Debes indicar el estado de stock."),
});
