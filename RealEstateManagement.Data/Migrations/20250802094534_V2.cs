using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterestedProperty_AspNetUsers_RenterId",
                table: "InterestedProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestedProperty_Properties_PropertyId",
                table: "InterestedProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterestedProperty",
                table: "InterestedProperty");

            migrationBuilder.RenameTable(
                name: "InterestedProperty",
                newName: "InterestedProperties");

            migrationBuilder.RenameIndex(
                name: "IX_InterestedProperty_Status",
                table: "InterestedProperties",
                newName: "IX_InterestedProperties_Status");

            migrationBuilder.RenameIndex(
                name: "IX_InterestedProperty_RenterId",
                table: "InterestedProperties",
                newName: "IX_InterestedProperties_RenterId");

            migrationBuilder.RenameIndex(
                name: "IX_InterestedProperty_PropertyId_RenterId",
                table: "InterestedProperties",
                newName: "IX_InterestedProperties_PropertyId_RenterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterestedProperties",
                table: "InterestedProperties",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedProperties_AspNetUsers_RenterId",
                table: "InterestedProperties",
                column: "RenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedProperties_Properties_PropertyId",
                table: "InterestedProperties",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterestedProperties_AspNetUsers_RenterId",
                table: "InterestedProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestedProperties_Properties_PropertyId",
                table: "InterestedProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterestedProperties",
                table: "InterestedProperties");

            migrationBuilder.RenameTable(
                name: "InterestedProperties",
                newName: "InterestedProperty");

            migrationBuilder.RenameIndex(
                name: "IX_InterestedProperties_Status",
                table: "InterestedProperty",
                newName: "IX_InterestedProperty_Status");

            migrationBuilder.RenameIndex(
                name: "IX_InterestedProperties_RenterId",
                table: "InterestedProperty",
                newName: "IX_InterestedProperty_RenterId");

            migrationBuilder.RenameIndex(
                name: "IX_InterestedProperties_PropertyId_RenterId",
                table: "InterestedProperty",
                newName: "IX_InterestedProperty_PropertyId_RenterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterestedProperty",
                table: "InterestedProperty",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f37868b9-1d6b-4aee-8403-412aea4dbab9", new DateTime(2025, 8, 1, 15, 55, 1, 309, DateTimeKind.Local).AddTicks(3056), "AQAAAAIAAYagAAAAEP0ZdmNPzKkw5CrG7PqxbpVbzD99WmiqCbAPWITLEoAB5rB9jsyfa5qrxPittSymTQ==", "518ae8e7-5f8a-4b49-85e2-b4258591cb0d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "916ecf49-541a-4da0-85d0-cbcb4967dff6", new DateTime(2025, 8, 1, 15, 55, 1, 371, DateTimeKind.Local).AddTicks(9826), "AQAAAAIAAYagAAAAEOwZ9nwZoU96evaXI7CCXGrpEai8yohFBBXQV37akYiPYfoRe5YaxwWPX2k7DwJgKw==", "53dee088-06bd-44a7-80a2-ae23fa505f69" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "797b70bb-d663-42ba-b470-e2921bd685d4", new DateTime(2025, 8, 1, 15, 55, 1, 436, DateTimeKind.Local).AddTicks(2538), "AQAAAAIAAYagAAAAEMrHFhTANlkxwm6luNr2wm3UoNf4F6RrGk4mo5OhpstdwpNs1Ovf1ouZwgNxCPkSfg==", "e8e4109c-ddfe-4209-adab-8ba521f7da9f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "61f1a892-56d2-4b11-a1ff-527a8445408f", new DateTime(2025, 7, 31, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(370), "AQAAAAIAAYagAAAAENXbsX5MOAbWl7Br+n+HmGpHeIHotNq6gOk5dBuZztn6hdjEcgpbEoOJpeyoMNFV4w==", "c0ff5730-7943-4658-99a8-0aeb137302c6" });

            migrationBuilder.UpdateData(
                table: "InterestedProperty",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 7, 31, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(2184));

            migrationBuilder.UpdateData(
                table: "InterestedProperty",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 1, 3, 55, 1, 498, DateTimeKind.Local).AddTicks(2188), new DateTime(2025, 8, 1, 5, 55, 1, 498, DateTimeKind.Local).AddTicks(2188) });

            migrationBuilder.UpdateData(
                table: "InterestedProperty",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 1, 10, 55, 1, 498, DateTimeKind.Local).AddTicks(2190), new DateTime(2025, 8, 1, 12, 55, 1, 498, DateTimeKind.Local).AddTicks(2190) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1045));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1048));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1051));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1160), new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1161) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1254), new DateTime(2025, 7, 31, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1255) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1257), new DateTime(2025, 7, 30, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1258) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 31, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1446), new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1445) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1448), new DateTime(2025, 7, 31, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1448) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 28, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1450), new DateTime(2025, 7, 30, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1449) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1705), new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1703) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1713));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1370));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(885));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(887));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(889));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 8, 55, 1, 498, DateTimeKind.Utc).AddTicks(1293));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1409));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 15, 55, 1, 498, DateTimeKind.Local).AddTicks(1414));

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedProperty_AspNetUsers_RenterId",
                table: "InterestedProperty",
                column: "RenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedProperty_Properties_PropertyId",
                table: "InterestedProperty",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
