var s=document.getElementById("loadingModal"),d=document.getElementById("spinnerContent"),l=document.getElementById("resultModal"),r=document.getElementById("resultIcon"),i=document.getElementById("resultTitle"),u=document.getElementById("resultMessage"),c=document.getElementById("resultButton"),v=!1,n=null,p={creating:{icon:"\u2728",iconClass:"icon-pulse",title:"Creando Registro",subtitle:"Por favor espere",colorTitle:"var(--color-mint-green)",colorSubtitle:"var(--color-dark-chocolate)",type:""},editing:{icon:"\u270F\uFE0F",iconClass:"icon-rotate",title:"Editando Datos",subtitle:"Actualizando informaci\xF3n",colorTitle:"var(--color-golden-yellow)",colorSubtitle:"var(--color-dark-chocolate)",type:""},deleting:{icon:"\u{1F5D1}\uFE0F",iconClass:"icon-pulse",title:"Eliminando",subtitle:"Eliminando registro",colorTitle:"var(--color-burgundy-red)",colorSubtitle:"var(--color-dark-chocolate)",type:"circle"},uploading:{icon:"\u{1F4E4}",iconClass:"icon-bounce",title:"Subiendo Archivo",subtitle:"Cargando",colorTitle:"var(--color-olive-green)",colorSubtitle:"var(--color-dark-chocolate)",type:"upload"},processing:{icon:"\u2699\uFE0F",iconClass:"icon-rotate",title:"Procesando Datos",subtitle:"Analizando informaci\xF3n",colorTitle:"var(--color-nut-brown)",colorSubtitle:"var(--color-dark-chocolate)",type:"bars"},saving:{icon:"\u{1F4BE}",iconClass:"icon-pulse",title:"Guardando Cambios",subtitle:"Almacenando datos",colorTitle:"var(--color-orange-warm)",colorSubtitle:"var(--color-dark-chocolate)",type:"dots"},loading:{icon:"",iconClass:"icon-bounce",title:"Cargando",subtitle:"Obteniendo informaci\xF3n",colorTitle:"var(--color-golden-yellow)",colorSubtitle:"var(--color-dark-chocolate)",type:"circle"}};function m(e){let o={"":"",circle:'<div class="spinner mx-auto mb-6"><div class="spinner-ring"></div></div>',bars:`<div class="spinner-bars">${"<div class='spinner-bar'></div>".repeat(5)}</div>`,dots:`<div class="spinner-dots">${"<div class='spinner-dot'></div>".repeat(3)}</div>`,upload:`
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
    `}function y(e="loading",o=0,t){if(!s||!d)return;v=!0,n!==null&&(clearTimeout(n),n=null);let g=p[e]??p.loading;d.innerHTML=m(g),s.classList.add("show"),document.body.style.overflow="hidden",o>0&&(n=window.setTimeout(()=>{a(),t?.()},o))}function a(){s&&(v=!1,s.classList.remove("show"),document.body.style.overflow="",n!==null&&(clearTimeout(n),n=null))}async function T(e,o){y(e);try{let t=await o;return a(),t}catch(t){throw a(),t}}function S(e,o,t){!l||!r||!i||!u||!c||(e==="success"?(r.textContent="\u2705",i.style.color="var(--color-mint-green)",c.className="result-button btn-success"):(r.textContent="\u274C",i.style.color="var(--color-burgundy-red)",c.className="result-button btn-error"),i.textContent=o,u.textContent=t,l.classList.add("show"),document.body.style.overflow="hidden")}function b(){l&&(l.classList.remove("show"),document.body.style.overflow="")}document.addEventListener("keydown",e=>{e.key==="Escape"&&l?.classList.contains("show")&&b()});export{b as hideResultModal,a as hideSpinner,S as showResultModal,y as showSpinner,T as showSpinnerAsync};
//# sourceMappingURL=spinner.js.map
