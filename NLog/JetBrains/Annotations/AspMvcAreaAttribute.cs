// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.AspMvcAreaAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Parameter)]
  internal sealed class AspMvcAreaAttribute : PathReferenceAttribute
  {
    public AspMvcAreaAttribute()
    {
    }

    public AspMvcAreaAttribute([NotNull] string anonymousProperty)
    {
      this.AnonymousProperty = anonymousProperty;
    }

    [NotNull]
    public string AnonymousProperty { get; private set; }
  }
}
