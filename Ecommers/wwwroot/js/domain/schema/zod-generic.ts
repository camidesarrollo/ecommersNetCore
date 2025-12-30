import { z } from "zod";

export function castFormDataBySchema(formData: any, schema: any) {
    const raw = Object.fromEntries(formData.entries());
    const result: any = {};

    for (const [key, value] of Object.entries(raw)) {
        const fieldSchema = schema.shape?.[key];
  
        // Campo no existe en el schema → dejar tal cual
        if (fieldSchema == undefined) {
            continue;
        }
        if (!fieldSchema) {
            result[key] = value === "" ? null : value;
            continue;
        }

        // Detectar tipo base del schema
        const baseType = unwrapZodType(fieldSchema);
        // Cast por tipo usando _def.typeName (más seguro que instanceof)
        const typeName = baseType.type;
        if (
            typeName === 'number' ||
            typeName === 'optional' ||
            typeName === 'string'
        ) {
            if (value === "") {
                result[key] = null;
            } else {
                const num = Number(value);
                result[key] = isNaN(num) ? value : num;
            }
        }
        else if (typeName === 'boolean') {
            if (value === "true" || value === "on" || value === true) {
                result[key] = true;
            } else {
                result[key] = false;
            }
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
function unwrapZodType(schema: any): any {
    if (!schema || !schema._def) {
        return schema;
    }

    const typeName = schema._def.typeName;

    // Unwrap wrappers comunes
    if (
        typeName === 'ZodOptional' ||
        typeName === 'ZodNullable' ||
        typeName === 'ZodEffects' ||
        typeName === 'ZodDefault'
    ) {
        // Recursión para múltiples capas de wrappers
        return unwrapZodType(schema._def.innerType || schema._def.schema);
    }

    return schema;
}

// Versión alternativa si prefieres usar instanceof (requiere importaciones correctas)
export function castFormDataBySchemaAlt(formData: any, schema: any) {
    const raw = Object.fromEntries(formData.entries());
    const result: any = {};

    for (const [key, value] of Object.entries(raw)) {
        const fieldSchema = schema.shape?.[key];

        if (!fieldSchema) {
            result[key] = value === "" ? null : value;
            continue;
        }

        const baseType = unwrapZodType(fieldSchema);

        // Verificación segura con try-catch
        try {
            if (baseType instanceof z.ZodNumber) {
                result[key] = value === "" ? null : Number(value);
            }
            else if (baseType instanceof z.ZodBoolean) {
                result[key] = value === "true" || value === "on" || value === true;
            }
            else {
                result[key] = value === "" ? null : value;
            }
        } catch (e) {
            // Fallback si instanceof falla
            console.warn(`Error verificando tipo para ${key}, usando valor raw:`, e);
            result[key] = value === "" ? null : value;
        }
    }

    return result;
}