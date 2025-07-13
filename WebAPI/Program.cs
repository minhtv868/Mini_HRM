using Web.Application.Extensions;
using Web.Application.Settings;
using Web.Infrastructure.Extensions;
using Web.Persistence.Extensions;
using WebAPI.Filters;
using WebAPI.Utils;
using WebJob.Areas.Identity.Extensions;
using WebJob.Helpers.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddIdentityService();
builder.Services.AddHangfireService(builder.Configuration);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.SectionName));
builder.Services.AddHttpClient<HttpClientUtil>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//if (builder.Environment.IsDevelopment())
//{
//    builder.Services.AddSwaggerGen(c =>
//    {
//        c.SwaggerDoc("v2", new OpenApiInfo { Title = "Luat Web API", Version = "v2" });

//    });
//}
builder.Services.AddCors();
builder.Services.AddHealthChecks();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //  builder.Services.AddSwaggerGenNewtonsoftSupport();
}
else
{
    app.UseMiddleware<ApiKeyMiddleware>();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.MapControllers();
app.Run();
