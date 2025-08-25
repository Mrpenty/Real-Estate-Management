using FluentValidation;
using RealEstateManagement.Business.DTO.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Validators.News
{
    public class NewImageCreateDtoValidator : AbstractValidator<NewImageCreateDto>
    {
        public NewImageCreateDtoValidator()
        {
            RuleFor(x => x.NewId)
                .GreaterThan(0).WithMessage("NewId phải > 0.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("ImageUrl là bắt buộc.")
                .MustBeAbsoluteUrl();

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Order phải >= 0.");
            // IsPrimary: bool nên không cần rule riêng
        }
    }
}
