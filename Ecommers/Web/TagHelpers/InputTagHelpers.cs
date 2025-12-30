using Ecommers.Web.Models.Shared.TagHelpers.Ui;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ecommers.Web.TagHelpers
{
    [HtmlTargetElement("ui-input")]
    public class InputTagHelpers : TagHelper
    {
        public required UiInputModel UiInputModel { get; set; }

        private string _value = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            // ASP-FOR overrides
            string name = UiInputModel.AspFor?.Name ?? UiInputModel.Id;
            string id = UiInputModel.Id ?? name;
            _value = UiInputModel.AspFor?.Model?.ToString() ?? "";

            // Type (password toggle safe)
            string computedType = UiInputModel.Type == "password" ? "password" : UiInputModel.Type;

            string sizeClasses = UiInputModel.Size switch
            {
                "sm" => "py-2 text-sm",
                "lg" => "py-4 text-lg",
                _ => "py-3 text-base"
            };

            string variantClasses = UiInputModel.Variant switch
            {
                "filled" => "bg-beige border-transparent",
                "outlined" => "bg-transparent border-2",
                _ => "bg-white border-2"
            };

            string stateClasses =
                !string.IsNullOrWhiteSpace(UiInputModel.Error) ? "border-burgundy-red" :
                !string.IsNullOrWhiteSpace(UiInputModel.Success) ? "border-olive-green" :
                "border-gray-light";

            string paddingClass = "px-4";
            if (!string.IsNullOrEmpty(UiInputModel.PrefixIcon)) paddingClass = "pl-10 pr-4";
            if (!string.IsNullOrEmpty(UiInputModel.SuffixIcon) || (UiInputModel.Type == "password" && UiInputModel.ShowPasswordToggle))
                paddingClass = !string.IsNullOrEmpty(UiInputModel.PrefixIcon) ? "pl-10 pr-10" : "pl-4 pr-10";

            string fullClasses =
                $"w-full rounded-lg input-focus transition-all duration-300 text-dark-chocolate placeholder-gray-400 " +
                $"{paddingClass} {sizeClasses} {variantClasses} {stateClasses}";

            if (UiInputModel.Disabled)
                fullClasses += " opacity-60 cursor-not-allowed bg-gray-light";

            // ARIA
            string ariaInvalid = !string.IsNullOrWhiteSpace(UiInputModel.Error) ? "true" : "false";
            string describedBy = $"{id}-help {id}-error".Trim();

            bool hasPrefix = !string.IsNullOrEmpty(UiInputModel.PrefixIcon);
            bool hasSuffix = !string.IsNullOrEmpty(UiInputModel.SuffixIcon) || (UiInputModel.Type == "password" && UiInputModel.ShowPasswordToggle);

            string iconClass = "";
            if (hasPrefix) iconClass += " has-prefix";
            if (hasSuffix) iconClass += " has-suffix";

            output.Attributes.SetAttribute("class",
                $"input-wrapper{iconClass} {(string.IsNullOrEmpty(UiInputModel.Error) ? "" : "error")} {(string.IsNullOrEmpty(UiInputModel.Success) ? "" : "success")}");


            output.Content.AppendHtml($@"
<div class='relative'>

    {(string.IsNullOrEmpty(UiInputModel.PrefixIcon) ? "" : $@"
    <span class='input-icon-left'>
        <i class='{UiInputModel.PrefixIcon}'></i>
    </span>
    ")}

    <input id='{id}'
           name='{name}'
           type='{computedType}'
           value='{_value}'
           placeholder=' '
           class='{fullClasses}'
           aria-invalid='{ariaInvalid}'
           aria-describedby='{describedBy}'
           {(UiInputModel.Disabled ? "disabled" : "")}
           {(UiInputModel.ReadOnly ? "readonly" : "")}
           {(UiInputModel.Required ? "required" : "")}
           {(UiInputModel.MaxLength.HasValue ? $"maxlength='{UiInputModel.MaxLength}'" : "")} />

    {(string.IsNullOrEmpty(UiInputModel.Label) ? "" : $"<label for='{id}'>{UiInputModel.Label}</label>")}

    {(UiInputModel.Type == "password" && UiInputModel.ShowPasswordToggle ? $@"
    <span class='input-icon-right' onclick=""UiInput_TogglePassword('{id}')"">
        <i id='{id}-toggle' class='fas fa-eye'></i>
    </span>
    " : "")}

    {(string.IsNullOrEmpty(UiInputModel.SuffixIcon) ? "" : $@"
    <span class='input-icon-right'>
        <i class='{UiInputModel.SuffixIcon}'></i>
    </span>
    ")}

</div>

{(string.IsNullOrEmpty(UiInputModel.HelperText) ? "" :
$"<p id='{id}-help' class='ui-helper'>{UiInputModel.HelperText}</p>")}

{(string.IsNullOrEmpty(UiInputModel.Error) ? "" :
$"<p id='{id}-error' class='ui-error'><i class='fas fa-exclamation-circle'></i> {UiInputModel.Error}</p>")}

{(string.IsNullOrEmpty(UiInputModel.Success) ? "" :
$"<p class='ui-success'><i class='fas fa-check-circle'></i> {UiInputModel.Success}</p>")}

{(UiInputModel.MaxLength.HasValue && UiInputModel.ShowCounter ?
$"<p class='ui-counter'>0/{UiInputModel.MaxLength}</p>" : "")}
");
        }
    }
}
