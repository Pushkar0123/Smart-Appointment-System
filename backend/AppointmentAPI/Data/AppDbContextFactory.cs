// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using System;

// namespace AppointmentAPI.Data
// {
//     public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//     {
//         public AppDbContext CreateDbContext(string[] args)
//         {
//             var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

//             if (string.IsNullOrWhiteSpace(databaseUrl))
//             {
//                 throw new Exception("DATABASE_URL is required for migrations.");
//             }

//             var uri = new Uri(databaseUrl);
//             var userInfo = uri.UserInfo.Split(':');

//             var connectionString =
//                 $"Host={uri.Host};" +
//                 $"Port={uri.Port};" +
//                 $"Database={uri.AbsolutePath.TrimStart('/')};" +
//                 $"Username={userInfo[0]};" +
//                 $"Password={userInfo[1]};" +
//                 $"Ssl Mode=Require;" +
//                 $"Trust Server Certificate=true";

//             var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//             optionsBuilder.UseNpgsql(connectionString);

//             return new AppDbContext(optionsBuilder.Options);
//         }
//     }
// }

