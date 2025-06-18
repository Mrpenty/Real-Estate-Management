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
                values: new object[] { "753b597c-66dc-45e5-bf80-49410c50562c", new DateTime(2025, 6, 18, 14, 7, 7, 834, DateTimeKind.Local).AddTicks(5039), "AQAAAAIAAYagAAAAEO1Qynw8E8sgG/uD8rq51TqbRl4atT2LL8shMC8ivtbaRjvQyjFisuW2+2PucsEgrw==", "a91dfbc9-37f5-4c21-b05a-0aa3f214f56f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c1becf1f-d945-4b08-b9b7-f42038a334c8", new DateTime(2025, 6, 18, 14, 7, 7, 893, DateTimeKind.Local).AddTicks(5478), "AQAAAAIAAYagAAAAELG8asCbnLMm9netwMK3R6mzL3pnBc+kLxdE+Q2WIvvi0PfWiiaU9vKg8RN5H1M3NA==", "46d84339-931d-4dbe-b286-e68067521ae4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4fdc34d6-47c3-41f5-aea0-619315e10ded", new DateTime(2025, 6, 18, 14, 7, 7, 947, DateTimeKind.Local).AddTicks(9004), "AQAAAAIAAYagAAAAENXiV50UJIlheD4cnzkNbHg+sZeVFb7wcZxGSfYmGPfEL5D7eE5IxxPSsa/LKvZCoA==", "89190b54-47e2-42df-95a7-79d819ab88dc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "821e04b8-4ed8-4d79-8775-e45ea0edc404", new DateTime(2025, 6, 17, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(4064), "AQAAAAIAAYagAAAAECbU7BOZFHcYUUZJuSfA2EwMzfnlCNkxvKSeH7d0dWCd9lVJKpRDIGXTqtrWvw6+4Q==", "de672331-92d9-48da-8aae-da99f2aaadb2" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5378));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5047));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5056));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5060));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5326), new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5327) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 17, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5331), new DateTime(2025, 6, 17, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5332) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5334), new DateTime(2025, 6, 16, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5335) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5649), new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5648) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 17, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5651), new DateTime(2025, 6, 17, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5651) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 14, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5653), new DateTime(2025, 6, 16, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5652) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5703), new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5703) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5710));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5506));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5452));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5608));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5612));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 7, 8, 2, DateTimeKind.Local).AddTicks(5613));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8208), new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8206) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 14, 1, 31, 947, DateTimeKind.Local).AddTicks(8213));

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
    }
}
