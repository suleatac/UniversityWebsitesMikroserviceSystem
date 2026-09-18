using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Microservice.Web.TagHelpers
{
    /// <summary>
    /// Personel resmi icin img etiketi ureten TagHelper.
    /// Resim adresi bos veya null ise varsayilan avatar resmini
    /// (~/template1/images/member/4.jpg) kullanir.
    /// </summary>
    /// <example>
    /// <code>
    /// <img person-src="@personel.ResimUrl" person-name="@adSoyad" loading="lazy" />
    /// </code>
    /// </example>
    [HtmlTargetElement("img", Attributes = PersonSrcAttributeName)]
    public class PersonAvatarTagHelper : TagHelper
    {
        private const string PersonSrcAttributeName = "person-src";
        private const string PersonNameAttributeName = "person-name";

        /// <summary>
        /// Personel resmi bulunmadiginda kullanilacak varsayilan avatar.
        /// </summary>
        public const string DefaultAvatarPath = "/template1/images/member/4.jpg";

        /// <summary>
        /// Personelin resim adresi. Bos ise varsayilan avatar kullanilir.
        /// </summary>
        [HtmlAttributeName(PersonSrcAttributeName)]
        public string? ResimUrl { get; set; }

        /// <summary>
        /// Alt metni (erisilebilirlik icin; genelde ad soyad).
        /// </summary>
        [HtmlAttributeName(PersonNameAttributeName)]
        public string? Alt { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;

            var isDefaultAvatar = string.IsNullOrWhiteSpace(ResimUrl);
            var src = isDefaultAvatar ? DefaultAvatarPath : ResimUrl!.Trim();

            // Uygulama bir alt yolda (PathBase) calisiyorsa yerel yollari ona gore duzelt.
            if (src.StartsWith('/') && !src.StartsWith("//"))
            {
                var pathBase = ViewContext.HttpContext.Request.PathBase;
                if (pathBase.HasValue)
                {
                    src = $"{pathBase}{src}";
                }
            }

            output.Attributes.SetAttribute("src", src);

            if (!string.IsNullOrWhiteSpace(Alt))
            {
                output.Attributes.SetAttribute("alt", Alt);
            }
            else if (!output.Attributes.ContainsName("alt"))
            {
                output.Attributes.SetAttribute("alt", string.Empty);
            }

            // Bos avatar durumunda gorselin oranini koruyup tasmasini engelle.
            if (string.IsNullOrWhiteSpace(ResimUrl))
            {
                var existingStyle = output.Attributes["style"]?.Value?.ToString();
                var avatarStyle = "object-fit:cover;object-position:center top;";
                output.Attributes.SetAttribute("style", string.IsNullOrWhiteSpace(existingStyle)
                    ? avatarStyle
                    : $"{existingStyle}{avatarStyle}");
            }
        }
    }
}
