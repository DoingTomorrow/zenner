// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiHeaderEx
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MinomatHandler
{
  public sealed class SCGiHeaderEx
  {
    public bool CanIgnored { get; set; }

    public SCGiSequenceHeaderType HeaderType { get; set; }

    public SCGiSequenceHeaderType NextHeaderType { get; private set; }

    public byte[] Content { get; set; }

    public int BufferLength
    {
      get
      {
        switch (this.HeaderType)
        {
          case SCGiSequenceHeaderType.Debug:
            return 7;
          case SCGiSequenceHeaderType.Authentication:
            return 8;
          default:
            return 0;
        }
      }
    }

    public SCGiHeaderEx(bool canIgnored, SCGiSequenceHeaderType headerType, byte[] content)
    {
      this.CanIgnored = canIgnored;
      this.HeaderType = headerType;
      this.Content = content;
      this.NextHeaderType = SCGiSequenceHeaderType.None;
    }

    public byte[] Create(SCGiSequenceHeaderType nextHeaderType)
    {
      this.NextHeaderType = nextHeaderType;
      return this.Create();
    }

    public byte[] Create()
    {
      byte num1 = this.CanIgnored ? (byte) 0 : (byte) 128;
      byte num2 = (byte) ((uint) (byte) this.NextHeaderType << 4);
      byte[] numArray;
      switch (this.HeaderType)
      {
        case SCGiSequenceHeaderType.None:
          throw new ArgumentException("Invalid extended header type! Type: " + this.HeaderType.ToString());
        case SCGiSequenceHeaderType.Debug:
          if (this.Content == null)
            throw new ArgumentException("Timepoint can not be null!");
          numArray = this.Content.Length == 6 ? this.Content : throw new ArgumentException("Invalid length of timepoint! Actual: " + this.Content.Length.ToString() + " bytes, Expected: 6 bytes");
          break;
        case SCGiSequenceHeaderType.Authentication:
          if (this.Content == null)
            throw new ArgumentException("Password can not be null!");
          numArray = this.Content.Length == 6 ? new byte[this.Content.Length + 1] : throw new ArgumentException("Invalid password length! Actual: " + this.Content.Length.ToString() + " bytes, Expected: 6 bytes");
          numArray[0] = (byte) 1;
          Buffer.BlockCopy((Array) this.Content, 0, (Array) numArray, 1, this.Content.Length);
          break;
        default:
          throw new NotSupportedException("Extended header type is not supported! Type: " + this.HeaderType.ToString());
      }
      byte length = (byte) (numArray.Length + 1);
      byte[] dst = new byte[(int) length];
      byte num3 = (byte) ((uint) length / 2U);
      dst[0] = (byte) ((uint) num1 | (uint) num2 | (uint) num3);
      Buffer.BlockCopy((Array) numArray, 0, (Array) dst, 1, numArray.Length);
      return dst;
    }

    public static List<SCGiHeaderEx> Parse(SCGiHeader header, byte[] buffer)
    {
      if (header == null)
        throw new ArgumentNullException("Header can not be null!");
      if (buffer == null)
        throw new ArgumentNullException("SCGi packet buffer can not be null!");
      if (buffer.Length < 8)
        throw new ArgumentNullException("Wrong length of SCGi packet buffer!");
      List<SCGiHeaderEx> scGiHeaderExList = new List<SCGiHeaderEx>();
      int pos = 6;
      SCGiHeaderEx headerEx;
      for (SCGiSequenceHeaderType sequenceHeaderType = header.SequenceHeaderType; sequenceHeaderType != 0; sequenceHeaderType = headerEx.NextHeaderType)
      {
        headerEx = SCGiHeaderEx.ParseHeaderEx(header.SequenceHeaderType, buffer, ref pos);
        if (headerEx == null)
          return (List<SCGiHeaderEx>) null;
        scGiHeaderExList.Add(headerEx);
      }
      return scGiHeaderExList;
    }

    private static SCGiHeaderEx ParseHeaderEx(
      SCGiSequenceHeaderType type,
      byte[] buffer,
      ref int pos)
    {
      switch (type)
      {
        case SCGiSequenceHeaderType.Authentication:
          if (buffer.Length - pos < 8)
            return (SCGiHeaderEx) null;
          bool canIgnored = ((int) buffer[pos] & 128) == 0;
          SCGiSequenceHeaderType sequenceHeaderType = (SCGiSequenceHeaderType) Enum.ToObject(typeof (SCGiSequenceHeaderType), (int) buffer[pos] >> 4 & 7);
          if ((byte) (((int) buffer[pos] & 15) * 2) != (byte) 8)
            return (SCGiHeaderEx) null;
          byte[] numArray = new byte[6];
          Buffer.BlockCopy((Array) buffer, pos + 2, (Array) numArray, 0, numArray.Length);
          SCGiHeaderEx headerEx = new SCGiHeaderEx(canIgnored, type, numArray);
          headerEx.NextHeaderType = sequenceHeaderType;
          pos += 8;
          return headerEx;
        default:
          return (SCGiHeaderEx) null;
      }
    }
  }
}
