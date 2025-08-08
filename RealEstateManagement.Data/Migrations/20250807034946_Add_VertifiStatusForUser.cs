using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_VertifiStatusForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerificationStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp", "VerificationStatus" },
                values: new object[] { "08745c9f-4b03-4500-8f25-b8c6dd953159", new DateTime(2025, 8, 7, 10, 49, 42, 972, DateTimeKind.Local).AddTicks(1022), "AQAAAAIAAYagAAAAEOUCdkHmlWOLfWKQh3B8tYA5XDNzWEqzZYcGG7wG6Mxl00iHWBIktweTNq6y5ioKvA==", "2fe855d7-618f-450b-8e21-ea18279d7b54", "none" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp", "VerificationStatus" },
                values: new object[] { "c349209e-44eb-4c3b-854d-5c1cf85c2bbb", new DateTime(2025, 8, 7, 10, 49, 43, 40, DateTimeKind.Local).AddTicks(4854), "AQAAAAIAAYagAAAAEPVFuGwfuENfPXqD5F1m+QVrLzrtEV3kBSuDPkcgYSP/dEjyHCbR7P/MBjceMcIW7A==", "28ece820-f24d-45b3-b2ef-d133996d7a57", "none" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp", "VerificationStatus" },
                values: new object[] { "e5f52a43-7abb-48d8-865a-9458f06e739d", new DateTime(2025, 8, 7, 10, 49, 43, 107, DateTimeKind.Local).AddTicks(6223), "AQAAAAIAAYagAAAAEFJ4c7iNWfrAHegdMatF196gGyJEa5GVPIGnOzKCw3rsFKpbTDQ6EFlRU5i07w8IpA==", "de844788-a9db-4305-aede-1b50d9e099b3", "none" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp", "VerificationStatus" },
                values: new object[] { "25fcb626-2baa-4068-813a-b7f5e941818e", new DateTime(2025, 8, 6, 10, 49, 43, 171, DateTimeKind.Local).AddTicks(9899), "AQAAAAIAAYagAAAAECNywq9yXmlnRUZoUU8okUdG9qI+o7wj59Ll4BeIcDuOVKf7onTYme+TbTzPmP694w==", "42ad44d0-58e5-4969-b30b-f5432dce30c4", "none" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 6, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1677));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 6, 22, 49, 43, 172, DateTimeKind.Local).AddTicks(1685), new DateTime(2025, 8, 7, 0, 49, 43, 172, DateTimeKind.Local).AddTicks(1686) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 7, 5, 49, 43, 172, DateTimeKind.Local).AddTicks(1687), new DateTime(2025, 8, 7, 7, 49, 43, 172, DateTimeKind.Local).AddTicks(1688) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(825));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(829));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 5, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(832));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1007), new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1008) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 6, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1011), new DateTime(2025, 8, 6, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1012) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 5, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1014), new DateTime(2025, 8, 5, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1014) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 6, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1277), new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1276) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 5, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1280), new DateTime(2025, 8, 6, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1279) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 11, 3, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1281), new DateTime(2025, 8, 5, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1281) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1320), new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1319) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1324));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1147));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(630));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 5, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(635));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(638));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 3, 49, 43, 172, DateTimeKind.Utc).AddTicks(1054));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1240));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 7, 10, 49, 43, 172, DateTimeKind.Local).AddTicks(1241));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationStatus",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "354867ad-5ad0-4877-b8b2-fccbd97241ff", new DateTime(2025, 8, 2, 18, 7, 12, 607, DateTimeKind.Local).AddTicks(43), "AQAAAAIAAYagAAAAEGX2dcBYbr92uIhT8keMjtt037YrxLsQSTj3LbjtHh3bLeRLG9rbY6sG9CkZZdpg+A==", "4a2ca2c1-46da-4c4f-a461-3dcf491768ae" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "134ccd5a-f9f7-4253-997f-7ed209264795", new DateTime(2025, 8, 2, 18, 7, 12, 670, DateTimeKind.Local).AddTicks(2233), "AQAAAAIAAYagAAAAENGVIrff3XclHR05FKiKelUdXsYsxPhuvPAzUrEytV/dcuWo5YzOt8hkepJT9bfzIQ==", "6c051415-2fdb-4326-91ed-6143ce274d06" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4a28d93f-669b-4dc6-bc57-d133bf83a34f", new DateTime(2025, 8, 2, 18, 7, 12, 723, DateTimeKind.Local).AddTicks(2241), "AQAAAAIAAYagAAAAECMMJtpT6PPUog1s8MtrXUduT8Leh9XfCz5XHcNduphZYDNnjPSvf9SA/NKu9wfUQQ==", "8697c473-bc24-4da6-ba5b-8a7d2bef6e9d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b955d41-ee43-4176-ba3f-22399a342c4c", new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(6906), "AQAAAAIAAYagAAAAEKNMZht70u/LdYsI/3uVGT4FNABVd0PFBYbzLYElZ2/DAFGO9McVEVJdDNS7Hgl+KQ==", "fc6edccb-99f7-4a6e-a8cc-2c357f2de0df" });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 1,
                column: "InterestedAt",
                value: new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8853));

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 2, 6, 7, 12, 775, DateTimeKind.Local).AddTicks(8858), new DateTime(2025, 8, 2, 8, 7, 12, 775, DateTimeKind.Local).AddTicks(8859) });

            migrationBuilder.UpdateData(
                table: "InterestedProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "InterestedAt", "RenterReplyAt" },
                values: new object[] { new DateTime(2025, 8, 2, 13, 7, 12, 775, DateTimeKind.Local).AddTicks(8861), new DateTime(2025, 8, 2, 15, 7, 12, 775, DateTimeKind.Local).AddTicks(8861) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8034));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8038));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8040));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8181), new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8181) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8183), new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8185) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8186), new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8186) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8431), new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8431) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8436), new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8436) });

            migrationBuilder.UpdateData(
                table: "PropertyPromotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 10, 29, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8438), new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8437) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt" },
                values: new object[] { new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8482), new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8480) });

            migrationBuilder.UpdateData(
                table: "RentalContracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8490));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8350));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(7732));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(7735));

            migrationBuilder.UpdateData(
                table: "UserPreferences",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(7737));

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 11, 7, 12, 775, DateTimeKind.Utc).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8391));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8394));

            migrationBuilder.UpdateData(
                table: "promotionPackages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 2, 18, 7, 12, 775, DateTimeKind.Local).AddTicks(8395));
        }
    }
}
