using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "renter"),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmationCodeExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CitizenIdNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitizenIdIssuedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CitizenIdExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CitizenIdImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitizenIdFrontImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitizenIdBackImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationRejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "promotionPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotionPackages", x => x.Id);
                });

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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PriceRangeMin = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceRangeMax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amenities = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "WalletTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
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

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Area = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: false),
                    LandlordId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsPromoted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ViewsCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Properties_AspNetUsers_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RenterId = table.Column<int>(type: "int", nullable: false),
                    LandlordId = table.Column<int>(type: "int", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LastSentAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversation_AspNetUsers_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversation_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversation_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyAmenities",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyAmenities", x => new { x.PropertyId, x.AmenityId });
                    table.ForeignKey(
                        name: "FK_PropertyAmenities_Amenities_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyAmenities_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyImages_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    LandlordId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    ArchiveDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyPosts_AspNetUsers_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropertyPosts_AspNetUsers_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropertyPosts_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyPromotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyPromotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyPromotions_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyPromotions_promotionPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "promotionPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    RenterId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.CheckConstraint("CK_Review_Rating_Range", "Rating BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    NotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    PaymentCycle = table.Column<int>(type: "int", nullable: false),
                    PaymentDayOfMonth = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Air Conditioning", "AC" },
                    { 2, "High-speed Internet", "WiFi" },
                    { 3, "Parking Space", "Parking" },
                    { 4, "Private Balcony", "Balcony" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "Admin", "ADMIN" },
                    { 2, null, "Landlord", "LANDLORD" },
                    { 3, null, "Renter", "RENTER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CitizenIdBackImageUrl", "CitizenIdExpiryDate", "CitizenIdFrontImageUrl", "CitizenIdImageUrl", "CitizenIdIssuedDate", "CitizenIdNumber", "ConcurrencyStamp", "ConfirmationCode", "ConfirmationCodeExpiry", "CreatedAt", "Email", "EmailConfirmed", "IsActive", "IsVerified", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName", "VerificationRejectReason" },
                values: new object[,]
                {
                    { 1, 0, "https://example.com/cccd/admin_back.jpg", new DateTime(2030, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/admin_front.jpg", null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "01234567890", "424909f1-2a0a-41d0-b0e4-516cef6d4133", null, null, new DateTime(2025, 7, 12, 22, 9, 23, 60, DateTimeKind.Local).AddTicks(3861), "admin@example.com", false, true, true, false, null, "Admin User", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEABdLF+n0+Pbe5o27W+/gqpvDJOVnSMW2PxzydMX9xtMxg3WnfDhxiW6qEoSXaWfhA==", "+841234567891", true, null, null, null, "admin", "db18075b-9ca9-4759-9877-5e231b589b80", false, "admin@example.com", null },
                    { 2, 0, "https://example.com/cccd/landlord_back.jpg", new DateTime(2031, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/landlord_front.jpg", "https://example.com/cccd/landlord.jpg", new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "002345678901", "8096638a-fc01-4aa8-a475-92085d126e0a", null, null, new DateTime(2025, 7, 12, 22, 9, 23, 152, DateTimeKind.Local).AddTicks(4444), "landlord@example.com", false, true, true, false, null, "Landlord User", "LANDLORD@EXAMPLE.COM", "LANDLORD@EXAMPLE.COM", "AQAAAAIAAYagAAAAELDx2YpLBzRD4M9pl7TZv03G61yxar5pbF/LQH7loD1JP260Wrmfzsbd+Uyau0WpCQ==", "+842345678910", true, "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0", null, null, "landlord", "04b4cf68-3413-41e0-aa66-5e850de9fabd", false, "landlord@example.com", null },
                    { 3, 0, "https://example.com/cccd/renter_back.jpg", new DateTime(2032, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/renter_front.jpg", "https://example.com/cccd/renter.jpg", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "003456789012", "8253dc22-e9a4-4af6-8ca6-801d61d82948", null, null, new DateTime(2025, 7, 12, 22, 9, 23, 230, DateTimeKind.Local).AddTicks(8906), "renter@example.com", false, true, true, false, null, "Renter User", "RENTER@EXAMPLE.COM", "RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEL9OvBXKJ4ifB/n0FtIfXiBoT5UB2B5Tz/cyE+HFj3qr1RAON9/zYfj2MLW5jr6Dyw==", "+843345678910", true, null, null, null, "renter", "03266972-131c-414c-b818-1badb2d2eeac", false, "renter@example.com", null },
                    { 4, 0, "https://example.com/cccd/renter2_back.jpg", new DateTime(2033, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/renter2_front.jpg", "https://example.com/cccd/renter2.jpg", new DateTime(2023, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "004567890123", "3d59fbfc-e023-46da-b143-02b638a7f888", null, null, new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(8316), "renter2@example.com", false, false, true, false, null, "Renter User 2", "RENTER2@EXAMPLE.COM", "RENTER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEIxbGWRQ0naQlm5A728dyTnVYdVE+rqEyrTSlccer7/pqj6GAPFwochqZ4dsVs2mMg==", null, false, null, null, null, "renter", "a094b9bf-74d6-4e39-88c0-baca4e9645f2", false, "renter2@example.com", "Ảnh CCCD mặt sau bị mờ, thiếu ngày cấp. Vui lòng bổ sung lại!" }
                });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ho Chi Minh City" },
                    { 2, "Hanoi" }
                });

            migrationBuilder.InsertData(
                table: "promotionPackages",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationInDays", "IsActive", "Level", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9752), "Basic promotion package for property listings.", 30, true, 1, "Basic Promotion", 1000000m, null },
                    { 2, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9754), "Premium promotion package for property listings.", 60, true, 2, "Premium Promotion", 2000000m, null },
                    { 3, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9756), "Ultimate promotion package for property listings.", 90, true, 3, "Ultimate Promotion", 3000000m, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "UserPreferences",
                columns: new[] { "Id", "Amenities", "CreatedAt", "Location", "PriceRangeMax", "PriceRangeMin", "UserId" },
                values: new object[,]
                {
                    { 1, "WiFi,Parking", new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9041), "District 1", 6000000m, 3000000m, 3 },
                    { 2, "WiFi", new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9044), "Go Vap", 3000000m, 1500000m, 3 },
                    { 3, "AC", new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9051), "Tan Binh", 4000000m, 2000000m, 4 }
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "Balance", "CreatedAt", "UserId" },
                values: new object[] { 1, 0m, new DateTime(2025, 7, 12, 15, 9, 23, 324, DateTimeKind.Utc).AddTicks(9466), 1 });

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
                table: "WalletTransactions",
                columns: new[] { "Id", "Amount", "CreatedAt", "Description", "Type", "WalletId" },
                values: new object[] { 1, 100000m, new DateTime(2025, 7, 12, 15, 9, 23, 324, DateTimeKind.Utc).AddTicks(9565), "Nạp thử", "Deposit", 1 });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "DetailedAddress", "PropertyId", "ProvinceId", "StreetId", "WardId" },
                values: new object[,]
                {
                    { 1, "123", 1, 1, 1, 2 },
                    { 2, "456", 2, 1, 2, 3 },
                    { 3, "789", 3, 1, 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "AddressId", "Area", "Bedrooms", "CreatedAt", "Description", "IsVerified", "LandlordId", "Location", "Price", "Status", "Title", "Type" },
                values: new object[,]
                {
                    { 1, 1, 50.5m, 2, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9265), "Modern apartment with 2 bedrooms in the heart of HCMC.", true, 2, "10.7769,106.7009", 5000000m, "available", "2BR Apartment in District 1", "apartment" },
                    { 2, 2, 20.0m, 1, new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9270), "Cozy shared room for students.", true, 2, "10.8505,106.6737", 2000000m, "available", "Shared Room in Go Vap", "room" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "AddressId", "Area", "Bedrooms", "CreatedAt", "Description", "IsPromoted", "IsVerified", "LandlordId", "Location", "Price", "Status", "Title", "Type" },
                values: new object[] { 3, 3, 80.0m, 3, new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9273), "Spacious house with 3 bedrooms.", true, true, 2, "10.7982,106.6582", 8000000m, "available", "3BR House in Tan Binh", "house" });

            migrationBuilder.InsertData(
                table: "PropertyAmenities",
                columns: new[] { "AmenityId", "PropertyId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 2, 2 },
                    { 1, 3 },
                    { 4, 3 }
                });

            migrationBuilder.InsertData(
                table: "PropertyImages",
                columns: new[] { "Id", "IsPrimary", "Order", "PropertyId", "Url" },
                values: new object[] { 1, true, 1, 1, "https://example.com/apartment1.jpg" });

            migrationBuilder.InsertData(
                table: "PropertyImages",
                columns: new[] { "Id", "Order", "PropertyId", "Url" },
                values: new object[] { 2, 2, 1, "https://example.com/apartment2.jpg" });

            migrationBuilder.InsertData(
                table: "PropertyImages",
                columns: new[] { "Id", "IsPrimary", "Order", "PropertyId", "Url" },
                values: new object[,]
                {
                    { 3, true, 1, 2, "https://example.com/room1.jpg" },
                    { 4, true, 1, 3, "https://example.com/house1.jpg" }
                });

            migrationBuilder.InsertData(
                table: "PropertyPosts",
                columns: new[] { "Id", "ArchiveDate", "CreatedAt", "LandlordId", "PropertyId", "Status", "UpdatedAt", "VerifiedAt", "VerifiedBy" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9423), 2, 1, "Approved", null, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9424), 1 },
                    { 2, null, new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9427), 2, 2, "Approved", null, new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9428), 1 },
                    { 3, null, new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9430), 2, 3, "Approved", null, new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9431), 1 }
                });

            migrationBuilder.InsertData(
                table: "PropertyPromotions",
                columns: new[] { "Id", "EndDate", "PackageId", "PropertyId", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9792), 1, 1, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9791) },
                    { 2, new DateTime(2025, 8, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9795), 2, 2, new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9794) },
                    { 3, new DateTime(2025, 10, 8, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9797), 3, 3, new DateTime(2025, 7, 10, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9796) }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "IsApproved", "PropertyId", "Rating", "RenterId" },
                values: new object[] { 1, "Great location and clean apartment!", new DateTime(2025, 7, 11, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9602), true, 1, 4, 3 });

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
                table: "RentalContracts",
                columns: new[] { "Id", "ConfirmedAt", "ContactInfo", "ContractDurationMonths", "CreatedAt", "DepositAmount", "LandlordId", "MonthlyRent", "PaymentCycle", "PaymentDayOfMonth", "PaymentMethod", "PropertyPostId", "RenterId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9877), "renter@example.com | 03345678910", 12, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9876), 2000000m, 2, 5000000m, 0, 5, "Bank Transfer", 1, 3, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, null, "renter2@example.com | 0322222222", 6, new DateTime(2025, 7, 12, 22, 9, 23, 324, DateTimeKind.Local).AddTicks(9924), 1500000m, 2, 2000000m, 1, 10, "Momo", 2, 4, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 }
                });

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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_LandlordId",
                table: "Conversation",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_PropertyId",
                table: "Conversation",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_RenterId",
                table: "Conversation",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ConversationId",
                table: "Message",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SentAt",
                table: "Message",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AddressId",
                table: "Properties",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_LandlordId",
                table: "Properties",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAmenities_AmenityId",
                table: "PropertyAmenities",
                column: "AmenityId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImages_PropertyId",
                table: "PropertyImages",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPosts_LandlordId",
                table: "PropertyPosts",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPosts_PropertyId",
                table: "PropertyPosts",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPosts_VerifiedBy",
                table: "PropertyPosts",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPromotions_PackageId",
                table: "PropertyPromotions",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPromotions_PropertyId",
                table: "PropertyPromotions",
                column: "PropertyId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PropertyId",
                table: "Reviews",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RenterId",
                table: "Reviews",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_WardId",
                table: "Streets",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteProperties_PropertyId",
                table: "UserFavoriteProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteProperties_UserId_PropertyId",
                table: "UserFavoriteProperties",
                columns: new[] { "UserId", "PropertyId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId",
                table: "WalletTransactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_ProvinceId",
                table: "Wards",
                column: "ProvinceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "PropertyAmenities");

            migrationBuilder.DropTable(
                name: "PropertyImages");

            migrationBuilder.DropTable(
                name: "PropertyPromotions");

            migrationBuilder.DropTable(
                name: "RentalContracts");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "UserFavoriteProperties");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "promotionPackages");

            migrationBuilder.DropTable(
                name: "PropertyPosts");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Streets");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Provinces");
        }
    }
}
