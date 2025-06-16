using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class v02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d675da4e-34df-443e-bce8-7406a66c7dc2", new DateTime(2025, 6, 15, 13, 50, 18, 347, DateTimeKind.Local).AddTicks(1280), "AQAAAAIAAYagAAAAEJ1QCD6MBlHa1VXlBLGGRdNOP9qFF5q6h8AkRE+T/rywX7CFFMMfFvWEUv/glgonfQ==", "13fbd167-14fb-471f-840c-0d027b3ad095" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "c9a2782e-fdbb-465b-82ae-739f22d11105", new DateTime(2025, 6, 15, 13, 50, 18, 398, DateTimeKind.Local).AddTicks(2319), "AQAAAAIAAYagAAAAEM1eIz547E36YSCubBYITZh9ADmWMnGYxz82qzZuwMFP82qt1L97J4Nzg1blk/+r4Q==", "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0", "44c68afa-5a38-4d83-9bde-48b64a82ace5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22d7d0cb-f692-4c4d-96f4-4bce9e5a1e6f", new DateTime(2025, 6, 15, 13, 50, 18, 449, DateTimeKind.Local).AddTicks(333), "AQAAAAIAAYagAAAAEL3dyWwaX9uyoKb1Ai8QtzBG2FTx390kmn06YFoFzytockjdiJ6TOzzoyJb3n/o6ZQ==", "d71e645f-33d2-4d36-a337-9af1911b9436" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d18eb4e-4960-4bb6-a99a-f4b97b2b97d4", new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(2258), "AQAAAAIAAYagAAAAEKTMnYDULkPj12JUaYVf0ZQOkKbtfCPFwt1e9v0429Md33XVjdOoirtrSibbxx+Tjg==", "b5beeb8d-6cb6-45bb-9dd2-6a3f75e65abe" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3100));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(2914));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(2918));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(2922));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3045), new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3057) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3064), new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3064) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 13, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3066), new DateTime(2025, 6, 13, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3067) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3354), new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3353) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3357), new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3355) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 11, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3358), new DateTime(2025, 6, 13, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3358) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3165));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3130));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3324));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3326));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3328));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "96ccffc9-2716-4440-a19c-85298454dc73", new DateTime(2025, 6, 13, 10, 6, 13, 961, DateTimeKind.Local).AddTicks(4239), "AQAAAAIAAYagAAAAEONQrF0kvqVp4buZb+Hkh10PnVZgsM72an+XxaNJSqStNdLVXWYjmvFg66LtaKspHw==", "6c4c8de1-b0bd-4b2b-a560-69c37835253a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "5d2b964c-6489-4beb-b7c8-d89aa20233db", new DateTime(2025, 6, 13, 10, 6, 14, 11, DateTimeKind.Local).AddTicks(7204), "AQAAAAIAAYagAAAAEFfDyCOTRJLuxNGNjKjeuxtAYUPk9nHLSMf384s5NWWDexG8LusadbFckYLjSOsrXw==", null, "fee3941e-3b1d-4287-8c8c-8af9a4fec95e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "70daf717-341f-4d8e-8721-fe848e706390", new DateTime(2025, 6, 13, 10, 6, 14, 62, DateTimeKind.Local).AddTicks(7734), "AQAAAAIAAYagAAAAEApVUUALDYsxNwdLmRhG24rghOqU2rKneNt4YjOJCvwBxkVZKRR5rWr0VYQbyIxz7w==", "27b122c7-d71a-4008-ab2c-9523e9ebe2c0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5bd0a85e-adba-4469-b015-c5318fc70dc2", new DateTime(2025, 6, 12, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(1320), "AQAAAAIAAYagAAAAELdz+S8cRMcYSUrL7Dpj9EYpw5f/g7PDkamByx8ICnAyhZ5xAnXQ8G+e72SzfuCiBA==", "2e99ea00-d71e-4d22-be76-9107805bc43a" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2212));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2215));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 11, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2225));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2413), new DateTime(2025, 6, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2414) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 12, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2418), new DateTime(2025, 6, 12, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2420) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2421), new DateTime(2025, 6, 11, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2422) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2696), new DateTime(2025, 6, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2695) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 12, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2698), new DateTime(2025, 6, 12, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2698) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 9, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2700), new DateTime(2025, 6, 11, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2700) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2571));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2523));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2660));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2662));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 10, 6, 14, 114, DateTimeKind.Local).AddTicks(2664));
        }
    }
}
