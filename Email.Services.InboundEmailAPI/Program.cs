using AutoMapper;
using Email.Services.InboundEmailAPI;
using Email.Services.InboundEmailAPI.Data;
using Email.Services.InboundEmailAPI.Extensions;
using Email.Services.InboundEmailAPI.Models;
using Email.Services.InboundEmailAPI.Models.DTO;
using Email.Services.InboundEmailAPI.RabbitMQ;
using Email.Services.InboundEmailAPI.Services;
using Email.Services.InboundEmailAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped<IRabbitMQProcedure, RabbitMQProcedure>();

builder.Services.AddScoped<IInboundEmailService, InboundEmailService>();
builder.Services.AddSingleton<List<EmailDto>>();
builder.Services.AddAuthorization(); // Add this line to add authorization support


var emailConfig = builder.Configuration
        .GetSection("Smtp")
        .Get<EmailConfig>();
builder.Services.AddSingleton(emailConfig);

// Add logging
builder.Logging.AddConsole();
builder.AddAppAuthetication();

builder.Services.AddAuthorization();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
