using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.ESB.Adapter;
using Microsoft.BizTalk.Message.Interop;


namespace ESB.Extensions.Library.AdapterProviders.MSMQ
{
    /// <summary>
    /// Ths class implements IAdapterProvider for setting the context properties of
    /// the BTS MSMQ adapter.  Loaded by the AdapterMgr class
    /// </summary>
    public class MSMQAdapterProvider : BaseAdapterProvider
    {
        public override string AdapterName
        {
            get { return "MSMQ"; }
        }

        public override void SetEndpoint(Dictionary<string, string> ResolverDictionary, IBaseMessageContext msg)
        {
            base.SetEndpoint(ResolverDictionary, msg);
        }

        public override string AdapterContextPropertyNamespace
        {
            get
            {
                // if not using AdapterMgr for adapters that are not automatically registered to BizTalk Adapter table
                //return "http://schemas.microsoft.com/BizTalk/2003/msmq-properties";
                return "MSMQ";

            }
        }

        protected override void SetEndpointContextProperties(IBaseMessageContext pipelineContext, string endpointConfig)
        {
            //base.SetEndpointContextProperties(pipelineContext, endpointConfig);

            //string[] properties = endpointConfig.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            //foreach (string property in properties)
            //{
            //    string[] data = property.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            //    string key = data[0];
            //    string value = data.Length < 2 ? "" : data[1];

            //    pipelineContext.Write(key, this.AdapterContextPropertyNamespace, value);
            //}

            if (!string.IsNullOrEmpty(endpointConfig))
            {
                AdapterMgr.SetContextProperties(pipelineContext, endpointConfig, this.AdapterName, this.AdapterContextPropertyNamespace);
            }
            else
            {
                string defaultEC = this.GetDefaultEndpointConfig();
                if (!string.IsNullOrEmpty(defaultEC))
                {
                    AdapterMgr.SetContextProperties(pipelineContext, defaultEC, this.AdapterName, this.AdapterContextPropertyNamespace);
                }
            }
        }

        public override void SetEndpoint(Dictionary<string, string> ResolverDictionary, Microsoft.XLANGs.BaseTypes.XLANGMessage message)
        {
            base.SetEndpoint(ResolverDictionary, message);
        }

        protected override void SetEndpointContextProperties(Microsoft.XLANGs.BaseTypes.XLANGMessage message, string endpointConfig)
        {
            base.SetEndpointContextProperties(message, endpointConfig);
        }

        protected override void SetContextProperties(IBaseMessageContext pipelineContext, Dictionary<string, string> ResolverDictionary)
        {
            base.SetContextProperties(pipelineContext, ResolverDictionary);
        }

        protected override void SetContextProperties(Microsoft.XLANGs.BaseTypes.XLANGMessage message, Dictionary<string, string> ResolverDictionary)
        {
            base.SetContextProperties(message, ResolverDictionary);
        }
    }
}
