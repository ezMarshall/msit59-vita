using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace msit59_vita.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCustomerToVitaUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCustomer",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCustomer",
                table: "AspNetUsers");
        }
    }
}
