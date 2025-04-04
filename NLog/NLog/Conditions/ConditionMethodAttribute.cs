// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionMethodAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;

#nullable disable
namespace NLog.Conditions
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public sealed class ConditionMethodAttribute(string name) : NameBaseAttribute(name)
  {
  }
}
