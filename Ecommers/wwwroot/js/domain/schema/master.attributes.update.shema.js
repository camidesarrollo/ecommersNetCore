import { z } from "zod";
/**
 * Schema para crear MasterAttribute
 */
export const masterAttributesUpdateSchema = z.object({
    Id: z.number().int(), // BaseEntity<long>
    Name: z
        .string()
        .min(2, "El nombre es obligatorio")
        .max(255),
    Description: z
        .string()
        .max(5000)
        .optional()
        .nullable(),
    Slug: z
        .string()
        .min(2)
        .max(255)
        .regex(/^[a-z0-9_]+$/, "El slug solo puede contener minúsculas, números y _"),
    DataType: z
        .string()
        .min(2)
        .max(50),
    InputType: z
        .string()
        .min(2)
        .max(50),
    Unit: z
        .string()
        .max(50)
        .optional()
        .nullable(),
    IsRequired: z.boolean().default(false),
    IsFilterable: z.boolean().default(false),
    IsVariant: z.boolean().default(false),
    Category: z
        .string()
        .max(100)
        .optional()
        .nullable(),
    IsActive: z.boolean().default(true)
});
