// Decompiled with JetBrains decompiler
// Type: NLog.Targets.JsonConverterLegacy
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System.Text;

#nullable disable
namespace NLog.Targets
{
  internal class JsonConverterLegacy : IJsonConverter, IJsonSerializer
  {
    private readonly IJsonSerializer _jsonSerializer;

    public JsonConverterLegacy(IJsonSerializer jsonSerializer)
    {
      this._jsonSerializer = jsonSerializer;
    }

    public bool SerializeObject(object value, StringBuilder builder)
    {
      string str = this._jsonSerializer.SerializeObject(value);
      if (str == null)
        return false;
      builder.Append(str);
      return true;
    }

    string IJsonSerializer.SerializeObject(object value)
    {
      return this._jsonSerializer.SerializeObject(value);
    }
  }
}
