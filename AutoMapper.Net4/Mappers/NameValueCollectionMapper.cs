// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.NameValueCollectionMapper
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using System.Collections.Specialized;

#nullable disable
namespace AutoMapper.Mappers
{
  public class NameValueCollectionMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      if (!this.IsMatch(context) || context.SourceValue == null)
        return (object) null;
      NameValueCollection nameValueCollection = new NameValueCollection();
      NameValueCollection sourceValue = context.SourceValue as NameValueCollection;
      foreach (string allKey in sourceValue.AllKeys)
        nameValueCollection.Add(allKey, sourceValue[allKey]);
      return (object) nameValueCollection;
    }

    public bool IsMatch(ResolutionContext context)
    {
      return context.SourceType == typeof (NameValueCollection) && context.DestinationType == typeof (NameValueCollection);
    }
  }
}
