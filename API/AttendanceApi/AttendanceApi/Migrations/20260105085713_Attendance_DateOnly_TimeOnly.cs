using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceApi.Migrations
{
    /// <inheritdoc />
    public partial class Attendance_DateOnly_TimeOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Attendances",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2"
            );

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "CheckOutTime",
                table: "Attendances",
                type: "time",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "CheckInTime",
                table: "Attendances",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Attendances",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOutTime",
                table: "Attendances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                table: "Attendances",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time"
            );
        }
    }
}
