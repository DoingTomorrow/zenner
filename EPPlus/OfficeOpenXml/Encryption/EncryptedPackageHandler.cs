// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Encryption.EncryptedPackageHandler
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace OfficeOpenXml.Encryption
{
  internal class EncryptedPackageHandler
  {
    private readonly byte[] BlockKey_HashInput = new byte[8]
    {
      (byte) 254,
      (byte) 167,
      (byte) 210,
      (byte) 118,
      (byte) 59,
      (byte) 75,
      (byte) 158,
      (byte) 121
    };
    private readonly byte[] BlockKey_HashValue = new byte[8]
    {
      (byte) 215,
      (byte) 170,
      (byte) 15,
      (byte) 109,
      (byte) 48,
      (byte) 97,
      (byte) 52,
      (byte) 78
    };
    private readonly byte[] BlockKey_KeyValue = new byte[8]
    {
      (byte) 20,
      (byte) 110,
      (byte) 11,
      (byte) 231,
      (byte) 171,
      (byte) 172,
      (byte) 208,
      (byte) 214
    };
    private readonly byte[] BlockKey_HmacKey = new byte[8]
    {
      (byte) 95,
      (byte) 178,
      (byte) 173,
      (byte) 1,
      (byte) 12,
      (byte) 185,
      (byte) 225,
      (byte) 246
    };
    private readonly byte[] BlockKey_HmacValue = new byte[8]
    {
      (byte) 160,
      (byte) 103,
      (byte) 127,
      (byte) 2,
      (byte) 178,
      (byte) 44,
      (byte) 132,
      (byte) 51
    };

    internal MemoryStream DecryptPackage(FileInfo fi, ExcelEncryption encryption)
    {
      CompoundDocument doc = new CompoundDocument(fi);
      if (CompoundDocument.IsStorageFile(fi.FullName) == 0)
        return this.GetStreamFromPackage(doc, encryption);
      throw new InvalidDataException(string.Format("File {0} is not an encrypted package", (object) fi.FullName));
    }

    internal MemoryStream DecryptPackage(MemoryStream stream, ExcelEncryption encryption)
    {
      CompoundDocument.ILockBytes lockBytes = (CompoundDocument.ILockBytes) null;
      try
      {
        lockBytes = CompoundDocument.GetLockbyte(stream);
        if (CompoundDocument.IsStorageILockBytes(lockBytes) == 0)
          return this.GetStreamFromPackage(new CompoundDocument(lockBytes), encryption);
        Marshal.ReleaseComObject((object) lockBytes);
        throw new InvalidDataException("The stream is not an valid/supported encrypted document.");
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        Marshal.ReleaseComObject((object) lockBytes);
      }
    }

    internal MemoryStream EncryptPackage(byte[] package, ExcelEncryption encryption)
    {
      if (encryption.Version == EncryptionVersion.Standard)
        return this.EncryptPackageBinary(package, encryption);
      return encryption.Version == EncryptionVersion.Agile ? this.EncryptPackageAgile(package, encryption) : throw new ArgumentException("Unsupported encryption version.");
    }

    private MemoryStream EncryptPackageAgile(byte[] package, ExcelEncryption encryption)
    {
      string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" + "<encryption xmlns=\"http://schemas.microsoft.com/office/2006/encryption\" xmlns:p=\"http://schemas.microsoft.com/office/2006/keyEncryptor/password\" xmlns:c=\"http://schemas.microsoft.com/office/2006/keyEncryptor/certificate\">" + "<keyData saltSize=\"16\" blockSize=\"16\" keyBits=\"256\" hashSize=\"64\" cipherAlgorithm=\"AES\" cipherChaining=\"ChainingModeCBC\" hashAlgorithm=\"SHA512\" saltValue=\"\"/>" + "<dataIntegrity encryptedHmacKey=\"\" encryptedHmacValue=\"\"/>" + "<keyEncryptors>" + "<keyEncryptor uri=\"http://schemas.microsoft.com/office/2006/keyEncryptor/password\">" + "<p:encryptedKey spinCount=\"100000\" saltSize=\"16\" blockSize=\"16\" keyBits=\"256\" hashSize=\"64\" cipherAlgorithm=\"AES\" cipherChaining=\"ChainingModeCBC\" hashAlgorithm=\"SHA512\" saltValue=\"\" encryptedVerifierHashInput=\"\" encryptedVerifierHashValue=\"\" encryptedKeyValue=\"\" />" + "</keyEncryptor></keyEncryptors></encryption>";
      EncryptionInfoAgile encryptionInfoAgile = new EncryptionInfoAgile();
      encryptionInfoAgile.ReadFromXml(xml);
      EncryptionInfoAgile.EncryptionKeyEncryptor keyEncryptor = encryptionInfoAgile.KeyEncryptors[0];
      RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
      byte[] data1 = new byte[16];
      randomNumberGenerator.GetBytes(data1);
      encryptionInfoAgile.KeyData.SaltValue = data1;
      randomNumberGenerator.GetBytes(data1);
      keyEncryptor.SaltValue = data1;
      keyEncryptor.KeyValue = new byte[keyEncryptor.KeyBits / 8];
      randomNumberGenerator.GetBytes(keyEncryptor.KeyValue);
      HashAlgorithm hashProvider = this.GetHashProvider(encryptionInfoAgile.KeyEncryptors[0]);
      byte[] passwordHash = this.GetPasswordHash(hashProvider, keyEncryptor.SaltValue, encryption.Password, keyEncryptor.SpinCount, keyEncryptor.HashSize);
      this.FixHashSize(this.GetFinalHash(hashProvider, keyEncryptor, this.BlockKey_KeyValue, passwordHash), keyEncryptor.KeyBits / 8);
      byte[] data2 = this.EncryptDataAgile(package, encryptionInfoAgile, hashProvider);
      byte[] numArray = new byte[64];
      randomNumberGenerator.GetBytes(numArray);
      this.SetHMAC(encryptionInfoAgile, hashProvider, numArray, data2);
      keyEncryptor.VerifierHashInput = new byte[16];
      randomNumberGenerator.GetBytes(keyEncryptor.VerifierHashInput);
      keyEncryptor.VerifierHash = hashProvider.ComputeHash(keyEncryptor.VerifierHashInput);
      byte[] finalHash1 = this.GetFinalHash(hashProvider, keyEncryptor, this.BlockKey_HashInput, passwordHash);
      byte[] finalHash2 = this.GetFinalHash(hashProvider, keyEncryptor, this.BlockKey_HashValue, passwordHash);
      byte[] finalHash3 = this.GetFinalHash(hashProvider, keyEncryptor, this.BlockKey_KeyValue, passwordHash);
      MemoryStream ms1 = new MemoryStream();
      this.EncryptAgileFromKey(keyEncryptor, finalHash1, keyEncryptor.VerifierHashInput, 0L, (long) keyEncryptor.VerifierHashInput.Length, keyEncryptor.SaltValue, ms1);
      keyEncryptor.EncryptedVerifierHashInput = ms1.ToArray();
      MemoryStream ms2 = new MemoryStream();
      this.EncryptAgileFromKey(keyEncryptor, finalHash2, keyEncryptor.VerifierHash, 0L, (long) keyEncryptor.VerifierHash.Length, keyEncryptor.SaltValue, ms2);
      keyEncryptor.EncryptedVerifierHash = ms2.ToArray();
      MemoryStream ms3 = new MemoryStream();
      this.EncryptAgileFromKey(keyEncryptor, finalHash3, keyEncryptor.KeyValue, 0L, (long) keyEncryptor.KeyValue.Length, keyEncryptor.SaltValue, ms3);
      keyEncryptor.EncryptedKeyValue = ms3.ToArray();
      byte[] bytes = Encoding.UTF8.GetBytes(encryptionInfoAgile.Xml.OuterXml);
      MemoryStream memoryStream1 = new MemoryStream();
      memoryStream1.Write(BitConverter.GetBytes((ushort) 4), 0, 2);
      memoryStream1.Write(BitConverter.GetBytes((ushort) 4), 0, 2);
      memoryStream1.Write(BitConverter.GetBytes(64U), 0, 4);
      memoryStream1.Write(bytes, 0, bytes.Length);
      CompoundDocument doc = new CompoundDocument();
      this.CreateDataSpaces(doc);
      doc.Storage.DataStreams.Add("EncryptionInfo", memoryStream1.ToArray());
      doc.Storage.DataStreams.Add("EncryptedPackage", data2);
      MemoryStream memoryStream2 = new MemoryStream();
      byte[] buffer = doc.Save();
      memoryStream2.Write(buffer, 0, buffer.Length);
      return memoryStream2;
    }

    private byte[] EncryptDataAgile(
      byte[] data,
      EncryptionInfoAgile encryptionInfo,
      HashAlgorithm hashProvider)
    {
      EncryptionInfoAgile.EncryptionKeyEncryptor keyEncryptor = encryptionInfo.KeyEncryptors[0];
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.KeySize = keyEncryptor.KeyBits;
      rijndaelManaged.Mode = CipherMode.CBC;
      rijndaelManaged.Padding = PaddingMode.Zeros;
      int pos = 0;
      int num = 0;
      MemoryStream ms = new MemoryStream();
      ms.Write(BitConverter.GetBytes(data.LongLength), 0, 8);
      while (pos < data.Length)
      {
        int size = data.Length - pos > 4096 ? 4096 : data.Length - pos;
        byte[] numArray = new byte[4 + encryptionInfo.KeyData.SaltSize];
        Array.Copy((Array) encryptionInfo.KeyData.SaltValue, 0, (Array) numArray, 0, encryptionInfo.KeyData.SaltSize);
        Array.Copy((Array) BitConverter.GetBytes(num), 0, (Array) numArray, encryptionInfo.KeyData.SaltSize, 4);
        byte[] hash = hashProvider.ComputeHash(numArray);
        this.EncryptAgileFromKey(keyEncryptor, keyEncryptor.KeyValue, data, (long) pos, (long) size, hash, ms);
        pos += size;
        ++num;
      }
      ms.Flush();
      return ms.ToArray();
    }

    private void SetHMAC(
      EncryptionInfoAgile ei,
      HashAlgorithm hashProvider,
      byte[] salt,
      byte[] data)
    {
      byte[] finalHash1 = this.GetFinalHash(hashProvider, ei.KeyEncryptors[0], this.BlockKey_HmacKey, ei.KeyData.SaltValue);
      MemoryStream ms1 = new MemoryStream();
      this.EncryptAgileFromKey(ei.KeyEncryptors[0], ei.KeyEncryptors[0].KeyValue, salt, 0L, salt.LongLength, finalHash1, ms1);
      ei.DataIntegrity.EncryptedHmacKey = ms1.ToArray();
      byte[] hash = this.GetHmacProvider(ei.KeyEncryptors[0], salt).ComputeHash(data);
      MemoryStream ms2 = new MemoryStream();
      byte[] finalHash2 = this.GetFinalHash(hashProvider, ei.KeyEncryptors[0], this.BlockKey_HmacValue, ei.KeyData.SaltValue);
      this.EncryptAgileFromKey(ei.KeyEncryptors[0], ei.KeyEncryptors[0].KeyValue, hash, 0L, hash.LongLength, finalHash2, ms2);
      ei.DataIntegrity.EncryptedHmacValue = ms2.ToArray();
    }

    private HMAC GetHmacProvider(EncryptionInfoAgile.EncryptionKeyEncryptor ei, byte[] salt)
    {
      switch (ei.HashAlgorithm)
      {
        case eHashAlogorithm.SHA1:
          return (HMAC) new HMACSHA1(salt);
        case eHashAlogorithm.SHA256:
          return (HMAC) new HMACSHA256(salt);
        case eHashAlogorithm.SHA384:
          return (HMAC) new HMACSHA384(salt);
        case eHashAlogorithm.SHA512:
          return (HMAC) new HMACSHA512(salt);
        case eHashAlogorithm.MD5:
          return (HMAC) new HMACMD5(salt);
        case eHashAlogorithm.RIPEMD160:
          return (HMAC) new HMACRIPEMD160(salt);
        default:
          throw new NotSupportedException(string.Format("Hash method {0} not supported.", (object) ei.HashAlgorithm));
      }
    }

    private MemoryStream EncryptPackageBinary(byte[] package, ExcelEncryption encryption)
    {
      byte[] key;
      EncryptionInfoBinary encryptionInfo = this.CreateEncryptionInfo(encryption.Password, encryption.Algorithm == EncryptionAlgorithm.AES128 ? AlgorithmID.AES128 : (encryption.Algorithm == EncryptionAlgorithm.AES192 ? AlgorithmID.AES192 : AlgorithmID.AES256), out key);
      CompoundDocument doc = new CompoundDocument();
      this.CreateDataSpaces(doc);
      doc.Storage.DataStreams.Add("EncryptionInfo", encryptionInfo.WriteBinary());
      byte[] buffer1 = this.EncryptData(key, package, false);
      MemoryStream memoryStream1 = new MemoryStream();
      memoryStream1.Write(BitConverter.GetBytes((ulong) package.LongLength), 0, 8);
      memoryStream1.Write(buffer1, 0, buffer1.Length);
      doc.Storage.DataStreams.Add("EncryptedPackage", memoryStream1.ToArray());
      MemoryStream memoryStream2 = new MemoryStream();
      byte[] buffer2 = doc.Save();
      memoryStream2.Write(buffer2, 0, buffer2.Length);
      return memoryStream2;
    }

    private void CreateDataSpaces(CompoundDocument doc)
    {
      CompoundDocument.StoragePart storagePart1 = new CompoundDocument.StoragePart();
      doc.Storage.SubStorage.Add("\u0006DataSpaces", storagePart1);
      CompoundDocument.StoragePart storagePart2 = new CompoundDocument.StoragePart();
      storagePart1.DataStreams.Add("Version", this.CreateVersionStream());
      storagePart1.DataStreams.Add("DataSpaceMap", this.CreateDataSpaceMap());
      CompoundDocument.StoragePart storagePart3 = new CompoundDocument.StoragePart();
      storagePart1.SubStorage.Add("DataSpaceInfo", storagePart3);
      storagePart3.DataStreams.Add("StrongEncryptionDataSpace", this.CreateStrongEncryptionDataSpaceStream());
      CompoundDocument.StoragePart storagePart4 = new CompoundDocument.StoragePart();
      storagePart1.SubStorage.Add("TransformInfo", storagePart4);
      CompoundDocument.StoragePart storagePart5 = new CompoundDocument.StoragePart();
      storagePart4.SubStorage.Add("StrongEncryptionTransform", storagePart5);
      storagePart5.DataStreams.Add("\u0006Primary", this.CreateTransformInfoPrimary());
    }

    private byte[] CreateStrongEncryptionDataSpaceStream()
    {
      MemoryStream output = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write(8);
      binaryWriter.Write(1);
      string str = "StrongEncryptionTransform";
      binaryWriter.Write(str.Length * 2);
      binaryWriter.Write(Encoding.Unicode.GetBytes(str + "\0"));
      binaryWriter.Flush();
      return output.ToArray();
    }

    private byte[] CreateVersionStream()
    {
      MemoryStream output = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write((short) 60);
      binaryWriter.Write((short) 0);
      binaryWriter.Write(Encoding.Unicode.GetBytes("Microsoft.Container.DataSpaces"));
      binaryWriter.Write(1);
      binaryWriter.Write(1);
      binaryWriter.Write(1);
      binaryWriter.Flush();
      return output.ToArray();
    }

    private byte[] CreateDataSpaceMap()
    {
      MemoryStream output = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write(8);
      binaryWriter.Write(1);
      string s = "EncryptedPackage";
      string str = "StrongEncryptionDataSpace";
      binaryWriter.Write((s.Length + str.Length) * 2 + 22);
      binaryWriter.Write(1);
      binaryWriter.Write(0);
      binaryWriter.Write(s.Length * 2);
      binaryWriter.Write(Encoding.Unicode.GetBytes(s));
      binaryWriter.Write(str.Length * 2);
      binaryWriter.Write(Encoding.Unicode.GetBytes(str + "\0"));
      binaryWriter.Flush();
      return output.ToArray();
    }

    private byte[] CreateTransformInfoPrimary()
    {
      MemoryStream output = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      string s = "{FF9A3F03-56EF-4613-BDD5-5A41C1D07246}";
      string str = "Microsoft.Container.EncryptionTransform";
      binaryWriter.Write(s.Length * 2 + 12);
      binaryWriter.Write(1);
      binaryWriter.Write(s.Length * 2);
      binaryWriter.Write(Encoding.Unicode.GetBytes(s));
      binaryWriter.Write(str.Length * 2);
      binaryWriter.Write(Encoding.Unicode.GetBytes(str + "\0"));
      binaryWriter.Write(1);
      binaryWriter.Write(1);
      binaryWriter.Write(1);
      binaryWriter.Write(0);
      binaryWriter.Write(0);
      binaryWriter.Write(0);
      binaryWriter.Write(4);
      binaryWriter.Flush();
      return output.ToArray();
    }

    private EncryptionInfoBinary CreateEncryptionInfo(
      string password,
      AlgorithmID algID,
      out byte[] key)
    {
      if (algID == AlgorithmID.Flags || algID == AlgorithmID.RC4)
        throw new ArgumentException("algID must be AES128, AES192 or AES256");
      EncryptionInfoBinary encryptionInfo = new EncryptionInfoBinary();
      encryptionInfo.MajorVersion = (short) 4;
      encryptionInfo.MinorVersion = (short) 2;
      encryptionInfo.Flags = Flags.fCryptoAPI | Flags.fAES;
      encryptionInfo.Header = new EncryptionHeader();
      encryptionInfo.Header.AlgID = algID;
      encryptionInfo.Header.AlgIDHash = AlgorithmHashID.SHA1;
      encryptionInfo.Header.Flags = encryptionInfo.Flags;
      EncryptionHeader header = encryptionInfo.Header;
      int num;
      switch (algID)
      {
        case AlgorithmID.AES128:
          num = 128;
          break;
        case AlgorithmID.AES192:
          num = 192;
          break;
        default:
          num = 256;
          break;
      }
      header.KeySize = num;
      encryptionInfo.Header.ProviderType = ProviderType.AES;
      encryptionInfo.Header.CSPName = "Microsoft Enhanced RSA and AES Cryptographic Provider\0";
      encryptionInfo.Header.Reserved1 = 0;
      encryptionInfo.Header.Reserved2 = 0;
      encryptionInfo.Header.SizeExtra = 0;
      encryptionInfo.Verifier = new EncryptionVerifier();
      encryptionInfo.Verifier.Salt = new byte[16];
      RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
      randomNumberGenerator.GetBytes(encryptionInfo.Verifier.Salt);
      encryptionInfo.Verifier.SaltSize = 16U;
      key = this.GetPasswordHashBinary(password, encryptionInfo);
      byte[] numArray = new byte[16];
      randomNumberGenerator.GetBytes(numArray);
      encryptionInfo.Verifier.EncryptedVerifier = this.EncryptData(key, numArray, true);
      encryptionInfo.Verifier.VerifierHashSize = 32U;
      byte[] hash = new SHA1Managed().ComputeHash(numArray);
      encryptionInfo.Verifier.EncryptedVerifierHash = this.EncryptData(key, hash, false);
      return encryptionInfo;
    }

    private byte[] EncryptData(byte[] key, byte[] data, bool useDataSize)
    {
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.KeySize = key.Length * 8;
      rijndaelManaged.Mode = CipherMode.ECB;
      rijndaelManaged.Padding = PaddingMode.Zeros;
      ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(key, (byte[]) null);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(data, 0, data.Length);
      cryptoStream.FlushFinalBlock();
      if (!useDataSize)
        return memoryStream.ToArray();
      byte[] buffer = new byte[data.Length];
      memoryStream.Seek(0L, SeekOrigin.Begin);
      memoryStream.Read(buffer, 0, data.Length);
      return buffer;
    }

    private MemoryStream GetStreamFromPackage(CompoundDocument doc, ExcelEncryption encryption)
    {
      MemoryStream memoryStream = new MemoryStream();
      if (!doc.Storage.DataStreams.ContainsKey("EncryptionInfo") && !doc.Storage.DataStreams.ContainsKey("EncryptedPackage"))
        throw new InvalidDataException("Invalid document. EncryptionInfo or EncryptedPackage stream is missing");
      EncryptionInfo encryptionInfo = EncryptionInfo.ReadBinary(doc.Storage.DataStreams["EncryptionInfo"]);
      return this.DecryptDocument(doc.Storage.DataStreams["EncryptedPackage"], encryptionInfo, encryption.Password);
    }

    private MemoryStream DecryptDocument(
      byte[] data,
      EncryptionInfo encryptionInfo,
      string password)
    {
      long int64 = BitConverter.ToInt64(data, 0);
      byte[] numArray = new byte[data.Length - 8];
      Array.Copy((Array) data, 8, (Array) numArray, 0, numArray.Length);
      return encryptionInfo is EncryptionInfoBinary ? this.DecryptBinary((EncryptionInfoBinary) encryptionInfo, password, int64, numArray) : this.DecryptAgile((EncryptionInfoAgile) encryptionInfo, password, int64, numArray, data);
    }

    private MemoryStream DecryptAgile(
      EncryptionInfoAgile encryptionInfo,
      string password,
      long size,
      byte[] encryptedData,
      byte[] data)
    {
      MemoryStream memoryStream = new MemoryStream();
      if (encryptionInfo.KeyData.CipherAlgorithm != eCipherAlgorithm.AES)
        return (MemoryStream) null;
      EncryptionInfoAgile.EncryptionKeyEncryptor keyEncryptor = encryptionInfo.KeyEncryptors[0];
      HashAlgorithm hashProvider = this.GetHashProvider(keyEncryptor);
      byte[] passwordHash = this.GetPasswordHash(hashProvider, keyEncryptor.SaltValue, password, keyEncryptor.SpinCount, keyEncryptor.HashSize);
      byte[] finalHash1 = this.GetFinalHash(hashProvider, keyEncryptor, this.BlockKey_HashInput, passwordHash);
      byte[] finalHash2 = this.GetFinalHash(hashProvider, keyEncryptor, this.BlockKey_HashValue, passwordHash);
      byte[] finalHash3 = this.GetFinalHash(hashProvider, keyEncryptor, this.BlockKey_KeyValue, passwordHash);
      keyEncryptor.VerifierHashInput = this.DecryptAgileFromKey(keyEncryptor, finalHash1, keyEncryptor.EncryptedVerifierHashInput, (long) keyEncryptor.SaltSize, keyEncryptor.SaltValue);
      keyEncryptor.VerifierHash = this.DecryptAgileFromKey(keyEncryptor, finalHash2, keyEncryptor.EncryptedVerifierHash, (long) keyEncryptor.HashSize, keyEncryptor.SaltValue);
      keyEncryptor.KeyValue = this.DecryptAgileFromKey(keyEncryptor, finalHash3, keyEncryptor.EncryptedKeyValue, (long) (keyEncryptor.KeyBits / 8), keyEncryptor.SaltValue);
      if (!this.IsPasswordValid(hashProvider, keyEncryptor))
        throw new UnauthorizedAccessException("Invalid password");
      byte[] finalHash4 = this.GetFinalHash(hashProvider, keyEncryptor, this.BlockKey_HmacKey, encryptionInfo.KeyData.SaltValue);
      byte[] salt = this.DecryptAgileFromKey(keyEncryptor, keyEncryptor.KeyValue, encryptionInfo.DataIntegrity.EncryptedHmacKey, (long) encryptionInfo.KeyData.HashSize, finalHash4);
      byte[] finalHash5 = this.GetFinalHash(hashProvider, keyEncryptor, this.BlockKey_HmacValue, encryptionInfo.KeyData.SaltValue);
      byte[] numArray1 = this.DecryptAgileFromKey(keyEncryptor, keyEncryptor.KeyValue, encryptionInfo.DataIntegrity.EncryptedHmacValue, (long) encryptionInfo.KeyData.HashSize, finalHash5);
      byte[] hash1 = this.GetHmacProvider(keyEncryptor, salt).ComputeHash(data);
      for (int index = 0; index < hash1.Length; ++index)
      {
        if ((int) numArray1[index] != (int) hash1[index])
          throw new Exception("Dataintegrity key missmatch");
      }
      int sourceIndex = 0;
      uint num = 0;
      while ((long) sourceIndex < size)
      {
        int size1 = size - (long) sourceIndex > 4096L ? 4096 : (int) (size - (long) sourceIndex);
        int length = encryptedData.Length - sourceIndex > 4096 ? 4096 : encryptedData.Length - sourceIndex;
        byte[] numArray2 = new byte[4 + encryptionInfo.KeyData.SaltSize];
        Array.Copy((Array) encryptionInfo.KeyData.SaltValue, 0, (Array) numArray2, 0, encryptionInfo.KeyData.SaltSize);
        Array.Copy((Array) BitConverter.GetBytes(num), 0, (Array) numArray2, encryptionInfo.KeyData.SaltSize, 4);
        byte[] hash2 = hashProvider.ComputeHash(numArray2);
        byte[] numArray3 = new byte[length];
        Array.Copy((Array) encryptedData, sourceIndex, (Array) numArray3, 0, length);
        byte[] buffer = this.DecryptAgileFromKey(keyEncryptor, keyEncryptor.KeyValue, numArray3, (long) size1, hash2);
        memoryStream.Write(buffer, 0, buffer.Length);
        sourceIndex += size1;
        ++num;
      }
      memoryStream.Flush();
      return memoryStream;
    }

    private HashAlgorithm GetHashProvider(EncryptionInfoAgile.EncryptionKeyEncryptor encr)
    {
      switch (encr.HashAlgorithm)
      {
        case eHashAlogorithm.SHA1:
          return (HashAlgorithm) new SHA1CryptoServiceProvider();
        case eHashAlogorithm.SHA256:
          return (HashAlgorithm) new SHA256CryptoServiceProvider();
        case eHashAlogorithm.SHA384:
          return (HashAlgorithm) new SHA384CryptoServiceProvider();
        case eHashAlogorithm.SHA512:
          return (HashAlgorithm) new SHA512CryptoServiceProvider();
        case eHashAlogorithm.MD5:
          return (HashAlgorithm) new MD5CryptoServiceProvider();
        case eHashAlogorithm.RIPEMD160:
          return (HashAlgorithm) new RIPEMD160Managed();
        default:
          throw new NotSupportedException(string.Format("Hash provider is unsupported. {0}", (object) encr.HashAlgorithm));
      }
    }

    private MemoryStream DecryptBinary(
      EncryptionInfoBinary encryptionInfo,
      string password,
      long size,
      byte[] encryptedData)
    {
      MemoryStream memoryStream = new MemoryStream();
      if (encryptionInfo.Header.AlgID == AlgorithmID.AES128 || encryptionInfo.Header.AlgID == AlgorithmID.Flags && (encryptionInfo.Flags & (Flags.fCryptoAPI | Flags.fExternal | Flags.fAES)) == (Flags.fCryptoAPI | Flags.fAES) || encryptionInfo.Header.AlgID == AlgorithmID.AES192 || encryptionInfo.Header.AlgID == AlgorithmID.AES256)
      {
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.KeySize = encryptionInfo.Header.KeySize;
        rijndaelManaged.Mode = CipherMode.ECB;
        rijndaelManaged.Padding = PaddingMode.None;
        byte[] passwordHashBinary = this.GetPasswordHashBinary(password, encryptionInfo);
        ICryptoTransform transform = this.IsPasswordValid(passwordHashBinary, encryptionInfo) ? rijndaelManaged.CreateDecryptor(passwordHashBinary, (byte[]) null) : throw new UnauthorizedAccessException("Invalid password");
        CryptoStream cryptoStream = new CryptoStream((Stream) new MemoryStream(encryptedData), transform, CryptoStreamMode.Read);
        byte[] buffer = new byte[size];
        cryptoStream.Read(buffer, 0, (int) size);
        memoryStream.Write(buffer, 0, (int) size);
      }
      return memoryStream;
    }

    private bool IsPasswordValid(byte[] key, EncryptionInfoBinary encryptionInfo)
    {
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.KeySize = encryptionInfo.Header.KeySize;
      rijndaelManaged.Mode = CipherMode.ECB;
      rijndaelManaged.Padding = PaddingMode.None;
      ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(key, (byte[]) null);
      CryptoStream cryptoStream1 = new CryptoStream((Stream) new MemoryStream(encryptionInfo.Verifier.EncryptedVerifier), decryptor, CryptoStreamMode.Read);
      byte[] buffer1 = new byte[16];
      cryptoStream1.Read(buffer1, 0, 16);
      CryptoStream cryptoStream2 = new CryptoStream((Stream) new MemoryStream(encryptionInfo.Verifier.EncryptedVerifierHash), decryptor, CryptoStreamMode.Read);
      byte[] buffer2 = new byte[16];
      cryptoStream2.Read(buffer2, 0, 16);
      byte[] hash = new SHA1Managed().ComputeHash(buffer1);
      for (int index = 0; index < 16; ++index)
      {
        if ((int) hash[index] != (int) buffer2[index])
          return false;
      }
      return true;
    }

    private bool IsPasswordValid(HashAlgorithm sha, EncryptionInfoAgile.EncryptionKeyEncryptor encr)
    {
      byte[] hash = sha.ComputeHash(encr.VerifierHashInput);
      for (int index = 0; index < hash.Length; ++index)
      {
        if ((int) encr.VerifierHash[index] != (int) hash[index])
          return false;
      }
      return true;
    }

    private byte[] DecryptAgileFromKey(
      EncryptionInfoAgile.EncryptionKeyEncryptor encr,
      byte[] key,
      byte[] encryptedData,
      long size,
      byte[] iv)
    {
      SymmetricAlgorithm encryptionAlgorithm = this.GetEncryptionAlgorithm(encr);
      encryptionAlgorithm.BlockSize = encr.BlockSize << 3;
      encryptionAlgorithm.KeySize = encr.KeyBits;
      encryptionAlgorithm.Mode = encr.ChiptherChaining == eChainingMode.ChainingModeCBC ? CipherMode.CBC : CipherMode.CFB;
      encryptionAlgorithm.Padding = PaddingMode.Zeros;
      ICryptoTransform decryptor = encryptionAlgorithm.CreateDecryptor(this.FixHashSize(key, encr.KeyBits / 8), this.FixHashSize(iv, encr.BlockSize, (byte) 54));
      CryptoStream cryptoStream = new CryptoStream((Stream) new MemoryStream(encryptedData), decryptor, CryptoStreamMode.Read);
      byte[] buffer = new byte[size];
      cryptoStream.Read(buffer, 0, (int) size);
      return buffer;
    }

    private SymmetricAlgorithm GetEncryptionAlgorithm(
      EncryptionInfoAgile.EncryptionKeyEncryptor encr)
    {
      switch (encr.CipherAlgorithm)
      {
        case eCipherAlgorithm.AES:
          return (SymmetricAlgorithm) new RijndaelManaged();
        case eCipherAlgorithm.RC2:
          return (SymmetricAlgorithm) new RC2CryptoServiceProvider();
        case eCipherAlgorithm.DES:
          return (SymmetricAlgorithm) new DESCryptoServiceProvider();
        case eCipherAlgorithm.TRIPLE_DES:
        case eCipherAlgorithm.TRIPLE_DES_112:
          return (SymmetricAlgorithm) new TripleDESCryptoServiceProvider();
        default:
          throw new NotSupportedException(string.Format("Unsupported Cipher Algorithm: {0}", (object) encr.CipherAlgorithm.ToString()));
      }
    }

    private void EncryptAgileFromKey(
      EncryptionInfoAgile.EncryptionKeyEncryptor encr,
      byte[] key,
      byte[] data,
      long pos,
      long size,
      byte[] iv,
      MemoryStream ms)
    {
      SymmetricAlgorithm encryptionAlgorithm = this.GetEncryptionAlgorithm(encr);
      encryptionAlgorithm.BlockSize = encr.BlockSize << 3;
      encryptionAlgorithm.KeySize = encr.KeyBits;
      encryptionAlgorithm.Mode = encr.ChiptherChaining == eChainingMode.ChainingModeCBC ? CipherMode.CBC : CipherMode.CFB;
      encryptionAlgorithm.Padding = PaddingMode.Zeros;
      ICryptoTransform encryptor = encryptionAlgorithm.CreateEncryptor(this.FixHashSize(key, encr.KeyBits / 8), this.FixHashSize(iv, 16, (byte) 54));
      CryptoStream cryptoStream = new CryptoStream((Stream) ms, encryptor, CryptoStreamMode.Write);
      if (size % (long) encr.BlockSize != 0L)
      {
        int blockSize = encr.BlockSize;
        long num = size % (long) encr.BlockSize;
      }
      byte[] numArray = new byte[size];
      Array.Copy((Array) data, pos, (Array) numArray, 0L, size);
      cryptoStream.Write(numArray, 0, (int) size);
      for (; size % (long) encr.BlockSize != 0L; ++size)
        cryptoStream.WriteByte((byte) 0);
    }

    private byte[] GetPasswordHashBinary(string password, EncryptionInfoBinary encryptionInfo)
    {
      byte[] numArray1 = new byte[24];
      try
      {
        if (encryptionInfo.Header.AlgIDHash == AlgorithmHashID.SHA1 || encryptionInfo.Header.AlgIDHash == AlgorithmHashID.App && (encryptionInfo.Flags & Flags.fExternal) == (Flags) 0)
        {
          HashAlgorithm hashProvider = (HashAlgorithm) new SHA1CryptoServiceProvider();
          byte[] passwordHash = this.GetPasswordHash(hashProvider, encryptionInfo.Verifier.Salt, password, 50000, 20);
          Array.Copy((Array) passwordHash, (Array) numArray1, passwordHash.Length);
          Array.Copy((Array) BitConverter.GetBytes(0), 0, (Array) numArray1, passwordHash.Length, 4);
          byte[] hash1 = hashProvider.ComputeHash(numArray1);
          byte[] buffer = new byte[64];
          int size = encryptionInfo.Header.KeySize / 8;
          for (int index = 0; index < buffer.Length; ++index)
            buffer[index] = index < hash1.Length ? (byte) (54 ^ (int) hash1[index]) : (byte) 54;
          byte[] hash2 = hashProvider.ComputeHash(buffer);
          if ((int) encryptionInfo.Verifier.VerifierHashSize > size)
            return this.FixHashSize(hash2, size);
          for (int index = 0; index < buffer.Length; ++index)
            buffer[index] = index < hash1.Length ? (byte) (92 ^ (int) hash1[index]) : (byte) 92;
          byte[] hash3 = hashProvider.ComputeHash(buffer);
          byte[] numArray2 = new byte[hash2.Length + hash3.Length];
          Array.Copy((Array) hash2, 0, (Array) numArray2, 0, hash2.Length);
          Array.Copy((Array) hash3, 0, (Array) numArray2, hash2.Length, hash3.Length);
          return this.FixHashSize(numArray2, size);
        }
        if (encryptionInfo.Header.KeySize > 0 && encryptionInfo.Header.KeySize < 80)
          throw new NotSupportedException("RC4 Hash provider is not supported. Must be SHA1(AlgIDHash == 0x8004)");
        throw new NotSupportedException("Hash provider is invalid. Must be SHA1(AlgIDHash == 0x8004)");
      }
      catch (Exception ex)
      {
        throw new Exception("An error occured when the encryptionkey was created", ex);
      }
    }

    private byte[] GetPasswordHashAgile(
      string password,
      EncryptionInfoAgile.EncryptionKeyEncryptor encr,
      byte[] blockKey)
    {
      try
      {
        HashAlgorithm hashProvider = this.GetHashProvider(encr);
        byte[] passwordHash = this.GetPasswordHash(hashProvider, encr.SaltValue, password, encr.SpinCount, encr.HashSize);
        return this.FixHashSize(this.GetFinalHash(hashProvider, encr, blockKey, passwordHash), encr.KeyBits / 8);
      }
      catch (Exception ex)
      {
        throw new Exception("An error occured when the encryptionkey was created", ex);
      }
    }

    private byte[] GetFinalHash(
      HashAlgorithm hashProvider,
      EncryptionInfoAgile.EncryptionKeyEncryptor encr,
      byte[] blockKey,
      byte[] hash)
    {
      byte[] numArray = new byte[hash.Length + blockKey.Length];
      Array.Copy((Array) hash, (Array) numArray, hash.Length);
      Array.Copy((Array) blockKey, 0, (Array) numArray, hash.Length, blockKey.Length);
      return hashProvider.ComputeHash(numArray);
    }

    private byte[] GetPasswordHash(
      HashAlgorithm hashProvider,
      byte[] salt,
      string password,
      int spinCount,
      int hashSize)
    {
      byte[] numArray = new byte[4 + hashSize];
      byte[] hash = hashProvider.ComputeHash(this.CombinePassword(salt, password));
      for (int index = 0; index < spinCount; ++index)
      {
        Array.Copy((Array) BitConverter.GetBytes(index), (Array) numArray, 4);
        Array.Copy((Array) hash, 0, (Array) numArray, 4, hash.Length);
        hash = hashProvider.ComputeHash(numArray);
      }
      return hash;
    }

    private byte[] FixHashSize(byte[] hash, int size, byte fill = 0)
    {
      if (hash.Length == size)
        return hash;
      if (hash.Length < size)
      {
        byte[] destinationArray = new byte[size];
        Array.Copy((Array) hash, (Array) destinationArray, hash.Length);
        for (int length = hash.Length; length < size; ++length)
          destinationArray[length] = fill;
        return destinationArray;
      }
      byte[] destinationArray1 = new byte[size];
      Array.Copy((Array) hash, (Array) destinationArray1, size);
      return destinationArray1;
    }

    private byte[] CombinePassword(byte[] salt, string password)
    {
      if (password == "")
        password = "VelvetSweatshop";
      byte[] bytes = Encoding.Unicode.GetBytes(password);
      byte[] destinationArray = new byte[salt.Length + bytes.Length];
      Array.Copy((Array) salt, (Array) destinationArray, salt.Length);
      Array.Copy((Array) bytes, 0, (Array) destinationArray, salt.Length, bytes.Length);
      return destinationArray;
    }

    internal static ushort CalculatePasswordHash(string Password)
    {
      ushort num1 = 0;
      for (int index = Password.Length - 1; index >= 0; --index)
      {
        ushort num2 = (ushort) ((uint) num1 ^ (uint) Password[index]);
        num1 = (ushort) ((uint) (ushort) ((int) num2 >> 14 & 1) | (uint) (ushort) ((int) num2 << 1 & (int) short.MaxValue));
      }
      return (ushort) ((uint) (ushort) ((uint) num1 ^ 52811U) ^ (uint) (ushort) Password.Length);
    }
  }
}
