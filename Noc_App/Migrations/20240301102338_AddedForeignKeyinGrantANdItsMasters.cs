using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeyinGrantANdItsMasters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrainCoordinatesDetails_AspNetUsers_UpdatedBy",
                table: "DrainCoordinatesDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_NocPermissionTypeDetails_NocPermissionTypeId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_NocTypeDetails_NocTypeId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_ProjectTypeDetails_ProjectTypeId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_VillageDetails_VillageId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerDetails_OwnerTypeDetails_OwnerTypeId",
                table: "OwnerDetails");

            migrationBuilder.DropIndex(
                name: "IX_DrainCoordinatesDetails_UpdatedBy",
                table: "DrainCoordinatesDetails");

            migrationBuilder.RenameColumn(
                name: "VillageName",
                table: "VillageDetails",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "VillageId",
                table: "GrantDetails",
                newName: "VillageID");

            migrationBuilder.RenameColumn(
                name: "NocPermissionTypeId",
                table: "GrantDetails",
                newName: "NocPermissionTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_GrantDetails_VillageId",
                table: "GrantDetails",
                newName: "IX_GrantDetails_VillageID");

            migrationBuilder.RenameIndex(
                name: "IX_GrantDetails_NocPermissionTypeId",
                table: "GrantDetails",
                newName: "IX_GrantDetails_NocPermissionTypeID");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerTypeId",
                table: "OwnerDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VillageID",
                table: "GrantDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectTypeId",
                table: "GrantDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlotNo",
                table: "GrantDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
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

            migrationBuilder.AlterColumn<string>(
                name: "NocNumber",
                table: "GrantDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GrantDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Khasra",
                table: "GrantDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IDProofPhotoPath",
                table: "GrantDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Hadbast",
                table: "GrantDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizationLetterPhotoPath",
                table: "GrantDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressProofPhotoPath",
                table: "GrantDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DrainDetails",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "DrainCoordinatesDetails",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DrainCoordinatesDetails_ApplicationUserId",
                table: "DrainCoordinatesDetails",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrainCoordinatesDetails_AspNetUsers_ApplicationUserId",
                table: "DrainCoordinatesDetails",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

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
                name: "FK_GrantDetails_ProjectTypeDetails_ProjectTypeId",
                table: "GrantDetails",
                column: "ProjectTypeId",
                principalTable: "ProjectTypeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_VillageDetails_VillageID",
                table: "GrantDetails",
                column: "VillageID",
                principalTable: "VillageDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerDetails_OwnerTypeDetails_OwnerTypeId",
                table: "OwnerDetails",
                column: "OwnerTypeId",
                principalTable: "OwnerTypeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrainCoordinatesDetails_AspNetUsers_ApplicationUserId",
                table: "DrainCoordinatesDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_NocPermissionTypeDetails_NocPermissionTypeID",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_NocTypeDetails_NocTypeId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_ProjectTypeDetails_ProjectTypeId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_VillageDetails_VillageID",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerDetails_OwnerTypeDetails_OwnerTypeId",
                table: "OwnerDetails");

            migrationBuilder.DropIndex(
                name: "IX_DrainCoordinatesDetails_ApplicationUserId",
                table: "DrainCoordinatesDetails");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "DrainCoordinatesDetails");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VillageDetails",
                newName: "VillageName");

            migrationBuilder.RenameColumn(
                name: "VillageID",
                table: "GrantDetails",
                newName: "VillageId");

            migrationBuilder.RenameColumn(
                name: "NocPermissionTypeID",
                table: "GrantDetails",
                newName: "NocPermissionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_GrantDetails_VillageID",
                table: "GrantDetails",
                newName: "IX_GrantDetails_VillageId");

            migrationBuilder.RenameIndex(
                name: "IX_GrantDetails_NocPermissionTypeID",
                table: "GrantDetails",
                newName: "IX_GrantDetails_NocPermissionTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerTypeId",
                table: "OwnerDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "VillageId",
                table: "GrantDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectTypeId",
                table: "GrantDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "PlotNo",
                table: "GrantDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "NocTypeId",
                table: "GrantDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NocPermissionTypeId",
                table: "GrantDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "NocNumber",
                table: "GrantDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GrantDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Khasra",
                table: "GrantDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IDProofPhotoPath",
                table: "GrantDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Hadbast",
                table: "GrantDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizationLetterPhotoPath",
                table: "GrantDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AddressProofPhotoPath",
                table: "GrantDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DrainDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.CreateIndex(
                name: "IX_DrainCoordinatesDetails_UpdatedBy",
                table: "DrainCoordinatesDetails",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_DrainCoordinatesDetails_AspNetUsers_UpdatedBy",
                table: "DrainCoordinatesDetails",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_NocPermissionTypeDetails_NocPermissionTypeId",
                table: "GrantDetails",
                column: "NocPermissionTypeId",
                principalTable: "NocPermissionTypeDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_NocTypeDetails_NocTypeId",
                table: "GrantDetails",
                column: "NocTypeId",
                principalTable: "NocTypeDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_ProjectTypeDetails_ProjectTypeId",
                table: "GrantDetails",
                column: "ProjectTypeId",
                principalTable: "ProjectTypeDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_VillageDetails_VillageId",
                table: "GrantDetails",
                column: "VillageId",
                principalTable: "VillageDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerDetails_OwnerTypeDetails_OwnerTypeId",
                table: "OwnerDetails",
                column: "OwnerTypeId",
                principalTable: "OwnerTypeDetails",
                principalColumn: "Id");
        }
    }
}
