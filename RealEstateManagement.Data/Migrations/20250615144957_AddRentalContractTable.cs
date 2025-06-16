using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalContractTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentalContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyPostId = table.Column<int>(type: "int", nullable: false),
                    LandlordId = table.Column<int>(type: "int", nullable: false),
                    RenterId = table.Column<int>(type: "int", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContractDurationMonths = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalContracts_AspNetUsers_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalContracts_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalContracts_PropertyPosts_PropertyPostId",
                        column: x => x.PropertyPostId,
                        principalTable: "PropertyPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bc4ed01b-142a-4eb9-9c71-42b47a49a5ab", new DateTime(2025, 6, 15, 21, 49, 56, 26, DateTimeKind.Local).AddTicks(3505), "AQAAAAIAAYagAAAAEPYK73AJR6v0vpPhuruM0Ga4zD/waqVDKKsvNR+zgePHib8Z7tq+L+ZnMWKov3essQ==", "fec5fdca-f1b6-4e76-bc49-c53f66600d24" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2cb0ea3c-91a9-4a68-906d-3b9781bdc047", new DateTime(2025, 6, 15, 21, 49, 56, 85, DateTimeKind.Local).AddTicks(982), "AQAAAAIAAYagAAAAEJftu1j0r/7U/I45BOLVetH/H/UDNtEOBmKUeWB0NnmlbKhUe/FCUp3eL81VxUnWag==", "2cf67377-4046-4177-acec-151470cbe3f4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f362a8ee-d8ea-4f8e-b38f-06f485d521c3", new DateTime(2025, 6, 15, 21, 49, 56, 141, DateTimeKind.Local).AddTicks(9217), "AQAAAAIAAYagAAAAEABWvyet4BgyEMhql+K+MJhDB+lpDvVuiMOlqAY81Sb8F4xcHJGE5WN6zgpIOOgRRw==", "87e5c697-441f-433c-b159-5e76bc11ae23" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "64c5fa0e-7b41-4b2c-ba20-6bde3f4bdf2a", new DateTime(2025, 6, 14, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(317), "AQAAAAIAAYagAAAAECxuOtomOUIn26/y3/HXV07u+ZSB/qQL7x1nzbXjStiCmmD2TjB+XaT/fvGkmpyrEQ==", "27630a70-6baa-4953-beb1-63c1f93eeab5" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 15, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2117));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2128));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 15, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2307), new DateTime(2025, 6, 15, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2308) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 14, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2314), new DateTime(2025, 6, 14, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2315) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 13, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2316), new DateTime(2025, 6, 13, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2317) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2517));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 15, 21, 49, 56, 200, DateTimeKind.Local).AddTicks(2469));

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_LandlordId",
                table: "RentalContracts",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_PropertyPostId",
                table: "RentalContracts",
                column: "PropertyPostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_RenterId",
                table: "RentalContracts",
                column: "RenterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalContracts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "10e46776-cf68-43b0-8657-33f3ecf1703a", new DateTime(2025, 6, 12, 14, 36, 48, 252, DateTimeKind.Local).AddTicks(3058), "AQAAAAIAAYagAAAAEApEHr6//GUA2POWQza+e5nHkSrzqbVrLl8aq9yRTPOhop/Y+ef0PG2A+71DEWSdSg==", "e0f8917d-4ce4-4615-9c7c-63300e41d2cb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "03a90e5d-9c44-4a86-aece-c5e63982b429", new DateTime(2025, 6, 12, 14, 36, 48, 308, DateTimeKind.Local).AddTicks(6815), "AQAAAAIAAYagAAAAEMTlj2fXEy9lFpCC4Vcs6SVUZnm0StjOTnBYuEGFCaE8PebnboIMTAt9Zv6KD46U6w==", "b88ee6cb-bebf-4371-acdc-91bdd3e1d847" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e205075f-1d74-45b8-95e0-519e026952eb", new DateTime(2025, 6, 12, 14, 36, 48, 368, DateTimeKind.Local).AddTicks(5086), "AQAAAAIAAYagAAAAECWXbok5YeXQvgy+t00Q93W0uagcHHMPksprDxZdBOwPrkVwEwIIlfgddOq0LxUhow==", "147390c2-da0b-4fae-bd52-be3fa8291ece" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "24f3539f-80ea-434d-8582-0d7e16fe55a4", new DateTime(2025, 6, 11, 14, 36, 48, 426, DateTimeKind.Local).AddTicks(9462), "AQAAAAIAAYagAAAAEA2+j1Bi5lcZMdAA1pdLFVdXcbJoeHm0RJBj42S1Evvsi+T/I+8muWJcLW6d2qZCvA==", "a3d1d7a3-00a5-41b1-ac91-72a55cd71b17" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 12, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(359));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(137));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 11, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(141));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 10, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(151));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 12, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(314), new DateTime(2025, 6, 12, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(315) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(321), new DateTime(2025, 6, 11, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(322) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(324), new DateTime(2025, 6, 10, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(324) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 11, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(444));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(402));
        }
    }
}
