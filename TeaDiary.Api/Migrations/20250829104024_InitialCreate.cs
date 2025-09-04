using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeaDiary.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeaTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeaTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AvatarId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TeaTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    YearCollection = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    LinkPurchase = table.Column<string>(type: "text", nullable: true),
                    WouldBuyAgain = table.Column<bool>(type: "boolean", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkToPhoto = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teas_TeaTypes_TeaTypeId",
                        column: x => x.TeaTypeId,
                        principalTable: "TeaTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Teas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Impressions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TeaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impressions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impressions_Teas_TeaId",
                        column: x => x.TeaId,
                        principalTable: "Teas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Impressions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TeaTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, "Зеленый" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, "Черный" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, "Красный" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), null, "Улун" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), null, "Шу пуэр" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), null, "Шен пуэр" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), null, "Габа" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), null, "Белый" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Impressions_TeaId",
                table: "Impressions",
                column: "TeaId");

            migrationBuilder.CreateIndex(
                name: "IX_Impressions_UserId",
                table: "Impressions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teas_TeaTypeId",
                table: "Teas",
                column: "TeaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Teas_UserId",
                table: "Teas",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Impressions");

            migrationBuilder.DropTable(
                name: "Teas");

            migrationBuilder.DropTable(
                name: "TeaTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
