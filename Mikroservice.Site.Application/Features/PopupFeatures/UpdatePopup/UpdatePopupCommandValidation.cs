using FluentValidation;

namespace Mikroservice.Site.Application.Features.PopupFeatures.UpdatePopup
{
    public class UpdatePopupCommandValidation : AbstractValidator<UpdatePopupCommand>
    {
        public UpdatePopupCommandValidation()
        {
            RuleFor(x => x.Baslik).NotEmpty();
            RuleFor(x => x.IcerikMetni).NotEmpty();
            RuleFor(x => x.YayimTarihi).NotEmpty();
        }
    }
}
