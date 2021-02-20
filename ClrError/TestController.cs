using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClrIssueRepro
{
   public class TestController : Controller
   {
      private readonly TestDbContext _ctx;
      private readonly IDependency_1 _dependency1;

      public TestController(
         TestDbContext ctx,
         IDependency_1 dependency1)
      {
         _ctx = ctx;
         _dependency1 = dependency1 ?? throw new ArgumentNullException(nameof(dependency1));
      }

      [HttpPost]
      public async Task Test()
      {
         await _ctx.MainEntities
                   .AsSplitQuery()
                   .Include(s => s.NavPropCollection)
                   .Where(s => Program.MainEntityIds.Contains(s.Id))
                   .ToListAsync().ConfigureAwait(false);
      }
   }
}
