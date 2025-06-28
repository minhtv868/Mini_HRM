using Hangfire;
using Web.Application.Extensions;
using Web.Application.Settings;
using Web.Infrastructure.Extensions;
using Web.Persistence.Extensions;
using WebJob.Areas.Identity.Extensions;
using WebJob.Helpers.Extensions;
using WebJob.Helpers.Hangfire;
using AspNetCore.DataProtection.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Web.Application.Common.Helpers;
using Microsoft.AspNetCore.Http.Features;
using Web.Application.Common.Constants;
using WebJob.Filters;

//create the logger and setup your sinks, filters and properties
Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
            _ = loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

// Add services to the container.
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddIdentityService();
AppConstant.setIsDev(builder.Environment.IsDevelopment());
builder.Services.AddBindAppConfig(builder.Configuration);
builder.Services.AddHangfireService(builder.Configuration);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.SectionName));
builder.Services.AddScoped<ApiKeyFilter>();

//builder.Services.AddDataProtection()
//    .PersistKeysToSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"), builder.Configuration["DataProtection:Schema"], builder.Configuration["DataProtection:TableName"])
//    .SetApplicationName(builder.Configuration["DataProtection:ApplicationName"])
//    .SetDefaultKeyLifetime(TimeSpan.FromDays(int.Parse(builder.Configuration["DataProtection:DefaultKeyLifetimeDays"])))
//    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
//    {
//        EncryptionAlgorithm = Enum.Parse<EncryptionAlgorithm>(builder.Configuration["DataProtection:EncryptionAlgorithm"]),
//        ValidationAlgorithm = Enum.Parse<ValidationAlgorithm>(builder.Configuration["DataProtection:ValidationAlgorithm"])
//    })
//    .AddKeyManagementOptions(options =>
//    {
//        options.NewKeyLifetime = TimeSpan.FromDays(int.Parse(builder.Configuration["DataProtection:NewKeyLifetimeDays"]));
//        options.AutoGenerateKeys = bool.Parse(builder.Configuration["DataProtection:AutoGenerateKeys"]);
//    });
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie.Name = "cmsBongDa24hcloud";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    //options.EventsType = typeof(CustomCookieAuthenticationEvents);

});

// Set the JSON serializer options
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
});

// thêm upload file lớn
builder.Services.AddRazorPages(options =>
{
    options.Conventions
        .AddPageApplicationModelConvention("/[your cshtml page name with no extension]",
            model =>
            {
                model.Filters.Add(
                new RequestFormLimitsAttribute()
                {
                    KeyLengthLimit = int.MaxValue,
                    ValueLengthLimit = int.MaxValue,
                    MultipartBodyLengthLimit = int.MaxValue,
                    MultipartHeadersLengthLimit = int.MaxValue
                });

            });

});

builder.Services.Configure<FormOptions>(options =>
{
    options.KeyLengthLimit = int.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});
builder.Services.AddRazorPages();
//builder.Services.AddSession();

builder.Services.AddSession(options =>
{
    // Cookie settings
    options.Cookie.Name = ".cmsBongDa24hcloud.Session";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

//Add health check
builder.Services.AddHealthChecks();

builder.Services.AddHttpClient();

// Đăng ký Hosted Service để load dữ liệu vào class static khi khởi động
//builder.Services.AddHostedService<LoadStaticDataHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseMiddleware<ApiKeyMiddleware>();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.MapHealthChecks("/health");

app.Run();
