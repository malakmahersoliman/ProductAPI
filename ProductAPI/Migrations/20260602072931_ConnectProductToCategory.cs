using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class ConnectProductToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add CategoryId as nullable first so existing rows do not break.
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: true);

            // 2. Make sure a fallback category exists.
            migrationBuilder.Sql(@"
                    IF NOT EXISTS (SELECT 1 FROM Categories WHERE Name = 'General')
                    BEGIN
                        INSERT INTO Categories (Name, Description)
                        VALUES ('General', 'Default category for existing products')
                    END
                ");

            // 3. Create category rows from old product Category strings.
            // This preserves your old text categories as real category records.
            migrationBuilder.Sql(@"
        INSERT INTO Categories (Name, Description)
        SELECT DISTINCT LTRIM(RTRIM(p.Category)), 'Migrated from product category text'
        FROM Products p
        WHERE p.Category IS NOT NULL
          AND LTRIM(RTRIM(p.Category)) <> ''
          AND NOT EXISTS (
              SELECT 1
              FROM Categories c
              WHERE c.Name = LTRIM(RTRIM(p.Category))
          )
    ");

            // 4. Link products to matching category by old Category text.
            migrationBuilder.Sql(@"
        UPDATE p
        SET p.CategoryId = c.Id
        FROM Products p
        INNER JOIN Categories c
            ON c.Name = LTRIM(RTRIM(p.Category))
        WHERE p.Category IS NOT NULL
          AND LTRIM(RTRIM(p.Category)) <> ''
    ");

            // 5. Any product still without category goes to General.
            migrationBuilder.Sql(@"
                    UPDATE Products
                    SET CategoryId = (SELECT TOP 1 Id FROM Categories WHERE Name = 'General')
                    WHERE CategoryId IS NULL
                ");

            // 6. Now make CategoryId required.
            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // 7. Add index.
            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            // 8. Add FK with Restrict.
            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // 9. Drop old string column after data has been migrated.
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                        UPDATE p
                        SET p.Category = c.Name
                        FROM Products p
                        INNER JOIN Categories c
                            ON p.CategoryId = c.Id
                    ");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");
        }
    }
}
