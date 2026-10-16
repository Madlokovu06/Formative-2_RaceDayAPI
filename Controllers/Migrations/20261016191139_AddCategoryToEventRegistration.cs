
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceDayAPI.Migrations
{
    public partial class AddCategoryToEventRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Registration",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventID",
                table: "Category",
                type: "int",
                nullable: true);

            // Assign existing categories to their events.
            migrationBuilder.Sql(@"
                UPDATE dbo.Category
                SET EventID = CASE CategoryID
                    WHEN 1 THEN 2
                    WHEN 2 THEN 3
                    WHEN 3 THEN 4
                    WHEN 4 THEN 6
                END
                WHERE CategoryID IN (1, 2, 3, 4);
            ");

            // Give Event 6 its own category copied from Category 1.
            migrationBuilder.Sql(@"
                INSERT INTO dbo.Category (CategoryName, Description, EventID)
                SELECT CategoryName, Description, 6
                FROM dbo.Category
                WHERE CategoryID = 1;
            ");

            // Assign each existing registration to its event's category.
            migrationBuilder.Sql(@"
                UPDATE r
                SET r.CategoryID = c.CategoryID
                FROM dbo.Registration AS r
                INNER JOIN dbo.[Event] AS e
                    ON r.EventID = e.EventID
                INNER JOIN dbo.Category AS c
                    ON c.EventID = e.EventID
                    AND c.CategoryID = e.CategoryID;
            ");

            // Drop only the old foreign key that blocks removal
            // of Event.CategoryID. Preserve all other constraints.
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1
                    FROM sys.foreign_keys
                    WHERE name = N'FK__Event__CategoryI__628FA481'
                      AND parent_object_id = OBJECT_ID(N'dbo.Event')
                )
                BEGIN
                    ALTER TABLE dbo.[Event]
                    DROP CONSTRAINT [FK__Event__CategoryI__628FA481];
                END;
            ");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Event");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Registration",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventID",
                table: "Category",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registration_CategoryID",
                table: "Registration",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_EventID",
                table: "Category",
                column: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Event_EventID",
                table: "Category",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_Category_CategoryID",
                table: "Registration",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Event_EventID",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Registration_Category_CategoryID",
                table: "Registration");

            migrationBuilder.DropIndex(
                name: "IX_Registration_CategoryID",
                table: "Registration");

            migrationBuilder.DropIndex(
                name: "IX_Category_EventID",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Registration");

            migrationBuilder.DropColumn(
                name: "EventID",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
