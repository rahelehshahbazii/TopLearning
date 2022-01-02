using Microsoft.EntityFrameworkCore.Migrations;

namespace TopLearn.DataLayer.Migrations
{
    public partial class mig_permission2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Permission_parentId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permission_permissionId",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "permission");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_parentId",
                table: "permission",
                newName: "IX_permission_parentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission",
                table: "permission",
                column: "permissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_permission_permission_parentId",
                table: "permission",
                column: "parentId",
                principalTable: "permission",
                principalColumn: "permissionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_permission_permissionId",
                table: "RolePermission",
                column: "permissionId",
                principalTable: "permission",
                principalColumn: "permissionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_permission_permission_parentId",
                table: "permission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_permission_permissionId",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission",
                table: "permission");

            migrationBuilder.RenameTable(
                name: "permission",
                newName: "Permission");

            migrationBuilder.RenameIndex(
                name: "IX_permission_parentId",
                table: "Permission",
                newName: "IX_Permission_parentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "permissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Permission_parentId",
                table: "Permission",
                column: "parentId",
                principalTable: "Permission",
                principalColumn: "permissionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permission_permissionId",
                table: "RolePermission",
                column: "permissionId",
                principalTable: "Permission",
                principalColumn: "permissionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
