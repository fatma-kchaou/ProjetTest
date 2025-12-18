using System;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Tests
{
    public static class TestDbContextFactory
    {
        public static ApplicationContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // base unique pour chaque test
                .Options;

            return new ApplicationContext(options);
        }
    }
}
