using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ESB.Resolver;

namespace ESB.Extensions.Library.Resolvers.Bre
{
   

    public static  class ResolutionExtended
    {

       public static string Namespace(this Resolution resolution)
       {
           return resolution.MessageType.Contains("#") ? resolution.MessageType.Split('#')[0] : null;
       }

        public static string MessageName(this Resolution resolution)
        {
            return resolution.MessageType.Contains("#") ? resolution.MessageType.Split('#')[1] : resolution.MessageType;
        }
    }
}
