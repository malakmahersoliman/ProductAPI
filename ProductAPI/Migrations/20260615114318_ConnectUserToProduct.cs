using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class ConnectUserToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.Sql("""
                                UPDATE Orders
                                SET CreatedById = (SELECT TOP 1 Id FROM Users ORDER BY Id)
                                WHERE CreatedById IS NULL
                            """);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedById",
                table: "Orders",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CreatedById",
                table: "Orders",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CreatedById",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CreatedById",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Orders");
        }
    }
}
