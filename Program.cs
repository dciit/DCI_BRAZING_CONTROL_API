using BrazingControlAPI.Contexts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddDbContext<DBDCI>();
builder.Services.AddDbContext<DBHRM>();
builder.Services.AddDbContext<DBSCM>();
builder.Services.AddCors(options => options.AddPolicy("Cors", builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));
var app = builder.Build();
app.UseCors("Cors");
app.UseAuthorization();
app.MapControllers();
app.Run();
