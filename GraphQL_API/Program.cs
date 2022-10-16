using GraphQL_API.Profiles;
using GraphQL_API.Schema.Mutation;
using GraphQL_API.Schema.Query;
using LocationFinder.Domain.Interfaces;
using LocationFinder.Infrastructure.Data;
using LocationFinder.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ILocation, LocationService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")),
contextLifetime: ServiceLifetime.Singleton);

builder.Services.AddGraphQLServer()
    .AddQueryType<LocationQuery>()
    .AddMutationType<LocationMutation>();


// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGraphQL();

app.Run();
