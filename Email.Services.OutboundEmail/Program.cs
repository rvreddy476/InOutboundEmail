using AutoMapper;
using Email.Services.OutboundEmail.Data;
using Email.Services.OutboundEmail.Extensions;
using Email.Services.OutboundEmail.Models;
using Email.Services.OutboundEmail.RabbitMQ;
using Email.Services.OutboundEmail.Services;
using Email.Services.OutboundEmail.Services.IService;
using Microsoft.EntityFrameworkCore;
using OutboundEmailServices;
using Serilog;

    var builder = WebApplication.CreateBuilder(args);
   

    // Configure Serilog
    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

var emailConfig = builder.Configuration
        .GetSection("Smtp")
        .Get<EmailConfig>();
builder.Services.AddSingleton(emailConfig);

builder.AddAppAuthetication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IOutboundEmailService, OutboundEmailService>();
builder.Services.AddScoped<IRabbitMQProcedure, RabbitMQProcedure>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {

        Console.WriteLine(ex?.Message);
    }

}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
