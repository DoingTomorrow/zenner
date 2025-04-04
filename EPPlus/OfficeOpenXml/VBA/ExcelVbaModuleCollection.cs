// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.VBA.ExcelVbaModuleCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.VBA
{
  public class ExcelVbaModuleCollection : ExcelVBACollectionBase<ExcelVBAModule>
  {
    private ExcelVbaProject _project;

    internal ExcelVbaModuleCollection(ExcelVbaProject project) => this._project = project;

    internal void Add(ExcelVBAModule Item) => this._list.Add(Item);

    public ExcelVBAModule AddModule(string Name)
    {
      if (this[Name] != null)
        throw new ArgumentException("Vba modulename already exist.");
      ExcelVBAModule excelVbaModule = new ExcelVBAModule();
      excelVbaModule.Name = Name;
      excelVbaModule.Type = eModuleType.Module;
      excelVbaModule.Attributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Name",
        Value = Name,
        DataType = eAttributeDataType.String
      });
      excelVbaModule.Type = eModuleType.Module;
      this._list.Add(excelVbaModule);
      return excelVbaModule;
    }

    public ExcelVBAModule AddClass(string Name, bool Exposed)
    {
      ExcelVBAModule excelVbaModule = new ExcelVBAModule();
      excelVbaModule.Name = Name;
      excelVbaModule.Type = eModuleType.Class;
      excelVbaModule.Attributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Name",
        Value = Name,
        DataType = eAttributeDataType.String
      });
      excelVbaModule.Attributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Base",
        Value = "0{FCFB3D2A-A0FA-1068-A738-08002B3371B5}",
        DataType = eAttributeDataType.String
      });
      excelVbaModule.Attributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_GlobalNameSpace",
        Value = "False",
        DataType = eAttributeDataType.NonString
      });
      excelVbaModule.Attributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Creatable",
        Value = "False",
        DataType = eAttributeDataType.NonString
      });
      excelVbaModule.Attributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_PredeclaredId",
        Value = "False",
        DataType = eAttributeDataType.NonString
      });
      excelVbaModule.Attributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Exposed",
        Value = Exposed ? "True" : "False",
        DataType = eAttributeDataType.NonString
      });
      excelVbaModule.Attributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_TemplateDerived",
        Value = "False",
        DataType = eAttributeDataType.NonString
      });
      excelVbaModule.Attributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Customizable",
        Value = "False",
        DataType = eAttributeDataType.NonString
      });
      excelVbaModule.Private = !Exposed;
      this._list.Add(excelVbaModule);
      return excelVbaModule;
    }
  }
}
