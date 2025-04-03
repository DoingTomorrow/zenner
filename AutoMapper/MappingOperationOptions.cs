// Decompiled with JetBrains decompiler
// Type: AutoMapper.MappingOperationOptions
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace AutoMapper
{
  public class MappingOperationOptions : IMappingOperationOptions
  {
    public MappingOperationOptions()
    {
      this.Items = (IDictionary<string, object>) new Dictionary<string, object>();
    }

    public Func<Type, object> ServiceCtor { get; private set; }

    public bool CreateMissingTypeMaps { get; set; }

    public IDictionary<string, object> Items { get; private set; }

    void IMappingOperationOptions.ConstructServicesUsing(Func<Type, object> constructor)
    {
      this.ServiceCtor = constructor;
    }
  }
}
