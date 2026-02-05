using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceApi.Migrations
{
    /// <inheritdoc />
    public partial class FixRefreshTokenDateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsRevoked",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "RefreshTokens",
                type: "datetime2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "datetime2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsRevoked",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)"
            );
        }
    }
}
