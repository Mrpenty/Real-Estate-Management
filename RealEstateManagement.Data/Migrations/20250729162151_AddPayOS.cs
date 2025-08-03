using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPayOS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TxnRef",
                table: "WalletTransactions");

            migrationBuilder.RenameColumn(
                name: "VnPayTransactionNo",
                table: "WalletTransactions",
                newName: "PayOSOrderCode");

            migrationBuilder.RenameColumn(
                name: "ResponseCode",
                table: "WalletTransactions",
                newName: "CheckoutUrl");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9660d52d-504d-45c3-b465-590222613e08", new DateTime(2025, 7, 29, 23, 21, 50, 29, DateTimeKind.Local).AddTicks(8957), "AQAAAAIAAYagAAAAEJ4XysGXhs6bBDWO81/4X/BiRgRTtNyhrQ3U7Cg3jQyyAWPOWWxd83vTLq2WjZPs2g==", "70da04ab-2ce5-43e3-abb5-02807553192f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "335bd891-e12b-473a-bd04-da7fcf9ec400", new DateTime(2025, 7, 29, 23, 21, 50, 86, DateTimeKind.Local).AddTicks(9391), "AQAAAAIAAYagAAAAEClvrmBdau+j7jHAmt40RGQvncSsp2jiV9MjxZh7N65GCugQqMWfG0VWzmE6/D36PQ==", "07c8037a-a829-4e37-be2f-2e62851d142e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "92a020cc-2da3-497e-b9b5-eef78192a1ac", new DateTime(2025, 7, 29, 23, 21, 50, 144, DateTimeKind.Local).AddTicks(123), "AQAAAAIAAYagAAAAEDNwdGH6vcgiQEz4wlweLvlOPbFbXcAp4pk0jrEWR7PLLduWpLKquGwoJB8ILn9X7A==", "0fef42f5-d172-49cb-a7f4-29c231f86423" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "579fb81e-7eca-43b2-82a1-9ba7b206c4cc", new DateTime(2025, 7, 28, 23, 21, 50, 202, DateTimeKind.Local).AddTicks(9695), "AQAAAAIAAYagAAAAEOqwiZAcKXpcCpYi2n/+TlUQX54JhDg4unGHGmAVa6RENaCgANV879lhFpy2c67gZA==", "8e70deb5-d38b-4a57-9e5c-adedf7f45ffa" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(603));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(607));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 27, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(610));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(743), new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(744) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 28, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(746), new DateTime(2025, 7, 28, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(746) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 27, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(748), new DateTime(2025, 7, 27, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(749) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 28, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1044), new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1043) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 27, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1046), new DateTime(2025, 7, 28, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1045) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 25, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1048), new DateTime(2025, 7, 27, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1047) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1084), new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1084) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1094));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(869));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(426));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 27, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(430));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(433));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CheckoutUrl", "CreatedAt", "PayOSOrderCode" },
                values: new object[] { "", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 16, 21, 50, 203, DateTimeKind.Utc).AddTicks(779));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1005));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1007));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 21, 50, 203, DateTimeKind.Local).AddTicks(1009));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayOSOrderCode",
                table: "WalletTransactions",
                newName: "VnPayTransactionNo");

            migrationBuilder.RenameColumn(
                name: "CheckoutUrl",
                table: "WalletTransactions",
                newName: "ResponseCode");

            migrationBuilder.AddColumn<string>(
                name: "TxnRef",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "CreatedAt", "ResponseCode", "TxnRef", "VnPayTransactionNo" },
                values: new object[] { new DateTime(2025, 7, 23, 14, 59, 21, 219, DateTimeKind.Utc).AddTicks(5023), null, "901f3b30-f59a-40b1-897b-981c18b7ec90", null });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 23, 14, 59, 21, 219, DateTimeKind.Utc).AddTicks(4949));

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
    }
}
