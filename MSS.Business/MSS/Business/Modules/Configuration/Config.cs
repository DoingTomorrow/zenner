// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Configuration.Config
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Modules.Configuration
{
  public class Config
  {
    public string PropertyName { get; set; }

    public string PropertyValue { get; set; }

    public List<ConfigurationPropertyValue> ProperListValues { get; set; }

    public string Type { get; set; }

    public bool IsReadOnly { get; set; }

    public object Parameter { get; set; }

    public string Description { get; set; }
  }
}
