using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace Ecommers.Infrastructure.Web.TagHelpers
{
    [HtmlTargetElement("swiper")]
    public class SwiperTagHelper : TagHelper
    {
        public int SlidesPerView { get; set; } = 1;
        public bool Loop { get; set; } = false;
        public bool Navigation { get; set; } = true;
        public bool Pagination { get; set; } = false;
        public int SpaceBetween { get; set; } = 16;
        public bool Autoplay { get; set; } = true;
        public int AutoplayDelay { get; set; } = 5000;
        public string? Breakpoints { get; set; }
        public string CssClass { get; set; } = "";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var swiperId = $"swiper-{Guid.NewGuid():N}";

            output.TagName = "div";
            output.Attributes.SetAttribute("id", swiperId);
            output.Attributes.SetAttribute("class", $"swiper {CssClass}".Trim());

            var content = new StringBuilder();

            // Wrapper
            content.AppendLine("<div class=\"swiper-wrapper\">");
            content.AppendLine((await output.GetChildContentAsync()).GetContent());
            content.AppendLine("</div>");

            // Navigation
            if (Navigation)
            {
                content.AppendLine($"<div class=\"swiper-button-prev swiper-button-prev-{swiperId}\"></div>");
                content.AppendLine($"<div class=\"swiper-button-next swiper-button-next-{swiperId}\"></div>");
            }

            // Pagination
            if (Pagination)
            {
                content.AppendLine($"<div class=\"swiper-pagination swiper-pagination-{swiperId}\"></div>");
            }

            // Script
            content.AppendLine($@"
<script>
(function () {{

    function onPageLoaded(cb) {{
        if (document.readyState === 'complete') {{
            cb();
        }} else {{
            window.addEventListener('load', cb, {{ once: true }});
        }}
    }}

    onPageLoaded(function () {{

        if (typeof window.Swiper === 'undefined') {{
            console.error('Swiper no está cargado');
            return;
        }}

        var el = document.getElementById('{swiperId}');
        if (!el) {{
            console.warn('Swiper no encontrado: {swiperId}');
            return;
        }}

        new window.Swiper(el, {{
            slidesPerView: {SlidesPerView},
            spaceBetween: {SpaceBetween},
            loop: {Loop.ToString().ToLower()},
            {(Navigation ? $@"
            navigation: {{
                nextEl: '.swiper-button-next-{swiperId}',
                prevEl: '.swiper-button-prev-{swiperId}'
            }}," : "")}
            {(Pagination ? $@"
            pagination: {{
                el: '.swiper-pagination-{swiperId}',
                clickable: true
            }}," : "")}
            {(Autoplay ? $@"
            autoplay: {{
                delay: {AutoplayDelay},
                disableOnInteraction: false
            }}," : "")}
            {(!string.IsNullOrWhiteSpace(Breakpoints) ? $"breakpoints: {Breakpoints}," : "")}
        }});
    }});

}})();
</script>");

            output.Content.SetHtmlContent(content.ToString());
        }
    }
}
