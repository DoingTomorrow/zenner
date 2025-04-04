// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.LayeredValues
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace FluentNHibernate.MappingModel.Collections
{
  [Serializable]
  public class LayeredValues : Dictionary<int, object>
  {
    public LayeredValues()
    {
    }

    protected LayeredValues(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
