// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.CacheMappingsAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Castle.DynamicProxy
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
  [CLSCompliant(false)]
  public class CacheMappingsAttribute : Attribute
  {
    private static readonly ConstructorInfo constructor = typeof (CacheMappingsAttribute).GetConstructor(new Type[1]
    {
      typeof (byte[])
    });
    private readonly byte[] _serializedCacheMappings;

    public static void ApplyTo(
      AssemblyBuilder assemblyBuilder,
      Dictionary<CacheKey, string> mappings)
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, (object) mappings);
        byte[] array = serializationStream.ToArray();
        CustomAttributeBuilder customBuilder = new CustomAttributeBuilder(CacheMappingsAttribute.constructor, new object[1]
        {
          (object) array
        });
        assemblyBuilder.SetCustomAttribute(customBuilder);
      }
    }

    public CacheMappingsAttribute(byte[] serializedCacheMappings)
    {
      this._serializedCacheMappings = serializedCacheMappings;
    }

    public byte[] SerializedCacheMappings => this._serializedCacheMappings;

    public Dictionary<CacheKey, string> GetDeserializedMappings()
    {
      using (MemoryStream serializationStream = new MemoryStream(this.SerializedCacheMappings))
        return (Dictionary<CacheKey, string>) new BinaryFormatter().Deserialize((Stream) serializationStream);
    }
  }
}
