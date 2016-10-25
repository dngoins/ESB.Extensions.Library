using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESB.Extensions.Library.Resolvers.Bre
{
    [Serializable]
  public class PolicyNotFoundException:Exception
    {

      public PolicyNotFoundException(string message):base(message)
      {
          
      }
    }
}
