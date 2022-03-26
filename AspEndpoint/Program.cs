global using AspEndpoint.Data;
global using Microsoft.EntityFrameworkCore;
using AspEndpoint;
using AspEndpoint.Services.FileService;
using AspEndpoint.Services.UserService;
using FileManagerLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")); 
});
builder.Services.AddSingleton<IFileManager, FileManager>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            LifetimeValidator = Jwt.ValidateLifeTime,
                            ValidateLifetime = true,
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            IssuerSigningKey = Jwt.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
