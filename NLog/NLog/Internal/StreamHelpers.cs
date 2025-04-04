// Decompiled with JetBrains decompiler
// Type: NLog.Internal.StreamHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace NLog.Internal
{
  public static class StreamHelpers
  {
    public static void CopyAndSkipBom(this Stream input, Stream output, Encoding encoding)
    {
      int length = EncodingHelpers.Utf8BOM.Length;
      byte[] numArray = new byte[length];
      long position = input.Position;
      input.Read(numArray, 0, length);
      if (((IEnumerable<byte>) numArray).SequenceEqual<byte>((IEnumerable<byte>) EncodingHelpers.Utf8BOM))
      {
        InternalLogger.Debug("input has UTF8 BOM");
      }
      else
      {
        InternalLogger.Debug("input hasn't a UTF8 BOM");
        input.Position = position;
      }
      input.Copy(output);
    }

    public static void Copy(this Stream input, Stream output) => input.CopyWithOffset(output, 0);

    public static void CopyWithOffset(this Stream input, Stream output, int offset)
    {
      if (offset < 0)
        throw new ArgumentException("negative offset");
      if (offset > 0)
        input.Seek((long) offset, SeekOrigin.Current);
      byte[] buffer = new byte[4096];
      int count;
      while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
        output.Write(buffer, 0, count);
    }
  }
}
