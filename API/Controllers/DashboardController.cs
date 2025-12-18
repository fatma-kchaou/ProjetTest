using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.DTOs;
using API.DTOs.Dashboard;

namespace webappAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationContext context;

        public DashboardController(ApplicationContext context)
        {
            this.context = context;
        }

        // GET: api/dashboard/stats
        [HttpGet("stats")]
        public ActionResult<DashboardStatsDto>GetStats()
        {
            try
            {
                var totalListings = context.Livres.Count();
                var pendingBorroweds = context.Livres.Count(c => !c.IsBorrowed);
                var BorrowedListings = context.Livres.Count(c => c.IsBorrowed);
                var totalUsers = context.Users.Count();


                // Get recent listings
                var recentListings = context.Livres
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(10)
                    .Select(c => new RecentListingDto
                    {
                        LivreID = c.LivreID,
                        Title = c.Title,
                        Author = c.Author,
                        IsBorrowed = (bool)c.IsBorrowed,
                        CreatedAt = c.CreatedAt
                    })
                    .ToList();


                var stats = new DashboardStatsDto
                {
                    TotalListings = totalListings,
                    TotalUsers = totalUsers,
                    RecentListings = recentListings,

                };

               return Ok(stats); 
            }
            catch (Exception ex)
            {
                return BadRequest( "Error retrieving dashboard stats" );
            }
        }

        // GET: api/dashboard/analytics
        [HttpGet("analytics")]
        public ActionResult<DashboardAnalyticsDto> GetAnalytics()
        {
            try
            {
                // Get listings by category
                var listingsByCategory = context.Livres
                    .Where(c => c.Category != null)
                    .Include(c => c.Category)
                    .GroupBy(c => c.Category!.CategoryName)
                    .Select(g => new CategoryStatsDto
                    {
                        Category = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                // Add uncategorized count if any
                var uncategorizedCount = context.Livres
                    .Count(c => c.Category == null);

                if (uncategorizedCount > 0)
                {
                    listingsByCategory.Add(new CategoryStatsDto
                    {
                        Category = "Uncategorized",
                        Count = uncategorizedCount
                    });
                }

                // Get user registrations by month (last 12 months)
                var userRegistrations = new List<MonthlyRegistrationDto>();
                for (int i = 11; i >= 0; i--)
                {
                    var targetDate = DateTime.Now.AddMonths(-i);
                    var count = context.Users
                        .Where(u => u.Id != null) // Placeholder - modify if UserCreatedAt is added
                        .Count();

                    userRegistrations.Add(new MonthlyRegistrationDto
                    {
                        Month = targetDate.ToString("MMM yyyy"),
                        Count = count
                    });
                }

               

                var analytics = new DashboardAnalyticsDto
                {
                   
                    ListingsByCategory = listingsByCategory,
                    UserRegistrations = userRegistrations,
                };

                return Ok(analytics
                );
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving analytics" );
            }
        }

       
    }
}
