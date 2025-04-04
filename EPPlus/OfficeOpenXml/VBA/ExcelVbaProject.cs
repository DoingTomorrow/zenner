// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.VBA.ExcelVbaProject
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.VBA
{
  public class ExcelVbaProject
  {
    private const string schemaRelVba = "http://schemas.microsoft.com/office/2006/relationships/vbaProject";
    internal const string PartUri = "/xl/vbaProject.bin";
    internal ExcelWorkbook _wb;
    internal ZipPackage _pck;
    private ExcelVbaSignature _signature;
    private ExcelVbaProtection _protection;

    internal ExcelVbaProject(ExcelWorkbook wb)
    {
      this._wb = wb;
      this._pck = this._wb._package.Package;
      this.References = new ExcelVbaReferenceCollection();
      this.Modules = new ExcelVbaModuleCollection(this);
      ZipPackageRelationship packageRelationship = this._wb.Part.GetRelationshipsByType("http://schemas.microsoft.com/office/2006/relationships/vbaProject").FirstOrDefault<ZipPackageRelationship>();
      if (packageRelationship != null)
      {
        this.Uri = UriHelper.ResolvePartUri(packageRelationship.SourceUri, packageRelationship.TargetUri);
        this.Part = this._pck.GetPart(this.Uri);
        this.GetProject();
      }
      else
      {
        this.Lcid = 0;
        this.Part = (ZipPackagePart) null;
      }
    }

    public ExcelVbaProject.eSyskind SystemKind { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string HelpFile1 { get; set; }

    public string HelpFile2 { get; set; }

    public int HelpContextID { get; set; }

    public string Constants { get; set; }

    public int CodePage { get; internal set; }

    internal int LibFlags { get; set; }

    internal int MajorVersion { get; set; }

    internal int MinorVersion { get; set; }

    internal int Lcid { get; set; }

    internal int LcidInvoke { get; set; }

    internal string ProjectID { get; set; }

    internal string ProjectStreamText { get; set; }

    public ExcelVbaReferenceCollection References { get; set; }

    public ExcelVbaModuleCollection Modules { get; set; }

    public ExcelVbaSignature Signature
    {
      get
      {
        if (this._signature == null)
          this._signature = new ExcelVbaSignature(this.Part);
        return this._signature;
      }
    }

    public ExcelVbaProtection Protection
    {
      get
      {
        if (this._protection == null)
          this._protection = new ExcelVbaProtection(this);
        return this._protection;
      }
    }

    private void GetProject()
    {
      MemoryStream stream = this.Part.GetStream();
      byte[] numArray = new byte[stream.Length];
      stream.Read(numArray, 0, (int) stream.Length);
      this.Document = new CompoundDocument(numArray);
      this.ReadDirStream();
      this.ProjectStreamText = Encoding.GetEncoding(this.CodePage).GetString(this.Document.Storage.DataStreams["PROJECT"]);
      this.ReadModules();
      this.ReadProjectProperties();
    }

    private void ReadModules()
    {
      foreach (ExcelVBAModule module in (ExcelVBACollectionBase<ExcelVBAModule>) this.Modules)
      {
        byte[] bytes = CompoundDocument.DecompressPart(this.Document.Storage.SubStorage["VBA"].DataStreams[module.streamName], (int) module.ModuleOffset);
        string str = Encoding.GetEncoding(this.CodePage).GetString(bytes);
        int startIndex;
        int num;
        for (startIndex = 0; startIndex + 9 < str.Length && str.Substring(startIndex, 9) == "Attribute"; startIndex = num + 2)
        {
          num = str.IndexOf("\r\n", startIndex);
          string[] strArray;
          if (num > 0)
            strArray = str.Substring(startIndex + 9, num - startIndex - 9).Split('=');
          else
            strArray = str.Substring(startIndex + 9).Split(new char[1]
            {
              '='
            }, 1);
          if (strArray.Length > 1)
          {
            strArray[1] = strArray[1].Trim();
            ExcelVbaModuleAttribute vbaModuleAttribute = new ExcelVbaModuleAttribute()
            {
              Name = strArray[0].Trim(),
              DataType = strArray[1].StartsWith("\"") ? eAttributeDataType.String : eAttributeDataType.NonString,
              Value = strArray[1].StartsWith("\"") ? strArray[1].Substring(1, strArray[1].Length - 2) : strArray[1]
            };
            module.Attributes._list.Add(vbaModuleAttribute);
          }
        }
        module.Code = str.Substring(startIndex);
      }
    }

    private void ReadProjectProperties()
    {
      this._protection = new ExcelVbaProtection(this);
      string str1 = "";
      foreach (string str2 in Regex.Split(this.ProjectStreamText, "\r\n"))
      {
        if (!str2.StartsWith("["))
        {
          string[] strArray = str2.Split('=');
          if (strArray.Length > 1 && strArray[1].Length > 1 && strArray[1].StartsWith("\""))
            strArray[1] = strArray[1].Substring(1, strArray[1].Length - 2);
          switch (strArray[0])
          {
            case "ID":
              this.ProjectID = strArray[1];
              continue;
            case "Document":
              this.Modules[strArray[1].Substring(0, strArray[1].IndexOf("/&H"))].Type = eModuleType.Document;
              continue;
            case "Package":
              str1 = strArray[1];
              continue;
            case "BaseClass":
              this.Modules[strArray[1]].Type = eModuleType.Designer;
              this.Modules[strArray[1]].ClassID = str1;
              continue;
            case "Module":
              this.Modules[strArray[1]].Type = eModuleType.Module;
              continue;
            case "Class":
              this.Modules[strArray[1]].Type = eModuleType.Class;
              continue;
            case "CMG":
              byte[] numArray = this.Decrypt(strArray[1]);
              this._protection.UserProtected = ((int) numArray[0] & 1) != 0;
              this._protection.HostProtected = ((int) numArray[0] & 2) != 0;
              this._protection.VbeProtected = ((int) numArray[0] & 4) != 0;
              continue;
            case "DPB":
              byte[] sourceArray = this.Decrypt(strArray[1]);
              if (sourceArray.Length >= 28)
              {
                int num1 = (int) sourceArray[0];
                byte[] destinationArray1 = new byte[3];
                Array.Copy((Array) sourceArray, 1, (Array) destinationArray1, 0, 3);
                byte[] destinationArray2 = new byte[4];
                this._protection.PasswordKey = new byte[4];
                Array.Copy((Array) sourceArray, 4, (Array) destinationArray2, 0, 4);
                byte[] destinationArray3 = new byte[20];
                this._protection.PasswordHash = new byte[20];
                Array.Copy((Array) sourceArray, 8, (Array) destinationArray3, 0, 20);
                for (int index1 = 0; index1 < 24; ++index1)
                {
                  int num2 = 128 >> index1 % 8;
                  if (index1 < 4)
                  {
                    this._protection.PasswordKey[index1] = ((int) destinationArray1[0] & num2) != 0 ? destinationArray2[index1] : (byte) 0;
                  }
                  else
                  {
                    int index2 = (index1 - index1 % 8) / 8;
                    this._protection.PasswordHash[index1 - 4] = ((int) destinationArray1[index2] & num2) != 0 ? destinationArray3[index1 - 4] : (byte) 0;
                  }
                }
                continue;
              }
              continue;
            case "GC":
              this._protection.VisibilityState = this.Decrypt(strArray[1])[0] == byte.MaxValue;
              continue;
            default:
              continue;
          }
        }
      }
    }

    private byte[] Decrypt(string value)
    {
      byte[] numArray = this.GetByte(value);
      byte[] sourceArray = new byte[value.Length - 1];
      byte num1 = numArray[0];
      sourceArray[0] = (byte) ((uint) numArray[1] ^ (uint) num1);
      sourceArray[1] = (byte) ((uint) numArray[2] ^ (uint) num1);
      for (int index = 2; index < numArray.Length - 1; ++index)
        sourceArray[index] = (byte) ((uint) numArray[index + 1] ^ (uint) numArray[index - 1] + (uint) sourceArray[index - 1]);
      int num2 = (int) sourceArray[0];
      int num3 = (int) sourceArray[1];
      byte num4 = (byte) (((int) num1 & 6) / 2);
      int int32 = BitConverter.ToInt32(sourceArray, (int) num4 + 2);
      byte[] destinationArray = new byte[int32];
      Array.Copy((Array) sourceArray, 6 + (int) num4, (Array) destinationArray, 0, int32);
      return destinationArray;
    }

    private string Encrypt(byte[] value)
    {
      byte[] data = new byte[1];
      RandomNumberGenerator.Create().GetBytes(data);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) new MemoryStream());
      byte[] numArray = new byte[value.Length + 10];
      numArray[0] = data[0];
      numArray[1] = (byte) (2U ^ (uint) data[0]);
      byte num1 = 0;
      foreach (char ch in this.ProjectID)
        num1 += (byte) ch;
      numArray[2] = (byte) ((uint) num1 ^ (uint) data[0]);
      int num2 = ((int) data[0] & 6) / 2;
      for (int index = 0; index < num2; ++index)
        binaryWriter.Write(data[0]);
      binaryWriter.Write(value.Length);
      binaryWriter.Write(value);
      int index1 = 3;
      byte num3 = num1;
      foreach (byte num4 in ((MemoryStream) binaryWriter.BaseStream).ToArray())
      {
        numArray[index1] = (byte) ((uint) num4 ^ (uint) numArray[index1 - 2] + (uint) num3);
        ++index1;
        num3 = num4;
      }
      return this.GetString(numArray, index1 - 1);
    }

    private string GetString(byte[] value, int max)
    {
      string str = "";
      for (int index = 0; index <= max; ++index)
        str = value[index] >= (byte) 16 ? str + value[index].ToString("x") : str + "0" + value[index].ToString("x");
      return str.ToUpper();
    }

    private byte[] GetByte(string value)
    {
      byte[] numArray = new byte[value.Length / 2];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = byte.Parse(value.Substring(index * 2, 2), NumberStyles.AllowHexSpecifier);
      return numArray;
    }

    private void ReadDirStream()
    {
      BinaryReader br = new BinaryReader((Stream) new MemoryStream(CompoundDocument.DecompressPart(this.Document.Storage.SubStorage["VBA"].DataStreams["dir"])));
      ExcelVbaReference excelVbaReference1 = (ExcelVbaReference) null;
      string str = "";
      ExcelVBAModule excelVbaModule = (ExcelVBAModule) null;
      bool flag = false;
      while (br.BaseStream.Position < br.BaseStream.Length && !flag)
      {
        ushort num1 = br.ReadUInt16();
        uint size1 = br.ReadUInt32();
        switch (num1)
        {
          case 1:
            this.SystemKind = (ExcelVbaProject.eSyskind) br.ReadUInt32();
            continue;
          case 2:
            this.Lcid = (int) br.ReadUInt32();
            continue;
          case 3:
            this.CodePage = (int) br.ReadUInt16();
            continue;
          case 4:
            this.Name = this.GetString(br, size1);
            continue;
          case 5:
            this.Description = this.GetUnicodeString(br, size1);
            continue;
          case 6:
            this.HelpFile1 = this.GetString(br, size1);
            continue;
          case 7:
            this.HelpContextID = (int) br.ReadUInt32();
            continue;
          case 8:
            this.LibFlags = (int) br.ReadUInt32();
            continue;
          case 9:
            this.MajorVersion = (int) br.ReadUInt32();
            this.MinorVersion = (int) br.ReadUInt16();
            continue;
          case 12:
            this.Constants = this.GetUnicodeString(br, size1);
            continue;
          case 13:
            uint size2 = br.ReadUInt32();
            ExcelVbaReference excelVbaReference2 = new ExcelVbaReference();
            excelVbaReference2.Name = str;
            excelVbaReference2.ReferenceRecordID = (int) num1;
            excelVbaReference2.Libid = this.GetString(br, size2);
            int num2 = (int) br.ReadUInt32();
            int num3 = (int) br.ReadUInt16();
            this.References.Add(excelVbaReference2);
            continue;
          case 14:
            ExcelVbaReferenceProject referenceProject = new ExcelVbaReferenceProject();
            referenceProject.ReferenceRecordID = (int) num1;
            referenceProject.Name = str;
            uint size3 = br.ReadUInt32();
            referenceProject.Libid = this.GetString(br, size3);
            uint size4 = br.ReadUInt32();
            referenceProject.LibIdRelative = this.GetString(br, size4);
            referenceProject.MajorVersion = br.ReadUInt32();
            referenceProject.MinorVersion = br.ReadUInt16();
            this.References.Add((ExcelVbaReference) referenceProject);
            continue;
          case 15:
            int num4 = (int) br.ReadUInt16();
            continue;
          case 16:
            flag = true;
            continue;
          case 19:
            int num5 = (int) br.ReadUInt16();
            continue;
          case 20:
            this.LcidInvoke = (int) br.ReadUInt32();
            continue;
          case 22:
            str = this.GetUnicodeString(br, size1);
            continue;
          case 25:
            excelVbaModule = new ExcelVBAModule();
            excelVbaModule.Name = this.GetUnicodeString(br, size1);
            this.Modules.Add(excelVbaModule);
            continue;
          case 26:
            excelVbaModule.streamName = this.GetUnicodeString(br, size1);
            continue;
          case 28:
            excelVbaModule.Description = this.GetUnicodeString(br, size1);
            continue;
          case 30:
            excelVbaModule.HelpContext = (int) br.ReadUInt32();
            continue;
          case 37:
            excelVbaModule.ReadOnly = true;
            continue;
          case 40:
            excelVbaModule.Private = true;
            continue;
          case 44:
            excelVbaModule.Cookie = br.ReadUInt16();
            continue;
          case 47:
            ExcelVbaReferenceControl referenceControl1 = (ExcelVbaReferenceControl) excelVbaReference1;
            referenceControl1.ReferenceRecordID = (int) num1;
            uint size5 = br.ReadUInt32();
            referenceControl1.LibIdTwiddled = this.GetString(br, size5);
            int num6 = (int) br.ReadUInt32();
            int num7 = (int) br.ReadUInt16();
            continue;
          case 48:
            ExcelVbaReferenceControl referenceControl2 = (ExcelVbaReferenceControl) excelVbaReference1;
            uint size6 = br.ReadUInt32();
            referenceControl2.LibIdExternal = this.GetString(br, size6);
            int num8 = (int) br.ReadUInt32();
            int num9 = (int) br.ReadUInt16();
            referenceControl2.OriginalTypeLib = new Guid(br.ReadBytes(16));
            referenceControl2.Cookie = br.ReadUInt32();
            continue;
          case 49:
            excelVbaModule.ModuleOffset = br.ReadUInt32();
            continue;
          case 51:
            excelVbaReference1 = (ExcelVbaReference) new ExcelVbaReferenceControl();
            excelVbaReference1.ReferenceRecordID = (int) num1;
            excelVbaReference1.Name = str;
            excelVbaReference1.Libid = this.GetString(br, size1);
            this.References.Add(excelVbaReference1);
            continue;
          case 61:
            this.HelpFile2 = this.GetString(br, size1);
            continue;
          default:
            continue;
        }
      }
    }

    internal void Save()
    {
      if (!this.Validate())
        return;
      CompoundDocument compoundDocument = new CompoundDocument();
      compoundDocument.Storage = new CompoundDocument.StoragePart();
      CompoundDocument.StoragePart storagePart = new CompoundDocument.StoragePart();
      compoundDocument.Storage.SubStorage.Add("VBA", storagePart);
      storagePart.DataStreams.Add("_VBA_PROJECT", this.CreateVBAProjectStream());
      storagePart.DataStreams.Add("dir", this.CreateDirStream());
      foreach (ExcelVBAModule module in (ExcelVBACollectionBase<ExcelVBAModule>) this.Modules)
        storagePart.DataStreams.Add(module.Name, CompoundDocument.CompressPart(Encoding.GetEncoding(this.CodePage).GetBytes(module.Attributes.GetAttributeText() + module.Code)));
      if (this.Document != null)
      {
        foreach (KeyValuePair<string, CompoundDocument.StoragePart> keyValuePair in this.Document.Storage.SubStorage)
        {
          if (keyValuePair.Key != "VBA")
            compoundDocument.Storage.SubStorage.Add(keyValuePair.Key, keyValuePair.Value);
        }
        foreach (KeyValuePair<string, byte[]> dataStream in this.Document.Storage.DataStreams)
        {
          if (dataStream.Key != "dir" && dataStream.Key != "PROJECT" && dataStream.Key != "PROJECTwm")
            compoundDocument.Storage.DataStreams.Add(dataStream.Key, dataStream.Value);
        }
      }
      compoundDocument.Storage.DataStreams.Add("PROJECT", this.CreateProjectStream());
      compoundDocument.Storage.DataStreams.Add("PROJECTwm", this.CreateProjectwmStream());
      if (this.Part == null)
      {
        this.Uri = new Uri("/xl/vbaProject.bin", UriKind.Relative);
        this.Part = this._pck.CreatePart(this.Uri, "application/vnd.ms-office.vbaProject");
        this._wb.Part.CreateRelationship(this.Uri, TargetMode.Internal, "http://schemas.microsoft.com/office/2006/relationships/vbaProject");
      }
      byte[] buffer = compoundDocument.Save();
      MemoryStream stream = this.Part.GetStream(FileMode.Create);
      stream.Write(buffer, 0, buffer.Length);
      stream.Flush();
      this.Signature.Save(this);
    }

    private bool Validate()
    {
      this.Description = this.Description ?? "";
      this.HelpFile1 = this.HelpFile1 ?? "";
      this.HelpFile2 = this.HelpFile2 ?? "";
      this.Constants = this.Constants ?? "";
      return true;
    }

    private byte[] CreateVBAProjectStream()
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) new MemoryStream());
      binaryWriter.Write((ushort) 25036);
      binaryWriter.Write(ushort.MaxValue);
      binaryWriter.Write((byte) 0);
      binaryWriter.Write((ushort) 0);
      return ((MemoryStream) binaryWriter.BaseStream).ToArray();
    }

    private byte[] CreateDirStream()
    {
      BinaryWriter bw = new BinaryWriter((Stream) new MemoryStream());
      bw.Write((ushort) 1);
      bw.Write(4U);
      bw.Write((uint) this.SystemKind);
      bw.Write((ushort) 2);
      bw.Write(4U);
      bw.Write((uint) this.Lcid);
      bw.Write((ushort) 20);
      bw.Write(4U);
      bw.Write((uint) this.LcidInvoke);
      bw.Write((ushort) 3);
      bw.Write(2U);
      bw.Write((ushort) this.CodePage);
      bw.Write((ushort) 4);
      bw.Write((uint) this.Name.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(this.Name));
      bw.Write((ushort) 5);
      bw.Write((uint) this.Description.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(this.Description));
      bw.Write((ushort) 64);
      bw.Write((uint) (this.Description.Length * 2));
      bw.Write(Encoding.Unicode.GetBytes(this.Description));
      bw.Write((ushort) 6);
      bw.Write((uint) this.HelpFile1.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(this.HelpFile1));
      bw.Write((ushort) 61);
      bw.Write((uint) this.HelpFile2.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(this.HelpFile2));
      bw.Write((ushort) 7);
      bw.Write(4U);
      bw.Write((uint) this.HelpContextID);
      bw.Write((ushort) 8);
      bw.Write(4U);
      bw.Write(0U);
      bw.Write((ushort) 9);
      bw.Write(4U);
      bw.Write((uint) this.MajorVersion);
      bw.Write((ushort) this.MinorVersion);
      bw.Write((ushort) 12);
      bw.Write((uint) this.Constants.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(this.Constants));
      bw.Write((ushort) 60);
      bw.Write((uint) this.Constants.Length / 2U);
      bw.Write(Encoding.Unicode.GetBytes(this.Constants));
      foreach (ExcelVbaReference reference in (ExcelVBACollectionBase<ExcelVbaReference>) this.References)
      {
        this.WriteNameReference(bw, reference);
        if (reference.ReferenceRecordID == 47)
          this.WriteControlReference(bw, reference);
        else if (reference.ReferenceRecordID == 51)
          this.WriteOrginalReference(bw, reference);
        else if (reference.ReferenceRecordID == 13)
          this.WriteRegisteredReference(bw, reference);
        else if (reference.ReferenceRecordID == 14)
          this.WriteProjectReference(bw, reference);
      }
      bw.Write((ushort) 15);
      bw.Write(2U);
      bw.Write((ushort) this.Modules.Count);
      bw.Write((ushort) 19);
      bw.Write(2U);
      bw.Write(ushort.MaxValue);
      foreach (ExcelVBAModule module in (ExcelVBACollectionBase<ExcelVBAModule>) this.Modules)
        this.WriteModuleRecord(bw, module);
      bw.Write((ushort) 16);
      bw.Write(0U);
      return CompoundDocument.CompressPart(((MemoryStream) bw.BaseStream).ToArray());
    }

    private void WriteModuleRecord(BinaryWriter bw, ExcelVBAModule module)
    {
      bw.Write((ushort) 25);
      bw.Write((uint) module.Name.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(module.Name));
      bw.Write((ushort) 71);
      bw.Write((uint) (module.Name.Length * 2));
      bw.Write(Encoding.Unicode.GetBytes(module.Name));
      bw.Write((ushort) 26);
      bw.Write((uint) module.Name.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(module.Name));
      bw.Write((ushort) 50);
      bw.Write((uint) (module.Name.Length * 2));
      bw.Write(Encoding.Unicode.GetBytes(module.Name));
      module.Description = module.Description ?? "";
      bw.Write((ushort) 28);
      bw.Write((uint) module.Description.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(module.Description));
      bw.Write((ushort) 72);
      bw.Write((uint) (module.Description.Length * 2));
      bw.Write(Encoding.Unicode.GetBytes(module.Description));
      bw.Write((ushort) 49);
      bw.Write(4U);
      bw.Write(0U);
      bw.Write((ushort) 30);
      bw.Write(4U);
      bw.Write((uint) module.HelpContext);
      bw.Write((ushort) 44);
      bw.Write(2U);
      bw.Write(ushort.MaxValue);
      bw.Write(module.Type == eModuleType.Module ? (ushort) 33 : (ushort) 34);
      bw.Write(0U);
      if (module.ReadOnly)
      {
        bw.Write((ushort) 37);
        bw.Write(0U);
      }
      if (module.Private)
      {
        bw.Write((ushort) 40);
        bw.Write(0U);
      }
      bw.Write((ushort) 43);
      bw.Write(0U);
    }

    private void WriteNameReference(BinaryWriter bw, ExcelVbaReference reference)
    {
      bw.Write((ushort) 22);
      bw.Write((uint) reference.Name.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(reference.Name));
      bw.Write((ushort) 62);
      bw.Write((uint) (reference.Name.Length * 2));
      bw.Write(Encoding.Unicode.GetBytes(reference.Name));
    }

    private void WriteControlReference(BinaryWriter bw, ExcelVbaReference reference)
    {
      this.WriteOrginalReference(bw, reference);
      bw.Write((ushort) 47);
      ExcelVbaReferenceControl referenceControl = (ExcelVbaReferenceControl) reference;
      bw.Write((uint) (4 + referenceControl.LibIdTwiddled.Length + 4 + 2));
      bw.Write((uint) referenceControl.LibIdTwiddled.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(referenceControl.LibIdTwiddled));
      bw.Write(0U);
      bw.Write((ushort) 0);
      this.WriteNameReference(bw, reference);
      bw.Write((ushort) 48);
      bw.Write((uint) (4 + referenceControl.LibIdExternal.Length + 4 + 2 + 16 + 4));
      bw.Write((uint) referenceControl.LibIdExternal.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(referenceControl.LibIdExternal));
      bw.Write(0U);
      bw.Write((ushort) 0);
      bw.Write(referenceControl.OriginalTypeLib.ToByteArray());
      bw.Write(referenceControl.Cookie);
    }

    private void WriteOrginalReference(BinaryWriter bw, ExcelVbaReference reference)
    {
      bw.Write((ushort) 51);
      bw.Write((uint) reference.Libid.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(reference.Libid));
    }

    private void WriteProjectReference(BinaryWriter bw, ExcelVbaReference reference)
    {
      bw.Write((ushort) 14);
      ExcelVbaReferenceProject referenceProject = (ExcelVbaReferenceProject) reference;
      bw.Write((uint) (4 + referenceProject.Libid.Length + 4 + referenceProject.LibIdRelative.Length + 4 + 2));
      bw.Write((uint) referenceProject.Libid.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(referenceProject.Libid));
      bw.Write((uint) referenceProject.LibIdRelative.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(referenceProject.LibIdRelative));
      bw.Write(referenceProject.MajorVersion);
      bw.Write(referenceProject.MinorVersion);
    }

    private void WriteRegisteredReference(BinaryWriter bw, ExcelVbaReference reference)
    {
      bw.Write((ushort) 13);
      bw.Write((uint) (4 + reference.Libid.Length + 4 + 2));
      bw.Write((uint) reference.Libid.Length);
      bw.Write(Encoding.GetEncoding(this.CodePage).GetBytes(reference.Libid));
      bw.Write(0U);
      bw.Write((ushort) 0);
    }

    private byte[] CreateProjectwmStream()
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) new MemoryStream());
      foreach (ExcelVBAModule module in (ExcelVBACollectionBase<ExcelVBAModule>) this.Modules)
      {
        binaryWriter.Write(Encoding.GetEncoding(this.CodePage).GetBytes(module.Name));
        binaryWriter.Write((byte) 0);
        binaryWriter.Write(Encoding.Unicode.GetBytes(module.Name));
        binaryWriter.Write((ushort) 0);
      }
      binaryWriter.Write((ushort) 0);
      return CompoundDocument.CompressPart(((MemoryStream) binaryWriter.BaseStream).ToArray());
    }

    private byte[] CreateProjectStream()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("ID=\"{0}\"\r\n", (object) this.ProjectID);
      foreach (ExcelVBAModule module in (ExcelVBACollectionBase<ExcelVBAModule>) this.Modules)
      {
        if (module.Type == eModuleType.Document)
          stringBuilder.AppendFormat("Document={0}/&H00000000\r\n", (object) module.Name);
        else if (module.Type == eModuleType.Module)
          stringBuilder.AppendFormat("Module={0}\r\n", (object) module.Name);
        else if (module.Type == eModuleType.Class)
        {
          stringBuilder.AppendFormat("Class={0}\r\n", (object) module.Name);
        }
        else
        {
          stringBuilder.AppendFormat("Package={0}\r\n", (object) module.ClassID);
          stringBuilder.AppendFormat("BaseClass={0}\r\n", (object) module.Name);
        }
      }
      if (this.HelpFile1 != "")
        stringBuilder.AppendFormat("HelpFile={0}\r\n", (object) this.HelpFile1);
      stringBuilder.AppendFormat("Name=\"{0}\"\r\n", (object) this.Name);
      stringBuilder.AppendFormat("HelpContextID={0}\r\n", (object) this.HelpContextID);
      if (!string.IsNullOrEmpty(this.Description))
        stringBuilder.AppendFormat("Description=\"{0}\"\r\n", (object) this.Description);
      stringBuilder.AppendFormat("VersionCompatible32=\"393222000\"\r\n");
      stringBuilder.AppendFormat("CMG=\"{0}\"\r\n", (object) this.WriteProtectionStat());
      stringBuilder.AppendFormat("DPB=\"{0}\"\r\n", (object) this.WritePassword());
      stringBuilder.AppendFormat("GC=\"{0}\"\r\n\r\n", (object) this.WriteVisibilityState());
      stringBuilder.Append("[Host Extender Info]\r\n");
      stringBuilder.Append("&H00000001={3832D640-CF90-11CF-8E43-00A0C911005A};VBE;&H00000000\r\n");
      stringBuilder.Append("\r\n");
      stringBuilder.Append("[Workspace]\r\n");
      foreach (ExcelVBAModule module in (ExcelVBACollectionBase<ExcelVBAModule>) this.Modules)
        stringBuilder.AppendFormat("{0}=0, 0, 0, 0, C \r\n", (object) module.Name);
      string s = stringBuilder.ToString();
      return Encoding.GetEncoding(this.CodePage).GetBytes(s);
    }

    private string WriteProtectionStat()
    {
      return this.Encrypt(BitConverter.GetBytes((this._protection.UserProtected ? 1 : 0) | (this._protection.HostProtected ? 2 : 0) | (this._protection.VbeProtected ? 4 : 0)));
    }

    private string WritePassword()
    {
      byte[] buffer = new byte[3];
      byte[] numArray1 = new byte[4];
      byte[] numArray2 = new byte[20];
      if (this.Protection.PasswordKey == null)
        return this.Encrypt(new byte[1]);
      Array.Copy((Array) this.Protection.PasswordKey, (Array) numArray1, 4);
      Array.Copy((Array) this.Protection.PasswordHash, (Array) numArray2, 20);
      for (int index1 = 0; index1 < 24; ++index1)
      {
        byte num = (byte) (128 >> index1 % 8);
        if (index1 < 4)
        {
          if (numArray1[index1] == (byte) 0)
            numArray1[index1] = (byte) 1;
          else
            buffer[0] |= num;
        }
        else if (numArray2[index1 - 4] == (byte) 0)
        {
          numArray2[index1 - 4] = (byte) 1;
        }
        else
        {
          int index2 = (index1 - index1 % 8) / 8;
          buffer[index2] |= num;
        }
      }
      BinaryWriter binaryWriter = new BinaryWriter((Stream) new MemoryStream());
      binaryWriter.Write(byte.MaxValue);
      binaryWriter.Write(buffer);
      binaryWriter.Write(numArray1);
      binaryWriter.Write(numArray2);
      binaryWriter.Write((byte) 0);
      return this.Encrypt(((MemoryStream) binaryWriter.BaseStream).ToArray());
    }

    private string WriteVisibilityState()
    {
      return this.Encrypt(new byte[1]
      {
        this.Protection.VisibilityState ? byte.MaxValue : (byte) 0
      });
    }

    private string GetString(BinaryReader br, uint size)
    {
      return this.GetString(br, size, Encoding.GetEncoding(this.CodePage));
    }

    private string GetString(BinaryReader br, uint size, Encoding enc)
    {
      if (size <= 0U)
        return "";
      byte[] numArray = new byte[(IntPtr) size];
      byte[] bytes = br.ReadBytes((int) size);
      return enc.GetString(bytes);
    }

    private string GetUnicodeString(BinaryReader br, uint size)
    {
      string str1 = this.GetString(br, size);
      int num = (int) br.ReadUInt16();
      uint size1 = br.ReadUInt32();
      string str2 = this.GetString(br, size1, Encoding.Unicode);
      return str2.Length != 0 ? str2 : str1;
    }

    internal CompoundDocument Document { get; set; }

    internal ZipPackagePart Part { get; set; }

    internal Uri Uri { get; private set; }

    internal void Create()
    {
      if (this.Lcid > 0)
        throw new InvalidOperationException("Package already contains a VBAProject");
      this.ProjectID = "{5DD90D76-4904-47A2-AF0D-D69B4673604E}";
      this.Name = "VBAProject";
      this.SystemKind = ExcelVbaProject.eSyskind.Win32;
      this.Lcid = 1033;
      this.LcidInvoke = 1033;
      this.CodePage = Encoding.Default.CodePage;
      this.MajorVersion = 1361024421;
      this.MinorVersion = 6;
      this.HelpContextID = 0;
      this.Modules.Add(new ExcelVBAModule(new ModuleNameChange(this._wb.CodeNameChange))
      {
        Name = "ThisWorkbook",
        Code = "",
        Attributes = this.GetDocumentAttributes("ThisWorkbook", "0{00020819-0000-0000-C000-000000000046}"),
        Type = eModuleType.Document,
        HelpContext = 0
      });
      foreach (ExcelWorksheet worksheet in this._wb.Worksheets)
      {
        string nameFromWorksheet = this.GetModuleNameFromWorksheet(worksheet);
        if (!this.Modules.Exists(nameFromWorksheet))
          this.Modules.Add(new ExcelVBAModule(new ModuleNameChange(worksheet.CodeNameChange))
          {
            Name = nameFromWorksheet,
            Code = "",
            Attributes = this.GetDocumentAttributes(worksheet.Name, "0{00020820-0000-0000-C000-000000000046}"),
            Type = eModuleType.Document,
            HelpContext = 0
          });
      }
      this._protection = new ExcelVbaProtection(this)
      {
        UserProtected = false,
        HostProtected = false,
        VbeProtected = false,
        VisibilityState = true
      };
    }

    internal string GetModuleNameFromWorksheet(ExcelWorksheet sheet)
    {
      string nameFromWorksheet = sheet.Name;
      if (nameFromWorksheet.Any<char>((Func<char, bool>) (c => c > 'ÿ')) || this.Modules[nameFromWorksheet] != null)
      {
        int positionId = sheet.PositionID;
        nameFromWorksheet = "Sheet" + positionId.ToString();
        while (this.Modules[nameFromWorksheet] != null)
          nameFromWorksheet = "Sheet" + (++positionId).ToString();
      }
      return nameFromWorksheet;
    }

    internal ExcelVbaModuleAttributesCollection GetDocumentAttributes(string name, string clsid)
    {
      ExcelVbaModuleAttributesCollection documentAttributes = new ExcelVbaModuleAttributesCollection();
      documentAttributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Name",
        Value = name,
        DataType = eAttributeDataType.String
      });
      documentAttributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Base",
        Value = clsid,
        DataType = eAttributeDataType.String
      });
      documentAttributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_GlobalNameSpace",
        Value = "False",
        DataType = eAttributeDataType.NonString
      });
      documentAttributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Creatable",
        Value = "False",
        DataType = eAttributeDataType.NonString
      });
      documentAttributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_PredeclaredId",
        Value = "True",
        DataType = eAttributeDataType.NonString
      });
      documentAttributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Exposed",
        Value = "False",
        DataType = eAttributeDataType.NonString
      });
      documentAttributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_TemplateDerived",
        Value = "False",
        DataType = eAttributeDataType.NonString
      });
      documentAttributes._list.Add(new ExcelVbaModuleAttribute()
      {
        Name = "VB_Customizable",
        Value = "True",
        DataType = eAttributeDataType.NonString
      });
      return documentAttributes;
    }

    public void Remove()
    {
      if (this.Part == null)
        return;
      foreach (ZipPackageRelationship relationship in this.Part.GetRelationships())
        this._pck.DeleteRelationship(relationship.Id);
      if (this._pck.PartExists(this.Uri))
        this._pck.DeletePart(this.Uri);
      this.Part = (ZipPackagePart) null;
      this.Modules.Clear();
      this.References.Clear();
      this.Lcid = 0;
      this.LcidInvoke = 0;
      this.CodePage = 0;
      this.MajorVersion = 0;
      this.MinorVersion = 0;
      this.HelpContextID = 0;
    }

    public override string ToString() => this.Name;

    public enum eSyskind
    {
      Win16,
      Win32,
      Macintosh,
      Win64,
    }
  }
}
