// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.IArrayPool`1
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json
{
  public interface IArrayPool<T>
  {
    T[] Rent(int minimumLength);

    void Return(T[] array);
  }
}
