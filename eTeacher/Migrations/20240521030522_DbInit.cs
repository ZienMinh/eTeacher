using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTeacher.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    class_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    student_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    tutor_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subject_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subject_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    type_class = table.Column<byte>(type: "tinyint", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    number_of_session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.class_id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    order_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    wallet_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    order_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    class_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    order_type = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.order_id);
                });

            migrationBuilder.CreateTable(
                name: "Qualification",
                columns: table => new
                {
                    qualification_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    graduation_year = table.Column<int>(type: "int", nullable: false),
                    specialize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    classification = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    training_facility = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualification", x => x.qualification_id);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    report_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    content = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.report_id);
                });

            migrationBuilder.CreateTable(
                name: "Requirement",
                columns: table => new
                {
                    requirement_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subject_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subject_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    grade = table.Column<byte>(type: "tinyint", nullable: false),
                    rank = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    number_of_session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirement", x => x.requirement_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    user_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    gender = table.Column<byte>(type: "tinyint", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    phone_number = table.Column<int>(type: "int", nullable: false),
                    wallet_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    link_contact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rating = table.Column<byte>(type: "tinyint", nullable: false),
                    image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    role = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    wallet_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    balance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.wallet_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Users");

            migrationBuilder.DropTable(
                name: "Wallet");
        }
    }
}
