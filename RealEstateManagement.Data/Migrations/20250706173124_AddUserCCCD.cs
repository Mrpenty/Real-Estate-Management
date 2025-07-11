using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCCCD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CitizenIdBackImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CitizenIdExpiryDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitizenIdFrontImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitizenIdImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CitizenIdIssuedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitizenIdNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VerificationRejectReason",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CitizenIdBackImageUrl", "CitizenIdExpiryDate", "CitizenIdFrontImageUrl", "CitizenIdImageUrl", "CitizenIdIssuedDate", "CitizenIdNumber", "ConcurrencyStamp", "CreatedAt", "IsActive", "PasswordHash", "SecurityStamp", "VerificationRejectReason" },
                values: new object[] { "https://example.com/cccd/admin_back.jpg", new DateTime(2030, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/admin_front.jpg", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "001234567890", "5e4010ac-9f56-406e-99c2-bfb7685201d7", new DateTime(2025, 7, 7, 0, 31, 19, 505, DateTimeKind.Local).AddTicks(3667), true, "AQAAAAIAAYagAAAAEFu31BAjja/sfAKzOm63nBlc6UEk8sSsRZcIeZWbRbWOSz2KzqxhB2p7Vkgkl2oytA==", "b6ffaa68-0850-43e6-829a-4b6835d0285b", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CitizenIdBackImageUrl", "CitizenIdExpiryDate", "CitizenIdFrontImageUrl", "CitizenIdImageUrl", "CitizenIdIssuedDate", "CitizenIdNumber", "ConcurrencyStamp", "CreatedAt", "IsActive", "PasswordHash", "SecurityStamp", "VerificationRejectReason" },
                values: new object[] { "https://example.com/cccd/landlord_back.jpg", new DateTime(2031, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/landlord_front.jpg", "https://example.com/cccd/landlord.jpg", new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "002345678901", "12f26d9a-16c7-4cb6-a92f-81be1d908bfd", new DateTime(2025, 7, 7, 0, 31, 19, 629, DateTimeKind.Local).AddTicks(8891), true, "AQAAAAIAAYagAAAAEEPplvdOm3pjN07v/Ca5kravg8gsaekw14xma+quwXvDJgHFX24fHsQvVEfilcm7eA==", "eb55816f-6029-45de-86db-d46105eac557", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CitizenIdBackImageUrl", "CitizenIdExpiryDate", "CitizenIdFrontImageUrl", "CitizenIdImageUrl", "CitizenIdIssuedDate", "CitizenIdNumber", "ConcurrencyStamp", "CreatedAt", "IsActive", "PasswordHash", "SecurityStamp", "VerificationRejectReason" },
                values: new object[] { "https://example.com/cccd/renter_back.jpg", new DateTime(2032, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/renter_front.jpg", "https://example.com/cccd/renter.jpg", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "003456789012", "6c6760f4-ec66-4834-bf5a-3ea300052f9e", new DateTime(2025, 7, 7, 0, 31, 19, 764, DateTimeKind.Local).AddTicks(6267), true, "AQAAAAIAAYagAAAAEE6aENcUgwFRTP4Q6owLaUrIgHXKbpbiy324jdUQONuhgUt8EzxAzZ/BenqzNcd7sw==", "fcd967ef-d0ed-41fc-9113-c725c7c6d703", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CitizenIdBackImageUrl", "CitizenIdExpiryDate", "CitizenIdFrontImageUrl", "CitizenIdImageUrl", "CitizenIdIssuedDate", "CitizenIdNumber", "ConcurrencyStamp", "CreatedAt", "IsActive", "PasswordHash", "SecurityStamp", "VerificationRejectReason" },
                values: new object[] { "https://example.com/cccd/renter2_back.jpg", new DateTime(2033, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/renter2_front.jpg", "https://example.com/cccd/renter2.jpg", new DateTime(2023, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "004567890123", "bc4763a3-3265-47b0-b1cd-687ad839bde3", new DateTime(2025, 7, 6, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(5762), false, "AQAAAAIAAYagAAAAEJRTKODFOJvEMmPcJjg2ROxrEOdwWDXEIVU6HczqGznrsYv9RP+LFspviIuIK/XXzg==", "c25f59ea-7d79-4293-819e-e9dae3791fc7", "Ảnh CCCD mặt sau bị mờ, thiếu ngày cấp. Vui lòng bổ sung lại!" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7331));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(6958));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(6964));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 5, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(6969));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7278), new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7279) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7284), new DateTime(2025, 7, 6, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7285) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7287), new DateTime(2025, 7, 5, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7288) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 6, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7590), new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7589) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 5, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7594), new DateTime(2025, 7, 6, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7593) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 3, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7596), new DateTime(2025, 7, 5, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7595) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7657), new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7644) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7668));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7495));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7433));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(6655));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 5, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(6658));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(6662));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7539));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7545));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 7, 0, 31, 19, 882, DateTimeKind.Local).AddTicks(7547));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CitizenIdBackImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CitizenIdExpiryDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CitizenIdFrontImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CitizenIdImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CitizenIdIssuedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CitizenIdNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VerificationRejectReason",
                table: "AspNetUsers");

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
    }
}
