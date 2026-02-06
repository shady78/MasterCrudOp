using MasterCrudOp.Endpoints;
using MasterCrudOp.Persistence;
using MasterCrudOp.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MovieDbContex>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddTransient<IMovieService, MovieService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapMovieEndpoints();


await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbcontext = serviceScope.ServiceProvider.GetRequiredService<MovieDbContex>())
{
    await dbcontext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.Run();