using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Core.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImanageRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImanageRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImanageRoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImanageRoleClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImanageUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(maxLength: 150, nullable: true),
                    FirstName = table.Column<string>(maxLength: 150, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 150, nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: true),
                    Activated = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    UserTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImanageUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImanageUserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImanageUserClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImanageUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImanageUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.UniqueConstraint("AK_ImanageUserLogin_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImanageUserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImanageUserRole", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "ImanageUserToken",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImanageUserToken", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictApplications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(maxLength: 100, nullable: false),
                    ClientSecret = table.Column<string>(nullable: true),
                    ConcurrencyToken = table.Column<string>(maxLength: 50, nullable: true),
                    ConsentType = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Permissions = table.Column<string>(nullable: true),
                    PostLogoutRedirectUris = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    RedirectUris = table.Column<string>(nullable: true),
                    Type = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictScopes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyToken = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Properties = table.Column<string>(nullable: true),
                    Resources = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    ModifiedBy = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumber2 = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CAC = table.Column<string>(nullable: true),
                    NIN = table.Column<string>(nullable: true),
                    AccountType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    User_Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_ImanageUser_User_Id",
                        column: x => x.User_Id,
                        principalTable: "ImanageUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictAuthorizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationId = table.Column<string>(nullable: true),
                    ConcurrencyToken = table.Column<string>(maxLength: 50, nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    Scopes = table.Column<string>(nullable: true),
                    Status = table.Column<string>(maxLength: 25, nullable: false),
                    Subject = table.Column<string>(maxLength: 450, nullable: false),
                    Type = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationId = table.Column<string>(nullable: true),
                    AuthorizationId = table.Column<string>(nullable: true),
                    ConcurrencyToken = table.Column<string>(maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(nullable: true),
                    ExpirationDate = table.Column<DateTimeOffset>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    ReferenceId = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<string>(maxLength: 25, nullable: false),
                    Subject = table.Column<string>(maxLength: 450, nullable: false),
                    Type = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "OpenIddictAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ImanageRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("a1b6b6b0-0825-4975-a93d-df3dc86f8cc7"), "bb37771e9ab940ca999303cb06cbca98", "SYS_ADMIN", "SYS_ADMIN" },
                    { new Guid("0718ddef-4067-4f29-aaa1-98c1548c1807"), "fbee38d182ed4da1bbf583ab80f7a603", "SUPER_ADMIN", "SUPER_ADMIN" },
                    { new Guid("8b93d395-a71a-4620-9352-5b9e6b3b6045"), "45c361c6fbde48f48461d1dbf89c1208", "LandLord", "LandLord" },
                    { new Guid("c586e0ba-64a9-427b-8888-6f16755e5d9d"), "67234e38dfd04c23b87810ab4f7a42aa", "Estate_Manager", "Estate_Manager" }
                });

            migrationBuilder.InsertData(
                table: "ImanageUser",
                columns: new[] { "Id", "AccessFailedCount", "Activated", "ConcurrencyStamp", "CreatedOnUtc", "Email", "EmailConfirmed", "FirstName", "Gender", "IsDeleted", "LastLoginDate", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Unit", "UserName", "UserType", "UserTypeId" },
                values: new object[,]
                {
                    { new Guid("1989883f-4f99-43bf-a754-239bbbfec00b"), 0, true, "52f105ed-35cc-4ee9-9444-bc8b74fe8d8f", new DateTime(2020, 5, 27, 21, 9, 38, 265, DateTimeKind.Local).AddTicks(3497), "system@imanager.com", true, "Imanage", 1, false, new DateTime(2020, 5, 27, 21, 9, 38, 265, DateTimeKind.Local).AddTicks(6414), "Estate Manager", false, null, null, "SYSTEM@IMANAGER.COM", "SYSTEM@IMANAGER.COM", "AQAAAAEAACcQAAAAEJC8yRZbtg7CPzw6nIjTZVn82MLUiS6wUGuyWx3Kc5vu7Ro5WmjTqIGNDCJlvjIE3A==", "08062066851", true, "99ae0c45-d682-4542-9ba7-1281e471916b", false, null, "system@imanager.com", 2, null },
                    { new Guid("3fb897c8-c25d-4328-9813-cb1544369fba"), 0, true, "7a6195d1-dc32-4ea8-8538-e859759adf41", new DateTime(2020, 5, 27, 21, 9, 38, 281, DateTimeKind.Local).AddTicks(583), "superadmin@imanage.com", true, "Imanage", 1, false, new DateTime(2020, 5, 27, 21, 9, 38, 281, DateTimeKind.Local).AddTicks(600), "Estate Manager", false, null, null, "SUPERADMIN@IMANAGE.COM", "SUPERADMIN@IMANAGE.COM", "AQAAAAEAACcQAAAAEGS6VNr/qtIlSUq+G9mllAI38hYZcw5+S9aMp42oBm9T4vciHiq3GYf3etsCF4wmBw==", "08062066851", true, "016020e3-5c50-40b4-9e66-bba56c9f5bf2", false, null, "superadmin@imanage.com", 2, null },
                    { new Guid("5bcb4f11-3177-4905-bff8-fa645d0e055e"), 0, true, "fe4c9851-4cee-4261-a285-2fe45a12a564", new DateTime(2020, 5, 27, 21, 9, 38, 289, DateTimeKind.Local).AddTicks(2589), "adminuser@imanage.com", true, "System", 1, false, new DateTime(2020, 5, 27, 21, 9, 38, 289, DateTimeKind.Local).AddTicks(2594), "adminUser", false, null, null, "ADMINUSER@IMANAGE.COM", "ADMINUSER@IMANAGE.COM", "AQAAAAEAACcQAAAAEBzpCSgUb+oAlDUhPLQi63NffN/KOmrBwh7009EwvWmM7O47yI3K7RSpyTZ1xzPdhA==", "08062066851", true, "7d728c76-1c51-491a-99db-bde6a5b0655a", false, null, "adminuser@imanage.com", 2, null },
                    { new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a"), 0, true, "5a7b9caa-d9d2-4ad7-abab-6ef8feee6c52", new DateTime(2020, 5, 27, 21, 9, 38, 296, DateTimeKind.Local).AddTicks(8157), "admin@imanage.com", true, "System", 1, false, new DateTime(2020, 5, 27, 21, 9, 38, 296, DateTimeKind.Local).AddTicks(8163), "admin", false, null, null, "ADMIN@IMANAGE.COM", "ADMIN@IMANAGE.COM", "AQAAAAEAACcQAAAAEKt5HaJBfYvAQGw8lzfd60B9pdHmlyo5m9zdB56fX1Da7BwtzyIZoMBdUrTrgSLetA==", "08062066851", true, "5725abea-38f8-45f7-a485-0805b59d078b", false, null, "admin@imanage.com", 2, null }
                });

            migrationBuilder.InsertData(
                table: "ImanageUserClaim",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 57, "Permission", "REQUEST_ID", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 56, "Permission", "SYSTEM_VALIDATION", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 55, "Permission", "NAV_INFO", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 54, "Permission", "DASHBOARD_VIEW_DETAILS", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 53, "Permission", "DASHBOARD_VIEWER", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 52, "Permission", "DOCUMENT_VIEW_ACTIONS", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 51, "Permission", "DOCUMENT_VIEWER", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 50, "Permission", "TABLE_2_BULK_ACTION", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 46, "Permission", "TABLE_ACTION", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 48, "Permission", "TABLE_2_VIEW", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 47, "Permission", "TABLE_BULK_ACTION", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 58, "Permission", "GENERAL_INFO", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 45, "Permission", "TABLE_VIEW", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 44, "Permission", "SEARCH", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 43, "Permission", "DASHBOARD_VIEWER", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 42, "Permission", "SYSTEM_VALIDATION", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 49, "Permission", "TABLE_2_ACTION", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 59, "Permission", "VIEW_APPROVALS", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 63, "Permission", "SEARCH", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 61, "Permission", "ESTATE_MANAGER_PERM", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 78, "Permission", "VIEW_APPROVALS", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 77, "Permission", "GENERAL_INFO", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 76, "Permission", "REQUEST_ID", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 75, "Permission", "SYSTEM_VALIDATION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 74, "Permission", "NAV_INFO", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 73, "Permission", "DASHBOARD_VIEW_DETAILS", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 72, "Permission", "DASHBOARD_VIEWER", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 71, "Permission", "DOCUMENT_VIEW_ACTIONS", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 70, "Permission", "DOCUMENT_VIEWER", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 69, "Permission", "TABLE_2_BULK_ACTION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 68, "Permission", "TABLE_2_ACTION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 67, "Permission", "TABLE_2_VIEW", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 66, "Permission", "TABLE_BULK_ACTION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 65, "Permission", "TABLE_ACTION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 64, "Permission", "TABLE_VIEW", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 41, "Permission", "ESTATE_MANAGER_PERM", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 62, "Permission", "ESTATE_MANAGER_PERM", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 60, "Permission", "SUPER_ADMIN_PERM", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 40, "Permission", "GENERAL_INFO", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 38, "Permission", "SYS_ADMIN_PERM", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 17, "Permission", "NAV_INFO", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 16, "Permission", "DASHBOARD_VIEW_DETAILS", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 15, "Permission", "DASHBOARD_VIEWER", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 14, "Permission", "DOCUMENT_VIEW_ACTIONS", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 13, "Permission", "DOCUMENT_VIEWER", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 12, "Permission", "TABLE_2_BULK_ACTION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 11, "Permission", "TABLE_2_ACTION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 10, "Permission", "TABLE_2_VIEW", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 39, "Permission", "LANDLORD_PERM", new Guid("3fb897c8-c25d-4328-9813-cb1544369fba") },
                    { 9, "Permission", "TABLE_BULK_ACTION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 7, "Permission", "TABLE_VIEW", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 6, "Permission", "SEARCH", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 5, "Permission", "DASHBOARD_VIEWER", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 4, "Permission", "SYSTEM_VALIDATION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 3, "Permission", "ESTATE_MANAGER_PERM", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 2, "Permission", "GENERAL_INFO", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 1, "Permission", "LANDLORD_PERM", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 8, "Permission", "TABLE_ACTION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 18, "Permission", "SYSTEM_VALIDATION", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 19, "Permission", "REQUEST_ID", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 20, "Permission", "GENERAL_INFO", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") },
                    { 37, "Permission", "VIEW_APPROVALS", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 36, "Permission", "GENERAL_INFO", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 35, "Permission", "REQUEST_ID", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 34, "Permission", "SYSTEM_VALIDATION", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 33, "Permission", "NAV_INFO", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 32, "Permission", "DASHBOARD_VIEW_DETAILS", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 31, "Permission", "DASHBOARD_VIEWER", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 30, "Permission", "DOCUMENT_VIEW_ACTIONS", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 29, "Permission", "DOCUMENT_VIEWER", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 28, "Permission", "TABLE_2_BULK_ACTION", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 27, "Permission", "TABLE_2_ACTION", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 26, "Permission", "TABLE_2_VIEW", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 25, "Permission", "TABLE_BULK_ACTION", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 24, "Permission", "TABLE_ACTION", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 23, "Permission", "TABLE_VIEW", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 22, "Permission", "SEARCH", new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a") },
                    { 21, "Permission", "VIEW_APPROVALS", new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216") }
                });

            migrationBuilder.InsertData(
                table: "ImanageUserRole",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("3d26739c-e070-4e99-8844-6a2e4c2bc216"), new Guid("c586e0ba-64a9-427b-8888-6f16755e5d9d") },
                    { new Guid("129712e3-9214-4dd3-9c03-cfc4eb9ba979"), new Guid("880fd08d-044c-4068-b4e4-84c970344b3a") },
                    { new Guid("6ff03683-f832-4288-97b3-a79be8c6549c"), new Guid("8b93d395-a71a-4620-9352-5b9e6b3b6045") },
                    { new Guid("973af7a9-7f18-4e8b-acd3-bc906580561a"), new Guid("a1b6b6b0-0825-4975-a93d-df3dc86f8cc7") },
                    { new Guid("3fb897c8-c25d-4328-9813-cb1544369fba"), new Guid("0718ddef-4067-4f29-aaa1-98c1548c1807") },
                    { new Guid("30349956-8a58-40e2-a06e-72073e1b0174"), new Guid("16d9503e-5322-4126-90ef-34eecf3d5a46") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_User_Id",
                table: "Account",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_ClientId",
                table: "OpenIddictApplications",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type",
                table: "OpenIddictAuthorizations",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictScopes_Name",
                table: "OpenIddictScopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_AuthorizationId",
                table: "OpenIddictTokens",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ReferenceId",
                table: "OpenIddictTokens",
                column: "ReferenceId",
                unique: true,
                filter: "[ReferenceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type",
                table: "OpenIddictTokens",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "ImanageRole");

            migrationBuilder.DropTable(
                name: "ImanageRoleClaim");

            migrationBuilder.DropTable(
                name: "ImanageUserClaim");

            migrationBuilder.DropTable(
                name: "ImanageUserLogin");

            migrationBuilder.DropTable(
                name: "ImanageUserRole");

            migrationBuilder.DropTable(
                name: "ImanageUserToken");

            migrationBuilder.DropTable(
                name: "OpenIddictScopes");

            migrationBuilder.DropTable(
                name: "OpenIddictTokens");

            migrationBuilder.DropTable(
                name: "ImanageUser");

            migrationBuilder.DropTable(
                name: "OpenIddictAuthorizations");

            migrationBuilder.DropTable(
                name: "OpenIddictApplications");
        }
    }
}
