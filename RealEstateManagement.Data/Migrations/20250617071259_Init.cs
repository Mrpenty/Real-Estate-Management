using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstateManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotionPackages", x => x.Id);
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
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_AspNetUsers_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPreferences_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransactionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
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
                name: "UserPreferenceFavoriteProperties",
                columns: table => new
                {
                    UserPreferenceId = table.Column<int>(type: "int", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferenceFavoriteProperties", x => new { x.UserPreferenceId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_UserPreferenceFavoriteProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPreferenceFavoriteProperties_UserPreferences_UserPreferenceId",
                        column: x => x.UserPreferenceId,
                        principalTable: "UserPreferences",
                        principalColumn: "Id");
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
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "ConfirmationCode", "ConfirmationCodeExpiry", "CreatedAt", "Email", "EmailConfirmed", "IsVerified", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "0c65b431-763a-4970-b982-572224ee09f2", null, null, new DateTime(2025, 6, 17, 14, 12, 57, 975, DateTimeKind.Local).AddTicks(7817), "admin@example.com", false, true, false, null, "Admin User", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAECMgx/zqlGC/sCjv9u+70aQK4B2Fj9dguM1UDmI3OL87bGDJ21yEla1ZEQFVvgA7zw==", "12345678910", true, null, null, null, "admin", "f77827af-b2a5-4372-a835-03b46fdea8ff", false, "admin@example.com" },
                    { 2, 0, "f0750c33-bcf5-4b94-b901-884e7859e7c7", null, null, new DateTime(2025, 6, 17, 14, 12, 58, 58, DateTimeKind.Local).AddTicks(5855), "landlord@example.com", false, true, false, null, "Landlord User", "LANDLORD@EXAMPLE.COM", "LANDLORD@EXAMPLE.COM", "AQAAAAIAAYagAAAAEEG1+lssUZPHcxx7fNeThSxrVxAHQ/CEyp+2pSd+EZNLjwFw/7k9mVTyrq5RyPH5XA==", "02345678910", true, "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0", null, null, "landlord", "564a05b6-cb63-40fd-a9a9-1afb6432b255", false, "landlord@example.com" },
                    { 3, 0, "be3b5d19-3353-4ada-92c3-fd5f2f637ee2", null, null, new DateTime(2025, 6, 17, 14, 12, 58, 138, DateTimeKind.Local).AddTicks(3363), "renter@example.com", false, true, false, null, "Renter User", "RENTER@EXAMPLE.COM", "RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEJ0w5Ph1aKCaS5QeZSRePRUibXci+pO0Lj9fAAQecK3J+RvRaMM+ocAQ9FjfT7oJEw==", "03345678910", true, null, null, null, "renter", "acf3adaa-e204-49ac-b3a3-b2f4887433a6", false, "renter@example.com" },
                    { 4, 0, "22a9cd9a-1608-4df0-b288-e32c88a2d55a", null, null, new DateTime(2025, 6, 16, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(5817), "renter2@example.com", false, true, false, null, "Renter User 2", "RENTER2@EXAMPLE.COM", "RENTER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEAZpyw8BBEw19jv5N6Jfy616cQn43nstY37tiRhsaFZm2/oXYbdUZ9hwtNN3DzUplw==", null, false, null, null, null, "renter", "603e4fca-ea49-44e4-8671-c0e3a3eeafcc", false, "renter2@example.com" }
                });

            migrationBuilder.InsertData(
                table: "promotionPackages",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationInDays", "IsActive", "Level", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7285), "Basic promotion package for property listings.", 30, true, 1, "Basic Promotion", 1000000m, null },
                    { 2, new DateTime(2025, 6, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7288), "Premium promotion package for property listings.", 60, true, 2, "Premium Promotion", 2000000m, null },
                    { 3, new DateTime(2025, 6, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7291), "Ultimate promotion package for property listings.", 90, true, 3, "Ultimate Promotion", 3000000m, null }
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
                table: "Properties",
                columns: new[] { "Id", "Address", "Area", "Bedrooms", "CreatedAt", "Description", "IsVerified", "LandlordId", "Location", "Price", "Status", "Title", "Type" },
                values: new object[,]
                {
                    { 1, "123 Nguyen Hue, District 1, HCMC", 50.5m, 2, new DateTime(2025, 6, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(6778), "Modern apartment with 2 bedrooms in the heart of HCMC.", true, 2, "10.7769,106.7009", 5000000m, "available", "2BR Apartment in District 1", "apartment" },
                    { 2, "456 Le Van Tho, Go Vap, HCMC", 20.0m, 1, new DateTime(2025, 6, 16, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(6784), "Cozy shared room for students.", true, 2, "10.8505,106.6737", 2000000m, "available", "Shared Room in Go Vap", "room" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Area", "Bedrooms", "CreatedAt", "Description", "IsPromoted", "IsVerified", "LandlordId", "Location", "Price", "Status", "Title", "Type" },
                values: new object[] { 3, "789 Ly Thuong Kiet, Tan Binh, HCMC", 80.0m, 3, new DateTime(2025, 6, 15, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(6794), "Spacious house with 3 bedrooms.", true, true, 2, "10.7982,106.6582", 8000000m, "available", "3BR House in Tan Binh", "house" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CreatedAt", "Description", "TransactionType", "UserId" },
                values: new object[] { 1, 5000000m, new DateTime(2025, 6, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7135), "Deposit for apartment in District 1", "deposit", 3 });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "ContractId", "PaidAt", "PaymentMethod", "Status", "TransactionId" },
                values: new object[] { 1, 5000000m, 1, new DateTime(2025, 6, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7068), "Momo", "completed", 1 });

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
                    { 1, null, new DateTime(2025, 6, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7014), 2, 1, "Approved", null, new DateTime(2025, 6, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7015), 1 },
                    { 2, null, new DateTime(2025, 6, 16, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7021), 2, 2, "Approved", null, new DateTime(2025, 6, 16, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7022), 1 },
                    { 3, null, new DateTime(2025, 6, 15, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7024), 2, 3, "Approved", null, new DateTime(2025, 6, 15, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7025), 1 }
                });

            migrationBuilder.InsertData(
                table: "PropertyPromotions",
                columns: new[] { "Id", "EndDate", "PackageId", "PropertyId", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7338), 1, 1, new DateTime(2025, 6, 17, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7336) },
                    { 2, new DateTime(2025, 7, 16, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7340), 2, 2, new DateTime(2025, 6, 16, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7340) },
                    { 3, new DateTime(2025, 9, 13, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7343), 3, 3, new DateTime(2025, 6, 15, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7342) }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "IsApproved", "PropertyId", "Rating", "RenterId" },
                values: new object[] { 1, "Great location and clean apartment!", new DateTime(2025, 6, 16, 14, 12, 58, 245, DateTimeKind.Local).AddTicks(7192), true, 1, 4, 3 });

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
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId");

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
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferenceFavoriteProperties_PropertyId",
                table: "UserPreferenceFavoriteProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferenceFavoriteProperties_UserPreferenceId_PropertyId",
                table: "UserPreferenceFavoriteProperties",
                columns: new[] { "UserPreferenceId", "PropertyId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_PropertyId",
                table: "UserPreferences",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId");
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
                name: "Payments");

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
                name: "UserPreferenceFavoriteProperties");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "promotionPackages");

            migrationBuilder.DropTable(
                name: "PropertyPosts");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
