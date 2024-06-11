using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackUnderFlow.Application.Middleware;
using StackUnderFlow.Domains.Repository;
using StackUnderFlow.Domains.Services;
using StackUnderFlow.Infrastructure.Settings;

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
        var connectionString = builder.Configuration.GetConnectionString("database");
        builder.Services.AddDbContext<MySqlDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }
            );
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<IReactionService, ReactionService>();
        builder.Services.AddScoped<IRunnerService, RunnerService>();
        builder.Services.AddScoped<IScriptService, ScriptService>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IGroupRepository, GroupRepository>();
        builder.Services.AddScoped<ILikeRepository, LikeRepository>();
        builder.Services.AddScoped<IPipelineRepository, PipelineRepository>();
        builder.Services.AddScoped<IScriptRepository, ScriptRepository>();
        builder.Services.AddScoped<ISharingRepository, SharingRepository>();
        builder.Services.AddScoped<IStatusRepository, StatusRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISocialInteractionService, SocialInteractionService>();
        builder.Services.AddScoped<IScriptVersionRepository, ScriptVersionRepository>();
        builder.Services.AddScoped<IFriendRepository, FriendRepository>();
        builder.Services.AddScoped<IFollowRepository, FollowRepository>();

        builder.Services.AddSingleton<AuthenticationMiddleware>();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                MajorVersionTag,
                new OpenApiInfo
                {
                    Title = "StackUnderFlow API",
                    Version = "V1.0.0",
                    Description =
                        "StackUnderFlow API by Damien Willemain, Nourdine Arbaoui, Mohamed Seydou Traore",
                    Contact = new OpenApiContact
                    {
                        Name = "Traore Mohamed Seydou",
                        Email = "mohamedstrore@hotmail.fr",
                        Url = new Uri("https://github.com/Silverliee")
                    }
                }
            );

            options.AddSecurityDefinition(
                "JWT-BEARER-TOKEN",
                new OpenApiSecurityScheme
                {
                    Description = "Tu peux mettre ton token ici ;)",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                }
            );
        });

        // Configuration de JWT
        builder
            .Services.AddAuthentication(options =>
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
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                    )
                };
            });

        var app = builder.Build();

        app.UseCors("AllowAll");
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseCors("AllowAll");
        app.Run();
    }
}

public partial class Program
{
    protected Program() { }
}