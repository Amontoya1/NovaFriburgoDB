using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NovaFriburgoDB;
using NovaFriburgoDB.DataAccess;
using NovaFriburgoDB.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 2. Connection with SQL Server Express
const string CONNECTIONNAME = "NovaFriburgoDB";
var connectionString = builder.Configuration.GetConnectionString(CONNECTIONNAME);

// 3. Add Context to Services of builder
builder.Services.AddDbContext<NovaFriburgoDBContext>(options => options.UseSqlServer(connectionString));

// 7. Add Service of JWT Autorization
builder.Services.AddJwtTokenServices(builder.Configuration);

//  LOCALIZATION
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    o.JsonSerializerOptions.MaxDepth = 0;
});

// 4. Add Custom Services (folder Services)
builder.Services.AddScoped<IStudentsService, StudentsService>();
// TODO: Add the rest of services

// 8. Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnlyPolicy", policy => policy.RequireClaim("UserOnly", "User1"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddMvc().AddJsonOptions();
builder.Services.AddSwaggerGen(options =>
    {
    // We define the Security for authorzation
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization Header using Bearer Scheme"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type= ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                new string[]{}
            }
        });
}
);

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

// SUPPORTED CULTURES
var supportedCultures = new[] { "en-US", "es-ES", "fr-FR", "pt-BR" }; // USA's english, Spain's Spanish and Portugues's Brasil
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0]) // English by default
    .AddSupportedCultures(supportedCultures) // Add all supported cultures
    .AddSupportedUICultures(supportedCultures); // Add supported cultures to UI;

//  ADD LOCALIZATION to App
app.UseRequestLocalization(localizationOptions);

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
