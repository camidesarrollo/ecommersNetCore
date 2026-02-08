var l=document.getElementById("loadingModal"),d=document.getElementById("spinnerContent"),s=document.getElementById("resultModal"),a=document.getElementById("resultIcon"),r=document.getElementById("resultTitle"),u=document.getElementById("resultMessage"),i=document.getElementById("resultButton"),v=!1,t=null,p={creating:{icon:"\u2728",iconClass:"icon-pulse",title:"Creando Registro",subtitle:"Por favor espere",colorTitle:"var(--color-mint-green)",colorSubtitle:"var(--color-dark-chocolate)",type:""},editing:{icon:"\u270F\uFE0F",iconClass:"icon-rotate",title:"Editando Datos",subtitle:"Actualizando informaci\xF3n",colorTitle:"var(--color-golden-yellow)",colorSubtitle:"var(--color-dark-chocolate)",type:""},deleting:{icon:"\u{1F5D1}\uFE0F",iconClass:"icon-pulse",title:"Eliminando",subtitle:"Eliminando registro",colorTitle:"var(--color-burgundy-red)",colorSubtitle:"var(--color-dark-chocolate)",type:"circle"},uploading:{icon:"\u{1F4E4}",iconClass:"icon-bounce",title:"Subiendo Archivo",subtitle:"Cargando",colorTitle:"var(--color-olive-green)",colorSubtitle:"var(--color-dark-chocolate)",type:"upload"},processing:{icon:"\u2699\uFE0F",iconClass:"icon-rotate",title:"Procesando Datos",subtitle:"Analizando informaci\xF3n",colorTitle:"var(--color-nut-brown)",colorSubtitle:"var(--color-dark-chocolate)",type:"bars"},saving:{icon:"\u{1F4BE}",iconClass:"icon-pulse",title:"Guardando Cambios",subtitle:"Almacenando datos",colorTitle:"var(--color-orange-warm)",colorSubtitle:"var(--color-dark-chocolate)",type:"dots"},loading:{icon:"",iconClass:"icon-bounce",title:"Cargando",subtitle:"Obteniendo informaci\xF3n",colorTitle:"var(--color-golden-yellow)",colorSubtitle:"var(--color-dark-chocolate)",type:"circle"}};function m(e){let o={"":"",circle:'<div class="spinner mx-auto mb-6"><div class="spinner-ring"></div></div>',bars:`<div class="spinner-bars">${"<div class='spinner-bar'></div>".repeat(5)}</div>`,dots:`<div class="spinner-dots">${"<div class='spinner-dot'></div>".repeat(3)}</div>`,upload:`
            <div class="spinner-upload">
                <div class="upload-box">
                    <div class="upload-arrow">\u2B06\uFE0F</div>
                </div>
            </div>
            <div class="progress-bar-container">
                <div class="progress-bar"></div>
            </div>
        `};return`
        <div class="action-icon ${e.iconClass}">${e.icon}</div>
        ${o[e.type]}
        <div class="spinner-title" style="color:${e.colorTitle}">
            ${e.title}
        </div>
        <div class="spinner-subtitle loading-dots" style="color:${e.colorSubtitle}">
            ${e.subtitle}<span>.</span><span>.</span><span>.</span>
        </div>
    `}function y(e="loading",o=0,n){if(!l||!d)return;v=!0,t!==null&&(clearTimeout(t),t=null);let g=p[e]??p.loading;d.innerHTML=m(g),l.classList.add("show"),document.body.style.overflow="hidden",o>0&&(t=window.setTimeout(()=>{c(),n?.()},o))}function c(){l&&(v=!1,l.classList.remove("show"),document.body.style.overflow="",t!==null&&(clearTimeout(t),t=null))}async function S(e,o){y(e);try{let n=await o;return c(),n}catch(n){throw c(),n}}function h(e,o,n){!s||!a||!r||!u||!i||(e==="success"?(a.textContent="\u2705",r.style.color="var(--color-mint-green)",i.className="result-button btn-success"):(a.textContent="\u274C",r.style.color="var(--color-burgundy-red)",i.className="result-button btn-error"),r.textContent=o,u.textContent=n,s.classList.add("show"),document.body.style.overflow="hidden")}function b(){s&&(s.classList.remove("show"),document.body.style.overflow="")}function T(){i&&i.addEventListener("click",()=>{b()})}function f(){document.addEventListener("keydown",e=>{if(e.key==="Escape"){if(s?.classList.contains("show")){b();return}l?.classList.contains("show")&&c()}})}function w(){T(),f()}export{b as hideResultModal,c as hideSpinner,w as initGlobalUI,h as showResultModal,y as showSpinner,S as showSpinnerAsync};
//# sourceMappingURL=spinner.js.map
