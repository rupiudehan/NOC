using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class pendingApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Create PROCEDURE UnprocessedApplicationsToForwardGet(grant_Id in numeric,village_id in numeric,tehsil_id in numeric,subDivision_id in numeric,division_id in numeric)
                                LANGUAGE 'plpgsql'
                                AS $BODY$

                                BEGIN
	                                SELECT GD.*,V.""Name"" AS VillageName ,t.""Name"" AS TehsilName,
									s.""Name"" AS SubDivisionName,d.""Name"" AS DivisionName 
									FROM public.""GrantDetails"" AS GD
									JOIN public.""ProjectTypeDetails"" ON ""ProjectTypeDetails"".""Id"" = GD.""ProjectTypeId""
									JOIN public.""NocPermissionTypeDetails"" NP  ON NP.""Id"" = GD.""NocPermissionTypeID""
									JOIN public.""NocTypeDetails"" NT ON NT.""Id"" = GD.""NocTypeId""
									JOIN public.""VillageDetails"" AS V ON V.""Id"" = GD.""VillageID""
									JOIN public.""TehsilBlockDetails"" t ON t.""Id"" = V.""TehsilBlockId""
									JOIN public.""SubDivisionDetails"" s ON s.""Id"" = t.""SubDivisionId""
									JOIN public.""DivisionDetails"" d ON d.""Id"" = s.""DivisionId""
									WHERE ""IsPending""=true AND ""IsForwarded""=false
									AND GD.""Id""=(CASE WHEN grant_Id=0 THEN GD.""Id"" ELSE grant_Id END)
									AND V.""Id""=(CASE WHEN village_id=0 THEN V.""Id"" ELSE village_id END)
									AND t.""Id""=(CASE WHEN tehsil_id=0 THEN t.""Id"" ELSE tehsil_id END)
									AND s.""Id""=(CASE WHEN subDivision_id=0 THEN s.""Id"" ELSE subDivision_id END)
									AND d.""Id""=(CASE WHEN division_id=0 THEN d.""Id"" ELSE division_id END)
									;
                                END;
                                
                                $BODY$;";
            migrationBuilder.Sql(procedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Drop PROCEDURE UnprocessedApplicationsToForwardGet;";
            migrationBuilder.Sql(procedure);
        }
    }
}
