using Checkin.AccountService.Repository;
using Checkin.AccountService.Repository.Repositories;
using Checkin.AccountService.Service.Services;
using Checkin.Common.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// todo: config
builder.Services.AddDbContext<DbContext, AccountDbContext>(x =>
    x.UseNpgsql("Host=localhost;Database=account;Username=postgres;Password=postgres"));

//todo: incapsulate
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//todo: move to migrator or script
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DbContext>();
    context.Database.EnsureCreated();
    context.Database.MigrateAsync();
}

app.Run();