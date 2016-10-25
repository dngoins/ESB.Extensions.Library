namespace ESB.Extensions.Schemas
{
    using Microsoft.XLANGs.BaseTypes;


    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Property)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] { @"BatchSize", @"IsSolicitResponse", @"IsTwoWay", @"Name", @"Uri" })]
    public sealed class psActiveDirectory : Microsoft.XLANGs.BaseTypes.SchemaBase
    {

        [System.NonSerializedAttribute()]
        private static object _rawSchema;

        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://ESB.Extensions.Schemas.psActiveDirectory"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""https://ESB.Extensions.Schemas.psActiveDirectory"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:schemaInfo schema_type=""property"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" />
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""BatchSize"" type=""xs:int"">
    <xs:annotation>
      <xs:appinfo>
        <b:fieldInfo propertyGuid=""6df1d16f-9f02-49f6-bd6d-a8ab59b44746"" rootTypeName=""BatchSizeType"" />
      </xs:appinfo>
    </xs:annotation>
  </xs:element>
  <xs:element name=""IsSolicitResponse"" type=""xs:boolean"">
    <xs:annotation>
      <xs:appinfo>
        <b:fieldInfo propertyGuid=""2af9af56-d67b-495c-b002-915da0d464a0"" rootTypeName=""IsSolicitResponseType"" />
      </xs:appinfo>
    </xs:annotation>
  </xs:element>
  <xs:element name=""IsTwoWay"" type=""xs:boolean"">
    <xs:annotation>
      <xs:appinfo>
        <b:fieldInfo propertyGuid=""42098b2a-78c8-4c52-a455-0561efc929c4"" rootTypeName=""IsTwoWayType"" />
      </xs:appinfo>
    </xs:annotation>
  </xs:element>
  <xs:element name=""Name"" type=""xs:string"">
    <xs:annotation>
      <xs:appinfo>
        <b:fieldInfo propertyGuid=""ed22a543-1f70-4e3d-a979-d3c03e215132"" rootTypeName=""NameType"" />
      </xs:appinfo>
    </xs:annotation>
  </xs:element>
  <xs:element name=""Uri"" type=""xs:string"">
    <xs:annotation>
      <xs:appinfo>
        <b:fieldInfo propertyGuid=""0bb897cd-cc74-4979-ac59-3986957901c1"" rootTypeName=""UriType"" />
      </xs:appinfo>
    </xs:annotation>
  </xs:element>
</xs:schema>";

        public psActiveDirectory()
        {
        }

        public override string XmlContent
        {
            get
            {
                return _strSchema;
            }
        }

        public override string[] RootNodes
        {
            get
            {
                string[] _RootElements = new string[5];
                _RootElements[0] = "BatchSize";
                _RootElements[1] = "IsSolicitResponse";
                _RootElements[2] = "IsTwoWay";
                _RootElements[3] = "Name";
                _RootElements[4] = "Uri";
                return _RootElements;
            }
        }

        protected override object RawSchema
        {
            get
            {
                return _rawSchema;
            }
            set
            {
                _rawSchema = value;
            }
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [System.SerializableAttribute()]
    [PropertyType(@"BatchSize", @"https://ESB.Extensions.Schemas.psActiveDirectory", "int", "System.Int32")]
    [PropertyGuidAttribute(@"6df1d16f-9f02-49f6-bd6d-a8ab59b44746")]
    public sealed class BatchSizeType : Microsoft.XLANGs.BaseTypes.MessageDataPropertyBase
    {

        [System.NonSerializedAttribute()]
        private static System.Xml.XmlQualifiedName _QName = new System.Xml.XmlQualifiedName(@"BatchSize", @"https://ESB.Extensions.Schemas.psActiveDirectory");

        private static int PropertyValueType
        {
            get
            {
                throw new System.NotSupportedException();
            }
        }

        public override System.Xml.XmlQualifiedName Name
        {
            get
            {
                return _QName;
            }
        }

        public override System.Type Type
        {
            get
            {
                return typeof(int);
            }
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [System.SerializableAttribute()]
    [PropertyType(@"IsSolicitResponse", @"https://ESB.Extensions.Schemas.psActiveDirectory", "boolean", "System.Boolean")]
    [PropertyGuidAttribute(@"2af9af56-d67b-495c-b002-915da0d464a0")]
    public sealed class IsSolicitResponseType : Microsoft.XLANGs.BaseTypes.MessageDataPropertyBase
    {

        [System.NonSerializedAttribute()]
        private static System.Xml.XmlQualifiedName _QName = new System.Xml.XmlQualifiedName(@"IsSolicitResponse", @"https://ESB.Extensions.Schemas.psActiveDirectory");

        private static bool PropertyValueType
        {
            get
            {
                throw new System.NotSupportedException();
            }
        }

        public override System.Xml.XmlQualifiedName Name
        {
            get
            {
                return _QName;
            }
        }

        public override System.Type Type
        {
            get
            {
                return typeof(bool);
            }
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [System.SerializableAttribute()]
    [PropertyType(@"IsTwoWay", @"https://ESB.Extensions.Schemas.psActiveDirectory", "boolean", "System.Boolean")]
    [PropertyGuidAttribute(@"42098b2a-78c8-4c52-a455-0561efc929c4")]
    public sealed class IsTwoWayType : Microsoft.XLANGs.BaseTypes.MessageDataPropertyBase
    {

        [System.NonSerializedAttribute()]
        private static System.Xml.XmlQualifiedName _QName = new System.Xml.XmlQualifiedName(@"IsTwoWay", @"https://ESB.Extensions.Schemas.psActiveDirectory");

        private static bool PropertyValueType
        {
            get
            {
                throw new System.NotSupportedException();
            }
        }

        public override System.Xml.XmlQualifiedName Name
        {
            get
            {
                return _QName;
            }
        }

        public override System.Type Type
        {
            get
            {
                return typeof(bool);
            }
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [System.SerializableAttribute()]
    [PropertyType(@"Name", @"https://ESB.Extensions.Schemas.psActiveDirectory", "string", "System.String")]
    [PropertyGuidAttribute(@"ed22a543-1f70-4e3d-a979-d3c03e215132")]
    public sealed class NameType : Microsoft.XLANGs.BaseTypes.MessageDataPropertyBase
    {

        [System.NonSerializedAttribute()]
        private static System.Xml.XmlQualifiedName _QName = new System.Xml.XmlQualifiedName(@"Name", @"https://ESB.Extensions.Schemas.psActiveDirectory");

        private static string PropertyValueType
        {
            get
            {
                throw new System.NotSupportedException();
            }
        }

        public override System.Xml.XmlQualifiedName Name
        {
            get
            {
                return _QName;
            }
        }

        public override System.Type Type
        {
            get
            {
                return typeof(string);
            }
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [System.SerializableAttribute()]
    [PropertyType(@"Uri", @"https://ESB.Extensions.Schemas.psActiveDirectory", "string", "System.String")]
    [PropertyGuidAttribute(@"0bb897cd-cc74-4979-ac59-3986957901c1")]
    public sealed class UriType : Microsoft.XLANGs.BaseTypes.MessageDataPropertyBase
    {

        [System.NonSerializedAttribute()]
        private static System.Xml.XmlQualifiedName _QName = new System.Xml.XmlQualifiedName(@"Uri", @"https://ESB.Extensions.Schemas.psActiveDirectory");

        private static string PropertyValueType
        {
            get
            {
                throw new System.NotSupportedException();
            }
        }

        public override System.Xml.XmlQualifiedName Name
        {
            get
            {
                return _QName;
            }
        }

        public override System.Type Type
        {
            get
            {
                return typeof(string);
            }
        }
    }
}
