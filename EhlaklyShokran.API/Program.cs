//using EhlaklyShokran.Api.Components;
using EhlaklyShokran.Infrastructure.Data;
using EhlaklyShokran.Infrastructure.RealTime;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddPresentation(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);



builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EhlaklyShokran API",
        Version = "v1",
        Description = "API documentation for EhlaklyShokran project"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapScalarApiReference();

    await app.InitialiseDatabaseAsync();

}
else
{
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseCoreMiddlewares(builder.Configuration);

app.MapControllers();

app.UseAntiforgery();

app.MapHub<WorkOrderHub>("/hubs/workorders");

app.Run();