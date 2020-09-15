using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace BE.Dal.EfStructures
{
    public class BEIdentityContextFactory : IDesignTimeDbContextFactory<BEIdentityContext>
    {
        public BEIdentityContext CreateDbContext(string[] args)
        {
            var connectionString =
                @"Server=.; Database=BED; MultipleActiveResultSets=true; integrated security=true";
            var optionsBuilder = new DbContextOptionsBuilder<BEIdentityContext>().EnableSensitiveDataLogging(true);
            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            Console.WriteLine(connectionString);
            return new BEIdentityContext(optionsBuilder.Options);
        }
    }
}
