using MassTransit;
using Microsoft.EntityFrameworkCore;
using PokemonService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<PokemonDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddMassTransit(x => {
    // Add consumer 

    // Set prefix for all queues/exchanges, full name is pokemon-service + consumer
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("pokemonservice", false));

    x.UsingRabbitMq((context, config) => {
        // If RabbitMQ server doesn't respond, use message retry, 5 times with 10 seconds interval to try to connect
        config.UseMessageRetry(retry => {
            retry.Handle<RabbitMqConnectionException>();
            retry.Interval(5, TimeSpan.FromSeconds(10));
        });
        
        config.Host(builder.Configuration["RabbitMq:Host"], "/", host => 
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "hej"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "hej"));
        });

        config.ConfigureEndpoints(context);
    });
});

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
