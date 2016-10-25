using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESB.Extensions.Library.Resolvers.Bre
{

    /// <summary>
    /// Class used by the PolicyBroker class to resolve, and return information about another resolved, found BRE Policy.
    /// </summary>
  public class PolicyBrokerInfo
    {

        public string PolicyName { get; set; }

        public string Version { get; set; }

        public string DocNamespace { get; set; }

        public string DocName { get; set; }
    }
}
