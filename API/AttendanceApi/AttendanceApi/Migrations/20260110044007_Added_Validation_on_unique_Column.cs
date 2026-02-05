using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceApi.Migrations
{
    /// <inheritdoc />
    public partial class Added_Validation_on_unique_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Employees_EmailId", table: "Employees");

            migrationBuilder.DropIndex(name: "IX_Employees_EmployeeCode", table: "Employees");

            migrationBuilder.DropIndex(name: "IX_Employees_Username", table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmailId",
                table: "Employees",
                column: "EmailId",
                unique: true,
                filter: "[IsDeleted] = 0"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees",
                column: "EmployeeCode",
                unique: true,
                filter: "[IsDeleted] = 0"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhoneNumber",
                table: "Employees",
                column: "PhoneNumber",
                unique: true,
                filter: "[IsDeleted] = 0"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Username",
                table: "Employees",
                column: "Username",
                unique: true,
                filter: "[IsDeleted] = 0"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Employees_EmailId", table: "Employees");

            migrationBuilder.DropIndex(name: "IX_Employees_EmployeeCode", table: "Employees");

            migrationBuilder.DropIndex(name: "IX_Employees_PhoneNumber", table: "Employees");

            migrationBuilder.DropIndex(name: "IX_Employees_Username", table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmailId",
                table: "Employees",
                column: "EmailId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees",
                column: "EmployeeCode",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Username",
                table: "Employees",
                column: "Username",
                unique: true
            );
        }
    }
}
