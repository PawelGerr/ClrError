using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClrIssueRepro
{
   public class Dependency_1 : IDependency_1
   {
      private readonly ICulprit _culprit;

      public Dependency_1(TestDbContext ctx,
                                  IDependency_2 dependency2,
                                  IDependency_3 dependency3,
                                  ICulprit culprit)
      {
         _culprit = culprit ?? throw new ArgumentNullException(nameof(culprit));
      }
   }
}
