namespace Perigee.EntityFramework.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Customer",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>("datetime2", nullable: false),
                    CreatedBy = table.Column<string>("nvarchar(100)", nullable: false),
                    UpdatedOn = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedBy = table.Column<string>("nvarchar(100)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Customer", x => x.Id); });

            migrationBuilder.CreateTable(
                "JobType",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_JobType", x => x.Id); });

            migrationBuilder.CreateTable(
                "Address",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Address1 = table.Column<string>("nvarchar(150)", maxLength: 150, nullable: false),
                    Address2 = table.Column<string>("nvarchar(150)", maxLength: 150, nullable: true),
                    Suburb = table.Column<string>("nvarchar(30)", maxLength: 30, nullable: false),
                    State = table.Column<string>("nvarchar(30)", maxLength: 30, nullable: false),
                    PostCode = table.Column<string>("nvarchar(15)", maxLength: 15, nullable: false),
                    Country = table.Column<string>("nvarchar(30)", maxLength: 30, nullable: false),
                    IsCurrent = table.Column<bool>("bit", nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>("datetime2", nullable: false),
                    CreatedBy = table.Column<string>("nvarchar(100)", nullable: false),
                    UpdatedOn = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedBy = table.Column<string>("nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        "FK_Address_Customer_CustomerId",
                        x => x.CustomerId,
                        "Customer",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "PhoneNumber",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(maxLength: 20, nullable: false),
                    IsCurrent = table.Column<bool>("bit", nullable: false),
                    IsMobile = table.Column<bool>("bit", nullable: false),
                    ReceiveTextMessages = table.Column<bool>("bit", nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>("datetime2", nullable: false),
                    CreatedBy = table.Column<string>("nvarchar(100)", nullable: false),
                    UpdatedOn = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedBy = table.Column<string>("nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id);
                    table.ForeignKey(
                        "FK_PhoneNumber_Customer_CustomerId",
                        x => x.CustomerId,
                        "Customer",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Job",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    JobId = table.Column<string>(maxLength: 1000, nullable: false),
                    BookedDateTime = table.Column<DateTime>("datetime2", nullable: false),
                    GearReceivedDateTime = table.Column<DateTime>("datetime2", nullable: true),
                    DeliveredDateTime = table.Column<DateTime>("datetime2", nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    EquipmentReceivedDescription = table.Column<string>(nullable: true),
                    JobTypeId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>("datetime2", nullable: false),
                    CreatedBy = table.Column<string>("nvarchar(100)", nullable: false),
                    UpdatedOn = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedBy = table.Column<string>("nvarchar(100)", nullable: false),
                    IsDeleted = table.Column<bool>("bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                    table.ForeignKey(
                        "FK_Job_Customer_CustomerId",
                        x => x.CustomerId,
                        "Customer",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Job_JobType_CustomerId",
                        x => x.CustomerId,
                        "JobType",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Address_CustomerId",
                "Address",
                "CustomerId");

            migrationBuilder.CreateIndex(
                "IX_Job_CustomerId",
                "Job",
                "CustomerId");

            migrationBuilder.CreateIndex(
                "IX_PhoneNumber_CustomerId",
                "PhoneNumber",
                "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Address");

            migrationBuilder.DropTable(
                "Job");

            migrationBuilder.DropTable(
                "PhoneNumber");

            migrationBuilder.DropTable(
                "JobType");

            migrationBuilder.DropTable(
                "Customer");
        }
    }
}