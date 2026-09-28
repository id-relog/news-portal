using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedname = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrencystamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedusername = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedemail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    emailconfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    passwordhash = table.Column<string>(type: "text", nullable: true),
                    securitystamp = table.Column<string>(type: "text", nullable: true),
                    concurrencystamp = table.Column<string>(type: "text", nullable: true),
                    phonenumber = table.Column<string>(type: "text", nullable: true),
                    phonenumberconfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    twofactorenabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockoutend = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockoutenabled = table.Column<bool>(type: "boolean", nullable: false),
                    accessfailedcount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "newscategories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    slug = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    createdatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newscategories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "newssources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    istrusted = table.Column<bool>(type: "boolean", nullable: false),
                    createdatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newssources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "newstags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    slug = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    createdatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newstags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userbans",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    reason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    evidencemessage = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    evidencecommentid = table.Column<Guid>(type: "uuid", nullable: true),
                    bannedatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    bannedbyuserid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userbans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleid = table.Column<string>(type: "text", nullable: false),
                    claimtype = table.Column<string>(type: "text", nullable: true),
                    claimvalue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_roleid",
                        column: x => x.roleid,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<string>(type: "text", nullable: false),
                    claimtype = table.Column<string>(type: "text", nullable: true),
                    claimvalue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    loginprovider = table.Column<string>(type: "text", nullable: false),
                    providerkey = table.Column<string>(type: "text", nullable: false),
                    providerdisplayname = table.Column<string>(type: "text", nullable: true),
                    userid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.loginprovider, x.providerkey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    userid = table.Column<string>(type: "text", nullable: false),
                    roleid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.userid, x.roleid });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_roleid",
                        column: x => x.roleid,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    userid = table.Column<string>(type: "text", nullable: false),
                    loginprovider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.userid, x.loginprovider, x.name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categorysubscriptions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    categoryid = table.Column<Guid>(type: "uuid", nullable: false),
                    createdatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorysubscriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_categorysubscriptions_newscategories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "newscategories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "newsarticles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    categoryid = table.Column<Guid>(type: "uuid", nullable: false),
                    sourceid = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    slug = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    summary = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: false),
                    authoruserid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    publishedatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reviewedatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reviewedbyuserid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    reviewnote = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    createdatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newsarticles", x => x.id);
                    table.ForeignKey(
                        name: "FK_newsarticles_newscategories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "newscategories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_newsarticles_newssources_sourceid",
                        column: x => x.sourceid,
                        principalTable: "newssources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "articlecomments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    articleid = table.Column<Guid>(type: "uuid", nullable: false),
                    authoruserid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    displayname = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    body = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    createdatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    moderatedatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    moderatedbyuserid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlecomments", x => x.id);
                    table.ForeignKey(
                        name: "FK_articlecomments_newsarticles_articleid",
                        column: x => x.articleid,
                        principalTable: "newsarticles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "articlerevisions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    articleid = table.Column<Guid>(type: "uuid", nullable: false),
                    revisionnumber = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    summary = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: false),
                    editedbyuserid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    editedatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlerevisions", x => x.id);
                    table.ForeignKey(
                        name: "FK_articlerevisions_newsarticles_articleid",
                        column: x => x.articleid,
                        principalTable: "newsarticles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "articletags",
                columns: table => new
                {
                    articleid = table.Column<Guid>(type: "uuid", nullable: false),
                    tagid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articletags", x => new { x.articleid, x.tagid });
                    table.ForeignKey(
                        name: "FK_articletags_newsarticles_articleid",
                        column: x => x.articleid,
                        principalTable: "newsarticles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_articletags_newstags_tagid",
                        column: x => x.tagid,
                        principalTable: "newstags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "articleviewevents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    articleid = table.Column<Guid>(type: "uuid", nullable: false),
                    vieweruserid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    iphash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    useragent = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    viewedatutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articleviewevents", x => x.id);
                    table.ForeignKey(
                        name: "FK_articleviewevents_newsarticles_articleid",
                        column: x => x.articleid,
                        principalTable: "newsarticles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_articlecomments_articleid",
                table: "articlecomments",
                column: "articleid");

            migrationBuilder.CreateIndex(
                name: "IX_articlerevisions_articleid_revisionnumber",
                table: "articlerevisions",
                columns: new[] { "articleid", "revisionnumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_articletags_tagid",
                table: "articletags",
                column: "tagid");

            migrationBuilder.CreateIndex(
                name: "IX_articleviewevents_articleid",
                table: "articleviewevents",
                column: "articleid");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_roleid",
                table: "AspNetRoleClaims",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "normalizedname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_userid",
                table: "AspNetUserClaims",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_userid",
                table: "AspNetUserLogins",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_roleid",
                table: "AspNetUserRoles",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "normalizedemail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "normalizedusername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categorysubscriptions_categoryid",
                table: "categorysubscriptions",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_categorysubscriptions_userid_categoryid",
                table: "categorysubscriptions",
                columns: new[] { "userid", "categoryid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_newsarticles_categoryid",
                table: "newsarticles",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_newsarticles_slug",
                table: "newsarticles",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_newsarticles_sourceid",
                table: "newsarticles",
                column: "sourceid");

            migrationBuilder.CreateIndex(
                name: "IX_newscategories_slug",
                table: "newscategories",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_newstags_slug",
                table: "newstags",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userbans_userid_isactive",
                table: "userbans",
                columns: new[] { "userid", "isactive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articlecomments");

            migrationBuilder.DropTable(
                name: "articlerevisions");

            migrationBuilder.DropTable(
                name: "articletags");

            migrationBuilder.DropTable(
                name: "articleviewevents");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "categorysubscriptions");

            migrationBuilder.DropTable(
                name: "userbans");

            migrationBuilder.DropTable(
                name: "newstags");

            migrationBuilder.DropTable(
                name: "newsarticles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "newscategories");

            migrationBuilder.DropTable(
                name: "newssources");
        }
    }
}
