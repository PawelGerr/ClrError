using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClrIssueRepro
{
   public static class Program
   {
      public static readonly List<Guid> MainEntityIds = new()
                                                        {
                                                           new("bd0499b6-fe81-4b8f-871c-5d65040a35a8"),
                                                           new("55278dc8-88fb-4fa6-b19b-5a6ec9bdae1a"),
                                                           new("ec14afdf-412a-4b51-a6c5-6c4f920fef7a")
                                                        };

      public static async Task Main(string[] args)
      {
         var logLevel = LogLevel.Information;

         using var host = Host.CreateDefaultBuilder()
                              .ConfigureLogging(builder => builder.AddFilter(level => level >= logLevel))
                              .ConfigureServices((_, services) =>
                                                 {
                                                    services.AddDbContext<TestDbContext>((_, builder) => builder
                                                                                            .UseSqlite("Data Source=test.db;")
                                                                                        )
                                                            .AddScoped<IDependency_1, Dependency_1>()
                                                            .AddScoped<IDependency_2, Dependency_2>()
                                                            .AddScoped<IDependency_3, Dependency_3>()
                                                            .AddScoped<IDependency_4, Dependency_4>()
                                                            .AddScoped<IDependency_5, Dependency_5>()
                                                            .AddScoped<IDependency_6>()
                                                            .AddSingleton(ICulprit.Default) // <- the culprit
                                                            .AddControllers().AddApplicationPart(typeof(TestController).Assembly);
                                                 })
                              .ConfigureWebHost(builder => builder.UseKestrel()
                                                                  .Configure(app =>
                                                                             {
                                                                                app.UseRouting();
                                                                                app.UseEndpoints(endpoints => endpoints.MapControllerRoute("default", "api/{controller}/{action}"));
                                                                             }))
                              .Build();

         var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("test");

         using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
         {
            logger.LogInformation(">>>>>>>>>>>>>>>  Preparing DB");
            logLevel = LogLevel.Warning;

            var ctx = scope.ServiceProvider.GetRequiredService<TestDbContext>();
            await PrepareDbAsync(ctx);

            logLevel = LogLevel.Information;
            logger.LogInformation(">>>>>>>>>>>>>>>  DB prepared");
         }

         host.Start();

         using var httpClient = new HttpClient();

         for (var i = 0; i < 100; i++)
         {
            using var response = await httpClient.PostAsync("http://localhost:5000/api/test/test", new StringContent(String.Empty));
            response.EnsureSuccessStatusCode();

            logger.LogInformation($">>>>>>>>>>>>>>>  Iteration {i + 1}");
         }

         logger.LogInformation(">>>>>>>>>>>>>>>  Done");
      }

      private static async Task PrepareDbAsync(TestDbContext ctx)
      {
         await ctx.Database.EnsureCreatedAsync();

         foreach (var mainEntityId in MainEntityIds)
         {
            var mainEntity = await ctx.MainEntities.FirstOrDefaultAsync(s => s.Id == mainEntityId);

            if (mainEntity is null)
            {
               mainEntity = new MainEntity
                            {
                               Id = mainEntityId,
                               NavPropCollection = CreateNavEntities(mainEntityId),
                            };

               ctx.Add(mainEntity);
            }
         }

         await ctx.SaveChangesAsync();
      }

      private static ICollection<NavEntity> CreateNavEntities(Guid id)
      {
         var listings = new List<NavEntity>();

         for (var i = 0; i < 10000; i++)
         {
            listings.Add(new NavEntity
                         {
                            MainEntityId = id
                         });
         }

         return listings;
      }
   }
}
