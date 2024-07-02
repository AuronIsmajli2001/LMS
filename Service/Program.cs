using EcommercePersistence;
using EcommerceService;
using EcommerceService.Helpers;
using LMS.Application.Interfaces;
using LMS.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Runtime.Loader;
using LMS.Persistence.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Application.Employees;
using Application.Holidays;
using Application.Interfaces;
using Application.LeaveAllocations;
using Application.LeaveRequests;
using Application.LeaveTypes;

namespace LMS.Service;

class Program
{
    static void Main(string[] args)
    {
        var files = Directory.GetFiles(
            AppDomain.CurrentDomain.BaseDirectory,
            "Ecommerce*.dll");

        var assemblies = files
            .Select(p => AssemblyLoadContext.Default.LoadFromAssemblyPath(p));

        var builder = WebApplication.CreateBuilder(args);

        // Register Authentication and Authorization
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireEmployeeRole", policy => policy.RequireRole("Employee"));
            options.AddPolicy("RequireLeadRole", policy => policy.RequireRole("Lead"));
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // Add Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lms", Version = "v1" });
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://sso-sts.gjirafa.dev/connect/authorize"),
                        TokenUrl = new Uri("https://sso-sts.gjirafa.dev/connect/token"),
                        Scopes = new Dictionary<string, string> { { "life_2024_api", "LifeApi" } }
                    }
                }
            });
            c.DocumentFilter<LowercaseDocumentFilter>();
            c.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        builder.Services.AddAdvancedDependencyInjection();

        builder.Services.Scan(p => p.FromAssemblies(assemblies)
            .AddClasses()
            .AsMatchingInterface());

        // Register the DatabaseService with the configuration and options
        builder.Services.AddDbContext<DatabaseService>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("LMS.Persistence.Db")));

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
        builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
        builder.Services.AddScoped<ILeaveAllocationService, LeaveAllocationService>();
        builder.Services.AddScoped<IHolidayService, HolidayService>();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddScoped<LeaveAccrualService>();

        builder.Services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseDefaultTypeSerializer()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                UsePageLocksOnDequeue = true,
                DisableGlobalLocks = true
            }));

        builder.Services.AddHangfireServer();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                c.DefaultModelExpandDepth(0);
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Life Ecommerce");
                c.OAuthClientId("d8ce3b13-d539-4816-8d07-b1e4c7087fda");
                c.OAuthClientSecret("4a9db740-2460-471a-b3a1-6d86bb99b279");
                c.OAuthAppName("Life Ecommerce");
                c.OAuthUsePkce();
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHangfireDashboard();
        RecurringJob.AddOrUpdate<LeaveAccrualService>(x => x.AccrueLeave(), Cron.Monthly(1));
        RecurringJob.AddOrUpdate<LeaveAccrualService>(x => x.ResetAnnualLeave(), Cron.Yearly);

        app.MapControllers();
        app.UseAdvancedDependencyInjection();

        // Seed initial data
        DataSeeder.Seed(app);

        app.Run();
    }
}
