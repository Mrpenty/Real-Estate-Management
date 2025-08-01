using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_Table_NewImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "NewsImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "NewsImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cf587f0b-c6a1-42cb-a7fc-4af1f054ad4c", new DateTime(2025, 7, 29, 22, 20, 10, 991, DateTimeKind.Local).AddTicks(2176), "AQAAAAIAAYagAAAAEGERgx8VAu0Geiadft2jd1DpA7XJ6JyoupYE0ry8Y9rDKRwxtJyX4rrJv7yOy7I8jA==", "5234b076-c1d7-43d3-b913-db5c631554da" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a3dcf956-26eb-4c9c-bd67-c9302f649871", new DateTime(2025, 7, 29, 22, 20, 11, 70, DateTimeKind.Local).AddTicks(3533), "AQAAAAIAAYagAAAAEMmyleX9YwCKXtdRjY6mDjMbvpY2axgLEAUFNGgkVhYwE+vmtY58W6bIU2K7of4SeQ==", "244be741-1572-41e4-9303-f849e541b586" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6b0f5faa-702e-420d-bc02-51c1a52f2615", new DateTime(2025, 7, 29, 22, 20, 11, 149, DateTimeKind.Local).AddTicks(1757), "AQAAAAIAAYagAAAAEJJaK+3j55JGEY9kkxGR/gP8wEdvjboooSpL1VTktB7bI3kyi3GL4mCLinzcSzFSVw==", "ae5ccb4d-01ab-4b29-88a2-f8d305470c93" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e958b18b-5f01-40e8-b933-38fdc4c593b8", new DateTime(2025, 7, 28, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(2789), "AQAAAAIAAYagAAAAEIEyzLbsWEc1V3lmrUsjEexZEAA7tLifdfvXED+Ob2cjnHSBSCnxx2FLc0bHo3JwRA==", "163c5f8b-693e-41e6-859b-0e6c7123b57b" });

            migrationBuilder.UpdateData(
                table: "NewsImages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsPrimary", "Order" },
                values: new object[] { false, 0 });

            migrationBuilder.UpdateData(
                table: "NewsImages",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsPrimary", "Order" },
                values: new object[] { false, 0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4221));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4226));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 27, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4230));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4616), new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4618) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 28, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4623), new DateTime(2025, 7, 28, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4625) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 27, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4628), new DateTime(2025, 7, 27, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4628) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 28, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(5014), new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(5012) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 27, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(5018), new DateTime(2025, 7, 28, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(5017) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 25, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(5021), new DateTime(2025, 7, 27, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(5020) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(5099), new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(5088) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(5105));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4873));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(3867));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 27, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(3872));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(3876));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 15, 20, 11, 235, DateTimeKind.Utc).AddTicks(4814));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 15, 20, 11, 235, DateTimeKind.Utc).AddTicks(4684));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4944));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4951));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 22, 20, 11, 235, DateTimeKind.Local).AddTicks(4953));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "NewsImages");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "NewsImages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "85f69b30-2c2f-4a3d-b433-85c9e120e422", new DateTime(2025, 7, 17, 0, 3, 10, 149, DateTimeKind.Local).AddTicks(5143), "AQAAAAIAAYagAAAAELe0uP4N0vDaCb9pvHwGWe6Kb3qYTI+TyWFA0+DO9m8YOiYb0Rv6cuDhDf3Pb+xY3g==", "c0790dc8-6932-4f99-a5d4-927ff735775d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f67d8439-d399-429e-9860-083651c4e05a", new DateTime(2025, 7, 17, 0, 3, 10, 219, DateTimeKind.Local).AddTicks(2177), "AQAAAAIAAYagAAAAELkBPLGTyievy7jAxrupX5oMuP8aUdwEWpQZwVMzh/4gUqSu1Dq8htxDfmZY++AnpA==", "f1706dcd-670c-42c2-b912-da06c63821a0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eb437f3a-0ae5-4600-903b-4ac4406194b5", new DateTime(2025, 7, 17, 0, 3, 10, 296, DateTimeKind.Local).AddTicks(1242), "AQAAAAIAAYagAAAAEFMj4DJqpJqBn/WTiP2z7xh40NFSiFo4kLpImVQiUgECV3fE3jJvJNnzlMuuyTPJuw==", "0b0b9977-66fd-4267-b94a-76ac81e214f3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c69a901b-c09d-4ce1-96f9-fac32b5b3e97", new DateTime(2025, 7, 16, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(2597), "AQAAAAIAAYagAAAAEHUOy86nFeouIP8aLcUPTdpIYir1WoQRqd+l6KgOzk7e/w0ECXxeYvJ8KnBR5FtuAw==", "4ca037d1-dcc7-4255-bf77-4c5aa7ce225a" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3688));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 16, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3691));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 15, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3695));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3906), new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3906) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3909), new DateTime(2025, 7, 16, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3910) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3912), new DateTime(2025, 7, 15, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3912) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 16, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4268), new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4267) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 15, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4270), new DateTime(2025, 7, 16, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4269) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 13, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4272), new DateTime(2025, 7, 15, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4271) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4325), new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4325) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4336));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 16, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4071));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3303));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 15, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3307));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 16, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(3309));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 16, 17, 3, 10, 365, DateTimeKind.Utc).AddTicks(4030));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 16, 17, 3, 10, 365, DateTimeKind.Utc).AddTicks(3957));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4114));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4116));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 17, 0, 3, 10, 365, DateTimeKind.Local).AddTicks(4119));
        }
    }
}
