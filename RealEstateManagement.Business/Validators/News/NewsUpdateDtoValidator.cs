using FluentValidation;
using RealEstateManagement.Business.DTO.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Validators.News
{
    public class NewsUpdateDtoValidator : AbstractValidator<NewsUpdateDto>
    {
        public NewsUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id phải > 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title là bắt buộc.")
                .MaximumLength(200);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content là bắt buộc.");

            RuleFor(x => x.Summary)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Summary));

            RuleFor(x => x.AuthorName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.AuthorName));

            RuleFor(x => x.Source)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Source));

            // Khi cập nhật mà đánh dấu publish -> cần thêm ràng buộc
            When(x => x.IsPublished, () =>
            {
                RuleFor(x => x.Summary)
                    .NotEmpty().WithMessage("Summary là bắt buộc khi IsPublished = true.");
            });
        }
    }
}
