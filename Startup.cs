
using AutoMapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using packagesentinel.Automapper;
using packagesentinel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace packagesentinel
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {


      // One Db is used for both local and remote, however, they have different IP addresses because 
      //Virtual Machine can not access its Public IP address 
      // 142.55.32.86 and 172.17.0.2 points to the SAME exact sql server

      string LOCAL_DB_CONNECTION = "";
      string REMOTE_DB_CONNECTION = ""

      var workingDirectory = Environment.CurrentDirectory;
       // if the file dev, it's local and not the VM
      var file = $"{workingDirectory}\\{"DEV"}";
      var isLocal = File.Exists(file);
      string DB_CONNECTION_STRING = isLocal ? LOCAL_DB_CONNECTION : REMOTE_DB_CONNECTION;

      services.AddDbContextPool<APIContext>(options => options.UseSqlServer(DB_CONNECTION_STRING));
     
      services.AddSwaggerGen(swagger =>
      {
        //  swagger.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
          //This is to generate the Default UI of Swagger Documentation  
          swagger.SwaggerDoc("v1", new OpenApiInfo
        {
          Version = "v1",
          Title = "PACKAGE SENTINAL",
          Description = "API"
        });
              // To Enable authorization using Swagger (JWT)  
              swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer",
          BearerFormat = "JWT",
          In = ParameterLocation.Header,
          Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        });
        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
          {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
          });
      });

      services.AddAuthentication(option =>
      {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

      }).AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
           
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = false,
          ValidateIssuerSigningKey = true,
          ValidIssuer = Configuration["Jwt:Issuer"],
          ValidAudience = Configuration["Jwt:Issuer"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]  
              };
      });


      services.AddIdentity<ApplicationUser, IdentityRole>(options => {

          options.Password = new PasswordOptions
          {
              RequiredLength = 4,
              RequireDigit = false,
              RequireLowercase = false,
              RequireUppercase = false,
              RequireNonAlphanumeric = false,

          };
      }).AddEntityFrameworkStores<APIContext>().AddDefaultTokenProviders();

      services.AddScoped<APIContext, APIContext>();
      services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
      services.AddTransient<IDeviceRepository, DeviceRepository>();
      services.AddTransient<IUnitOfWork, UnitOfWork>();

      services.AddControllersWithViews().AddNewtonsoftJson().AddRazorRuntimeCompilation();


      FirebaseApp.Create(new AppOptions()
      {
        Credential = GoogleCredential.FromFile("package-sentinel-service-account-file.json"),
      });

            //Automapper configurations
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      //if (env.IsDevelopment())
      //{
      app.UseDeveloperExceptionPage();
      //}
      //else
      //{
      //    app.UseExceptionHandler("/Home/Error");
      //}
      app.UseStaticFiles();
      app.UseSwagger();

      app.UseRouting();
      app.UseHttpsRedirection();
      app.UseAuthorization();
      app.UseAuthentication();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
      });
      app.UseSwaggerUI(c =>
      {
         // c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "PACKAGE SENTINAL");
        
      });
      // InitialData.EnsurePopulated(app);
    }

  }
}

