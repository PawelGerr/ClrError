using System;

namespace ClrIssueRepro
{
   public class NavEntity
   {
      public Guid Id { get; set; }

      public Guid MainEntityId { get; set; }
      public MainEntity MainEntity { get; set; }
   }
}
