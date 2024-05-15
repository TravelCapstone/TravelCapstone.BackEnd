using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddSalaryHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Assurances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayMOQ = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assurances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRouteTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRouteTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DietaryPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DishTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FixDriverSalary = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MOQ = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacilityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JoinTourStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinTourStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OptionClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OptionQuotationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionQuotationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PortTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrivateTourStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateTourStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lng = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReferencePriceRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferencePriceRating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAvailabilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAdult = table.Column<bool>(type: "bit", nullable: false),
                    Money = table.Column<double>(type: "float", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsVerfiedPhoneNumber = table.Column<bool>(type: "bit", nullable: false),
                    IsVerifiedEmail = table.Column<bool>(type: "bit", nullable: false),
                    VerficationCodePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerficationCodeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TourGuideSalaryHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourGuideSalaryHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourGuideSalaryHistories_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssurancePriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    AssuranceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssurancePriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssurancePriceHistories_Assurances_AssuranceId",
                        column: x => x.AssuranceId,
                        principalTable: "Assurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DishTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_DishTypes_DishTypeId",
                        column: x => x.DishTypeId,
                        principalTable: "DishTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DriverSalaryHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverSalaryHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverSalaryHistories_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EventDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventDetails_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FacilityRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityTypeId = table.Column<int>(type: "int", nullable: false),
                    RatingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityRatings_FacilityTypes_FacilityTypeId",
                        column: x => x.FacilityTypeId,
                        principalTable: "FacilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FacilityRatings_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    PricePerAdult = table.Column<double>(type: "float", nullable: false),
                    PricePerChild = table.Column<double>(type: "float", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TourTypeId = table.Column<int>(type: "int", nullable: false),
                    QRCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TourStatusId = table.Column<int>(type: "int", nullable: false),
                    BasedOnTourId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tours_Tours_BasedOnTourId",
                        column: x => x.BasedOnTourId,
                        principalTable: "Tours",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tours_TourStatuses_TourStatusId",
                        column: x => x.TourStatusId,
                        principalTable: "TourStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tours_TourTypes_TourTypeId",
                        column: x => x.TourTypeId,
                        principalTable: "TourTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tours_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    SeatCapacity = table.Column<int>(type: "int", nullable: false),
                    EngineNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChassisNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    TravelCompanionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Customers_TravelCompanionId",
                        column: x => x.TravelCompanionId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EventDetailPriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    EventDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDetailPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventDetailPriceHistories_EventDetails_EventDetailId",
                        column: x => x.EventDetailId,
                        principalTable: "EventDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Communes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communes_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TourguideScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourguideScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourguideScopes_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TourguideScopes_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumOfChild = table.Column<int>(type: "int", nullable: false),
                    NumOfAdult = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Deposit = table.Column<double>(type: "float", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfContract = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractStatus = table.Column<int>(type: "int", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Contracts_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_AspNetUsers_UpdateBy",
                        column: x => x.UpdateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DayPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayPlans_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MaterialAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialAssignments_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MaterialAssignments_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PrivateJoinTourRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TravelCompanionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumOfAdult = table.Column<int>(type: "int", nullable: false),
                    NumOfChildren = table.Column<int>(type: "int", nullable: false),
                    JoinTourStatusId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateJoinTourRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateJoinTourRequests_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrivateJoinTourRequests_AspNetUsers_UpdateBy",
                        column: x => x.UpdateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivateJoinTourRequests_Customers_TravelCompanionId",
                        column: x => x.TravelCompanionId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrivateJoinTourRequests_JoinTourStatuses_JoinTourStatusId",
                        column: x => x.JoinTourStatusId,
                        principalTable: "JoinTourStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrivateJoinTourRequests_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TourguideAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourGuideSalary = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourguideAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourguideAssignments_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TourguideAssignments_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TourguideAssignments_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TourRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PresenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourRegistrations_Customers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TourRegistrations_Customers_PresenterId",
                        column: x => x.PresenterId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TourRegistrations_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TourTravellers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTravellers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourTravellers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TourTravellers_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommunceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facilities_Communes_CommunceId",
                        column: x => x.CommunceId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Facilities_FacilityRatings_FacilityRatingId",
                        column: x => x.FacilityRatingId,
                        principalTable: "FacilityRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Facilities_ServiceProviders_ServiceProviderId",
                        column: x => x.ServiceProviderId,
                        principalTable: "ServiceProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortType = table.Column<int>(type: "int", nullable: false),
                    CommuneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ports_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PrivateTourRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumOfDay = table.Column<int>(type: "int", nullable: false),
                    NumOfNight = table.Column<int>(type: "int", nullable: false),
                    NumOfAdult = table.Column<int>(type: "int", nullable: false),
                    NumOfChildren = table.Column<int>(type: "int", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivateTourStatusId = table.Column<int>(type: "int", nullable: false),
                    IsEnterprise = table.Column<bool>(type: "bit", nullable: false),
                    RecommendedTourUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumHotelRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SuggestedTourguideName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumRestaurantRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WishPrice = table.Column<double>(type: "float", nullable: false),
                    DietaryPreferenceId = table.Column<int>(type: "int", nullable: false),
                    StartLocationCommuneId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MainDestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateTourRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateTourRequests_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrivateTourRequests_AspNetUsers_UpdateBy",
                        column: x => x.UpdateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivateTourRequests_Communes_StartLocationCommuneId",
                        column: x => x.StartLocationCommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivateTourRequests_DietaryPreferences_DietaryPreferenceId",
                        column: x => x.DietaryPreferenceId,
                        principalTable: "DietaryPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrivateTourRequests_FacilityRatings_MinimumHotelRatingId",
                        column: x => x.MinimumHotelRatingId,
                        principalTable: "FacilityRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrivateTourRequests_FacilityRatings_MinimumRestaurantRatingId",
                        column: x => x.MinimumRestaurantRatingId,
                        principalTable: "FacilityRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrivateTourRequests_PrivateTourStatuses_PrivateTourStatusId",
                        column: x => x.PrivateTourStatusId,
                        principalTable: "PrivateTourStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrivateTourRequests_Provinces_MainDestinationId",
                        column: x => x.MainDestinationId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrivateTourRequests_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FacilityServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServingQuantity = table.Column<int>(type: "int", nullable: false),
                    SurchargePercent = table.Column<double>(type: "float", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceAvailabilityId = table.Column<int>(type: "int", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityServices_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FacilityServices_ServiceAvailabilities_ServiceAvailabilityId",
                        column: x => x.ServiceAvailabilityId,
                        principalTable: "ServiceAvailabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FacilityServices_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FacilityServices_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceTransportPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArrivalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdultPrice = table.Column<double>(type: "float", nullable: false),
                    ChildPrice = table.Column<double>(type: "float", nullable: false),
                    ReferencePriceRatingId = table.Column<int>(type: "int", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceTransportPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceTransportPrices_Ports_ArrivalId",
                        column: x => x.ArrivalId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ReferenceTransportPrices_Ports_DepartureId",
                        column: x => x.DepartureId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ReferenceTransportPrices_ReferencePriceRating_ReferencePriceRatingId",
                        column: x => x.ReferencePriceRatingId,
                        principalTable: "ReferencePriceRating",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EndPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PortStartPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PortEndPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentRouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_DayPlans_DayPlanId",
                        column: x => x.DayPlanId,
                        principalTable: "DayPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Routes_Facilities_EndPointId",
                        column: x => x.EndPointId,
                        principalTable: "Facilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Routes_Facilities_StartPointId",
                        column: x => x.StartPointId,
                        principalTable: "Facilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Routes_Ports_PortEndPointId",
                        column: x => x.PortEndPointId,
                        principalTable: "Ports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Routes_Ports_PortStartPointId",
                        column: x => x.PortStartPointId,
                        principalTable: "Ports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Routes_Routes_ParentRouteId",
                        column: x => x.ParentRouteId,
                        principalTable: "Routes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OptionQuotations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OptionClassId = table.Column<int>(type: "int", nullable: false),
                    MinTotal = table.Column<double>(type: "float", nullable: false),
                    MaxTotal = table.Column<double>(type: "float", nullable: false),
                    OptionQuotationStatusId = table.Column<int>(type: "int", nullable: false),
                    PrivateTourRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssurancePriceHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionQuotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionQuotations_AssurancePriceHistories_AssurancePriceHistoryId",
                        column: x => x.AssurancePriceHistoryId,
                        principalTable: "AssurancePriceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OptionQuotations_OptionClasses_OptionClassId",
                        column: x => x.OptionClassId,
                        principalTable: "OptionClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OptionQuotations_OptionQuotationStatuses_OptionQuotationStatusId",
                        column: x => x.OptionQuotationStatusId,
                        principalTable: "OptionQuotationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OptionQuotations_PrivateTourRequests_PrivateTourRequestId",
                        column: x => x.PrivateTourRequestId,
                        principalTable: "PrivateTourRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    NumOfAdult = table.Column<int>(type: "int", nullable: false),
                    NumOfChildren = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivateTourRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_PrivateTourRequests_PrivateTourRequestId",
                        column: x => x.PrivateTourRequestId,
                        principalTable: "PrivateTourRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestedLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivateTourRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestedLocations_PrivateTourRequests_PrivateTourRequestId",
                        column: x => x.PrivateTourRequestId,
                        principalTable: "PrivateTourRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RequestedLocations_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DietaryPreferenceId = table.Column<int>(type: "int", nullable: false),
                    MealTypeId = table.Column<int>(type: "int", nullable: false),
                    FacilityServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Menus_AspNetUsers_UpdateBy",
                        column: x => x.UpdateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Menus_DietaryPreferences_DietaryPreferenceId",
                        column: x => x.DietaryPreferenceId,
                        principalTable: "DietaryPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Menus_FacilityServices_FacilityServiceId",
                        column: x => x.FacilityServiceId,
                        principalTable: "FacilityServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Menus_MealTypes_MealTypeId",
                        column: x => x.MealTypeId,
                        principalTable: "MealTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TransportServiceDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServingQuantity = table.Column<int>(type: "int", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    FacilityServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportServiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportServiceDetails_FacilityServices_FacilityServiceId",
                        column: x => x.FacilityServiceId,
                        principalTable: "FacilityServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TransportServiceDetails_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRoutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceRouteTypeId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRoutes_AttendanceRouteTypes_AttendanceRouteTypeId",
                        column: x => x.AttendanceRouteTypeId,
                        principalTable: "AttendanceRouteTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AttendanceRoutes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "VehicleRoutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceBrandName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleRoutes_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleRoutes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VehicleRoutes_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VehicleQuotationDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    NumOfRentingDay = table.Column<int>(type: "int", nullable: false),
                    StartPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartPointDistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EndPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EndPointDistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MinPrice = table.Column<double>(type: "float", nullable: false),
                    MaxPrice = table.Column<double>(type: "float", nullable: false),
                    NumOfVehicle = table.Column<int>(type: "int", nullable: false),
                    OptionQuotationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleQuotationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleQuotationDetails_Districts_EndPointDistrictId",
                        column: x => x.EndPointDistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleQuotationDetails_Districts_StartPointDistrictId",
                        column: x => x.StartPointDistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleQuotationDetails_OptionQuotations_OptionQuotationId",
                        column: x => x.OptionQuotationId,
                        principalTable: "OptionQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VehicleQuotationDetails_Provinces_EndPointId",
                        column: x => x.EndPointId,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleQuotationDetails_Provinces_StartPointId",
                        column: x => x.StartPointId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MenuDishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuDishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MenuDishes_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "QuotationDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityOfAdult = table.Column<int>(type: "int", nullable: false),
                    QuantityOfChild = table.Column<int>(type: "int", nullable: false),
                    FacilityRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    ServingQuantity = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinPrice = table.Column<double>(type: "float", nullable: false),
                    MaxPrice = table.Column<double>(type: "float", nullable: false),
                    MinRedundancyCost = table.Column<double>(type: "float", nullable: false),
                    MaxRedundancyCost = table.Column<double>(type: "float", nullable: false),
                    OptionQuotationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationDetails_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuotationDetails_FacilityRatings_FacilityRatingId",
                        column: x => x.FacilityRatingId,
                        principalTable: "FacilityRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuotationDetails_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuotationDetails_OptionQuotations_OptionQuotationId",
                        column: x => x.OptionQuotationId,
                        principalTable: "OptionQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuotationDetails_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SellPriceHistorys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MOQ = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RedundancyCost = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FacilityServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransportServiceDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellPriceHistorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellPriceHistorys_FacilityServices_FacilityServiceId",
                        column: x => x.FacilityServiceId,
                        principalTable: "FacilityServices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SellPriceHistorys_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SellPriceHistorys_TransportServiceDetails_TransportServiceDetailId",
                        column: x => x.TransportServiceDetailId,
                        principalTable: "TransportServiceDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceCostHistorys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RedundancyCost = table.Column<double>(type: "float", nullable: false),
                    MOQ = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FacilityServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransportServiceDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCostHistorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCostHistorys_FacilityServices_FacilityServiceId",
                        column: x => x.FacilityServiceId,
                        principalTable: "FacilityServices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceCostHistorys_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceCostHistorys_TransportServiceDetails_TransportServiceDetailId",
                        column: x => x.TransportServiceDetailId,
                        principalTable: "TransportServiceDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttendanceDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceTypeId = table.Column<int>(type: "int", nullable: false),
                    TourTravellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceRouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceDetails_AttendanceRoutes_AttendanceRouteId",
                        column: x => x.AttendanceRouteId,
                        principalTable: "AttendanceRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AttendanceDetails_AttendanceTypes_AttendanceTypeId",
                        column: x => x.AttendanceTypeId,
                        principalTable: "AttendanceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AttendanceDetails_TourTravellers_TourTravellerId",
                        column: x => x.TourTravellerId,
                        principalTable: "TourTravellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlanServiceCostDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellPriceHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceTransportPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanServiceCostDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanServiceCostDetails_ReferenceTransportPrices_ReferenceTransportPriceId",
                        column: x => x.ReferenceTransportPriceId,
                        principalTable: "ReferenceTransportPrices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlanServiceCostDetails_SellPriceHistorys_SellPriceHistoryId",
                        column: x => x.SellPriceHistoryId,
                        principalTable: "SellPriceHistorys",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlanServiceCostDetails_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "02962efa-1273-46c0-b103-7167b1742ef3", "02962efa-1273-46c0-b103-7167b1742ef3", "CUSTOMER", "customer" },
                    { "6a32e12a-60b5-4d93-8306-82231e1232d7", "6a32e12a-60b5-4d93-8306-82231e1232d7", "ADMIN", "admin" },
                    { "814f9270-78f5-4503-b7d3-0c567e5812ba", "814f9270-78f5-4503-b7d3-0c567e5812ba", "TOUR GUIDE", "tour guide" },
                    { "85b6791c-49d8-4a61-ad0b-8274ec27e764", "85b6791c-49d8-4a61-ad0b-8274ec27e764", "STAFF", "staff" }
                });

            migrationBuilder.InsertData(
                table: "AttendanceRouteTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "IN" },
                    { 1, "OUT" }
                });

            migrationBuilder.InsertData(
                table: "AttendanceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NOTYET" },
                    { 1, "ATTENDEDED" },
                    { 2, "ABSENT" }
                });

            migrationBuilder.InsertData(
                table: "DietaryPreferences",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "OMNIVORES" },
                    { 1, "VEGAN" },
                    { 2, "VEGETARIAN" },
                    { 3, "GLUTEN_FREE" },
                    { 4, "HALAL" },
                    { 5, "KOSHER" },
                    { 6, "PESCATARIAN" }
                });

            migrationBuilder.InsertData(
                table: "FacilityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "HOTEL" },
                    { 1, "RESTAURANTS" },
                    { 2, "ENTERTAINMENT" },
                    { 3, "VEHICLE_SUPPLY" },
                    { 4, "AIR_TICKET_SUPPLY" }
                });

            migrationBuilder.InsertData(
                table: "JoinTourStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "APPROVED" },
                    { 2, "REJECTED" }
                });

            migrationBuilder.InsertData(
                table: "MaterialTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "PASSPORT" });

            migrationBuilder.InsertData(
                table: "MealTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Breakfast" },
                    { 1, "Lunch" },
                    { 2, "Dinner" }
                });

            migrationBuilder.InsertData(
                table: "OptionClasses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "ECONOMY" },
                    { 1, "MIDDLE" },
                    { 2, "PREMIUM" }
                });

            migrationBuilder.InsertData(
                table: "OptionQuotationStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "ACTIVE" },
                    { 2, "IN_ACTIVE" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "PAID" }
                });

            migrationBuilder.InsertData(
                table: "PrivateTourStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "WAITINGFORCUSTOMER" },
                    { 2, "APPROVED" },
                    { 3, "REJECTED" }
                });

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "LODGING" },
                    { 1, "HOTEL_TWOSTAR" }
                });

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "HOTEL_THREESTAR" },
                    { 3, "HOTEL_FOURSTAR" },
                    { 4, "HOTEL_FIVESTAR" },
                    { 5, "RESTAURENT_CASUAL" },
                    { 6, "RESTAURENT_TWOSTAR" },
                    { 7, "RESTAURENT_THREESTAR" },
                    { 8, "RESTAURENT_FOURSTAR" },
                    { 9, "RESTAURENT_FIVESTAR" },
                    { 10, "RESORT" },
                    { 11, "TOURIST_AREA" }
                });

            migrationBuilder.InsertData(
                table: "ReferencePriceRating",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "ECONOMY_AIR_BRAND" },
                    { 1, "STANDARD_AIR_BRAND" },
                    { 2, "MIDDLE_AIR_BRAND" },
                    { 3, "STANDARD_FERRY_TERMINAL" }
                });

            migrationBuilder.InsertData(
                table: "ServiceAvailabilities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "ADULT" },
                    { 1, "CHILD" },
                    { 2, "BOTH" }
                });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "RESTING" },
                    { 1, "FOODANDBEVARAGE" },
                    { 2, "ENTERTAIMENT" },
                    { 3, "VEHICLE" },
                    { 4, "ATTRACTION" },
                    { 5, "GUIDING" },
                    { 6, "DRIVING" },
                    { 7, "HOLD" }
                });

            migrationBuilder.InsertData(
                table: "TourStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "PENDING" },
                    { 2, "SUFFICIENT" },
                    { 3, "SCHEDULING_COMPLETED" },
                    { 4, "STARTING" },
                    { 5, "COMPLETED" }
                });

            migrationBuilder.InsertData(
                table: "TourTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "PUBLIC" },
                    { 1, "CUSTOM" },
                    { 2, "ENTERPRISE" }
                });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "RECHARGE_FROM_VNPAY" },
                    { 1, "RECHARGE_FROM_MOMO" },
                    { 2, "PAYMENT_FOR_SERVICES" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "DAY" },
                    { 1, "ROOM" },
                    { 2, "PERSON" }
                });

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "BUS" },
                    { 1, "COACH" }
                });

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "LIMOUSINE" },
                    { 3, "CAR" },
                    { 4, "PLANE" },
                    { 5, "BOAT" },
                    { 6, "BICYCLE" },
                    { 7, "HELICOPTER" }
                });

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("1ba853db-84e4-4e9c-b750-dbe432d1a2b0"), 0, 2 },
                    { new Guid("1e955290-e063-439f-8e17-d2edcb2ad69b"), 0, 1 },
                    { new Guid("23c8e757-ac2d-4232-a569-d5461deb9b05"), 1, 6 },
                    { new Guid("34115f50-25d5-4750-8500-5cb917f27da5"), 0, 0 },
                    { new Guid("4b3961ff-aa19-41fd-9a3e-1a6b37f99d09"), 1, 9 },
                    { new Guid("994cb1a9-6d4e-4d34-b904-91b229a12d5c"), 2, 11 },
                    { new Guid("a59bbb50-4fcc-473f-bbf4-b311f06b2ed8"), 1, 7 },
                    { new Guid("d39d6346-6831-4155-b27a-a2a779271ba2"), 0, 10 },
                    { new Guid("ef3fc5d9-7c7a-44c3-8fc1-8da12ca7e93b"), 0, 3 },
                    { new Guid("f2603607-d947-4aca-888e-9fd3bc3c0339"), 0, 4 },
                    { new Guid("f48d19ee-36ae-49bd-88ed-a3d326b1b1d8"), 1, 5 },
                    { new Guid("fa63bf87-51a3-46b7-a6df-aa29fb9b9057"), 1, 8 }
                });

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
                name: "IX_AssurancePriceHistories_AssuranceId",
                table: "AssurancePriceHistories",
                column: "AssuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDetails_AttendanceRouteId",
                table: "AttendanceDetails",
                column: "AttendanceRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDetails_AttendanceTypeId",
                table: "AttendanceDetails",
                column: "AttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDetails_TourTravellerId",
                table: "AttendanceDetails",
                column: "TourTravellerId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRoutes_AttendanceRouteTypeId",
                table: "AttendanceRoutes",
                column: "AttendanceRouteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRoutes_RouteId",
                table: "AttendanceRoutes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Communes_DistrictId",
                table: "Communes",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CreateBy",
                table: "Contracts",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CustomerId",
                table: "Contracts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_TourId",
                table: "Contracts",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_UpdateBy",
                table: "Contracts",
                column: "UpdateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AccountId",
                table: "Customers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DayPlans_TourId",
                table: "DayPlans",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_DishTypeId",
                table: "Dishes",
                column: "DishTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverSalaryHistories_DriverId",
                table: "DriverSalaryHistories",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_EventDetailPriceHistories_EventDetailId",
                table: "EventDetailPriceHistories",
                column: "EventDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_EventDetails_EventId",
                table: "EventDetails",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_CommunceId",
                table: "Facilities",
                column: "CommunceId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_FacilityRatingId",
                table: "Facilities",
                column: "FacilityRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_ServiceProviderId",
                table: "Facilities",
                column: "ServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityRatings_FacilityTypeId",
                table: "FacilityRatings",
                column: "FacilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityRatings_RatingId",
                table: "FacilityRatings",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityServices_FacilityId",
                table: "FacilityServices",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityServices_ServiceAvailabilityId",
                table: "FacilityServices",
                column: "ServiceAvailabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityServices_ServiceTypeId",
                table: "FacilityServices",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityServices_UnitId",
                table: "FacilityServices",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignments_MaterialId",
                table: "MaterialAssignments",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignments_TourId",
                table: "MaterialAssignments",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDishes_DishId",
                table: "MenuDishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDishes_MenuId",
                table: "MenuDishes",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_CreateBy",
                table: "Menus",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_DietaryPreferenceId",
                table: "Menus",
                column: "DietaryPreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_FacilityServiceId",
                table: "Menus",
                column: "FacilityServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MealTypeId",
                table: "Menus",
                column: "MealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UpdateBy",
                table: "Menus",
                column: "UpdateBy");

            migrationBuilder.CreateIndex(
                name: "IX_OptionQuotations_AssurancePriceHistoryId",
                table: "OptionQuotations",
                column: "AssurancePriceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionQuotations_OptionClassId",
                table: "OptionQuotations",
                column: "OptionClassId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionQuotations_OptionQuotationStatusId",
                table: "OptionQuotations",
                column: "OptionQuotationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionQuotations_PrivateTourRequestId",
                table: "OptionQuotations",
                column: "PrivateTourRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatusId",
                table: "Orders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PrivateTourRequestId",
                table: "Orders",
                column: "PrivateTourRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TourId",
                table: "Orders",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanServiceCostDetails_ReferenceTransportPriceId",
                table: "PlanServiceCostDetails",
                column: "ReferenceTransportPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanServiceCostDetails_SellPriceHistoryId",
                table: "PlanServiceCostDetails",
                column: "SellPriceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanServiceCostDetails_TourId",
                table: "PlanServiceCostDetails",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Ports_CommuneId",
                table: "Ports",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateJoinTourRequests_CreateBy",
                table: "PrivateJoinTourRequests",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateJoinTourRequests_JoinTourStatusId",
                table: "PrivateJoinTourRequests",
                column: "JoinTourStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateJoinTourRequests_TourId",
                table: "PrivateJoinTourRequests",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateJoinTourRequests_TravelCompanionId",
                table: "PrivateJoinTourRequests",
                column: "TravelCompanionId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateJoinTourRequests_UpdateBy",
                table: "PrivateJoinTourRequests",
                column: "UpdateBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_CreateBy",
                table: "PrivateTourRequests",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_DietaryPreferenceId",
                table: "PrivateTourRequests",
                column: "DietaryPreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_MainDestinationId",
                table: "PrivateTourRequests",
                column: "MainDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_MinimumHotelRatingId",
                table: "PrivateTourRequests",
                column: "MinimumHotelRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_MinimumRestaurantRatingId",
                table: "PrivateTourRequests",
                column: "MinimumRestaurantRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_PrivateTourStatusId",
                table: "PrivateTourRequests",
                column: "PrivateTourStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_StartLocationCommuneId",
                table: "PrivateTourRequests",
                column: "StartLocationCommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_TourId",
                table: "PrivateTourRequests",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_UpdateBy",
                table: "PrivateTourRequests",
                column: "UpdateBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_DistrictId",
                table: "QuotationDetails",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_FacilityRatingId",
                table: "QuotationDetails",
                column: "FacilityRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_MenuId",
                table: "QuotationDetails",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_OptionQuotationId",
                table: "QuotationDetails",
                column: "OptionQuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_ServiceTypeId",
                table: "QuotationDetails",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceTransportPrices_ArrivalId",
                table: "ReferenceTransportPrices",
                column: "ArrivalId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceTransportPrices_DepartureId",
                table: "ReferenceTransportPrices",
                column: "DepartureId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceTransportPrices_ReferencePriceRatingId",
                table: "ReferenceTransportPrices",
                column: "ReferencePriceRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedLocations_PrivateTourRequestId",
                table: "RequestedLocations",
                column: "PrivateTourRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedLocations_ProvinceId",
                table: "RequestedLocations",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_DayPlanId",
                table: "Routes",
                column: "DayPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_EndPointId",
                table: "Routes",
                column: "EndPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ParentRouteId",
                table: "Routes",
                column: "ParentRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_PortEndPointId",
                table: "Routes",
                column: "PortEndPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_PortStartPointId",
                table: "Routes",
                column: "PortStartPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StartPointId",
                table: "Routes",
                column: "StartPointId");

            migrationBuilder.CreateIndex(
                name: "IX_SellPriceHistorys_FacilityServiceId",
                table: "SellPriceHistorys",
                column: "FacilityServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SellPriceHistorys_MenuId",
                table: "SellPriceHistorys",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_SellPriceHistorys_TransportServiceDetailId",
                table: "SellPriceHistorys",
                column: "TransportServiceDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCostHistorys_FacilityServiceId",
                table: "ServiceCostHistorys",
                column: "FacilityServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCostHistorys_MenuId",
                table: "ServiceCostHistorys",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCostHistorys_TransportServiceDetailId",
                table: "ServiceCostHistorys",
                column: "TransportServiceDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TourguideAssignments_AccountId",
                table: "TourguideAssignments",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TourguideAssignments_ProvinceId",
                table: "TourguideAssignments",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_TourguideAssignments_TourId",
                table: "TourguideAssignments",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideSalaryHistories_AccountId",
                table: "TourGuideSalaryHistories",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TourguideScopes_AccountId",
                table: "TourguideScopes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TourguideScopes_DistrictId",
                table: "TourguideScopes",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_TourRegistrations_FollowerId",
                table: "TourRegistrations",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_TourRegistrations_PresenterId",
                table: "TourRegistrations",
                column: "PresenterId");

            migrationBuilder.CreateIndex(
                name: "IX_TourRegistrations_TourId",
                table: "TourRegistrations",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_BasedOnTourId",
                table: "Tours",
                column: "BasedOnTourId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourStatusId",
                table: "Tours",
                column: "TourStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourTypeId",
                table: "Tours",
                column: "TourTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_VehicleTypeId",
                table: "Tours",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTravellers_CustomerId",
                table: "TourTravellers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTravellers_TourId",
                table: "TourTravellers",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionTypeId",
                table: "Transactions",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TravelCompanionId",
                table: "Transactions",
                column: "TravelCompanionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportServiceDetails_FacilityServiceId",
                table: "TransportServiceDetails",
                column: "FacilityServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportServiceDetails_VehicleTypeId",
                table: "TransportServiceDetails",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_EndPointDistrictId",
                table: "VehicleQuotationDetails",
                column: "EndPointDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_EndPointId",
                table: "VehicleQuotationDetails",
                column: "EndPointId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_OptionQuotationId",
                table: "VehicleQuotationDetails",
                column: "OptionQuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_StartPointDistrictId",
                table: "VehicleQuotationDetails",
                column: "StartPointDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_StartPointId",
                table: "VehicleQuotationDetails",
                column: "StartPointId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRoutes_DriverId",
                table: "VehicleRoutes",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRoutes_RouteId",
                table: "VehicleRoutes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRoutes_VehicleId",
                table: "VehicleRoutes",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId");
        }

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
                name: "AttendanceDetails");

            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "ContractStatuses");

            migrationBuilder.DropTable(
                name: "DriverSalaryHistories");

            migrationBuilder.DropTable(
                name: "EventDetailPriceHistories");

            migrationBuilder.DropTable(
                name: "MaterialAssignments");

            migrationBuilder.DropTable(
                name: "MenuDishes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PlanServiceCostDetails");

            migrationBuilder.DropTable(
                name: "PortTypes");

            migrationBuilder.DropTable(
                name: "PrivateJoinTourRequests");

            migrationBuilder.DropTable(
                name: "QuotationDetails");

            migrationBuilder.DropTable(
                name: "RequestedLocations");

            migrationBuilder.DropTable(
                name: "ServiceCostHistorys");

            migrationBuilder.DropTable(
                name: "TourguideAssignments");

            migrationBuilder.DropTable(
                name: "TourGuideSalaryHistories");

            migrationBuilder.DropTable(
                name: "TourguideScopes");

            migrationBuilder.DropTable(
                name: "TourRegistrations");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "VehicleQuotationDetails");

            migrationBuilder.DropTable(
                name: "VehicleRoutes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AttendanceRoutes");

            migrationBuilder.DropTable(
                name: "AttendanceTypes");

            migrationBuilder.DropTable(
                name: "TourTravellers");

            migrationBuilder.DropTable(
                name: "EventDetails");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "ReferenceTransportPrices");

            migrationBuilder.DropTable(
                name: "SellPriceHistorys");

            migrationBuilder.DropTable(
                name: "JoinTourStatuses");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "OptionQuotations");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "AttendanceRouteTypes");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "MaterialTypes");

            migrationBuilder.DropTable(
                name: "DishTypes");

            migrationBuilder.DropTable(
                name: "ReferencePriceRating");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "TransportServiceDetails");

            migrationBuilder.DropTable(
                name: "AssurancePriceHistories");

            migrationBuilder.DropTable(
                name: "OptionClasses");

            migrationBuilder.DropTable(
                name: "OptionQuotationStatuses");

            migrationBuilder.DropTable(
                name: "PrivateTourRequests");

            migrationBuilder.DropTable(
                name: "DayPlans");

            migrationBuilder.DropTable(
                name: "Ports");

            migrationBuilder.DropTable(
                name: "MealTypes");

            migrationBuilder.DropTable(
                name: "FacilityServices");

            migrationBuilder.DropTable(
                name: "Assurances");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DietaryPreferences");

            migrationBuilder.DropTable(
                name: "PrivateTourStatuses");

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "ServiceAvailabilities");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "TourStatuses");

            migrationBuilder.DropTable(
                name: "TourTypes");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "Communes");

            migrationBuilder.DropTable(
                name: "FacilityRatings");

            migrationBuilder.DropTable(
                name: "ServiceProviders");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "FacilityTypes");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Provinces");
        }
    }
}
