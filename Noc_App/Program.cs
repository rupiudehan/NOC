
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Noc_App.Clients;
using Noc_App.Context;
using Noc_App.Helpers;
using Noc_App.Models;
using Noc_App.Models.IFMSPayment;
using Noc_App.Models.interfaces;
using Noc_App.Models.Repository;
using Noc_App.Models.ViewModel;
using Noc_App.PaymentUtilities;
using Noc_App.UtilityService;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
        //option.LoginPath = "/Account/Login";
        option.LoginPath = "/Account/Verify";
        option.AccessDeniedPath = "/Account/Verify";
        //option.AccessDeniedPath = "/Account/Login";
    });
builder.Services.AddAuthorization();

//Register
builder.Services.AddDbContextPool<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DBCS"))
    );

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMvc(
//    options => { 
    
//    var policy=new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//    options.Filters.Add(new AuthorizeFilter(policy));
//}
);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("reCaptcha"));
builder.Services.Configure<IFMS_PaymentConfig>(builder.Configuration.GetSection("IFMSPayOptions"));

//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//builder.Services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
builder.Services.AddTransient<GoogleCaptchaService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICalculations, Calculations>();
builder.Services.AddScoped<IRepository<DistrictDetails>, Repository<DistrictDetails>>();
builder.Services.AddScoped<IRepository<DivisionDetails>, Repository<DivisionDetails>>();
builder.Services.AddScoped<IRepository<SubDivisionDetails>, Repository<SubDivisionDetails>>();
builder.Services.AddScoped<IRepository<TehsilBlockDetails>, Repository<TehsilBlockDetails>>();
builder.Services.AddScoped<IRepository<VillageDetails>, Repository<VillageDetails>>();
builder.Services.AddScoped<IRepository<OwnerTypeDetails>, Repository<OwnerTypeDetails>>();
builder.Services.AddScoped<IRepository<OwnerDetails>, Repository<OwnerDetails>>();
builder.Services.AddScoped<IRepository<NocPermissionTypeDetails>, Repository<NocPermissionTypeDetails>>();
builder.Services.AddScoped<IRepository<NocTypeDetails>, Repository<NocTypeDetails>>();
builder.Services.AddScoped<IRepository<ProjectTypeDetails>, Repository<ProjectTypeDetails>>();
builder.Services.AddScoped<IRepository<SiteAreaUnitDetails>, Repository<SiteAreaUnitDetails>>();
builder.Services.AddScoped<IRepository<GrantKhasraDetails>, Repository<GrantKhasraDetails>>();
builder.Services.AddScoped<IRepository<GrantDetails>, Repository<GrantDetails>>();
builder.Services.AddScoped<IRepository<GrantPaymentDetails>, Repository<GrantPaymentDetails>>();
builder.Services.AddScoped<IRepository<GrantApprovalDetail>, Repository<GrantApprovalDetail>>();
builder.Services.AddScoped<IRepository<GrantApprovalProcessDocumentsDetails>, Repository<GrantApprovalProcessDocumentsDetails>>();
builder.Services.AddScoped<IRepository<GrantApprovalMaster>, Repository<GrantApprovalMaster>>();
builder.Services.AddScoped<IRepository<GrantUnprocessedAppDetails>, Repository<GrantUnprocessedAppDetails>>();
builder.Services.AddScoped<IRepository<ReportApplicationCountViewModel>, Repository<ReportApplicationCountViewModel>>();
builder.Services.AddScoped<IRepository<SiteUnitMaster>, Repository<SiteUnitMaster>>();
builder.Services.AddScoped<IRepository<ChallanDetails>, Repository<ChallanDetails>>();
builder.Services.AddScoped<IRepository<DashboardPendencyAll>, Repository<DashboardPendencyAll>>();
builder.Services.AddScoped<IRepository<DashboardPendencyViewModel>, Repository<DashboardPendencyViewModel>>();
builder.Services.AddScoped<IRepository<DaysCheckMaster>, Repository<DaysCheckMaster>>();
builder.Services.AddScoped<IRepository<UserRoleDetails>, Repository<UserRoleDetails>>();
builder.Services.AddScoped<IRepository<RecommendationDetail>, Repository<RecommendationDetail>>();
builder.Services.AddScoped<IRepository<GrantSectionsDetails>, Repository<GrantSectionsDetails>>();
builder.Services.AddScoped<IRepository<GrantRejectionShortfallSection>, Repository<GrantRejectionShortfallSection>>();
builder.Services.AddScoped<IRepository<PlanSanctionAuthorityMaster>, Repository<PlanSanctionAuthorityMaster>>();
builder.Services.AddScoped<IRepository<DrainWidthTypeDetails>, Repository<DrainWidthTypeDetails>>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseStatusCodePagesWithRedirects("/Error/{0}");
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Grant}/{action=Create}/{id?}");

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseRotativa();
//app.UseMiddleware<LoggingMiddleware>();
app.Run();
