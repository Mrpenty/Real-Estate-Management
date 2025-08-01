using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Notification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CitizenIdImageUrl",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Audience = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserNotifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserNotifications", x => new { x.NotificationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserNotifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2025, 7, 30, 5, 10, 20, 321, DateTimeKind.Utc).AddTicks(2261), 2 });

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

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNotifications_UserId",
                table: "ApplicationUserNotifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserNotifications");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "CitizenIdImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CitizenIdImageUrl", "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "424909f1-2a0a-41d0-b0e4-516cef6d4133", new DateTime(2025, 7, 12, 22, 9, 23, 60, DateTimeKind.Local).AddTicks(3861), "AQAAAAIAAYagAAAAEABdLF+n0+Pbe5o27W+/gqpvDJOVnSMW2PxzydMX9xtMxg3WnfDhxiW6qEoSXaWfhA==", "db18075b-9ca9-4759-9877-5e231b589b80" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CitizenIdImageUrl", "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "https://example.com/cccd/landlord.jpg", "8096638a-fc01-4aa8-a475-92085d126e0a", new DateTime(2025, 7, 12, 22, 9, 23, 152, DateTimeKind.Local).AddTicks(4444), "AQAAAAIAAYagAAAAELDx2YpLBzRD4M9pl7TZv03G61yxar5pbF/LQH7loD1JP260Wrmfzsbd+Uyau0WpCQ==", "04b4cf68-3413-41e0-aa66-5e850de9fabd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CitizenIdImageUrl", "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "https://example.com/cccd/renter.jpg", "8253dc22-e9a4-4af6-8ca6-801d61d82948", new DateTime(2025, 7, 12, 22, 9, 23, 230, DateTimeKind.Local).AddTicks(8906), "AQAAAAIAAYagAAAAEL9OvBXKJ4ifB/n0FtIfXiBoT5UB2B5Tz/cyE+HFj3qr1RAON9/zYfj2MLW5jr6Dyw==", "03266972-131c-414c-b818-1badb2d2eeac" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CitizenIdImageUrl", "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "https://example.com/cccd/renter2.jpg", "3d59fbfc-e023-46da-b143-02b638a7f888", new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(8316), "AQAAAAIAAYagAAAAEIxbGWRQ0naQlm5A728dyTnVYdVE+rqEyrTSlccer7/pqj6GAPFwochqZ4dsVs2mMg==", "a094b9bf-74d6-4e39-88c0-baca4e9645f2" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9265));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9270));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9423), new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9424) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9427), new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9428) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9430), new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9431) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9792), new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9791) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9795), new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9794) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 8, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9797), new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9796) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9877), new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9876) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9924));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9602));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9041));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9044));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9051));

            migrationBuilder.UpdateData(
                table: "WalletTransactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 15, 9, 23, 324, DateTimeKind.Utc).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 9, 23, 324, DateTimeKind.Utc).AddTicks(9466), 1 });

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9754));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9756));
        }
    }
}
