// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.VBA.ExcelVBAModule
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.VBA
{
  public class ExcelVBAModule
  {
    private string _name = "";
    private ModuleNameChange _nameChangeCallback;
    private string _code = "";

    internal ExcelVBAModule() => this.Attributes = new ExcelVbaModuleAttributesCollection();

    internal ExcelVBAModule(ModuleNameChange nameChangeCallback)
      : this()
    {
      this._nameChangeCallback = nameChangeCallback;
    }

    public string Name
    {
      get => this._name;
      set
      {
        if (value.Any<char>((Func<char, bool>) (c => c > 'ÿ')))
          throw new InvalidOperationException("Vba module names can't contain unicode characters");
        if (!(value != this._name))
          return;
        this._name = value;
        this.streamName = value;
        if (this._nameChangeCallback == null)
          return;
        this._nameChangeCallback(value);
      }
    }

    public string Description { get; set; }

    public string Code
    {
      get => this._code;
      set
      {
        this._code = !value.StartsWith("Attribute", StringComparison.InvariantCultureIgnoreCase) && !value.StartsWith("VERSION", StringComparison.InvariantCultureIgnoreCase) ? value : throw new InvalidOperationException("Code can't start with an Attribute or VERSION keyword. Attributes can be accessed through the Attributes collection.");
      }
    }

    public int HelpContext { get; set; }

    public ExcelVbaModuleAttributesCollection Attributes { get; internal set; }

    public eModuleType Type { get; internal set; }

    public bool ReadOnly { get; set; }

    public bool Private { get; set; }

    internal string streamName { get; set; }

    internal ushort Cookie { get; set; }

    internal uint ModuleOffset { get; set; }

    internal string ClassID { get; set; }

    public override string ToString() => this.Name;
  }
}
