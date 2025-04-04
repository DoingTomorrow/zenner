// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.IReferenceResolver
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public interface IReferenceResolver
  {
    object ResolveReference(object context, string reference);

    string GetReference(object context, object value);

    bool IsReferenced(object context, object value);

    void AddReference(object context, string reference, object value);
  }
}
