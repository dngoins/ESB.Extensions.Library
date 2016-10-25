using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTS;
using Microsoft.Practices.ESB.Adapter;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.Practices.ESB.Common;
using Microsoft.Practices.ESB.GlobalPropertyContext;


namespace ESB.Extensions.Library.AdapterProviders.Http
{
    /// <summary>
    /// Ths class implements IAdapterProvider for setting the context properties of
    /// the BTS HTTP adapter.  Loaded by the AdapterMgr class
    /// </summary>
    public class HTTPAdapterProvider : BaseAdapterProvider
    {
        public override string AdapterName
        {
            get { return "HTTP"; }
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
                return "HTTP";

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
            // Need to use http_legacy because ESB is looking for hard coded "http" for WCF Basic
            string str = ResolverDictionary["Resolver.TransportLocation"];
            // String should be "http_legacy://something/something"
            string correctedLocation = str.Replace("http_legacy", "http");

            pipelineContext.Write(BtsProperties.OutboundTransportLocation.Name, BtsProperties.OutboundTransportLocation.Namespace, correctedLocation );
            pipelineContext.Write(BtsProperties.OutboundTransportType.Name, BtsProperties.OutboundTransportType.Namespace, this.AdapterName);

            
        }

        protected override void SetContextProperties(Microsoft.XLANGs.BaseTypes.XLANGMessage message, Dictionary<string, string> ResolverDictionary)
        {
            string str = ResolverDictionary["Resolver.TransportLocation"];
            string correctedLocation = str.Replace("http_legacy", "http");

            string str2 = ResolverDictionary["Resolver.OutboundTransportCLSID"];
            message.SetMsgProperty<OutboundTransportLocation, string>(correctedLocation );
            message.SetMsgProperty<OutboundTransportType, string>(this.AdapterName);
            message.SetMsgProperty<OutboundTransportCLSID, string>(str2);

        }
    }
}
