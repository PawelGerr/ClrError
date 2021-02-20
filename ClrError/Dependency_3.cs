using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClrIssueRepro
{
   public class Dependency_3 : IDependency_3
   {
      public Dependency_3(
         TestDbContext ctx,
         IDependency_4 dependency4,
         IDependency_5 dependency5,
         IDependency_6 dependency6)
      {
      }
   }
}
