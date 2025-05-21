using Azure.Core;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mock_Project.Data;
using Mock_Project.Repositories;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<Mock_Project.Filters.ApiExceptionFilter>();
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("server=USMUMAPRAMO1\\MSSQLSERVER01;database=MockDatabase;TrustServerCertificate=true;Integrated Security=true"));           //.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

//Registers repository interfaces and their implementations with the dependency injection container. These services are created per request (Scoped lifetime).
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITeamSelfieRepository, TeamSelfieRepository>();
builder.Services.AddScoped<ITrainingActivityRepository, TrainingActivityRepository>();
builder.Services.AddScoped<ITrainingSelfieRepository, TrainingSelfieRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddEndpointsApiExplorer();

//Configures Swagger for API documentation. It defines the API title and version and maps IFormFile to a binary format for file uploads.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mock Project API", Version = "v1" });
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
});

var corsSettings = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    //Configures a CORS policy named AllowAngularApp to allow requests from specified origins(corsSettings) with any HTTP method and header.
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(corsSettings)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mock Project v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles(); //Serves static files (e.g., images, CSS, JavaScript) from the wwwroot folder.
app.UseRouting();
app.UseAuthorization();
app.UseCors("AllowAngularApp");
app.MapControllers();
app.Run();


