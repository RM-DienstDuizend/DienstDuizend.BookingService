using DienstDuizend.BookingService.Infrastructure;
using DienstDuizend.BookingService.Infrastructure.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    // We do not have to validate the token at all, this has been done at the gateway. We do however need to map those
    // claims to the user property inside the httpContext, which is why we have to do this. 
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateActor = false,
        ValidateIssuerSigningKey = false,
        ValidateLifetime = false,
        ValidateTokenReplay = false,
        SignatureValidator = (token, _) => new JsonWebToken(token)
    };
});

builder.Services.AddAuthorization(options =>
{
    // This will make it so authentication is always required, unless specified otherwise.
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<VogenSchemaFilter>();

    c.CustomSchemaIds(t => t.FullName?.Replace("+", ".", StringComparison.Ordinal));
});

builder.Services.AddControllers();

// Exception handling
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();

// This will configure the database
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseExceptionHandler();

app.MapPrometheusScrapingEndpoint().AllowAnonymous();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
