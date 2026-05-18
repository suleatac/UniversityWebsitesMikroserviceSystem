using FluentValidation;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.CreateAuditLog
{
    public class CreateAuditLogCommandValidation : AbstractValidator<CreateAuditLogCommand>
    {
        public CreateAuditLogCommandValidation()
        {
            RuleFor(x => x.UserId)
              .MaximumLength(200)
                .When(x => !string.IsNullOrEmpty(x.EntityName));


            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MaximumLength(100).WithMessage("Kullanıcı adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Action)
                .NotEmpty().WithMessage("Eylem boş olamaz.")
                .MaximumLength(100).WithMessage("Eylem en fazla 100 karakter olabilir.");

            RuleFor(x => x.EntityName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.EntityName));

            RuleFor(x => x.EntityId)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.EntityId));

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.IpAddress)
                .NotEmpty().WithMessage("IP adresi boş olamaz.")
                .Matches(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$").WithMessage("Geçerli bir IP adresi giriniz.");
        }
    }
}

           