using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LiveClinic.Pharmacy.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Movement = table.Column<int>(type: "int", nullable: false),
                    MovementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    OrderRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrugId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionOrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrugCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Days = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    QuantityDispensed = table.Column<double>(type: "float", nullable: false),
                    DrugId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrescriptionOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrescriptionOrderItems_PrescriptionOrders_PrescriptionOrderId",
                        column: x => x.PrescriptionOrderId,
                        principalTable: "PrescriptionOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionOrderItems_PrescriptionOrderId",
                table: "PrescriptionOrderItems",
                column: "PrescriptionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_DrugId",
                table: "StockTransactions",
                column: "DrugId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionOrderItems");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "PrescriptionOrders");

            migrationBuilder.DropTable(
                name: "Drugs");
        }
    }
}
