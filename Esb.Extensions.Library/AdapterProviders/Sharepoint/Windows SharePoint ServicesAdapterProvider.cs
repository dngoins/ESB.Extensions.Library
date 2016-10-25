using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.ESB.Adapter;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.Practices.ESB.Exception.Management;


namespace ESB.Extensions.Library.AdapterProviders.SharePoint
{
    /// <summary>
    /// Ths class implements IAdapterProvider for setting the context properties of
    /// the BTS MSMQ adapter.  Loaded by the AdapterMgr class
    /// </summary>
    public class WSSAdapterProvider : BaseAdapterProvider
    {
        public override string AdapterName
        {
            get { return "Windows SharePoint Services"; }
        }
        
        public override string AdapterContextPropertyNamespace
        {
            get
            {
                // If not using the AdapterMgr
               //return "http://schemas.microsoft.com/BizTalk/2006/WindowsSharePointServices-properties";
                //when using the AdapterMgr because it appends the namespace of the WSS Schema base class
                return "WSS";
            }
        }

        protected override void SetEndpointContextProperties(IBaseMessageContext pipelineContext, string endpointConfig)
        {
            // If not using Adapter Mgr
           // base.SetEndpointContextProperties(pipelineContext, endpointConfig);

            //string[] properties = endpointConfig.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            //foreach (string property in properties)
            //{
            //    string[] data = property.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            //    string key = data[0];
            //    string value = data.Length < 2 ? "" : data[1];

            //    pipelineContext.Write(key, this.AdapterContextPropertyNamespace, value);
            //}

            try
            {
                //Using the Adapter Mgr
                if (!string.IsNullOrEmpty(endpointConfig))
                {
                    AdapterMgr.SetContextProperties(pipelineContext, endpointConfig, this.AdapterName,
                                                    this.AdapterContextPropertyNamespace);
                }
                else
                {
                    string defaultEC = this.GetDefaultEndpointConfig();
                    if (!string.IsNullOrEmpty(defaultEC))
                    {
                        AdapterMgr.SetContextProperties(pipelineContext, defaultEC, this.AdapterName,
                                                        this.AdapterContextPropertyNamespace);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.Write("Windows SharePoint Services::SetEndpointContextProperties threw an exception\n{0}",
                                  ex.ToString());

            }
        }
      
    }
}
