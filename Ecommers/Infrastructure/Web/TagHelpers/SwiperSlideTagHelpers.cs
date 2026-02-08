using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ecommers.Infrastructure.Web.TagHelpers
{
    [HtmlTargetElement("swiper-slide", ParentTag = "swiper")]
    public class SwiperSlideTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "swiper-slide");
        }
    }
}
