using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceApi.Migrations
{
    /// <inheritdoc />
    public partial class ImproveAttendanceSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "Attendances",
                newName: "CheckInPhotoPath"
            );

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Attendances",
                newName: "CheckInLongitude"
            );

            migrationBuilder.RenameColumn(
                name: "LocationAddress",
                table: "Attendances",
                newName: "CheckInLocationAddress"
            );

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Attendances",
                newName: "CheckInLatitude"
            );

            migrationBuilder.AddColumn<double>(
                name: "CheckOutLatitude",
                table: "Attendances",
                type: "float",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "CheckOutLocationAddress",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true
            );

            migrationBuilder.AddColumn<double>(
                name: "CheckOutLongitude",
                table: "Attendances",
                type: "float",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "CheckOutPhotoPath",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Attendances",
                type: "datetime2",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "CheckOutLatitude", table: "Attendances");

            migrationBuilder.DropColumn(name: "CheckOutLocationAddress", table: "Attendances");

            migrationBuilder.DropColumn(name: "CheckOutLongitude", table: "Attendances");

            migrationBuilder.DropColumn(name: "CheckOutPhotoPath", table: "Attendances");

            migrationBuilder.DropColumn(name: "RejectionReason", table: "Attendances");

            migrationBuilder.DropColumn(name: "VerifiedAt", table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "CheckInPhotoPath",
                table: "Attendances",
                newName: "PhotoPath"
            );

            migrationBuilder.RenameColumn(
                name: "CheckInLongitude",
                table: "Attendances",
                newName: "Longitude"
            );

            migrationBuilder.RenameColumn(
                name: "CheckInLocationAddress",
                table: "Attendances",
                newName: "LocationAddress"
            );

            migrationBuilder.RenameColumn(
                name: "CheckInLatitude",
                table: "Attendances",
                newName: "Latitude"
            );
        }
    }
}
