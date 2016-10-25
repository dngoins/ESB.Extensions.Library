using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ACTIVEDIRECTORY
{

    // Type: ACTIVEDIRECTORY.xxType
    // Assembly: EsbExtensionsLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
    
    /*
     * 	<property name="BatchSize" type="ACTIVEADAPTER.BatchSizeType" assembly="activeAdapterPropertySchemas" />
		<property name="IsSolicitResponseProp" type="ACTIVEADAPTER.IsSolicitResponsePropType" assembly="activeAdapterPropertySchemas" />
    <property name="IsTwoWay" type="ACTIVEADAPTER.IsTwoWayType" assembly="activeAdapterPropertySchemas" />
    <property name="Uri" type="ACTIVEADAPTER.UriType" assembly="activeAdapterPropertySchemas" />
    <property name="Name" type="ACTIVEADAPTER.NameType" assembly="activeAdapterPropertySchemas" />
     * */

    using Microsoft.XLANGs.BaseTypes;
    using System;
    using System.Runtime.InteropServices;
    using System.Xml;


    [PropertyType("IsSolicitResponseType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties", "bool", "System.Boolean")]
    [IsSensitiveProperty(false)]
    [PropertyGuid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
    [Guid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
    [Serializable]
    public sealed class IsSolicitResponseType : MessageContextPropertyBase
    {
        private static XmlQualifiedName _QName = new XmlQualifiedName("IsSolicitResponseType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties");

        private static int PropertyValueType
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override XmlQualifiedName Name
        {
            get
            {
                return IsSolicitResponseType._QName;
            }
        }

        public override Type Type
        {
            get
            {
                return typeof(int);
            }
        }

        static IsSolicitResponseType ()
        {
        }
    }

    
        [PropertyType("BatchSizeType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties", "int", "System.Int32")]
        [IsSensitiveProperty(false)]
        [PropertyGuid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
        [Guid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
        [Serializable]
        public sealed class BatchSizeType : MessageContextPropertyBase
        {
            private static XmlQualifiedName _QName = new XmlQualifiedName("BatchSizeType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties");

            private static int PropertyValueType
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            public override XmlQualifiedName Name
            {
                get
                {
                    return BatchSizeType._QName;
                }
            }

            public override Type Type
            {
                get
                {
                    return typeof(int);
                }
            }

            static BatchSizeType()
            {
            }
        }


        [PropertyType("IsTwoWayType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties", "bool", "System.Boolean")]
        [IsSensitiveProperty(false)]
        [PropertyGuid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
        [Guid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
        [Serializable]
        public sealed class IsTwoWayType : MessageContextPropertyBase
        {
            private static XmlQualifiedName _QName = new XmlQualifiedName("IsTwoWayType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties");

            private static int PropertyValueType
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            public override XmlQualifiedName Name
            {
                get
                {
                    return IsTwoWayType._QName;
                }
            }

            public override Type Type
            {
                get
                {
                    return typeof(int);
                }
            }

            static IsTwoWayType()
            {
            }
        }


        [PropertyType("NameType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties", "string", "System.String")]
        [IsSensitiveProperty(false)]
        [PropertyGuid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
        [Guid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
        [Serializable]
        public sealed class NameType : MessageContextPropertyBase
        {
            private static XmlQualifiedName _QName = new XmlQualifiedName("NameType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties");

            private static int PropertyValueType
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            public override XmlQualifiedName Name
            {
                get
                {
                    return NameType._QName;
                }
            }

            public override Type Type
            {
                get
                {
                    return typeof(int);
                }
            }

            static NameType()
            {
            }
        }


        [PropertyType("UriType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties", "string", "System.String")]
        [IsSensitiveProperty(false)]
        [PropertyGuid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
        [Guid("5b4a7e97-1733-4c6e-89e1-9d0c9c2acb03")]
        [Serializable]
        public sealed class UriType : MessageContextPropertyBase
        {
            private static XmlQualifiedName _QName = new XmlQualifiedName("UriType", "http://schemas.whitecase.com/BizTalk/2013/activedirectory-properties");

            private static int PropertyValueType
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            public override XmlQualifiedName Name
            {
                get
                {
                    return UriType._QName;
                }
            }

            public override Type Type
            {
                get
                {
                    return typeof(int);
                }
            }

            static UriType()
            {
            }
        }




}

