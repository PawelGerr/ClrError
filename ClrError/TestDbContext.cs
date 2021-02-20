using Microsoft.EntityFrameworkCore;

namespace ClrIssueRepro
{
   public class TestDbContext : DbContext
   {
      public DbSet<MainEntity> MainEntities { get; set; }
      public DbSet<NavEntity> NavEntities { get; set; }

      public TestDbContext(DbContextOptions<TestDbContext> options)
         : base(options)
      {
      }
   }
}
