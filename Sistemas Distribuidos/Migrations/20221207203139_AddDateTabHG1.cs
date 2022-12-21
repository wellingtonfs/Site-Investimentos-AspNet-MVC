using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistemas_Distribuidos.Migrations
{
    public partial class AddDateTabHG1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "updateAt",
                table: "Taxas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updateAt",
                table: "Moedas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updateAt",
                table: "Indices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updateAt",
                table: "Corretoras",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "updateAt",
                table: "Taxas");

            migrationBuilder.DropColumn(
                name: "updateAt",
                table: "Moedas");

            migrationBuilder.DropColumn(
                name: "updateAt",
                table: "Indices");

            migrationBuilder.DropColumn(
                name: "updateAt",
                table: "Corretoras");
        }
    }
}
