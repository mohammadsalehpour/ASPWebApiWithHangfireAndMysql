using Hangfire;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WebApiWithHangfire.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL SERVER START -----------------------------------------------------

// //Create sql server connection string
// var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// //Add database service
// builder.Services.AddDbContext<AppDbContext>(options =>
// { options.UseSqlServer(ConnectionString ?? throw new InvalidOperationException("Connection string not found")); });

// // Adding Hangfire service
// builder.Services.AddHangfire(sp => { sp.UseSqlServerStorage(ConnectionString); });
// builder.Services.AddHangfireServer();

// SQL SERVER END --------------------------------


// MYSQL START ----------------------------------------------------------

//Create mysql connection string
var ConnectionString = builder.Configuration.GetConnectionString("MysqlConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(ConnectionString, new MySqlServerVersion(new System.Version(5, 7, 34)))
);

MySqlStorageOptions storageOptions = new MySqlStorageOptions
{
    TransactionIsolationLevel = IsolationLevel.ReadCommitted,
    QueuePollInterval = TimeSpan.FromSeconds(15),
    JobExpirationCheckInterval = TimeSpan.FromHours(1),
    CountersAggregateInterval = TimeSpan.FromMinutes(5),
    PrepareSchemaIfNecessary = true,
    DashboardJobListLimit = 50000,
    TransactionTimeout = TimeSpan.FromMinutes(1),
    TablesPrefix = "Hangfire"
};

var storage = new MySqlStorage(ConnectionString, storageOptions);

GlobalConfiguration.Configuration.UseStorage(storage);

// Adding Hangfire service
builder.Services.AddHangfire(sp => { sp.UseStorage(storage); });
builder.Services.AddHangfireServer();

// MYSQL END -------------------------------------

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

// Adding Hangfire Dashboard
app.UseHangfireDashboard();

app.Run();
