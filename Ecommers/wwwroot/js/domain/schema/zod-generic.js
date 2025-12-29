//// zod-generic.ts
import { z } from "zod";
export function castFormDataBySchema(formData, schema) {
    const raw = Object.fromEntries(formData.entries());
    const result = {};
    for (const [key, value] of Object.entries(raw)) {
        const fieldSchema = schema.shape[key];
        // Campo no existe en el schema → dejar tal cual
        if (!fieldSchema) {
            result[key] = value === "" ? null : value;
            continue;
        }
        // Detectar tipo base del schema
        const baseType = unwrapZodType(fieldSchema);
        // Cast por tipo
        if (baseType instanceof z.ZodNumber) {
            result[key] = value === "" ? null : Number(value);
        }
        else if (baseType instanceof z.ZodBoolean) {
            result[key] = value === "true" || value === "on" || value === true;
        }
        else {
            // string, date, etc
            result[key] = value === "" ? null : value;
        }
    }
    return result;
}
/**
 * Quita optional(), nullable(), effects(), etc.
 */
function unwrapZodType(schema) {
    while (schema instanceof z.ZodOptional ||
        schema instanceof z.ZodNullable ||
        schema instanceof z.ZodEffects) {
        schema = schema._def.schema;
    }
    return schema;
}
