using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRentalContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPaymentDate",
                table: "RentalContracts",
                type: "datetime2",
                nullable: true);

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
                columns: new[] { "ConfirmedAt", "CreatedAt", "LastPaymentDate" },
                values: new object[] { new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4866), new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4865), null });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "LastPaymentDate" },
                values: new object[] { new DateTime(2025, 8, 17, 17, 25, 3, 948, DateTimeKind.Local).AddTicks(4874), null });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPaymentDate",
                table: "RentalContracts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e670c4f1-8140-4340-ad52-6722ef52a09f", new DateTime(2025, 8, 12, 14, 40, 20, 874, DateTimeKind.Local).AddTicks(78), "AQAAAAIAAYagAAAAEBBqqD/vIvECTp6XkEiM+U2oV1FQxNOFaYlTk0RSYEia1GLgawcgk3/iowXotso7cg==", "4ad20275-38aa-4849-955b-481112c0b018" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e732c05f-8c57-41ee-b25d-43c6aaccf053", new DateTime(2025, 8, 12, 14, 40, 20, 929, DateTimeKind.Local).AddTicks(6161), "AQAAAAIAAYagAAAAEHKcxutrE+8z1ELtso5hU6H/rZHooa1uXanUqgA/797U25m/0G0oYh0UQ1qP0tWBxg==", "406b57da-3d40-45c3-82c7-140c7b8da1d5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a744dd6-71f6-4e68-9403-9c7d05ca72fe", new DateTime(2025, 8, 12, 14, 40, 20, 985, DateTimeKind.Local).AddTicks(2756), "AQAAAAIAAYagAAAAEB0XESZ+vBcPlRPxDpUU6wqkGHXcGNdDph0WKrpXUhkm6QRXvY4V375fqcNkMZ3hRg==", "1a2e645d-af7b-439b-847e-7bd7f910761a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fd09a617-e333-405d-a4b7-74c7bc75a4ab", new DateTime(2025, 8, 11, 14, 40, 21, 40, DateTimeKind.Local).AddTicks(3390), "AQAAAAIAAYagAAAAEHAVr13XP2458WEUWjCwfidpxFMUAU9XQpJwTBpPsMdbeI4yXxzk3rbVds3JXw8hVA==", "1100623b-4066-4af7-9153-3b5be8d2ac99" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dd717dbc-1512-4d06-bbc6-d92f46aa8372", new DateTime(2025, 8, 12, 14, 40, 21, 94, DateTimeKind.Local).AddTicks(9694), "AQAAAAIAAYagAAAAECun+MH1B6jktqqexppukJx9bPm+RKnccea0XCmo6SVlZdEHFFvuEHl5BkLfwl0g4Q==", "a9bad81f-e3ec-407b-9f2c-83760cba1e8e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f9c28e54-763d-4895-8796-4a46890dde51", new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(357), "AQAAAAIAAYagAAAAEO6VGh11QrkzLSr+Jt4rhikos1adWJNti/oofNu23fGRSpyS7UmnVyZUK4c9BcUf0A==", "7d7a22d1-1e4b-47b7-bff1-2b882ae22021" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2652));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 12, 2, 40, 21, 150, DateTimeKind.Local).AddTicks(2677), new DateTime(2025, 8, 12, 4, 40, 21, 150, DateTimeKind.Local).AddTicks(2678) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 12, 9, 40, 21, 150, DateTimeKind.Local).AddTicks(2680), new DateTime(2025, 8, 12, 11, 40, 21, 150, DateTimeKind.Local).AddTicks(2681) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1489));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1493));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1496));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1499));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1501));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1505));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1508));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1511));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1513));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1516));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1519));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1521));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1585));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1588));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1592));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 28, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1595));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1797), new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1797) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1800), new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1801) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1803), new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1803) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1805), new DateTime(2025, 8, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1821) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1828), new DateTime(2025, 8, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1829) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1831), new DateTime(2025, 8, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1831) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1834), new DateTime(2025, 8, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1835) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1850), new DateTime(2025, 8, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1850) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1852), new DateTime(2025, 8, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 3, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1855), new DateTime(2025, 8, 3, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1855) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1857), new DateTime(2025, 8, 2, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1858) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 1, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1859), new DateTime(2025, 8, 1, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1860) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1862), new DateTime(2025, 7, 31, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1863) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 30, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1864), new DateTime(2025, 7, 30, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1865) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 29, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1870), new DateTime(2025, 7, 29, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1870) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 28, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1872), new DateTime(2025, 7, 28, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1873) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2094), new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2092) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2097), new DateTime(2025, 8, 9, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2096) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2099), new DateTime(2025, 8, 8, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2098) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2101), new DateTime(2025, 8, 7, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2100) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2102), new DateTime(2025, 8, 6, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2102) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2104), new DateTime(2025, 8, 5, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2104) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 3, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2106), new DateTime(2025, 8, 4, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2105) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2154), new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2154) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2161));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1184));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1187));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 11, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(1199));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 7, 40, 21, 150, DateTimeKind.Utc).AddTicks(1920));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 7, 40, 21, 150, DateTimeKind.Utc).AddTicks(1922));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 7, 40, 21, 150, DateTimeKind.Utc).AddTicks(1923));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2046));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2048));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 12, 14, 40, 21, 150, DateTimeKind.Local).AddTicks(2050));
        }
    }
}
