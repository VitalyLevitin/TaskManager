using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAssignment.Migrations
{
    /// <inheritdoc />
    public partial class changedUsertoString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Assignments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 3, 0, 41, 53, 939, DateTimeKind.Local).AddTicks(7330),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 4, 2, 22, 45, 26, 406, DateTimeKind.Local).AddTicks(2838));

            migrationBuilder.AddColumn<string>(
                name: "AssignedToUser",
                table: "Assignments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToUser",
                table: "Assignments");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "longblob",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "longblob",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Assignments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 22, 45, 26, 406, DateTimeKind.Local).AddTicks(2838),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 4, 3, 0, 41, 53, 939, DateTimeKind.Local).AddTicks(7330));
        }
    }
}
