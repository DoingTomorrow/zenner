// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ArrayHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.Internal
{
  internal static class ArrayHelper
  {
    internal static T[] Empty<T>() => ArrayHelper.EmptyArray<T>.Instance;

    private static class EmptyArray<T>
    {
      internal static readonly T[] Instance = new T[0];
    }
  }
}
