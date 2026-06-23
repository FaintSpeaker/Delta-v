using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class ProfileEnabledStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "dv_profile_enabled",
                table: "profile",
                type: "boolean",
                nullable: false,
                defaultValue: false);
            
            migrationBuilder.Sql("""
                                UPDATE profile
                                SET dv_profile_enabled = true
                                FROM preference pref
                                WHERE pref.selected_character_slot = slot
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dv_profile_enabled",
                table: "profile");
        }
    }
}
