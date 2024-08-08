using SearchService.Data.Initializer;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<GrpcSender>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
	await DbInitializer.InitDatabase(app);
}
catch (Exception e)
{
	throw new Exception("Error initializing database", e);
}

app.Run();