using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;

using Microsoft.Practices.ESB.Exception.Management;
using Microsoft.Practices.ESB.Resolver;
using Microsoft.RuleEngine;
using Microsoft.Practices.ESB.Resolver.Itinerary.Facts;
   

namespace ESB.Extensions.Library.Resolvers.Bre
{
    /// <summary>
    /// A Policy Broker Class used to resolve/lookup the name, version and policy information of another published BRE Policy. The purpose of this class is to minimize the amount of rules needed inside a policy for complex designs, as well as allowing better organization of rules within Policies. This class also assists in complex situtations where multiple rules can fire due to multiple conditions evaluating successfully. The Policy Broker class will allow an architect divide multiple conditions inside multiple rules into better designed, categorized single-rule policies such that only 1 rule fires.
    /// </summary>
    public class PolicyBroker
    {
        private PolicyExecutor policyExecutor;

        public PolicyBroker(bool debug, string outputPath)
        {
           this.policyExecutor = new PolicyExecutor(debug, outputPath );
        }


        /// <summary>
        /// This method uses a BRE Policy to lookup and return information about another BRE Policy. (Broker Pattern)
        /// </summary>
        /// <param name="message">Xml Message for use of the policy lookup process</param>
        /// <param name="resolution">An instance of the resolution class for use in the policy lookup process</param>
        /// <param name="PolicyName">The Name of the BRE Policy for looking up which policy information to return</param>
        /// <param name="itineraryInfo">Instance of an Itinerary class which contains the attached itinerary for use in the policy lookup process</param>
        /// <param name="factRetreiver">Instance of MessageContextFactRetriever which contains BizTalk Message Promoted and Distinguished properties for use in the policy lookup process</param>
        /// <param name="major">Major version of the business rule to execute for determining which policyinfo to return</param>
        /// <param name="minor">Minor version of the business rule to execute for determining which policyinfo to return</param>
        /// <returns>PolicyBrokerInfo class containing information about the name, version, document type, and namespace of a Policy to execute</returns>
      public PolicyBrokerInfoCollection  GetPolicies(XmlDocument message, Resolution resolution, string PolicyName, ItineraryFact itineraryInfo, MessageContextFactRetriever factRetreiver, int major, int minor)
      {

          EventLogger.Write("Executing PolicyBroker!");

          string documentType = "Microsoft.Practices.ESB.ResolveProviderMessage";
        
          
          string docNameSpace = resolution.Namespace();
          string docName = resolution.MessageName();

          EventLogger.Write("Document Namespace is: " + docNameSpace);
          EventLogger.Write("Document Name is: " + docName);

          PolicyBrokerInfo _info = new PolicyBrokerInfo() { DocNamespace = docNameSpace, DocName = docName };
          PolicyBrokerInfoCollection infos = new PolicyBrokerInfoCollection();
          infos.AddPolicyInfo(docName, docNameSpace, string.Format("{0}#{1}", docNameSpace, docName), "");

          object[] facts = new object[5];
          TypedXmlDocument document = new TypedXmlDocument(documentType, message);
         
          facts[0] = resolution;
          facts[1] = document;
          facts[2] = infos;
          facts[3] = itineraryInfo;
          facts[4] = factRetreiver;
          

          EventLogger.Write("Document Content is: " + message.InnerXml);

          bool useVersioning = (major > 0 || minor > 0);
          if (useVersioning )
              this.policyExecutor.ExecutePolicy(PolicyName, facts, major, minor );
          else
            this.policyExecutor.ExecutePolicy(PolicyName, facts);

          foreach (var policyInfo in infos.PolicyInfos )
          {
              var info = policyInfo.Value;
                  if (string.IsNullOrEmpty(info.PolicyName))
                  {
                      EventLogger.Write(string.Format(
                                  "Policy Broker could not found policy in Policy {0} for message name {1} with namespace {2}!",
                                  info.PolicyName,
                                  info.DocName,
                                  info.DocNamespace));
                  }
              EventLogger.Write("Found Policy " + info.PolicyName + "\nVersion: " + info.Version);
              
          }
          return infos;
      }
    }
}
