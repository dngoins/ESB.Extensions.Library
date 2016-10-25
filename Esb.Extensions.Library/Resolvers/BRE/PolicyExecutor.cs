using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESB.Extensions.Library.Resolvers.Bre
{
    using System.IO;

    using Microsoft.Practices.ESB.Exception.Management;
    using Microsoft.RuleEngine;
    
    public class PolicyExecutor
    {
       public bool debug;

        private string debugPath;
    

      public PolicyExecutor(bool turnOnDebugInterceptor, string debugOutputPath)
        {
            try
            {
                bool results = turnOnDebugInterceptor ;

                if (!results)
                {
                    return;
                }

                if (debug)
                {
                    this.debugPath = debugOutputPath;
                    EventLogger.Write("Enabling debuging support.");
                }
              
            }
            catch (Exception ex)
            {
                EventLogger.Write("Unable to load Debug Flag!\n" + ex);
            }
            
        }



        public void ExecutePolicy(string PolicyName,object facts)
        {
            ExecutePolicy(PolicyName, facts, 0, 0);
        }

        public void ExecutePolicy(string PolicyName, object facts, int Major, int Minor)
        {
            Policy brokerPolicy = null;
            if (Major > 0 || Minor > 0)
            {
                brokerPolicy = new Policy(PolicyName, Major, Minor);
            }
            else
            {
                brokerPolicy = new Policy(PolicyName);
            } 

            if (this.debug)
            {
                string fileName = string.Format("PolicyBrokerDebug_{0}_{1}.xml", PolicyName, Guid.NewGuid());

                string folderPath = Path.GetDirectoryName(this.debugPath);
                string outputPath = Path.Combine(folderPath, fileName);

                EventLogger.Write("Writing output to " + outputPath);

                DebugTrackingInterceptor debugTracking = new DebugTrackingInterceptor(outputPath);

                brokerPolicy.Execute(facts, debugTracking);
            }
            else
            {
                brokerPolicy.Execute(facts);
            }

            brokerPolicy.Dispose();
        }

    }
}
