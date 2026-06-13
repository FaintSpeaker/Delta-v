using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class PreferenceScopedJobsAntags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dv_player_antags",
                columns: table => new
                {
                    dv_player_antags_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    preference_id = table.Column<int>(type: "INTEGER", nullable: false),
                    antag_name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dv_player_antags", x => x.dv_player_antags_id);
                    table.ForeignKey(
                        name: "FK_dv_player_antags_preference_preference_id",
                        column: x => x.preference_id,
                        principalTable: "preference",
                        principalColumn: "preference_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dv_player_jobs",
                columns: table => new
                {
                    dv_player_jobs_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    preference_id = table.Column<int>(type: "INTEGER", nullable: false),
                    job_name = table.Column<string>(type: "TEXT", nullable: false),
                    priority = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dv_player_jobs", x => x.dv_player_jobs_id);
                    table.ForeignKey(
                        name: "FK_dv_player_jobs_preference_preference_id",
                        column: x => x.preference_id,
                        principalTable: "preference",
                        principalColumn: "preference_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dv_player_antags_preference_id_antag_name",
                table: "dv_player_antags",
                columns: new[] { "preference_id", "antag_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dv_player_jobs_preference_id",
                table: "dv_player_jobs",
                column: "preference_id");

            migrationBuilder.CreateIndex(
                name: "IX_dv_player_jobs_preference_id_job_name",
                table: "dv_player_jobs",
                columns: new[] { "preference_id", "job_name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dv_player_antags");

            migrationBuilder.DropTable(
                name: "dv_player_jobs");
        }
    }
}
