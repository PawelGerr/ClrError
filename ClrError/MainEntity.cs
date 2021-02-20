using System;
using System.Collections.Generic;

namespace ClrIssueRepro
{
   public class MainEntity
   {
      public Guid Id { get; set; }

      public ICollection<NavEntity> NavPropCollection { get; set; }

      public MainEntity()
      {
         NavPropCollection = new HashSet<NavEntity>();
      }
   }
}
