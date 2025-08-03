using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReportEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReportedByUserId = table.Column<int>(type: "int", nullable: false),
                    ReportedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResolvedByUserId = table.Column<int>(type: "int", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ReportedByUserId",
                        column: x => x.ReportedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ResolvedByUserId",
                        column: x => x.ResolvedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02e931ca-ccd8-40f9-88c4-a84516714836", new DateTime(2025, 7, 31, 0, 37, 11, 629, DateTimeKind.Local).AddTicks(7707), "AQAAAAIAAYagAAAAECRAhVnJ2MxY4340Ly+dUNnxGWzO7cyt/vAGGo4bRmXxk+3jD/NziG78y70LdEVaqg==", "e360421e-7eb2-4199-a3d1-7de28b924e97" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af72fbc9-d8f6-423d-ab02-9bf9464d35b3", new DateTime(2025, 7, 31, 0, 37, 11, 688, DateTimeKind.Local).AddTicks(8588), "AQAAAAIAAYagAAAAEKcL7cv6LOaSN2NPs+3Uz9VgsI1WF7TRmJx8RAKYjc/xTUWyqEB7zVdJsL/x/b8Dmg==", "048ddbb0-48d9-4ce7-aeab-fbe2841b8ce8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0e53ac4-2475-46ae-a0db-b1c9e34490cd", new DateTime(2025, 7, 31, 0, 37, 11, 746, DateTimeKind.Local).AddTicks(3799), "AQAAAAIAAYagAAAAEDZB6eJOtSIMcUDm9UZLRId5IkWgxchhQ2C3tV9r8zyDgt1OICOnvYU7kqarZIY/7w==", "ac0a3a77-9921-4e98-b3bd-2800140aa9ed" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "507f4841-e395-4b8f-a39d-d9072180b053", new DateTime(2025, 7, 30, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1091), "AQAAAAIAAYagAAAAEEgmvtxEmxufW5Mb+lG1JYaRQIMf6S284BiRi0sdCx0B/b7JHdmKRieotUoyYpIn7Q==", "cbd165db-fa8a-4d10-b9ea-6cc85863f7f3" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1753));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1757));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1761));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1867), new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1868) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1871), new DateTime(2025, 7, 30, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1872) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1873), new DateTime(2025, 7, 29, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1874) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 30, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2156), new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2155) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 29, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2159), new DateTime(2025, 7, 30, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2158) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2161), new DateTime(2025, 7, 29, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2160) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2201), new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2201) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2206));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2085));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1633));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1637));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(1640));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 17, 37, 11, 804, DateTimeKind.Utc).AddTicks(1902));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2126));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2128));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 0, 37, 11, 804, DateTimeKind.Local).AddTicks(2130));

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedByUserId",
                table: "Reports",
                column: "ReportedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ResolvedByUserId",
                table: "Reports",
                column: "ResolvedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

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
    }
}
