// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.LmpFeatures
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  [Flags]
  public enum LmpFeatures : long
  {
    None = 0,
    ThreeSlotPackets = 1,
    FiveSlotPackets = 2,
    Encryption = 4,
    SlotOffset = 8,
    TimingAccuracy = 16, // 0x0000000000000010
    RoleSwitch = 32, // 0x0000000000000020
    HoldMode = 64, // 0x0000000000000040
    SniffMode = 128, // 0x0000000000000080
    ParkState = 256, // 0x0000000000000100
    PowerControlRequests = 512, // 0x0000000000000200
    ChannelQualityDrivenDataRate = 1024, // 0x0000000000000400
    ScoLink = 2048, // 0x0000000000000800
    HV2Packets = 4096, // 0x0000000000001000
    HV3Packets = 8192, // 0x0000000000002000
    MuLawLogSynchronousData = 16384, // 0x0000000000004000
    ALawLogSynchronousData = 32768, // 0x0000000000008000
    CvsdSynchronousData = 65536, // 0x0000000000010000
    PagingParameterNegotiation = 131072, // 0x0000000000020000
    PowerControl = 262144, // 0x0000000000040000
    TransparentSynchronousData = 524288, // 0x0000000000080000
    FlowControlLag_LeastSignificantBit = 1048576, // 0x0000000000100000
    FlowControlLag_MiddleBit = 2097152, // 0x0000000000200000
    FlowControlLag_MostSignificantBit = 4194304, // 0x0000000000400000
    BroadcastEncryption = 8388608, // 0x0000000000800000
    EnhancedDataRateAcl2MbpsMode = 33554432, // 0x0000000002000000
    EnhancedDataRateAcl3MbpsMode = 67108864, // 0x0000000004000000
    EnhancedInquiryScan = 134217728, // 0x0000000008000000
    InterlacedInquiryScan = 268435456, // 0x0000000010000000
    InterlacedPageScan = 536870912, // 0x0000000020000000
    RssiWithInquiryResults = 1073741824, // 0x0000000040000000
    ExtendedScoLinkEV3Packets = 2147483648, // 0x0000000080000000
    EV4Packets = 4294967296, // 0x0000000100000000
    EV5Packets = 8589934592, // 0x0000000200000000
    AfhCapableSlave = 34359738368, // 0x0000000800000000
    AfhClassificationSlave = 68719476736, // 0x0000001000000000
    BrEdrNotSupported = 137438953472, // 0x0000002000000000
    LeSupported_Controller = 274877906944, // 0x0000004000000000
    ThreeSlotEnhancedDataRateAclPackets = 549755813888, // 0x0000008000000000
    FiveSlotEnhancedDataRateAclPackets = 1099511627776, // 0x0000010000000000
    SniffSubrating = 2199023255552, // 0x0000020000000000
    PauseEncryption = 4398046511104, // 0x0000040000000000
    AFHCapableMaster = 8796093022208, // 0x0000080000000000
    AFHClassificationMaster = 17592186044416, // 0x0000100000000000
    EnhancedDataRateESco2MbpsMode = 35184372088832, // 0x0000200000000000
    EnhancedDataRateESco3MbpsMode = 70368744177664, // 0x0000400000000000
    ThreeSlotEnhancedDataRateEScoPackets = 140737488355328, // 0x0000800000000000
    ExtendedInquiryResponse = 281474976710656, // 0x0001000000000000
    SimultaneousLeAndBrEdrToSameDeviceCapable_Controller = 562949953421312, // 0x0002000000000000
    SecureSimplePairing = 2251799813685248, // 0x0008000000000000
    EncapsulatedPdu = 4503599627370496, // 0x0010000000000000
    ErroneousDataReporting = 9007199254740992, // 0x0020000000000000
    NonFlushablePacketBoundaryFlag = 18014398509481984, // 0x0040000000000000
    LinkSupervisionTimeoutChangedEvent = 72057594037927936, // 0x0100000000000000
    InquiryTxPowerLevel = 144115188075855872, // 0x0200000000000000
    EnhancedPowerControl = InquiryTxPowerLevel, // 0x0200000000000000
    ExtendedFeatures = -9223372036854775808, // 0x8000000000000000
  }
}
