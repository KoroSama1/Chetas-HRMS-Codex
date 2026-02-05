using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceApi.Migrations
{
    /// <inheritdoc />
    public partial class Split_Status_Into_CheckIn_CheckOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Attendances",
                newName: "CheckInStatus"
            );

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "Attendances",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "CheckOutPhotoPath",
                table: "Attendances",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "CheckOutLocationAddress",
                table: "Attendances",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "CheckInPhotoPath",
                table: "Attendances",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)"
            );

            migrationBuilder.AlterColumn<string>(
                name: "CheckInLocationAddress",
                table: "Attendances",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)"
            );

            migrationBuilder.AddColumn<string>(
                name: "CheckOutStatus",
                table: "Attendances",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CheckInStatus",
                table: "Attendances",
                column: "CheckInStatus"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CheckOutStatus",
                table: "Attendances",
                column: "CheckOutStatus"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_Date",
                table: "Attendances",
                column: "Date"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Attendances_CheckInStatus", table: "Attendances");

            migrationBuilder.DropIndex(name: "IX_Attendances_CheckOutStatus", table: "Attendances");

            migrationBuilder.DropIndex(name: "IX_Attendances_Date", table: "Attendances");

            migrationBuilder.DropColumn(name: "CheckOutStatus", table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "CheckInStatus",
                table: "Attendances",
                newName: "Status"
            );

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "CheckOutPhotoPath",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "CheckOutLocationAddress",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "CheckInPhotoPath",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500
            );

            migrationBuilder.AlterColumn<string>(
                name: "CheckInLocationAddress",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300
            );
        }
    }
}
