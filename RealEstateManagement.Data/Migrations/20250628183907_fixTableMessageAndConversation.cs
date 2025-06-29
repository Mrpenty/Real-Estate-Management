using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixTableMessageAndConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "57eb1a59-0b82-4636-b5b6-574fd25b3209", new DateTime(2025, 6, 29, 1, 39, 4, 225, DateTimeKind.Local).AddTicks(6927), "AQAAAAIAAYagAAAAEA409+W/GrX1x03Wl5zUe13yYWgNl0IDEH/xORDEznWgEsMYUh3CO6a28fH5U8JLhA==", "+841234567891", "74f31f05-2d24-4b65-aa20-d7fc89a228e9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "1f3dc938-0883-4cdd-bd24-9fc62cd08db3", new DateTime(2025, 6, 29, 1, 39, 4, 284, DateTimeKind.Local).AddTicks(4153), "AQAAAAIAAYagAAAAEE0TnYLUw9Gn4UqKNy9exLwJE9pKH9dXJBaV5G7YB35Sa5DLPX8kA+blpt/sA3Du6Q==", "+842345678910", "aae1c041-469c-4832-8342-17af1a45c5e3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "4061ac2c-df67-4cf8-afc0-20671f15720b", new DateTime(2025, 6, 29, 1, 39, 4, 343, DateTimeKind.Local).AddTicks(8170), "AQAAAAIAAYagAAAAEGgi+fG8OPSk62cZmEktgiW441gwS3vR9Vte/aBjHyFIy518RHXNTrDRbU+NzW+jvg==", "+843345678910", "26653870-e3c9-40da-bca4-70477b458ab0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2cd4932c-b3f9-4be3-b237-cefc6bf9b4bd", new DateTime(2025, 6, 28, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(7022), "AQAAAAIAAYagAAAAEAVZ16tHf+uywE/FBFmEVdMhnbqsoQXWFC4UbvDFFcmjwOhKtCrjcoTQOUfI75VdlQ==", "43026ce6-594b-40e4-b203-28430935c859" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8614));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8225));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 28, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 27, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8232));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8451), new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8451) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 28, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8457), new DateTime(2025, 6, 28, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8458) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 27, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8460), new DateTime(2025, 6, 27, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8461) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8888), new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8888) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 28, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8892), new DateTime(2025, 6, 28, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8891) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 25, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8893), new DateTime(2025, 6, 27, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8893) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8948), new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8939) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8957));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 28, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8794));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8742));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(7915));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 27, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(7918));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 28, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(7921));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8841));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8843));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 39, 4, 401, DateTimeKind.Local).AddTicks(8845));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "8fa41f46-6e23-4ea5-b4bb-60eab5651d6d", new DateTime(2025, 6, 20, 13, 17, 21, 847, DateTimeKind.Local).AddTicks(2701), "AQAAAAIAAYagAAAAEDWquDAh8LprxXe74mQaxY7YKefT6rp3TbeSyR9S7g3EQd+nauzpwdYNsNHrb9ftPg==", "12345678910", "76b91521-6345-4214-845b-270726aec8a5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "a7b00fa3-b067-41d3-a803-b537c0675861", new DateTime(2025, 6, 20, 13, 17, 21, 908, DateTimeKind.Local).AddTicks(2922), "AQAAAAIAAYagAAAAEIgK4lkVFzdympA3DnYmNuxcyKeKmzLpc4WsbMHkKQE6j9nw84m0pfZF4NFQlVF2/g==", "02345678910", "fb16e249-555a-4f25-9370-154abeea76e8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "cc1aff9a-1191-446f-abb2-d3f3e13acd58", new DateTime(2025, 6, 20, 13, 17, 21, 969, DateTimeKind.Local).AddTicks(5107), "AQAAAAIAAYagAAAAEO9ACCCVBUmj588oDnlWp1hWPXvwBsitBx7v7K5FRTv9r2v4zd1/5dL0zKzNl7y2Fg==", "03345678910", "dadd51ea-fd8c-449f-b058-b19e0dfcfde6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79128b50-5400-4b8c-80d8-8de9404c4c3e", new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(6626), "AQAAAAIAAYagAAAAEIxUWH2eemh+914qPkvAk+hVng69Iu81B9KDhH4TvLbedb0W7Ubh+Tnefy9cenDSTA==", "2cce8fcd-5eda-4c9b-b3fb-7c042f1ea7ea" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7910));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7592));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7597));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7600));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7866), new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7867) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7872), new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7874) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7875), new DateTime(2025, 6, 18, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7876) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1305), new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1304) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 19, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1307), new DateTime(2025, 6, 19, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1307) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 16, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1309), new DateTime(2025, 6, 18, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1308) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1381), new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1356) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1385));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1229));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1145));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7379));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7382));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7386));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1268));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1271));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1273));
        }
    }
}
