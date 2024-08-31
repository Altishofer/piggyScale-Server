using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiggyScaleApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    userid = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    userpassword = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "weight",
                columns: table => new
                {
                    weightid = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    datetime = table.Column<string>(type: "TEXT", nullable: false),
                    box = table.Column<uint>(type: "INTEGER", nullable: false),
                    weight = table.Column<float>(type: "REAL", nullable: false),
                    stddev = table.Column<float>(type: "REAL", nullable: false),
                    userid = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight", x => x.weightid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_userid",
                table: "user",
                column: "userid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weight_weightid",
                table: "weight",
                column: "weightid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "weight");
        }
    }
}
