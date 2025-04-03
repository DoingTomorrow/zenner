// Decompiled with JetBrains decompiler
// Type: S3_Handler.C5_FirmwareUpdate
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using DeviceCollector;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  internal class C5_FirmwareUpdate
  {
    internal static Logger C5_FirmwareUpdateLogger = LogManager.GetLogger(nameof (C5_FirmwareUpdate));
    private S3_HandlerFunctions MyFunctions;
    private const int BLH_StartAddress = 20480;
    private const int BSL_TmpAddress = 20992;
    private const int BLH_BSL_size = 4096;
    private bool backupOnRead;
    private bool onlyOnePerday;
    private bool backupOnWrite;
    private bool dbPasswordIsDefined;
    private uint meterKey = uint.MaxValue;
    private ByteField recoveryData;
    private FirmwareUpdateSettings mySettings;
    private S3_DeviceDescriptor deviceDescriptor;
    private ReadVersionData bslVersionData;
    private ReadVersionData meterVersionData;
    private string bootLoadHelper = "  79 da 31 40 80 2b 3c 40 00 1c 3e 40 02 00 cc 43 \r\n                                    00 00 1c 53 1e 83 fb 23 30 40 f4 50 3c 40 00 1c \r\n                                    3d 40 02 00 b0 13 38 51 31 80 06 00 b0 13 18 51 \r\n                                    3e 40 00 08 3d 40 00 10 3c 40 00 52 b0 13 62 50 \r\n                                    0c 41 3e 40 5a 51 3d 40 06 00 b0 13 4a 51 3e 40 \r\n                                    06 00 3d 40 f2 17 0c 41 b0 13 62 50 31 50 06 00 \r\n                                    10 01 32 c2 03 43 b2 40 00 a5 44 01 b2 40 40 a5 \r\n                                    40 01 04 3c ed 4c 00 00 1c 53 1d 53 0f 4e 0e 4f \r\n                                    3e 53 0f 93 f7 23 b2 40 00 a5 40 01 b2 40 10 a5 \r\n                                    44 01 03 43 32 d2 10 01 b2 40 33 00 68 01 92 c3 \r\n                                    6c 01 f2 d0 c0 00 6c 01 05 3c b2 f0 f4 ff 6e 01 \r\n                                    a2 c3 02 01 a2 b3 02 01 f8 2f b2 f0 3f ff 6c 01 \r\n                                    b2 f0 ff 3f 82 01 10 01 32 c2 03 43 b2 40 00 a5 \r\n                                    44 01 b2 40 02 a5 40 01 cc 43 00 00 92 b3 44 01 \r\n                                    fd 2f b2 40 00 a5 40 01 b2 40 10 a5 44 01 03 43 \r\n                                    32 d2 10 01 b2 40 80 5a 5c 01 32 c2 03 43 b0 13 \r\n                                    98 50 b0 13 28 50 b2 40 04 a5 20 01 92 43 00 1c \r\n                                    82 93 00 1c fd 23 10 01 3c 40 00 10 b0 13 c8 50 \r\n                                    3c 40 00 12 b0 13 c8 50 3c 40 00 14 b0 13 c8 50 \r\n                                    3c 40 00 16 80 00 c8 50 0f 4c 0f 5d 03 3c cc 43 \r\n                                    00 00 1c 53 0c 9f fb 23 10 01 0c 12 fc 4e 00 00 \r\n                                    1c 53 1d 83 fb 23 3c 41 10 01 02 10 a5 3c 5a c3 \r\n                                    ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff";
    private int bootLoaderVersion_4K_RAM = 16777221;
    private ushort bootStartAddressAt_4KRAM = 11258;
    private string bootLoader_4K_RAM = "   05 08 31 40 80 2B 3C 40 00 1C 3E 40 11 04 CC 43\r\n                                        00 00 1C 53 1E 83 FB 23 30 40 C2 10 8C 00 00 1C\r\n                                        3E 40 11 04 3F 40 00 00 B0 13 78 17 21 83 F2 B0\r\n                                        24 00 CA 05 2A 20 0A 12 0F 12 92 B3 DC 05 11 28\r\n                                        92 C3 DC 05 1A 42 CC 05 4C 4A B0 13 4A 14 40 18\r\n                                        C2 4C 0F 20 40 18 D2 93 0F 20 15 20 92 C3 DA 05\r\n                                        12 3C B2 B2 DC 05 FD 2B A2 C3 DC 05 1D 43 0C 41\r\n                                        B0 13 D4 15 4C 93 05 24 6F 41 4F 4F 82 4F CE 05\r\n                                        02 3C B0 13 10 17 3F 41 3A 41 21 53 00 13 32 C2\r\n                                        03 43 B2 B0 40 00 44 01 04 28 B2 40 40 A5 44 01\r\n                                        03 3C B2 40 00 A5 44 01 B2 40 02 A5 40 01 CC 43\r\n                                        00 00 B2 40 00 A5 40 01 B2 40 10 A5 44 01 32 D2\r\n                                        10 01 B2 40 80 5A 5C 01 B0 13 CC 12 B0 13 12 16\r\n                                        40 18 D2 43 10 20 05 3C 40 18 C2 43 0F 20 B0 13\r\n                                        24 11 40 18 C2 93 10 20 0B 24 40 18 D2 93 0F 20\r\n                                        F8 2B 40 18 D2 93 0F 20 EF 27 B0 13 10 17 F1 3F\r\n                                        8C 00 00 12 B0 13 8E 10 8C 00 00 14 B0 13 8E 10\r\n                                        8C 00 00 16 B0 13 8E 10 92 C3 80 01 B2 40 34 12\r\n                                        5C 01 10 01 0A 14 31 80 0E 00 81 43 0A 00 81 43\r\n                                        0C 00 40 18 1F 42 06 20 40 18 1F 82 08 20 3F F0\r\n                                        FF 03 3F 50 FD FF 81 4F 00 00 2C 41 B0 13 4E 16\r\n                                        81 4C 02 00 3F 40 FD FF 40 18 1F 52 06 20 40 18\r\n                                        5A 4F 06 1C 4A 4A 3F 40 FE FF 40 18 1F 52 06 20\r\n                                        40 18 5F 4F 06 1C 4F 4F 3F F0 FF 00 8F 10 0A DF\r\n                                        1D 43 0C 41 2C 52 B0 13 D4 15 3D 40 03 00 0C 41\r\n                                        3C 50 0A 00 B0 13 D4 15 2D 43 0C 41 B0 13 D4 15\r\n                                        F1 90 53 00 04 00 09 24 81 9A 02 00 06 24 B0 13\r\n                                        10 17 B2 40 1A 00 CE 05 85 3C 5E 41 04 00 4E 83\r\n                                        1C 24 5E 83 5D 24 5E 83 66 24 5E 83 6E 24 7E 80\r\n                                        50 00 78 20 1F 43 40 18 1F 52 08 20 40 18 FF 90\r\n                                        06 00 06 1C 07 20 B0 13 10 17 B0 13 8A 15 B0 13\r\n                                        2C 17 68 3C B0 13 10 17 65 3C B0 13 10 17 2E 41\r\n                                        3E 50 06 00 F1 40 68 00 06 00 C1 4E 07 00 8E 10\r\n                                        3E F0 FF 00 C1 4E 08 00 F1 40 68 00 09 00 2D 42\r\n                                        0C 41 3C 50 06 00 B0 13 86 16 1D 43 0C 41 2C 52\r\n                                        B0 13 86 16 3D 40 03 00 0C 41 3C 50 0A 00 B0 13\r\n                                        86 16 2D 43 0C 41 B0 13 86 16 2D 41 3C 01 0A 00\r\n                                        B0 13 B6 17 2C 41 B0 13 4E 16 81 4C 02 00 2D 43\r\n                                        0C 41 2C 53 B0 13 86 16 F1 40 16 00 06 00 1D 43\r\n                                        0C 41 3C 50 06 00 B0 13 86 16 B0 13 2C 17 22 3C\r\n                                        2D 41 3C 01 0A 00 B0 13 EC 16 B0 13 10 17 B2 40\r\n                                        E5 00 CE 05 17 3C 3C 01 0A 00 B0 13 8E 10 B0 13\r\n                                        10 17 B2 40 E5 00 CE 05 0D 3C 1E 41 0A 00 1F 41\r\n                                        0C 00 40 18 82 4E 02 1C 40 18 82 4F 04 1C 40 18\r\n                                        C2 43 10 20 31 50 0E 00 0A 16 10 01 C2 43 42 02\r\n                                        C2 43 44 02 C2 43 46 02 C2 43 48 02 C2 43 4A 02\r\n                                        3C 40 0A 00 B0 13 A2 17 0F 43 5E 42 40 02 40 19\r\n                                        4E 10 3D 40 10 00 06 3C 4C 4E 0C FD 0F DC 40 19\r\n                                        4E 10 5D 07 0D 93 F8 23 F2 D0 38 00 46 02 3C 40\r\n                                        0A 00 B0 13 A2 17 5E 42 40 02 F2 D0 38 00 42 02\r\n                                        3C 40 0A 00 B0 13 A2 17 5E E2 40 02 3D 40 20 00\r\n                                        06 3C 4C 4E 0C FD 0F DC 40 19 4E 10 5D 07 0D 93\r\n                                        F8 23 1D 43 11 3C 0C 12 C1 4F 00 00 3C 41 7C F0\r\n                                        03 00 0E 4D 5C 83 02 30 CC 18 0E 5E 5C 53 40 18\r\n                                        82 DE 00 1C 5F 07 5D 0E 3D 90 00 10 EC 3B F2 F0\r\n                                        C7 00 46 02 B2 40 52 2D C0 01 F2 40 1F 00 CC 01\r\n                                        F2 40 1F 00 CD 01 E2 43 DF 01 D2 43 D1 01 A2 43\r\n                                        C2 01 C2 43 02 02 F2 40 C0 00 04 02 C2 43 06 02\r\n                                        C2 43 08 02 F2 40 30 00 0A 02 E2 43 0B 02 F2 40\r\n                                        80 00 2A 02 F2 40 80 00 22 02 F2 40 88 00 24 02\r\n                                        C2 43 42 02 F2 40 C7 00 44 02 C2 43 46 02 C2 43\r\n                                        48 02 C2 43 4A 02 C2 43 43 02 F2 43 45 02 C2 43\r\n                                        47 02 C2 43 49 02 C2 43 4B 02 C2 43 62 02 F2 43\r\n                                        64 02 C2 43 66 02 C2 43 68 02 C2 43 6A 02 F2 C0\r\n                                        10 00 63 02 F2 40 1F 00 65 02 C2 43 67 02 C2 43\r\n                                        69 02 C2 43 6B 02 B2 40 33 00 68 01 92 C3 6C 01\r\n                                        F2 D0 C0 00 6C 01 05 3C B2 F0 F4 FF 6E 01 A2 C3\r\n                                        02 01 A2 B3 02 01 F8 2F B2 F0 3F FF 6C 01 B0 13\r\n                                        BC 16 B0 13 2C 15 32 D2 10 01 0A 14 4C 12 4A 43\r\n                                        40 18 5F 42 0E 20 4F 4F E0 0F 09 3C 56 3C 14 3C\r\n                                        1E 3C 39 3C 52 3C 51 3C 42 3C 4F 3C 56 3C B0 13\r\n                                        90 17 4C 93 07 24 F1 90 68 00 00 00 03 20 40 18\r\n                                        E2 42 0E 20 4C 43 46 3C 40 18 F2 40 06 00 0E 20\r\n                                        6F 41 4F 4F 40 18 82 4F 0A 20 4C 43 3B 3C 40 18\r\n                                        F2 42 0E 20 40 18 B2 92 0A 20 03 20 F1 92 00 00\r\n                                        08 24 6F 41 4F 4F 3F F0 FF 00 8F 10 40 18 82 DF\r\n                                        0A 20 40 18 B2 90 00 04 0A 20 03 28 7C 40 05 00\r\n                                        21 3C 4C 43 1F 3C F1 90 68 00 00 00 06 20 40 18\r\n                                        F2 40 0E 00 0E 20 4C 43 15 3C 6C 43 13 3C 40 18\r\n                                        1F 42 0A 20 0E 4F 3E 53 40 18 82 4E 0A 20 0F 93\r\n                                        04 20 40 18 F2 40 12 00 0E 20 1D 43 0C 41 B0 13\r\n                                        86 16 4C 4A 21 53 0A 16 10 01 F1 90 16 00 00 00\r\n                                        02 20 5A 43 F2 3F 6C 43 F5 3F 03 43 B2 40 1C 3B\r\n                                        00 0A B2 43 0A 0A B2 40 3F 00 0C 0A B2 40 20 00\r\n                                        08 0A 92 D3 00 0A 8F 00 20 0A 0E 43 04 3C 8F 43\r\n                                        00 00 EF 03 1E 53 3E 90 0A 00 F9 3B D2 43 21 0A\r\n                                        F2 40 CB 00 22 0A F2 40 6E 00 23 0A F2 40 CD 00\r\n                                        24 0A F2 40 EE 00 25 0A F2 40 0F 00 26 0A F2 40\r\n                                        2F 00 27 0A D2 43 28 0A 10 01 31 80 28 00 0F 41\r\n                                        0C 4F 8E 00 BA 17 8D 00 28 00 B0 13 60 17 40 18\r\n                                        D1 42 00 1C 18 00 8F 00 00 1C D1 4F 01 00 19 00\r\n                                        6E 42 05 3C 4F 4E 0F 51 E1 5F 26 00 5E 53 7E 90\r\n                                        26 00 F8 2B 3D 40 28 00 0C 41 B0 13 86 16 31 50\r\n                                        28 00 10 01 CE 0C 10 3C 40 18 1F 42 08 20 40 18\r\n                                        DE 4F 06 1C 00 00 40 18 92 53 08 20 AE 00 01 00\r\n                                        40 18 B2 F0 FF 03 08 20 0F 4D 0D 4F 3D 53 0F 93\r\n                                        06 24 B0 13 90 17 4C 93 E7 27 4C 43 10 01 5C 43\r\n                                        10 01 21 83 B1 40 02 10 00 00 2D 43 0C 41 B0 13\r\n                                        86 16 B0 13 AC 17 81 4C 00 00 2D 43 0C 41 B0 13\r\n                                        86 16 2D 43 8C 00 FE 2B B0 13 EC 16 2D 43 8C 00\r\n                                        F6 2B B0 13 EC 16 92 D3 80 01 21 53 10 01 1B 14\r\n                                        21 83 0A 4C B2 43 54 01 40 18 1B 42 08 20 06 3C\r\n                                        1D 43 0C 41 B0 13 D4 15 E2 41 50 01 0F 4A 0A 4F\r\n                                        3A 53 0F 93 F5 23 40 18 82 4B 08 20 1C 42 54 01\r\n                                        21 53 1A 16 10 01 CE 0C 0F 3C 40 18 1F 42 06 20\r\n                                        40 18 EF 4E 06 1C AE 00 01 00 40 18 92 53 06 20\r\n                                        40 18 B2 F0 FF 03 06 20 0F 4D 0D 4F 3D 53 0F 93\r\n                                        04 24 B0 13 46 17 4C 93 E8 27 10 01 82 43 DA 05\r\n                                        D2 43 C0 05 B2 40 15 04 D2 05 B2 40 12 00 C6 05\r\n                                        B2 40 00 21 C8 05 B2 40 B1 C0 C0 05 D2 C3 C0 05\r\n                                        82 43 DC 05 B2 40 03 00 DA 05 10 01 32 C2 03 43\r\n                                        B2 40 00 A5 44 01 B2 40 40 A5 40 01 B0 13 D4 15\r\n                                        B2 40 00 A5 40 01 B2 40 10 A5 44 01 32 D2 10 01\r\n                                        40 18 92 42 06 20 08 20 40 18 C2 43 0E 20 40 18\r\n                                        C2 43 0F 20 B2 40 03 00 DA 05 10 01 21 83 1D 43\r\n                                        0C 41 B0 13 D4 15 4C 93 04 24 6F 41 4F 4F 82 4F\r\n                                        CE 05 21 53 10 01 40 18 1F 42 06 20 40 18 1F 82\r\n                                        08 20 3F 90 FF 03 02 20 5C 43 10 01 4C 43 10 01\r\n                                        00 18 4C 12 FC 4E 00 00 AC 00 01 00 BD 00 01 00\r\n                                        F9 23 00 18 7C 41 10 01 1F 15 0F 16 CE 0C EE 0F\r\n                                        04 3C CC 43 00 00 AC 00 01 00 DC 0E FA 23 10 01\r\n                                        40 18 92 92 08 20 06 20 02 20 5C 43 10 01 4C 43\r\n                                        10 01 0C 93 02 24 3C 53 FC 3F 10 01 8F 00 2C 10\r\n                                        0F 14 1C 17 10 01 80 00 86 16 68 22 22 68 08 FE\r\n                                        72 00 00 00 00 49 6A 88 04 00 00 00 00 0F 05 00\r\n                                        00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00\r\n                                        00 16 FF FF FF FF FF FF FF FF FF FF FF FF FF FF";
    private int bootLoaderVersion_8K_RAM = 16842757;
    private ushort bootStartAddressAt_8KRAM = 15354;
    private string bootLoader_8K_RAM = "   DA 9A 31 40 80 3B 3C 40 00 1C 3E 40 11 04 CC 43\r\n                                        00 00 1C 53 1E 83 FB 23 30 40 C2 10 8C 00 00 1C\r\n                                        3E 40 11 04 3F 40 00 00 B0 13 46 17 21 83 F2 B0\r\n                                        24 00 CA 05 2A 20 0A 12 0F 12 92 B3 DC 05 11 28\r\n                                        92 C3 DC 05 1A 42 CC 05 4C 4A B0 13 26 14 40 18\r\n                                        C2 4C 0F 20 40 18 D2 93 0F 20 15 20 92 C3 DA 05\r\n                                        12 3C B2 B2 DC 05 FD 2B A2 C3 DC 05 1D 43 0C 41\r\n                                        B0 13 DC 15 4C 93 05 24 6F 41 4F 4F 82 4F CE 05\r\n                                        02 3C B0 13 E0 16 3F 41 3A 41 21 53 00 13 32 C2\r\n                                        03 43 B2 B0 40 00 44 01 0F 7F 45 18 0F 5F 3F 50\r\n                                        40 A5 82 4F 44 01 B2 40 02 A5 40 01 CC 43 00 00\r\n                                        B2 40 00 A5 40 01 B2 40 10 A5 44 01 03 43 32 D2\r\n                                        10 01 B2 40 80 5A 5C 01 B0 13 B0 12 B0 13 9A 15\r\n                                        40 18 D2 43 10 20 05 3C 40 18 C2 43 0F 20 B0 13\r\n                                        24 11 40 18 C2 93 10 20 0B 24 40 18 C2 93 0F 20\r\n                                        F8 27 40 18 D2 93 0F 20 EF 27 B0 13 E0 16 F1 3F\r\n                                        8C 00 00 12 B0 13 8E 10 8C 00 00 14 B0 13 8E 10\r\n                                        8C 00 00 16 B0 13 8E 10 92 C3 80 01 B2 40 34 12\r\n                                        5C 01 10 01 5B 14 31 80 0E 00 81 43 0A 00 81 43\r\n                                        0C 00 40 18 1F 42 06 20 40 18 1F 82 08 20 3F F0\r\n                                        FF 03 3F 50 FD FF 81 4F 00 00 2C 41 B0 13 1E 16\r\n                                        81 4C 02 00 40 18 1F 42 06 20 3F 50 FD FF 40 18\r\n                                        5A 4F 06 1C 40 18 1F 42 06 20 2F 83 40 18 5F 4F\r\n                                        06 1C 8F 10 0A DF 0B 41 2B 52 1D 43 CC 0B B0 13\r\n                                        DC 15 08 41 38 50 0A 00 3D 40 03 00 CC 08 B0 13\r\n                                        DC 15 09 41 2D 43 CC 09 B0 13 DC 15 F1 90 53 00\r\n                                        04 00 09 24 81 9A 02 00 06 24 B0 13 E0 16 B2 40\r\n                                        1A 00 CE 05 79 3C 5E 41 04 00 4E 83 1C 24 5E 83\r\n                                        53 24 5E 83 5C 24 5E 83 64 24 7E 80 50 00 6C 20\r\n                                        40 18 1F 42 08 20 1F 53 40 18 FF 90 06 00 06 1C\r\n                                        07 20 B0 13 E0 16 B0 13 50 15 B0 13 2E 17 5C 3C\r\n                                        B0 13 E0 16 59 3C B0 13 E0 16 2E 41 3E 50 06 00\r\n                                        F1 40 68 00 06 00 C1 4E 07 00 8E 10 C1 4E 08 00\r\n                                        F1 40 68 00 09 00 0A 41 3A 50 06 00 86 00 54 16\r\n                                        2D 42 CC 0A 46 13 1D 43 CC 0B 46 13 3D 40 03 00\r\n                                        CC 08 46 13 2D 43 CC 09 46 13 2D 41 3C 01 0A 00\r\n                                        B0 13 84 17 2C 41 B0 13 1E 16 81 4C 02 00 2D 43\r\n                                        0C 41 2C 53 46 13 F1 40 16 00 06 00 1D 43 CC 0A\r\n                                        46 13 B0 13 2E 17 20 3C 2D 41 3C 01 0A 00 B0 13\r\n                                        BA 16 B0 13 E0 16 B2 40 E5 00 CE 05 15 3C 3C 01\r\n                                        0A 00 B0 13 8E 10 B0 13 E0 16 B2 40 E5 00 CE 05\r\n                                        0B 3C 40 18 92 41 0A 00 02 1C 40 18 92 41 0C 00\r\n                                        04 1C 40 18 C2 43 10 20 31 50 0E 00 56 16 10 01\r\n                                        1B 14 C2 43 42 02 C2 43 44 02 C2 43 46 02 C2 43\r\n                                        48 02 C2 43 4A 02 3C 40 0A 00 B0 13 70 17 0B 43\r\n                                        5A 42 40 02 5A 03 3F 40 10 00 05 3C 4E 4A 0E FF\r\n                                        0B DE 5A 03 5F 07 0F 93 F9 23 F2 D0 38 00 46 02\r\n                                        3C 40 0A 00 B0 13 70 17 5A 42 40 02 F2 D0 38 00\r\n                                        42 02 3C 40 0A 00 B0 13 70 17 5A E2 40 02 3F 40\r\n                                        20 00 05 3C 4E 4A 0E FF 0B DE 5A 03 5F 07 0F 93\r\n                                        F9 23 1E 43 0D 3C 4D 4B 7D F0 03 00 0F 4E 5D 83\r\n                                        02 30 CD 18 0F 5F 40 18 82 DF 00 1C 5B 07 5E 0E\r\n                                        3E 90 00 10 F0 3B F2 F0 C7 00 46 02 B2 40 52 2D\r\n                                        C0 01 F2 40 1F 00 CC 01 F2 40 1F 00 CD 01 E2 43\r\n                                        DF 01 D2 43 D1 01 A2 43 C2 01 C2 43 02 02 F2 40\r\n                                        C0 00 04 02 C2 43 06 02 C2 43 08 02 F2 40 30 00\r\n                                        0A 02 E2 43 0B 02 F2 40 80 00 2A 02 F2 40 80 00\r\n                                        22 02 F2 40 88 00 24 02 C2 43 42 02 F2 40 C7 00\r\n                                        44 02 C2 43 46 02 C2 43 48 02 C2 43 4A 02 C2 43\r\n                                        43 02 F2 43 45 02 C2 43 47 02 C2 43 49 02 C2 43\r\n                                        4B 02 C2 43 62 02 F2 43 64 02 C2 43 66 02 C2 43\r\n                                        68 02 C2 43 6A 02 F2 C0 10 00 63 02 F2 40 1F 00\r\n                                        65 02 C2 43 67 02 C2 43 69 02 C2 43 6B 02 B2 40\r\n                                        33 00 68 01 92 C3 6C 01 F2 D0 C0 00 6C 01 05 3C\r\n                                        B2 F0 F4 FF 6E 01 A2 C3 02 01 A2 B3 02 01 F8 2F\r\n                                        B2 F0 3F FF 6C 01 B0 13 8A 16 B0 13 F2 14 03 43\r\n                                        32 D2 1A 16 10 01 0A 14 4C 12 4A 43 40 18 5F 42\r\n                                        0E 20 E0 0F 09 3C 4D 3C 14 3C 1D 3C 33 3C 49 3C\r\n                                        48 3C 39 3C 46 3C 4D 3C B0 13 5E 17 4C 93 07 24\r\n                                        F1 90 68 00 00 00 03 20 40 18 E2 42 0E 20 4C 43\r\n                                        3D 3C 40 18 F2 40 06 00 0E 20 6F 41 40 18 82 4F\r\n                                        0A 20 4C 43 33 3C 40 18 F2 42 0E 20 40 18 B2 92\r\n                                        0A 20 03 20 F1 92 00 00 05 24 6F 41 8F 10 40 18\r\n                                        82 DF 0A 20 40 18 B2 90 00 04 0A 20 E0 2B 7C 40\r\n                                        05 00 1C 3C 7C 90 68 00 21 20 40 18 F2 40 0E 00\r\n                                        0E 20 4C 43 13 3C 40 18 1F 42 0A 20 0E 4F 3E 53\r\n                                        40 18 82 4E 0A 20 0F 93 04 20 40 18 F2 40 12 00\r\n                                        0E 20 1D 43 0C 41 B0 13 54 16 4C 4A 21 53 0A 16\r\n                                        10 01 7C 90 16 00 02 20 5A 43 F3 3F 6C 43 F6 3F\r\n                                        03 43 B2 40 1C 3B 00 0A B2 43 0A 0A B2 40 3F 00\r\n                                        0C 0A B2 40 20 00 08 0A 92 D3 00 0A 8F 00 20 0A\r\n                                        0E 43 04 3C 8F 43 00 00 EF 03 1E 53 3E 90 0A 00\r\n                                        F9 3B D2 43 21 0A F2 40 CB 00 22 0A F2 40 6E 00\r\n                                        23 0A F2 40 CD 00 24 0A F2 40 EE 00 25 0A F2 40\r\n                                        0F 00 26 0A F2 40 2F 00 27 0A D2 43 28 0A 10 01\r\n                                        31 80 28 00 0C 41 8E 00 88 17 8D 00 28 00 B0 13\r\n                                        16 17 40 18 D1 42 00 1C 18 00 40 18 1E 42 00 1C\r\n                                        8E 10 C1 4E 19 00 6E 42 05 3C 4F 4E 0F 51 E1 5F\r\n                                        26 00 5E 53 7E 90 26 00 F8 2B 3D 40 28 00 0C 41\r\n                                        B0 13 54 16 31 50 28 00 10 01 0A 14 21 83 B1 40\r\n                                        02 10 00 00 0A 41 2D 43 CC 0A B0 13 54 16 B0 13\r\n                                        7A 17 81 4C 00 00 2D 43 CC 0A B0 13 54 16 2D 43\r\n                                        8C 00 FE 3B B0 13 BA 16 2D 43 8C 00 F6 3B B0 13\r\n                                        BA 16 92 D3 80 01 21 53 0A 16 10 01 1B 14 CB 0C\r\n                                        0A 4D 10 3C 40 18 1F 42 08 20 40 18 DB 4F 06 1C\r\n                                        00 00 AB 00 01 00 40 18 92 53 08 20 40 18 B2 F0\r\n                                        FF 03 08 20 0F 4A 3A 53 0F 93 06 24 B0 13 5E 17\r\n                                        4C 93 E8 27 4C 43 01 3C 5C 43 1A 16 10 01 1B 14\r\n                                        21 83 0A 4C B2 43 54 01 40 18 1B 42 08 20 06 3C\r\n                                        1D 43 0C 41 B0 13 DC 15 E2 41 50 01 0F 4A 3A 53\r\n                                        0F 93 F6 23 40 18 82 4B 08 20 1C 42 54 01 21 53\r\n                                        1A 16 10 01 1B 14 CB 0C 0A 4D 0D 3C 40 18 1F 42\r\n                                        06 20 40 18 FF 4B 06 1C 40 18 92 53 06 20 40 18\r\n                                        B2 F0 FF 03 06 20 0F 4A 3A 53 0F 93 04 24 B0 13\r\n                                        FC 16 4C 93 EB 27 1A 16 10 01 82 43 DA 05 D2 43\r\n                                        C0 05 B2 40 15 04 D2 05 B2 40 12 00 C6 05 B2 40\r\n                                        00 21 C8 05 B2 40 B1 C0 C0 05 D2 C3 C0 05 82 43\r\n                                        DC 05 B2 40 03 00 DA 05 10 01 32 C2 03 43 B2 40\r\n                                        00 A5 44 01 B2 40 40 A5 40 01 B0 13 DC 15 B2 40\r\n                                        00 A5 40 01 B2 40 10 A5 44 01 03 43 32 D2 10 01\r\n                                        40 18 92 42 06 20 08 20 40 18 C2 43 0E 20 40 18\r\n                                        C2 43 0F 20 B2 40 03 00 DA 05 10 01 40 18 1F 42\r\n                                        06 20 40 18 1F 82 08 20 3F 90 FF 03 02 24 4C 43\r\n                                        10 01 5C 43 10 01 00 18 4C 12 FC 4E 00 00 AC 00\r\n                                        01 00 BD 00 01 00 F9 23 00 18 7C 41 10 01 21 83\r\n                                        1D 43 0C 41 B0 13 DC 15 4C 93 03 24 6F 41 82 4F\r\n                                        CE 05 21 53 10 01 1F 15 0F 16 CE 0C EE 0F 04 3C\r\n                                        CC 43 00 00 AC 00 01 00 DC 0E FA 23 10 01 40 18\r\n                                        92 92 08 20 06 20 02 24 4C 43 10 01 5C 43 10 01\r\n                                        0C 93 02 24 3C 53 FC 3F 10 01 3C 40 2C 10 3D 40\r\n                                        00 00 10 01 80 00 54 16 68 22 22 68 08 FE 72 00\r\n                                        00 00 00 49 6A 88 04 00 00 00 00 0F 05 00 01 01\r\n                                        00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16\r\n                                        FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF\r\n                                        FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF\r\n                                        FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF\r\n                                        FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF";

    public C5_FirmwareUpdate(S3_HandlerFunctions MyFunctions) => this.MyFunctions = MyFunctions;

    public C5_FirmwareUpdate(S3_HandlerFunctions MyFunctions, FirmwareUpdateSettings settings)
    {
      this.MyFunctions = MyFunctions;
      this.mySettings = settings;
    }

    internal void StartUpdate_To_FW_5_2_5()
    {
      C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Start update");
      this.MyFunctions.Clear();
      this.loadDatabaseWriteAccess();
      this.MyFunctions.MyCommands.ReadVersion(out this.meterVersionData);
      if ((long) this.meterVersionData.Version != (long) this.bootLoaderVersion_4K_RAM && (long) this.meterVersionData.Version != (long) this.bootLoaderVersion_8K_RAM)
      {
        if (this.meterVersionData.Version != 84025349U && this.meterVersionData.Version != 84029445U)
          throw new HandlerMessageException("FirmwareVersion is not made for this special Update: FW=0x" + this.meterVersionData.Version.ToString("X"));
        this.PrepareConnectedMeterForUpdate();
      }
      int startAddr = 27648;
      int endAddr = 81920;
      bool eraseInfoMem = false;
      string FirmwareFile = string.Empty;
      this.updateProgressMessage(0, "...Read Firmwarefile from Database");
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        string selectSql = "SELECT * FROM ProgFiles WHERE MapID = 73";
        DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection, out DbCommandBuilder _);
        HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
        dataAdapter.Fill((DataTable) progFilesDataTable);
        if (progFilesDataTable.Count > 0)
          FirmwareFile = progFilesDataTable[0].HexText;
      }
      if (FirmwareFile == string.Empty)
        throw new HandlerMessageException("FimwareFile is empty");
      this.Initialize_BootLoader();
      this.Erase_Firmware(20480, endAddr, eraseInfoMem);
      this.Write_Firmware(FirmwareFile, startAddr);
      this.StartFirmware();
      this.FinishConnectedDeviceUpdate();
      this.updateProgressMessage(100, "...Update complete!!!");
      this.reloadDatabaseWriteAccess();
    }

    private void PrepareConnectedMeterForUpdate()
    {
      C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Prepare connected meter for update");
      this.updateProgressMessage(0, "...read Connected-Meter");
      if (!this.MyFunctions.ReadConnectedDevice())
        throw new HandlerMessageException("Read original Meter failed!!!");
      C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Meter read ok. MeterID: " + this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.MeterId.ToString());
      this.updateProgressMessage(0, "...write Backup");
      if (!this.MyFunctions.SaveDevice())
        throw new HandlerMessageException("Safe original Backup failed!!!");
      uint meterId = this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.MeterId;
      string str = "BFix-FirmwareUpdate FW5.2.5. Meter is ";
      string pValue;
      if (this.MyFunctions.MyMeters.ConnectedMeter.IsWriteProtected)
      {
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Connected meter is protected");
        pValue = str + "protected";
      }
      else
      {
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Connected meter is not protected");
        pValue = str + "open";
      }
      this.SetDeviceProtectionToMeterData(meterId, pValue);
      this.GetMeterKeyFromDatabase(out this.meterKey);
      C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("MeterKey from data base: 0x" + this.meterKey.ToString("x08"));
      if (!this.dbPasswordIsDefined)
        throw new HandlerMessageException("dbPassword is not defined to recreate MeterKey!!!");
      this.updateProgressMessage(0, "...delete Deviceprotection");
      if (this.MyFunctions.MyCommands.DeviceProtectionGet())
      {
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Open device by sending MeterKey: 0x" + this.meterKey.ToString("x08"));
        if (!this.MyFunctions.MyCommands.DeviceProtectionReset(this.meterKey))
          throw new HandlerMessageException("Cannot open connected device!!!");
      }
      else
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Device is open");
      if (!this.MyFunctions.MyCommands.RunBackup())
        throw new HandlerMessageException("Cannot run Backup!!!");
      this.updateProgressMessage(0, "...read RecoveryData");
      if (!this.MyFunctions.MyCommands.ReadMemory(MemoryLocation.FLASH, 20480, 4096, out this.recoveryData))
        throw new HandlerMessageException("Cannot read RecoveryData!!!");
    }

    private void FinishConnectedDeviceUpdate()
    {
      C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Finish connected meter");
      if (this.recoveryData != null)
      {
        this.updateProgressMessage(99, "...write RecoveryData");
        if (!this.MyFunctions.MyCommands.WriteMemory(MemoryLocation.FLASH, 20480, this.recoveryData))
          throw new HandlerMessageException("Cannot write RecoveryData!!!");
      }
      if (this.meterKey == uint.MaxValue)
      {
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("meterKey == 0xffffffff");
        if (!this.MyFunctions.ReadConnectedDevice())
          throw new HandlerMessageException("Read Meter failed!!!");
        this.GetMeterKeyFromDatabase(out this.meterKey);
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("MeterKey from data base: 0x" + this.meterKey.ToString("x08"));
        if (!this.dbPasswordIsDefined)
          throw new HandlerMessageException("dbPassword is not defined to recreate MeterKey!!!");
      }
      C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Set MeterKey to device: 0x" + this.meterKey.ToString("x08"));
      if (!this.MyFunctions.MyCommands.DeviceProtectionSetKey(this.meterKey))
        throw new HandlerMessageException("Cannot set MeterKey!!!");
      if (this.MyFunctions.MyCommands.DeviceProtectionGet())
      {
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Reset device protection by MeterKey: 0x" + this.meterKey.ToString("x08"));
        if (!this.MyFunctions.MyCommands.DeviceProtectionReset(this.meterKey))
          throw new HandlerMessageException("Cannot open connected device!!!");
      }
      else
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Device is not protected");
      S3_DeviceId DeviceId;
      this.MyFunctions.MyMeters.ReadHardwareIdentification(out DeviceId);
      this.updateProgressMessage(99, "...read Connected-Meter");
      if (!this.MyFunctions.ReadConnectedDevice())
        throw new HandlerMessageException("Read original Meter failed!!!");
      this.MyFunctions.MyMeters.WorkMeter.MyIdentification.HardwareTypeId = DeviceId.HardwareTypeId;
      SortedList<string, S3_Parameter> parameterByName1 = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName;
      S3_ParameterNames s3ParameterNames = S3_ParameterNames.Con_HardwareTypeId;
      string key1 = s3ParameterNames.ToString();
      parameterByName1[key1].SetUintValue(DeviceId.HardwareTypeId);
      SortedList<string, S3_Parameter> parameterByName2 = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.ApprovalRevison;
      string key2 = s3ParameterNames.ToString();
      parameterByName2[key2].SetByteValue((byte) 9);
      this.updateProgressMessage(99, "...write HardwareTypeID and ApprovalRevision");
      if (!this.MyFunctions.WriteChangesToConnectedDevice())
        throw new HandlerMessageException("Write Meter failed!!!");
      if (this.GetDeviceProtectionFromMeterData((int) DeviceId.MeterId))
      {
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Device was protected on start firmware update. Set protection.");
        this.updateProgressMessage(99, "...set Deviceprotection");
        if (!this.MyFunctions.MyCommands.DeviceProtectionSet())
          throw new HandlerMessageException("Cannot set Deviceprotection!!!");
        if (!this.MyFunctions.MyCommands.RunBackup())
          throw new HandlerMessageException("Cannot run Backup!!!");
      }
      else
        C5_FirmwareUpdate.C5_FirmwareUpdateLogger.Trace("Device was not protected on start firmware update.");
      this.updateProgressMessage(99, "...read Connected-Meter");
      if (!this.MyFunctions.ReadConnectedDevice())
        throw new HandlerMessageException("Read original Meter failed!!!");
      this.updateProgressMessage(99, "...write Backup");
      if (!this.MyFunctions.SaveDevice())
        throw new HandlerMessageException("Safe original Backup failed!!!");
    }

    internal void StartUpdate()
    {
      if (this.mySettings == null)
        throw new HandlerMessageException("no Settings for Update");
      if (this.mySettings.firmwareFile == null || this.mySettings.firmwareFile == string.Empty)
        throw new HandlerMessageException("Firmwarefile is empty");
      this.loadDatabaseWriteAccess();
      this.MyFunctions.Clear();
      this.MyFunctions.ConnectDevice();
      this.MyFunctions.MyMeters.ConnectedMeter.LoadMapVars();
      this.MyFunctions.MyMeters.ConnectedMeter.MyParameters.AddressLables.TryGetValue("CSTACK", out this.mySettings.adr_cstack);
      if (!this.mySettings.ignoreOriginalMeter && !this.mySettings.firmwareOnly)
        this.ReadOriginalMeter();
      if (this.mySettings.firmwareOnly)
        this.GetMeterKeyFromDatabase(out this.meterKey);
      this.checkFirmwareCompatibility(this.mySettings.mapID, this.mySettings.firmwareOnly);
      this.Initialize_BootLoader();
      this.DoUpdate();
      this.StartFirmware();
      if (!this.mySettings.ignoreOriginalMeter && !this.mySettings.firmwareOnly)
        this.updateNewMeterConfigParameter();
      this.reloadDatabaseWriteAccess();
      this.updateProgressMessage(100, "update COMPLETE !!!");
    }

    private void StartFirmware()
    {
      ByteField DataBlock = new ByteField(this.prepareMBusBuffer(0, (byte) 3, 0));
      this.MyFunctions.MyCommands.SendBlock(ref DataBlock);
      Thread.Sleep(1000);
      int num = 0;
      while (!this.MyFunctions.MyCommands.ReadVersion(out this.meterVersionData) && num < 4)
        ++num;
      if (this.meterVersionData.Version == 0U || (long) this.meterVersionData.Version == (long) this.bootLoaderVersion_4K_RAM || (long) this.meterVersionData.Version == (long) this.bootLoaderVersion_8K_RAM)
        throw new ArgumentException("Firmware is not running!!!");
      byte[] numArray = new byte[3]
      {
        (byte) (this.meterVersionData.Version >> 24),
        (byte) (this.meterVersionData.Version >> 16),
        (byte) (this.meterVersionData.Version >> 12 & 15U)
      };
      this.updateProgressMessage(100, string.Format("...FW: {0}.{1}.{2} is running!", (object) numArray[0], (object) numArray[1], (object) numArray[2]));
    }

    private string _bootloadstarter
    {
      get => this.bootLoadHelper.Replace(" ", string.Empty).Replace("\r\n", string.Empty);
    }

    private string _bootloader
    {
      get
      {
        string str;
        switch (this.deviceDescriptor.Chip)
        {
          case S3_DeviceDescriptor.ChipDevices.F6736:
          case S3_DeviceDescriptor.ChipDevices.F6726:
            str = this.bootLoader_8K_RAM;
            break;
          case S3_DeviceDescriptor.ChipDevices.F6725:
          case S3_DeviceDescriptor.ChipDevices.F6724:
          case S3_DeviceDescriptor.ChipDevices.F6723:
            str = this.bootLoader_4K_RAM;
            break;
          default:
            throw new HandlerMessageException("device " + this.deviceDescriptor.Chip.ToString() + " not supported for firmwareupdate...");
        }
        return str.Replace(" ", string.Empty).Replace("\r\n", string.Empty);
      }
    }

    private ushort _RAMadr_for_BSLStartadr
    {
      get
      {
        if (this.mySettings.adr_cstack == 10752)
          return this.bootStartAddressAt_4KRAM;
        if (this.mySettings.adr_cstack == 14848)
          return this.bootStartAddressAt_8KRAM;
        throw new HandlerMessageException("device " + this.deviceDescriptor.Chip.ToString() + " not supported for firmwareupdate...");
      }
    }

    private void Initialize_BootLoader()
    {
      if (this.isBootLoaderRunning())
        return;
      this.deviceDescriptor = this.MyFunctions.ReadDeviceDescriptor();
      if (!this.WriteBootLoader())
        throw new HandlerMessageException("write bootloader failed! Check writepermission...");
      if (!this.StartBootLoader())
        throw new HandlerMessageException("start bootloader failed! Check writepermission...");
    }

    private bool WriteBootLoader()
    {
      this.updateProgressMessage(10, "...initialize Bootloadsector");
      if (!this.MyFunctions.MyCommands.SetEmergencyMode() || !this.MyFunctions.MyCommands.EraseFlash(20480, 4096))
        return false;
      this.updateProgressMessage(30, "...write BootLoaderStarter");
      ByteField byteField1 = new ByteField(Util.HexStringToByteArray(this._bootloadstarter));
      if (!this.MyFunctions.MyCommands.WriteMemory(MemoryLocation.FLASH, 20480, byteField1))
        return false;
      this.updateProgressMessage(35, "...verify BootLoaderStarter");
      this.VerifyMemory(byteField1, 20480, byteField1.Count);
      this.updateProgressMessage(70, "...write BootLoader");
      ByteField byteField2 = new ByteField(Util.HexStringToByteArray(this._bootloader));
      if (!this.MyFunctions.MyCommands.WriteMemory(MemoryLocation.FLASH, 20992, byteField2))
        return false;
      this.updateProgressMessage(75, "...verify BootLoader");
      this.VerifyMemory(byteField2, 20992, byteField2.Count);
      return true;
    }

    private bool StartBootLoader()
    {
      this.updateProgressMessage(90, "...start bootloader");
      ByteField data = new ByteField();
      data.Add(2);
      data.Add(80);
      if (!this.MyFunctions.MyCommands.WriteMemory(MemoryLocation.RAM, (int) this._RAMadr_for_BSLStartadr, data) || !this.isBootLoaderRunning())
        return false;
      this.updateProgressMessage(100, "...run bootloader");
      return true;
    }

    private bool isBootLoaderRunning()
    {
      int num = 0;
      ReadVersionData versionData;
      do
      {
        Thread.Sleep(200);
        this.MyFunctions.MyCommands.ReadVersion(out versionData);
        if (num++ > 5)
          return false;
      }
      while ((long) versionData.Version != (long) this.bootLoaderVersion_4K_RAM && (long) versionData.Version != (long) this.bootLoaderVersion_8K_RAM);
      this.bslVersionData = versionData;
      return true;
    }

    private void DoUpdate()
    {
      int startAddr = 16384;
      int endAddr = 81920;
      if ((long) this.bslVersionData.Version == (long) this.bootLoaderVersion_8K_RAM)
        endAddr = 147456;
      if (this.mySettings.firmwareOnly)
        startAddr = this.FindStartAddress(this.mySettings.firmwareFile, 24576);
      this.Erase_Firmware(startAddr, endAddr, this.mySettings.eraseInfoMemory);
      this.Write_Firmware(this.mySettings.firmwareFile, startAddr);
    }

    private int FindStartAddress(string FirmwareFile, int threshold)
    {
      StringReader stringReader = new StringReader(FirmwareFile);
      string str = string.Empty;
      while (!str.Contains("q"))
      {
        str = stringReader.ReadLine();
        if (str.Contains("@"))
        {
          str = str.Replace("@", string.Empty);
          int uint16 = (int) Convert.ToUInt16(str, 16);
          if (uint16 >= threshold)
            return uint16;
        }
      }
      throw new ArgumentException("find startAddress failed!!!");
    }

    private void Erase_Firmware(int startAddr, int endAddr, bool eraseInfoMem)
    {
      if (eraseInfoMem)
        this.C5_EraseInfoMemory();
      this.C5_EraseFlashMemory(startAddr, endAddr);
    }

    private void C5_EraseInfoMemory()
    {
      int flashAddress = 6144;
      this.C5_EraseSegment(flashAddress);
      this.C5_EraseSegment(flashAddress + 128);
      this.C5_EraseSegment(flashAddress + 256);
      this.C5_EraseSegment(flashAddress + 384);
    }

    private void C5_EraseFlashMemory(int startAddr, int endAddr)
    {
      int flashAddress = startAddr;
      int num = endAddr - startAddr;
      int Progress1 = flashAddress * 100 / num;
      string Message1 = string.Format("{0}%...erase Firmware", (object) Progress1);
      this.updateProgressMessage(Progress1, Message1);
      for (; flashAddress < endAddr; flashAddress += 512)
      {
        int Progress2 = (flashAddress - startAddr) * 100 / num;
        string Message2 = string.Format("{0}%...erase Firmware", (object) Progress2);
        this.updateProgressMessage(Progress2, Message2);
        this.C5_EraseSegment(flashAddress);
      }
      this.updateProgressMessage(100, string.Format("{0}%...erase Firmware", (object) 100));
    }

    private void C5_EraseSegment(int flashAddress)
    {
      ByteField DataBlock = new ByteField(this.prepareMBusBuffer(flashAddress, (byte) 2, 0));
      int num = 0;
      while (num++ < 4)
      {
        this.MyFunctions.MyCommands.SendBlock(ref DataBlock);
        if (this.ReceiveAckNack())
          return;
      }
      throw new HandlerMessageException("erase Segment @ 0x" + flashAddress.ToString("X") + " failed!!!");
    }

    private void Write_Firmware(string FirmwareFile, int startAddr)
    {
      StringReader stringReader = !(FirmwareFile == string.Empty) ? new StringReader(FirmwareFile) : throw new HandlerMessageException("FimwareFile is empty");
      int flashAddress = 0;
      string empty = string.Empty;
      string str = string.Empty;
      int Progress1 = flashAddress * 100 / 147456;
      string Message1 = string.Format("{0}%...write Firmware", (object) Progress1);
      this.updateProgressMessage(Progress1, Message1);
      while (!str.Contains("q"))
      {
        while (empty.Length + str.Length <= 1024)
        {
          if (str.Contains("@"))
          {
            flashAddress = (int) Convert.ToUInt16(str.Replace("@", string.Empty), 16);
            empty = string.Empty;
          }
          else if (flashAddress >= startAddr)
            empty += str;
          str = stringReader.ReadLine().Replace(" ", string.Empty).Replace("\r\n", string.Empty);
          if (str.Contains("@") || str.Contains("q"))
            break;
        }
        if (empty != string.Empty)
        {
          byte[] byteArray = Util.HexStringToByteArray(empty);
          this.C5_WriteFlash(flashAddress, byteArray);
          int Progress2 = flashAddress * 100 / 147456;
          string Message2 = string.Format("{0}%...write Firmware", (object) Progress2);
          this.updateProgressMessage(Progress2, Message2);
          flashAddress += byteArray.Length;
          empty = string.Empty;
        }
      }
      this.updateProgressMessage(100, string.Format("{0}%...write Firmware", (object) 100));
    }

    private void C5_WriteFlash(int flashAddress, byte[] fwData)
    {
      ByteField buffer = new ByteField(this.prepareMBusBuffer(flashAddress, fwData));
      int num = 0;
      while (num++ < 4)
      {
        this.MyFunctions.MyCommands.TransmitBlock(ref buffer);
        if (this.ReceiveAckNack())
          return;
      }
      throw new HandlerMessageException("write Memory failed");
    }

    private bool ValidateUpdate() => throw new NotImplementedException();

    private void ReadOriginalMeter()
    {
      if (this.isBootLoaderRunning())
        return;
      this.updateProgressMessage(0, "...read original Meter");
      this.MyFunctions._meterBackupOnRead = true;
      if (!this.MyFunctions.ReadConnectedDevice())
        throw new ArgumentException("read original Meter failed!!!");
      this.MyFunctions.MyMeters.DbMeter = this.MyFunctions.MyMeters.ConnectedMeter;
    }

    private void updateNewMeterConfigParameter()
    {
      this.updateProgressMessage(100, "...read new Meter");
      this.MyFunctions._meterBackupOnRead = false;
      if (!this.MyFunctions.ReadConnectedDevice())
        throw new ArgumentException("read new Meter failed!!!");
      if (this.MyFunctions.MyMeters.DbMeter != null)
      {
        this.updateProgressMessage(100, "...reconstruct FirmwareParameter");
        if (!this.updateParameter())
          throw new ArgumentException("reconstruct FirmwareParameter failed!!!");
        this.updateProgressMessage(100, "...write Parameter");
        if (!this.MyFunctions.WriteChangesToConnectedDevice())
          throw new ArgumentException("write Parameter failed!!!");
      }
      else
        ZR_ClassLibMessages.AddWarning("FirmwareUpdate succeed. ConfigParameter are NOT updated!!!");
    }

    private bool updateParameter()
    {
      uint uintValue = this.MyFunctions.MyMeters.DbMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_MeterId.ToString()].GetUintValue();
      this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_MeterId.ToString()].SetUintValue(uintValue);
      this.MyFunctions.MyMeters.WorkMeter.MyIdentification.MeterId = uintValue;
      this.MyFunctions.MyMeters.DbMeter.MyIdentification.MapId = this.MyFunctions.MyMeters.WorkMeter.MyIdentification.MapId;
      bool[] overwriteSelection = new bool[21];
      overwriteSelection[10] = true;
      overwriteSelection[9] = true;
      return this.MyFunctions.MyMeters.OverwriteFromType.OverwriteRun(this.MyFunctions.MyMeters.DbMeter, overwriteSelection, OverwriteOptions.None);
    }

    private void updateProgressMessage(int Progress, string Message)
    {
      this.MyFunctions.SetMessageInfo(Message);
      this.MyFunctions.SendMessage(Progress, GMM_EventArgs.MessageType.C5_FirmwareUpdate);
    }

    private byte[] prepareMBusBuffer(int flashAddress, byte cmd, int size)
    {
      if (cmd < (byte) 0 || cmd > (byte) 3)
        throw new IndexOutOfRangeException("cmd out of range");
      byte[] buffer = new byte[6]
      {
        cmd,
        (byte) flashAddress,
        (byte) (flashAddress >> 8),
        (byte) (flashAddress >> 16),
        (byte) size,
        (byte) (size >> 8)
      };
      ushort num = Util.CalculatesCRC16_CC430(buffer);
      return new byte[13]
      {
        (byte) 104,
        (byte) 7,
        (byte) 0,
        (byte) 104,
        buffer[0],
        buffer[1],
        buffer[2],
        buffer[3],
        buffer[4],
        buffer[5],
        (byte) num,
        (byte) ((uint) num >> 8),
        (byte) 22
      };
    }

    private byte[] prepareMBusBuffer(int flashAddress, byte[] fwData)
    {
      int num1 = fwData.Length + 7;
      byte[] buffer = new byte[fwData.Length + 6];
      buffer[0] = (byte) 1;
      buffer[1] = (byte) flashAddress;
      buffer[2] = (byte) (flashAddress >> 8);
      buffer[3] = (byte) (flashAddress >> 16);
      buffer[4] = (byte) fwData.Length;
      buffer[5] = (byte) (fwData.Length >> 8);
      for (int index = 0; index < fwData.Length; ++index)
        buffer[index + 6] = fwData[index];
      ushort num2 = Util.CalculatesCRC16_CC430(buffer);
      byte[] numArray = new byte[buffer.Length + 7];
      numArray[0] = (byte) 104;
      numArray[1] = (byte) num1;
      numArray[2] = (byte) (num1 >> 8);
      numArray[3] = (byte) 104;
      for (int index = 0; index < buffer.Length; ++index)
        numArray[index + 4] = buffer[index];
      numArray[buffer.Length + 4] = (byte) num2;
      numArray[buffer.Length + 5] = (byte) ((uint) num2 >> 8);
      numArray[buffer.Length + 6] = (byte) 22;
      return numArray;
    }

    private bool ReceiveAckNack()
    {
      ByteField DataBlock = new ByteField();
      if (!this.MyFunctions.MyCommands.ReceiveBlock(ref DataBlock, 1, true))
        return false;
      if (DataBlock.Data[0] == (byte) 229)
        return true;
      return DataBlock.Data[0] == (byte) 26 && false;
    }

    private void checkFirmwareCompatibility(int MapId, bool firmwareOnly)
    {
      if (this.isBootLoaderRunning())
        return;
      this.MyFunctions.MyCommands.ReadVersion(out this.meterVersionData);
      string hardwareString = ParameterService.GetHardwareString(this.meterVersionData.hardwareIdentification);
      if (hardwareString.Contains("Radio") && MapId < 59)
        throw new ArgumentException("selected firmware_version and connected hardware (" + hardwareString + ") are not compatible!!!");
      if ((hardwareString.Contains("Ultrasonic") || hardwareString.Contains("UltrasonicDirect")) && MapId < 58)
        throw new ArgumentException("selected firmware_version and connected hardware (" + hardwareString + ") are not compatible!!!");
      if (firmwareOnly && this.meterVersionData.Version != 84025349U && this.meterVersionData.Version != 84029445U)
        throw new ArgumentException("Update firmware only is not allowed!!!");
    }

    private void loadDatabaseWriteAccess()
    {
      this.backupOnRead = this.MyFunctions._meterBackupOnRead;
      this.onlyOnePerday = this.MyFunctions._onlyOneReadBackupPerDay;
      this.backupOnWrite = this.MyFunctions._meterBackupOnWrite;
      this.MyFunctions._meterBackupOnRead = false;
      this.MyFunctions._onlyOneReadBackupPerDay = false;
      this.MyFunctions._meterBackupOnWrite = false;
    }

    private void reloadDatabaseWriteAccess()
    {
      this.MyFunctions._meterBackupOnRead = this.backupOnRead;
      this.MyFunctions._onlyOneReadBackupPerDay = this.onlyOnePerday;
      this.MyFunctions._meterBackupOnWrite = this.backupOnWrite;
    }

    private void GetMeterKeyFromDatabase(out uint meterKey)
    {
      this.updateProgressMessage(0, "...recreate MeterKey");
      uint userKeyChecksum = (uint) UserRights.GlobalUserRights.GetUserKeyChecksum("ZelsiusLockKey");
      this.dbPasswordIsDefined = userKeyChecksum > 0U;
      MeterDBAccess.ValueTypes ValueType;
      if (!this.MyFunctions.MyDatabase.GetDeviceKeys((int) this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.MeterId, out meterKey, out ValueType) || ValueType != MeterDBAccess.ValueTypes.GovernmentRandomNr)
        return;
      meterKey ^= userKeyChecksum;
    }

    private void SetDeviceProtectionToMeterData(uint meterID, string pValue)
    {
      string SQLCommand = "SELECT * FROM MeterData WHERE ((MeterId = " + meterID.ToString() + ") and (PValueID = " + 716.ToString() + "))";
      Schema.MeterDataDataTable Table = new Schema.MeterDataDataTable();
      this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand, (DataTable) Table);
      if (Table.Count != 0)
        return;
      BaseDbConnection baseDb = this.MyFunctions.MyDatabase.BaseDb;
      using (DbConnection newConnection = baseDb.GetNewConnection())
      {
        newConnection.Open();
        DbTransaction transaction = newConnection.BeginTransaction();
        DbDataAdapter dataAdapter = baseDb.GetDataAdapter("SELECT * FROM MeterData", newConnection, transaction, out DbCommandBuilder _);
        Schema.MeterDataRow row = Table.NewMeterDataRow();
        row.MeterID = (int) meterID;
        DateTime now = DateTime.Now;
        row.TimePoint = now.AddMilliseconds((double) (now.Millisecond * -1));
        row.PValueID = 716;
        row.PValue = pValue;
        Table.AddMeterDataRow(row);
        dataAdapter.Update((DataTable) Table);
        transaction.Commit();
        newConnection.Close();
      }
    }

    private bool GetDeviceProtectionFromMeterData(int meterID)
    {
      bool protectionFromMeterData = true;
      string SQLCommand = "SELECT * FROM MeterData WHERE ((MeterId = " + meterID.ToString() + ") and (PValueID = " + 716.ToString() + "))  ORDER BY TimePoint DESC";
      Schema.MeterDataDataTable Table = new Schema.MeterDataDataTable();
      this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand, (DataTable) Table);
      if (Table.Count == 0)
        throw new HandlerMessageException("Update-Entry in MeterData is missing!!!");
      if (Table[0].PValue.IndexOf("open") != -1)
        protectionFromMeterData = false;
      return protectionFromMeterData;
    }

    private void VerifyMemory(ByteField memToCompare, int memAddress, int size)
    {
      ByteField MemoryData;
      if (!this.MyFunctions.MyCommands.ReadMemory(MemoryLocation.FLASH, memAddress, size, out MemoryData))
        throw new HandlerMessageException("Cannot read Memory!!!");
      for (int index = 0; index < memToCompare.Count; ++index)
      {
        if ((int) MemoryData.Data[index] != (int) memToCompare.Data[index])
          throw new HandlerMessageException("Verify failed!!! \r\nMemAdr.: 0x" + (memAddress + index).ToString("X"));
      }
    }
  }
}
