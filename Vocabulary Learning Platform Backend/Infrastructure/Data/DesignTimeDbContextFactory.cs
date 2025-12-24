//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Data
//{
//    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//    {
//        public AppDbContext CreateDbContext(string[] args)
//        {
//            // Get the directory where the Infrastructure project is located
//            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../Api"));

//            // Or try this alternative if above doesn't work:
//            // var basePath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
//            // basePath = Path.Combine(basePath, "Api");

//            IConfiguration configuration = new ConfigurationBuilder()
//                .SetBasePath(basePath) // Point to the API project folder
//                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//                .AddJsonFile($"appsettings.Development.json", optional: true)
//                .Build();

//            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//            var connectionString = configuration.GetConnectionString("DefaultConnection");

//            if (string.IsNullOrEmpty(connectionString))
//            {
//                // Fallback connection string for design-time
//                connectionString = "Server=.\\SQLEXPRESS; Database=VocabLearningDb; Trusted_Connection=True; TrustServerCertificate=true; MultipleActiveResultSets=true";
//            }

//            optionsBuilder.UseSqlServer(connectionString);

//            return new AppDbContext(optionsBuilder.Options);
//        }
//    }
//}
