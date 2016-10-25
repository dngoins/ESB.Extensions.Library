using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESB.Extensions.Library.Resolvers.Bre
{
    public class PolicyBrokerInfoCollection
    {
        public Dictionary<string, PolicyBrokerInfo> PolicyInfos { get { return _policyInfos; } set { _policyInfos = value; } }

        private Dictionary<string, PolicyBrokerInfo> _policyInfos;

        public PolicyBrokerInfoCollection()
        {
            _policyInfos = new Dictionary<string, PolicyBrokerInfo>();

        }

        public void AddPolicyInfo(string docName, string docNS, string policyName, string version)
        {
            _policyInfos.Add(string.Format("{0}#{1}#{2}", docNS, docName, policyName), new PolicyBrokerInfo() { DocName=docName, DocNamespace=docNS, PolicyName=policyName, Version=version });

        }

        public PolicyBrokerInfo GetPolicyInfo(string docNS, string docName, string policyName)
        {
            var key = string.Format("{0}#{1}#{2}", docNS, docName, policyName);
            if (_policyInfos.ContainsKey(key))
            {
                return _policyInfos[key];
            }
            return null;
        }

        public void UpdatePolicyInfo(string docNS, string docName, string policyToUpdate, string propertyName, string val)
        {            
            var policyInfo = GetPolicyInfo(docNS, docName, policyToUpdate);
            if (policyInfo == null) return;

            switch (propertyName.ToLowerInvariant())
            {
                case "docname":
                    policyInfo.DocName = val;
                    break;
                case "docnamespace":
                    policyInfo.DocNamespace = val;
                    break;
                case "policyname":
                    policyInfo.PolicyName = val;
                    break;
                case "version":
                    policyInfo.Version = val;                
                    break;
            }

        }
    }
}
