using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "CreatedAt", "Status", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 12, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(314), "Approved", new DateTime(2025, 6, 12, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(315) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Status", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(321), "Approved", new DateTime(2025, 6, 11, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(322) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Status", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(324), "Approved", new DateTime(2025, 6, 10, 14, 36, 48, 427, DateTimeKind.Local).AddTicks(324) });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0f5e8cd3-daa6-4dd7-99ec-201c2ffd1cc0", new DateTime(2025, 6, 6, 23, 23, 25, 598, DateTimeKind.Local).AddTicks(1928), "AQAAAAIAAYagAAAAEOAbKxAROu0rHxzGonqv5PusTpSeIXzR1szF8o4jNToR5SkmdfqARDdAHynUQGNcIA==", "6729fc8f-c583-486d-b100-d06596d5962b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "01e61fc6-1c84-440f-8df7-d4c3d6db0c81", new DateTime(2025, 6, 6, 23, 23, 25, 662, DateTimeKind.Local).AddTicks(2633), "AQAAAAIAAYagAAAAEDCMg7TUEbtIcshH2EP67QtlpIu+yMP6gxe8v1tmD9j1k2TyqMLApl5o1C9xiT4/dg==", "ab961f6a-52b9-481c-8b66-53b67823c72e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0fc9de50-1ef5-4251-a496-1b930dbdd6a0", new DateTime(2025, 6, 6, 23, 23, 25, 736, DateTimeKind.Local).AddTicks(478), "AQAAAAIAAYagAAAAELo3SAaXzQhmpf1kOX4zROukQqAF80ZgRjy/IZ+9obANrhvsNi/q6tdxPj8WADZMkQ==", "b5afbf40-9d24-43de-8fcd-c8e93a0a32b8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6b173485-df55-44dd-a798-403b143220cd", new DateTime(2025, 6, 5, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(7267), "AQAAAAIAAYagAAAAEIMfmmWUsp02vLc6awlBXSdlOn129wtcg5GJh33J3aE1kLoOJ+IIZODo7xPIQFfYOw==", "7a7c3aee-39e9-4a44-acb9-91d054dd150a" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaidAt",
                value: new DateTime(2025, 6, 6, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(8119));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 6, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(7852));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 5, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(7860));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 4, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(7865));

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Status", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 6, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(8060), "approved", new DateTime(2025, 6, 6, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(8062) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Status", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(8068), "approved", new DateTime(2025, 6, 5, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(8069) });

            migrationBuilder.UpdateData(
                table: "PropertyPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Status", "VerifiedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(8072), "approved", new DateTime(2025, 6, 4, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(8076) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 5, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(8236));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 6, 23, 23, 25, 803, DateTimeKind.Local).AddTicks(8170));
        }
    }
}
