// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.ExcelOpenDocument.ExcelOdWorkbook
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.IO;
using System.Linq;

#nullable disable
namespace ZENNER.CommonLibrary.ExcelOpenDocument
{
  public class ExcelOdWorkbook
  {
    private SpreadsheetDocument TheDocument;

    public void Open(string filePath)
    {
      this.TheDocument = this.TheDocument == null ? SpreadsheetDocument.Open(filePath, true) : throw new Exception("An open document exists");
    }

    public void New(string filePath, string sheetName)
    {
      if (this.TheDocument != null)
        throw new Exception("An open document exists");
      if (File.Exists(filePath))
        File.Delete(filePath);
      this.TheDocument = SpreadsheetDocument.Create(filePath, (SpreadsheetDocumentType) 0);
      WorkbookPart workbookPart = this.TheDocument.AddWorkbookPart();
      workbookPart.Workbook = new Workbook();
      WorksheetPart worksheetPart = ((OpenXmlPartContainer) workbookPart).AddNewPart<WorksheetPart>();
      SheetData sheetData = new SheetData();
      worksheetPart.Worksheet = new Worksheet(new OpenXmlElement[1]
      {
        (OpenXmlElement) sheetData
      });
      ((OpenXmlElement) ((OpenXmlCompositeElement) this.TheDocument.WorkbookPart.Workbook).AppendChild<Sheets>(new Sheets())).Append(new OpenXmlElement[1]
      {
        (OpenXmlElement) new Sheet()
        {
          Id = StringValue.op_Implicit(((OpenXmlPartContainer) this.TheDocument.WorkbookPart).GetIdOfPart((OpenXmlPart) worksheetPart)),
          SheetId = UInt32Value.op_Implicit(1U),
          Name = StringValue.op_Implicit(sheetName)
        }
      });
      ((OpenXmlPartRootElement) workbookPart.Workbook).Save();
    }

    public void Close()
    {
      if (this.TheDocument == null)
        throw new Exception("No document for closing");
      ((OpenXmlPackage) this.TheDocument).Close();
      this.TheDocument = (SpreadsheetDocument) null;
    }

    public ExcelOdSheet GetSheet(string sheetName)
    {
      if (this.TheDocument == null)
        throw new Exception("Not a open document available");
      return new ExcelOdSheet((WorksheetPart) ((OpenXmlPartContainer) this.TheDocument.WorkbookPart).GetPartById(StringValue.op_Implicit((((OpenXmlElement) ((OpenXmlElement) this.TheDocument.WorkbookPart.Workbook).GetFirstChild<Sheets>()).Elements<Sheet>().FirstOrDefault<Sheet>((Func<Sheet, bool>) (s => StringValue.op_Implicit(s.Name) == sheetName)) ?? throw new Exception("Sheet " + sheetName + " not found")).Id)), this.TheDocument);
    }
  }
}
