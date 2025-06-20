using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Properties");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "promotionPackages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "promotionPackages",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "promotionPackages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "promotionPackages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wards_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Streets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streets_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceId = table.Column<int>(type: "int", nullable: true),
                    WardId = table.Column<int>(type: "int", nullable: true),
                    StreetId = table.Column<int>(type: "int", nullable: true),
                    DetailedAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Addresses_Streets_StreetId",
                        column: x => x.StreetId,
                        principalTable: "Streets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Addresses_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8fa41f46-6e23-4ea5-b4bb-60eab5651d6d", new DateTime(2025, 6, 20, 13, 17, 21, 847, DateTimeKind.Local).AddTicks(2701), "AQAAAAIAAYagAAAAEDWquDAh8LprxXe74mQaxY7YKefT6rp3TbeSyR9S7g3EQd+nauzpwdYNsNHrb9ftPg==", "76b91521-6345-4214-845b-270726aec8a5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a7b00fa3-b067-41d3-a803-b537c0675861", new DateTime(2025, 6, 20, 13, 17, 21, 908, DateTimeKind.Local).AddTicks(2922), "AQAAAAIAAYagAAAAEIgK4lkVFzdympA3DnYmNuxcyKeKmzLpc4WsbMHkKQE6j9nw84m0pfZF4NFQlVF2/g==", "fb16e249-555a-4f25-9370-154abeea76e8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cc1aff9a-1191-446f-abb2-d3f3e13acd58", new DateTime(2025, 6, 20, 13, 17, 21, 969, DateTimeKind.Local).AddTicks(5107), "AQAAAAIAAYagAAAAEO9ACCCVBUmj588oDnlWp1hWPXvwBsitBx7v7K5FRTv9r2v4zd1/5dL0zKzNl7y2Fg==", "dadd51ea-fd8c-449f-b058-b19e0dfcfde6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79128b50-5400-4b8c-80d8-8de9404c4c3e", new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(6626), "AQAAAAIAAYagAAAAEIxUWH2eemh+914qPkvAk+hVng69Iu81B9KDhH4TvLbedb0W7Ubh+Tnefy9cenDSTA==", "2cce8fcd-5eda-4c9b-b3fb-7c042f1ea7ea" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7910));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddressId", "CreatedAt" },
                values: new object[] { 1, new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7592) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AddressId", "CreatedAt" },
                values: new object[] { 2, new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7597) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddressId", "CreatedAt" },
                values: new object[] { 3, new DateTime(2025, 6, 18, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7600) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7866), new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7867) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7872), new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7874) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7875), new DateTime(2025, 6, 18, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7876) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1305), new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1304) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 19, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1307), new DateTime(2025, 6, 19, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1307) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 16, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1309), new DateTime(2025, 6, 18, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1308) });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ho Chi Minh City" },
                    { 2, "Hanoi" }
                });

            migrationBuilder.InsertData(
                table: "RentalContracts",
                columns: new[] { "Id", "ConfirmedAt", "ContactInfo", "ContractDurationMonths", "CreatedAt", "DepositAmount", "LandlordId", "MonthlyRent", "PaymentMethod", "PropertyPostId", "RenterId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1381), "renter@example.com | 03345678910", 12, new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1356), 2000000m, 2, 5000000m, "Bank Transfer", 1, 3, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, null, "renter2@example.com | 0322222222", 6, new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1385), 1500000m, 2, 2000000m, "Momo", 2, 4, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 }
                });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1229));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1145));

            migrationBuilder.InsertData(
                table: "UserFavoriteProperties",
                columns: new[] { "PropertyId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "UserPreferences",
                columns: new[] { "Id", "Amenities", "CreatedAt", "Location", "PriceRangeMax", "PriceRangeMin", "UserId" },
                values: new object[,]
                {
                    { 1, "WiFi,Parking", new DateTime(2025, 6, 20, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7379), "District 1", 6000000m, 3000000m, 3 },
                    { 2, "WiFi", new DateTime(2025, 6, 18, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7382), "Go Vap", 3000000m, 1500000m, 3 },
                    { 3, "AC", new DateTime(2025, 6, 19, 13, 17, 22, 30, DateTimeKind.Local).AddTicks(7386), "Tan Binh", 4000000m, 2000000m, 4 }
                });

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1268));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1271));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 20, 13, 17, 22, 31, DateTimeKind.Local).AddTicks(1273));

            migrationBuilder.InsertData(
                table: "Wards",
                columns: new[] { "Id", "Name", "ProvinceId" },
                values: new object[,]
                {
                    { 1, "Ben Nghe", 1 },
                    { 2, "Nguyen Hue", 1 },
                    { 3, "Ward 10", 1 },
                    { 4, "Ward 4", 1 },
                    { 5, "Tan Dinh", 1 },
                    { 6, "Da Kao", 1 },
                    { 7, "Thao Dien", 1 },
                    { 8, "An Phu", 1 },
                    { 9, "Binh Thanh", 1 },
                    { 10, "Phu Nhuan", 1 },
                    { 11, "Hoan Kiem", 2 },
                    { 12, "Ba Dinh", 2 },
                    { 13, "Hai Ba Trung", 2 },
                    { 14, "Dong Da", 2 },
                    { 15, "Cau Giay", 2 },
                    { 16, "Tay Ho", 2 },
                    { 17, "Long Bien", 2 },
                    { 18, "Nam Tu Liem", 2 }
                });

            migrationBuilder.InsertData(
                table: "Streets",
                columns: new[] { "Id", "Name", "WardId" },
                values: new object[,]
                {
                    { 1, "Nguyen Hue", 2 },
                    { 2, "Le Van Tho", 3 },
                    { 3, "Ly Thuong Kiet", 4 },
                    { 4, "Hai Ba Trung", 5 },
                    { 5, "Pasteur", 6 },
                    { 6, "Quang Trung", 7 },
                    { 7, "An Phu", 8 },
                    { 8, "Dinh Bo Linh", 9 },
                    { 9, "Phan Dinh Phung", 10 },
                    { 10, "Hang Dao", 11 },
                    { 11, "Hoang Dieu", 12 },
                    { 12, "Le Duan", 13 },
                    { 13, "Chua Boc", 14 },
                    { 14, "Nguyen Van Huyen", 15 },
                    { 15, "Thanh Nien", 16 },
                    { 16, "Nguyen Khoai", 17 },
                    { 17, "Pham Van Dong", 18 }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "DetailedAddress", "PropertyId", "ProvinceId", "StreetId", "WardId" },
                values: new object[,]
                {
                    { 1, "123", 1, 1, 1, 2 },
                    { 2, "456", 2, 1, 2, 3 },
                    { 3, "789", 3, 1, 3, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AddressId",
                table: "Properties",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ProvinceId",
                table: "Addresses",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StreetId",
                table: "Addresses",
                column: "StreetId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_WardId",
                table: "Addresses",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_WardId",
                table: "Streets",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_ProvinceId",
                table: "Wards",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Streets");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Properties_AddressId",
                table: "Properties");

            migrationBuilder.DeleteData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserFavoriteProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserFavoriteProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "UserFavoriteProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Properties");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Properties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "promotionPackages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "promotionPackages",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "promotionPackages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "promotionPackages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d1e8d527-ff13-4950-8500-b3321cf87953", new DateTime(2025, 6, 18, 13, 18, 35, 404, DateTimeKind.Local).AddTicks(7222), "AQAAAAIAAYagAAAAEHNkvtbOabtLTUHR0sTkkGZigH5lFFQxBFppfcin98r9rItCKD9Dg4k2WN/Z4bytiA==", "c310a2a6-dd4f-4a32-89a5-7939159f8dc6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4648da0b-d0a8-4fe5-bdee-24ec40249e6d", new DateTime(2025, 6, 18, 13, 18, 35, 460, DateTimeKind.Local).AddTicks(5975), "AQAAAAIAAYagAAAAEIsEGd/UDbTrlSG9UtF8duQsLNu+FfXAcnVJaFNW8TuTFEvkXc7xwn0vgOLxdB42Zg==", "e3f52398-b98d-49b9-a877-61e4dc3c1f9e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3947427b-25d9-42e8-83fc-e2863f690e1f", new DateTime(2025, 6, 18, 13, 18, 35, 516, DateTimeKind.Local).AddTicks(7541), "AQAAAAIAAYagAAAAEISB1LfG3K9AcuPUT6Y3rC/7T1iyL7Md4tPRifw9SD2Vj2BjwogYXzXuUtxQk055Zg==", "cb3cbc2c-3fe3-4eba-b074-f5776d4c7eff" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee9514db-3821-4795-b50e-116bda96771b", new DateTime(2025, 6, 17, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(7028), "AQAAAAIAAYagAAAAEG/U2wp5cHc0L4TWxb8EW36yBHtY2HnRDSPhPPoYjYdGycX8xB5lvAbGFJF3b0a8OA==", "5111a63a-0ed5-40de-9bf9-4fbc88e8c304" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8336));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { "123 Nguyen Hue, District 1, HCMC", new DateTime(2025, 6, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(7987) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { "456 Le Van Tho, Go Vap, HCMC", new DateTime(2025, 6, 17, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(7991) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { "789 Ly Thuong Kiet, Tan Binh, HCMC", new DateTime(2025, 6, 16, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(7997) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8292), new DateTime(2025, 6, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8293) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 17, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8297), new DateTime(2025, 6, 17, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8298) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8300), new DateTime(2025, 6, 16, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8300) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8560), new DateTime(2025, 6, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8559) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8562), new DateTime(2025, 6, 17, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8561) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 14, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8564), new DateTime(2025, 6, 16, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8563) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8433));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8393));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8519));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8521));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 18, 13, 18, 35, 571, DateTimeKind.Local).AddTicks(8523));
        }
    }
}
