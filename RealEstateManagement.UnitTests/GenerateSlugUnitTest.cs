using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System;

[TestClass]
public class GenerateSlug
{
    private static class SlugHelper
    {
        public static string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title)) return "";

            string slug = RemoveDiacritics(title).ToLowerInvariant();
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = Regex.Replace(slug, @"\-{2,}", "-");
            slug = slug.Trim('-');
            return slug;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }

    [TestMethod]
    public void GenerateSlug_WithVietnameseTitle_ReturnsCorrectSlug()
    {
        string title = "Bất động sản Việt Nam và những thách thức";
        string expectedSlug = "bat-dong-san-viet-nam-va-nhung-thach-thuc";
        string actualSlug = SlugHelper.GenerateSlug(title);
        Assert.AreEqual(expectedSlug, actualSlug);
    }

    [TestMethod]
    public void GenerateSlug_WithSpecialCharactersAndSpaces_ReturnsCorrectSlug()
    {
        string title = "   BĐS: Giá nhà tăng 10% !? ";
        string expectedSlug = "bds-gia-nha-tang-10";
        string actualSlug = SlugHelper.GenerateSlug(title);
        Assert.AreEqual(expectedSlug, actualSlug);
    }

    [TestMethod]
    public void GenerateSlug_WithMultipleHyphens_ReturnsCorrectSlug()
    {
        string title = "đầu---tư---bds";
        string expectedSlug = "dau-tu-bds";
        string actualSlug = SlugHelper.GenerateSlug(title);
        Assert.AreEqual(expectedSlug, actualSlug);
    }

    [TestMethod]
    public void GenerateSlug_WithLeadingAndTrailingHyphens_ReturnsCorrectSlug()
    {
        string title = "-bán nhà--giá tốt-";
        string expectedSlug = "ban-nha-gia-tot";
        string actualSlug = SlugHelper.GenerateSlug(title);
        Assert.AreEqual(expectedSlug, actualSlug);
    }

    [TestMethod]
    public void GenerateSlug_WithEmptyString_ReturnsEmptyString()
    {
        string title = "";
        string expectedSlug = "";
        string actualSlug = SlugHelper.GenerateSlug(title);
        Assert.AreEqual(expectedSlug, actualSlug);
    }

    [TestMethod]
    public void GenerateSlug_WithNullInput_ReturnsEmptyString()
    {
        string title = null;
        string expectedSlug = "";
        string actualSlug = SlugHelper.GenerateSlug(title);
        Assert.AreEqual(expectedSlug, actualSlug);
    }
}
