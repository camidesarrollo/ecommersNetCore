import dayjs from "dayjs";

// Plugins
import customParseFormat from "dayjs/plugin/customParseFormat";
import localizedFormat from "dayjs/plugin/localizedFormat";
import relativeTime from "dayjs/plugin/relativeTime";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";

// Locale
import "dayjs/locale/es";

// Extender dayjs
dayjs.extend(customParseFormat);
dayjs.extend(localizedFormat);
dayjs.extend(relativeTime);
dayjs.extend(utc);
dayjs.extend(timezone);

// Idioma por defecto
dayjs.locale("es");

// Exportar instancia configurada
export { dayjs };
