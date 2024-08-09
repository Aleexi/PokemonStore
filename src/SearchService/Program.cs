using MassTransit;
using SearchService.Consumers;
using SearchService.Data.Initializer;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<GrpcSender>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x =>
{
	x.AddConsumersFromNamespaceContaining<PokemonCreatedConsumer>();

	x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("searchservice", false));

	x.UsingRabbitMq((context, config) =>
	{
		config.UseMessageRetry(retry =>
		{
			retry.Handle<RabbitMqConnectionException>();
			retry.Interval(5, TimeSpan.FromSeconds(5));
		});

		config.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
		{
			host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
			host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
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
	await DbInitializer.InitDatabase(app);
}
catch (Exception e)
{
	throw new Exception("Error initializing database", e);
}

app.Run();