using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExtendContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ProposedAt",
                table: "RentalContracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProposedContractDurationMonths",
                table: "RentalContracts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProposedEndDate",
                table: "RentalContracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProposedMonthlyRent",
                table: "RentalContracts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProposedPaymentCycle",
                table: "RentalContracts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProposedPaymentDayOfMonth",
                table: "RentalContracts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RenterApproved",
                table: "RentalContracts",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d8e22723-b11a-4dd8-a139-8abff629f3a8", new DateTime(2025, 8, 18, 12, 5, 19, 369, DateTimeKind.Local).AddTicks(2142), "AQAAAAIAAYagAAAAEDwtjbqpSj8C9YwftP1iUC4pTMosiTu7PtNZUMflLP8x4JeOfwmfxkzLsYXGpsAtqA==", "6664ba90-1331-40d2-80f5-e4cc7b7533a5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "28a00642-8156-40c9-91a9-264ff7837413", new DateTime(2025, 8, 18, 12, 5, 19, 425, DateTimeKind.Local).AddTicks(3845), "AQAAAAIAAYagAAAAEPDrnqI8xok6VxAIioztYcEo/X2zxTIBg3AtiNRoa3TRD76Op/IuUsPNo4G6ZD6kog==", "f8057099-f25b-4a84-be13-fdc42fcd8583" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0c2f5b4-8be1-4096-8977-5d8546feca2c", new DateTime(2025, 8, 18, 12, 5, 19, 481, DateTimeKind.Local).AddTicks(3191), "AQAAAAIAAYagAAAAEC79xjlekroD+oots3LPUZ3P1hjV7376ow+GcPiqK0w1k1+SESLzLp3Tuzv3JXiyEA==", "ab2807c0-a145-4037-97fa-9d2e0a05fdd7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b4041fd6-0b4d-4afe-a9aa-fcea6ece81bc", new DateTime(2025, 8, 17, 12, 5, 19, 537, DateTimeKind.Local).AddTicks(6212), "AQAAAAIAAYagAAAAECv9Eb0IUKU/oKrsyHDLhJQuqJgOrNow5ynrxpY1XeC97wUisegtzgxczBMWnDIQdQ==", "efc7d6f8-8cef-4061-bdf4-2e943a150b35" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9c469b60-28f9-45ee-b939-2f888ae36dc9", new DateTime(2025, 8, 18, 12, 5, 19, 593, DateTimeKind.Local).AddTicks(5094), "AQAAAAIAAYagAAAAEA//srP3fgDIzxzUP2o0XciMLUn0Agh3m5MVluIeBt0ntd1POB1W1TTB8WjOgJC3Rg==", "e5a4dbc1-7758-46bb-b966-587d94225ba8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3ffdc6f4-dc91-49e0-a075-c270a7665da1", new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(3429), "AQAAAAIAAYagAAAAEDCNF/P+dOrZDvZwcPf2rVfJUCazp6oH67wl6MH3EXWw/fQ4TTVAUvKMQk9nSpvpaA==", "ba34eec3-5768-4a1d-b589-54eed4f009e2" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 17, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(7032));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 18, 0, 5, 19, 650, DateTimeKind.Local).AddTicks(7035), new DateTime(2025, 8, 18, 2, 5, 19, 650, DateTimeKind.Local).AddTicks(7036) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 18, 7, 5, 19, 650, DateTimeKind.Local).AddTicks(7038), new DateTime(2025, 8, 18, 9, 5, 19, 650, DateTimeKind.Local).AddTicks(7038) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4651));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4654));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 16, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4657));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4660));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4683));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 13, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4686));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4688));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 11, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4693));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 9, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4696));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4698));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4700));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4703));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 5, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4705));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4707));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4710));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6261), new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6267) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 17, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6270), new DateTime(2025, 8, 17, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6271) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 16, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6283), new DateTime(2025, 8, 16, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6293) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 15, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6295), new DateTime(2025, 8, 15, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6295) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 14, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6305), new DateTime(2025, 8, 14, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6306) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6307), new DateTime(2025, 8, 13, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6308) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 12, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6310), new DateTime(2025, 8, 12, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6310) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 11, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6317), new DateTime(2025, 8, 11, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6318) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6320), new DateTime(2025, 8, 10, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6320) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6322), new DateTime(2025, 8, 9, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6322) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6324), new DateTime(2025, 8, 8, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6325) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6326), new DateTime(2025, 8, 7, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6327) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 6, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6328), new DateTime(2025, 8, 6, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6329) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 5, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6330), new DateTime(2025, 8, 5, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6331) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 4, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6335), new DateTime(2025, 8, 4, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6335) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 3, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6337), new DateTime(2025, 8, 3, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6338) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 15, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6557), new DateTime(2025, 8, 16, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6555) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 14, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6558), new DateTime(2025, 8, 15, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6558) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 13, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6560), new DateTime(2025, 8, 14, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6560) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 12, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6562), new DateTime(2025, 8, 13, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6561) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 11, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6564), new DateTime(2025, 8, 12, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6563) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 10, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6565), new DateTime(2025, 8, 11, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6565) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 9, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6567), new DateTime(2025, 8, 10, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6566) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt", "ProposedAt", "ProposedContractDurationMonths", "ProposedEndDate", "ProposedMonthlyRent", "ProposedPaymentCycle", "ProposedPaymentDayOfMonth", "RenterApproved" },
                values: new object[] { new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6603), new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6602), null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ProposedAt", "ProposedContractDurationMonths", "ProposedEndDate", "ProposedMonthlyRent", "ProposedPaymentCycle", "ProposedPaymentDayOfMonth", "RenterApproved" },
                values: new object[] { new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6611), null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4433));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 16, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4436));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(4444));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 5, 5, 19, 650, DateTimeKind.Utc).AddTicks(6381));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 5, 5, 19, 650, DateTimeKind.Utc).AddTicks(6383));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 5, 5, 19, 650, DateTimeKind.Utc).AddTicks(6384));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6496));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6498));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 18, 12, 5, 19, 650, DateTimeKind.Local).AddTicks(6503));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProposedAt",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "ProposedContractDurationMonths",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "ProposedEndDate",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "ProposedMonthlyRent",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "ProposedPaymentCycle",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "ProposedPaymentDayOfMonth",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "RenterApproved",
                table: "RentalContracts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "267b8eba-1337-4621-9987-473c4ec6b6ad", new DateTime(2025, 8, 17, 17, 25, 3, 617, DateTimeKind.Local).AddTicks(8054), "AQAAAAIAAYagAAAAELDdJdvGCxxsinRXAJA1uLquoXNzpgIrBKZhUc/Ia/8uGcvd322mJQK5kVVmoSQpgA==", "89838c3a-5453-40a7-afe4-1d240501cb05" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ba43247b-1322-4873-996d-d43fcaae0f89", new DateTime(2025, 8, 17, 17, 25, 3, 681, DateTimeKind.Local).AddTicks(542), "AQAAAAIAAYagAAAAEOq/Mwx82b2OUKDq31XqJZlGOPTccDcxQhyqhDktr9e3pC4xP3N4C/hCjbMeP575gw==", "fbde5d89-4dab-462a-ae31-4b1733ecbe93" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bf9ceba8-b71f-4266-9da3-ea6bf7351c0e", new DateTime(2025, 8, 17, 17, 25, 3, 750, DateTimeKind.Local).AddTicks(3788), "AQAAAAIAAYagAAAAEIqT1cwq4+VD26sy1GBqOvKEOtFtI/3B5eBugUNzysNgOp8TWOpgUZMADTkbGf4Z+g==", "9dd69f7a-c570-4b5b-9bfa-187c45246f17" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e4f95ec-f370-4f6d-b015-09849dbd28a6", new DateTime(2025, 8, 16, 17, 25, 3, 821, DateTimeKind.Local).AddTicks(6454), "AQAAAAIAAYagAAAAEC1aDZXXAeB8J1wqM1ACuwqikEqV+UIeKD/WMy/xb/KZp+NswhSY3qrIExESm1F96w==", "f37f9d12-fef5-4428-bcf6-2b2fb2d674fd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6f84a124-a892-41e5-bcb5-b2b49a329480", new DateTime(2025, 8, 17, 17, 25, 3, 890, DateTimeKind.Local).AddTicks(2777), "AQAAAAIAAYagAAAAEEB8/INQZv6T6H2UbzvGd/IW6j4yPsIC2eZiUnkst3YN1FJ04POLC1qwQTGxd8nNjg==", "3b6b0865-527e-43b5-9232-8f1d47560ba4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8c198e7-b079-4cfd-b304-f6a017664878", new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(3153), "AQAAAAIAAYagAAAAEIRy2/VZFM72Mc55aoIafyzMwO/8d0g4LTMgNCM7xFA8KQedmFg7L6aDcfVFYS33yg==", "0018d4eb-1375-44b3-b744-9b6d28140d78" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 16, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(5371));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 17, 5, 25, 3, 948, DateTimeKind.Local).AddTicks(5375), new DateTime(2025, 8, 17, 7, 25, 3, 948, DateTimeKind.Local).AddTicks(5376) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 17, 12, 25, 3, 948, DateTimeKind.Local).AddTicks(5377), new DateTime(2025, 8, 17, 14, 25, 3, 948, DateTimeKind.Local).AddTicks(5378) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4311));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 16, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4315));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4318));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 14, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4320));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 13, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4323));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4326));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 11, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4328));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4331));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 9, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4333));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4336));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4339));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4341));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 5, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4344));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4346));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4349));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4402));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4561), new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4561) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 16, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4565), new DateTime(2025, 8, 16, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4566) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 15, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4574), new DateTime(2025, 8, 15, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 14, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4586), new DateTime(2025, 8, 14, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4586) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 13, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4588), new DateTime(2025, 8, 13, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4588) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 12, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4590), new DateTime(2025, 8, 12, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4590) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 11, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4592), new DateTime(2025, 8, 11, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4593) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 10, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4600), new DateTime(2025, 8, 10, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4600) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4602), new DateTime(2025, 8, 9, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4602) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4604), new DateTime(2025, 8, 8, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4605) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4606), new DateTime(2025, 8, 7, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4607) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 6, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4608), new DateTime(2025, 8, 6, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4609) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 5, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4610), new DateTime(2025, 8, 5, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4611) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 4, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4612), new DateTime(2025, 8, 4, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4613) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 3, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4617), new DateTime(2025, 8, 3, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4618) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4619), new DateTime(2025, 8, 2, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4620) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 14, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4813), new DateTime(2025, 8, 15, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4812) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 13, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4816), new DateTime(2025, 8, 14, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4815) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 12, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4817), new DateTime(2025, 8, 13, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4817) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 11, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4819), new DateTime(2025, 8, 12, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4819) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 10, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4821), new DateTime(2025, 8, 11, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4820) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 9, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4822), new DateTime(2025, 8, 10, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4822) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 8, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4824), new DateTime(2025, 8, 9, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4824) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4866), new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4865) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4874));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4059));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 15, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4062));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 16, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4072));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 10, 25, 3, 948, DateTimeKind.Utc).AddTicks(4666));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 10, 25, 3, 948, DateTimeKind.Utc).AddTicks(4667));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 10, 25, 3, 948, DateTimeKind.Utc).AddTicks(4668));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4766));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4769));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4778));
        }
    }
}
