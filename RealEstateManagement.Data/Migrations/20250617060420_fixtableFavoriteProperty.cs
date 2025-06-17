using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixtableFavoriteProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Properties_PropertyId",
                table: "UserPreferences");

            migrationBuilder.DropTable(
                name: "UserPreferenceFavoriteProperties");

            migrationBuilder.DropIndex(
                name: "IX_UserPreferences_PropertyId",
                table: "UserPreferences");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "UserPreferences");

            migrationBuilder.CreateTable(
                name: "UserFavoriteProperties",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteProperties", x => new { x.UserId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteProperties_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFavoriteProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3ce6b779-2d82-472c-b69c-149f045fca37", new DateTime(2025, 6, 17, 13, 4, 17, 724, DateTimeKind.Local).AddTicks(1126), "AQAAAAIAAYagAAAAEA8h+4H261ct1tK3cP4zpIeI/0AlUArnQNtLPsqwAYy4ro8mNs3HYfrdjextGz3D5Q==", "b67c99e2-2994-4070-98c0-4b43ae20eebc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78b101fe-c773-45c2-8fa5-b72e6cb1d15b", new DateTime(2025, 6, 17, 13, 4, 17, 776, DateTimeKind.Local).AddTicks(7971), "AQAAAAIAAYagAAAAEHwlqyym210n5iH8Wwd/WC6JwGQhbSRA7J7lzpURS8Vm9Gzx5/lLenaVQto43Pt2WQ==", "a6f158f0-d284-4b0f-80c8-72379fe4d389" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "99833363-45d2-49ee-9809-8457b21e192d", new DateTime(2025, 6, 17, 13, 4, 17, 827, DateTimeKind.Local).AddTicks(8593), "AQAAAAIAAYagAAAAEM3CbOdnmw0jzhgz5Xe7D8Cey3ALvVWy2q8EpLY5LIsVw4FS0qfTWjnwzRX6UdsBRg==", "32e7e40f-15a3-4573-9b32-594f8f40b6cb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c5dc6718-7257-49d0-9fb3-56df51506a9c", new DateTime(2025, 6, 16, 13, 4, 17, 878, DateTimeKind.Local).AddTicks(9018), "AQAAAAIAAYagAAAAEAQJRm/IT3dkN8gNDue+RF8mszRdccuHgfAsHt3U+0aDrIC5im72XHpAZFm7/T3NzQ==", "1900364b-2388-4485-9f71-fa88ff62d2dc" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 17, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(240));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 13, 4, 17, 878, DateTimeKind.Local).AddTicks(9941));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 13, 4, 17, 878, DateTimeKind.Local).AddTicks(9954));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 4, 17, 878, DateTimeKind.Local).AddTicks(9957));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 17, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(191), new DateTime(2025, 6, 17, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(192) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(199), new DateTime(2025, 6, 16, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(200) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 15, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(201), new DateTime(2025, 6, 15, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(202) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(469), new DateTime(2025, 6, 17, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(468) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(472), new DateTime(2025, 6, 16, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(471) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 13, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(473), new DateTime(2025, 6, 15, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(473) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(354));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(294));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(430));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(433));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 13, 4, 17, 879, DateTimeKind.Local).AddTicks(434));

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteProperties_PropertyId",
                table: "UserFavoriteProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteProperties_UserId_PropertyId",
                table: "UserFavoriteProperties",
                columns: new[] { "UserId", "PropertyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteProperties");

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "UserPreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserPreferenceFavoriteProperties",
                columns: table => new
                {
                    UserPreferenceId = table.Column<int>(type: "int", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferenceFavoriteProperties", x => new { x.UserPreferenceId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_UserPreferenceFavoriteProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPreferenceFavoriteProperties_UserPreferences_UserPreferenceId",
                        column: x => x.UserPreferenceId,
                        principalTable: "UserPreferences",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d675da4e-34df-443e-bce8-7406a66c7dc2", new DateTime(2025, 6, 15, 13, 50, 18, 347, DateTimeKind.Local).AddTicks(1280), "AQAAAAIAAYagAAAAEJ1QCD6MBlHa1VXlBLGGRdNOP9qFF5q6h8AkRE+T/rywX7CFFMMfFvWEUv/glgonfQ==", "13fbd167-14fb-471f-840c-0d027b3ad095" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c9a2782e-fdbb-465b-82ae-739f22d11105", new DateTime(2025, 6, 15, 13, 50, 18, 398, DateTimeKind.Local).AddTicks(2319), "AQAAAAIAAYagAAAAEM1eIz547E36YSCubBYITZh9ADmWMnGYxz82qzZuwMFP82qt1L97J4Nzg1blk/+r4Q==", "44c68afa-5a38-4d83-9bde-48b64a82ace5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22d7d0cb-f692-4c4d-96f4-4bce9e5a1e6f", new DateTime(2025, 6, 15, 13, 50, 18, 449, DateTimeKind.Local).AddTicks(333), "AQAAAAIAAYagAAAAEL3dyWwaX9uyoKb1Ai8QtzBG2FTx390kmn06YFoFzytockjdiJ6TOzzoyJb3n/o6ZQ==", "d71e645f-33d2-4d36-a337-9af1911b9436" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d18eb4e-4960-4bb6-a99a-f4b97b2b97d4", new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(2258), "AQAAAAIAAYagAAAAEKTMnYDULkPj12JUaYVf0ZQOkKbtfCPFwt1e9v0429Md33XVjdOoirtrSibbxx+Tjg==", "b5beeb8d-6cb6-45bb-9dd2-6a3f75e65abe" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3100));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(2914));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(2918));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(2922));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3045), new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3057) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3064), new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3064) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 13, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3066), new DateTime(2025, 6, 13, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3067) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3354), new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3353) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3357), new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3355) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 11, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3358), new DateTime(2025, 6, 13, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3358) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3165));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3130));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3324));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3326));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 13, 50, 18, 501, DateTimeKind.Local).AddTicks(3328));

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_PropertyId",
                table: "UserPreferences",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferenceFavoriteProperties_PropertyId",
                table: "UserPreferenceFavoriteProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferenceFavoriteProperties_UserPreferenceId_PropertyId",
                table: "UserPreferenceFavoriteProperties",
                columns: new[] { "UserPreferenceId", "PropertyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Properties_PropertyId",
                table: "UserPreferences",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
