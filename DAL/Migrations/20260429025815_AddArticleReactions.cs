using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articlereactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    articleid = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    islike = table.Column<bool>(type: "boolean", nullable: false),
                    reactedatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlereactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_articlereactions_newsarticles_articleid",
                        column: x => x.articleid,
                        principalTable: "newsarticles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_articlereactions_articleid_userid",
                table: "articlereactions",
                columns: new[] { "articleid", "userid" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articlereactions");
        }
    }
}
