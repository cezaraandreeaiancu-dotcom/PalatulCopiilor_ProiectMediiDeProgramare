using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Palatul_Copiilor.Data;
using Palatul_Copiilor.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Contextul aplicației (tabelele tale: Department, Activity, Teacher, Reservation)
builder.Services.AddDbContext<Palatul_CopiilorContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Palatul_CopiilorContext")
        ?? throw new InvalidOperationException("Connection string 'Palatul_CopiilorContext' not found.")));

// Contextul de Identity (tabelele AspNetUsers, AspNetRoles, etc.)
builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Palatul_CopiilorContext")
        ?? throw new InvalidOperationException("Connection string 'Palatul_CopiilorContext' not found.")));

// Identity (cookie auth este adăugat automat aici)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<LibraryIdentityContext>();

// =====================
// CORS pentru MAUI
// =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("maui", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// =====================
// JWT Auth pentru Mobile
// =====================
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? "";
var jwtIssuer = jwtSection["Issuer"] ?? "";
var jwtAudience = jwtSection["Audience"] ?? "";

builder.Services.AddAuthentication()
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});


builder.Services.AddAuthorization();

// =====================
// Swagger (recomandat pt test API)
// =====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PalatulCopiilor API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introdu: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Seed roluri / useri etc.
using (var scope = app.Services.CreateScope())
{
    await IdentitySeed.SeedAsync(scope.ServiceProvider);
}

// =====================
// Pipeline
// =====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("maui");

// IMPORTANT pentru Identity + JWT:
app.UseAuthentication();
app.UseAuthorization();

// =====================
// API: LOGIN (JWT)
// =====================
app.MapPost("/api/auth/login", async (
    LoginRequest req,
    UserManager<IdentityUser> userManager,
    IConfiguration config) =>
{
    var user = await userManager.FindByEmailAsync(req.Email);
    if (user == null) return Results.Unauthorized();

    var ok = await userManager.CheckPasswordAsync(user, req.Password);
    if (!ok) return Results.Unauthorized();

    var jwt = config.GetSection("Jwt");
    var keyStr = jwt["Key"] ?? "";
    if (keyStr.Length < 16) return Results.Problem("Jwt:Key lipsește sau e prea scurtă în appsettings.json.");

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Name, user.UserName ?? "")
    };

    var expireMinutes = int.TryParse(jwt["ExpireMinutes"], out var m) ? m : 120;
    var expires = DateTime.UtcNow.AddMinutes(expireMinutes);

    var token = new JwtSecurityToken(
        issuer: jwt["Issuer"],
        audience: jwt["Audience"],
        claims: claims,
        expires: expires,
        signingCredentials: creds
    );

    var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new LoginResponse(tokenStr, expires));
});

// =====================
// API: Rezervările MELE (JWT required)
// =====================
app.MapGet("/api/reservations/my", async (
    ClaimsPrincipal user,
    Palatul_CopiilorContext db) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

    var items = await db.Reservation
        .Where(r => r.UserId == userId)
        .Include(r => r.Activity)!.ThenInclude(a => a!.Department)
        .Include(r => r.Activity)!.ThenInclude(a => a!.Teacher)
        .OrderBy(r => r.Activity!.StartAt)
        .Select(r => new ReservationDto(
            r.Id,
            r.ReservedAt,
            r.ActivityId,
            r.Activity!.Title,
            r.Activity!.StartAt,
            r.Activity!.Department != null ? r.Activity.Department.Name : null,
            r.Activity!.Teacher != null ? r.Activity.Teacher.FullName : null
        ))
        .ToListAsync();

    return Results.Ok(items);
})
.RequireAuthorization(new AuthorizeAttribute
{
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
});

// Razor Pages (web)
app.MapRazorPages();

app.Run();
