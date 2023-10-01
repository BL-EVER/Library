using Common.ActionFilters;
using Common.Authorizations;
using Common.HttpClients;
using Common.Repository;
using Common.Service;
using Common.Utilities;
using LibraryOrderService.DataContext;
using LibraryOrderService.Interfaces.Repository;
using LibraryOrderService.Interfaces.Service;
using LibraryOrderService.Repositories;
using LibraryOrderService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryOrderService API", Version = "v1" });

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
builder.Services.AddControllers(options =>
{
    options.Filters.Add<PopulateBookResultFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAutoMapper(typeof(LibraryOrderService.AutoMapper.AutoMapperProfile));

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

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICacheRepository, CacheRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();

builder.Services.AddScoped<IPopulateBookService, PopulateBookService>();

builder.Services.AddScoped<ICachedHttpClientService, CachedHttpClientService>();

builder.Services.AddHttpClient();
builder.Services.AddCustomHttpClientPolicies();

var app = builder.Build();

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

app.Run();
