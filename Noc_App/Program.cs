using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Noc_App.Context;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using Noc_App.Models.Repository;
using Noc_App.UtilityService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Register
builder.Services.AddDbContextPool<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DBCS"))
    );

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMvc(options => { 
    var policy=new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//builder.Services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRepository<DivisionDetails>, Repository<DivisionDetails>>();
builder.Services.AddScoped<IRepository<SubDivisionDetails>, Repository<SubDivisionDetails>>();
builder.Services.AddScoped<IRepository<TehsilBlockDetails>, Repository<TehsilBlockDetails>>();
builder.Services.AddScoped<IRepository<VillageDetails>, Repository<VillageDetails>>();
builder.Services.AddScoped<IRepository<DrainCoordinatesDetails>, Repository<DrainCoordinatesDetails>>();
builder.Services.AddScoped<IRepository<DrainDetails>, Repository<DrainDetails>>();
builder.Services.AddScoped<IRepository<OwnerTypeDetails>, Repository<OwnerTypeDetails>>();
builder.Services.AddScoped<IRepository<OwnerDetails>, Repository<OwnerDetails>>();
builder.Services.AddScoped<IRepository<NocPermissionTypeDetails>, Repository<NocPermissionTypeDetails>>();
builder.Services.AddScoped<IRepository<NocTypeDetails>, Repository<NocTypeDetails>>();
builder.Services.AddScoped<IRepository<ProjectTypeDetails>, Repository<ProjectTypeDetails>>();
builder.Services.AddScoped<IRepository<GrantDetails>, Repository<GrantDetails>>();
builder.Services.AddHttpContextAccessor();
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomeLanding}/{action=Index}/{id?}");

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.Run();
