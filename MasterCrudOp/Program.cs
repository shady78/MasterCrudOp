using FluentValidation;
using MasterCrudOp.Endpoints;
using MasterCrudOp.Exceptions;
using MasterCrudOp.Middleware;
using MasterCrudOp.Persistence;
using MasterCrudOp.Services;
using MasterCrudOp.Validators;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try
{
    Log.Information("Starting Server");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, LoggerConfiguration) =>
    {        
        LoggerConfiguration.WriteTo.Console();
        LoggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


    builder.Services.AddDbContext<MovieDbContex>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseNpgsql(connectionString);
    });

    builder.Services.AddTransient<IMovieService, MovieService>();

    builder.Services.AddValidatorsFromAssemblyContaining<CreateMovieValidator>();
    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    
    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();

    //app.UseMiddleware<ExceptionHandlingMiddleware>();


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

}
catch (Exception ex)
{
    
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
       Log.CloseAndFlush();
}