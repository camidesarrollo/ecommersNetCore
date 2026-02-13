// vendors_swiper.ts
// Importar todos los exports con nombre
import * as SwiperModule from 'swiper';
import type Swiper from 'swiper';

// El constructor podría estar en diferentes lugares
const SwiperConstructor = (SwiperModule as any).default || 
                         (SwiperModule as any).Swiper || 
                         SwiperModule;

console.log('Módulo Swiper completo:', SwiperModule);
console.log('Tipo de SwiperConstructor:', typeof SwiperConstructor);
console.log('Es función?', typeof SwiperConstructor === 'function');

// Forzar asignación global
(window as any).Swiper = SwiperConstructor;

export default SwiperConstructor;