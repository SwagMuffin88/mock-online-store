using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TARge25Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFileSupportForKindergarten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpaceshipId",
                table: "FilesToApis",
                newName: "ObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ObjectId",
                table: "FilesToApis",
                newName: "SpaceshipId");
        }
    }
}
