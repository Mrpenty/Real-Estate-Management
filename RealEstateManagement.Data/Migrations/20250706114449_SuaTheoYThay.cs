using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class SuaTheoYThay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentCycle",
                table: "RentalContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentDayOfMonth",
                table: "RentalContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9adecddf-da6b-48f3-9925-45e578ea10f8", new DateTime(2025, 7, 6, 18, 44, 47, 921, DateTimeKind.Local).AddTicks(5689), "AQAAAAIAAYagAAAAENCEGMnMhHohNZf0C1FtwSoLApsN5vJthN2UEY044DaF2vfQsl8hcmZF6+mJImb1ZQ==", "5b913c88-3adc-4b3a-9d6a-31550a520c43" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "340decd1-40a1-4b57-a142-96a927817983", new DateTime(2025, 7, 6, 18, 44, 47, 993, DateTimeKind.Local).AddTicks(4561), "AQAAAAIAAYagAAAAEEUfnN/WpIwmDD61ZNhVVrWv0tIjfePxNvuCna5T6uLna3We/9ZvLwi3v3BgMNilAQ==", "8631a873-01d7-4829-b909-cee4be80d217" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e98620a6-6973-44bf-8dc8-da794282baba", new DateTime(2025, 7, 6, 18, 44, 48, 64, DateTimeKind.Local).AddTicks(8773), "AQAAAAIAAYagAAAAECOH0kFncU+MrcfoxLyk42D1hTP5DzE1DO9VFEu75q2Z7ttJBBog4UpjiDe/++SO8A==", "673b71bb-7074-42ef-925a-51a15af78ed2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eb324841-4d73-4959-beb6-68df09529339", new DateTime(2025, 7, 5, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(1328), "AQAAAAIAAYagAAAAEGaU/tlMjKcDug5B7tfIEQYxf0aRDdl4AbRovHGapc2UPBgKslNBBWnA1OY7FqfnJg==", "c5c3b012-189b-4764-87b4-ab5ddf52139b" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2370));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2173));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 5, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2177));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2180));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2325), new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2326) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2332), new DateTime(2025, 7, 5, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2333) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 4, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2335), new DateTime(2025, 7, 4, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2336) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 5, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2661), new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2660) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 4, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2664), new DateTime(2025, 7, 5, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2663) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 2, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2666), new DateTime(2025, 7, 4, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2665) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt", "PaymentCycle", "PaymentDayOfMonth" },
                values: new object[] { new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2737), new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2736), 0, 5 });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PaymentCycle", "PaymentDayOfMonth" },
                values: new object[] { new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2776), 1, 10 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 5, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2584));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2546));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(1894));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(1897));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 5, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(1900));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2619));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2622));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 18, 44, 48, 136, DateTimeKind.Local).AddTicks(2624));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentCycle",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "PaymentDayOfMonth",
                table: "RentalContracts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9b73b4fe-53ae-4b3e-b8fe-4211055690e0", new DateTime(2025, 6, 29, 1, 42, 37, 461, DateTimeKind.Local).AddTicks(3527), "AQAAAAIAAYagAAAAEP2FJCp3IAdhU3fr/kmBxLgql3BpUbpGullNljroMk5eV3rGy6qjOcPylEozKroacw==", "13bc0f22-6c65-49fa-8c1b-865715054fb1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2342a298-6100-4d03-a3f7-d45655bb8c75", new DateTime(2025, 6, 29, 1, 42, 37, 520, DateTimeKind.Local).AddTicks(1480), "AQAAAAIAAYagAAAAEEvI/mNTohkDsYqZWoTpkxYSYbc34gQv9uoAOu3c1HYTJNuduFDPqBOUdj6eNp06Ug==", "9007d6d0-5bbe-4a59-ab89-f299e93a8387" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c9572e0e-06e6-4a61-96a0-37daf8d80d76", new DateTime(2025, 6, 29, 1, 42, 37, 575, DateTimeKind.Local).AddTicks(8035), "AQAAAAIAAYagAAAAEMnPKDyu10LZmRnsSLLLQCObOcDB7JaQK7XYvFNLBTXtXWSn+ouRLlUxmGc7Sj+8Zw==", "d2bd0f7b-0a6a-4098-9f8f-8ebba90f57cf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "69722805-0e72-4725-b9db-6476d9b2f129", new DateTime(2025, 6, 28, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(92), "AQAAAAIAAYagAAAAECwERbQ915k3wJ7dQx+w8U2BGeX6/YhnlnEMfSitXOaQumyhjwY0zbqIU0QhtRBOyw==", "76bebb91-76e4-4ea3-940b-9b781ea4f92f" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1310));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1069));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 28, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1072));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 27, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1075));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1255), new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1255) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 28, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1260), new DateTime(2025, 6, 28, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1261) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 27, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1263), new DateTime(2025, 6, 27, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1264) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1686), new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1685) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 28, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1689), new DateTime(2025, 6, 28, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1688) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 25, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1690), new DateTime(2025, 6, 27, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1690) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1750), new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1744) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1755));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 28, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1471));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1423));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(807));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 27, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(810));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 28, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(813));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1640));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1642));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 29, 1, 42, 37, 630, DateTimeKind.Local).AddTicks(1644));
        }
    }
}
