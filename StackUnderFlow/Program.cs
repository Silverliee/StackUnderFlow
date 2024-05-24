using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public partial class Program
{
    private static FileVersionInfo GetAssemblyFileVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return FileVersionInfo.GetVersionInfo(assembly.Location);
    }

    private static readonly string Name =
        Assembly.GetExecutingAssembly().GetName().Name ?? "No name";

    private static readonly string FullVersion = GetAssemblyFileVersion().FileVersion ?? "0.0.0";
    private static readonly string MajorVersionTag = $"v{GetAssemblyFileVersion().FileMajorPart}";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                MajorVersionTag,
                new OpenApiInfo
                {
                    Title = "StackUnderFlow API",
                    Version = "V1.0.0",
                    Description = "StackUnderFlow API by Damien Willemain, Nourdine Arbaoui, Mohamed Seydou Traore",
                    Contact = new OpenApiContact
                    {
                        Name = "Traore Mohamed Seydou",
                        Email = "mohamedstrore@hotmail.fr",
                        Url = new Uri("https://github.com/Silverliee")
                    }
                });
        });

        // Configuration de JWT
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer =builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                    )
                };
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
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
    }
}

public partial class Program
{
    protected Program()
    {
    }
}