using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
namespace RealEstateManagement.Business.Validators
{
    public static class CommonRulesExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeAbsoluteUrl<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule.Must(url =>
                   !string.IsNullOrWhiteSpace(url) &&
                   Uri.TryCreate(url, UriKind.Absolute, out var u) &&
                   (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps))
                .WithMessage("ImageUrl phải là URL tuyệt đối (http/https).");
        }

        public static IRuleBuilderOptions<T, int?> NonNegativeIfHasValue<T>(
            this IRuleBuilder<T, int?> rule)
        {
            return rule.Must(v => !v.HasValue || v.Value >= 0)
                .WithMessage("Order (nếu có) phải >= 0.");
        }
    }
}
