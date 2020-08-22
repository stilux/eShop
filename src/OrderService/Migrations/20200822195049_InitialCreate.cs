using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace OrderService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderStatusId = table.Column<byte>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    Customer = table.Column<string>(nullable: false),
                    PaymentMethodId = table.Column<byte>(nullable: true),
                    DeliveryMethod = table.Column<string>(nullable: true),
                    DeliveryAddress = table.Column<string>(nullable: true),
                    PlannedDeliveryDate = table.Column<DateTime>(nullable: true),
                    ReserveId = table.Column<int>(nullable: true),
                    TotalPrice = table.Column<float>(nullable: false, defaultValue: 0f),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatus_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<short>(nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatusId",
                table: "Orders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentMethodId",
                table: "Orders",
                column: "PaymentMethodId");
            
            
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "New" });
            
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Submitted" });
            
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "ProductReserved" });
            
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Paid" });
            
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Shipped" });
            
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Delivered" });
            
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "Cancelled" });
            
            
            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Online" });
            
            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "PaymentOnDelivery" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "PaymentMethod");
        }
    }
}
