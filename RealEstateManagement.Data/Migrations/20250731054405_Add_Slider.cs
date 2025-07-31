using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Slider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22bc3807-3615-46ac-8750-ff7438afd01a", new DateTime(2025, 7, 31, 12, 44, 1, 472, DateTimeKind.Local).AddTicks(3226), "AQAAAAIAAYagAAAAEHcZZ+ee6zm2nOaujRPoEbkaJN1DmVTjKVaJAJt5jJnWtbOkahYLRHoJq6Y53LeG0A==", "beec5080-eaef-4e31-b6f1-0f9354333e58" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3726d17d-8ff4-4b6d-b6da-a8ceb926c14a", new DateTime(2025, 7, 31, 12, 44, 1, 548, DateTimeKind.Local).AddTicks(682), "AQAAAAIAAYagAAAAELjiZkg/oXDSfcvAoB6Y2iq8R4SEu6856BrQiQ+PvjUjRlYbjr0BvXpDe8GZv3wd7g==", "38aab6ff-00bb-4642-b67d-49e3cd530635" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "18369739-0a3f-47fa-a09d-a32cf4500f36", new DateTime(2025, 7, 31, 12, 44, 1, 624, DateTimeKind.Local).AddTicks(679), "AQAAAAIAAYagAAAAEAfJjO7o5JIyXqguBEYeF1/46M9E9i2Yz5XwQGG3VXjz85y4prYe3Vgfw6Jbt4vcjw==", "5a5f0b35-4c0b-4479-8398-c211a3c5c9df" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4268ed90-3002-437e-ab94-300e0b68cfe3", new DateTime(2025, 7, 30, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(8514), "AQAAAAIAAYagAAAAEGNsjJwr9HrJqi2hXbWOPt2j0Al7K5WXNZBHbYbVjVuANkIRQDSHf3VN0NOdaHBrtg==", "b45efd73-a861-4101-bbf6-a23452fec018" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9396));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9402));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9405));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9544), new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9545) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9547), new DateTime(2025, 7, 30, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9548) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9550), new DateTime(2025, 7, 29, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9550) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 30, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9768), new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9767) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 29, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9770), new DateTime(2025, 7, 30, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9770) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 27, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9772), new DateTime(2025, 7, 29, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9772) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9821), new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9820) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9830));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9139));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9142));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9146));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 5, 44, 1, 697, DateTimeKind.Utc).AddTicks(9668));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 5, 44, 1, 697, DateTimeKind.Utc).AddTicks(9583));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9738));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 12, 44, 1, 697, DateTimeKind.Local).AddTicks(9742));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0e852059-b099-430b-83d5-245c4eec8e18", new DateTime(2025, 7, 30, 12, 10, 20, 131, DateTimeKind.Local).AddTicks(3534), "AQAAAAIAAYagAAAAEDdGoYqvSxAkDu3Dcp/jzi2KtaAklnAojoig60zbndR8ldDuevVoDPOF19Wyhdpykg==", "72aae217-d635-4f40-9de5-46cbe546dd0f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3a72f5de-8074-452d-8dc6-1937e54688cb", new DateTime(2025, 7, 30, 12, 10, 20, 194, DateTimeKind.Local).AddTicks(2519), "AQAAAAIAAYagAAAAEPb7ohZqGQsp/m2nK2+2zCMsmxp3gDgkvHFeeQ927zdVLaEVgaJb5xtLvrs0u2YlnQ==", "cea0528f-9d9b-46e4-b9f6-ed30cb647960" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "514fa9b4-2bca-4cff-9fa6-22c6985ac72c", new DateTime(2025, 7, 30, 12, 10, 20, 259, DateTimeKind.Local).AddTicks(2310), "AQAAAAIAAYagAAAAEErEkYBq+HXW7X8ycdj7L9fnBDRvgsUdq+sly3uuME0bK35g2k4ZDNM42cQLOvs/bA==", "5362b253-5640-48ea-a4ca-3edd23355b9d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "439bec1d-af6c-40a2-929f-5318cfcc6568", new DateTime(2025, 7, 29, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(1473), "AQAAAAIAAYagAAAAEB5YqbGwj4uF0b3jeIYlkvBvueY4GGCtLMOEMJ6r2IqL+XAhIShFbF2cP89+REMZdw==", "8f0fc1f8-a334-415b-ada9-6d38736003fc" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2093));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2097));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2099));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2222), new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2223) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2225), new DateTime(2025, 7, 29, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2226) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 28, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2227), new DateTime(2025, 7, 28, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2228) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 29, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2498), new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2498) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 28, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2500), new DateTime(2025, 7, 29, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2500) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 26, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2502), new DateTime(2025, 7, 28, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2501) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2530), new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2530) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2542));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2355));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(1914));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(1917));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(1920));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 5, 10, 20, 321, DateTimeKind.Utc).AddTicks(2327));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 5, 10, 20, 321, DateTimeKind.Utc).AddTicks(2261));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2460));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 12, 10, 20, 321, DateTimeKind.Local).AddTicks(2465));
        }
    }
}
