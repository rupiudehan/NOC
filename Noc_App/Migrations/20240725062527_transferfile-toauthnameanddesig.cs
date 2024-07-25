using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class transferfiletoauthnameanddesig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_MasterPlanDetails_MasterPlanId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_NocPermissionTypeDetails_NocPermissionTypeID",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_NocTypeDetails_NocTypeId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_SiteAreaUnitDetails_SiteAreaUnitId",
                table: "GrantDetails");

           

            migrationBuilder.AddColumn<string>(
                name: "FromDesignationName",
                table: "GrantFileTransferDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromName",
                table: "GrantFileTransferDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToDesignationName",
                table: "GrantFileTransferDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToName",
                table: "GrantFileTransferDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SiteAreaUnitId",
                table: "GrantDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NocTypeId",
                table: "GrantDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NocPermissionTypeID",
                table: "GrantDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "MasterPlanId",
                table: "GrantDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsExtension",
                table: "GrantDetails",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_MasterPlanDetails_MasterPlanId",
                table: "GrantDetails",
                column: "MasterPlanId",
                principalTable: "MasterPlanDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_NocPermissionTypeDetails_NocPermissionTypeID",
                table: "GrantDetails",
                column: "NocPermissionTypeID",
                principalTable: "NocPermissionTypeDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_NocTypeDetails_NocTypeId",
                table: "GrantDetails",
                column: "NocTypeId",
                principalTable: "NocTypeDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_SiteAreaUnitDetails_SiteAreaUnitId",
                table: "GrantDetails",
                column: "SiteAreaUnitId",
                principalTable: "SiteAreaUnitDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_MasterPlanDetails_MasterPlanId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_NocPermissionTypeDetails_NocPermissionTypeID",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_NocTypeDetails_NocTypeId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_SiteAreaUnitDetails_SiteAreaUnitId",
                table: "GrantDetails");


            migrationBuilder.DropColumn(
                name: "FromDesignationName",
                table: "GrantFileTransferDetails");

            migrationBuilder.DropColumn(
                name: "FromName",
                table: "GrantFileTransferDetails");

            migrationBuilder.DropColumn(
                name: "ToDesignationName",
                table: "GrantFileTransferDetails");

            migrationBuilder.DropColumn(
                name: "ToName",
                table: "GrantFileTransferDetails");

            migrationBuilder.AlterColumn<int>(
                name: "SiteAreaUnitId",
                table: "GrantDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NocTypeId",
                table: "GrantDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NocPermissionTypeID",
                table: "GrantDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MasterPlanId",
                table: "GrantDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsExtension",
                table: "GrantDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_MasterPlanDetails_MasterPlanId",
                table: "GrantDetails",
                column: "MasterPlanId",
                principalTable: "MasterPlanDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_NocPermissionTypeDetails_NocPermissionTypeID",
                table: "GrantDetails",
                column: "NocPermissionTypeID",
                principalTable: "NocPermissionTypeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_NocTypeDetails_NocTypeId",
                table: "GrantDetails",
                column: "NocTypeId",
                principalTable: "NocTypeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_SiteAreaUnitDetails_SiteAreaUnitId",
                table: "GrantDetails",
                column: "SiteAreaUnitId",
                principalTable: "SiteAreaUnitDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
