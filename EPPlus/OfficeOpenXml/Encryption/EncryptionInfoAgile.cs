// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Encryption.EncryptionInfoAgile
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Encryption
{
  internal class EncryptionInfoAgile : EncryptionInfo
  {
    private XmlNamespaceManager _nsm;

    public EncryptionInfoAgile()
    {
      this._nsm = new XmlNamespaceManager((XmlNameTable) new NameTable());
      this._nsm.AddNamespace("d", "http://schemas.microsoft.com/office/2006/encryption");
      this._nsm.AddNamespace("c", "http://schemas.microsoft.com/office/2006/keyEncryptor/certificate");
      this._nsm.AddNamespace("p", "http://schemas.microsoft.com/office/2006/keyEncryptor/password");
    }

    internal EncryptionInfoAgile.EncryptionDataIntegrity DataIntegrity { get; set; }

    internal EncryptionInfoAgile.EncryptionKeyData KeyData { get; set; }

    internal List<EncryptionInfoAgile.EncryptionKeyEncryptor> KeyEncryptors { get; private set; }

    internal XmlDocument Xml { get; set; }

    internal override void Read(byte[] data)
    {
      byte[] numArray = new byte[data.Length - 8];
      Array.Copy((Array) data, 8, (Array) numArray, 0, data.Length - 8);
      this.ReadFromXml(Encoding.UTF8.GetString(numArray));
    }

    internal void ReadFromXml(string xml)
    {
      this.Xml = new XmlDocument();
      XmlHelper.LoadXmlSafe(this.Xml, xml, Encoding.UTF8);
      this.KeyData = new EncryptionInfoAgile.EncryptionKeyData(this._nsm, this.Xml.SelectSingleNode("/d:encryption/d:keyData", this._nsm));
      this.DataIntegrity = new EncryptionInfoAgile.EncryptionDataIntegrity(this._nsm, this.Xml.SelectSingleNode("/d:encryption/d:dataIntegrity", this._nsm));
      this.KeyEncryptors = new List<EncryptionInfoAgile.EncryptionKeyEncryptor>();
      XmlNodeList xmlNodeList = this.Xml.SelectNodes("/d:encryption/d:keyEncryptors/d:keyEncryptor/p:encryptedKey", this._nsm);
      if (xmlNodeList == null)
        return;
      foreach (XmlNode topNode in xmlNodeList)
        this.KeyEncryptors.Add(new EncryptionInfoAgile.EncryptionKeyEncryptor(this._nsm, topNode));
    }

    internal class EncryptionKeyData(XmlNamespaceManager nsm, XmlNode topNode) : XmlHelper(nsm, topNode)
    {
      internal byte[] SaltValue
      {
        get
        {
          string xmlNodeString = this.GetXmlNodeString("@saltValue");
          return !string.IsNullOrEmpty(xmlNodeString) ? Convert.FromBase64String(xmlNodeString) : (byte[]) null;
        }
        set => this.SetXmlNodeString("@saltValue", Convert.ToBase64String(value));
      }

      internal eHashAlogorithm HashAlgorithm
      {
        get => this.GetHashAlgorithm(this.GetXmlNodeString("@hashAlgorithm"));
        set => this.SetXmlNodeString("@hashAlgorithm", this.GetHashAlgorithmString(value));
      }

      private eHashAlogorithm GetHashAlgorithm(string v)
      {
        switch (v)
        {
          case "RIPEMD-128":
            return eHashAlogorithm.RIPEMD128;
          case "RIPEMD-160":
            return eHashAlogorithm.RIPEMD160;
          case "SHA-1":
            return eHashAlogorithm.SHA1;
          default:
            try
            {
              return (eHashAlogorithm) Enum.Parse(typeof (eHashAlogorithm), v);
            }
            catch
            {
              throw new InvalidDataException("Invalid Hash algorithm");
            }
        }
      }

      private string GetHashAlgorithmString(eHashAlogorithm value)
      {
        switch (value)
        {
          case eHashAlogorithm.SHA1:
            return "SHA-1";
          case eHashAlogorithm.RIPEMD128:
            return "RIPEMD-128";
          case eHashAlogorithm.RIPEMD160:
            return "RIPEMD-160";
          default:
            return value.ToString();
        }
      }

      internal eChainingMode ChiptherChaining
      {
        get
        {
          string xmlNodeString = this.GetXmlNodeString("@cipherChaining");
          try
          {
            return (eChainingMode) Enum.Parse(typeof (eChainingMode), xmlNodeString);
          }
          catch
          {
            throw new InvalidDataException("Invalid chaining mode");
          }
        }
        set => this.SetXmlNodeString("@cipherChaining", value.ToString());
      }

      internal eCipherAlgorithm CipherAlgorithm
      {
        get => this.GetCipherAlgorithm(this.GetXmlNodeString("@cipherAlgorithm"));
        set => this.SetXmlNodeString("@cipherAlgorithm", this.GetCipherAlgorithmString(value));
      }

      private eCipherAlgorithm GetCipherAlgorithm(string v)
      {
        switch (v)
        {
          case "3DES":
            return eCipherAlgorithm.TRIPLE_DES;
          case "3DES_112":
            return eCipherAlgorithm.TRIPLE_DES_112;
          default:
            try
            {
              return (eCipherAlgorithm) Enum.Parse(typeof (eCipherAlgorithm), v);
            }
            catch
            {
              throw new InvalidDataException("Invalid Hash algorithm");
            }
        }
      }

      private string GetCipherAlgorithmString(eCipherAlgorithm alg)
      {
        switch (alg)
        {
          case eCipherAlgorithm.TRIPLE_DES:
            return "3DES";
          case eCipherAlgorithm.TRIPLE_DES_112:
            return "3DES_112";
          default:
            return alg.ToString();
        }
      }

      internal int HashSize
      {
        get => this.GetXmlNodeInt("@hashSize");
        set => this.SetXmlNodeString("@hashSize", value.ToString());
      }

      internal int KeyBits
      {
        get => this.GetXmlNodeInt("@keyBits");
        set => this.SetXmlNodeString("@keyBits", value.ToString());
      }

      internal int BlockSize
      {
        get => this.GetXmlNodeInt("@blockSize");
        set => this.SetXmlNodeString("@blockSize", value.ToString());
      }

      internal int SaltSize
      {
        get => this.GetXmlNodeInt("@saltSize");
        set => this.SetXmlNodeString("@saltSize", value.ToString());
      }
    }

    internal class EncryptionDataIntegrity(XmlNamespaceManager nsm, XmlNode topNode) : XmlHelper(nsm, topNode)
    {
      internal byte[] EncryptedHmacValue
      {
        get
        {
          string xmlNodeString = this.GetXmlNodeString("@encryptedHmacValue");
          return !string.IsNullOrEmpty(xmlNodeString) ? Convert.FromBase64String(xmlNodeString) : (byte[]) null;
        }
        set => this.SetXmlNodeString("@encryptedHmacValue", Convert.ToBase64String(value));
      }

      internal byte[] EncryptedHmacKey
      {
        get
        {
          string xmlNodeString = this.GetXmlNodeString("@encryptedHmacKey");
          return !string.IsNullOrEmpty(xmlNodeString) ? Convert.FromBase64String(xmlNodeString) : (byte[]) null;
        }
        set => this.SetXmlNodeString("@encryptedHmacKey", Convert.ToBase64String(value));
      }
    }

    internal class EncryptionKeyEncryptor(XmlNamespaceManager nsm, XmlNode topNode) : 
      EncryptionInfoAgile.EncryptionKeyData(nsm, topNode)
    {
      internal byte[] EncryptedKeyValue
      {
        get
        {
          string xmlNodeString = this.GetXmlNodeString("@encryptedKeyValue");
          return !string.IsNullOrEmpty(xmlNodeString) ? Convert.FromBase64String(xmlNodeString) : (byte[]) null;
        }
        set => this.SetXmlNodeString("@encryptedKeyValue", Convert.ToBase64String(value));
      }

      internal byte[] EncryptedVerifierHash
      {
        get
        {
          string xmlNodeString = this.GetXmlNodeString("@encryptedVerifierHashValue");
          return !string.IsNullOrEmpty(xmlNodeString) ? Convert.FromBase64String(xmlNodeString) : (byte[]) null;
        }
        set => this.SetXmlNodeString("@encryptedVerifierHashValue", Convert.ToBase64String(value));
      }

      internal byte[] EncryptedVerifierHashInput
      {
        get
        {
          string xmlNodeString = this.GetXmlNodeString("@encryptedVerifierHashInput");
          return !string.IsNullOrEmpty(xmlNodeString) ? Convert.FromBase64String(xmlNodeString) : (byte[]) null;
        }
        set => this.SetXmlNodeString("@encryptedVerifierHashInput", Convert.ToBase64String(value));
      }

      internal byte[] VerifierHashInput { get; set; }

      internal byte[] VerifierHash { get; set; }

      internal byte[] KeyValue { get; set; }

      internal int SpinCount
      {
        get => this.GetXmlNodeInt("@spinCount");
        set => this.SetXmlNodeString("@spinCount", value.ToString());
      }
    }
  }
}
