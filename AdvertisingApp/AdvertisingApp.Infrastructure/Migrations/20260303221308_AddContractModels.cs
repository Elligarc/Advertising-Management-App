using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AdvertisingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContractModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contract_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SurfaceId = table.Column<int>(type: "integer", nullable: false),
                    ContractId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceType = table.Column<int>(type: "integer", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractItem_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractItem_Surface_SurfaceId",
                        column: x => x.SurfaceId,
                        principalTable: "Surface",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractItemSchedule",
                columns: table => new
                {
                    ContractItemId = table.Column<int>(type: "integer", nullable: false),
                    DaysOfWeek = table.Column<string>(type: "text", nullable: false),
                    HoursInDay = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractItemSchedule", x => x.ContractItemId);
                    table.ForeignKey(
                        name: "FK_ContractItemSchedule_ContractItem_ContractItemId",
                        column: x => x.ContractItemId,
                        principalTable: "ContractItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractRegistry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContractItemId = table.Column<int>(type: "integer", nullable: false),
                    ContractId = table.Column<int>(type: "integer", nullable: false),
                    ContractStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ContractEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ContractTotalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ContractStatus = table.Column<int>(type: "integer", nullable: false),
                    ItemStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ItemEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ItemPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ItemPriceType = table.Column<int>(type: "integer", nullable: false),
                    ItemTotalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SurfaceId = table.Column<int>(type: "integer", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    ClientName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClientPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractRegistry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractRegistry_ContractItem_ContractItemId",
                        column: x => x.ContractItemId,
                        principalTable: "ContractItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ClientId",
                table: "Contract",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractItem_ContractId",
                table: "ContractItem",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractItem_SurfaceId",
                table: "ContractItem",
                column: "SurfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractRegistry_ClientId",
                table: "ContractRegistry",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractRegistry_ContractItemId",
                table: "ContractRegistry",
                column: "ContractItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractRegistry_ContractStatus",
                table: "ContractRegistry",
                column: "ContractStatus");

            migrationBuilder.CreateIndex(
                name: "IX_ContractRegistry_ItemEndDate",
                table: "ContractRegistry",
                column: "ItemEndDate");

            migrationBuilder.CreateIndex(
                name: "IX_ContractRegistry_ItemStartDate",
                table: "ContractRegistry",
                column: "ItemStartDate");

            migrationBuilder.CreateIndex(
                name: "IX_ContractRegistry_SurfaceId",
                table: "ContractRegistry",
                column: "SurfaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractItemSchedule");

            migrationBuilder.DropTable(
                name: "ContractRegistry");

            migrationBuilder.DropTable(
                name: "ContractItem");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
