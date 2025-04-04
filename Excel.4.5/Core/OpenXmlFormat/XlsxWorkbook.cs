// Decompiled with JetBrains decompiler
// Type: Excel.Core.OpenXmlFormat.XlsxWorkbook
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Excel.Core.OpenXmlFormat
{
  internal class XlsxWorkbook
  {
    private const string N_sheet = "sheet";
    private const string N_t = "t";
    private const string N_si = "si";
    private const string N_cellXfs = "cellXfs";
    private const string N_numFmts = "numFmts";
    private const string A_sheetId = "sheetId";
    private const string A_name = "name";
    private const string A_rid = "r:id";
    private const string N_rel = "Relationship";
    private const string A_id = "Id";
    private const string A_target = "Target";
    private List<XlsxWorksheet> sheets;
    private XlsxSST _SST;
    private XlsxStyles _Styles;

    private XlsxWorkbook()
    {
    }

    public XlsxWorkbook(
      Stream workbookStream,
      Stream relsStream,
      Stream sharedStringsStream,
      Stream stylesStream)
    {
      if (workbookStream == null)
        throw new ArgumentNullException();
      this.ReadWorkbook(workbookStream);
      this.ReadWorkbookRels(relsStream);
      this.ReadSharedStrings(sharedStringsStream);
      this.ReadStyles(stylesStream);
    }

    public List<XlsxWorksheet> Sheets
    {
      get => this.sheets;
      set => this.sheets = value;
    }

    public XlsxSST SST => this._SST;

    public XlsxStyles Styles => this._Styles;

    private void ReadStyles(Stream xmlFileStream)
    {
      if (xmlFileStream == null)
        return;
      this._Styles = new XlsxStyles();
      bool flag = false;
      using (XmlReader xmlReader = XmlReader.Create(xmlFileStream))
      {
        while (xmlReader.Read())
        {
          if (!flag && xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "numFmts")
          {
            while (xmlReader.Read() && (xmlReader.NodeType != XmlNodeType.Element || xmlReader.Depth != 1))
            {
              if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "numFmt")
                this._Styles.NumFmts.Add(new XlsxNumFmt(int.Parse(xmlReader.GetAttribute("numFmtId")), xmlReader.GetAttribute("formatCode")));
            }
            flag = true;
          }
          if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "cellXfs")
          {
            while (xmlReader.Read() && (xmlReader.NodeType != XmlNodeType.Element || xmlReader.Depth != 1))
            {
              if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "xf")
              {
                string attribute1 = xmlReader.GetAttribute("xfId");
                string attribute2 = xmlReader.GetAttribute("numFmtId");
                this._Styles.CellXfs.Add(new XlsxXf(attribute1 == null ? -1 : int.Parse(attribute1), attribute2 == null ? -1 : int.Parse(attribute2), xmlReader.GetAttribute("applyNumberFormat")));
              }
            }
            break;
          }
        }
        xmlFileStream.Close();
      }
    }

    private void ReadSharedStrings(Stream xmlFileStream)
    {
      if (xmlFileStream == null)
        return;
      this._SST = new XlsxSST();
      using (XmlReader xmlReader = XmlReader.Create(xmlFileStream))
      {
        bool flag = false;
        string str = "";
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "si")
          {
            if (flag)
              this._SST.Add(str);
            else
              flag = true;
            str = "";
          }
          if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "t")
            str += xmlReader.ReadElementContentAsString();
        }
        if (flag)
          this._SST.Add(str);
        xmlFileStream.Close();
      }
    }

    private void ReadWorkbook(Stream xmlFileStream)
    {
      this.sheets = new List<XlsxWorksheet>();
      using (XmlReader xmlReader = XmlReader.Create(xmlFileStream))
      {
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "sheet")
            this.sheets.Add(new XlsxWorksheet(xmlReader.GetAttribute("name"), int.Parse(xmlReader.GetAttribute("sheetId")), xmlReader.GetAttribute("r:id")));
        }
        xmlFileStream.Close();
      }
    }

    private void ReadWorkbookRels(Stream xmlFileStream)
    {
      using (XmlReader xmlReader = XmlReader.Create(xmlFileStream))
      {
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "Relationship")
          {
            string attribute = xmlReader.GetAttribute("Id");
            for (int index = 0; index < this.sheets.Count; ++index)
            {
              XlsxWorksheet sheet = this.sheets[index];
              if (sheet.RID == attribute)
              {
                sheet.Path = xmlReader.GetAttribute("Target");
                this.sheets[index] = sheet;
                break;
              }
            }
          }
        }
        xmlFileStream.Close();
      }
    }
  }
}
