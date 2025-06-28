using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebJob.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysFunctionRoles",
                columns: table => new
                {
                    SysFunctionId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysFunctionRoles", x => new { x.SysFunctionId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "SysFunctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FunctionDesc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ParentItemId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IconPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsShow = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsEnable = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    HasChild = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CssMenuActive = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CssMenuOpen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TreeOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TreeLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysFunctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysFunctionUsers",
                columns: table => new
                {
                    SysFunctionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysFunctionUsers", x => new { x.SysFunctionId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "SysLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceContext = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Application = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FromIP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserAction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CrDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DefaultActionId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AvatarPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    LastTimeChangePass = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastTimeLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CrDateTime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getdate()"),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
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
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
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
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "1", "Administrator", "Admin", "ADMIN" },
                    { 2, "2", "Quảng cáo", "Quảng cáo", "ADVERT" }
                });

            migrationBuilder.InsertData(
                table: "SysFunctionRoles",
                columns: new[] { "RoleId", "SysFunctionId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 }
                });

            migrationBuilder.InsertData(
                table: "SysFunctions",
                columns: new[] { "Id", "CssMenuActive", "CssMenuOpen", "DisplayOrder", "FunctionDesc", "FunctionName", "IconPath", "TreeLevel", "TreeOrder", "Url" },
                values: new object[] { 1, null, null, 10, "Hệ thống", "Hệ thống", "nav-icon fas fa-cog", (byte)1, 1, "/" });

            migrationBuilder.InsertData(
                table: "SysFunctions",
                columns: new[] { "Id", "CssMenuActive", "CssMenuOpen", "DisplayOrder", "FunctionDesc", "FunctionName", "IconPath", "ParentItemId", "TreeLevel", "TreeOrder", "Url" },
                values: new object[,]
                {
                    { 2, null, null, 1, "Danh sách user", "Danh sách user", "", 1, (byte)2, 2, "/Identity/SysUsers" },
                    { 3, null, null, 2, "Danh sách quyền", "Danh sách quyền", "", 1, (byte)2, 3, "/Identity/SysRoles" },
                    { 4, null, null, 3, "Danh sách chức năng", "Danh sách chức năng", "", 1, (byte)2, 4, "/Identity/SysFunctions" },
                    { 5, null, null, 4, "Log đăng nhập", "Log đăng nhập", "", 1, (byte)2, 5, "/Identity/UserLogs" },
                    { 6, null, null, 5, "Log hệ thống", "Log hệ thống", "", 1, (byte)2, 6, "/Identity/SysLogs" },
                    { 7, null, null, 6, "Log file", "Log file", "", 1, (byte)2, 7, "/Identity/SysLogFiles" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "AvatarPath", "BirthDay", "ConcurrencyStamp", "Email", "FullName", "LastTimeChangePass", "LastTimeLogin", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "SecurityStamp", "UserName" },
                values: new object[] { 1, null, null, null, "icsoft.vn", "admin@icsoft.vn", "Administrator", null, null, null, "admin@icsoft.vn", "admin", null, "AQAAAAIAAYagAAAAELyaXMNNOvN8a4+HOqsxmMKGQhu20VoHqMYjjRSAw05HUotUAjSt2/jBtf83n1W4aw==", null, "icsoft.vn", "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SysFunctionRoles");

            migrationBuilder.DropTable(
                name: "SysFunctions");

            migrationBuilder.DropTable(
                name: "SysFunctionUsers");

            migrationBuilder.DropTable(
                name: "SysLogs");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
