using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class addLastMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastMessage",
                table: "Conversation",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSentAt",
                table: "Conversation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8efd2ec-2c86-437b-9bd8-64326d41bf01", new DateTime(2025, 7, 4, 1, 45, 57, 595, DateTimeKind.Local).AddTicks(5485), "AQAAAAIAAYagAAAAELFzEDWgpS5QAOjtzw1Byrclfs+a0a8HrhGaKbNJSHBQHMvPNM71OA4/wvqwbsqX5w==", "c554cd6a-552f-48e9-8bb8-4413c878eda8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "41e180c1-c449-45fc-9f3f-d5b076406fc0", new DateTime(2025, 7, 4, 1, 45, 57, 648, DateTimeKind.Local).AddTicks(7121), "AQAAAAIAAYagAAAAEHI3/5rHuJDxUMUkXlW8hPrJaep5qmfkqx3TTY8bZ/+BMmi1zpiEFfnWroBncOApZg==", "0e9dfa76-b9ff-474c-812d-b1e680c1555d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eedb4461-2dc6-4073-a5c4-f24c2327887d", new DateTime(2025, 7, 4, 1, 45, 57, 704, DateTimeKind.Local).AddTicks(5105), "AQAAAAIAAYagAAAAEBEy4/yfLE3owhf7Kiyx7C6isC5c8Olc2p/kmp8RjNQJqx5lkco+eecuNbeq3tk+mg==", "b10d3961-fd82-4a74-8ad9-4ed5c258ed45" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fac98c05-0950-420d-a861-723e655c78c6", new DateTime(2025, 7, 3, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(47), "AQAAAAIAAYagAAAAEJBwi/MnQCalT7RdNQHhVQVDoRANdOP9Qpfg9MT2eYCHPbIDW8XdIOKSsKoIXY1X5Q==", "d6871b65-2a55-46c3-9af3-762b417bf12a" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1652));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1404));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 3, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1407));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 2, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1413));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1588), new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1589) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 3, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1594), new DateTime(2025, 7, 3, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1595) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1597), new DateTime(2025, 7, 2, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1598) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 3, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(2059), new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(2059) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 2, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(2061), new DateTime(2025, 7, 3, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(2061) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 30, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(2063), new DateTime(2025, 7, 2, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(2063) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(2114), new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(2114) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(2133));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 3, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1815));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1750));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 2, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1170));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 3, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1173));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1863));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1869));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 1, 45, 57, 761, DateTimeKind.Local).AddTicks(1871));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessage",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "LastSentAt",
                table: "Conversation");

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
