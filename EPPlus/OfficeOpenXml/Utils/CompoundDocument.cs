// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Utils.CompoundDocument
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace OfficeOpenXml.Utils
{
  internal class CompoundDocument
  {
    internal CompoundDocument.StoragePart Storage;

    internal CompoundDocument() => this.Storage = new CompoundDocument.StoragePart();

    internal CompoundDocument(FileInfo fi) => this.Read(fi);

    internal CompoundDocument(CompoundDocument.ILockBytes lb) => this.Read(lb);

    internal CompoundDocument(byte[] doc) => this.Read(doc);

    internal void Read(FileInfo fi) => this.Read(File.ReadAllBytes(fi.FullName));

    internal void Read(byte[] doc)
    {
      CompoundDocument.ILockBytes ppLkbyt;
      CompoundDocument.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out ppLkbyt);
      IntPtr num = Marshal.AllocHGlobal(doc.Length);
      Marshal.Copy(doc, 0, num, doc.Length);
      ppLkbyt.WriteAt(0L, num, doc.Length, out UIntPtr _);
      Marshal.FreeHGlobal(num);
      this.Read(ppLkbyt);
    }

    internal void Read(CompoundDocument.ILockBytes lb)
    {
      if (CompoundDocument.StgIsStorageILockBytes(lb) != 0)
        throw new InvalidDataException(string.Format("Part is not a compound document"));
      CompoundDocument.IStorage ppstgOpen = (CompoundDocument.IStorage) null;
      if (CompoundDocument.StgOpenStorageOnILockBytes(lb, (CompoundDocument.IStorage) null, CompoundDocument.STGM.SHARE_EXCLUSIVE, IntPtr.Zero, 0U, out ppstgOpen) != 0)
        return;
      this.Storage = new CompoundDocument.StoragePart();
      this.ReadParts(ppstgOpen, this.Storage);
      Marshal.ReleaseComObject((object) ppstgOpen);
    }

    internal static byte[] CompressPart(byte[] part)
    {
      MemoryStream output = new MemoryStream(4096);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write((byte) 1);
      int num1 = 1;
      int num2 = 4098;
      int startPos = 0;
      for (int index = part.Length < 4096 ? part.Length : 4096; startPos < index && num1 < num2; index = part.Length < startPos + 4096 ? part.Length : startPos + 4096)
      {
        byte[] buffer = CompoundDocument.CompressChunk(part, ref startPos);
        if (buffer != null && buffer.Length != 0)
        {
          ushort num3 = (ushort) ((uint) (ushort) (buffer.Length - 1 & 4095) | 45056U);
          binaryWriter.Write(num3);
          binaryWriter.Write(buffer);
        }
      }
      binaryWriter.Flush();
      return output.ToArray();
    }

    private static byte[] CompressChunk(byte[] buffer, ref int startPos)
    {
      byte[] numArray = new byte[4096];
      int index1 = 0;
      int destinationIndex = 1;
      int index2 = startPos;
      int num1 = startPos + 4096 < buffer.Length ? startPos + 4096 : buffer.Length;
      while (index2 < num1)
      {
        byte num2 = 0;
        for (int index3 = 0; index3 < 8; ++index3)
        {
          if (index2 - startPos > 0)
          {
            int num3 = -1;
            int num4 = 0;
            int index4 = index2 - 1;
            int lengthBits = CompoundDocument.GetLengthBits(index2 - startPos);
            ushort num5 = (ushort) ((int) ushort.MaxValue >> 16 - lengthBits);
            for (; index4 >= startPos; --index4)
            {
              if ((int) buffer[index4] == (int) buffer[index2])
              {
                int num6 = 1;
                while (buffer.Length > index2 + num6 && (int) buffer[index4 + num6] == (int) buffer[index2 + num6] && num6 < (int) num5)
                  ++num6;
                if (num6 > num4)
                {
                  num3 = index4;
                  num4 = num6;
                  if (num4 == (int) num5)
                    break;
                }
              }
            }
            if (num4 >= 3)
            {
              num2 |= (byte) (1 << index3);
              Array.Copy((Array) BitConverter.GetBytes((ushort) ((uint) (ushort) (index2 - (num3 + 1)) << lengthBits | (uint) (ushort) (num4 - 3))), 0, (Array) numArray, destinationIndex, 2);
              index2 += num4;
              destinationIndex += 2;
            }
            else
              numArray[destinationIndex++] = buffer[index2++];
          }
          else
            numArray[destinationIndex++] = buffer[index2++];
          if (index2 >= num1)
            break;
        }
        numArray[index1] = num2;
        index1 = destinationIndex++;
      }
      byte[] destinationArray = new byte[destinationIndex - 1];
      Array.Copy((Array) numArray, (Array) destinationArray, destinationArray.Length);
      startPos = num1;
      return destinationArray;
    }

    internal static byte[] DecompressPart(byte[] part) => CompoundDocument.DecompressPart(part, 0);

    internal static byte[] DecompressPart(byte[] part, int startPos)
    {
      if (part[startPos] != (byte) 1)
        return (byte[]) null;
      MemoryStream output = new MemoryStream(4096);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      int pos = startPos + 1;
      while (pos < part.Length - 1)
      {
        byte[] chunk = CompoundDocument.GetChunk(part, ref pos);
        if (chunk != null)
          binaryWriter.Write(chunk);
      }
      binaryWriter.Flush();
      return output.ToArray();
    }

    private static byte[] GetChunk(byte[] compBuffer, ref int pos)
    {
      ushort uint16_1 = BitConverter.ToUInt16(compBuffer, pos);
      int length1 = 0;
      byte[] sourceArray = new byte[4098];
      int length2 = ((int) uint16_1 & 4095) + 3;
      int num1 = pos + length2;
      int num2 = ((int) uint16_1 & 32768) >> 15;
      pos += 2;
      if (num2 == 1)
      {
        while (pos < compBuffer.Length && pos < num1)
        {
          byte num3 = compBuffer[pos++];
          if (pos < num1)
          {
            for (int index1 = 0; index1 < 8; ++index1)
            {
              if (((int) num3 & 1 << index1) == 0)
              {
                sourceArray[length1++] = compBuffer[pos++];
              }
              else
              {
                ushort uint16_2 = BitConverter.ToUInt16(compBuffer, pos);
                int lengthBits = CompoundDocument.GetLengthBits(length1);
                ushort num4 = (ushort) ((int) ushort.MaxValue >> 16 - lengthBits);
                ushort num5 = ~num4;
                int num6 = ((int) num4 & (int) uint16_2) + 3;
                int num7 = ((int) num5 & (int) uint16_2) >> lengthBits;
                int num8 = length1 - num7 - 1;
                for (int index2 = 0; index2 < num6; ++index2)
                  sourceArray[length1++] = sourceArray[num8++];
                pos += 2;
              }
              if (pos >= num1)
                break;
            }
          }
          else
            break;
        }
        if (length1 <= 0)
          return (byte[]) null;
        byte[] destinationArray = new byte[length1];
        Array.Copy((Array) sourceArray, (Array) destinationArray, length1);
        return destinationArray;
      }
      byte[] destinationArray1 = new byte[length2];
      Array.Copy((Array) compBuffer, pos, (Array) destinationArray1, 0, length2);
      pos += length2;
      return destinationArray1;
    }

    private static int GetLengthBits(int decompPos)
    {
      if (decompPos <= 16)
        return 12;
      if (decompPos <= 32)
        return 11;
      if (decompPos <= 64)
        return 10;
      if (decompPos <= 128)
        return 9;
      if (decompPos <= 256)
        return 8;
      if (decompPos <= 512)
        return 7;
      if (decompPos <= 1024)
        return 6;
      if (decompPos <= 2048)
        return 5;
      return decompPos <= 4096 ? 4 : 12 - (int) Math.Truncate(Math.Log((double) (decompPos - 1 >> 4), 2.0) + 1.0);
    }

    [DllImport("ole32.dll")]
    private static extern int StgIsStorageFile([MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

    [DllImport("ole32.dll")]
    private static extern int StgIsStorageILockBytes(CompoundDocument.ILockBytes plkbyt);

    [DllImport("ole32.dll")]
    private static extern int StgOpenStorage(
      [MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
      CompoundDocument.IStorage pstgPriority,
      CompoundDocument.STGM grfMode,
      IntPtr snbExclude,
      uint reserved,
      out CompoundDocument.IStorage ppstgOpen);

    [DllImport("ole32.dll")]
    private static extern int StgOpenStorageOnILockBytes(
      CompoundDocument.ILockBytes plkbyt,
      CompoundDocument.IStorage pStgPriority,
      CompoundDocument.STGM grfMode,
      IntPtr snbEnclude,
      uint reserved,
      out CompoundDocument.IStorage ppstgOpen);

    [DllImport("ole32.dll")]
    private static extern int CreateILockBytesOnHGlobal(
      IntPtr hGlobal,
      bool fDeleteOnRelease,
      out CompoundDocument.ILockBytes ppLkbyt);

    [DllImport("ole32.dll")]
    private static extern int StgCreateDocfileOnILockBytes(
      CompoundDocument.ILockBytes plkbyt,
      CompoundDocument.STGM grfMode,
      int reserved,
      out CompoundDocument.IStorage ppstgOpen);

    internal static int IsStorageFile(string Name) => CompoundDocument.StgIsStorageFile(Name);

    internal static int IsStorageILockBytes(CompoundDocument.ILockBytes lb)
    {
      return CompoundDocument.StgIsStorageILockBytes(lb);
    }

    internal static CompoundDocument.ILockBytes GetLockbyte(MemoryStream stream)
    {
      CompoundDocument.ILockBytes ppLkbyt;
      CompoundDocument.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out ppLkbyt);
      byte[] buffer = stream.GetBuffer();
      IntPtr num = Marshal.AllocHGlobal(buffer.Length);
      Marshal.Copy(buffer, 0, num, buffer.Length);
      ppLkbyt.WriteAt(0L, num, buffer.Length, out UIntPtr _);
      Marshal.FreeHGlobal(num);
      return ppLkbyt;
    }

    private MemoryStream ReadParts(
      CompoundDocument.IStorage storage,
      CompoundDocument.StoragePart storagePart)
    {
      MemoryStream memoryStream = (MemoryStream) null;
      System.Runtime.InteropServices.ComTypes.STATSTG pstatstg;
      storage.Stat(out pstatstg, 0U);
      CompoundDocument.IEnumSTATSTG ppenum = (CompoundDocument.IEnumSTATSTG) null;
      storage.EnumElements(0U, IntPtr.Zero, 0U, out ppenum);
      System.Runtime.InteropServices.ComTypes.STATSTG[] rgelt = new System.Runtime.InteropServices.ComTypes.STATSTG[1]
      {
        pstatstg
      };
      uint pceltFetched = 0;
      for (uint index = ppenum.Next(1U, rgelt, out pceltFetched); index != 1U; index = ppenum.Next(1U, rgelt, out pceltFetched))
      {
        foreach (System.Runtime.InteropServices.ComTypes.STATSTG statstg in rgelt)
        {
          if (statstg.type == 1)
          {
            CompoundDocument.IStorage ppstg;
            storage.OpenStorage(statstg.pwcsName, (CompoundDocument.IStorage) null, CompoundDocument.STGM.SHARE_EXCLUSIVE, IntPtr.Zero, 0U, out ppstg);
            CompoundDocument.StoragePart storagePart1 = new CompoundDocument.StoragePart();
            storagePart.SubStorage.Add(statstg.pwcsName, storagePart1);
            this.ReadParts(ppstg, storagePart1);
          }
          else
            storagePart.DataStreams.Add(statstg.pwcsName, this.GetOleStream(storage, statstg));
        }
      }
      Marshal.ReleaseComObject((object) ppenum);
      return memoryStream;
    }

    private byte[] GetOleStream(CompoundDocument.IStorage storage, System.Runtime.InteropServices.ComTypes.STATSTG statstg)
    {
      IStream ppstm;
      storage.OpenStream(statstg.pwcsName, IntPtr.Zero, 16U, 0U, out ppstm);
      byte[] pv = new byte[statstg.cbSize];
      ppstm.Read(pv, (int) statstg.cbSize, IntPtr.Zero);
      Marshal.ReleaseComObject((object) ppstm);
      return pv;
    }

    internal byte[] Save()
    {
      CompoundDocument.ILockBytes ppLkbyt;
      CompoundDocument.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out ppLkbyt);
      CompoundDocument.IStorage ppstgOpen = (CompoundDocument.IStorage) null;
      byte[] destination = (byte[]) null;
      if (CompoundDocument.StgCreateDocfileOnILockBytes(ppLkbyt, CompoundDocument.STGM.TRANSACTED | CompoundDocument.STGM.READWRITE | CompoundDocument.STGM.SHARE_EXCLUSIVE | CompoundDocument.STGM.CREATE, 0, out ppstgOpen) == 0)
      {
        foreach (KeyValuePair<string, CompoundDocument.StoragePart> keyValuePair in this.Storage.SubStorage)
          this.CreateStore(keyValuePair.Key, keyValuePair.Value, ppstgOpen);
        this.CreateStreams(this.Storage, ppstgOpen);
        ppLkbyt.Flush();
        System.Runtime.InteropServices.ComTypes.STATSTG pstatstg = new System.Runtime.InteropServices.ComTypes.STATSTG();
        ppLkbyt.Stat(out pstatstg, 0);
        int cbSize = (int) pstatstg.cbSize;
        IntPtr num = Marshal.AllocHGlobal(cbSize);
        destination = new byte[cbSize];
        ppLkbyt.ReadAt(0L, num, cbSize, out UIntPtr _);
        Marshal.Copy(num, destination, 0, cbSize);
        Marshal.FreeHGlobal(num);
      }
      Marshal.ReleaseComObject((object) ppstgOpen);
      Marshal.ReleaseComObject((object) ppLkbyt);
      return destination;
    }

    private void CreateStore(
      string name,
      CompoundDocument.StoragePart subStore,
      CompoundDocument.IStorage storage)
    {
      CompoundDocument.IStorage ppstg;
      storage.CreateStorage(name, 4113U, 0U, 0U, out ppstg);
      storage.Commit(0U);
      foreach (KeyValuePair<string, CompoundDocument.StoragePart> keyValuePair in subStore.SubStorage)
        this.CreateStore(keyValuePair.Key, keyValuePair.Value, ppstg);
      this.CreateStreams(subStore, ppstg);
    }

    private void CreateStreams(
      CompoundDocument.StoragePart subStore,
      CompoundDocument.IStorage subStorage)
    {
      foreach (KeyValuePair<string, byte[]> dataStream in subStore.DataStreams)
      {
        IStream ppstm;
        subStorage.CreateStream(dataStream.Key, 4113U, 0U, 0U, out ppstm);
        ppstm.Write(dataStream.Value, dataStream.Value.Length, IntPtr.Zero);
      }
      subStorage.Commit(0U);
    }

    internal class StoragePart
    {
      internal Dictionary<string, CompoundDocument.StoragePart> SubStorage = new Dictionary<string, CompoundDocument.StoragePart>();
      internal Dictionary<string, byte[]> DataStreams = new Dictionary<string, byte[]>();
    }

    [Guid("0000000d-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IEnumSTATSTG
    {
      [MethodImpl(MethodImplOptions.PreserveSig)]
      uint Next(uint celt, [MarshalAs(UnmanagedType.LPArray), Out] System.Runtime.InteropServices.ComTypes.STATSTG[] rgelt, out uint pceltFetched);

      void Skip(uint celt);

      void Reset();

      [return: MarshalAs(UnmanagedType.Interface)]
      CompoundDocument.IEnumSTATSTG Clone();
    }

    [Guid("0000000b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    private interface IStorage
    {
      void CreateStream(
        string pwcsName,
        uint grfMode,
        uint reserved1,
        uint reserved2,
        out IStream ppstm);

      void OpenStream(
        string pwcsName,
        IntPtr reserved1,
        uint grfMode,
        uint reserved2,
        out IStream ppstm);

      void CreateStorage(
        string pwcsName,
        uint grfMode,
        uint reserved1,
        uint reserved2,
        out CompoundDocument.IStorage ppstg);

      void OpenStorage(
        string pwcsName,
        CompoundDocument.IStorage pstgPriority,
        CompoundDocument.STGM grfMode,
        IntPtr snbExclude,
        uint reserved,
        out CompoundDocument.IStorage ppstg);

      void CopyTo(
        [In] uint ciidExclude,
        [In] Guid[] rgiidExclude,
        [In] IntPtr snbExclude,
        [In] CompoundDocument.IStorage pstgDest);

      void MoveElementTo(
        string pwcsName,
        CompoundDocument.IStorage pstgDest,
        string pwcsNewName,
        uint grfFlags);

      void Commit(uint grfCommitFlags);

      void Revert();

      void EnumElements(
        uint reserved1,
        IntPtr reserved2,
        uint reserved3,
        out CompoundDocument.IEnumSTATSTG ppenum);

      void DestroyElement(string pwcsName);

      void RenameElement(string pwcsOldName, string pwcsNewName);

      void SetElementTimes(string pwcsName, System.Runtime.InteropServices.ComTypes.FILETIME pctime, System.Runtime.InteropServices.ComTypes.FILETIME patime, System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

      void SetClass(Guid clsid);

      void SetStateBits(uint grfStateBits, uint grfMask);

      void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, uint grfStatFlag);
    }

    [ComVisible(false)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0000000A-0000-0000-C000-000000000046")]
    [ComImport]
    internal interface ILockBytes
    {
      void ReadAt(long ulOffset, IntPtr pv, int cb, out UIntPtr pcbRead);

      void WriteAt(long ulOffset, IntPtr pv, int cb, out UIntPtr pcbWritten);

      void Flush();

      void SetSize(long cb);

      void LockRegion(long libOffset, long cb, int dwLockType);

      void UnlockRegion(long libOffset, long cb, int dwLockType);

      void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);
    }

    [Flags]
    internal enum STGM
    {
      DIRECT = 0,
      TRANSACTED = 65536, // 0x00010000
      SIMPLE = 134217728, // 0x08000000
      READ = 0,
      WRITE = 1,
      READWRITE = 2,
      SHARE_DENY_NONE = 64, // 0x00000040
      SHARE_DENY_READ = 48, // 0x00000030
      SHARE_DENY_WRITE = 32, // 0x00000020
      SHARE_EXCLUSIVE = 16, // 0x00000010
      PRIORITY = 262144, // 0x00040000
      DELETEONRELEASE = 67108864, // 0x04000000
      NOSCRATCH = 1048576, // 0x00100000
      CREATE = 4096, // 0x00001000
      CONVERT = 131072, // 0x00020000
      FAILIFTHERE = 0,
      NOSNAPSHOT = 2097152, // 0x00200000
      DIRECT_SWMR = 4194304, // 0x00400000
    }

    internal enum STATFLAG : uint
    {
      STATFLAG_DEFAULT,
      STATFLAG_NONAME,
      STATFLAG_NOOPEN,
    }

    internal enum STGTY
    {
      STGTY_STORAGE = 1,
      STGTY_STREAM = 2,
      STGTY_LOCKBYTES = 3,
      STGTY_PROPERTY = 4,
    }
  }
}
