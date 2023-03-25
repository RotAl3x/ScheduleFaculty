using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScheduleFaculty.API.Utils;
using ScheduleFaculty.Core;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllersWithViews().AddNewtonsoftJson();
services.AddRazorPages();

//services.AddAutoMapper(typeof(MappingProfile));

services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
services.AddSwaggerGen();

var connectionString = configuration.GetConnectionString("Default");
services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString, 
    x => x.MigrationsAssembly("ScheduleFaculty.Core")));

var app = builder.Build();

app.Run();