using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class v01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "39bc1ff8-2048-4891-bdc4-7f742adb89fc", new DateTime(2025, 6, 18, 14, 1, 31, 783, DateTimeKind.Local).AddTicks(1633), "AQAAAAIAAYagAAAAEDI78yGzKXVLP4bYnx3wS1MC0m59eaB/OKQiGexlfhg27fAhyXzbZCN5ZJgm3CK4HQ==", "40971ea2-5f3d-43a9-bd25-03d2e209e672" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c4e42a8f-91ce-4de3-a43a-7004e1d75e46", new DateTime(2025, 6, 18, 14, 1, 31, 838, DateTimeKind.Local).AddTicks(5045), "AQAAAAIAAYagAAAAEF/6v02fwLCtxsfq2dTNFYCRnmDxwMENKCTRVJPMEFn6eHah60UL01qlfnwoDY/BHw==", "091ee72c-bb69-4984-ac2e-f149cb069c5c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1737c3ea-0e57-489c-b871-ea9402fac74c", new DateTime(2025, 6, 18, 14, 1, 31, 894, DateTimeKind.Local).AddTicks(1235), "AQAAAAIAAYagAAAAECHhpM0+Sv2gTWDjRtJs1VFlx89ZzwFK/9kQmR0LsUvWa8wzcRuZv0DBbRI8Fvm9rw==", "6454de3e-f01f-4418-adac-691749be7ce4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8dc50a30-ca12-4134-87f4-41b945c84e09", new DateTime(2025, 6, 17, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(5591), "AQAAAAIAAYagAAAAEFoJ39oWUMCBUED06L9BsG52kZDKMYy8Z44xSl8iLyQ1PU6rbztr4NUxCgAQHjd0eg==", "c4f9f587-431d-4ccd-bea1-5e2021abe839" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(7916));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(6589));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(6592));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(6602));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(7853), new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(7861) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 17, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(7866), new DateTime(2025, 6, 17, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(7870) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(7872), new DateTime(2025, 6, 16, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(7872) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8143), new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8142) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 17, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8145), new DateTime(2025, 6, 17, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8144) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 14, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8147), new DateTime(2025, 6, 16, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8146) });

            migrationBuilder.InsertData(
                table: "RentalContracts",
                columns: new[] { "Id", "ConfirmedAt", "ContactInfo", "ContractDurationMonths", "CreatedAt", "DepositAmount", "LandlordId", "MonthlyRent", "PaymentMethod", "PropertyPostId", "RenterId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8208), "renter@example.com | 03345678910", 12, new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8206), 2000000m, 2, 5000000m, "Bank Transfer", 1, 3, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, null, "renter2@example.com | 0322222222", 6, new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8213), 1500000m, 2, 2000000m, "Momo", 2, 4, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 }
                });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8015));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(7968));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8100));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8102));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8104));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "04b7fe73-2b53-46ed-9ac7-2ee9638bd19c", new DateTime(2025, 6, 18, 13, 57, 53, 877, DateTimeKind.Local).AddTicks(9110), "AQAAAAIAAYagAAAAEDpGYaaGIhxkUnBfwm/4/FSs0zYF8Wnj65sjdzoL3cXE7juLonwt5jPh9TM5uJNBHg==", "4b3c1c31-bfd3-4a94-af9d-4be50eba0baa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a67d8444-9c52-4167-9874-174cff2a9a9e", new DateTime(2025, 6, 18, 13, 57, 53, 935, DateTimeKind.Local).AddTicks(8100), "AQAAAAIAAYagAAAAEJlJyJcuGIbFJVx9TU2oAk36JVM2zRino7Qv99gKK6hREowe8DxqQZSBvKWtPrGJkA==", "51b165ac-32bc-4a5b-b2d7-48176912ad01" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "774830c8-4cec-41fc-a470-b5bca3e6bfe8", new DateTime(2025, 6, 18, 13, 57, 53, 990, DateTimeKind.Local).AddTicks(3884), "AQAAAAIAAYagAAAAEN0EU4xTbOMCVdZS7qXmeLt3N57DTyzL6HzuJP5UdTg24jy/pSfjj+3ZEa9EuGohOw==", "e1c8185d-6d9a-46a1-a73a-a8d754cc31ef" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7453f45-b2ce-4421-b994-c872addcd5ff", new DateTime(2025, 6, 17, 13, 57, 54, 45, DateTimeKind.Local).AddTicks(9327), "AQAAAAIAAYagAAAAEFFhJl5rgQ6T1o3zbfsOfJGye95SFySpr62NcU+lhB21UQ1YHPWC2TS1OlOlc0uA9g==", "66fc9d39-68dd-4bdb-a13d-4eb98bca202a" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(1009));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(656));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(675));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(681));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(936), new DateTime(2025, 6, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(937) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 17, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(944), new DateTime(2025, 6, 17, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(945) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(948), new DateTime(2025, 6, 16, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(949) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(5060), new DateTime(2025, 6, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(5059) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(5068), new DateTime(2025, 6, 17, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(5067) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 14, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(5069), new DateTime(2025, 6, 16, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(5069) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(1179));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(1099));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(4791));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(4809));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 57, 54, 46, DateTimeKind.Local).AddTicks(4811));
        }
    }
}
