using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Mvc.Migrations
{
    public partial class M2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressLine = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRoleMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifiedUser = table.Column<string>(maxLength: 128, nullable: true),
                    RecordStatus = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    RoleId = table.Column<string>(maxLength: 128, nullable: true),
                    ApplicationUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRoleMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifiedUser = table.Column<string>(maxLength: 128, nullable: true),
                    RecordStatus = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    UserLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAgentMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifiedUser = table.Column<string>(maxLength: 128, nullable: true),
                    RecordStatus = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<int>(nullable: false),
                    IsSpecifiedForCompany = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAgentMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUserRoleMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifiedUser = table.Column<string>(maxLength: 128, nullable: true),
                    RecordStatus = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    RoleId = table.Column<string>(maxLength: 128, nullable: true),
                    CompanyUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUserRoleMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Demands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifiedUser = table.Column<string>(maxLength: 128, nullable: true),
                    RecordStatus = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Text = table.Column<string>(maxLength: 512, nullable: true),
                    OrderOfUrgencyId = table.Column<int>(nullable: false),
                    IsAccepted = table.Column<bool>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    IsDissolved = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CompanyUserAccountId = table.Column<Guid>(nullable: false),
                    ApplicationUserAccountId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderOfUrgency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifiedUser = table.Column<string>(maxLength: 128, nullable: true),
                    RecordStatus = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOfUrgency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifiedUser = table.Column<string>(maxLength: 128, nullable: true),
                    RecordStatus = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    TrIdentityNumber = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    AddressId = table.Column<int>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    FixedNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    AccountId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(maxLength: 128, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifiedUser = table.Column<string>(maxLength: 128, nullable: true),
                    RecordStatus = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    TrIdentityNumber = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    AddressId = table.Column<int>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    FixedNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    AccountId = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyUser_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyUser_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_AddressId",
                table: "ApplicationUsers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_AddressId",
                table: "CompanyUser",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_CompanyId",
                table: "CompanyUser",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserRoleMapping");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "CompanyAgentMapping");

            migrationBuilder.DropTable(
                name: "CompanyUser");

            migrationBuilder.DropTable(
                name: "CompanyUserRoleMapping");

            migrationBuilder.DropTable(
                name: "Demands");

            migrationBuilder.DropTable(
                name: "OrderOfUrgency");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
