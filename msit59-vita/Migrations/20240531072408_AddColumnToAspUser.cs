using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace msit59_vita.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnToAspUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CusDis",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VitaUserName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CusDis",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VitaUserName",
                table: "AspNetUsers");
        }
    }
}
