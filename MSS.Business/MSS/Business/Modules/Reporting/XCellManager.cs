// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Reporting.XCellManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Excel;
using MSS.Business.Modules.Archiving;
using MSS.Business.Utils;
using MSS.DTO.Minomat;
using MSSArchive.Core.Model.Meters;
using NHibernate;
using NHibernate.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Business.Modules.Reporting
{
  public class XCellManager
  {
    public void WriteToFile(string filename, List<string[]> nodeList)
    {
      ExcelPackage excelPackage = new ExcelPackage(new FileInfo(filename));
      excelPackage.Workbook.Properties.Author = "Minol™";
      ExcelWorksheet excelWorksheet = ((IEnumerable<ExcelWorksheet>) excelPackage.Workbook.Worksheets).Any<ExcelWorksheet>((System.Func<ExcelWorksheet, bool>) (w => w.Name == "Structure")) ? excelPackage.Workbook.Worksheets["Structure"] : excelPackage.Workbook.Worksheets.Add("Structure");
      excelWorksheet.Column(1).Width = 15.0;
      int rows = excelWorksheet.Dimension != null ? excelWorksheet.Dimension.Rows : 0;
      foreach (string[] node in nodeList)
      {
        ++rows;
        int Col = 0;
        foreach (string str in node)
        {
          ++Col;
          excelWorksheet.Cells[rows, Col].Value = (object) str;
        }
      }
      excelPackage.Save();
    }

    public List<string[]> ReadStructureFromFile(string filename)
    {
      FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read);
      using (fileStream)
      {
        IExcelDataReader openXmlReader = ExcelReaderFactory.CreateOpenXmlReader((Stream) fileStream);
        IEnumerable<DataRow> dataRows = openXmlReader.AsDataSet().Tables[openXmlReader.Name].Rows.Cast<DataRow>().Select<DataRow, DataRow>((System.Func<DataRow, DataRow>) (a => a));
        List<string[]> nodeList = new List<string[]>();
        TypeHelperExtensionMethods.ForEach<DataRow>(dataRows, (Action<DataRow>) (row => nodeList.Add(Array.ConvertAll<object, string>(row.ItemArray, (Converter<object, string>) (x => x?.ToString())))));
        openXmlReader.Close();
        return nodeList;
      }
    }

    public List<MinomatImportDTO> ReadMinomatsFromFile(string filename)
    {
      FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read);
      using (fileStream)
      {
        IExcelDataReader openXmlReader = ExcelReaderFactory.CreateOpenXmlReader((Stream) fileStream);
        openXmlReader.IsFirstRowAsColumnNames = true;
        return openXmlReader.AsDataSet().Tables[openXmlReader.Name].ToList<MinomatImportDTO>();
      }
    }

    public void WriteArchiveToFile<T>(string fileName, Expression<System.Func<T, bool>> expression)
    {
      ISessionFactory factoryMssArchive = ArchiveManagerNHibernate.GetSessionFactoryMSSArchive();
      int parameterValue = MSS.Business.Utils.AppContext.Current.GetParameterValue<int>("LoadSizeForExportOfArchive");
      ReportingHelper reportingHelper = new ReportingHelper();
      PagingProvider<T> pagingProvider = expression != null ? new PagingProvider<T>(factoryMssArchive, expression) : new PagingProvider<T>(factoryMssArchive);
      int num = pagingProvider.FetchCount();
      ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName));
      excelPackage.Workbook.Properties.Author = "Minol™";
      ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("ArchiveMeterReadingValue");
      excelWorksheet.Column(1).Width = 15.0;
      IList<T> archiveList = pagingProvider.FetchRange(1, parameterValue);
      int startIndex = 2;
      string[] source = reportingHelper.WriteArchiveListHeader<ArchiveMeterReadingValue>().Split(',');
      for (int Col = 1; Col <= ((IEnumerable<string>) source).Count<string>(); ++Col)
        excelWorksheet.Cells[1, Col].Value = (object) source[Col - 1];
      while (startIndex < num)
      {
        List<string[]> archiveListRows = reportingHelper.GetArchiveListRows<ArchiveMeterReadingValue>((IEnumerable<ArchiveMeterReadingValue>) archiveList);
        int Row = startIndex - 1;
        startIndex += parameterValue;
        archiveList = pagingProvider.FetchRange(startIndex, parameterValue);
        foreach (string[] strArray in archiveListRows)
        {
          ++Row;
          int Col = 0;
          foreach (string str in strArray)
          {
            ++Col;
            excelWorksheet.Cells[Row, Col].Value = (object) str;
          }
        }
      }
      excelPackage.Save();
    }
  }
}
