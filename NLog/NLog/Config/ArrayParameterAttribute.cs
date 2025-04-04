// Decompiled with JetBrains decompiler
// Type: NLog.Config.ArrayParameterAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Config
{
  [AttributeUsage(AttributeTargets.Property)]
  public sealed class ArrayParameterAttribute : Attribute
  {
    public ArrayParameterAttribute(Type itemType, string elementName)
    {
      this.ItemType = itemType;
      this.ElementName = elementName;
    }

    public Type ItemType { get; private set; }

    public string ElementName { get; private set; }
  }
}
