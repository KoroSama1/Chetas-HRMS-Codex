using System.Text;
using AttendanceApi.Data;
using AttendanceApi.Middlewares;
using AttendanceApi.Repositories.Implementations;
using AttendanceApi.Repositories.Interfaces;
using AttendanceApi.Services.Implementations;
using AttendanceApi.Services.Interfaces;
using AttendanceApi.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides; // to trust Nginx proxy 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FIX 1: Use "DefaultConnection" to match your docker-compose environment variable
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ======================= repos =======================
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
builder.Services.AddScoped<IDesignationRepo, DesignationRepo>();
builder.Services.AddScoped<IRegionRepo, RegionRepo>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();

// ======================= SERVICES =======================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// ======================= General =======================
builder.Services.AddScoped<JwtHelper>();

// FIX 2: Updated CORS to ensure it covers both the IP and localhost for testing
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("https://192.168.1.123", "http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"] ?? "YourSuperSecretFallbackKey123!"
                )
            ),
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// FIX 3: Auto-migrate Database on startup (Useful for Docker deployments)
//using (var scope = app.Services.CreateScope())
//{
//   var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//   db.Database.Migrate();
//}

// ======================= MIDDLEWARE =======================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

/* Exception Middleware */
app.UseMiddleware<ExceptionMiddleware>();

// to trust Nginx proxy 
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
});

/* ?? STATIC FILES FOR IMAGES */
var uploadPath = builder.Configuration["FileStorage:UploadPath"];

app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.GetFullPath(uploadPath)),
        RequestPath = "/uploads",
    }
);

app.UseSwagger();
app.UseSwaggerUI();

// app.UseRouting() is implicit in .NET 6+, but keeping it is fine.
app.UseRouting();

// CORS must be strictly after UseRouting and before UseAuthentication
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
