using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistemas_Distribuidos.Migrations
{
    public partial class AddTagTabHG1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updateAt",
                table: "Taxas",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "selic",
                table: "Taxas",
                newName: "Selic");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Taxas",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "cdi",
                table: "Taxas",
                newName: "Cdi");

            migrationBuilder.RenameColumn(
                name: "selic_daily",
                table: "Taxas",
                newName: "SelicDaily");

            migrationBuilder.RenameColumn(
                name: "daily_factor",
                table: "Taxas",
                newName: "DailyFactor");

            migrationBuilder.RenameColumn(
                name: "cdi_daily",
                table: "Taxas",
                newName: "CdiDaily");

            migrationBuilder.RenameColumn(
                name: "variation",
                table: "Moedas",
                newName: "Variation");

            migrationBuilder.RenameColumn(
                name: "updateAt",
                table: "Moedas",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "sell",
                table: "Moedas",
                newName: "Sell");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Moedas",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "buy",
                table: "Moedas",
                newName: "Buy");

            migrationBuilder.RenameColumn(
                name: "variation",
                table: "Indices",
                newName: "Variation");

            migrationBuilder.RenameColumn(
                name: "updateAt",
                table: "Indices",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "points",
                table: "Indices",
                newName: "Points");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Indices",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "Indices",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "variation",
                table: "Corretoras",
                newName: "Variation");

            migrationBuilder.RenameColumn(
                name: "updateAt",
                table: "Corretoras",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "sell",
                table: "Corretoras",
                newName: "Sell");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Corretoras",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "last",
                table: "Corretoras",
                newName: "Last");

            migrationBuilder.RenameColumn(
                name: "buy",
                table: "Corretoras",
                newName: "Buy");

            migrationBuilder.RenameColumn(
                name: "format_moeda",
                table: "Corretoras",
                newName: "Tag");

            migrationBuilder.RenameColumn(
                name: "format_idioma",
                table: "Corretoras",
                newName: "FormatMoeda");

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Moedas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Indices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FormatIdioma",
                table: "Corretoras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Moedas");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Indices");

            migrationBuilder.DropColumn(
                name: "FormatIdioma",
                table: "Corretoras");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Taxas",
                newName: "updateAt");

            migrationBuilder.RenameColumn(
                name: "Selic",
                table: "Taxas",
                newName: "selic");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Taxas",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Cdi",
                table: "Taxas",
                newName: "cdi");

            migrationBuilder.RenameColumn(
                name: "SelicDaily",
                table: "Taxas",
                newName: "selic_daily");

            migrationBuilder.RenameColumn(
                name: "DailyFactor",
                table: "Taxas",
                newName: "daily_factor");

            migrationBuilder.RenameColumn(
                name: "CdiDaily",
                table: "Taxas",
                newName: "cdi_daily");

            migrationBuilder.RenameColumn(
                name: "Variation",
                table: "Moedas",
                newName: "variation");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Moedas",
                newName: "updateAt");

            migrationBuilder.RenameColumn(
                name: "Sell",
                table: "Moedas",
                newName: "sell");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Moedas",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Buy",
                table: "Moedas",
                newName: "buy");

            migrationBuilder.RenameColumn(
                name: "Variation",
                table: "Indices",
                newName: "variation");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Indices",
                newName: "updateAt");

            migrationBuilder.RenameColumn(
                name: "Points",
                table: "Indices",
                newName: "points");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Indices",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Indices",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Variation",
                table: "Corretoras",
                newName: "variation");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Corretoras",
                newName: "updateAt");

            migrationBuilder.RenameColumn(
                name: "Sell",
                table: "Corretoras",
                newName: "sell");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Corretoras",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Last",
                table: "Corretoras",
                newName: "last");

            migrationBuilder.RenameColumn(
                name: "Buy",
                table: "Corretoras",
                newName: "buy");

            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "Corretoras",
                newName: "format_moeda");

            migrationBuilder.RenameColumn(
                name: "FormatMoeda",
                table: "Corretoras",
                newName: "format_idioma");
        }
    }
}
