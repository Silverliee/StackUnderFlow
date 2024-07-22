using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using Bugsnag.AspNet.Core;
using Hangfire;
using Hangfire.Console;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using StackUnderFlow.Application.Middleware;
using StackUnderFlow.Application.Security;
using StackUnderFlow.Domains.Repository;
using StackUnderFlow.Domains.Services;
using StackUnderFlow.Infrastructure.Kubernetes;
using StackUnderFlow.Infrastructure.Settings;

namespace StackUnderFlow;

public abstract partial class Program
{
    private static FileVersionInfo GetAssemblyFileVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return FileVersionInfo.GetVersionInfo(assembly.Location);
    }

    private static readonly string MajorVersionTag = $"v{GetAssemblyFileVersion().FileMajorPart}";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Configuration de scope
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<IReactionService, ReactionService>();
        builder.Services.AddScoped<IRunnerService, RunnerService>();
        builder.Services.AddScoped<IScriptService, ScriptService>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IGroupRepository, GroupRepository>();
        builder.Services.AddScoped<ILikeRepository, LikeRepository>();
        builder.Services.AddScoped<IScriptRepository, ScriptRepository>();
        builder.Services.AddScoped<ISharingRepository, SharingRepository>();
        builder.Services.AddScoped<IStatusRepository, StatusRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISocialInteractionService, SocialInteractionService>();
        builder.Services.AddScoped<IScriptVersionRepository, ScriptVersionRepository>();
        builder.Services.AddScoped<IFriendRepository, FriendRepository>();
        builder.Services.AddScoped<IFollowRepository, FollowRepository>();
        builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        builder.Services.AddScoped<IFavoriteService, FavoriteService>();
        builder.Services.AddScoped<ICryptographer, Cryptographer>();
        // Configuration de singleton
        builder.Services.AddSingleton<AuthenticationMiddleware>();
        builder.Services.AddSingleton<KubernetesService>();
        builder.Services.AddSingleton<NotificationService>();
        builder.Services.AddSingleton(new ConcurrentDictionary<string, WebSocket>());
        // Configuration des controllers/endpoints
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        // Configuration de metrics
        builder.Services.AddOpenTelemetry().WithMetrics(x =>
        {
            x.AddPrometheusExporter();
            x.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
            x.AddView("Requests-duration",
                new ExplicitBucketHistogramConfiguration
                {
                    Boundaries = [0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2, 5, 10, 30, 60]
                });
        });
        // Configuration du health check
        builder.Services.AddHealthChecks();
        // Configuration de la base de données
        builder.Services.AddDbContext<MySqlDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("database"));
        });
        // Configuration de bugsnag
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddBugsnag(configuration =>
            {
                configuration.ApiKey = "ed4a17461033eee3aba33a045f4a5022";
                configuration.ReleaseStage = "development";
            });
        }
        else
        {
            builder.Services.AddBugsnag(configuration =>
            {
                configuration.ApiKey = "ed4a17461033eee3aba33a045f4a5022";
                configuration.ReleaseStage = "production";
            });
        }

        // Configuration de CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                policyBuilder => { policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }
            );
        });
        // Configuration de swagger
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
                    Description = "Tu peux mettre ton token ici ;) PS: ajoute Bearer suivie d'un espace avant le token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                }
            );

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "JWT-BEARER-TOKEN"
                        }
                    },
                    new string[] { }
                }
            });
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
        // Configuration de hangfire
        builder.Services.AddHangfire(o =>
        {
            o.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseFilter(new AutomaticRetryAttribute { Attempts = 0 });
            o.UseMemoryStorage();
            o.UseConsole();
        });
        builder.Services.AddHangfireServer(s => { s.SchedulePollingInterval = TimeSpan.FromSeconds(1); });


        var app = builder.Build();
        // Configure the HTTP request pipeline.
        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        // configure swagger
        app.UseSwagger();
        app.UseSwaggerUI();
        // configure authentication
        app.UseAuthentication();
        app.UseAuthorization();
        // configure metrics
        app.MapPrometheusScrapingEndpoint();
        app.UseWebSockets();

        // configure websocket
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/notifications"))
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var id = context.Request.Query["id"].ToString();
                    var notificationService = context.RequestServices.GetRequiredService<NotificationService>();
                    await notificationService.HandleAsync(context, id);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                await next();
            }
        });
        // configure hangfire
        app.UseHangfireDashboard(
            options: new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationMiddleware() },
                DarkModeEnabled = true,
                DisplayStorageConnectionString = false
            }
        );
        // map controllers and run the app
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.Run();
    }
}

public abstract partial class Program
{
}