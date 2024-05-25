﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTeacher.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
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
                name: "Wallet",
                columns: table => new
                {
                    Wallet_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Wallet_id);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    First_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<byte>(type: "tinyint", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Wallet_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    Link_contact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Rating = table.Column<byte>(type: "tinyint", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Role = table.Column<byte>(type: "tinyint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Wallet_Wallet_id",
                        column: x => x.Wallet_id,
                        principalTable: "Wallet",
                        principalColumn: "Wallet_id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Class_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    student_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    student_id1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tutor_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Subject_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Subject_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    End_date = table.Column<DateOnly>(type: "date", nullable: false),
                    Start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    End_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Type_class = table.Column<byte>(type: "tinyint", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Number_of_session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Class_id);
                    table.ForeignKey(
                        name: "FK_Classes_AspNetUsers_Tutor_id",
                        column: x => x.Tutor_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_AspNetUsers_student_id",
                        column: x => x.student_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_AspNetUsers_student_id1",
                        column: x => x.student_id1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Order_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Wallet_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Order_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    User_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Class_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Order_type = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Order_id);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_User_id",
                        column: x => x.User_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Wallet_Wallet_id",
                        column: x => x.Wallet_id,
                        principalTable: "Wallet",
                        principalColumn: "Wallet_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Qualification",
                columns: table => new
                {
                    Qualification_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    User_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Graduation_year = table.Column<int>(type: "int", nullable: false),
                    Specialize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Classification = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Training_facility = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualification", x => x.Qualification_id);
                    table.ForeignKey(
                        name: "FK_Qualification_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Qualification_AspNetUsers_User_id",
                        column: x => x.User_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Report_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    User_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Report_id);
                    table.ForeignKey(
                        name: "FK_Report_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Report_AspNetUsers_User_id",
                        column: x => x.User_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requirement",
                columns: table => new
                {
                    Requirement_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    User_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Subject_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Subject_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    End_date = table.Column<DateOnly>(type: "date", nullable: false),
                    Start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    End_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Grade = table.Column<byte>(type: "tinyint", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Number_of_session = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirement", x => x.Requirement_id);
                    table.ForeignKey(
                        name: "FK_Requirement_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requirement_AspNetUsers_User_id",
                        column: x => x.User_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_AspNetUsers_Wallet_id",
                table: "AspNetUsers",
                column: "Wallet_id",
                unique: true,
                filter: "[Wallet_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_student_id",
                table: "Classes",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_student_id1",
                table: "Classes",
                column: "student_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Tutor_id",
                table: "Classes",
                column: "Tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_User_id",
                table: "Order",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Wallet_id",
                table: "Order",
                column: "Wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_Qualification_User_id",
                table: "Qualification",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Qualification_UserId",
                table: "Qualification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_User_id",
                table: "Report",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Report_UserId",
                table: "Report",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Requirement_User_id",
                table: "Requirement",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Requirement_UserId",
                table: "Requirement",
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
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Qualification");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Requirement");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Wallet");
        }
    }
}
