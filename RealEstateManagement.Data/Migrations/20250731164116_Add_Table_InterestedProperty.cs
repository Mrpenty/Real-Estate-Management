using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_InterestedProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterestedProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    RenterId = table.Column<int>(type: "int", nullable: false),
                    InterestedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RenterReplyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LandlordReplyAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestedProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestedProperty_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestedProperty_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c3e2412-45dc-49c4-b033-5d050b0f3558", new DateTime(2025, 7, 31, 23, 41, 15, 882, DateTimeKind.Local).AddTicks(1069), "AQAAAAIAAYagAAAAEP6fGjN1SGOgbwp3R8H5ovzQD9+AkL5LAe6mjFhjW+qy14ywl6ReIKLPQnmPmVpHDw==", "08192b6d-c0ba-4d5b-8ffd-a0535fa8605c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "902f1e38-572b-48b7-b1f1-8c0c12f9932e", new DateTime(2025, 7, 31, 23, 41, 15, 944, DateTimeKind.Local).AddTicks(7837), "AQAAAAIAAYagAAAAEGRmbrmumzvs/JA/J+nTnUUxgROxdYiqud/bBpLzy01SJrDWcw9eHCvv/aPryySAcA==", "733300b6-165f-4053-816a-62bef4e4bcbd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "01960ca2-e499-48f8-a0b5-8d1184041e51", new DateTime(2025, 7, 31, 23, 41, 15, 999, DateTimeKind.Local).AddTicks(2034), "AQAAAAIAAYagAAAAEFEg8GCgEySDOuyo3PhtbRYWQD9XK3/uzKZwaoBX0PfvfZfPu61aW9efd8IRRKk6jw==", "4ad142eb-a302-4138-9ad2-eb5aba442f75" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4394f86f-54a4-478f-b4c0-2791a2d2c5c1", new DateTime(2025, 7, 30, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(7265), "AQAAAAIAAYagAAAAEEsVXs18rHkALK/W8XD67yTf2JXvhuPukVDNznpi4kSXswbokhRjfdERWQbk1PYZew==", "162b40c6-a3b5-4b86-8425-55e2804413ec" });

            migrationBuilder.InsertData(
                table: "InterestedProperty",
                columns: new[] { "Id", "InterestedAt", "LandlordReplyAt", "PropertyId", "RenterId", "RenterReplyAt", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 30, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8820), null, 1, 3, null, 1 },
                    { 2, new DateTime(2025, 7, 31, 11, 41, 16, 54, DateTimeKind.Local).AddTicks(8873), null, 2, 4, new DateTime(2025, 7, 31, 13, 41, 16, 54, DateTimeKind.Local).AddTicks(8873), 2 },
                    { 3, new DateTime(2025, 7, 31, 18, 41, 16, 54, DateTimeKind.Local).AddTicks(8875), null, 3, 1, new DateTime(2025, 7, 31, 20, 41, 16, 54, DateTimeKind.Local).AddTicks(8876), 3 }
                });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(7995));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(7998));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8001));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8169), new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8170) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8171), new DateTime(2025, 7, 30, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8176) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8177), new DateTime(2025, 7, 29, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8178) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 30, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8437), new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8437) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 29, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8440), new DateTime(2025, 7, 30, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8439) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 27, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8441), new DateTime(2025, 7, 29, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8441) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8484), new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8484) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8488));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8357));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(7830));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(7833));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(7835));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 16, 41, 16, 54, DateTimeKind.Utc).AddTicks(8259));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 16, 41, 16, 54, DateTimeKind.Utc).AddTicks(8217));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8398));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8400));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 41, 16, 54, DateTimeKind.Local).AddTicks(8402));

            migrationBuilder.CreateIndex(
                name: "IX_InterestedProperty_PropertyId_RenterId",
                table: "InterestedProperty",
                columns: new[] { "PropertyId", "RenterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterestedProperty_RenterId",
                table: "InterestedProperty",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestedProperty_Status",
                table: "InterestedProperty",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestedProperty");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "df460e3b-88ec-46c4-b8ae-1e6dac389a55", new DateTime(2025, 7, 31, 23, 32, 56, 884, DateTimeKind.Local).AddTicks(9762), "AQAAAAIAAYagAAAAEFkY5qonB78ao/T2+bm4acd51hl67sEty0VO0DPwDlHqF+6/m6/7Vww+Hmuh3Ax50Q==", "f1521f52-5597-4e8c-a113-650356f7aa4f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dd876ca4-08a7-4f0b-a9da-705189d04985", new DateTime(2025, 7, 31, 23, 32, 56, 973, DateTimeKind.Local).AddTicks(2545), "AQAAAAIAAYagAAAAEJHwGj3ahZAw29FW2SIJf9TqoO66vnB/K+UfXcNRfiVWznyXHcn0M/WWEo3BJ6Nw0g==", "dff72edd-5365-492c-987b-d45f720792e6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b48472a8-fb13-47a0-8bfb-ef4b65fa506e", new DateTime(2025, 7, 31, 23, 32, 57, 57, DateTimeKind.Local).AddTicks(1641), "AQAAAAIAAYagAAAAEHcRmHi5uSeTBMBA5KYYOGHzCLTvLP3I8GTEsu982U6HEbkeTHWENiXP6vEHbrtmSQ==", "8acc6781-8fdd-4c15-8f99-71ba2de1493f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cc66e9dc-f71e-4945-afad-6d1800c0f314", new DateTime(2025, 7, 30, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(6271), "AQAAAAIAAYagAAAAEGR1wteio9WhxVB0U1v4temmGtIJvSDKuzNVDDO++cs2tEweBRfN+TKfV6Xr6BMRJw==", "71ed1bdc-1c3d-4df0-829f-4154a350c025" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7592));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7597));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7601));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7847), new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7848) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7856), new DateTime(2025, 7, 30, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7858) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7860), new DateTime(2025, 7, 29, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7861) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 30, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8253), new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8252) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 29, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8256), new DateTime(2025, 7, 30, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8255) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 27, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8258), new DateTime(2025, 7, 29, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8257) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8318), new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8311) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8323));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8148));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7311));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7315));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(7319));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 16, 32, 57, 151, DateTimeKind.Utc).AddTicks(8009));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 16, 32, 57, 151, DateTimeKind.Utc).AddTicks(7914));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8205));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8208));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 23, 32, 57, 151, DateTimeKind.Local).AddTicks(8210));
        }
    }
}
