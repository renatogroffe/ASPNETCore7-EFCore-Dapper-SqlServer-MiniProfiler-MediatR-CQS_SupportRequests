using Microsoft.EntityFrameworkCore;
using APISupport.Data;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddDbContext<SupportContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DBSupport"));
    if (isDevelopment)
        options.EnableSensitiveDataLogging();
});

builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssembly(typeof(SupportContext).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (isDevelopment)
    builder.Services.AddMiniProfiler(options =>
        options.RouteBasePath = "/profiler").AddEntityFramework();

var app = builder.Build();

if (isDevelopment)
{
    var logger = app.Logger;
    logger.LogInformation("Ativando o middleware do MiniProfiler...");

    // Rotas poss�veis com a configura��o do MiniProfiler:
    logger.LogInformation("MiniProfiler - �ltima opera��o: /profiler/results");
    logger.LogInformation("MiniProfiler - Listagem de todas as opera��es: /profiler/results-index");
    logger.LogInformation("MiniProfiler - Opera��o espec�fica: /profiler/results?id=<Guid Profiler>");

    app.UseMiniProfiler();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();