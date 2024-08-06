using MassTransit;
using Microsoft.EntityFrameworkCore;
using PokemonService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<PokemonDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

/*
builder.Services.AddMassTransit(x => {
    // Fill this out for RabbitMQ
});
*/


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    DbInitializer.InitalizeDatabase(app);
}
catch (Exception e)
{
    
    throw new Exception("Error initializing database", e);
}

app.Run();
