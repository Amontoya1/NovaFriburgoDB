using Microsoft.EntityFrameworkCore;
using NovaFriburgoDB.DataAccess;
using NovaFriburgoDB.Services;

var builder = WebApplication.CreateBuilder(args);

// 2. Connection with SQL Server Express
const string CONNECTIONNAME = "NovaFriburgoDB";
var connectionString = builder.Configuration.GetConnectionString(CONNECTIONNAME);

// 3. Add Context to Services of builder
builder.Services.AddDbContext<NovaFriburgoDBContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllers();

// 4. Add Custom Services (folder Services)
builder.Services.AddScoped<IStudentsService, StudentsService>();
// TODO: Add the rest of services

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
}
);

//services antes del build
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

// 6. Tell app to use CORS
app.UseCors("CorsPolicy");

app.Run();
