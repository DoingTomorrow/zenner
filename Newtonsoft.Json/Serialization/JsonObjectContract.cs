// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonObjectContract
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public class JsonObjectContract : JsonContainerContract
  {
    internal bool ExtensionDataIsJToken;
    private bool? _hasRequiredOrDefaultValueProperties;
    private ConstructorInfo _parametrizedConstructor;
    private ConstructorInfo _overrideConstructor;
    private ObjectConstructor<object> _overrideCreator;
    private ObjectConstructor<object> _parameterizedCreator;
    private JsonPropertyCollection _creatorParameters;
    private Type _extensionDataValueType;

    public MemberSerialization MemberSerialization { get; set; }

    public Required? ItemRequired { get; set; }

    public JsonPropertyCollection Properties { get; private set; }

    [Obsolete("ConstructorParameters is obsolete. Use CreatorParameters instead.")]
    public JsonPropertyCollection ConstructorParameters => this.CreatorParameters;

    public JsonPropertyCollection CreatorParameters
    {
      get
      {
        if (this._creatorParameters == null)
          this._creatorParameters = new JsonPropertyCollection(this.UnderlyingType);
        return this._creatorParameters;
      }
    }

    [Obsolete("OverrideConstructor is obsolete. Use OverrideCreator instead.")]
    public ConstructorInfo OverrideConstructor
    {
      get => this._overrideConstructor;
      set
      {
        this._overrideConstructor = value;
        this._overrideCreator = value != (ConstructorInfo) null ? JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) value) : (ObjectConstructor<object>) null;
      }
    }

    [Obsolete("ParametrizedConstructor is obsolete. Use OverrideCreator instead.")]
    public ConstructorInfo ParametrizedConstructor
    {
      get => this._parametrizedConstructor;
      set
      {
        this._parametrizedConstructor = value;
        this._parameterizedCreator = value != (ConstructorInfo) null ? JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) value) : (ObjectConstructor<object>) null;
      }
    }

    public ObjectConstructor<object> OverrideCreator
    {
      get => this._overrideCreator;
      set
      {
        this._overrideCreator = value;
        this._overrideConstructor = (ConstructorInfo) null;
      }
    }

    internal ObjectConstructor<object> ParameterizedCreator => this._parameterizedCreator;

    public ExtensionDataSetter ExtensionDataSetter { get; set; }

    public ExtensionDataGetter ExtensionDataGetter { get; set; }

    public Type ExtensionDataValueType
    {
      get => this._extensionDataValueType;
      set
      {
        this._extensionDataValueType = value;
        this.ExtensionDataIsJToken = value != (Type) null && typeof (JToken).IsAssignableFrom(value);
      }
    }

    internal bool HasRequiredOrDefaultValueProperties
    {
      get
      {
        if (!this._hasRequiredOrDefaultValueProperties.HasValue)
        {
          this._hasRequiredOrDefaultValueProperties = new bool?(false);
          if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
          {
            this._hasRequiredOrDefaultValueProperties = new bool?(true);
          }
          else
          {
            foreach (JsonProperty property in (Collection<JsonProperty>) this.Properties)
            {
              if (property.Required == Required.Default)
              {
                DefaultValueHandling? defaultValueHandling1 = property.DefaultValueHandling;
                DefaultValueHandling? nullable = defaultValueHandling1.HasValue ? new DefaultValueHandling?(defaultValueHandling1.GetValueOrDefault() & DefaultValueHandling.Populate) : new DefaultValueHandling?();
                DefaultValueHandling defaultValueHandling2 = DefaultValueHandling.Populate;
                if ((nullable.GetValueOrDefault() == defaultValueHandling2 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                  continue;
              }
              this._hasRequiredOrDefaultValueProperties = new bool?(true);
              break;
            }
          }
        }
        return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
      }
    }

    public JsonObjectContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Object;
      this.Properties = new JsonPropertyCollection(this.UnderlyingType);
    }

    [SecuritySafeCritical]
    internal object GetUninitializedObject()
    {
      if (!JsonTypeReflector.FullyTrusted)
        throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.NonNullableUnderlyingType));
      return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
    }
  }
}
