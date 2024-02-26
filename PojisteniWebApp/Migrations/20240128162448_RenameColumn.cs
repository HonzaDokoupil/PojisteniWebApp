using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PojisteniWebApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Policyholder",
                table: "InsuredPersons",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "InsuredPersons",
                newName: "Policyholder");
        }
    }
}
