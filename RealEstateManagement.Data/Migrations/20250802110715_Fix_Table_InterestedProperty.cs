using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Table_InterestedProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LandlordConfirmed",
                table: "InterestedProperties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RenterConfirmed",
                table: "InterestedProperties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "354867ad-5ad0-4877-b8b2-fccbd97241ff", new DateTime(2025, 8, 2, 18, 7, 12, 607, DateTimeKind.Local).AddTicks(43), "AQAAAAIAAYagAAAAEGX2dcBYbr92uIhT8keMjtt037YrxLsQSTj3LbjtHh3bLeRLG9rbY6sG9CkZZdpg+A==", "4a2ca2c1-46da-4c4f-a461-3dcf491768ae" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "134ccd5a-f9f7-4253-997f-7ed209264795", new DateTime(2025, 8, 2, 18, 7, 12, 670, DateTimeKind.Local).AddTicks(2233), "AQAAAAIAAYagAAAAENGVIrff3XclHR05FKiKelUdXsYsxPhuvPAzUrEytV/dcuWo5YzOt8hkepJT9bfzIQ==", "6c051415-2fdb-4326-91ed-6143ce274d06" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4a28d93f-669b-4dc6-bc57-d133bf83a34f", new DateTime(2025, 8, 2, 18, 7, 12, 723, DateTimeKind.Local).AddTicks(2241), "AQAAAAIAAYagAAAAECMMJtpT6PPUog1s8MtrXUduT8Leh9XfCz5XHcNduphZYDNnjPSvf9SA/NKu9wfUQQ==", "8697c473-bc24-4da6-ba5b-8a7d2bef6e9d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b955d41-ee43-4176-ba3f-22399a342c4c", new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(6906), "AQAAAAIAAYagAAAAEKNMZht70u/LdYsI/3uVGT4FNABVd0PFBYbzLYElZ2/DAFGO9McVEVJdDNS7Hgl+KQ==", "fc6edccb-99f7-4a6e-a8cc-2c357f2de0df" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8853));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 2, 6, 7, 12, 775, DateTimeKind.Local).AddTicks(8858), new DateTime(2025, 8, 2, 8, 7, 12, 775, DateTimeKind.Local).AddTicks(8859) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 2, 13, 7, 12, 775, DateTimeKind.Local).AddTicks(8861), new DateTime(2025, 8, 2, 15, 7, 12, 775, DateTimeKind.Local).AddTicks(8861) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8034));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8038));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8040));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8181), new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8181) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8183), new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8185) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8186), new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8186) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8431), new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8431) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8436), new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8436) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 29, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8438), new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8437) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8482), new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8480) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8490));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8350));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(7732));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(7735));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(7737));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 11, 7, 12, 775, DateTimeKind.Utc).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8391));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8394));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8395));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandlordConfirmed",
                table: "InterestedProperties");

            migrationBuilder.DropColumn(
                name: "RenterConfirmed",
                table: "InterestedProperties");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c872d952-db7f-411d-8b43-461edc9c4859", new DateTime(2025, 8, 2, 16, 45, 31, 533, DateTimeKind.Local).AddTicks(7698), "AQAAAAIAAYagAAAAECo6nUVnyo4mSBDB7C/IpsdlAaRz6rVnpu5/4etgCRKmjiRseIerkCwFQ22SI5lt9A==", "5a4a0ccf-d296-404d-8678-ab7121ea2e28" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "24c43dd4-3c9a-4ddb-8480-3c27f95ab848", new DateTime(2025, 8, 2, 16, 45, 31, 589, DateTimeKind.Local).AddTicks(8115), "AQAAAAIAAYagAAAAEC5yWTrP9btmVirkH2+VuY4fXoLb1GQ3LCK74hwEH3npEviw+MiCGcv3c85PbV+icA==", "21bb77ce-81a3-449b-bc64-320c96e5b6f2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "19a34afe-5e12-4bfb-850d-2a957de97294", new DateTime(2025, 8, 2, 16, 45, 31, 645, DateTimeKind.Local).AddTicks(3246), "AQAAAAIAAYagAAAAEFJ2q5JrY0M2BIHjm8U0wOpX5hfzXvDM4AQ9L0c9stEMBwFPUO/csTG5BSFauCE/Sg==", "9ddfdeb8-d08a-498f-a318-8518d44552d6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fcd2e817-ccbc-4a66-9ea2-33f9d467434d", new DateTime(2025, 8, 1, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(7906), "AQAAAAIAAYagAAAAEJ5EYqpMF9E8BDTGIoMZax2F79PGzaEyepycWirI5vb97ttHVzdytwrt6V+MERbW3g==", "c20fc4a7-815e-4983-9e5d-068712e9f2a7" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 1, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 2, 4, 45, 31, 701, DateTimeKind.Local).AddTicks(9576), new DateTime(2025, 8, 2, 6, 45, 31, 701, DateTimeKind.Local).AddTicks(9577) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 2, 11, 45, 31, 701, DateTimeKind.Local).AddTicks(9604), new DateTime(2025, 8, 2, 13, 45, 31, 701, DateTimeKind.Local).AddTicks(9604) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8824));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8827));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8830));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8975), new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8976) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 1, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8978), new DateTime(2025, 8, 1, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8979) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8981), new DateTime(2025, 7, 31, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8981) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9238), new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9238) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 31, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9240), new DateTime(2025, 8, 1, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9240) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 29, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9242), new DateTime(2025, 7, 31, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9241) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9285), new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9284) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9289));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9167));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8662));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8665));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(8668));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 9, 45, 31, 701, DateTimeKind.Utc).AddTicks(9025));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9203));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9205));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 16, 45, 31, 701, DateTimeKind.Local).AddTicks(9206));
        }
    }
}
