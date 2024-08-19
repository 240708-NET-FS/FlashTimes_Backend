using Microsoft.EntityFrameworkCore;
using FlashTimes.Services;
using FlashTimes.Repositories;
using FlashTimes.Utilities;
using FlashTimes.Entities;


var builder = WebApplication.CreateBuilder(args);

// Bind JwtSettings from configuration
//GetSection() argument must match appsettings.json file
//Configure<TOptions>() automatically registers the settings object in the DI container as a singleton.
//When using IOptions<T> or IOptionsMonitor<T>, you do not need AddSingleton:
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


// Add services to the container.
builder.Services.AddControllers();

//CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS 
builder.Services.AddCors(co =>
{
    co.AddPolicy("CORS", pb =>
    {
        pb.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
//CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS

//JSON serealizer to ignore cycles
builder.Services.AddControllers()
.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//Register dependencies
//builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IFlashCardService, FlashCardService>();
builder.Services.AddScoped<ISetService, SetService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IFlashCardRepository, FlashCardRepository>();
builder.Services.AddScoped<ISetRepository, SetRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IHasher, Hasher>();


builder.Services.AddDbContext<FlashTimesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS
app.UseCors("CORS"); //<-USE CORS with your policy name
//CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS CORS

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();