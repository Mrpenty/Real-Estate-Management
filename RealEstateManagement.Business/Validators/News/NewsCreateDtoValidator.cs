using FluentValidation;
using RealEstateManagement.Business.DTO.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Validators.News
{
    public class NewsCreateDtoValidator : AbstractValidator<NewsCreateDto>
    {
        public NewsCreateDtoValidator()
        {
            // Áp dụng mặc định cho Create
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

            // Nếu bạn có ý niệm publish ngay khi tạo (tuỳ use case), có thể thêm RuleSet "Publish"
            RuleSet("Publish", () =>
            {
                RuleFor(x => x.Summary)
                    .NotEmpty().WithMessage("Summary bắt buộc khi publish.")
                    .MaximumLength(500);
            });
        }
    }
}
