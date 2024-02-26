using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PojisteniWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ReportsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceTypesCount = table.Column<int>(type: "int", nullable: false),
                    InsuredPersonsCount = table.Column<int>(type: "int", nullable: false),
                    IndividualContractsCount = table.Column<int>(type: "int", nullable: false),
                    InsuranceEventsCount = table.Column<int>(type: "int", nullable: false),
                    UsersCount = table.Column<int>(type: "int", nullable: false),
                    InsuredValueTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DamageValueTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PersonsWithNoContractsCount = table.Column<int>(type: "int", nullable: false),
                    PersonsWithNoEventCount = table.Column<int>(type: "int", nullable: false),
                    UseresWithNoRole = table.Column<int>(type: "int", nullable: false),
                    InsuredPersonsWithNoAccount = table.Column<int>(type: "int", nullable: false),
                    PersonWithHighestInsuredValueTotal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonWithLowestInsuredValueTotal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonWithHighestDamageValueTotal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonWithLowestDamageValueTotal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MostContractedInsurenceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeastContractedInsurenceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
