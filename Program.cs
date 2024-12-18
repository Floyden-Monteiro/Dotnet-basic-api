using BagAPI.Data;
using BagAPI.Middleware;
using BagAPI.ProductsEndpoints;
using BagAPI.RolesEndpoints;
using BagAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; 
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the database
builder.Services.AddDbContext<BagDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register application services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<RoleService>();

// Configure JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("user", "admin"));
});

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();


var app = builder.Build();



// Enable CORS for all origins
app.UseCors("AllowAll");

// Add URLs (useful in Docker or specific setups)
app.Urls.Add("http://0.0.0.0:5003");
app.Urls.Add("http://localhost:9001");

// Database initialization (use Migrate instead of EnsureCreated for production)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BagDBContext>();
    context.Database.EnsureCreated(); // Replace with context.Database.Migrate() for migrations
}

// Enable authentication and authorization
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

// Map your endpoints
// app.MapUserEndpoints();
// app.MapProductsEndpoints();
// app.MapRolesEndpoint();

// Log application startup
app.Logger.LogInformation("App is running now... 🙌🙌");

// Run the application
app.Run();
