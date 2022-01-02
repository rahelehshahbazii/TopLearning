﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TopLearn.DataLayer.Migrations
{
    public partial class mig_CourseGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseGroup",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupTitle = table.Column<string>(maxLength: 200, nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroup", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_CourseGroup_CourseGroup_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CourseGroup",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroup_ParentId",
                table: "CourseGroup",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseGroup");
        }
    }
}
