using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class Shared : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shared.Person",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Firstname = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    Lastname = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    TransStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shared.Person", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shared.Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Access = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    TransStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shared.Role", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shared.User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Username = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    PersonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Start = table.Column<DateTime>(type: "date", nullable: false),
                    End = table.Column<DateTime>(type: "date", nullable: false),
                    TransStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shared.User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shared.User_Shared.Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Shared.Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shared.Log",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IP = table.Column<string>(type: "varchar(45)", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "longtext", nullable: false),
                    TransStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shared.Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shared.Log_Shared.User_UserId",
                        column: x => x.UserId,
                        principalTable: "Shared.User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shared.User+Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TransStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shared.User+Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shared.User+Role_Shared.Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Shared.Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shared.User+Role_Shared.User_UserId",
                        column: x => x.UserId,
                        principalTable: "Shared.User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shared.History",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Table = table.Column<string>(type: "longtext", nullable: false),
                    LogId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Data = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    TransStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shared.History", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shared.History_Shared.Log_LogId",
                        column: x => x.LogId,
                        principalTable: "Shared.Log",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Shared.History_LogId",
                table: "Shared.History",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_Shared.Log_UserId",
                table: "Shared.Log",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shared.User_PersonId",
                table: "Shared.User",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Shared.User+Role_RoleId",
                table: "Shared.User+Role",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Shared.User+Role_UserId",
                table: "Shared.User+Role",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shared.History");

            migrationBuilder.DropTable(
                name: "Shared.User+Role");

            migrationBuilder.DropTable(
                name: "Shared.Log");

            migrationBuilder.DropTable(
                name: "Shared.Role");

            migrationBuilder.DropTable(
                name: "Shared.User");

            migrationBuilder.DropTable(
                name: "Shared.Person");
        }
    }
}
