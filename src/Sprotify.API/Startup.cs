using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sprotify.API.Entities;
using Microsoft.EntityFrameworkCore;
using Sprotify.API.Services;
using Microsoft.AspNetCore.Http;

namespace Sprotify.API
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();

            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddJsonFormatters().AddDataAnnotations()
                .AddAuthorization();
                     
            // Never put a production connection string in an appsettings file.  
            // Use environment variables for that.
            var connectionString = Configuration["connectionStrings:sprotifyDBConnectionString"];
            services.AddDbContext<SprotifyContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<ISprotifyRepository, SprotifyRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            SprotifyContext sprotifyContext)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        // ensure generic 500 status code on fault.
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            // use default files (like index.html for root)
            app.UseDefaultFiles();

            // allow serving up files from wwwroot (eg images)
            app.UseStaticFiles();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Playlist, Models.Playlist>();
                cfg.CreateMap<Services.Models.PlaylistWithSongs, Models.PlaylistWithSongs>();
                cfg.CreateMap<Models.PlaylistForCreation, Entities.Playlist>();
                cfg.CreateMap<Entities.Song, Models.Song>();
                cfg.CreateMap<Services.Models.SongWithPlaylistInfo, Models.SongInPlaylist>();
                cfg.CreateMap<Models.SongForCreation, Entities.Song>();
                cfg.CreateMap<Models.SongForUpdate, Entities.Song>().ReverseMap();
                cfg.CreateMap<Entities.User, Models.User>();
                cfg.CreateMap<Entities.User, Models.UserWithPlaylists>();

                cfg.CreateMap<Models.SongForPlaylist, Entities.PlaylistSong>();
                
                cfg.CreateMap<Entities.PlaylistSong, Models.SongInPlaylist>()
                    .ForMember(x => x.Id, opt => opt.MapFrom(src => src.SongId))
                    .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Song.Title))
                    .ForMember(x => x.Band, opt => opt.MapFrom(src => src.Song.Band))
                    .ForMember(x => x.Duration, opt => opt.MapFrom(src => src.Song.Duration));
            });

            // test for, and if required, migrate the DB
            sprotifyContext.Database.Migrate();

            // ensure seed data
            sprotifyContext.EnsureSeedDataForContext();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "https://localhost:44375/",
                RequireHttpsMetadata = true,
                ApiName = "sprotifyapi"
            });
           
            app.UseMvc();             
        }
    }
}
