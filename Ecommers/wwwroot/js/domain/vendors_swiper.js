// vendors_swiper.ts
// OPCIÓN 1: Usar el bundle completo (MÁS SIMPLE - RECOMENDADO)
import Swiper from "swiper/swiper-bundle.mjs";
// Exponer Swiper globalmente para uso en Razor/HTML
window.Swiper = Swiper;
// Con el bundle, todos los módulos YA están incluidos automáticamente
// No necesitas importar Navigation, Pagination, etc. por separado
export default Swiper;
// ============================================================
// OPCIÓN 2: Si prefieres importar módulos individuales
// (Descomenta esta sección y comenta la OPCIÓN 1 de arriba)
// ============================================================
/*
import Swiper from "swiper";
import Navigation from "swiper/modules/navigation.mjs";
import Pagination from "swiper/modules/pagination.mjs";
import Autoplay from "swiper/modules/autoplay.mjs";
import EffectFade from "swiper/modules/effect-fade.mjs";

// Exponer globalmente
(window as any).Swiper = Swiper;
(window as any).SwiperModules = {
    Navigation,
    Pagination,
    Autoplay,
    EffectFade
};

export default Swiper;
export { Navigation, Pagination, Autoplay, EffectFade };
*/ 
