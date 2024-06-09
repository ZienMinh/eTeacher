using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Subject_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Subject_name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Subject_id);
                    table.UniqueConstraint("AK_Subject_Subject_name", x => x.Subject_name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    First_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<byte>(type: "tinyint", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Link_contact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Rating = table.Column<byte>(type: "tinyint", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Role = table.Column<byte>(type: "tinyint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Class_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Student_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Tutor_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Subject_name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Type_class = table.Column<byte>(type: "tinyint", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Number_of_session = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Class_id);
                    table.ForeignKey(
                        name: "FK_Classes_Subject_Subject_name",
                        column: x => x.Subject_name,
                        principalTable: "Subject",
                        principalColumn: "Subject_name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Users_Student_id",
                        column: x => x.Student_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Users_Tutor_id",
                        column: x => x.Tutor_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Order_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Order_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    User_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Class_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Order_type = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Order_id);
                    table.ForeignKey(
                        name: "FK_Order_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Otp",
                columns: table => new
                {
                    Otp_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    User_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Otp_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Expiry_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otp", x => x.Otp_id);
                    table.ForeignKey(
                        name: "FK_Otp_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "Id",
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
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualification", x => x.Qualification_id);
                    table.ForeignKey(
                        name: "FK_Qualification_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Report_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    User_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Class_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Report_id);
                    table.ForeignKey(
                        name: "FK_Report_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requirement",
                columns: table => new
                {
                    Requirement_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    User_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Subject_name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Grade = table.Column<byte>(type: "tinyint", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Number_of_session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirement", x => x.Requirement_id);
                    table.ForeignKey(
                        name: "FK_Requirement_Subject_Subject_name",
                        column: x => x.Subject_name,
                        principalTable: "Subject",
                        principalColumn: "Subject_name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requirement_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Student_id",
                table: "Classes",
                column: "Student_id");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Subject_name",
                table: "Classes",
                column: "Subject_name");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Tutor_id",
                table: "Classes",
                column: "Tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_UserId",
                table: "Classes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_User_id",
                table: "Order",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Otp_User_id",
                table: "Otp",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Qualification_User_id",
                table: "Qualification",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Report_User_id",
                table: "Report",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Requirement_Subject_name",
                table: "Requirement",
                column: "Subject_name");

            migrationBuilder.CreateIndex(
                name: "IX_Requirement_User_id",
                table: "Requirement",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_Subject_name",
                table: "Subject",
                column: "Subject_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Otp");

            migrationBuilder.DropTable(
                name: "Qualification");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Requirement");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
