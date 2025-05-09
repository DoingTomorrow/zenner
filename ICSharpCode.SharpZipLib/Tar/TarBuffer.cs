﻿// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Tar.TarBuffer
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System;
using System.IO;

#nullable disable
namespace ICSharpCode.SharpZipLib.Tar
{
  public class TarBuffer
  {
    public const int BlockSize = 512;
    public const int DefaultBlockFactor = 20;
    public const int DefaultRecordSize = 10240;
    private Stream inputStream;
    private Stream outputStream;
    private byte[] recordBuffer;
    private int currentBlockIndex;
    private int currentRecordIndex;
    private int recordSize = 10240;
    private int blockFactor = 20;

    public int RecordSize => this.recordSize;

    [Obsolete("Use RecordSize property instead")]
    public int GetRecordSize() => this.recordSize;

    public int BlockFactor => this.blockFactor;

    [Obsolete("Use BlockFactor property instead")]
    public int GetBlockFactor() => this.blockFactor;

    protected TarBuffer()
    {
    }

    public static TarBuffer CreateInputTarBuffer(Stream inputStream)
    {
      return inputStream != null ? TarBuffer.CreateInputTarBuffer(inputStream, 20) : throw new ArgumentNullException(nameof (inputStream));
    }

    public static TarBuffer CreateInputTarBuffer(Stream inputStream, int blockFactor)
    {
      if (inputStream == null)
        throw new ArgumentNullException(nameof (inputStream));
      if (blockFactor <= 0)
        throw new ArgumentOutOfRangeException(nameof (blockFactor), "Factor cannot be negative");
      TarBuffer inputTarBuffer = new TarBuffer();
      inputTarBuffer.inputStream = inputStream;
      inputTarBuffer.outputStream = (Stream) null;
      inputTarBuffer.Initialize(blockFactor);
      return inputTarBuffer;
    }

    public static TarBuffer CreateOutputTarBuffer(Stream outputStream)
    {
      return outputStream != null ? TarBuffer.CreateOutputTarBuffer(outputStream, 20) : throw new ArgumentNullException(nameof (outputStream));
    }

    public static TarBuffer CreateOutputTarBuffer(Stream outputStream, int blockFactor)
    {
      if (outputStream == null)
        throw new ArgumentNullException(nameof (outputStream));
      if (blockFactor <= 0)
        throw new ArgumentOutOfRangeException(nameof (blockFactor), "Factor cannot be negative");
      TarBuffer outputTarBuffer = new TarBuffer();
      outputTarBuffer.inputStream = (Stream) null;
      outputTarBuffer.outputStream = outputStream;
      outputTarBuffer.Initialize(blockFactor);
      return outputTarBuffer;
    }

    private void Initialize(int blockFactor)
    {
      this.blockFactor = blockFactor;
      this.recordSize = blockFactor * 512;
      this.recordBuffer = new byte[this.RecordSize];
      if (this.inputStream != null)
      {
        this.currentRecordIndex = -1;
        this.currentBlockIndex = this.BlockFactor;
      }
      else
      {
        this.currentRecordIndex = 0;
        this.currentBlockIndex = 0;
      }
    }

    [Obsolete("Use IsEndOfArchiveBlock instead")]
    public bool IsEOFBlock(byte[] block)
    {
      if (block == null)
        throw new ArgumentNullException(nameof (block));
      if (block.Length != 512)
        throw new ArgumentException("block length is invalid");
      for (int index = 0; index < 512; ++index)
      {
        if (block[index] != (byte) 0)
          return false;
      }
      return true;
    }

    public static bool IsEndOfArchiveBlock(byte[] block)
    {
      if (block == null)
        throw new ArgumentNullException(nameof (block));
      if (block.Length != 512)
        throw new ArgumentException("block length is invalid");
      for (int index = 0; index < 512; ++index)
      {
        if (block[index] != (byte) 0)
          return false;
      }
      return true;
    }

    public void SkipBlock()
    {
      if (this.inputStream == null)
        throw new TarException("no input stream defined");
      if (this.currentBlockIndex >= this.BlockFactor && !this.ReadRecord())
        throw new TarException("Failed to read a record");
      ++this.currentBlockIndex;
    }

    public byte[] ReadBlock()
    {
      if (this.inputStream == null)
        throw new TarException("TarBuffer.ReadBlock - no input stream defined");
      if (this.currentBlockIndex >= this.BlockFactor && !this.ReadRecord())
        throw new TarException("Failed to read a record");
      byte[] destinationArray = new byte[512];
      Array.Copy((Array) this.recordBuffer, this.currentBlockIndex * 512, (Array) destinationArray, 0, 512);
      ++this.currentBlockIndex;
      return destinationArray;
    }

    private bool ReadRecord()
    {
      if (this.inputStream == null)
        throw new TarException("no input stream stream defined");
      this.currentBlockIndex = 0;
      int offset = 0;
      long num;
      for (int recordSize = this.RecordSize; recordSize > 0; recordSize -= (int) num)
      {
        num = (long) this.inputStream.Read(this.recordBuffer, offset, recordSize);
        if (num > 0L)
          offset += (int) num;
        else
          break;
      }
      ++this.currentRecordIndex;
      return true;
    }

    public int CurrentBlock => this.currentBlockIndex;

    [Obsolete("Use CurrentBlock property instead")]
    public int GetCurrentBlockNum() => this.currentBlockIndex;

    public int CurrentRecord => this.currentRecordIndex;

    [Obsolete("Use CurrentRecord property instead")]
    public int GetCurrentRecordNum() => this.currentRecordIndex;

    public void WriteBlock(byte[] block)
    {
      if (block == null)
        throw new ArgumentNullException(nameof (block));
      if (this.outputStream == null)
        throw new TarException("TarBuffer.WriteBlock - no output stream defined");
      if (block.Length != 512)
        throw new TarException(string.Format("TarBuffer.WriteBlock - block to write has length '{0}' which is not the block size of '{1}'", (object) block.Length, (object) 512));
      if (this.currentBlockIndex >= this.BlockFactor)
        this.WriteRecord();
      Array.Copy((Array) block, 0, (Array) this.recordBuffer, this.currentBlockIndex * 512, 512);
      ++this.currentBlockIndex;
    }

    public void WriteBlock(byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (this.outputStream == null)
        throw new TarException("TarBuffer.WriteBlock - no output stream stream defined");
      if (offset < 0 || offset >= buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (offset + 512 > buffer.Length)
        throw new TarException(string.Format("TarBuffer.WriteBlock - record has length '{0}' with offset '{1}' which is less than the record size of '{2}'", (object) buffer.Length, (object) offset, (object) this.recordSize));
      if (this.currentBlockIndex >= this.BlockFactor)
        this.WriteRecord();
      Array.Copy((Array) buffer, offset, (Array) this.recordBuffer, this.currentBlockIndex * 512, 512);
      ++this.currentBlockIndex;
    }

    private void WriteRecord()
    {
      if (this.outputStream == null)
        throw new TarException("TarBuffer.WriteRecord no output stream defined");
      this.outputStream.Write(this.recordBuffer, 0, this.RecordSize);
      this.outputStream.Flush();
      this.currentBlockIndex = 0;
      ++this.currentRecordIndex;
    }

    private void Flush()
    {
      if (this.outputStream == null)
        throw new TarException("TarBuffer.Flush no output stream defined");
      if (this.currentBlockIndex > 0)
      {
        int index = this.currentBlockIndex * 512;
        Array.Clear((Array) this.recordBuffer, index, this.RecordSize - index);
        this.WriteRecord();
      }
      this.outputStream.Flush();
    }

    public void Close()
    {
      if (this.outputStream != null)
      {
        this.Flush();
        this.outputStream.Close();
        this.outputStream = (Stream) null;
      }
      else
      {
        if (this.inputStream == null)
          return;
        this.inputStream.Close();
        this.inputStream = (Stream) null;
      }
    }
  }
}
