using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PokemonService;
using PokemonService.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PokemonDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x => {
    // Add consumer, right now for testing purposes
    // x.AddConsumersFromNamespaceContaining<PokemonCreatedConsumer>();

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
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        config.ConfigureEndpoints(context);
    });
});

// Adding service for pokemon service to be able to authenticate user against the identity service through the access_token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];

        options.RequireHttpsMetadata = false; // Needed since identity server is running on Http

        options.TokenValidationParameters.ValidateAudience = false;

        // Tells JWT bearer middleware to look for and use a claim type of "username" to authorize user 
        options.TokenValidationParameters.NameClaimType = "username";
    });

var app = builder.Build();

// Middleware

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGrpcService<GrpcListener>();

// Custom policy to wait for Postgres to be ready to accept connections
var retryPolicy = Policy.Handle<NpgsqlException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(10));

retryPolicy.ExecuteAndCapture(() => DbInitializer.InitalizeDatabase(app));

app.Run();