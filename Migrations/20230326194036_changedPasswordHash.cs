using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAssignment.Migrations
{
    /// <inheritdoc />
    public partial class changedPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Assignments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 22, 40, 36, 343, DateTimeKind.Local).AddTicks(2413),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 4, 2, 18, 28, 35, 272, DateTimeKind.Local).AddTicks(2965));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Assignments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 18, 28, 35, 272, DateTimeKind.Local).AddTicks(2965),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 4, 2, 22, 40, 36, 343, DateTimeKind.Local).AddTicks(2413));
        }
    }
}
