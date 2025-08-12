using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_PropertySeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e670c4f1-8140-4340-ad52-6722ef52a09f", new DateTime(2025, 8, 12, 14, 40, 20, 874, DateTimeKind.Local).AddTicks(78), "AQAAAAIAAYagAAAAEBBqqD/vIvECTp6XkEiM+U2oV1FQxNOFaYlTk0RSYEia1GLgawcgk3/iowXotso7cg==", "4ad20275-38aa-4849-955b-481112c0b018" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e732c05f-8c57-41ee-b25d-43c6aaccf053", new DateTime(2025, 8, 12, 14, 40, 20, 929, DateTimeKind.Local).AddTicks(6161), "AQAAAAIAAYagAAAAEHKcxutrE+8z1ELtso5hU6H/rZHooa1uXanUqgA/797U25m/0G0oYh0UQ1qP0tWBxg==", "406b57da-3d40-45c3-82c7-140c7b8da1d5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a744dd6-71f6-4e68-9403-9c7d05ca72fe", new DateTime(2025, 8, 12, 14, 40, 20, 985, DateTimeKind.Local).AddTicks(2756), "AQAAAAIAAYagAAAAEB0XESZ+vBcPlRPxDpUU6wqkGHXcGNdDph0WKrpXUhkm6QRXvY4V375fqcNkMZ3hRg==", "1a2e645d-af7b-439b-847e-7bd7f910761a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fd09a617-e333-405d-a4b7-74c7bc75a4ab", new DateTime(2025, 8, 11, 14, 40, 21, 40, DateTimeKind.Local).AddTicks(3390), "AQAAAAIAAYagAAAAEHAVr13XP2458WEUWjCwfidpxFMUAU9XQpJwTBpPsMdbeI4yXxzk3rbVds3JXw8hVA==", "1100623b-4066-4af7-9153-3b5be8d2ac99" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dd717dbc-1512-4d06-bbc6-d92f46aa8372", new DateTime(2025, 8, 12, 14, 40, 21, 94, DateTimeKind.Local).AddTicks(9694), "AQAAAAIAAYagAAAAECun+MH1B6jktqqexppukJx9bPm+RKnccea0XCmo6SVlZdEHFFvuEHl5BkLfwl0g4Q==", "a9bad81f-e3ec-407b-9f2c-83760cba1e8e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f9c28e54-763d-4895-8796-4a46890dde51", new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(357), "AQAAAAIAAYagAAAAEO6VGh11QrkzLSr+Jt4rhikos1adWJNti/oofNu23fGRSpyS7UmnVyZUK4c9BcUf0A==", "7d7a22d1-1e4b-47b7-bff1-2b882ae22021" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2652));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 12, 2, 40, 21, 150, DateTimeKind.Local).AddTicks(2677), new DateTime(2025, 8, 12, 4, 40, 21, 150, DateTimeKind.Local).AddTicks(2678) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 12, 9, 40, 21, 150, DateTimeKind.Local).AddTicks(2680), new DateTime(2025, 8, 12, 11, 40, 21, 150, DateTimeKind.Local).AddTicks(2681) });

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsPublished",
                value: false);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsPublished",
                value: false);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsPublished",
                value: false);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsPublished",
                value: false);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsPublished",
                value: false);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsPublished",
                value: false);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsPublished",
                value: false);

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1489));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1493));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1496));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1499), "apartment" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1501), "house" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1505), "apartment" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1508), "apartment" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1511));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1513), "room" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1516));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 2, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1519), "room" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1521));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1585));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1588));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 7, 29, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1592), "house" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 7, 28, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1595), "apartment" });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1797), new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1797) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1800), new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1801) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1803), new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1803) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1805), new DateTime(2025, 8, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1821) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1828), new DateTime(2025, 8, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1829) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1831), new DateTime(2025, 8, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1831) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1834), new DateTime(2025, 8, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1835) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1850), new DateTime(2025, 8, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1850) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1852), new DateTime(2025, 8, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 3, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1855), new DateTime(2025, 8, 3, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1855) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1857), new DateTime(2025, 8, 2, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1858) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 1, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1859), new DateTime(2025, 8, 1, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1860) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1862), new DateTime(2025, 7, 31, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1863) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1864), new DateTime(2025, 7, 30, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1865) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1870), new DateTime(2025, 7, 29, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1870) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 28, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1872), new DateTime(2025, 7, 28, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1873) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2094), new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2092) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2097), new DateTime(2025, 8, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2096) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2099), new DateTime(2025, 8, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2098) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2101), new DateTime(2025, 8, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2100) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2102), new DateTime(2025, 8, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2102) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2104), new DateTime(2025, 8, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2104) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2106), new DateTime(2025, 8, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2105) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2154), new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2154) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2161));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1184));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1187));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1199));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 7, 40, 21, 150, DateTimeKind.Utc).AddTicks(1920));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 7, 40, 21, 150, DateTimeKind.Utc).AddTicks(1922));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 7, 40, 21, 150, DateTimeKind.Utc).AddTicks(1923));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2046));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2048));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2050));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bac491d5-7817-4168-9a44-380b933b12b3", new DateTime(2025, 8, 8, 20, 40, 46, 324, DateTimeKind.Local).AddTicks(1949), "AQAAAAIAAYagAAAAEEwvLDVbH5rbD9xX7qJMDuAQ3DM7sJUjxQdflZ2aUFoikyhCOyic8cK5oJfvUDTQ5w==", "a0c30732-8dc7-47d0-b0e5-e14fc52fd55b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "99a4d35e-319d-487d-9678-2502bc798d15", new DateTime(2025, 8, 8, 20, 40, 46, 424, DateTimeKind.Local).AddTicks(1827), "AQAAAAIAAYagAAAAELlutL0IeapWSupd5c+VChOIqfZv0rI4o7yEwIocC1oZIvYerCKBU4hrgwbtfGrFyA==", "fdb26fba-93ba-4467-8265-cd72bf68c4fd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fdc48e71-5e78-439a-862e-a36d4497c576", new DateTime(2025, 8, 8, 20, 40, 46, 514, DateTimeKind.Local).AddTicks(6096), "AQAAAAIAAYagAAAAEL08/yDUF3ez4SG44HiqJdv5O1cwLGbTf22BUlT1V4p92gfDYOmmki57eQAdadUuRg==", "00fef329-1eb2-4171-a47a-196a325c1850" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c07b2aa3-74b7-4dcd-8cda-8e9c24b6aa03", new DateTime(2025, 8, 7, 20, 40, 46, 595, DateTimeKind.Local).AddTicks(3256), "AQAAAAIAAYagAAAAEPnrS7APxbLgKYQi1b8z33TQOAPn4D8E6ZE/3T9+k7gTXdji5Zv4Vun225KHYyN6BQ==", "d7142926-3978-49ca-ae34-8303c66e617d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d7db698b-6fe6-469e-8d11-9ad47bd8799f", new DateTime(2025, 8, 8, 20, 40, 46, 658, DateTimeKind.Local).AddTicks(9486), "AQAAAAIAAYagAAAAEFfEqEFuJRfmLzluOG1rJ57/HMtnYZT9kkWFKJeXnBpCTTG4c7y6jjzxTxtFuj4JgA==", "7103703f-763d-42dc-a81b-667e57a02468" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "411a0322-20fc-4e1c-a7c9-f9fff4a95cf2", new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5156), "AQAAAAIAAYagAAAAEOV9DK+sMBfdsRkH/t35/GgrilLaQUegOpjRYlyTgNLq9YKe2s/PWbManWi3hR7P1g==", "4c061a76-fd37-4a3d-95e9-88010d6689cf" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 8, 8, 40, 46, 727, DateTimeKind.Local).AddTicks(6942), new DateTime(2025, 8, 8, 10, 40, 46, 727, DateTimeKind.Local).AddTicks(6943) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 8, 15, 40, 46, 727, DateTimeKind.Local).AddTicks(6944), new DateTime(2025, 8, 8, 17, 40, 46, 727, DateTimeKind.Local).AddTicks(6945) });

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsPublished",
                value: true);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsPublished",
                value: true);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsPublished",
                value: true);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsPublished",
                value: true);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsPublished",
                value: true);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsPublished",
                value: true);

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsPublished",
                value: true);

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5903));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5907));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5909));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6015), "studio" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6017), "villa" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6020), "dormitory" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 8, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6022), "penthouse" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6024));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 7, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6027), "loft" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6030));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 7, 29, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6032), "studio" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6034));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 27, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6036));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 26, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6038));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 7, 25, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6041), "villa" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2025, 7, 24, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6043), "condo" });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6196), new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6196) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6198), new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6199) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6200), new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6201) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6202), new DateTime(2025, 8, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6208) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6213), new DateTime(2025, 8, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6213) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6215), new DateTime(2025, 8, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6215) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6217), new DateTime(2025, 8, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6217) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6223), new DateTime(2025, 8, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6223) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6225), new DateTime(2025, 7, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6225) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6226), new DateTime(2025, 7, 30, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6227) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6228), new DateTime(2025, 7, 29, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6229) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 28, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6230), new DateTime(2025, 7, 28, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6230) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 27, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6232), new DateTime(2025, 7, 27, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6232) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 26, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6234), new DateTime(2025, 7, 26, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6234) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 25, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6235), new DateTime(2025, 7, 25, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6236) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 24, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6237), new DateTime(2025, 7, 24, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6237) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6447), new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6446) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6449), new DateTime(2025, 8, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6449) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6451), new DateTime(2025, 8, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6450) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6453), new DateTime(2025, 8, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6452) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6454), new DateTime(2025, 8, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6454) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6456), new DateTime(2025, 8, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6455) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 30, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6457), new DateTime(2025, 7, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6457) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6499), new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6499) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6504));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5702));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5709));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5716));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 13, 40, 46, 727, DateTimeKind.Utc).AddTicks(6281));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 13, 40, 46, 727, DateTimeKind.Utc).AddTicks(6283));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 13, 40, 46, 727, DateTimeKind.Utc).AddTicks(6284));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6405));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6407));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6409));
        }
    }
}
