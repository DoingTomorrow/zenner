// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MapClassGenerator
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace HandlerLib.MapManagement
{
  internal class MapClassGenerator
  {
    private string ClassName;
    private string NamespaceName;
    private StringBuilder classText = new StringBuilder();
    private StringBuilder lineText = new StringBuilder();

    internal void StartClassGeneration(
      byte formatVersion,
      string namespaceName,
      string className,
      string[] usedFieldes,
      string[] usedSections = null,
      string[] usedTyps = null,
      uint[] usedSectionsAddress = null,
      uint[] usedSectionSize = null)
    {
      this.classText.Clear();
      this.lineText.Clear();
      this.NamespaceName = namespaceName;
      this.ClassName = className;
      this.classText.AppendLine("using System;");
      this.classText.AppendLine("using HandlerLib.MapManagement;");
      this.classText.AppendLine("");
      this.classText.AppendLine("// Created from MapClassGenerator");
      StringBuilder classText = this.classText;
      DateTime now = DateTime.Now;
      string longDateString = now.ToLongDateString();
      now = DateTime.Now;
      string longTimeString = now.ToLongTimeString();
      string str = "// Creation time: " + longDateString + " " + longTimeString;
      classText.AppendLine(str);
      this.classText.AppendLine("");
      this.classText.AppendLine("namespace " + this.NamespaceName);
      this.classText.AppendLine("{");
      this.classText.AppendLine("    public class " + this.ClassName + " : MapDefClassBase");
      this.classText.AppendLine("    {");
      this.classText.AppendLine("        public override byte[] FullByteList { get { return fullByteList; } }");
      if (usedSections != null && usedTyps != null)
      {
        this.classText.AppendLine("        public override string[] ParameterList { get { return parameterList; } }");
        this.classText.AppendLine("        public override string[] TypList { get { return typList; } }");
        this.classText.AppendLine("        public override string[] SectionList { get { return sectionList; } }");
        this.classText.AppendLine("        public override uint[] SectionAddress { get { return sectionAddress; } }");
        this.classText.AppendLine("        public override uint[] SectionSize { get { return sectionSize; } }");
        this.classText.AppendLine();
        this.classText.Append("        string[] parameterList = { ");
        foreach (string usedFielde in usedFieldes)
          this.classText.Append("\"" + usedFielde + "\",");
        this.classText.AppendLine("};");
        if (usedTyps != null)
        {
          this.classText.Append("        string[] typList = { ");
          foreach (string usedTyp in usedTyps)
            this.classText.Append("\"" + usedTyp + "\",");
          this.classText.AppendLine("};");
        }
        if (usedSections != null)
        {
          this.classText.Append("        string[] sectionList = { ");
          foreach (string usedSection in usedSections)
            this.classText.Append("\"" + usedSection + "\",");
          this.classText.AppendLine("};");
        }
        if (usedSectionsAddress != null)
        {
          this.classText.Append("        uint[] sectionAddress = { ");
          foreach (uint num in usedSectionsAddress)
            this.classText.Append("0x" + num.ToString("x8") + ", ");
          this.classText.AppendLine("};");
        }
        if (usedSectionSize != null)
        {
          this.classText.Append("        uint[] sectionSize = { ");
          foreach (uint num in usedSectionSize)
            this.classText.Append("0x" + num.ToString("x4") + ", ");
          this.classText.AppendLine("};");
        }
      }
      this.classText.AppendLine();
      this.classText.Append("        /*UsedFields: ");
      foreach (string usedFielde in usedFieldes)
        this.classText.Append(usedFielde + ",");
      this.classText.AppendLine("*/");
      this.classText.Append("        byte[] fullByteList = { /*FormatVersion*/ ");
      this.AddAsByteListForced(formatVersion);
      this.NewLine();
    }

    internal void FinaliseClassGeneraton()
    {
      this.NewLine();
      this.classText.AppendLine("                                     };");
      this.classText.AppendLine("    }");
      this.classText.AppendLine("}");
    }

    internal void SaveClass(string fileName)
    {
      using (StreamWriter streamWriter = new StreamWriter(fileName, false, Encoding.Unicode))
      {
        streamWriter.Write(this.classText.ToString());
        streamWriter.Close();
      }
    }

    internal void NewLine()
    {
      this.classText.AppendLine(this.lineText.ToString());
      this.lineText.Clear();
      this.lineText.Append("       ");
    }

    internal void InsertComment(string comment) => this.lineText.Append(" /*" + comment + "*/ ");

    internal void StartCommentBlock(string comment)
    {
      this.classText.AppendLine();
      this.classText.AppendLine("/******** " + comment + " ********");
    }

    internal void FinishCommentBlock() => this.classText.AppendLine("*/");

    internal void AddAddressParameterLineToCommentBlock(uint address, string parameterName)
    {
      this.classText.AppendLine(address.ToString("x08") + ": " + parameterName);
    }

    internal void AddParameterNotDefined() => this.AddByteCode((byte) 0);

    internal void AddAsByteList(string theString)
    {
      if (theString == null)
      {
        this.AddByteCode((byte) 0);
      }
      else
      {
        this.AddByteCode((byte) 1);
        this.AddAsByteListForced(theString);
      }
    }

    internal void AddAsByteListForced(string theString)
    {
      byte[] numArray = theString != null ? Encoding.UTF8.GetBytes(theString) : throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_PARAMETER_IS_NULL, "MapClassGenerator ERROR!!!", (Exception) new ArgumentNullException("String has to be defined"));
      this.AddLengthBytes(numArray.Length);
      for (int index = 0; index < numArray.Length; ++index)
        this.AddByteCode(numArray[index]);
    }

    internal void AddAsByteList(byte? theByte)
    {
      if (!theByte.HasValue)
      {
        this.AddByteCode((byte) 0);
      }
      else
      {
        this.AddByteCode((byte) 1);
        this.AddAsByteListForced(theByte.Value);
      }
    }

    internal void AddAsByteListForced(byte theByte) => this.AddByteCode(theByte);

    internal void AddAsByteList(byte[] theBytes)
    {
      if (theBytes == null)
      {
        this.AddByteCode((byte) 0);
      }
      else
      {
        this.AddByteCode((byte) 1);
        this.AddAsByteListForced(theBytes);
      }
    }

    internal void AddAsByteListForced(byte[] theBytes)
    {
      this.AddLengthBytes(theBytes.Length);
      for (int index = 0; index < theBytes.Length; ++index)
        this.AddByteCode(theBytes[index]);
    }

    internal void AddAsByteList(int? theInt)
    {
      if (!theInt.HasValue)
      {
        this.AddByteCode((byte) 0);
      }
      else
      {
        this.AddByteCode((byte) 1);
        this.AddAsByteListForced(theInt.Value);
      }
    }

    internal void AddAsByteListForced(int theInt)
    {
      foreach (byte theByte in BitConverter.GetBytes(theInt))
        this.AddByteCode(theByte);
    }

    internal void AddAsByteList(uint? theUInt)
    {
      if (!theUInt.HasValue)
      {
        this.AddByteCode((byte) 0);
      }
      else
      {
        this.AddByteCode((byte) 1);
        this.AddAsByteListForced(theUInt.Value);
      }
    }

    internal void AddAsByteListForced(uint theUInt)
    {
      foreach (byte theByte in BitConverter.GetBytes(theUInt))
        this.AddByteCode(theByte);
    }

    internal void AddAsByteList(ushort? theUShort)
    {
      if (!theUShort.HasValue)
      {
        this.AddByteCode((byte) 0);
      }
      else
      {
        this.AddByteCode((byte) 1);
        this.AddAsByteListForced(theUShort.Value);
      }
    }

    internal void AddAsByteListForced(ushort theUShort)
    {
      foreach (byte theByte in BitConverter.GetBytes(theUShort))
        this.AddByteCode(theByte);
    }

    internal void AddAsByteList(double? theDouble)
    {
      if (!theDouble.HasValue)
      {
        this.AddByteCode((byte) 0);
      }
      else
      {
        this.AddByteCode((byte) 1);
        this.AddAsByteListForced(theDouble.Value);
      }
    }

    internal void AddAsByteListForced(double theDouble)
    {
      foreach (byte theByte in BitConverter.GetBytes(theDouble))
        this.AddByteCode(theByte);
    }

    internal void AddAsByteList(bool? theBool)
    {
      if (!theBool.HasValue)
      {
        this.AddByteCode((byte) 0);
      }
      else
      {
        this.AddByteCode((byte) 1);
        this.AddAsByteListForced(theBool.Value);
      }
    }

    internal void AddAsByteListForced(bool theBool)
    {
      if (theBool)
        this.AddByteCode((byte) 1);
      else
        this.AddByteCode((byte) 0);
    }

    private void AddLengthBytes(int length)
    {
      if (length < (int) byte.MaxValue)
      {
        this.AddByteCode((byte) length);
      }
      else
      {
        this.AddByteCode(byte.MaxValue);
        byte[] bytes = BitConverter.GetBytes(length);
        if (length < (int) ushort.MaxValue)
        {
          for (int index = 0; index < 2; ++index)
            this.AddByteCode(bytes[index]);
        }
        else
        {
          this.AddByteCode(byte.MaxValue);
          this.AddByteCode(byte.MaxValue);
          for (int index = 0; index < bytes.Length; ++index)
            this.AddByteCode(bytes[index]);
        }
      }
    }

    private void AddByteCode(byte theByte)
    {
      this.lineText.Append("0x" + theByte.ToString("x02") + ",");
    }
  }
}
