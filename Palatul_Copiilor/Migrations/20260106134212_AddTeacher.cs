using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Palatul_Copiilor.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Activity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_TeacherId",
                table: "Activity",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Teacher_TeacherId",
                table: "Activity",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Teacher_TeacherId",
                table: "Activity");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Activity_TeacherId",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Activity");
        }
    }
}
