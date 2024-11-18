
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Noc_App.Context;
using Noc_App.Helpers;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using Noc_App.Models.Repository;
using Noc_App.Models.ViewModel;
using Noc_App.UtilityService;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => 
{ 
    options.ConfigureHttpsDefaults(httpsOptions => 
    { 
        // Set the TLS protocols to only allow TLS 1.2 and 1.3 
        httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13; 
    });  
}); 
// Disable the X-Powered-By header
builder.Services.Configure<IISOptions>(options =>
{
    options.ForwardClientCertificate = false;  // Optional, if using client certificates
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.ExpireTimeSpan = TimeSpan.FromMinutes(1);
        option.LoginPath = "/Account/Login";
        //option.LoginPath = "/Account/Verify";
        //option.AccessDeniedPath = "/Account/Verify";
        option.AccessDeniedPath = "/Account/Login";
        option.SlidingExpiration = true;
        //option.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Ensures cookies are sent only over HTTPS
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
builder.Services.Configure<PasswordEncryption>(builder.Configuration.GetSection("aesEncrypt"));


//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//builder.Services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
builder.Services.AddTransient<GoogleCaptchaService>();
builder.Services.AddTransient<PasswordEncryptionService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICalculations, Calculations>();
builder.Services.AddScoped<IRepository<DistrictDetails>, Repository<DistrictDetails>>();
builder.Services.AddScoped<IRepository<DivisionDetails>, Repository<DivisionDetails>>();
builder.Services.AddScoped<IRepository<SubDivisionDetails>, Repository<SubDivisionDetails>>();
builder.Services.AddScoped<IRepository<TehsilBlockDetails>, Repository<TehsilBlockDetails>>();
//builder.Services.AddScoped<IRepository<VillageDetails>, Repository<VillageDetails>>();
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
builder.Services.AddScoped<IRepository<GrantFileTransferDetails>,Repository<GrantFileTransferDetails>>();
builder.Services.AddScoped<IRepository<MasterPlanDetails>, Repository<MasterPlanDetails>>();
builder.Services.AddScoped<IRepository<CircleDetails>, Repository<CircleDetails>>();
builder.Services.AddScoped<IRepository<CircleDivisionMapping>, Repository<CircleDivisionMapping>>();
builder.Services.AddScoped<IRepository<EstablishmentOfficeDetails>, Repository<EstablishmentOfficeDetails>>();
builder.Services.AddScoped<IRepository<ProcessedApplicationsViewModel>, Repository<ProcessedApplicationsViewModel>>();
builder.Services.AddScoped<IRepository<ReportApplicationsViewModel>, Repository<ReportApplicationsViewModel>>();
builder.Services.AddScoped<IRepository<TransferedApplicationsViewModel>, Repository<TransferedApplicationsViewModel>>();
builder.Services.AddScoped<IRepository<UserSessionDetails>, Repository<UserSessionDetails>>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
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
    // app.UseHsts(); // Enable HSTS in production
}

// Add the X-Content-Type-Options header to prevent MIME type sniffing
app.Use((context, next) =>
{
    // Strict Transport Security (HSTS)
    context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
    // Content Security Policy (CSP)
    //context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' https://example.com; style-src 'self' 'unsafe-inline'; img-src 'self'; font-src 'self'; frame-ancestors 'none'; object-src 'none'; media-src 'self'; connect-src 'self'";
    // Prevent Clickjacking
    context.Response.Headers["X-Frame-Options"] = "DENY"; // or "SAMEORIGIN"
    // Enable XSS protection in browsers
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    // Prevent MIME type sniffing
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    return next();
});

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
//app.UseMiddleware<SessionValidationMiddleware>(); // Add custom session validation
//app.UseMiddleware<IpBindingMiddleware>(); // Register IP Binding Middleware
//app.UseMiddleware<SessionExpiryMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseCookiePolicy();

//// Enforce the Content Security Policy header
////app.Use(async (context, next) =>
////{
////    context.Response.Headers.Add("Content-Security-Policy",
////        "default-src 'self'; " +
////        "script-src 'self' 'sha256-xyz123' https://fonts.googleapis.com https://www.google.com https://cdnjs.cloudflare.com https://www.gstatic.com; " +
////        "style-src 'self' 'sha256-abc456' 'sha256-tBDRz+h5TwaRZiRUjI+KLkSeHJw/FQBW2qY/I2NjUzQ=' https://fonts.googleapis.com http://49.50.66.74;  " +
////        "img-src 'self' data: https://trusted-images.example.com; " +
////        "font-src 'self' https://fonts.gstatic.com; " +
////        "connect-src 'self' https://api.example.com http://localhost:44322 http://localhost:44132; " +
////        "object-src 'none'; " +
////        "base-uri 'self'; " +
////        "form-action 'self'; " +
////        "frame-ancestors 'none'; " +
////        "child-src 'none'");

////    await next();
////});
///
// Middleware to remove the X-Powered-By header
app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("X-Powered-By");  // Remove X-Powered-By header
    await next();
});
// Middleware to add Cache-Control headers
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";  // For HTTP/1.0 compatibility
    context.Response.Headers["Expires"] = "0";  // Ensure content is not cached
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Grant}/{action=Create}/{id?}");

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseRotativa();
//app.UseMiddleware<LoggingMiddleware>();
app.Run();
