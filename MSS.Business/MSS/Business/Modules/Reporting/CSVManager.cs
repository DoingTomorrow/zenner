// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Reporting.CSVManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.VisualBasic.FileIO;
using MSS.Business.Errors;
using MSS.Localisation;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace MSS.Business.Modules.Reporting
{
  public class CSVManager
  {
    public void WriteToFileCultureDependent(
      string filename,
      List<string[]> nodeList,
      string valueSeparator)
    {
      string directoryName = Path.GetDirectoryName(filename);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      StreamWriter streamWriter = new StreamWriter(filename, true);
      foreach (string[] node in nodeList)
        streamWriter.WriteLine(string.Join(valueSeparator, node));
      streamWriter.Close();
    }

    public void WriteToFile(string filename, List<string[]> nodeList)
    {
      StreamWriter streamWriter = new StreamWriter(filename, true);
      foreach (string[] node in nodeList)
        streamWriter.WriteLine(string.Join(",", node));
      streamWriter.Close();
    }

    public List<string[]> ReadStructureFromFile(string fileName, string[] delimiters = null)
    {
      List<string[]> strArrayList = new List<string[]>();
      try
      {
        using (TextFieldParser textFieldParser1 = new TextFieldParser(fileName))
        {
          TextFieldParser textFieldParser2 = textFieldParser1;
          string[] strArray1 = delimiters;
          if (strArray1 == null)
            strArray1 = new string[1]{ "," };
          textFieldParser2.SetDelimiters(strArray1);
          textFieldParser1.HasFieldsEnclosedInQuotes = true;
          while (!textFieldParser1.EndOfData)
          {
            string[] strArray2 = textFieldParser1.ReadFields();
            strArrayList.Add(strArray2);
          }
        }
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        throw new BaseApplicationException(Resources.MSS_Client_ImportCSV_OpenFileError);
      }
      return strArrayList;
    }

    public static List<string[]> AddQuatForCSV(List<string[]> rows)
    {
      List<string[]> strArrayList = new List<string[]>();
      foreach (string[] row in rows)
      {
        string[] strArray = new string[row.Length];
        for (int index = 0; index < row.Length; ++index)
        {
          int num;
          if (!string.IsNullOrEmpty(row[index]))
            num = row[index].IndexOfAny(new char[2]
            {
              '"',
              ','
            }) != -1 ? 1 : 0;
          else
            num = 0;
          strArray[index] = num == 0 ? row[index] : string.Format("\"{0}\"", (object) row[index].Replace("\"", "\"\""));
        }
        strArrayList.Add(strArray);
      }
      return strArrayList;
    }
  }
}
