using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 17, 14, 27, 31, 401, DateTimeKind.Utc).AddTicks(8197), "$2a$11$pWOJdTTUgcfHiGLAMpe9SuHuMWGrDPyekTABwtEJlsHQFrrBePnz6", "Admin", "admin" },
                    { 2, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(8168), "$2a$11$p1tEJaGn1OXEzUyPviSsLe302EJJoA1igBbKR0IDoMKfwmpoT6Ayi", "Viewer", "viewer" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedAt", "Department", "Designation", "Email", "FirstName", "JoinDate", "LastName", "Phone", "Salary", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9786), "Engineering", "Software Engineer", "rajak@gmail.com", "Raja", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chinnapu", "9376543210", 580000m, "Active", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9788) },
                    { 2, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9820), "Marketing", "Marketing Exec", "roopam@gmail.com", "Roopa", new DateTime(2020, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chinnapu", "9523456780", 760000m, "InActive", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9821) },
                    { 4, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9827), "HR", "HR Executive", "vishnu@gmail.com", "Vishnu", new DateTime(2019, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kasireddy", "9676512340", 850000m, "Active", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9827) },
                    { 5, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9831), "HR", "HR Executive", "venkata@gmail.com", "Srinath", new DateTime(2019, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Venkata", "9576511340", 950000m, "InActive", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9831) },
                    { 6, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9835), "HR", "HR Executive", "rana@gmail.com", "keerthi", new DateTime(2019, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Renigunta", "8576512341", 750000m, "InActive", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9835) },
                    { 7, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9838), "Engineering", "Junior Developer", "keerati@gmail.com", "Yamini", new DateTime(2022, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kanuma", "9076512330", 650000m, "Active", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9838) },
                    { 8, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9842), "Marketing", "Agent Operator", "maruthi@gmail.com", "Kumar", new DateTime(2022, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maruthi", "9116512342", 850000m, "InActive", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9844) },
                    { 9, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9847), "HR", "HR Executive", "tamatam@gmail.com", "Manish", new DateTime(2020, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thamatam", "9644452340", 550000m, "Active", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9848) },
                    { 10, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9851), "HR", "HR Executive", "hamsa@gmail.com", "Prakash", new DateTime(2022, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hamsa", "9111512340", 850000m, "Active", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9852) },
                    { 11, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9855), "Engineering", "Senior Developer", "katmurre@gmail.com", "Suresh", new DateTime(2020, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Katmurre", "9871512380", 800000m, "InActive", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9855) },
                    { 12, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9858), "HR", "HR Executive", "mavilla@gmail.com", "Vijay", new DateTime(2021, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mavilla", "9116512340", 750000m, "Active", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9859) },
                    { 13, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9862), "HR", "HR Executive", "thappeta@gmail.com", "Naresh", new DateTime(2019, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thappeta", "9676512340", 850000m, "Active", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9862) },
                    { 14, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9865), "HR", "HR Executive", "penderi@gmail.com", "Santhosh", new DateTime(2019, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Penderi", "9676512340", 850000m, "Active", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9865) },
                    { 15, new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9868), "HR", "HR Executive", "jitta@gmail.com", "Padhhu", new DateTime(2019, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jitta", "9676512340", 850000m, "Active", new DateTime(2026, 4, 17, 14, 27, 31, 626, DateTimeKind.Utc).AddTicks(9869) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
