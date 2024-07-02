using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using LMS.Persistence.Db;
using System.Linq;
using LMS.Domain.LeaveTypes;

namespace LMS.Service.Services
{
    public static class DataSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseService>();

                if (!context.LeaveTypes.Any())
                {
                    context.LeaveTypes.AddRange(
                        new LeaveType { Name = "Annual", DefaultDays = 0 },
                        new LeaveType { Name = "Sick", DefaultDays = 20 },
                        new LeaveType { Name = "Replacement", DefaultDays = 0 },
                        new LeaveType { Name = "Unpaid", DefaultDays = 10 }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
