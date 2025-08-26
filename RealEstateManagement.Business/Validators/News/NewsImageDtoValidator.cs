using FluentValidation;
using RealEstateManagement.Business.DTO.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Validators.News
{
    public class NewsImageDtoValidator : AbstractValidator<NewsImageDto>
    {
        public NewsImageDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id phải > 0.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("ImageUrl là bắt buộc.")
                .MustBeAbsoluteUrl();

            RuleFor(x => x.Order)
                .NonNegativeIfHasValue();
        }
    }
}
