using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactordatebase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailabilityDoctor");

            migrationBuilder.DropTable(
                name: "DoctorTimeSlot");

            migrationBuilder.DropTable(
                name: "Availabilities");

            migrationBuilder.DropColumn(
                name: "From",
                table: "TimeSlots");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "TimeSlots");

            migrationBuilder.DropColumn(
                name: "AppointmentDuration",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "TimeSlots",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "ProfilePecture",
                table: "AspNetUsers",
                newName: "ProfilePicture");

            migrationBuilder.AddColumn<string>(
                name: "PationDetails_AgeRange",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PationDetails_FullName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PationDetails_ProblemDescription",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PationDetails_gender",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TimeSlotID",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TimeSlotID",
                table: "Appointments",
                column: "TimeSlotID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_TimeSlots_TimeSlotID",
                table: "Appointments",
                column: "TimeSlotID",
                principalTable: "TimeSlots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_TimeSlots_TimeSlotID",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_TimeSlotID",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PationDetails_AgeRange",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PationDetails_FullName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PationDetails_ProblemDescription",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PationDetails_gender",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "TimeSlotID",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "TimeSlots",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "AspNetUsers",
                newName: "ProfilePecture");

            migrationBuilder.AddColumn<DateTime>(
                name: "From",
                table: "TimeSlots",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "TimeSlots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDuration",
                table: "Doctors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Duration",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorTimeSlot",
                columns: table => new
                {
                    DoctorsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeSlotsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorTimeSlot", x => new { x.DoctorsId, x.TimeSlotsId });
                    table.ForeignKey(
                        name: "FK_DoctorTimeSlot_Doctors_DoctorsId",
                        column: x => x.DoctorsId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorTimeSlot_TimeSlots_TimeSlotsId",
                        column: x => x.TimeSlotsId,
                        principalTable: "TimeSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AvailabilityDoctor",
                columns: table => new
                {
                    AvailabilitiesId = table.Column<int>(type: "int", nullable: false),
                    DoctorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilityDoctor", x => new { x.AvailabilitiesId, x.DoctorsId });
                    table.ForeignKey(
                        name: "FK_AvailabilityDoctor_Availabilities_AvailabilitiesId",
                        column: x => x.AvailabilitiesId,
                        principalTable: "Availabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailabilityDoctor_Doctors_DoctorsId",
                        column: x => x.DoctorsId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityDoctor_DoctorsId",
                table: "AvailabilityDoctor",
                column: "DoctorsId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorTimeSlot_TimeSlotsId",
                table: "DoctorTimeSlot",
                column: "TimeSlotsId");
        }
    }
}
