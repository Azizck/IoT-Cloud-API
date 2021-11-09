using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using packagesentinel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace packagesentinel.Models
{
    public class InitialData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            APIContext context = app.ApplicationServices
                                        .CreateScope().ServiceProvider
                                        .GetRequiredService<APIContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Device.Any())
            {

                context.Device.AddRange(
                        new Device() {Note = "Created by Default",Status="On",CreatedAt=DateTime.Now,Version="V.12"}                                        

                    );
              



                context.SaveChanges();




            }


        }
    }
}
