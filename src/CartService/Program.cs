using CartService.Data.Initializers;
using CartService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<GrpcSender>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];

        options.RequireHttpsMetadata = false; // Needed since identity server is running on Http

        options.TokenValidationParameters.ValidateAudience = false;

        options.TokenValidationParameters.NameClaimType = "username";
    });



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

