using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVnPayFieldsToWalletTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "WalletTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseCode",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TxnRef",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VnPayTransactionNo",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f76babfa-c7a9-43aa-86b3-80d68ec5808a", new DateTime(2025, 7, 23, 21, 59, 21, 45, DateTimeKind.Local).AddTicks(4466), "AQAAAAIAAYagAAAAEOXuuIhXzacig4tw+k6VHSMQfzZCijNMH2E2esF1gLQPVDqI+crzB9TvFXqFBPT7yA==", "27be1e75-074f-4039-811a-cad4f625e41a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0a3faecd-b1c9-4a1f-8e4a-f39b2341cfc0", new DateTime(2025, 7, 23, 21, 59, 21, 102, DateTimeKind.Local).AddTicks(1698), "AQAAAAIAAYagAAAAEAWpPCR/yzQvrKrqobdxNisg8kI02HjjwWzyQDZr5XkLLGsWMLiMzyfnoisAQ/jCHA==", "2940ecef-3553-4e3f-8c37-3c78589968b4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aff16033-4ed7-40e1-83f0-37cfe005d53f", new DateTime(2025, 7, 23, 21, 59, 21, 159, DateTimeKind.Local).AddTicks(3709), "AQAAAAIAAYagAAAAEGXZLNIXmF6n3/+3SJTlgBPFoCMs74aL+9MtqVWz23rf5Z3s9gffnsqX7Cns8OyR8g==", "d239e440-fa9b-414c-ae5e-d9f27026ce52" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02173567-1920-4d60-aafe-6b53e53eb71c", new DateTime(2025, 7, 22, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(3955), "AQAAAAIAAYagAAAAEPlyCEFpfBI1s07Ew4BF5F2Rpc8SjLHtWXVmKC/0MU43tICkClcbgpMF0CbkRElnDw==", "e069b66e-93b1-4a9d-8015-97919653b6ce" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4774));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 22, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4778));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4781));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4912), new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4912) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 22, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4914), new DateTime(2025, 7, 22, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4915) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 21, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4917), new DateTime(2025, 7, 21, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4918) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 22, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5193), new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5192) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 21, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5195), new DateTime(2025, 7, 22, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5194) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 19, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5197), new DateTime(2025, 7, 21, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5196) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5232), new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5231) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5239));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 22, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5118));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4427));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4430));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 22, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(4432));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PaidAt", "ResponseCode", "Status", "TxnRef", "VnPayTransactionNo" },
                values: new object[] { new DateTime(2025, 7, 23, 14, 59, 21, 219, DateTimeKind.Utc).AddTicks(5023), null, null, "Success", "901f3b30-f59a-40b1-897b-981c18b7ec90", null });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2025, 7, 23, 14, 59, 21, 219, DateTimeKind.Utc).AddTicks(4949), 2 });

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5162));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5165));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 23, 21, 59, 21, 219, DateTimeKind.Local).AddTicks(5167));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "ResponseCode",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "TxnRef",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "VnPayTransactionNo",
                table: "WalletTransactions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "424909f1-2a0a-41d0-b0e4-516cef6d4133", new DateTime(2025, 7, 12, 22, 9, 23, 60, DateTimeKind.Local).AddTicks(3861), "AQAAAAIAAYagAAAAEABdLF+n0+Pbe5o27W+/gqpvDJOVnSMW2PxzydMX9xtMxg3WnfDhxiW6qEoSXaWfhA==", "db18075b-9ca9-4759-9877-5e231b589b80" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8096638a-fc01-4aa8-a475-92085d126e0a", new DateTime(2025, 7, 12, 22, 9, 23, 152, DateTimeKind.Local).AddTicks(4444), "AQAAAAIAAYagAAAAELDx2YpLBzRD4M9pl7TZv03G61yxar5pbF/LQH7loD1JP260Wrmfzsbd+Uyau0WpCQ==", "04b4cf68-3413-41e0-aa66-5e850de9fabd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8253dc22-e9a4-4af6-8ca6-801d61d82948", new DateTime(2025, 7, 12, 22, 9, 23, 230, DateTimeKind.Local).AddTicks(8906), "AQAAAAIAAYagAAAAEL9OvBXKJ4ifB/n0FtIfXiBoT5UB2B5Tz/cyE+HFj3qr1RAON9/zYfj2MLW5jr6Dyw==", "03266972-131c-414c-b818-1badb2d2eeac" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3d59fbfc-e023-46da-b143-02b638a7f888", new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(8316), "AQAAAAIAAYagAAAAEIxbGWRQ0naQlm5A728dyTnVYdVE+rqEyrTSlccer7/pqj6GAPFwochqZ4dsVs2mMg==", "a094b9bf-74d6-4e39-88c0-baca4e9645f2" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9265));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9270));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9423), new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9424) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9427), new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9428) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9430), new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9431) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9792), new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9791) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9795), new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9794) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 8, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9797), new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9796) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9877), new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9876) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9924));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9602));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9041));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9044));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9051));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 15, 9, 23, 324, DateTimeKind.Utc).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 9, 23, 324, DateTimeKind.Utc).AddTicks(9466), 1 });

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9754));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9756));
        }
    }
}
