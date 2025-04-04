// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.BIFFRECORDTYPE
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal enum BIFFRECORDTYPE : ushort
  {
    BLANK_OLD = 1,
    INTEGER_OLD = 2,
    NUMBER_OLD = 3,
    LABEL_OLD = 4,
    BOOLERR_OLD = 5,
    FORMULA_OLD = 6,
    BOF_V2 = 9,
    EOF = 10, // 0x000A
    CALCCOUNT = 12, // 0x000C
    CALCMODE = 13, // 0x000D
    PRECISION = 14, // 0x000E
    REFMODE = 15, // 0x000F
    DELTA = 16, // 0x0010
    ITERATION = 17, // 0x0011
    PROTECT = 18, // 0x0012
    PASSWORD = 19, // 0x0013
    HEADER = 20, // 0x0014
    FOOTER = 21, // 0x0015
    WINDOWPROTECT = 25, // 0x0019
    VERTICALPAGEBREAKS = 26, // 0x001A
    NOTE = 28, // 0x001C
    SELECTION = 29, // 0x001D
    FORMAT_V23 = 30, // 0x001E
    RECORD1904 = 34, // 0x0022
    PRINTHEADERS = 42, // 0x002A
    PRINTGRIDLINES = 43, // 0x002B
    FONT = 49, // 0x0031
    CONTINUE = 60, // 0x003C
    WINDOW1 = 61, // 0x003D
    BACKUP = 64, // 0x0040
    CODEPAGE = 66, // 0x0042
    XF_V2 = 67, // 0x0043
    DFAULTCOLWIDTH = 85, // 0x0055
    XCT = 89, // 0x0059
    WRITEACCESS = 92, // 0x005C
    OBJ = 93, // 0x005D
    UNCALCED = 94, // 0x005E
    SAVERECALC = 95, // 0x005F
    GUTS = 128, // 0x0080
    WSBOOL = 129, // 0x0081
    GRIDSET = 130, // 0x0082
    HCENTER = 131, // 0x0083
    VCENTER = 132, // 0x0084
    BOUNDSHEET = 133, // 0x0085
    COUNTRY = 140, // 0x008C
    HIDEOBJ = 141, // 0x008D
    FNGROUPCOUNT = 156, // 0x009C
    PRINTSETUP = 161, // 0x00A1
    SHRFMLA_OLD = 188, // 0x00BC
    MULRK = 189, // 0x00BD
    MULBLANK = 190, // 0x00BE
    MMS = 193, // 0x00C1
    SXDB = 198, // 0x00C6
    RSTRING = 214, // 0x00D6
    DBCELL = 215, // 0x00D7
    BOOKBOOL = 218, // 0x00DA
    PARAMQRY = 220, // 0x00DC
    SXEXT = 220, // 0x00DC
    XF = 224, // 0x00E0
    INTERFACEHDR = 225, // 0x00E1
    INTERFACEEND = 226, // 0x00E2
    MSODRAWINGGROUP = 235, // 0x00EB
    MSODRAWING = 236, // 0x00EC
    MSODRAWINGSELECTION = 237, // 0x00ED
    SXRULE = 240, // 0x00F0
    SXEX = 241, // 0x00F1
    SXFILT = 242, // 0x00F2
    SXNAME = 246, // 0x00F6
    SXSELECT = 247, // 0x00F7
    SXPAIR = 248, // 0x00F8
    SXFMLA = 249, // 0x00F9
    SXFORMAT = 251, // 0x00FB
    SST = 252, // 0x00FC
    LABELSST = 253, // 0x00FD
    EXTSST = 255, // 0x00FF
    SXVDEX = 256, // 0x0100
    SXFORMULA = 259, // 0x0103
    SXDBEX = 290, // 0x0122
    TABID = 317, // 0x013D
    USESELFS = 352, // 0x0160
    DSF = 353, // 0x0161
    XL5MODIFY = 354, // 0x0162
    USERBVIEW = 425, // 0x01A9
    USERSVIEWBEGIN = 426, // 0x01AA
    USERSVIEWEND = 427, // 0x01AB
    QSI = 429, // 0x01AD
    SUPBOOK = 430, // 0x01AE
    PROT4REV = 431, // 0x01AF
    CONDFMT = 432, // 0x01B0
    CF = 433, // 0x01B1
    DVAL = 434, // 0x01B2
    DCONBIN = 437, // 0x01B5
    TXO = 438, // 0x01B6
    REFRESHALL = 439, // 0x01B7
    HLINK = 440, // 0x01B8
    CODENAME = 442, // 0x01BA
    SXFDBTYPE = 443, // 0x01BB
    PROT4REVPASSWORD = 444, // 0x01BC
    DV = 446, // 0x01BE
    DIMENSIONS = 512, // 0x0200
    BLANK = 513, // 0x0201
    INTEGER = 514, // 0x0202
    NUMBER = 515, // 0x0203
    LABEL = 516, // 0x0204
    BOOLERR = 517, // 0x0205
    STRING = 519, // 0x0207
    ROW = 520, // 0x0208
    BOF_V3 = 521, // 0x0209
    INDEX = 523, // 0x020B
    ARRAY = 545, // 0x0221
    DEFAULTROWHEIGHT = 549, // 0x0225
    FONT_V34 = 561, // 0x0231
    WINDOW2 = 574, // 0x023E
    XF_V3 = 579, // 0x0243
    RK = 638, // 0x027E
    STYLE = 659, // 0x0293
    FORMULA = 1030, // 0x0406
    BOF_V4 = 1033, // 0x0409
    FORMAT = 1054, // 0x041E
    XF_V4 = 1091, // 0x0443
    SHRFMLA = 1212, // 0x04BC
    QUICKTIP = 2048, // 0x0800
    BOF = 2057, // 0x0809
  }
}
