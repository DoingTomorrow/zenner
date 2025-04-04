// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.HtmlElementAttributesAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = true)]
  internal sealed class HtmlElementAttributesAttribute : Attribute
  {
    public HtmlElementAttributesAttribute()
    {
    }

    public HtmlElementAttributesAttribute([NotNull] string name) => this.Name = name;

    [NotNull]
    public string Name { get; private set; }
  }
}
