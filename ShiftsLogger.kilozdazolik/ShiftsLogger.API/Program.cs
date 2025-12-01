using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using ShiftsLogger.API;
using ShiftsLogger.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddDbContext<ShiftsLoggerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ShiftsLogger API",
        Description = "API for log shitfs"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShiftsLogger API V1");
        c.RoutePrefix = string.Empty;
    });

}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
