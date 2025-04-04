// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutoMappingException
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace FluentNHibernate.Automapping
{
  [Serializable]
  public class AutoMappingException : Exception
  {
    public AutoMappingException(string message)
      : base(message)
    {
    }

    protected AutoMappingException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
