using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DoctorID = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    TimeSlot = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Schedules_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DoctorID = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleID = table.Column<int>(type: "INTEGER", nullable: true),
                    PatientName = table.Column<string>(type: "TEXT", nullable: false),
                    AppointmentDate = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Schedules_ScheduleID",
                        column: x => x.ScheduleID,
                        principalTable: "Schedules",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "ID", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "kovalenko@clinic.ua", "Др. Олена Коваленко", "hash_doctor_1" },
                    { 2, "melnyk@clinic.ua", "Др. Андрій Мельник", "hash_doctor_2" },
                    { 3, "bondar@clinic.ua", "Др. Ірина Бондар", "hash_doctor_3" }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "ID", "Date", "DoctorID", "Status", "TimeSlot" },
                values: new object[,]
                {
                    { 1, "2026-11-20", 1, "Available", "09:00 - 10:00" },
                    { 2, "2026-11-20", 1, "Booked", "10:00 - 11:00" },
                    { 3, "2026-11-21", 2, "Available", "12:00 - 13:00" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "ID", "AppointmentDate", "DoctorID", "PatientName", "ScheduleID", "Status" },
                values: new object[,]
                {
                    { 1, "2026-11-20 10:00", 1, "Іван Петренко", 2, "Confirmed" },
                    { 2, "2026-11-20 09:00", 1, "Марія Сидоренко", 1, "Pending" },
                    { 3, "2026-11-21 12:00", 2, "Олег Ткаченко", 3, "Pending" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorID",
                table: "Appointments",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ScheduleID",
                table: "Appointments",
                column: "ScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Email",
                table: "Doctors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DoctorID",
                table: "Schedules",
                column: "DoctorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Doctors");
        }
    }
}
