using Entites.Models;
using Microsoft.AspNetCore.Identity;
using Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using Repository;
using Service.Contracts;
using Service;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ExternalLibraryService;

namespace EveryPinApi.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
    services.AddCors(options =>
    {
        // Cors 관련 설정

        // 현재는 모든 도메인을 CORS 정책으로부터 허용함
        options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

        // 특정 도메인만 허용하도록 수정
        //options.AddPolicy("CorsPolicy", builder =>
        //builder.WithOrigins("https://example.com")      // 입력한 도메인만 접근 가능
        //.WithMethods("POST", "GET")                     // POST, GET 방식만 접근 가능
        //.WithHeaders("accept", "content-type"));        // 특정 헤더만 허용

    });

    public static void ConfigureLoggerFile(this IServiceCollection services) =>
    services.Configure<AzureFileLoggerOptions>(options =>
    {
        // Azure Logging 파일시스템 관련 설정
        options.FileName = "logs-";
        options.FileSizeLimit = 50 * 1024;      // 파일 크기 제한
        options.RetainedFileCountLimit = 5;     // 최대 보존 파일 수
    });

    public static void ConfigureLoggerBlob(this IServiceCollection services) =>
    services.Configure<AzureBlobLoggerOptions>(options =>
    {
        // Azure Logging 블롭 관련 설정
        options.BlobName = "log.txt";
    });

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
    services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
    services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
    services.AddSqlServer<RepositoryContext>(configuration.GetConnectionString("everypindb"));

    public static void ConfigureBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        string storageAccount = configuration.GetConnectionString("azure-storage-account");
        string storageContainer = configuration.GetConnectionString("azure-storage-container");
        string storageAccessKey = configuration.GetConnectionString("azure-storage-access-key");

        services.AddSingleton(new BlobHandlingService(storageAccessKey, storageAccount, storageContainer));
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(option =>
        {
            // Password settings.
            //option.Password.RequireDigit = true;
            //option.Password.RequireLowercase = false;
            //option.Password.RequireUppercase = false;
            //option.Password.RequireNonAlphanumeric = false;
            //option.Password.RequiredLength = 10;
            //option.Password.RequiredUniqueChars = 1;

            option.Password.RequireDigit = false;
            option.Password.RequireLowercase = false;
            option.Password.RequireUppercase = false;
            option.Password.RequireNonAlphanumeric = false;
            option.Password.RequiredLength = 1; // 비밀번호 길이 요구 사항을 최소값으로 설정


            // Lockout settings.
            //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //options.Lockout.MaxFailedAccessAttempts = 5;
            //options.Lockout.AllowedForNewUsers = true;

            // User settings.
            option.User.AllowedUserNameCharacters = string.Empty;       // 한글 사용 가능
            option.User.RequireUniqueEmail = true;                     // true: 유저 이메일 중복 불가능, false: 중복 가능
        })
        .AddEntityFrameworkStores<RepositoryContext>()
        .AddDefaultTokenProviders();
    }

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var validIssuer = configuration.GetConnectionString("JwtSettings-validIssuer");
        var validAudience = configuration.GetConnectionString("JwtSettings-validAudience");
        var secretKey = configuration.GetConnectionString("JwtSettings-SECRET");
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer.ToString(),
                ValidAudience = validAudience.ToString(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey.ToString()))
            };
        });
    }

}
