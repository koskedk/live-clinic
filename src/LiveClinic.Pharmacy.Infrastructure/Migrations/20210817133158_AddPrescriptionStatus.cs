using Microsoft.EntityFrameworkCore.Migrations;

namespace LiveClinic.Pharmacy.Infrastructure.Migrations
{
    public partial class AddPrescriptionStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PrescriptionOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PrescriptionOrders");
        }
    }
}
