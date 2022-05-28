using Microsoft.EntityFrameworkCore;
using NovaFriburgoDB.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// 2. Connection with SQL Server Express
const string CONNECTIONNAME = "NovaFriburgoDB";
var connectionString = builder.Configuration.GetConnectionString(CONNECTIONNAME);

// 3. Add Context to Services of builder
//#pragma warning disable CS8604 // Poss�vel argumento de refer�ncia nula.
builder.Services.AddDbContext<NovaFriburgoDBContext>(options => options.UseSqlServer(connectionString));
#pragma warning restore CS8604 // Poss�vel argumento de refer�ncia nula.

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
