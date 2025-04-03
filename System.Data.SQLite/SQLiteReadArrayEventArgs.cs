// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteReadArrayEventArgs
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  public class SQLiteReadArrayEventArgs : SQLiteReadEventArgs
  {
    private long dataOffset;
    private byte[] byteBuffer;
    private char[] charBuffer;
    private int bufferOffset;
    private int length;

    internal SQLiteReadArrayEventArgs(
      long dataOffset,
      byte[] byteBuffer,
      int bufferOffset,
      int length)
    {
      this.dataOffset = dataOffset;
      this.byteBuffer = byteBuffer;
      this.bufferOffset = bufferOffset;
      this.length = length;
    }

    internal SQLiteReadArrayEventArgs(
      long dataOffset,
      char[] charBuffer,
      int bufferOffset,
      int length)
    {
      this.dataOffset = dataOffset;
      this.charBuffer = charBuffer;
      this.bufferOffset = bufferOffset;
      this.length = length;
    }

    public long DataOffset
    {
      get => this.dataOffset;
      set => this.dataOffset = value;
    }

    public byte[] ByteBuffer => this.byteBuffer;

    public char[] CharBuffer => this.charBuffer;

    public int BufferOffset
    {
      get => this.bufferOffset;
      set => this.bufferOffset = value;
    }

    public int Length
    {
      get => this.length;
      set => this.length = value;
    }
  }
}
