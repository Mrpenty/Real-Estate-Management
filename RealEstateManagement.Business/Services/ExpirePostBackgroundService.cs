using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services
{
    public class ExpirePostBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpirePostBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RentalDbContext>();

                var now = DateTime.UtcNow;

                var expiredPosts = await context.PropertyPosts
                .Include(p => p.Property)
                .Where(p => p.ArchiveDate.HasValue
                            && p.ArchiveDate < now
                            && p.Status != PropertyPost.PropertyPostStatus.Rejected
                            && p.Status != PropertyPost.PropertyPostStatus.Sold
                            && p.Status != PropertyPost.PropertyPostStatus.Rented)
                .ToListAsync();


                foreach (var post in expiredPosts)
                {
                    post.Status = PropertyPost.PropertyPostStatus.Expired;
                }

                if (expiredPosts.Any())
                    await context.SaveChangesAsync();

                // Lặp lại mỗi 6 tiếng
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }
}
