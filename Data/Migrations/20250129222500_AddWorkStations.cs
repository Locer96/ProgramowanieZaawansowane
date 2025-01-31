using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkStations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkStations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkStationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PCSerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Display = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Keyboard = table.Column<bool>(type: "bit", nullable: false),
                    Mouse = table.Column<bool>(type: "bit", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserWorkStations",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkStationId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserWorkStations", x => new { x.UserId, x.WorkStationId });
                    table.ForeignKey(
                        name: "FK_AspNetUserWorkStations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserWorkStations_WorkStations_WorkStationId",
                        column: x => x.WorkStationId,
                        principalTable: "WorkStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserWorkStations_WorkStationId",
                table: "AspNetUserWorkStations",
                column: "WorkStationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkStations_WorkStationNumber",
                table: "WorkStations",
                column: "WorkStationNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserWorkStations");

            migrationBuilder.DropTable(
                name: "WorkStations");
        }
    }
}
