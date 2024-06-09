using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTeacher.Migrations
{
    /// <inheritdoc />
    public partial class DbInt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_student_id",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "student_id1",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "Classes",
                newName: "Student_id");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_student_id",
                table: "Classes",
                newName: "IX_Classes_Student_id");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Tutor_id",
                table: "Classes",
                column: "Tutor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_Student_id",
                table: "Classes",
                column: "Student_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_Tutor_id",
                table: "Classes",
                column: "Tutor_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_Student_id",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_Tutor_id",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_Tutor_id",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "Student_id",
                table: "Classes",
                newName: "student_id");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_Student_id",
                table: "Classes",
                newName: "IX_Classes_student_id");

            migrationBuilder.AddColumn<string>(
                name: "student_id1",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_student_id",
                table: "Classes",
                column: "student_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
