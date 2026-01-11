using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VsaSample.Infrastructure.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationsAndTenantScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var defaultOrgId = new Guid("00000000-0000-0000-0000-000000000001");

            migrationBuilder.CreateTable(
                name: "Organizations",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Name",
                schema: "public",
                table: "Organizations",
                column: "Name",
                unique: true);

            migrationBuilder.InsertData(
                schema: "public",
                table: "Organizations",
                columns: new[] { "Id", "Name", "IsActive" },
                values: new object[] { defaultOrgId, "Default", true });

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "Templates",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "DietPlans",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "Clients",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "Appointments",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql(
                $"UPDATE public.\"Users\" SET \"OrganizationId\" = '{defaultOrgId}' WHERE \"OrganizationId\" IS NULL;");
            migrationBuilder.Sql(
                $"UPDATE public.\"Templates\" SET \"OrganizationId\" = '{defaultOrgId}' WHERE \"OrganizationId\" IS NULL;");
            migrationBuilder.Sql(
                $"UPDATE public.\"DietPlans\" SET \"OrganizationId\" = '{defaultOrgId}' WHERE \"OrganizationId\" IS NULL;");
            migrationBuilder.Sql(
                $"UPDATE public.\"Clients\" SET \"OrganizationId\" = '{defaultOrgId}' WHERE \"OrganizationId\" IS NULL;");
            migrationBuilder.Sql(
                $"UPDATE public.\"Appointments\" SET \"OrganizationId\" = '{defaultOrgId}' WHERE \"OrganizationId\" IS NULL;");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "Users",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "Templates",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "DietPlans",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "Clients",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                schema: "public",
                table: "Appointments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId_Id",
                schema: "public",
                table: "Users",
                columns: new[] { "OrganizationId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_OrganizationId_Id",
                schema: "public",
                table: "Templates",
                columns: new[] { "OrganizationId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_DietPlans_OrganizationId_ClientId",
                schema: "public",
                table: "DietPlans",
                columns: new[] { "OrganizationId", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_DietPlans_OrganizationId_Id",
                schema: "public",
                table: "DietPlans",
                columns: new[] { "OrganizationId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_OrganizationId_Id",
                schema: "public",
                table: "Clients",
                columns: new[] { "OrganizationId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OrganizationId_ClientId",
                schema: "public",
                table: "Appointments",
                columns: new[] { "OrganizationId", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OrganizationId_Id",
                schema: "public",
                table: "Appointments",
                columns: new[] { "OrganizationId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Organizations_OrganizationId",
                schema: "public",
                table: "Appointments",
                column: "OrganizationId",
                principalSchema: "public",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Organizations_OrganizationId",
                schema: "public",
                table: "Clients",
                column: "OrganizationId",
                principalSchema: "public",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DietPlans_Organizations_OrganizationId",
                schema: "public",
                table: "DietPlans",
                column: "OrganizationId",
                principalSchema: "public",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Organizations_OrganizationId",
                schema: "public",
                table: "Templates",
                column: "OrganizationId",
                principalSchema: "public",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                schema: "public",
                table: "Users",
                column: "OrganizationId",
                principalSchema: "public",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Organizations_OrganizationId",
                schema: "public",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Organizations_OrganizationId",
                schema: "public",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_DietPlans_Organizations_OrganizationId",
                schema: "public",
                table: "DietPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Organizations_OrganizationId",
                schema: "public",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                schema: "public",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganizationId_Id",
                schema: "public",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Templates_OrganizationId_Id",
                schema: "public",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_DietPlans_OrganizationId_ClientId",
                schema: "public",
                table: "DietPlans");

            migrationBuilder.DropIndex(
                name: "IX_DietPlans_OrganizationId_Id",
                schema: "public",
                table: "DietPlans");

            migrationBuilder.DropIndex(
                name: "IX_Clients_OrganizationId_Id",
                schema: "public",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_OrganizationId_ClientId",
                schema: "public",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_OrganizationId_Id",
                schema: "public",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "public",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "public",
                table: "DietPlans");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "public",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "public",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Organizations",
                schema: "public");
        }
    }
}
