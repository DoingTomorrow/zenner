// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Message.Message
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.DTO.MessageHandler;

#nullable disable
namespace MSS.DTO.Message
{
  public class Message
  {
    public MessageTypeEnum MessageType { get; set; }

    public string MessageText { get; set; }

    public Message()
    {
    }

    public Message(MessageTypeEnum messageType, string messageText)
    {
      this.MessageType = messageType;
      this.MessageText = messageText;
    }
  }
}
