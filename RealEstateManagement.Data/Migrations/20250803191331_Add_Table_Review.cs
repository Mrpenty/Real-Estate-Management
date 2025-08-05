using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_Review : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "ReviewReplies",
    columns: table => new
    {
        Id = table.Column<int>(nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        ReviewId = table.Column<int>(nullable: false),
        LandlordId = table.Column<int>(nullable: false),
        ReplyContent = table.Column<string>(maxLength: 2000, nullable: false),
        CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
        IsFlagged = table.Column<bool>(nullable: false, defaultValue: false),
        IsVisible = table.Column<bool>(nullable: false, defaultValue: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_ReviewReplies", x => x.Id);
        table.ForeignKey(
            name: "FK_ReviewReplies_Reviews_ReviewId",
            column: x => x.ReviewId,
            principalTable: "Reviews",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_ReviewReplies_AspNetUsers_LandlordId",
            column: x => x.LandlordId,
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReplies_ReviewId",
                table: "ReviewReplies",
                column: "ReviewId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReplies_LandlordId",
                table: "ReviewReplies",
                column: "LandlordId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ddbbc7f7-641c-4f33-8e04-e832e9e44a8a", new DateTime(2025, 8, 4, 2, 13, 30, 424, DateTimeKind.Local).AddTicks(2014), "AQAAAAIAAYagAAAAECT7lKFULe1WOF1YhXzmE8wEqOtKdXNs7C0sTeYr+4eJybATGHQhWpXTJux+gKQJfA==", "8cd0c304-8cd4-49ab-8967-f8f6f0826db5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7ee97443-ebc1-4024-95a1-3d66f93161dc", new DateTime(2025, 8, 4, 2, 13, 30, 480, DateTimeKind.Local).AddTicks(8610), "AQAAAAIAAYagAAAAEOc1odHDKAvfO3XQVLAWC9InU82PCMrdllaKJZ/PNF7SxJ5ViRnzus8OvLtrVaK3iA==", "24dc71f7-9b24-4e45-9c8b-af88db44cac6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af6392f1-86d0-4121-a5ec-0bbb77b6d544", new DateTime(2025, 8, 4, 2, 13, 30, 544, DateTimeKind.Local).AddTicks(2231), "AQAAAAIAAYagAAAAEC40W6qyMPysD6JkgjObVkGIIwo6rV88jkNJWS6IFo7yElwrfevSDn1/yphz3Db4pA==", "c5a47129-8e1e-4e59-abea-9431f015ebbf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e498106f-90d6-419e-940c-7daaf3f99e67", new DateTime(2025, 8, 3, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(2780), "AQAAAAIAAYagAAAAENAnq063KE+kffkAW2kUQXPA6qEADYfVw3azwncWuNGb+F6Sdwwi3wN3jRqxAZqORA==", "a9bf9a5f-5c40-4126-87f1-32e8f682da48" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 3, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 3, 14, 13, 30, 609, DateTimeKind.Local).AddTicks(5055), new DateTime(2025, 8, 3, 16, 13, 30, 609, DateTimeKind.Local).AddTicks(5056) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 3, 21, 13, 30, 609, DateTimeKind.Local).AddTicks(5058), new DateTime(2025, 8, 3, 23, 13, 30, 609, DateTimeKind.Local).AddTicks(5059) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(3992));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(3996));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(3999));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4181), new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4182) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 3, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4184), new DateTime(2025, 8, 3, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4185) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4187), new DateTime(2025, 8, 2, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4187) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 3, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4384), new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4384) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 2, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4387), new DateTime(2025, 8, 3, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4386) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 31, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4388), new DateTime(2025, 8, 2, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4388) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4429), new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4429) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4437));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(3618));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(3623));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(3625));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 19, 13, 30, 609, DateTimeKind.Utc).AddTicks(4228));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4338));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4340));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 13, 30, 609, DateTimeKind.Local).AddTicks(4342));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4e616e89-c215-4cda-adc6-e07ff94c6394", new DateTime(2025, 8, 4, 2, 5, 16, 20, DateTimeKind.Local).AddTicks(8146), "AQAAAAIAAYagAAAAEDip7UfApCX3tvjA9XSaG8lFnEZl9MZAVP1XXJNSDIiuReTXcl6VLkRRhSp9iCUztw==", "162c38e4-dfa3-4f7b-9ab7-5e9b8f796a8c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a4689b36-651d-476a-a706-c0a5a48b2f96", new DateTime(2025, 8, 4, 2, 5, 16, 76, DateTimeKind.Local).AddTicks(5997), "AQAAAAIAAYagAAAAEO+bkLriRIXMdJrrD10wDK6J3gP2lIfLBWFrzYjySSDPNXN7D+uv/qXawtoJNiFDkg==", "74d4bc64-c89f-4072-92a6-f43535d031ef" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1b2d63c3-e3cc-4ca4-930c-330019f66f46", new DateTime(2025, 8, 4, 2, 5, 16, 130, DateTimeKind.Local).AddTicks(2392), "AQAAAAIAAYagAAAAEL20Gf25/SQTbv08r9ltuzwOtIht6vKPEuA33tAnQ1+NCXWuShygIUuaxQIIM7TcZg==", "6327795e-9e79-4f35-91e2-1fd34cab5c05" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b6b339ed-e4e8-48ce-bbba-9645347299cd", new DateTime(2025, 8, 3, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(8498), "AQAAAAIAAYagAAAAEOKEMo+U25ec0Crft1eawdUoxno8/pwL3gKxCvyZ+aguOXzM3DyMbqBxPF9wjTBhtg==", "ea45279c-dc7d-44a0-b6ca-970addf63e40" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 3, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(594));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 3, 14, 5, 16, 186, DateTimeKind.Local).AddTicks(609), new DateTime(2025, 8, 3, 16, 5, 16, 186, DateTimeKind.Local).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 3, 21, 5, 16, 186, DateTimeKind.Local).AddTicks(653), new DateTime(2025, 8, 3, 23, 5, 16, 186, DateTimeKind.Local).AddTicks(654) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9664));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9667));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9670));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 4, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9847), new DateTime(2025, 8, 4, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9847) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 3, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9849), new DateTime(2025, 8, 3, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9851) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9852), new DateTime(2025, 8, 2, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9853) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 3, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(155), new DateTime(2025, 8, 4, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(155) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 2, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(158), new DateTime(2025, 8, 3, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(157) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 31, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(159), new DateTime(2025, 8, 2, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(159) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 4, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(200), new DateTime(2025, 8, 4, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(199) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(203));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9451));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9454));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 2, 5, 16, 185, DateTimeKind.Local).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 19, 5, 16, 185, DateTimeKind.Utc).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(116));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(118));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 2, 5, 16, 186, DateTimeKind.Local).AddTicks(120));
        }
    }
}
