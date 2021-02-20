using System;

namespace ClrIssueRepro
{
   public interface ICulprit
   {
      public static readonly ICulprit Default = new Culprit();

      private struct Culprit : ICulprit
      {
      }
   }
}
