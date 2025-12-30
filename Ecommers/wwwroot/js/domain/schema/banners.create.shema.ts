import { z } from "zod";

/**
 * Zod schema para crear Banners
 * Nombres en PascalCase (igual que backend C#)
 */
export const bannersCreateSchema = z.object({
  Seccion: z
    .string()
    .min(1, "La sección es obligatoria.")
    .max(255, "La sección no puede superar los 255 caracteres."),

  Image: z
    .string()
    .min(1, "La imagen es obligatoria.")
    .max(255, "La imagen no puede superar los 255 caracteres."),

  AltText: z
    .string()
    .min(1, "El texto alternativo es obligatorio.")
    .max(255, "El texto alternativo no puede superar los 255 caracteres."),

  Subtitulo: z
    .string()
    .min(1, "El subtítulo es obligatorio.")
    .max(255, "El subtítulo no puede superar los 255 caracteres."),

  Titulo: z
    .string()
    .min(1, "El título es obligatorio.")
    .max(255, "El título no puede superar los 255 caracteres."),

  BotonTexto: z
    .string()
    .min(1, "El texto del botón es obligatorio.")
    .max(255, "El texto del botón no puede superar los 255 caracteres."),

  BotonEnlace: z
    .string()
    .min(1, "El enlace del botón es obligatorio.")
        .max(255, "El enlace del botón no puede superar los 255 caracteres."),
    SortOrder: z
        .number()
        .int("El orden debe ser un número entero.")
        .nullable()
        .optional(),
    IsActive: z.boolean(),
});
