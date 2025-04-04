// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.VBA.ExcelVbaSignature
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#nullable disable
namespace OfficeOpenXml.VBA
{
  public class ExcelVbaSignature
  {
    private const string schemaRelVbaSignature = "http://schemas.microsoft.com/office/2006/relationships/vbaProjectSignature";
    private ZipPackagePart _vbaPart;

    internal ExcelVbaSignature(ZipPackagePart vbaPart)
    {
      this._vbaPart = vbaPart;
      this.GetSignature();
    }

    private void GetSignature()
    {
      if (this._vbaPart == null)
        return;
      ZipPackageRelationship packageRelationship = this._vbaPart.GetRelationshipsByType("http://schemas.microsoft.com/office/2006/relationships/vbaProjectSignature").FirstOrDefault<ZipPackageRelationship>();
      if (packageRelationship != null)
      {
        this.Uri = UriHelper.ResolvePartUri(packageRelationship.SourceUri, packageRelationship.TargetUri);
        this.Part = this._vbaPart.Package.GetPart(this.Uri);
        BinaryReader binaryReader = new BinaryReader((Stream) this.Part.GetStream());
        uint count1 = binaryReader.ReadUInt32();
        int num1 = (int) binaryReader.ReadUInt32();
        int num2 = (int) binaryReader.ReadUInt32();
        int num3 = (int) binaryReader.ReadUInt32();
        int num4 = (int) binaryReader.ReadUInt32();
        int num5 = (int) binaryReader.ReadUInt32();
        int num6 = (int) binaryReader.ReadUInt32();
        int num7 = (int) binaryReader.ReadUInt32();
        int num8 = (int) binaryReader.ReadUInt32();
        byte[] encodedMessage = binaryReader.ReadBytes((int) count1);
        int num9 = (int) binaryReader.ReadUInt32();
        int num10 = (int) binaryReader.ReadUInt32();
        for (uint index = binaryReader.ReadUInt32(); index != 0U; index = binaryReader.ReadUInt32())
        {
          int num11 = (int) binaryReader.ReadUInt32();
          uint count2 = binaryReader.ReadUInt32();
          if (count2 > 0U)
          {
            byte[] rawData = binaryReader.ReadBytes((int) count2);
            if (index == 32U)
              this.Certificate = new X509Certificate2(rawData);
          }
        }
        int num12 = (int) binaryReader.ReadUInt32();
        int num13 = (int) binaryReader.ReadUInt32();
        int num14 = (int) binaryReader.ReadUInt16();
        int num15 = (int) binaryReader.ReadUInt16();
        this.Verifier = new SignedCms();
        this.Verifier.Decode(encodedMessage);
      }
      else
      {
        this.Certificate = (X509Certificate2) null;
        this.Verifier = (SignedCms) null;
      }
    }

    internal void Save(ExcelVbaProject proj)
    {
      if (this.Certificate == null)
        return;
      if (!this.Certificate.HasPrivateKey)
      {
        X509Certificate2 x509Certificate2 = this.GetCertFromStore(StoreLocation.CurrentUser) ?? this.GetCertFromStore(StoreLocation.LocalMachine);
        if (x509Certificate2 != null && x509Certificate2.HasPrivateKey)
        {
          this.Certificate = x509Certificate2;
        }
        else
        {
          foreach (ZipPackageRelationship relationship in this.Part.GetRelationships())
            this.Part.DeleteRelationship(relationship.Id);
          this.Part.Package.DeletePart(this.Part.Uri);
          return;
        }
      }
      MemoryStream output = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      byte[] certStore = this.GetCertStore();
      byte[] buffer = this.SignProject(proj);
      binaryWriter.Write((uint) buffer.Length);
      binaryWriter.Write(44U);
      binaryWriter.Write((uint) certStore.Length);
      binaryWriter.Write((uint) (buffer.Length + 44));
      binaryWriter.Write(0U);
      binaryWriter.Write((uint) (buffer.Length + certStore.Length + 44));
      binaryWriter.Write(0U);
      binaryWriter.Write(0U);
      binaryWriter.Write((uint) (buffer.Length + certStore.Length + 44 + 2));
      binaryWriter.Write(buffer);
      binaryWriter.Write(certStore);
      binaryWriter.Write((ushort) 0);
      binaryWriter.Write((ushort) 0);
      binaryWriter.Write((ushort) 0);
      binaryWriter.Flush();
      ZipPackageRelationship packageRelationship = proj.Part.GetRelationshipsByType("http://schemas.microsoft.com/office/2006/relationships/vbaProjectSignature").FirstOrDefault<ZipPackageRelationship>();
      if (this.Part == null)
      {
        if (packageRelationship != null)
        {
          this.Uri = packageRelationship.TargetUri;
          this.Part = proj._pck.GetPart(packageRelationship.TargetUri);
        }
        else
        {
          this.Uri = new Uri("/xl/vbaProjectSignature.bin", UriKind.Relative);
          this.Part = proj._pck.CreatePart(this.Uri, "application/vnd.ms-office.vbaProjectSignature");
        }
      }
      if (packageRelationship == null)
        proj.Part.CreateRelationship(UriHelper.ResolvePartUri(proj.Uri, this.Uri), TargetMode.Internal, "http://schemas.microsoft.com/office/2006/relationships/vbaProjectSignature");
      byte[] array = output.ToArray();
      this.Part.GetStream(FileMode.Create).Write(array, 0, array.Length);
    }

    private X509Certificate2 GetCertFromStore(StoreLocation loc)
    {
      try
      {
        X509Store x509Store = new X509Store(loc);
        x509Store.Open(OpenFlags.ReadOnly);
        try
        {
          return x509Store.Certificates.Find(X509FindType.FindByThumbprint, (object) this.Certificate.Thumbprint, true).OfType<X509Certificate2>().FirstOrDefault<X509Certificate2>();
        }
        finally
        {
          x509Store.Close();
        }
      }
      catch
      {
        return (X509Certificate2) null;
      }
    }

    private byte[] GetCertStore()
    {
      MemoryStream output = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write(0U);
      binaryWriter.Write(1414677827U);
      byte[] rawData = this.Certificate.RawData;
      binaryWriter.Write(32U);
      binaryWriter.Write(1U);
      binaryWriter.Write((uint) rawData.Length);
      binaryWriter.Write(rawData);
      binaryWriter.Write(0U);
      binaryWriter.Write(0UL);
      binaryWriter.Flush();
      return output.ToArray();
    }

    private void WriteProp(BinaryWriter bw, int id, byte[] data)
    {
      bw.Write((uint) id);
      bw.Write(1U);
      bw.Write((uint) data.Length);
      bw.Write(data);
    }

    internal byte[] SignProject(ExcelVbaProject proj)
    {
      if (!this.Certificate.HasPrivateKey)
      {
        this.Certificate = (X509Certificate2) null;
        return (byte[]) null;
      }
      byte[] contentHash = this.GetContentHash(proj);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) new MemoryStream());
      binaryWriter.Write((byte) 48);
      binaryWriter.Write((byte) 50);
      binaryWriter.Write((byte) 48);
      binaryWriter.Write((byte) 14);
      binaryWriter.Write((byte) 6);
      binaryWriter.Write((byte) 10);
      binaryWriter.Write(new byte[10]
      {
        (byte) 43,
        (byte) 6,
        (byte) 1,
        (byte) 4,
        (byte) 1,
        (byte) 130,
        (byte) 55,
        (byte) 2,
        (byte) 1,
        (byte) 29
      });
      binaryWriter.Write((byte) 4);
      binaryWriter.Write((byte) 0);
      binaryWriter.Write((byte) 48);
      binaryWriter.Write((byte) 32);
      binaryWriter.Write((byte) 48);
      binaryWriter.Write((byte) 12);
      binaryWriter.Write((byte) 6);
      binaryWriter.Write((byte) 8);
      binaryWriter.Write(new byte[8]
      {
        (byte) 42,
        (byte) 134,
        (byte) 72,
        (byte) 134,
        (byte) 247,
        (byte) 13,
        (byte) 2,
        (byte) 5
      });
      binaryWriter.Write((byte) 5);
      binaryWriter.Write((byte) 0);
      binaryWriter.Write((byte) 4);
      binaryWriter.Write((byte) contentHash.Length);
      binaryWriter.Write(contentHash);
      this.Verifier = new SignedCms(new ContentInfo(((MemoryStream) binaryWriter.BaseStream).ToArray())
      {
        ContentType = {
          Value = "1.3.6.1.4.1.311.2.1.4"
        }
      });
      this.Verifier.ComputeSignature(new CmsSigner(this.Certificate), false);
      return this.Verifier.Encode();
    }

    private byte[] GetContentHash(ExcelVbaProject proj)
    {
      Encoding encoding = Encoding.GetEncoding(proj.CodePage);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) new MemoryStream());
      binaryWriter.Write(encoding.GetBytes(proj.Name));
      binaryWriter.Write(encoding.GetBytes(proj.Constants));
      foreach (ExcelVbaReference reference in (ExcelVBACollectionBase<ExcelVbaReference>) proj.References)
      {
        if (reference.ReferenceRecordID == 13)
          binaryWriter.Write((byte) 123);
        if (reference.ReferenceRecordID == 14)
        {
          foreach (byte num in BitConverter.GetBytes((uint) reference.Libid.Length))
          {
            if (num != (byte) 0)
              binaryWriter.Write(num);
            else
              break;
          }
        }
      }
      foreach (ExcelVBAModule module in (ExcelVBACollectionBase<ExcelVBAModule>) proj.Modules)
      {
        string code = module.Code;
        char[] separator = new char[2]{ '\r', '\n' };
        foreach (string s in code.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        {
          if (!s.StartsWith("attribute", true, (CultureInfo) null))
            binaryWriter.Write(encoding.GetBytes(s));
        }
      }
      byte[] array = (binaryWriter.BaseStream as MemoryStream).ToArray();
      return MD5.Create().ComputeHash(array);
    }

    public X509Certificate2 Certificate { get; set; }

    public SignedCms Verifier { get; internal set; }

    internal CompoundDocument Signature { get; set; }

    internal ZipPackagePart Part { get; set; }

    internal Uri Uri { get; private set; }
  }
}
