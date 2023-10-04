using Common.Authorizations;
using Common.ExceptionManagement;
using Common.Health;
using Common.HttpClients;
using Common.Repository;
using Common.Service;
using Common.Utilities;
using LibraryCatalogService.DataContext;
using LibraryCatalogService.Interfaces.Repository;
using LibraryCatalogService.Interfaces.Service;
using LibraryCatalogService.Repositories;
using LibraryCatalogService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Polly;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

#region Authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration.GetConnectionString("KEYCLOAK");
        options.MetadataAddress = builder.Configuration.GetConnectionString("KEYCLOAK") + "/.well-known/openid-configuration";
        options.Audience = "account";
        options.RequireHttpsMetadata = false;

    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnedResourcePolicy", policy =>
        policy.Requirements.Add(new OwnedResourceRequirement()));
});
builder.Services.AddSingleton<IAuthorizationHandler, OwnedResourceHandler>();

builder.Services.AddHttpContextAccessor();
#endregion

#region Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryCatalogService API", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
    c.CustomSchemaIds(type => type.ToString());
});

#endregion

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAutoMapper(typeof(LibraryCatalogService.AutoMapper.AutoMapperProfile));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DB")));

/*builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("CACHE")));*/
builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("CACHE"));
    configuration.AbortOnConnectFail = false;

    return ConnectionMultiplexer.Connect(configuration);
});


builder.Services.AddScoped<DbContext, AppDbContext>();

builder.Services.AddScoped(typeof(IGenericRepository<,,,,,,>), typeof(GenericRepository<,,,,,,>));
builder.Services.AddScoped(typeof(IGenericService<,,,,,,>), typeof(GenericService<,,,,,,>));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICacheRepository, CacheRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ICachedHttpClientService, CachedHttpClientService>();

builder.Services.AddHttpClient();
builder.Services.AddCustomHttpClientPolicies();

builder.Services.AddHealthChecks()
        .AddCheck("Redis", new RedisHealthCheck(builder.Configuration.GetConnectionString("CACHE")))
        .AddCheck<ApplicationHealthCheck>("Application")
        .AddDbContextCheck<AppDbContext>();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = CustomOutput.WriteResponse
});

app.Run();