using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class IntV2 : Migration
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
                    CitizenIdFrontImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitizenIdBackImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationRejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

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
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
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
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReportedByUserId = table.Column<int>(type: "int", nullable: false),
                    ReportedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResolvedByUserId = table.Column<int>(type: "int", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ReportedByUserId",
                        column: x => x.ReportedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ResolvedByUserId",
                        column: x => x.ResolvedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "NewsImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsImages_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayOSOrderCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckoutUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                name: "InterestedProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    RenterId = table.Column<int>(type: "int", nullable: false),
                    InterestedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RenterReplyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LandlordReplyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RenterConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LandlordConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestedProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestedProperties_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestedProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    RenterId = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsFlagged = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.CheckConstraint("CK_Review_Rating_Range", "Rating BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_RentalContracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "RentalContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    LandlordId = table.Column<int>(type: "int", nullable: false),
                    ReplyContent = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsFlagged = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewReplies_AspNetUsers_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewReplies_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
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
                columns: new[] { "Id", "AccessFailedCount", "CitizenIdBackImageUrl", "CitizenIdExpiryDate", "CitizenIdFrontImageUrl", "CitizenIdIssuedDate", "CitizenIdNumber", "ConcurrencyStamp", "ConfirmationCode", "ConfirmationCodeExpiry", "CreatedAt", "Email", "EmailConfirmed", "IsActive", "IsVerified", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName", "VerificationRejectReason", "VerificationStatus" },
                values: new object[,]
                {
                    { 1, 0, "https://example.com/cccd/admin_back.jpg", new DateTime(2030, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/admin_front.jpg", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "01234567890", "bac491d5-7817-4168-9a44-380b933b12b3", null, null, new DateTime(2025, 8, 8, 20, 40, 46, 324, DateTimeKind.Local).AddTicks(1949), "admin@example.com", true, true, true, false, null, "Admin User", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEEwvLDVbH5rbD9xX7qJMDuAQ3DM7sJUjxQdflZ2aUFoikyhCOyic8cK5oJfvUDTQ5w==", "+841234567891", true, null, null, null, "admin", "a0c30732-8dc7-47d0-b0e5-e14fc52fd55b", false, "admin@example.com", null, "none" },
                    { 2, 0, "https://example.com/cccd/landlord_back.jpg", new DateTime(2031, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/landlord_front.jpg", new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "002345678901", "99a4d35e-319d-487d-9678-2502bc798d15", null, null, new DateTime(2025, 8, 8, 20, 40, 46, 424, DateTimeKind.Local).AddTicks(1827), "MinhTri@example.com", true, true, true, false, null, "Minh Trisgei", "TRI@EXAMPLE.COM", "MINHTRI", "AQAAAAIAAYagAAAAELlutL0IeapWSupd5c+VChOIqfZv0rI4o7yEwIocC1oZIvYerCKBU4hrgwbtfGrFyA==", "+842345678910", true, "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0", null, null, "landlord", "fdb26fba-93ba-4467-8265-cd72bf68c4fd", false, "MinhTri", null, "none" },
                    { 3, 0, "https://example.com/cccd/renter_back.jpg", new DateTime(2032, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/renter_front.jpg", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "003456789012", "fdc48e71-5e78-439a-862e-a36d4497c576", null, null, new DateTime(2025, 8, 8, 20, 40, 46, 514, DateTimeKind.Local).AddTicks(6096), "Khanh@example.com", true, true, true, false, null, "Khanh", "KHANH@EXAMPLE.COM", "KHANH", "AQAAAAIAAYagAAAAEL08/yDUF3ez4SG44HiqJdv5O1cwLGbTf22BUlT1V4p92gfDYOmmki57eQAdadUuRg==", "+843345678910", true, null, null, null, "renter", "00fef329-1eb2-4171-a47a-196a325c1850", false, "Khanh", null, "none" },
                    { 4, 0, "https://example.com/cccd/renter2_back.jpg", new DateTime(2033, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/renter2_front.jpg", new DateTime(2023, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "004567890123", "c07b2aa3-74b7-4dcd-8cda-8e9c24b6aa03", null, null, new DateTime(2025, 8, 7, 20, 40, 46, 595, DateTimeKind.Local).AddTicks(3256), "renter2@example.com", true, false, true, false, null, "Duongkhmt", "RENTER2@EXAMPLE.COM", "DUONGKHMT", "AQAAAAIAAYagAAAAEPnrS7APxbLgKYQi1b8z33TQOAPn4D8E6ZE/3T9+k7gTXdji5Zv4Vun225KHYyN6BQ==", "+846574837475", true, null, null, null, "renter", "d7142926-3978-49ca-ae34-8303c66e617d", false, "Duongkhmt", "Ảnh CCCD mặt sau bị mờ, thiếu ngày cấp. Vui lòng bổ sung lại!", "none" },
                    { 5, 0, "https://example.com/cccd/landlord_back.jpg", new DateTime(2031, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/landlord_front.jpg", new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "002345678901", "d7db698b-6fe6-469e-8d11-9ad47bd8799f", null, null, new DateTime(2025, 8, 8, 20, 40, 46, 658, DateTimeKind.Local).AddTicks(9486), "Manh@example.com", true, true, true, false, null, "Manh home", "MANH@EXAMPLE.COM", "MANH", "AQAAAAIAAYagAAAAEFfEqEFuJRfmLzluOG1rJ57/HMtnYZT9kkWFKJeXnBpCTTG4c7y6jjzxTxtFuj4JgA==", "+840987654567", true, "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0", null, null, "landlord", "7103703f-763d-42dc-a81b-667e57a02468", false, "Manh", null, "none" },
                    { 6, 0, "https://example.com/cccd/landlord_back.jpg", new DateTime(2031, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/cccd/landlord_front.jpg", new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "002345678901", "411a0322-20fc-4e1c-a7c9-f9fff4a95cf2", null, null, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5156), "Tadong@example.com", true, true, true, false, null, "DongAUTO", "TADONG@EXAMPLE.COM", "DONGVT", "AQAAAAIAAYagAAAAEOV9DK+sMBfdsRkH/t35/GgrilLaQUegOpjRYlyTgNLq9YKe2s/PWbManWi3hR7P1g==", "+843541234567", true, "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0", null, null, "landlord", "4c061a76-fd37-4a3d-95e9-88010d6689cf", false, "DongVT", null, "none" }
                });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "AuthorName", "Content", "CreatedAt", "IsPublished", "PublishedAt", "Slug", "Source", "Summary", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Admin", "Dưới đây là 5 điều bạn nên cân nhắc khi thuê nhà trọ...", new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "5-luu-y-khi-thue-nha-tro-tai-tphcm", null, "Các lưu ý quan trọng khi thuê trọ tại TP.HCM", "5 lưu ý khi thuê nhà trọ tại TP.HCM", null },
                    { 2, "AI Bot", "Giá thuê nhà ở hai thành phố lớn có sự chênh lệch như thế nào...", new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "so-sanh-gia-thue-nha-giua-tphcm-va-ha-noi", "Vietnamnet.vn", "Giá thuê nhà giữa TP.HCM và Hà Nội có gì khác biệt?", "So sánh giá thuê nhà giữa TP.HCM và Hà Nội", null },
                    { 3, "Nguyen Van A", "Một số mẹo giúp bạn chọn khu trọ an toàn và tiện nghi tại Hà Nội...", new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "meo-chon-khu-tro-an-toan-tai-ha-noi", "TuoiTre.vn", "Hướng dẫn chọn khu trọ an toàn tại Hà Nội", "Mẹo chọn khu trọ an toàn tại Hà Nội", null },
                    { 4, "Tran Thi B", "Các xu hướng mới trong thị trường thuê nhà tại Việt Nam năm 2025...", new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "xu-huong-thue-nha-nam-2025", "VTV.vn", "Tìm hiểu xu hướng thuê nhà trong năm nay", "Xu hướng thuê nhà năm 2025", null },
                    { 5, "Le Van C", "Những điều cần biết khi ký hợp đồng thuê nhà để tránh rủi ro...", new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "huong-dan-ky-hop-dong-thue-nha", null, "Hướng dẫn chi tiết về ký hợp đồng thuê nhà", "Hướng dẫn ký hợp đồng thuê nhà", null },
                    { 6, "Pham Thi D", "Thuê nhà dài hạn mang lại nhiều lợi ích cho người thuê và chủ nhà...", new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "loi-ich-cua-viec-thue-nha-dai-han", "ThanhNien.vn", "Tìm hiểu lợi ích khi thuê nhà dài hạn", "Lợi ích của việc thuê nhà dài hạn", null },
                    { 7, "Hoang Van E", "Danh sách các khu trọ giá rẻ và chất lượng tại Đà Nẵng năm 2025...", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "top-5-khu-tro-gia-re-tai-da-nang", "NguoiLaoDong.vn", "Khám phá 5 khu trọ giá rẻ tại Đà Nẵng", "Top 5 khu trọ giá rẻ tại Đà Nẵng", null }
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
                    { 1, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6405), "Basic promotion package for property listings.", 30, true, 1, "Basic Promotion", 10000m, null },
                    { 2, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6407), "Premium promotion package for property listings.", 60, true, 2, "Premium Promotion", 400000m, null },
                    { 3, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6409), "Ultimate promotion package for property listings.", 90, true, 3, "Ultimate Promotion", 50000m, null }
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
                table: "NewsImages",
                columns: new[] { "Id", "ImageUrl", "IsPrimary", "NewsId", "Order" },
                values: new object[,]
                {
                    { 1, "/uploads/news/news1-img1.jpg", false, 1, 0 },
                    { 2, "/uploads/news/news2-img1.jpg", false, 2, 0 }
                });

            migrationBuilder.InsertData(
                table: "UserPreferences",
                columns: new[] { "Id", "Amenities", "CreatedAt", "Location", "PriceRangeMax", "PriceRangeMin", "UserId" },
                values: new object[,]
                {
                    { 1, "WiFi,Parking", new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5702), "District 1", 6000000m, 3000000m, 3 },
                    { 2, "WiFi", new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5709), "Go Vap", 3000000m, 1500000m, 3 },
                    { 3, "AC", new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5716), "Tan Binh", 4000000m, 2000000m, 4 }
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "Balance", "CreatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 1000000m, new DateTime(2025, 8, 8, 13, 40, 46, 727, DateTimeKind.Utc).AddTicks(6281), 2 },
                    { 2, 500000m, new DateTime(2025, 8, 8, 13, 40, 46, 727, DateTimeKind.Utc).AddTicks(6283), 5 },
                    { 3, 500000m, new DateTime(2025, 8, 8, 13, 40, 46, 727, DateTimeKind.Utc).AddTicks(6284), 6 }
                });

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
                columns: new[] { "Id", "Amount", "CheckoutUrl", "CreatedAt", "Description", "PaidAt", "PayOSOrderCode", "Status", "Type", "WalletId" },
                values: new object[] { 1, 100000m, "", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nạp thử", null, "", "Success", "Deposit", 1 });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "DetailedAddress", "PropertyId", "ProvinceId", "StreetId", "WardId" },
                values: new object[,]
                {
                    { 1, "123", 1, 1, 1, 2 },
                    { 2, "456", 2, 1, 2, 3 },
                    { 3, "789", 3, 1, 3, 4 },
                    { 4, "101", 4, 1, 4, 5 },
                    { 5, "202", 5, 1, 5, 6 },
                    { 6, "303", 6, 1, 6, 7 },
                    { 7, "404", 7, 1, 7, 8 },
                    { 8, "505", 8, 1, 8, 9 },
                    { 9, "606", 9, 1, 9, 10 },
                    { 10, "707", 10, 2, 10, 11 },
                    { 11, "808", 11, 2, 11, 12 },
                    { 12, "909", 12, 2, 12, 13 },
                    { 13, "111", 13, 2, 13, 14 },
                    { 14, "222", 14, 2, 14, 15 },
                    { 15, "333", 15, 2, 15, 16 },
                    { 16, "444", 16, 2, 16, 17 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "AddressId", "Area", "Bedrooms", "CreatedAt", "Description", "IsVerified", "LandlordId", "Location", "Price", "Status", "Title", "Type" },
                values: new object[,]
                {
                    { 1, 1, 50.5m, 2, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5903), "Modern apartment with 2 bedrooms in the heart of HCMC.", true, 2, "10.7769,106.7009", 5000000m, "available", "2BR Apartment in District 1", "apartment" },
                    { 2, 2, 20.0m, 1, new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5907), "Cozy shared room for students.", true, 2, "10.8505,106.6737", 2000000m, "available", "Shared Room in Go Vap", "room" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "AddressId", "Area", "Bedrooms", "CreatedAt", "Description", "IsPromoted", "IsVerified", "LandlordId", "Location", "Price", "Status", "Title", "Type" },
                values: new object[,]
                {
                    { 3, 3, 80.0m, 3, new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(5909), "Spacious house with 3 bedrooms.", true, true, 2, "10.7982,106.6582", 8000000m, "available", "3BR House in Tan Binh", "house" },
                    { 4, 4, 35.0m, 1, new DateTime(2025, 8, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6015), "High-end studio apartment with modern amenities and city view.", true, true, 5, "10.7769,106.7009", 6500000m, "available", "Luxury Studio in District 1", "studio" },
                    { 5, 5, 120.0m, 4, new DateTime(2025, 8, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6017), "Beautiful villa with garden, perfect for families.", true, true, 5, "10.7308,106.7267", 15000000m, "available", "Family Villa in District 7", "villa" },
                    { 6, 6, 15.0m, 1, new DateTime(2025, 8, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6020), "Affordable dormitory for students near universities.", true, true, 6, "10.7829,106.6889", 1500000m, "available", "Student Dormitory in District 3", "dormitory" },
                    { 7, 7, 150.0m, 3, new DateTime(2025, 8, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6022), "Luxury penthouse with panoramic city views.", true, true, 6, "10.8105,106.7091", 25000000m, "available", "Penthouse in Binh Thanh", "penthouse" },
                    { 8, 8, 45.0m, 2, new DateTime(2025, 8, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6024), "Well-maintained apartment in quiet neighborhood.", true, true, 6, "10.7947,106.6789", 5500000m, "available", "Cozy Apartment in Phu Nhuan", "apartment" },
                    { 9, 9, 60.0m, 2, new DateTime(2025, 7, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6027), "Industrial-style loft with high ceilings and open space.", true, true, 2, "10.7871,106.7492", 7500000m, "available", "Modern Loft in District 2", "loft" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "AddressId", "Area", "Bedrooms", "CreatedAt", "Description", "IsVerified", "LandlordId", "Location", "Price", "Status", "Title", "Type" },
                values: new object[,]
                {
                    { 10, 10, 70.0m, 3, new DateTime(2025, 7, 30, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6030), "Charming traditional Vietnamese house in historic area.", true, 2, "21.0285,105.8542", 6000000m, "available", "Traditional House in Hanoi Old Quarter", "house" },
                    { 11, 11, 25.0m, 1, new DateTime(2025, 7, 29, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6032), "Compact studio perfect for young professionals.", true, 5, "21.0352,105.8342", 3500000m, "available", "Studio in Ba Dinh District", "studio" },
                    { 12, 12, 40.0m, 2, new DateTime(2025, 7, 28, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6034), "Furnished shared apartment with utilities included.", true, 5, "21.0122,105.8441", 4000000m, "available", "Shared Apartment in Hai Ba Trung", "apartment" },
                    { 13, 13, 18.0m, 1, new DateTime(2025, 7, 27, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6036), "Budget-friendly room for students near universities.", true, 5, "21.0188,105.8292", 1800000m, "available", "Student Room in Dong Da", "room" },
                    { 14, 14, 85.0m, 3, new DateTime(2025, 7, 26, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6038), "Spacious apartment suitable for families with children.", true, 6, "21.0367,105.7826", 7000000m, "available", "Family Apartment in Cau Giay", "apartment" },
                    { 15, 15, 200.0m, 4, new DateTime(2025, 7, 25, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6041), "Beautiful villa with lake view and private garden.", true, 6, "21.0811,105.8144", 20000000m, "available", "Lakeside Villa in Tay Ho", "villa" },
                    { 16, 16, 55.0m, 2, new DateTime(2025, 7, 24, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6043), "Newly built condo with modern amenities and security.", true, 2, "21.0455,105.8952", 6500000m, "available", "Modern Condo in Long Bien", "condo" }
                });

            migrationBuilder.InsertData(
                table: "InterestedProperties",
                columns: new[] { "Id", "InterestedAt", "LandlordReplyAt", "PropertyId", "RenterId", "RenterReplyAt", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6936), null, 1, 3, null, 1 },
                    { 2, new DateTime(2025, 8, 8, 8, 40, 46, 727, DateTimeKind.Local).AddTicks(6942), null, 2, 4, new DateTime(2025, 8, 8, 10, 40, 46, 727, DateTimeKind.Local).AddTicks(6943), 2 },
                    { 3, new DateTime(2025, 8, 8, 15, 40, 46, 727, DateTimeKind.Local).AddTicks(6944), null, 3, 1, new DateTime(2025, 8, 8, 17, 40, 46, 727, DateTimeKind.Local).AddTicks(6945), 3 }
                });

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
                    { 1, null, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6196), 2, 1, "Approved", null, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6196), 1 },
                    { 2, null, new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6198), 2, 2, "Approved", null, new DateTime(2025, 8, 7, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6199), 1 },
                    { 3, null, new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6200), 2, 3, "Approved", null, new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6201), 1 },
                    { 4, null, new DateTime(2025, 8, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6202), 5, 4, "Approved", null, new DateTime(2025, 8, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6208), 1 },
                    { 5, null, new DateTime(2025, 8, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6213), 5, 5, "Approved", null, new DateTime(2025, 8, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6213), 1 },
                    { 6, null, new DateTime(2025, 8, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6215), 6, 6, "Approved", null, new DateTime(2025, 8, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6215), 1 },
                    { 7, null, new DateTime(2025, 8, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6217), 6, 7, "Approved", null, new DateTime(2025, 8, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6217), 1 },
                    { 8, null, new DateTime(2025, 8, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6223), 6, 8, "Approved", null, new DateTime(2025, 8, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6223), 1 },
                    { 9, null, new DateTime(2025, 7, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6225), 2, 9, "Approved", null, new DateTime(2025, 7, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6225), 1 },
                    { 10, null, new DateTime(2025, 7, 30, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6226), 2, 10, "Approved", null, new DateTime(2025, 7, 30, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6227), 1 },
                    { 11, null, new DateTime(2025, 7, 29, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6228), 5, 11, "Approved", null, new DateTime(2025, 7, 29, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6229), 1 },
                    { 12, null, new DateTime(2025, 7, 28, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6230), 5, 12, "Approved", null, new DateTime(2025, 7, 28, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6230), 1 },
                    { 13, null, new DateTime(2025, 7, 27, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6232), 5, 13, "Approved", null, new DateTime(2025, 7, 27, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6232), 1 },
                    { 14, null, new DateTime(2025, 7, 26, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6234), 6, 14, "Approved", null, new DateTime(2025, 7, 26, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6234), 1 },
                    { 15, null, new DateTime(2025, 7, 25, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6235), 6, 15, "Approved", null, new DateTime(2025, 7, 25, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6236), 1 },
                    { 16, null, new DateTime(2025, 7, 24, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6237), 2, 16, "Approved", null, new DateTime(2025, 7, 24, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6237), 1 }
                });

            migrationBuilder.InsertData(
                table: "PropertyPromotions",
                columns: new[] { "Id", "EndDate", "PackageId", "PropertyId", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6447), 1, 3, new DateTime(2025, 8, 6, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6446) },
                    { 2, new DateTime(2025, 9, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6449), 2, 4, new DateTime(2025, 8, 5, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6449) },
                    { 3, new DateTime(2025, 9, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6451), 3, 5, new DateTime(2025, 8, 4, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6450) },
                    { 4, new DateTime(2025, 9, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6453), 1, 6, new DateTime(2025, 8, 3, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6452) },
                    { 5, new DateTime(2025, 9, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6454), 2, 7, new DateTime(2025, 8, 2, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6454) },
                    { 6, new DateTime(2025, 8, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6456), 3, 8, new DateTime(2025, 8, 1, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6455) },
                    { 7, new DateTime(2025, 8, 30, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6457), 1, 9, new DateTime(2025, 7, 31, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6457) }
                });

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
                    { 1, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6499), "renter@example.com | 03345678910", 12, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6499), 2000000m, 2, 5000000m, 0, 5, "Bank Transfer", 1, 3, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, null, "renter2@example.com | 0322222222", 6, new DateTime(2025, 8, 8, 20, 40, 46, 727, DateTimeKind.Local).AddTicks(6504), 1500000m, 2, 2000000m, 1, 10, "Momo", 2, 4, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "ApplicationUserId", "ContractId", "CreatedAt", "IsVisible", "PropertyId", "Rating", "RenterId", "ReviewText" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2025, 7, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 5, 3, "Căn hộ rất sạch sẽ, chủ nhà thân thiện. Sẽ giới thiệu bạn bè!" },
                    { 2, null, 2, new DateTime(2025, 8, 5, 15, 30, 0, 0, DateTimeKind.Unspecified), true, 2, 4, 4, "Giá hợp lý, vị trí thuận tiện. Chủ nhà hỗ trợ tốt." }
                });

            migrationBuilder.InsertData(
                table: "ReviewReplies",
                columns: new[] { "Id", "CreatedAt", "IsVisible", "LandlordId", "ReplyContent", "ReviewId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 16, 9, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ của chúng tôi!", 1 },
                    { 2, new DateTime(2025, 8, 6, 8, 30, 0, 0, DateTimeKind.Unspecified), true, 2, "Cảm ơn bạn đã phản hồi tích cực. Chúc bạn luôn vui vẻ!", 2 }
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
                name: "IX_ApplicationUserNotifications_UserId",
                table: "ApplicationUserNotifications",
                column: "UserId");

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
                name: "IX_InterestedProperties_PropertyId_RenterId",
                table: "InterestedProperties",
                columns: new[] { "PropertyId", "RenterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterestedProperties_RenterId",
                table: "InterestedProperties",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestedProperties_Status",
                table: "InterestedProperties",
                column: "Status");

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
                name: "IX_NewsImages_NewsId",
                table: "NewsImages",
                column: "NewsId");

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
                name: "IX_Reports_ReportedByUserId",
                table: "Reports",
                column: "ReportedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ResolvedByUserId",
                table: "Reports",
                column: "ResolvedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReplies_LandlordId",
                table: "ReviewReplies",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReplies_ReviewId",
                table: "ReviewReplies",
                column: "ReviewId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ContractId",
                table: "Reviews",
                column: "ContractId",
                unique: true);

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
                name: "ApplicationUserNotifications");

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
                name: "InterestedProperties");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "NewsImages");

            migrationBuilder.DropTable(
                name: "PropertyAmenities");

            migrationBuilder.DropTable(
                name: "PropertyImages");

            migrationBuilder.DropTable(
                name: "PropertyPromotions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "ReviewReplies");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "UserFavoriteProperties");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "promotionPackages");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "RentalContracts");

            migrationBuilder.DropTable(
                name: "PropertyPosts");

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
