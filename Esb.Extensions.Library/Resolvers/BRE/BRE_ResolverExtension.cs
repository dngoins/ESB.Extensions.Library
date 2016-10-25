namespace ESB.Extensions.Library.Resolvers.Bre 
{

    using System;
    
    #region Extender NS
    using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
    using Microsoft.Practices.Services.ItineraryDsl;
    using Microsoft.Practices.Modeling.Common;
    using Microsoft.Practices.Services.Extenders.Exporters.Common;
    using Microsoft.Practices.Modeling.Services.Clients;
    using System.ServiceModel;
    using System.ComponentModel;
    using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;

    #endregion

    
    using System.Drawing.Design;
    using Microsoft.Practices.Modeling.Services.Design;
    using Microsoft.Practices.Modeling.Common.Design;
    using System.Web.UI.Design;

   
    #region Resolver Extender Implementation

    /// <summary>
    /// Implementation of DSL Itinerary Resolver Design Extension required for use with ESB Itineraries
    /// </summary>
    [Serializable]
    [ObjectExtender(typeof(Microsoft.Practices.Services.ItineraryDsl.Resolver))]
    public class BRE_ExtendedResolver : ObjectExtender<Microsoft.Practices.Services.ItineraryDsl.Resolver>, IQueryResolverService
    {
        // An example
        //[Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory), Description("Comma separated list of action values"), DisplayName("Value"), ReadOnly(false), Browsable(true)]
        //[StringLengthValidator(1, RangeBoundaryType.Inclusive, 0, RangeBoundaryType.Ignore, MessageTemplate = "The 'Value' property should not be null or empty.")]
        //public string Value { get; set; }

        // Properties copied from existing BRE Resolver
        [DisplayName("Message File"), Editor(typeof(XmlFileEditor), typeof(UITypeEditor)), Description("Specifies the message path to send to BRE resolver. The file content will be used in the 'Test Resolver Configuration...' menu option."), Browsable(true), ReadOnly(false), TypeConverter(typeof(TypeConverter)), Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory)]
        public string MessageFile
        { get; set;}

        [EditorOutputProperty("Version", "Version", new string[] { }), Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory), Description("Specifies the policy."), DisplayName("Policy"), ReadOnly(false), Browsable(true), Editor(typeof(BiztalkPolicyEditor), typeof(UITypeEditor)), TypeConverter(typeof(TypeConverter))]
        public string Policy
        {get; set;}

        [DisplayName("Recognize Message Format"), Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory), Description("Used to recognize DocType. Note: This is a messaging only scenario and is not supported for Orchestration.i.e in case of orcehstrations the field must always be set to 'false'."), Browsable(true), ReadOnly(false)]
        public bool RecognizeMessageFormat
        {
            get; set;
        }

        [Browsable(true), Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory), Description("Specifies whether to pass the message as a fact into the BRE policy."), DisplayName("UseMsg"), ReadOnly(false)]
        public bool UseMsg
        {
            get; set;
        }

        [ReadOnly(false), Description("Specifies the policy version. If left empty, the latest deployed policy will be used."), DisplayName("Version"), Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory), Browsable(true)]
        public VersionNumber Version
        {
            get; set;
        }

        #region "Extended section"

        [Browsable(true), Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory), Description("Specifies whether to pass the message context as a fact into the BRE policy. This is a messaging only scenario and is not supported for Orchestration.i.e in case of orcehstrations the field must always be set to 'false'."), DisplayName("Use MsgCtxt"), ReadOnly(false)]
        public bool UseMsgCtxt
        {
            get;
            set;
        }

        [DisplayName("Message Context Values"), Description("Specifies a case sensitive Pipe Delimited list of  Fully qualified BizTalk Context property names (NS#Name). An empty value, in conjunction with the Use MstCtxt property will enumerate all context properties. i.e. http://schemas.microsoft.com/BizTalk/2003/system-properties#MessageType"), Browsable(true), ReadOnly(false),  Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory)]
        public string MsgCtxtValues
        { get; set; }

        [Browsable(true), Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory), Description("Specifies whether to enable the Business Rule default debug tracking interceptor."), DisplayName("Use Debugging Interceptor"), ReadOnly(false)]
        public bool UseDebugInterceptor
        {
            get;
            set;
        }

        [DisplayName("Debug Output File"), Editor(typeof(XmlFileEditor), typeof(UITypeEditor)), Description("Specifies the file path to send to  Debug tracking interceptor events when the Debugging tracking interceptor is enabled."), Browsable(true), ReadOnly(false), TypeConverter(typeof(TypeConverter)), Category(BREResolverExtensionProvider.ExtensionProviderPropertyCategory)]
        public string DebugOutputPath
        {
            get; 
            set;
        }

        #endregion

        #region IQueryResolverService Members

        public object Resolve(string serviceAddress)
        {
            string config = ItineraryModelExporter.GetResolverConfiguration(this.ModelElement);
            return ResolverServiceClientHelper.Resolve(new EndpointAddress(serviceAddress), config);

        }

        #endregion
    }

    [ExtensionProvider("9015A6F4-DA1E-4D26-9143-68C86B2E5FA5", "BRE.EXT", "JMFE BRE Extension Resolver Extension", typeof(ItineraryDslDomainModel))]
    [ResolverExtensionProvider]
    public class BREResolverExtensionProvider : ExtensionProviderBase
    {
        public BREResolverExtensionProvider() : base(typeof(BRE_ExtendedResolver))         { }
        }

    #endregion

}