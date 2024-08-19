using CartService.Data.Initializers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    await DbInitializer.InitializeDatabase(app);
}
catch (Exception e)
{

    throw new Exception("Error initializing database in cart service...", e);
}

app.Run();

