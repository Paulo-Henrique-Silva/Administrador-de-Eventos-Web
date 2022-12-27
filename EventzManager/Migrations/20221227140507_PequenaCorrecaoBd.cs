using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventzManager.Migrations
{
    /// <inheritdoc />
    public partial class PequenaCorrecaoBd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tb_usuarios_email",
                table: "tb_usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_usuarios_email",
                table: "tb_usuarios");
        }
    }
}
