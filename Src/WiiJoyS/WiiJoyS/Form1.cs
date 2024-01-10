using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Interceptor;
namespace WiiJoyS
{
    public partial class Form1 : Form
    {
        [DllImport("advapi32.dll")]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        [DllImport("WiimotePairing.dll", EntryPoint = "connect")]
        private static extern bool connect();
        [DllImport("WiimotePairing.dll", EntryPoint = "disconnect")]
        private static extern bool disconnect();
        [DllImport("LeftJoyconPairing.dll", EntryPoint = "disconnectLeft")]
        private static extern bool disconnectLeft();
        [DllImport("LeftJoyconPairing.dll", EntryPoint = "lconnect")]
        private static extern bool lconnect();
        [DllImport("hid.dll")]
        private static extern void HidD_GetHidGuid(out Guid gHid);
        [DllImport("hid.dll")]
        private extern static bool HidD_SetOutputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);
        [DllImport("setupapi.dll")]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string Enumerator, IntPtr hwndParent, UInt32 Flags);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInvo, ref Guid interfaceClassGuid, Int32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("Kernel32.dll")]
        private static extern SafeFileHandle CreateFile(string fileName, [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess, [MarshalAs(UnmanagedType.U4)] FileShare fileShare, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] uint flags, IntPtr template);
        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateFile(string fileName, System.IO.FileAccess fileAccess, System.IO.FileShare fileShare, IntPtr securityAttributes, System.IO.FileMode creationDisposition, EFileAttributes flags, IntPtr template);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_read_timeout")]
        private static extern int Lhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_write")]
        private static extern int Lhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_open_path")]
        private static extern SafeFileHandle Lhid_open_path(IntPtr handle);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_close")]
        private static extern void Lhid_close(SafeFileHandle device);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        [DllImport("mouse.dll", EntryPoint = "MoveMouseTo", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MoveMouseTo(int x, int y);
        [DllImport("mouse.dll", EntryPoint = "MoveMouseBy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MoveMouseBy(int x, int y);
        [DllImport("keyboard.dll", EntryPoint = "SendKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendKey(UInt16 bVk, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SendKeyF", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendKeyF(UInt16 bVk, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SendKeyArrows", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendKeyArrows(UInt16 bVk, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SendKeyArrowsF", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendKeyArrowsF(UInt16 bVk, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonLeft", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendMouseEventButtonLeft();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonLeftF", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendMouseEventButtonLeftF();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonRight", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendMouseEventButtonRight();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonRightF", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendMouseEventButtonRightF();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonMiddle", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendMouseEventButtonMiddle();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonMiddleF", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendMouseEventButtonMiddleF();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonWheelUp", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendMouseEventButtonWheelUp();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonWheelDown", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendMouseEventButtonWheelDown();
        [DllImport("mouse.dll", EntryPoint = "MouseMW3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseMW3(int x, int y);
        [DllImport("mouse.dll", EntryPoint = "MouseBrink", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseBrink(int x, int y);
        [DllImport("keyboard.dll", EntryPoint = "SimulateKeyDown", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyDown(UInt16 keyCode, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SimulateKeyUp", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyUp(UInt16 keyCode, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SimulateKeyDownArrows", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyDownArrows(UInt16 keyCode, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SimulateKeyUpArrows", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyUpArrows(UInt16 keyCode, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "LeftClick", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LeftClick();
        [DllImport("keyboard.dll", EntryPoint = "LeftClickF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LeftClickF();
        [DllImport("keyboard.dll", EntryPoint = "RightClick", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RightClick();
        [DllImport("keyboard.dll", EntryPoint = "RightClickF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RightClickF();
        [DllImport("keyboard.dll", EntryPoint = "MiddleClick", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiddleClick();
        [DllImport("keyboard.dll", EntryPoint = "MiddleClickF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiddleClickF();
        [DllImport("keyboard.dll", EntryPoint = "WheelDownF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WheelDownF();
        [DllImport("keyboard.dll", EntryPoint = "WheelUpF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WheelUpF();
        [DllImport("user32.dll")]
        private static extern void SetPhysicalCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        private static extern void SetCaretPos(int X, int Y);
        [DllImport("user32.dll")]
        private static extern void SetCursorPos(int X, int Y);
        private const ushort VK_LBUTTON = (ushort)0x01;
        private const ushort VK_RBUTTON = (ushort)0x02;
        private const ushort VK_CANCEL = (ushort)0x03;
        private const ushort VK_MBUTTON = (ushort)0x04;
        private const ushort VK_XBUTTON1 = (ushort)0x05;
        private const ushort VK_XBUTTON2 = (ushort)0x06;
        private const ushort VK_BACK = (ushort)0x08;
        private const ushort VK_Tab = (ushort)0x09;
        private const ushort VK_CLEAR = (ushort)0x0C;
        private const ushort VK_Return = (ushort)0x0D;
        private const ushort VK_SHIFT = (ushort)0x10;
        private const ushort VK_CONTROL = (ushort)0x11;
        private const ushort VK_MENU = (ushort)0x12;
        private const ushort VK_PAUSE = (ushort)0x13;
        private const ushort VK_CAPITAL = (ushort)0x14;
        private const ushort VK_KANA = (ushort)0x15;
        private const ushort VK_HANGEUL = (ushort)0x15;
        private const ushort VK_HANGUL = (ushort)0x15;
        private const ushort VK_JUNJA = (ushort)0x17;
        private const ushort VK_FINAL = (ushort)0x18;
        private const ushort VK_HANJA = (ushort)0x19;
        private const ushort VK_KANJI = (ushort)0x19;
        private const ushort VK_Escape = (ushort)0x1B;
        private const ushort VK_CONVERT = (ushort)0x1C;
        private const ushort VK_NONCONVERT = (ushort)0x1D;
        private const ushort VK_ACCEPT = (ushort)0x1E;
        private const ushort VK_MODECHANGE = (ushort)0x1F;
        private const ushort VK_Space = (ushort)0x20;
        private const ushort VK_PRIOR = (ushort)0x21;
        private const ushort VK_NEXT = (ushort)0x22;
        private const ushort VK_END = (ushort)0x23;
        private const ushort VK_HOME = (ushort)0x24;
        private const ushort VK_LEFT = (ushort)0x25;
        private const ushort VK_UP = (ushort)0x26;
        private const ushort VK_RIGHT = (ushort)0x27;
        private const ushort VK_DOWN = (ushort)0x28;
        private const ushort VK_SELECT = (ushort)0x29;
        private const ushort VK_PRINT = (ushort)0x2A;
        private const ushort VK_EXECUTE = (ushort)0x2B;
        private const ushort VK_SNAPSHOT = (ushort)0x2C;
        private const ushort VK_INSERT = (ushort)0x2D;
        private const ushort VK_DELETE = (ushort)0x2E;
        private const ushort VK_HELP = (ushort)0x2F;
        private const ushort VK_APOSTROPHE = (ushort)0xDE;
        private const ushort VK_0 = (ushort)0x30;
        private const ushort VK_1 = (ushort)0x31;
        private const ushort VK_2 = (ushort)0x32;
        private const ushort VK_3 = (ushort)0x33;
        private const ushort VK_4 = (ushort)0x34;
        private const ushort VK_5 = (ushort)0x35;
        private const ushort VK_6 = (ushort)0x36;
        private const ushort VK_7 = (ushort)0x37;
        private const ushort VK_8 = (ushort)0x38;
        private const ushort VK_9 = (ushort)0x39;
        private const ushort VK_A = (ushort)0x41;
        private const ushort VK_B = (ushort)0x42;
        private const ushort VK_C = (ushort)0x43;
        private const ushort VK_D = (ushort)0x44;
        private const ushort VK_E = (ushort)0x45;
        private const ushort VK_F = (ushort)0x46;
        private const ushort VK_G = (ushort)0x47;
        private const ushort VK_H = (ushort)0x48;
        private const ushort VK_I = (ushort)0x49;
        private const ushort VK_J = (ushort)0x4A;
        private const ushort VK_K = (ushort)0x4B;
        private const ushort VK_L = (ushort)0x4C;
        private const ushort VK_M = (ushort)0x4D;
        private const ushort VK_N = (ushort)0x4E;
        private const ushort VK_O = (ushort)0x4F;
        private const ushort VK_P = (ushort)0x50;
        private const ushort VK_Q = (ushort)0x51;
        private const ushort VK_R = (ushort)0x52;
        private const ushort VK_S = (ushort)0x53;
        private const ushort VK_T = (ushort)0x54;
        private const ushort VK_U = (ushort)0x55;
        private const ushort VK_V = (ushort)0x56;
        private const ushort VK_W = (ushort)0x57;
        private const ushort VK_X = (ushort)0x58;
        private const ushort VK_Y = (ushort)0x59;
        private const ushort VK_Z = (ushort)0x5A;
        private const ushort VK_LWIN = (ushort)0x5B;
        private const ushort VK_RWIN = (ushort)0x5C;
        private const ushort VK_APPS = (ushort)0x5D;
        private const ushort VK_SLEEP = (ushort)0x5F;
        private const ushort VK_NUMPAD0 = (ushort)0x60;
        private const ushort VK_NUMPAD1 = (ushort)0x61;
        private const ushort VK_NUMPAD2 = (ushort)0x62;
        private const ushort VK_NUMPAD3 = (ushort)0x63;
        private const ushort VK_NUMPAD4 = (ushort)0x64;
        private const ushort VK_NUMPAD5 = (ushort)0x65;
        private const ushort VK_NUMPAD6 = (ushort)0x66;
        private const ushort VK_NUMPAD7 = (ushort)0x67;
        private const ushort VK_NUMPAD8 = (ushort)0x68;
        private const ushort VK_NUMPAD9 = (ushort)0x69;
        private const ushort VK_MULTIPLY = (ushort)0x6A;
        private const ushort VK_ADD = (ushort)0x6B;
        private const ushort VK_SEPARATOR = (ushort)0x6C;
        private const ushort VK_SUBTRACT = (ushort)0x6D;
        private const ushort VK_DECIMAL = (ushort)0x6E;
        private const ushort VK_DIVIDE = (ushort)0x6F;
        private const ushort VK_F1 = (ushort)0x70;
        private const ushort VK_F2 = (ushort)0x71;
        private const ushort VK_F3 = (ushort)0x72;
        private const ushort VK_F4 = (ushort)0x73;
        private const ushort VK_F5 = (ushort)0x74;
        private const ushort VK_F6 = (ushort)0x75;
        private const ushort VK_F7 = (ushort)0x76;
        private const ushort VK_F8 = (ushort)0x77;
        private const ushort VK_F9 = (ushort)0x78;
        private const ushort VK_F10 = (ushort)0x79;
        private const ushort VK_F11 = (ushort)0x7A;
        private const ushort VK_F12 = (ushort)0x7B;
        private const ushort VK_F13 = (ushort)0x7C;
        private const ushort VK_F14 = (ushort)0x7D;
        private const ushort VK_F15 = (ushort)0x7E;
        private const ushort VK_F16 = (ushort)0x7F;
        private const ushort VK_F17 = (ushort)0x80;
        private const ushort VK_F18 = (ushort)0x81;
        private const ushort VK_F19 = (ushort)0x82;
        private const ushort VK_F20 = (ushort)0x83;
        private const ushort VK_F21 = (ushort)0x84;
        private const ushort VK_F22 = (ushort)0x85;
        private const ushort VK_F23 = (ushort)0x86;
        private const ushort VK_F24 = (ushort)0x87;
        private const ushort VK_NUMLOCK = (ushort)0x90;
        private const ushort VK_SCROLL = (ushort)0x91;
        private const ushort VK_LeftShift = (ushort)0xA0;
        private const ushort VK_RightShift = (ushort)0xA1;
        private const ushort VK_LeftControl = (ushort)0xA2;
        private const ushort VK_RightControl = (ushort)0xA3;
        private const ushort VK_LMENU = (ushort)0xA4;
        private const ushort VK_RMENU = (ushort)0xA5;
        private const ushort VK_BROWSER_BACK = (ushort)0xA6;
        private const ushort VK_BROWSER_FORWARD = (ushort)0xA7;
        private const ushort VK_BROWSER_REFRESH = (ushort)0xA8;
        private const ushort VK_BROWSER_STOP = (ushort)0xA9;
        private const ushort VK_BROWSER_SEARCH = (ushort)0xAA;
        private const ushort VK_BROWSER_FAVORITES = (ushort)0xAB;
        private const ushort VK_BROWSER_HOME = (ushort)0xAC;
        private const ushort VK_VOLUME_MUTE = (ushort)0xAD;
        private const ushort VK_VOLUME_DOWN = (ushort)0xAE;
        private const ushort VK_VOLUME_UP = (ushort)0xAF;
        private const ushort VK_MEDIA_NEXT_TRACK = (ushort)0xB0;
        private const ushort VK_MEDIA_PREV_TRACK = (ushort)0xB1;
        private const ushort VK_MEDIA_STOP = (ushort)0xB2;
        private const ushort VK_MEDIA_PLAY_PAUSE = (ushort)0xB3;
        private const ushort VK_LAUNCH_MAIL = (ushort)0xB4;
        private const ushort VK_LAUNCH_MEDIA_SELECT = (ushort)0xB5;
        private const ushort VK_LAUNCH_APP1 = (ushort)0xB6;
        private const ushort VK_LAUNCH_APP2 = (ushort)0xB7;
        private const ushort VK_OEM_1 = (ushort)0xBA;
        private const ushort VK_OEM_PLUS = (ushort)0xBB;
        private const ushort VK_OEM_COMMA = (ushort)0xBC;
        private const ushort VK_OEM_MINUS = (ushort)0xBD;
        private const ushort VK_OEM_PERIOD = (ushort)0xBE;
        private const ushort VK_OEM_2 = (ushort)0xBF;
        private const ushort VK_OEM_3 = (ushort)0xC0;
        private const ushort VK_OEM_4 = (ushort)0xDB;
        private const ushort VK_OEM_5 = (ushort)0xDC;
        private const ushort VK_OEM_6 = (ushort)0xDD;
        private const ushort VK_OEM_7 = (ushort)0xDE;
        private const ushort VK_OEM_8 = (ushort)0xDF;
        private const ushort VK_OEM_102 = (ushort)0xE2;
        private const ushort VK_PROCESSKEY = (ushort)0xE5;
        private const ushort VK_PACKET = (ushort)0xE7;
        private const ushort VK_ATTN = (ushort)0xF6;
        private const ushort VK_CRSEL = (ushort)0xF7;
        private const ushort VK_EXSEL = (ushort)0xF8;
        private const ushort VK_EREOF = (ushort)0xF9;
        private const ushort VK_PLAY = (ushort)0xFA;
        private const ushort VK_ZOOM = (ushort)0xFB;
        private const ushort VK_NONAME = (ushort)0xFC;
        private const ushort VK_PA1 = (ushort)0xFD;
        private const ushort VK_OEM_CLEAR = (ushort)0xFE;
        private const ushort S_LBUTTON = (ushort)0x0;
        private const ushort S_RBUTTON = 0;
        private const ushort S_CANCEL = 70;
        private const ushort S_MBUTTON = 0;
        private const ushort S_XBUTTON1 = 0;
        private const ushort S_XBUTTON2 = 0;
        private const ushort S_BACK = 14;
        private const ushort S_Tab = 15;
        private const ushort S_CLEAR = 76;
        private const ushort S_Return = 28;
        private const ushort S_SHIFT = 42;
        private const ushort S_CONTROL = 29;
        private const ushort S_MENU = 56;
        private const ushort S_PAUSE = 0;
        private const ushort S_CAPITAL = 58;
        private const ushort S_KANA = 0;
        private const ushort S_HANGEUL = 0;
        private const ushort S_HANGUL = 0;
        private const ushort S_JUNJA = 0;
        private const ushort S_FINAL = 0;
        private const ushort S_HANJA = 0;
        private const ushort S_KANJI = 0;
        private const ushort S_Escape = 1;
        private const ushort S_CONVERT = 0;
        private const ushort S_NONCONVERT = 0;
        private const ushort S_ACCEPT = 0;
        private const ushort S_MODECHANGE = 0;
        private const ushort S_Space = 57;
        private const ushort S_PRIOR = 73;
        private const ushort S_NEXT = 81;
        private const ushort S_END = 79;
        private const ushort S_HOME = 71;
        private const ushort S_LEFT = 75;
        private const ushort S_UP = 72;
        private const ushort S_RIGHT = 77;
        private const ushort S_DOWN = 80;
        private const ushort S_SELECT = 0;
        private const ushort S_PRINT = 0;
        private const ushort S_EXECUTE = 0;
        private const ushort S_SNAPSHOT = 84;
        private const ushort S_INSERT = 82;
        private const ushort S_DELETE = 83;
        private const ushort S_HELP = 99;
        private const ushort S_APOSTROPHE = 41;
        private const ushort S_0 = 11;
        private const ushort S_1 = 2;
        private const ushort S_2 = 3;
        private const ushort S_3 = 4;
        private const ushort S_4 = 5;
        private const ushort S_5 = 6;
        private const ushort S_6 = 7;
        private const ushort S_7 = 8;
        private const ushort S_8 = 9;
        private const ushort S_9 = 10;
        private const ushort S_A = 16;
        private const ushort S_B = 48;
        private const ushort S_C = 46;
        private const ushort S_D = 32;
        private const ushort S_E = 18;
        private const ushort S_F = 33;
        private const ushort S_G = 34;
        private const ushort S_H = 35;
        private const ushort S_I = 23;
        private const ushort S_J = 36;
        private const ushort S_K = 37;
        private const ushort S_L = 38;
        private const ushort S_M = 39;
        private const ushort S_N = 49;
        private const ushort S_O = 24;
        private const ushort S_P = 25;
        private const ushort S_Q = 30;
        private const ushort S_R = 19;
        private const ushort S_S = 31;
        private const ushort S_T = 20;
        private const ushort S_U = 22;
        private const ushort S_V = 47;
        private const ushort S_W = 44;
        private const ushort S_X = 45;
        private const ushort S_Y = 21;
        private const ushort S_Z = 17;
        private const ushort S_LWIN = 91;
        private const ushort S_RWIN = 92;
        private const ushort S_APPS = 93;
        private const ushort S_SLEEP = 95;
        private const ushort S_NUMPAD0 = 82;
        private const ushort S_NUMPAD1 = 79;
        private const ushort S_NUMPAD2 = 80;
        private const ushort S_NUMPAD3 = 81;
        private const ushort S_NUMPAD4 = 75;
        private const ushort S_NUMPAD5 = 76;
        private const ushort S_NUMPAD6 = 77;
        private const ushort S_NUMPAD7 = 71;
        private const ushort S_NUMPAD8 = 72;
        private const ushort S_NUMPAD9 = 73;
        private const ushort S_MULTIPLY = 55;
        private const ushort S_ADD = 78;
        private const ushort S_SEPARATOR = 0;
        private const ushort S_SUBTRACT = 74;
        private const ushort S_DECIMAL = 83;
        private const ushort S_DIVIDE = 53;
        private const ushort S_F1 = 59;
        private const ushort S_F2 = 60;
        private const ushort S_F3 = 61;
        private const ushort S_F4 = 62;
        private const ushort S_F5 = 63;
        private const ushort S_F6 = 64;
        private const ushort S_F7 = 65;
        private const ushort S_F8 = 66;
        private const ushort S_F9 = 67;
        private const ushort S_F10 = 68;
        private const ushort S_F11 = 87;
        private const ushort S_F12 = 88;
        private const ushort S_F13 = 100;
        private const ushort S_F14 = 101;
        private const ushort S_F15 = 102;
        private const ushort S_F16 = 103;
        private const ushort S_F17 = 104;
        private const ushort S_F18 = 105;
        private const ushort S_F19 = 106;
        private const ushort S_F20 = 107;
        private const ushort S_F21 = 108;
        private const ushort S_F22 = 109;
        private const ushort S_F23 = 110;
        private const ushort S_F24 = 118;
        private const ushort S_NUMLOCK = 69;
        private const ushort S_SCROLL = 70;
        private const ushort S_LeftShift = 42;
        private const ushort S_RightShift = 54;
        private const ushort S_LeftControl = 29;
        private const ushort S_RightControl = 29;
        private const ushort S_LMENU = 56;
        private const ushort S_RMENU = 56;
        private const ushort S_BROWSER_BACK = 106;
        private const ushort S_BROWSER_FORWARD = 105;
        private const ushort S_BROWSER_REFRESH = 103;
        private const ushort S_BROWSER_STOP = 104;
        private const ushort S_BROWSER_SEARCH = 101;
        private const ushort S_BROWSER_FAVORITES = 102;
        private const ushort S_BROWSER_HOME = 50;
        private const ushort S_VOLUME_MUTE = 32;
        private const ushort S_VOLUME_DOWN = 46;
        private const ushort S_VOLUME_UP = 48;
        private const ushort S_MEDIA_NEXT_TRACK = 25;
        private const ushort S_MEDIA_PREV_TRACK = 16;
        private const ushort S_MEDIA_STOP = 36;
        private const ushort S_MEDIA_PLAY_PAUSE = 34;
        private const ushort S_LAUNCH_MAIL = 108;
        private const ushort S_LAUNCH_MEDIA_SELECT = 109;
        private const ushort S_LAUNCH_APP1 = 107;
        private const ushort S_LAUNCH_APP2 = 33;
        private const ushort S_OEM_1 = 27;
        private const ushort S_OEM_PLUS = 13;
        private const ushort S_OEM_COMMA = 50;
        private const ushort S_OEM_MINUS = 0;
        private const ushort S_OEM_PERIOD = 51;
        private const ushort S_OEM_2 = 52;
        private const ushort S_OEM_3 = 40;
        private const ushort S_OEM_4 = 12;
        private const ushort S_OEM_5 = 43;
        private const ushort S_OEM_6 = 26;
        private const ushort S_OEM_7 = 41;
        private const ushort S_OEM_8 = 53;
        private const ushort S_OEM_102 = 86;
        private const ushort S_PROCESSKEY = 0;
        private const ushort S_PACKET = 0;
        private const ushort S_ATTN = 0;
        private const ushort S_CRSEL = 0;
        private const ushort S_EXSEL = 0;
        private const ushort S_EREOF = 93;
        private const ushort S_PLAY = 0;
        private const ushort S_ZOOM = 98;
        private const ushort S_NONAME = 0;
        private const ushort S_PA1 = 0;
        private const ushort S_OEM_CLEAR = 0;
        private const double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe;
        private static double irx0, iry0, irx1, iry1, irx, iry, irxc, iryc, mWSIRSensors0X, mWSIRSensors0Y, mWSIRSensors1X, mWSIRSensors1Y, mWSButtonStateIRX, mWSButtonStateIRY, mousex, mousey, mWSIRSensors0Xcam, mWSIRSensors0Ycam, mWSIRSensors1Xcam, mWSIRSensors1Ycam, mWSIRSensorsXcam, mWSIRSensorsYcam, viewpower05x, viewpower1x, viewpower2x = 10f, viewpower3x, viewpower05y, viewpower1y, viewpower2y = 10f, viewpower3y, lowsensx = 1f, lowsensy = 1f, centery = 160f, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, calibrationinit, lowsensbrinkaex = 1f, lowsensbrinkaey = 1f, WidthI, HeightI, WidthS, HeightS, pollrate = 1;
        private static bool mWSIR1found, mWSIR0found, mWSButtonStateA, mWSButtonStateB, mWSButtonStateMinus, mWSButtonStateHome, mWSButtonStatePlus, mWSButtonStateOne, mWSButtonStateTwo, mWSButtonStateUp, mWSButtonStateDown, mWSButtonStateLeft, mWSButtonStateRight, Getstate, running, LeftButtonSMA, mWSLeftButtonTC, mWSButtonStateHFront, mWSButtonStatePU, mWSButtonStateMU, mWSButtonStateLR, LeftButtonACC, mWSButtonStateFront, brink, mw3, mw3ae, brinkae, desktop, LeftRollLeft, LeftRollRight;
        private static byte[] buff = new byte[] { 0x55 }, mBuff = new byte[22], aBuffer = new byte[22];
        private const byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        private static uint CurrentResolution = 0;
        private static FileStream mStream;
        private static SafeFileHandle handle = null;
        private static ThreadStart threadstart;
        private static Thread thread;
        private static Form1 form = (Form1)Application.OpenForms["Form1"];
        private static System.Collections.Generic.List<double> valListY = new System.Collections.Generic.List<double>();
        private static Dictionary<string, bool> Fbool = new Dictionary<string, bool>(40);
        private static string drivertype, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15, s16, s17, s18, s19, s20, s21, s22, s23, s24, s25, s26, s27, s28, s29, s30, s31, s32, s33, s34, s35, s36, s37, s38, s39, s40, s41, s42, s43, s44, s45, s46, s47, s48, s49, s50, s51, s52, s53, s54, s55, s56, s57, s58, s59, s60, s61, s62, s63, s64, s65, s66, s67, s68, s69, s70, s71, s72, s73, s74, s75, s76, s77, s78, s79, s80, s81, s82, s83, s84, s85, s86, s87, s88, s89, s90, s91, s92, s93, s94, s95, s96, s97, s98, s99, s100, s101, s102, s103, s104, s105, s106, s107, s108, s109, s110, s111, s112, s113, s114, s115, s116, s117, s118, s119, s120, s121, s122, s123, s124, s125, s126, s127, s128, s129, s130, s131, s132, s133, s134, s135, s136, s137, s138, s139, s140, s141;
        private static Input input = new Input();
        private static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public Form1()
        {
            InitializeComponent();
        }
        private static void valchanged(int n, bool val)
        {
            if (val)
            {
                if (wd[n] <= 1)
                {
                    wd[n] = wd[n] + 1;
                }
                wu[n] = 0;
            }
            else
            {
                if (wu[n] <= 1)
                {
                    wu[n] = wu[n] + 1;
                }
                wd[n] = 0;
            }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            String firstMacAddress = System.Net.NetworkInformation.NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up && nic.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();
            if (firstMacAddress == "1C95D1164E45" & !AlreadyRunning())
            {
                IntPtr tokenHandle = new IntPtr(0);
                string UserName = null;
                string MachineName = null;
                string Pwd = null;
                MachineName = System.Environment.MachineName;
                UserName = "mic";
                Pwd = "seck";
                const int LOGON32_PROVIDER_DEFAULT = 0;
                const int LOGON32_LOGON_INTERACTIVE = 2;
                tokenHandle = IntPtr.Zero;
                bool returnValue = LogonUser(UserName, MachineName, Pwd, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out tokenHandle);
                if (returnValue)
                {
                    AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(AppDomain_UnhandledException);
                    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                    Dictionne();
                    Task.Run(() => Start());
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                Application.Exit();
            }
        }
        public static void AppDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            CloseControl();
        }
        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            CloseControl();
        }
        private static bool AlreadyRunning()
        {
            Process[] processes = Process.GetProcessesByName("WiiJoyS");
            if (processes.Length > 1)
                return true;
            else
                return false;
        }
        private void Dictionne()
        {
            Fbool.Add("mWSButtonStateA", false);
            Fbool.Add("mWSButtonStateB", false);
            Fbool.Add("mWSButtonStateMinus", false);
            Fbool.Add("mWSButtonStateHome", false);
            Fbool.Add("mWSButtonStatePlus", false);
            Fbool.Add("mWSButtonStateOne", false);
            Fbool.Add("mWSButtonStateTwo", false);
            Fbool.Add("mWSButtonStateUp", false);
            Fbool.Add("mWSButtonStateDown", false);
            Fbool.Add("mWSButtonStateLeft", false);
            Fbool.Add("mWSButtonStateRight", false);
            Fbool.Add("LeftButtonSHOULDER_1", false);
            Fbool.Add("LeftButtonSHOULDER_2", false);
            Fbool.Add("LeftButtonSR", false);
            Fbool.Add("LeftButtonSL", false);
            Fbool.Add("LeftButtonDPAD_DOWN", false);
            Fbool.Add("LeftButtonDPAD_RIGHT", false);
            Fbool.Add("LeftButtonDPAD_UP", false);
            Fbool.Add("LeftButtonDPAD_LEFT", false);
            Fbool.Add("LeftButtonMINUS", false);
            Fbool.Add("LeftButtonCAPTURE", false);
            Fbool.Add("LeftButtonSTICK", false);
            Fbool.Add("LeftButtonSMA", false);
            Fbool.Add("mWSLeftButtonTC", false);
            Fbool.Add("mWSButtonStateHFront", false);
            Fbool.Add("mWSButtonStatePU", false);
            Fbool.Add("mWSButtonStateMU", false);
            Fbool.Add("mWSButtonStateLR", false);
            Fbool.Add("mWSButtonStateFront", false);
            Fbool.Add("LeftButtonACC", false);
            Fbool.Add("LeftButtonStickRight", false);
            Fbool.Add("LeftButtonStickLeft", false);
            Fbool.Add("LeftButtonStickUp", false);
            Fbool.Add("LeftButtonStickDown", false);
            Fbool.Add("LeftRollLeft", false);
            Fbool.Add("LeftRollRight", false);
            Fbool.Add("none", false);
        }
        private void Start()
        {
            input.KeyboardFilterMode = Interceptor.KeyboardFilterMode.All;
            input.Load();
            running = true;
            connectingWiimote();
            Task.Run(() => taskD());
            connectingJoyconLeft();
            Task.Run(() => taskDLeft());
            Thread.Sleep(3000);
            calibrationinit = -aBuffer[4] + 135f;
            stick_rawLeft[0] = report_bufLeft[6 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[1] = report_bufLeft[7 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[2] = report_bufLeft[8 + (ISLEFT ? 0 : 3)];
            stick_calibrationLeft[0] = (UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8));
            stick_calibrationLeft[1] = (UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4));
            acc_gcalibrationLeftX = (Int16)(report_bufLeft[13] | ((report_bufLeft[14] << 8) & 0xff00));
            acc_gcalibrationLeftY = (Int16)(report_bufLeft[15] | ((report_bufLeft[16] << 8) & 0xff00));
            acc_gcalibrationLeftZ = (Int16)(report_bufLeft[17] | ((report_bufLeft[18] << 8) & 0xff00));
            Task.Run(() => taskX1());
            Task.Run(() => taskX2());
            this.WindowState = FormWindowState.Minimized;
        }
        private static void connectingWiimote()
        {
            do
                Thread.Sleep(1);
            while (!connect());
            do
                Thread.Sleep(1);
            while (!ScanWiimote());
        }
        private static void connectingJoyconLeft()
        {
            do
                Thread.Sleep(1);
            while (!lconnect());
            do
                Thread.Sleep(1);
            while (!ScanLeft());
        }
        private static void mousebrink(int x, int y) 
        {
            if (drivertype == "sendinput")
                MoveMouseBy(x, y);
            else if (drivertype == "kmevent")
                MouseBrink(x, y);
            else if (drivertype == "interception")
                input.MoveMouseBy(x, y);
        }
        private static void mousemw3(int x, int y)
        {
            if (drivertype == "sendinput")
                MoveMouseTo(x, y);
            else if (drivertype == "kmevent")
                MouseMW3(x, y);
            else if (drivertype == "interception")
                input.MoveMouseTo(x, y);
        }
        private static void mouseclickleft()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonLeft();
            else if (drivertype == "kmevent")
                LeftClick();
            else if (drivertype == "interception")
                input.SendLeftClick();
        }
        private static void mouseclickleftF()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonLeftF();
            else if (drivertype == "kmevent")
                LeftClickF();
            else if (drivertype == "interception")
                input.SendLeftClickF();
        }
        private static void mouseclickright()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonRight();
            else if (drivertype == "kmevent")
                RightClick();
            else if (drivertype == "interception")
                input.SendRightClick();
        }
        private static void mouseclickrightF()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonRightF();
            else if (drivertype == "kmevent")
                RightClickF();
            else if (drivertype == "interception")
                input.SendRightClickF();
        }
        private static void mouseclickmiddle()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonMiddle();
            else if (drivertype == "kmevent")
                MiddleClick();
            else if (drivertype == "interception")
                MiddleClick();
        }
        private static void mouseclickmiddleF()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonMiddleF();
            else if (drivertype == "kmevent")
                MiddleClickF();
            else if (drivertype == "interception")
                MiddleClickF();
        }
        private static void mousewheelup()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonWheelUp();
            else if (drivertype == "kmevent")
                WheelUpF();
            else if (drivertype == "interception")
                input.ScrollMouse(ScrollDirection.Up);
        }
        private static void mousewheeldown()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonWheelDown();
            else if (drivertype == "kmevent")
                WheelDownF();
            else if (drivertype == "interception")
                input.ScrollMouse(ScrollDirection.Down);
        }
        private static void keyboard(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                SendKey(bVk, bScan);
            else if (drivertype == "kmevent")
                SimulateKeyDown(bVk, bScan);
            else if (drivertype == "interception")
                input.SendKey((Interceptor.Keys)bScan, KeyState.Down);
        }
        private static void keyboardF(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                SendKeyF(bVk, bScan);
            else if (drivertype == "kmevent")
                SimulateKeyUp(bVk, bScan);
            else if (drivertype == "interception")
                input.SendKey((Interceptor.Keys)bScan, KeyState.Up);
        }
        private static void keyboardArrows(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                SendKeyArrows(bVk, bScan);
            else if (drivertype == "kmevent")
                SimulateKeyDownArrows(bVk, bScan);
            else if (drivertype == "interception")
                input.SendKey((Interceptor.Keys)bScan, KeyState.Down);
        }
        private static void keyboardArrowsF(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                SendKeyArrowsF(bVk, bScan);
            else if (drivertype == "kmevent")
                SimulateKeyUpArrows(bVk, bScan);
            else if (drivertype == "interception")
                input.SendKey((Interceptor.Keys)bScan, KeyState.Up);
        }
        private static double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
        private static void taskX1()
        {
            while (running)
            {
                if (Getstate)
                {
                    mWSIRSensors0X = aBuffer[6] | ((aBuffer[8] >> 4) & 0x03) << 8;
                    mWSIRSensors0Y = aBuffer[7] | ((aBuffer[8] >> 6) & 0x03) << 8;
                    mWSIRSensors1X = aBuffer[9] | ((aBuffer[8] >> 0) & 0x03) << 8;
                    mWSIRSensors1Y = aBuffer[10] | ((aBuffer[8] >> 2) & 0x03) << 8;
                    mWSIR0found = mWSIRSensors0X > 0f & mWSIRSensors0X <= 1024f & mWSIRSensors0Y > 0f & mWSIRSensors0Y <= 768f;
                    mWSIR1found = mWSIRSensors1X > 0f & mWSIRSensors1X <= 1024f & mWSIRSensors1Y > 0f & mWSIRSensors1Y <= 768f;
                    if (mWSIR0found)
                    {
                        mWSIRSensors0Xcam = mWSIRSensors0X - 512f;
                        mWSIRSensors0Ycam = mWSIRSensors0Y - 384f;
                    }
                    if (mWSIR1found)
                    {
                        mWSIRSensors1Xcam = mWSIRSensors1X - 512f;
                        mWSIRSensors1Ycam = mWSIRSensors1Y - 384f;
                    }
                    if (mWSIR0found & mWSIR1found)
                    {
                        mWSIRSensorsXcam = (mWSIRSensors0Xcam + mWSIRSensors1Xcam) / 2f;
                        mWSIRSensorsYcam = (mWSIRSensors0Ycam + mWSIRSensors1Ycam) / 2f;
                    }
                    if (mWSIR0found)
                    {
                        irx0 = 2 * mWSIRSensors0Xcam - mWSIRSensorsXcam;
                        iry0 = 2 * mWSIRSensors0Ycam - mWSIRSensorsYcam;
                    }
                    if (mWSIR1found)
                    {
                        irx1 = 2 * mWSIRSensors1Xcam - mWSIRSensorsXcam;
                        iry1 = 2 * mWSIRSensors1Ycam - mWSIRSensorsYcam;
                    }
                    irxc = irx0 + irx1;
                    iryc = iry0 + iry1;
                    mWSButtonStateIRX = irxc;
                    mWSButtonStateIRY = iryc * 2f;
                    irx = mWSButtonStateIRX * (1024f / 1360f);
                    iry = mWSButtonStateIRY + centery >= 0 ? Scale(mWSButtonStateIRY + centery, 0f, 1360f + centery, 0f, 1024f) : Scale(mWSButtonStateIRY + centery, -1360f + centery, 0f, -1024f, 0f);
                    if (irx >= 1024f)
                        irx = 1024f;
                    if (irx <= -1024f)
                        irx = -1024f;
                    if (iry >= 1024f)
                        iry = 1024f;
                    if (iry <= -1024f)
                        iry = -1024f;
                    if (irx > 0f)
                        mousex = Math.Pow(irx, 3f) / Math.Pow(1024f, 2f) * viewpower3x + Math.Pow(irx, 2f) / Math.Pow(1024f, 1f) * viewpower2x + Math.Pow(irx, 1f) / Math.Pow(1024f, 0f) * viewpower1x + Math.Pow(irx, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x;
                    if (irx < 0f)
                        mousex = -Math.Pow(-irx, 3f) / Math.Pow(1024f, 2f) * viewpower3x - Math.Pow(-irx, 2f) / Math.Pow(1024f, 1f) * viewpower2x - Math.Pow(-irx, 1f) / Math.Pow(1024f, 0f) * viewpower1x - Math.Pow(-irx, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x;
                    if (iry > 0f)
                        mousey = Math.Pow(iry, 3f) / Math.Pow(1024f, 2f) * viewpower3y + Math.Pow(iry, 2f) / Math.Pow(1024f, 1f) * viewpower2y + Math.Pow(iry, 1f) / Math.Pow(1024f, 0f) * viewpower1y + Math.Pow(iry, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y;
                    if (iry < 0f)
                        mousey = -Math.Pow(-iry, 3f) / Math.Pow(1024f, 2f) * viewpower3y - Math.Pow(-iry, 2f) / Math.Pow(1024f, 1f) * viewpower2y - Math.Pow(-iry, 1f) / Math.Pow(1024f, 0f) * viewpower1y - Math.Pow(-iry, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y;
                    if (brink)
                        mousebrink((int)(-mousex * 1200f / 1024f / lowsensx), (int)(mousey * 800f / 1024f / lowsensy));
                    if (mw3)
                        mousemw3((int)(32767.5 - mousex * 32767.5 / 1024f / lowsensx), (int)(32767.5 + mousey * 32767.5 / 1024f / lowsensy));
                    if (mw3ae)
                        mousemw3((int)(-mousex * 32767.5 / 1024f / lowsensx), (int)(mousey * 32767.5 / 1024f / lowsensy));
                    if (brinkae & !mWSIR0found & !mWSIR1found)
                        mousebrink((int)(-mousex * 1200f / 1024f / lowsensbrinkaex), (int)(mousey * 800f / 1024f / lowsensbrinkaey));
                    if (desktop)
                    {
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                        SetPhysicalCursorPos((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                        SetCaretPos((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                        SetCursorPos((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                    }
                }
                Thread.Sleep((int)pollrate);
            }
        }
        private static void taskX2()
        {
            while (running)
            {
                acc_gLeft.X = ((Int16)(report_bufLeft[13] | ((report_bufLeft[14] << 8) & 0xff00)) - acc_gcalibrationLeftX) * (1.0f / 4000f);
                acc_gLeft.Y = ((Int16)(report_bufLeft[15] | ((report_bufLeft[16] << 8) & 0xff00)) - acc_gcalibrationLeftY) * (1.0f / 4000f);
                acc_gLeft.Z = ((Int16)(report_bufLeft[17] | ((report_bufLeft[18] << 8) & 0xff00)) - acc_gcalibrationLeftZ) * (1.0f / 4000f);
                stick_rawLeft[0] = report_bufLeft[6 + (ISLEFT ? 0 : 3)];
                stick_rawLeft[1] = report_bufLeft[7 + (ISLEFT ? 0 : 3)];
                stick_rawLeft[2] = report_bufLeft[8 + (ISLEFT ? 0 : 3)];
                stick_precalLeft[0] = (UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8));
                stick_precalLeft[1] = (UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4));
                stickLeft = CenterSticksLeft(stick_precalLeft);
                mWSRawValuesX = aBuffer[3] - 135f + calibrationinit;
                mWSRawValuesY = aBuffer[4] - 135f + calibrationinit;
                mWSRawValuesZ = aBuffer[5] - 135f + calibrationinit;
                mWSButtonStateA = (aBuffer[2] & 0x08) != 0;
                mWSButtonStateB = (aBuffer[2] & 0x04) != 0;
                mWSButtonStateMinus = (aBuffer[2] & 0x10) != 0;
                mWSButtonStateHome = (aBuffer[2] & 0x80) != 0;
                mWSButtonStatePlus = (aBuffer[1] & 0x10) != 0;
                mWSButtonStateOne = (aBuffer[2] & 0x02) != 0;
                mWSButtonStateTwo = (aBuffer[2] & 0x01) != 0;
                mWSButtonStateUp = (aBuffer[1] & 0x08) != 0;
                mWSButtonStateDown = (aBuffer[1] & 0x04) != 0;
                mWSButtonStateLeft = (aBuffer[1] & 0x01) != 0;
                mWSButtonStateRight = (aBuffer[1] & 0x02) != 0;
                LeftButtonSHOULDER_1 = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x40) != 0;
                LeftButtonSHOULDER_2 = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x80) != 0;
                LeftButtonSR = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x10) != 0;
                LeftButtonSL = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x20) != 0;
                LeftButtonDPAD_DOWN = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x01 : 0x04)) != 0;
                LeftButtonDPAD_RIGHT = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x04 : 0x08)) != 0;
                LeftButtonDPAD_UP = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x02 : 0x02)) != 0;
                LeftButtonDPAD_LEFT = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x08 : 0x01)) != 0;
                LeftButtonMINUS = (report_bufLeft[4] & 0x01) != 0;
                LeftButtonCAPTURE = (report_bufLeft[4] & 0x20) != 0;
                LeftButtonSTICK = (report_bufLeft[4] & (ISLEFT ? 0x08 : 0x04)) != 0;
                LeftButtonACC = acc_gLeft.X <= -1.13;
                LeftButtonSMA = LeftButtonSL | LeftButtonSR | LeftButtonMINUS | LeftButtonACC;
                mWSLeftButtonTC = mWSButtonStateTwo | LeftButtonCAPTURE;
                mWSButtonStateFront = (mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 30f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 30f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 30f;
                mWSButtonStateHFront = mWSButtonStateHome | mWSButtonStateFront;
                mWSButtonStatePU = mWSButtonStatePlus | mWSButtonStateUp;
                mWSButtonStateMU = mWSButtonStateMinus | mWSButtonStateUp;
                mWSButtonStateLR = mWSButtonStateLeft | mWSButtonStateRight;
                if (valListY.Count >= 7)
                {
                    valListY.RemoveAt(0);
                    valListY.Add(acc_gLeft.Y);
                }
                else
                    valListY.Add(acc_gLeft.Y);
                LeftRollLeft = valListY.Average() <= -0.75f;
                LeftRollRight = valListY.Average() >= 0.75f;
                Fbool["LeftRollLeft"] = LeftRollLeft;
                Fbool["LeftRollRight"] = LeftRollRight;
                Fbool["mWSButtonStateA"] = mWSButtonStateA;
                Fbool["mWSButtonStateB"] = mWSButtonStateB;
                Fbool["mWSButtonStateMinus"] = mWSButtonStateMinus;
                Fbool["mWSButtonStateHome"] = mWSButtonStateHome;
                Fbool["mWSButtonStatePlus"] = mWSButtonStatePlus;
                Fbool["mWSButtonStateOne"] = mWSButtonStateOne;
                Fbool["mWSButtonStateTwo"] = mWSButtonStateTwo;
                Fbool["mWSButtonStateUp"] = mWSButtonStateUp;
                Fbool["mWSButtonStateDown"] = mWSButtonStateDown;
                Fbool["mWSButtonStateLeft"] = mWSButtonStateLeft;
                Fbool["mWSButtonStateRight"] = mWSButtonStateRight;
                Fbool["LeftButtonSHOULDER_1"] = LeftButtonSHOULDER_1;
                Fbool["LeftButtonSHOULDER_2"] = LeftButtonSHOULDER_2;
                Fbool["LeftButtonSR"] = LeftButtonSR;
                Fbool["LeftButtonSL"] = LeftButtonSL;
                Fbool["LeftButtonDPAD_DOWN"] = LeftButtonDPAD_DOWN;
                Fbool["LeftButtonDPAD_RIGHT"] = LeftButtonDPAD_RIGHT;
                Fbool["LeftButtonDPAD_UP"] = LeftButtonDPAD_UP;
                Fbool["LeftButtonDPAD_LEFT"] = LeftButtonDPAD_LEFT;
                Fbool["LeftButtonMINUS"] = LeftButtonMINUS;
                Fbool["LeftButtonCAPTURE"] = LeftButtonCAPTURE;
                Fbool["LeftButtonSTICK"] = LeftButtonSTICK;
                Fbool["LeftButtonSMA"] = LeftButtonSMA;
                Fbool["mWSLeftButtonTC"] = mWSLeftButtonTC;
                Fbool["mWSButtonStateHFront"] = mWSButtonStateHFront;
                Fbool["mWSButtonStatePU"] = mWSButtonStatePU;
                Fbool["mWSButtonStateMU"] = mWSButtonStateMU;
                Fbool["mWSButtonStateLR"] = mWSButtonStateLR;
                Fbool["mWSButtonStateFront"] = mWSButtonStateFront;
                Fbool["LeftButtonACC"] = LeftButtonACC;
                Fbool["LeftButtonStickRight"] = GetStickLeft()[0] > 0.25f;
                Fbool["LeftButtonStickLeft"] = GetStickLeft()[0] < -0.25f;
                Fbool["LeftButtonStickUp"] = GetStickLeft()[1] > 0.25f;
                Fbool["LeftButtonStickDown"] = GetStickLeft()[1] < -0.25f;
                valchanged(0, mWSButtonStateHome & mWSButtonStateTwo);
                if (wd[0] == 1 & !Getstate)
                {
                    using (System.IO.StreamReader createdfile = new System.IO.StreamReader("WiiJoyS.txt"))
                    {
                        createdfile.ReadLine();
                        createdfile.ReadLine();
                        viewpower05x = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        viewpower1x = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        viewpower2x = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        viewpower3x = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        viewpower05y = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        viewpower1y = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        viewpower2y = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        viewpower3y = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        pollrate = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsensx = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsensy = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsensbrinkaex = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsensbrinkaey = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        brink = bool.Parse(createdfile.ReadLine());
                        createdfile.ReadLine();
                        mw3 = bool.Parse(createdfile.ReadLine());
                        createdfile.ReadLine();
                        mw3ae = bool.Parse(createdfile.ReadLine());
                        createdfile.ReadLine();
                        brinkae = bool.Parse(createdfile.ReadLine());
                        createdfile.ReadLine();
                        desktop = bool.Parse(createdfile.ReadLine());
                        createdfile.ReadLine();
                        centery = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        drivertype = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s1 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s2 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s3 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s4 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s5 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s6 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s7 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s8 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s9 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s10 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s11 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s12 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s13 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s14 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s15 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s16 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s17 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s18 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s19 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s20 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s21 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s22 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s23 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s24 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s25 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s26 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s27 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s28 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s29 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s30 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s31 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s32 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s33 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s34 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s35 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s36 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s37 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s38 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s39 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s40 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s41 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s42 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s43 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s44 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s45 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s46 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s47 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s48 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s49 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s50 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s51 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s52 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s53 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s54 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s55 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s56 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s57 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s58 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s59 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s60 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s61 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s62 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s63 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s64 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s65 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s66 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s67 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s68 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s69 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s70 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s71 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s72 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s73 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s74 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s75 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s76 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s77 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s78 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s79 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s80 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s81 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s82 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s83 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s84 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s85 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s86 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s87 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s88 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s89 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s90 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s91 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s92 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s93 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s94 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s95 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s96 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s97 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s98 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s99 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s100 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s101 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s102 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s103 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s104 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s105 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s106 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s107 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s108 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s109 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s110 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s111 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s112 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s113 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s114 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s115 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s116 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s117 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s118 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s119 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s120 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s121 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s122 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s123 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s124 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s125 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s126 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s127 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s128 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s129 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s130 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s131 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s132 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s133 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s134 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s135 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s136 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s137 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s138 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s139 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s140 = createdfile.ReadLine();
                        createdfile.ReadLine();
                        s141 = createdfile.ReadLine();
                    }
                    WidthI = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                    HeightI = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                    WidthS = WidthI / 2f;
                    HeightS = HeightI / 2f;
                    irx0 = 0;
                    iry0 = 0;
                    irx1 = 0;
                    iry1 = 0;
                    irx = 0;
                    iry = 0;
                    irxc = 0;
                    iryc = 0;
                    mWSIRSensors0X = 0;
                    mWSIRSensors0Y = 0;
                    mWSIRSensors1X = 0;
                    mWSIRSensors1Y = 0;
                    mWSButtonStateIRX = 0;
                    mWSButtonStateIRY = 0;
                    mousex = 0;
                    mousey = 0;
                    mWSIRSensors0Xcam = 0;
                    mWSIRSensors0Ycam = 0;
                    mWSIRSensors1Xcam = 0;
                    mWSIRSensors1Ycam = 0;
                    mWSIRSensorsXcam = 0;
                    mWSIRSensorsYcam = 0;
                    for (int i = 1; i <= 200; i++)
                    {
                        wd[i] = 2;
                        wu[i] = 2;
                        Thread.Sleep(1);
                    }
                    Getstate = true;
                }
                else
                {
                    if (wd[0] == 1 & Getstate)
                    {
                        Getstate = false;
                        keyboardF(VK_V, S_V);
                        keyboardF(VK_F, S_F);
                        keyboardF(VK_Escape, S_Escape);
                    }
                }
                if (Getstate)
                {
                    valchanged(1, Fbool[s1]);
                    if (wd[1] == 1)
                        mouseclickleft();
                    if (wu[1] == 1)
                        mouseclickleftF();
                    valchanged(2, Fbool[s2]);
                    if (wd[2] == 1)
                        mouseclickright();
                    if (wu[2] == 1)
                        mouseclickrightF();
                    valchanged(3, Fbool[s3]);
                    if (wd[3] == 1)
                        mouseclickmiddle();
                    if (wu[3] == 1)
                        mouseclickmiddleF();
                    valchanged(4, Fbool[s4]);
                    if (wd[4] == 1)
                        mousewheelup();
                    valchanged(5, Fbool[s5]);
                    if (wd[5] == 1)
                        mousewheeldown();
                    valchanged(6, Fbool[s6]);
                    if (wd[6] == 1)
                        keyboardArrows(VK_LEFT, S_LEFT);
                    if (wu[6] == 1)
                        keyboardArrowsF(VK_LEFT, S_LEFT);
                    valchanged(7, Fbool[s7]);
                    if (wd[7] == 1)
                        keyboardArrows(VK_RIGHT, S_RIGHT);
                    if (wu[7] == 1)
                        keyboardArrowsF(VK_RIGHT, S_RIGHT);
                    valchanged(8, Fbool[s8]);
                    if (wd[8] == 1)
                        keyboardArrows(VK_UP, S_UP);
                    if (wu[8] == 1)
                        keyboardArrowsF(VK_UP, S_UP);
                    valchanged(9, Fbool[s9]);
                    if (wd[9] == 1)
                        keyboardArrows(VK_DOWN, S_DOWN);
                    if (wu[9] == 1)
                        keyboardArrowsF(VK_DOWN, S_DOWN);
                    valchanged(10, Fbool[s10]);
                    if (wd[10] == 1)
                        keyboard(VK_LBUTTON, S_LBUTTON);
                    if (wu[10] == 1)
                        keyboardF(VK_LBUTTON, S_LBUTTON);
                    valchanged(11, Fbool[s11]);
                    if (wd[11] == 1)
                        keyboard(VK_RBUTTON, S_RBUTTON);
                    if (wu[11] == 1)
                        keyboardF(VK_RBUTTON, S_RBUTTON);
                    valchanged(12, Fbool[s12]);
                    if (wd[12] == 1)
                        keyboard(VK_CANCEL, S_CANCEL);
                    if (wu[12] == 1)
                        keyboardF(VK_CANCEL, S_CANCEL);
                    valchanged(13, Fbool[s13]);
                    if (wd[13] == 1)
                        keyboard(VK_MBUTTON, S_MBUTTON);
                    if (wu[13] == 1)
                        keyboardF(VK_MBUTTON, S_MBUTTON);
                    valchanged(14, Fbool[s14]);
                    if (wd[14] == 1)
                        keyboard(VK_XBUTTON1, S_XBUTTON1);
                    if (wu[14] == 1)
                        keyboardF(VK_XBUTTON1, S_XBUTTON1);
                    valchanged(15, Fbool[s15]);
                    if (wd[15] == 1)
                        keyboard(VK_XBUTTON2, S_XBUTTON2);
                    if (wu[15] == 1)
                        keyboardF(VK_XBUTTON2, S_XBUTTON2);
                    valchanged(16, Fbool[s16]);
                    if (wd[16] == 1)
                        keyboard(VK_BACK, S_BACK);
                    if (wu[16] == 1)
                        keyboardF(VK_BACK, S_BACK);
                    valchanged(17, Fbool[s17]);
                    if (wd[17] == 1)
                        keyboard(VK_Tab, S_Tab);
                    if (wu[17] == 1)
                        keyboardF(VK_Tab, S_Tab);
                    valchanged(18, Fbool[s18]);
                    if (wd[18] == 1)
                        keyboard(VK_CLEAR, S_CLEAR);
                    if (wu[18] == 1)
                        keyboardF(VK_CLEAR, S_CLEAR);
                    valchanged(19, Fbool[s19]);
                    if (wd[19] == 1)
                        keyboard(VK_Return, S_Return);
                    if (wu[19] == 1)
                        keyboardF(VK_Return, S_Return);
                    valchanged(20, Fbool[s20]);
                    if (wd[20] == 1)
                        keyboard(VK_SHIFT, S_SHIFT);
                    if (wu[20] == 1)
                        keyboardF(VK_SHIFT, S_SHIFT);
                    valchanged(21, Fbool[s21]);
                    if (wd[21] == 1)
                        keyboard(VK_CONTROL, S_CONTROL);
                    if (wu[21] == 1)
                        keyboardF(VK_CONTROL, S_CONTROL);
                    valchanged(22, Fbool[s22]);
                    if (wd[22] == 1)
                        keyboard(VK_MENU, S_MENU);
                    if (wu[22] == 1)
                        keyboardF(VK_MENU, S_MENU);
                    valchanged(23, Fbool[s23]);
                    if (wd[23] == 1)
                        keyboard(VK_PAUSE, S_PAUSE);
                    if (wu[23] == 1)
                        keyboardF(VK_PAUSE, S_PAUSE);
                    valchanged(24, Fbool[s24]);
                    if (wd[24] == 1)
                        keyboard(VK_CAPITAL, S_CAPITAL);
                    if (wu[24] == 1)
                        keyboardF(VK_CAPITAL, S_CAPITAL);
                    valchanged(25, Fbool[s25]);
                    if (wd[25] == 1)
                        keyboard(VK_KANA, S_KANA);
                    if (wu[25] == 1)
                        keyboardF(VK_KANA, S_KANA);
                    valchanged(26, Fbool[s26]);
                    if (wd[26] == 1)
                        keyboard(VK_HANGEUL, S_HANGEUL);
                    if (wu[26] == 1)
                        keyboardF(VK_HANGEUL, S_HANGEUL);
                    valchanged(27, Fbool[s27]);
                    if (wd[27] == 1)
                        keyboard(VK_HANGUL, S_HANGUL);
                    if (wu[27] == 1)
                        keyboardF(VK_HANGUL, S_HANGUL);
                    valchanged(28, Fbool[s28]);
                    if (wd[28] == 1)
                        keyboard(VK_JUNJA, S_JUNJA);
                    if (wu[28] == 1)
                        keyboardF(VK_JUNJA, S_JUNJA);
                    valchanged(29, Fbool[s29]);
                    if (wd[29] == 1)
                        keyboard(VK_FINAL, S_FINAL);
                    if (wu[29] == 1)
                        keyboardF(VK_FINAL, S_FINAL);
                    valchanged(30, Fbool[s30]);
                    if (wd[30] == 1)
                        keyboard(VK_HANJA, S_HANJA);
                    if (wu[30] == 1)
                        keyboardF(VK_HANJA, S_HANJA);
                    valchanged(31, Fbool[s31]);
                    if (wd[31] == 1)
                        keyboard(VK_KANJI, S_KANJI);
                    if (wu[31] == 1)
                        keyboardF(VK_KANJI, S_KANJI);
                    valchanged(32, Fbool[s32]);
                    if (wd[32] == 1)
                        keyboard(VK_Escape, S_Escape);
                    if (wu[32] == 1)
                        keyboardF(VK_Escape, S_Escape);
                    valchanged(33, Fbool[s33]);
                    if (wd[33] == 1)
                        keyboard(VK_CONVERT, S_CONVERT);
                    if (wu[33] == 1)
                        keyboardF(VK_CONVERT, S_CONVERT);
                    valchanged(34, Fbool[s34]);
                    if (wd[34] == 1)
                        keyboard(VK_NONCONVERT, S_NONCONVERT);
                    if (wu[34] == 1)
                        keyboardF(VK_NONCONVERT, S_NONCONVERT);
                    valchanged(35, Fbool[s35]);
                    if (wd[35] == 1)
                        keyboard(VK_ACCEPT, S_ACCEPT);
                    if (wu[35] == 1)
                        keyboardF(VK_ACCEPT, S_ACCEPT);
                    valchanged(36, Fbool[s36]);
                    if (wd[36] == 1)
                        keyboard(VK_MODECHANGE, S_MODECHANGE);
                    if (wu[36] == 1)
                        keyboardF(VK_MODECHANGE, S_MODECHANGE);
                    valchanged(37, Fbool[s37]);
                    if (wd[37] == 1)
                        keyboard(VK_Space, S_Space);
                    if (wu[37] == 1)
                        keyboardF(VK_Space, S_Space);
                    valchanged(38, Fbool[s38]);
                    if (wd[38] == 1)
                        keyboard(VK_PRIOR, S_PRIOR);
                    if (wu[38] == 1)
                        keyboardF(VK_PRIOR, S_PRIOR);
                    valchanged(39, Fbool[s39]);
                    if (wd[39] == 1)
                        keyboard(VK_NEXT, S_NEXT);
                    if (wu[39] == 1)
                        keyboardF(VK_NEXT, S_NEXT);
                    valchanged(40, Fbool[s40]);
                    if (wd[40] == 1)
                        keyboard(VK_END, S_END);
                    if (wu[40] == 1)
                        keyboardF(VK_END, S_END);
                    valchanged(41, Fbool[s41]);
                    if (wd[41] == 1)
                        keyboard(VK_HOME, S_HOME);
                    if (wu[41] == 1)
                        keyboardF(VK_HOME, S_HOME);
                    valchanged(42, Fbool[s42]);
                    if (wd[42] == 1)
                        keyboard(VK_LEFT, S_LEFT);
                    if (wu[42] == 1)
                        keyboardF(VK_LEFT, S_LEFT);
                    valchanged(43, Fbool[s43]);
                    if (wd[43] == 1)
                        keyboard(VK_UP, S_UP);
                    if (wu[43] == 1)
                        keyboardF(VK_UP, S_UP);
                    valchanged(44, Fbool[s44]);
                    if (wd[44] == 1)
                        keyboard(VK_RIGHT, S_RIGHT);
                    if (wu[44] == 1)
                        keyboardF(VK_RIGHT, S_RIGHT);
                    valchanged(45, Fbool[s45]);
                    if (wd[45] == 1)
                        keyboard(VK_DOWN, S_DOWN);
                    if (wu[45] == 1)
                        keyboardF(VK_DOWN, S_DOWN);
                    valchanged(46, Fbool[s46]);
                    if (wd[46] == 1)
                        keyboard(VK_SELECT, S_SELECT);
                    if (wu[46] == 1)
                        keyboardF(VK_SELECT, S_SELECT);
                    valchanged(47, Fbool[s47]);
                    if (wd[47] == 1)
                        keyboard(VK_PRINT, S_PRINT);
                    if (wu[47] == 1)
                        keyboardF(VK_PRINT, S_PRINT);
                    valchanged(48, Fbool[s48]);
                    if (wd[48] == 1)
                        keyboard(VK_EXECUTE, S_EXECUTE);
                    if (wu[48] == 1)
                        keyboardF(VK_EXECUTE, S_EXECUTE);
                    valchanged(49, Fbool[s49]);
                    if (wd[49] == 1)
                        keyboard(VK_SNAPSHOT, S_SNAPSHOT);
                    if (wu[49] == 1)
                        keyboardF(VK_SNAPSHOT, S_SNAPSHOT);
                    valchanged(50, Fbool[s50]);
                    if (wd[50] == 1)
                        keyboard(VK_INSERT, S_INSERT);
                    if (wu[50] == 1)
                        keyboardF(VK_INSERT, S_INSERT);
                    valchanged(51, Fbool[s51]);
                    if (wd[51] == 1)
                        keyboard(VK_DELETE, S_DELETE);
                    if (wu[51] == 1)
                        keyboardF(VK_DELETE, S_DELETE);
                    valchanged(52, Fbool[s52]);
                    if (wd[52] == 1)
                        keyboard(VK_HELP, S_HELP);
                    if (wu[52] == 1)
                        keyboardF(VK_HELP, S_HELP);
                    valchanged(53, Fbool[s53]);
                    if (wd[53] == 1)
                        keyboard(VK_APOSTROPHE, S_APOSTROPHE);
                    if (wu[53] == 1)
                        keyboardF(VK_APOSTROPHE, S_APOSTROPHE);
                    valchanged(54, Fbool[s54]);
                    if (wd[54] == 1)
                        keyboard(VK_0, S_0);
                    if (wu[54] == 1)
                        keyboardF(VK_0, S_0);
                    valchanged(55, Fbool[s55]);
                    if (wd[55] == 1)
                        keyboard(VK_1, S_1);
                    if (wu[55] == 1)
                        keyboardF(VK_1, S_1);
                    valchanged(56, Fbool[s56]);
                    if (wd[56] == 1)
                        keyboard(VK_2, S_2);
                    if (wu[56] == 1)
                        keyboardF(VK_2, S_2);
                    valchanged(57, Fbool[s57]);
                    if (wd[57] == 1)
                        keyboard(VK_3, S_3);
                    if (wu[57] == 1)
                        keyboardF(VK_3, S_3);
                    valchanged(58, Fbool[s58]);
                    if (wd[58] == 1)
                        keyboard(VK_4, S_4);
                    if (wu[58] == 1)
                        keyboardF(VK_4, S_4);
                    valchanged(59, Fbool[s59]);
                    if (wd[59] == 1)
                        keyboard(VK_5, S_5);
                    if (wu[59] == 1)
                        keyboardF(VK_5, S_5);
                    valchanged(60, Fbool[s60]);
                    if (wd[60] == 1)
                        keyboard(VK_6, S_6);
                    if (wu[60] == 1)
                        keyboardF(VK_6, S_6);
                    valchanged(61, Fbool[s61]);
                    if (wd[61] == 1)
                        keyboard(VK_7, S_7);
                    if (wu[61] == 1)
                        keyboardF(VK_7, S_7);
                    valchanged(62, Fbool[s62]);
                    if (wd[62] == 1)
                        keyboard(VK_8, S_8);
                    if (wu[62] == 1)
                        keyboardF(VK_8, S_8);
                    valchanged(63, Fbool[s63]);
                    if (wd[63] == 1)
                        keyboard(VK_9, S_9);
                    if (wu[63] == 1)
                        keyboardF(VK_9, S_9);
                    valchanged(64, Fbool[s64]);
                    if (wd[64] == 1)
                        keyboard(VK_A, S_A);
                    if (wu[64] == 1)
                        keyboardF(VK_A, S_A);
                    valchanged(65, Fbool[s65]);
                    if (wd[65] == 1)
                        keyboard(VK_B, S_B);
                    if (wu[65] == 1)
                        keyboardF(VK_B, S_B);
                    valchanged(66, Fbool[s66]);
                    if (wd[66] == 1)
                        keyboard(VK_C, S_C);
                    if (wu[66] == 1)
                        keyboardF(VK_C, S_C);
                    valchanged(67, Fbool[s67]);
                    if (wd[67] == 1)
                        keyboard(VK_D, S_D);
                    if (wu[67] == 1)
                        keyboardF(VK_D, S_D);
                    valchanged(68, Fbool[s68]);
                    if (wd[68] == 1)
                        keyboard(VK_E, S_E);
                    if (wu[68] == 1)
                        keyboardF(VK_E, S_E);
                    valchanged(69, Fbool[s69]);
                    if (wd[69] == 1)
                        keyboard(VK_F, S_F);
                    if (wu[69] == 1)
                        keyboardF(VK_F, S_F);
                    valchanged(70, Fbool[s70]);
                    if (wd[70] == 1)
                        keyboard(VK_G, S_G);
                    if (wu[70] == 1)
                        keyboardF(VK_G, S_G);
                    valchanged(71, Fbool[s71]);
                    if (wd[71] == 1)
                        keyboard(VK_H, S_H);
                    if (wu[71] == 1)
                        keyboardF(VK_H, S_H);
                    valchanged(72, Fbool[s72]);
                    if (wd[72] == 1)
                        keyboard(VK_I, S_I);
                    if (wu[72] == 1)
                        keyboardF(VK_I, S_I);
                    valchanged(73, Fbool[s73]);
                    if (wd[73] == 1)
                        keyboard(VK_J, S_J);
                    if (wu[73] == 1)
                        keyboardF(VK_J, S_J);
                    valchanged(74, Fbool[s74]);
                    if (wd[74] == 1)
                        keyboard(VK_K, S_K);
                    if (wu[74] == 1)
                        keyboardF(VK_K, S_K);
                    valchanged(75, Fbool[s75]);
                    if (wd[75] == 1)
                        keyboard(VK_L, S_L);
                    if (wu[75] == 1)
                        keyboardF(VK_L, S_L);
                    valchanged(76, Fbool[s76]);
                    if (wd[76] == 1)
                        keyboard(VK_M, S_M);
                    if (wu[76] == 1)
                        keyboardF(VK_M, S_M);
                    valchanged(77, Fbool[s77]);
                    if (wd[77] == 1)
                        keyboard(VK_N, S_N);
                    if (wu[77] == 1)
                        keyboardF(VK_N, S_N);
                    valchanged(78, Fbool[s78]);
                    if (wd[78] == 1)
                        keyboard(VK_O, S_O);
                    if (wu[78] == 1)
                        keyboardF(VK_O, S_O);
                    valchanged(79, Fbool[s79]);
                    if (wd[79] == 1)
                        keyboard(VK_P, S_P);
                    if (wu[79] == 1)
                        keyboardF(VK_P, S_P);
                    valchanged(80, Fbool[s80]);
                    if (wd[80] == 1)
                        keyboard(VK_Q, S_Q);
                    if (wu[80] == 1)
                        keyboardF(VK_Q, S_Q);
                    valchanged(81, Fbool[s81]);
                    if (wd[81] == 1)
                        keyboard(VK_R, S_R);
                    if (wu[81] == 1)
                        keyboardF(VK_R, S_R);
                    valchanged(82, Fbool[s82]);
                    if (wd[82] == 1)
                        keyboard(VK_S, S_S);
                    if (wu[82] == 1)
                        keyboardF(VK_S, S_S);
                    valchanged(83, Fbool[s83]);
                    if (wd[83] == 1)
                        keyboard(VK_T, S_T);
                    if (wu[83] == 1)
                        keyboardF(VK_T, S_T);
                    valchanged(84, Fbool[s84]);
                    if (wd[84] == 1)
                        keyboard(VK_U, S_U);
                    if (wu[84] == 1)
                        keyboardF(VK_U, S_U);
                    valchanged(85, Fbool[s85]);
                    if (wd[85] == 1)
                        keyboard(VK_V, S_V);
                    if (wu[85] == 1)
                        keyboardF(VK_V, S_V);
                    valchanged(86, Fbool[s86]);
                    if (wd[86] == 1)
                        keyboard(VK_W, S_W);
                    if (wu[86] == 1)
                        keyboardF(VK_W, S_W);
                    valchanged(87, Fbool[s87]);
                    if (wd[87] == 1)
                        keyboard(VK_X, S_X);
                    if (wu[87] == 1)
                        keyboardF(VK_X, S_X);
                    valchanged(88, Fbool[s88]);
                    if (wd[88] == 1)
                        keyboard(VK_Y, S_Y);
                    if (wu[88] == 1)
                        keyboardF(VK_Y, S_Y);
                    valchanged(89, Fbool[s89]);
                    if (wd[89] == 1)
                        keyboard(VK_Z, S_Z);
                    if (wu[89] == 1)
                        keyboardF(VK_Z, S_Z);
                    valchanged(90, Fbool[s90]);
                    if (wd[90] == 1)
                        keyboard(VK_LWIN, S_LWIN);
                    if (wu[90] == 1)
                        keyboardF(VK_LWIN, S_LWIN);
                    valchanged(91, Fbool[s91]);
                    if (wd[91] == 1)
                        keyboard(VK_RWIN, S_RWIN);
                    if (wu[91] == 1)
                        keyboardF(VK_RWIN, S_RWIN);
                    valchanged(92, Fbool[s92]);
                    if (wd[92] == 1)
                        keyboard(VK_APPS, S_APPS);
                    if (wu[92] == 1)
                        keyboardF(VK_APPS, S_APPS);
                    valchanged(93, Fbool[s93]);
                    if (wd[93] == 1)
                        keyboard(VK_SLEEP, S_SLEEP);
                    if (wu[93] == 1)
                        keyboardF(VK_SLEEP, S_SLEEP);
                    valchanged(94, Fbool[s94]);
                    if (wd[94] == 1)
                        keyboard(VK_NUMPAD0, S_NUMPAD0);
                    if (wu[94] == 1)
                        keyboardF(VK_NUMPAD0, S_NUMPAD0);
                    valchanged(95, Fbool[s95]);
                    if (wd[95] == 1)
                        keyboard(VK_NUMPAD1, S_NUMPAD1);
                    if (wu[95] == 1)
                        keyboardF(VK_NUMPAD1, S_NUMPAD1);
                    valchanged(96, Fbool[s96]);
                    if (wd[96] == 1)
                        keyboard(VK_NUMPAD2, S_NUMPAD2);
                    if (wu[96] == 1)
                        keyboardF(VK_NUMPAD2, S_NUMPAD2);
                    valchanged(97, Fbool[s97]);
                    if (wd[97] == 1)
                        keyboard(VK_NUMPAD3, S_NUMPAD3);
                    if (wu[97] == 1)
                        keyboardF(VK_NUMPAD3, S_NUMPAD3);
                    valchanged(98, Fbool[s98]);
                    if (wd[98] == 1)
                        keyboard(VK_NUMPAD4, S_NUMPAD4);
                    if (wu[98] == 1)
                        keyboardF(VK_NUMPAD4, S_NUMPAD4);
                    valchanged(99, Fbool[s99]);
                    if (wd[99] == 1)
                        keyboard(VK_NUMPAD5, S_NUMPAD5);
                    if (wu[99] == 1)
                        keyboardF(VK_NUMPAD5, S_NUMPAD5);
                    valchanged(100, Fbool[s100]);
                    if (wd[100] == 1)
                        keyboard(VK_NUMPAD6, S_NUMPAD6);
                    if (wu[100] == 1)
                        keyboardF(VK_NUMPAD6, S_NUMPAD6);
                    valchanged(101, Fbool[s101]);
                    if (wd[101] == 1)
                        keyboard(VK_NUMPAD7, S_NUMPAD7);
                    if (wu[101] == 1)
                        keyboardF(VK_NUMPAD7, S_NUMPAD7);
                    valchanged(102, Fbool[s102]);
                    if (wd[102] == 1)
                        keyboard(VK_NUMPAD8, S_NUMPAD8);
                    if (wu[102] == 1)
                        keyboardF(VK_NUMPAD8, S_NUMPAD8);
                    valchanged(103, Fbool[s103]);
                    if (wd[103] == 1)
                        keyboard(VK_NUMPAD9, S_NUMPAD9);
                    if (wu[103] == 1)
                        keyboardF(VK_NUMPAD9, S_NUMPAD9);
                    valchanged(104, Fbool[s104]);
                    if (wd[104] == 1)
                        keyboard(VK_MULTIPLY, S_MULTIPLY);
                    if (wu[104] == 1)
                        keyboardF(VK_MULTIPLY, S_MULTIPLY);
                    valchanged(105, Fbool[s105]);
                    if (wd[105] == 1)
                        keyboard(VK_ADD, S_ADD);
                    if (wu[105] == 1)
                        keyboardF(VK_ADD, S_ADD);
                    valchanged(106, Fbool[s106]);
                    if (wd[106] == 1)
                        keyboard(VK_SEPARATOR, S_SEPARATOR);
                    if (wu[106] == 1)
                        keyboardF(VK_SEPARATOR, S_SEPARATOR);
                    valchanged(107, Fbool[s107]);
                    if (wd[107] == 1)
                        keyboard(VK_SUBTRACT, S_SUBTRACT);
                    if (wu[107] == 1)
                        keyboardF(VK_SUBTRACT, S_SUBTRACT);
                    valchanged(108, Fbool[s108]);
                    if (wd[108] == 1)
                        keyboard(VK_DECIMAL, S_DECIMAL);
                    if (wu[108] == 1)
                        keyboardF(VK_DECIMAL, S_DECIMAL);
                    valchanged(109, Fbool[s109]);
                    if (wd[109] == 1)
                        keyboard(VK_DIVIDE, S_DIVIDE);
                    if (wu[109] == 1)
                        keyboardF(VK_DIVIDE, S_DIVIDE);
                    valchanged(110, Fbool[s110]);
                    if (wd[110] == 1)
                        keyboard(VK_F1, S_F1);
                    if (wu[110] == 1)
                        keyboardF(VK_F1, S_F1);
                    valchanged(111, Fbool[s111]);
                    if (wd[111] == 1)
                        keyboard(VK_F2, S_F2);
                    if (wu[111] == 1)
                        keyboardF(VK_F2, S_F2);
                    valchanged(112, Fbool[s112]);
                    if (wd[112] == 1)
                        keyboard(VK_F3, S_F3);
                    if (wu[112] == 1)
                        keyboardF(VK_F3, S_F3);
                    valchanged(113, Fbool[s113]);
                    if (wd[113] == 1)
                        keyboard(VK_F4, S_F4);
                    if (wu[113] == 1)
                        keyboardF(VK_F4, S_F4);
                    valchanged(114, Fbool[s114]);
                    if (wd[114] == 1)
                        keyboard(VK_F5, S_F5);
                    if (wu[114] == 1)
                        keyboardF(VK_F5, S_F5);
                    valchanged(115, Fbool[s115]);
                    if (wd[115] == 1)
                        keyboard(VK_F6, S_F6);
                    if (wu[115] == 1)
                        keyboardF(VK_F6, S_F6);
                    valchanged(116, Fbool[s116]);
                    if (wd[116] == 1)
                        keyboard(VK_F7, S_F7);
                    if (wu[116] == 1)
                        keyboardF(VK_F7, S_F7);
                    valchanged(117, Fbool[s117]);
                    if (wd[117] == 1)
                        keyboard(VK_F8, S_F8);
                    if (wu[117] == 1)
                        keyboardF(VK_F8, S_F8);
                    valchanged(118, Fbool[s118]);
                    if (wd[118] == 1)
                        keyboard(VK_F9, S_F9);
                    if (wu[118] == 1)
                        keyboardF(VK_F9, S_F9);
                    valchanged(119, Fbool[s119]);
                    if (wd[119] == 1)
                        keyboard(VK_F10, S_F10);
                    if (wu[119] == 1)
                        keyboardF(VK_F10, S_F10);
                    valchanged(120, Fbool[s120]);
                    if (wd[120] == 1)
                        keyboard(VK_F11, S_F11);
                    if (wu[120] == 1)
                        keyboardF(VK_F11, S_F11);
                    valchanged(121, Fbool[s121]);
                    if (wd[121] == 1)
                        keyboard(VK_F12, S_F12);
                    if (wu[121] == 1)
                        keyboardF(VK_F12, S_F12);
                    valchanged(122, Fbool[s122]);
                    if (wd[122] == 1)
                        keyboard(VK_F13, S_F13);
                    if (wu[122] == 1)
                        keyboardF(VK_F13, S_F13);
                    valchanged(123, Fbool[s123]);
                    if (wd[123] == 1)
                        keyboard(VK_F14, S_F14);
                    if (wu[123] == 1)
                        keyboardF(VK_F14, S_F14);
                    valchanged(124, Fbool[s124]);
                    if (wd[124] == 1)
                        keyboard(VK_F15, S_F15);
                    if (wu[124] == 1)
                        keyboardF(VK_F15, S_F15);
                    valchanged(125, Fbool[s125]);
                    if (wd[125] == 1)
                        keyboard(VK_F16, S_F16);
                    if (wu[125] == 1)
                        keyboardF(VK_F16, S_F16);
                    valchanged(126, Fbool[s126]);
                    if (wd[126] == 1)
                        keyboard(VK_F17, S_F17);
                    if (wu[126] == 1)
                        keyboardF(VK_F17, S_F17);
                    valchanged(127, Fbool[s127]);
                    if (wd[127] == 1)
                        keyboard(VK_F18, S_F18);
                    if (wu[127] == 1)
                        keyboardF(VK_F18, S_F18);
                    valchanged(128, Fbool[s128]);
                    if (wd[128] == 1)
                        keyboard(VK_F19, S_F19);
                    if (wu[128] == 1)
                        keyboardF(VK_F19, S_F19);
                    valchanged(129, Fbool[s129]);
                    if (wd[129] == 1)
                        keyboard(VK_F20, S_F20);
                    if (wu[129] == 1)
                        keyboardF(VK_F20, S_F20);
                    valchanged(130, Fbool[s130]);
                    if (wd[130] == 1)
                        keyboard(VK_F21, S_F21);
                    if (wu[130] == 1)
                        keyboardF(VK_F21, S_F21);
                    valchanged(131, Fbool[s131]);
                    if (wd[131] == 1)
                        keyboard(VK_F22, S_F22);
                    if (wu[131] == 1)
                        keyboardF(VK_F22, S_F22);
                    valchanged(132, Fbool[s132]);
                    if (wd[132] == 1)
                        keyboard(VK_F23, S_F23);
                    if (wu[132] == 1)
                        keyboardF(VK_F23, S_F23);
                    valchanged(133, Fbool[s133]);
                    if (wd[133] == 1)
                        keyboard(VK_F24, S_F24);
                    if (wu[133] == 1)
                        keyboardF(VK_F24, S_F24);
                    valchanged(134, Fbool[s134]);
                    if (wd[134] == 1)
                        keyboard(VK_NUMLOCK, S_NUMLOCK);
                    if (wu[134] == 1)
                        keyboardF(VK_NUMLOCK, S_NUMLOCK);
                    valchanged(135, Fbool[s135]);
                    if (wd[135] == 1)
                        keyboard(VK_SCROLL, S_SCROLL);
                    if (wu[135] == 1)
                        keyboardF(VK_SCROLL, S_SCROLL);
                    valchanged(136, Fbool[s136]);
                    if (wd[136] == 1)
                        keyboard(VK_LeftShift, S_LeftShift);
                    if (wu[136] == 1)
                        keyboardF(VK_LeftShift, S_LeftShift);
                    valchanged(137, Fbool[s137]);
                    if (wd[137] == 1)
                        keyboard(VK_RightShift, S_RightShift);
                    if (wu[137] == 1)
                        keyboardF(VK_RightShift, S_RightShift);
                    valchanged(138, Fbool[s138]);
                    if (wd[138] == 1)
                        keyboard(VK_LeftControl, S_LeftControl);
                    if (wu[138] == 1)
                        keyboardF(VK_LeftControl, S_LeftControl);
                    valchanged(139, Fbool[s139]);
                    if (wd[139] == 1)
                        keyboard(VK_RightControl, S_RightControl);
                    if (wu[139] == 1)
                        keyboardF(VK_RightControl, S_RightControl);
                    valchanged(140, Fbool[s140]);
                    if (wd[140] == 1)
                        keyboard(VK_LMENU, S_LMENU);
                    if (wu[140] == 1)
                        keyboardF(VK_LMENU, S_LMENU);
                    valchanged(141, Fbool[s141]);
                    if (wd[141] == 1)
                        keyboard(VK_RMENU, S_RMENU);
                    if (wu[141] == 1)
                        keyboardF(VK_RMENU, S_RMENU);
                }
                Thread.Sleep(10);
            }
        }
        private static void taskD()
        {
            while (running)
            {
                try
                {
                    mStream.Read(aBuffer, 0, 22);
                }
                catch { }
            }
        }
        private static void taskDLeft()
        {
            while (running)
            {
                try
                {
                    Lhid_read_timeout(handleLeft, report_bufLeft, (UIntPtr)report_lenLeft);
                }
                catch { }
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseControl();
        }
        private static void CloseControl()
        {
            if (!AlreadyRunning())
            {
                try
                {
                    running = false;
                    Thread.Sleep(100);
                    input.Unload();
                    threadstart = new ThreadStart(FormCloseLeft);
                    thread = new Thread(threadstart);
                    thread.Start();
                    Thread.Sleep(6000);
                    threadstart = new ThreadStart(FormClose);
                    thread = new Thread(threadstart);
                    thread.Start();
                }
                catch { }
            }
        }
        private static void FormClose()
        {
            try
            {
                mStream.Close();
                handle.Close();
                disconnect();
            }
            catch { }
        }
        private static void FormCloseLeft()
        {
            try
            {
                Lhid_close(handleLeft);
                handleLeft.Close();
                disconnectLeft();
            }
            catch { }
        }
        private const string vendor_id = "57e", vendor_id_ = "057e", product_r1 = "0330", product_r2 = "0306", product_l = "2006";
        private enum EFileAttributes : uint
        {
            Overlapped = 0x40000000,
            Normal = 0x80
        };
        struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr RESERVED;
        }
        struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
        private static bool ScanWiimote()
        {
            int index = 0;
            Guid guid;
            HidD_GetHidGuid(out guid);
            IntPtr hDevInfo = SetupDiGetClassDevs(ref guid, null, new IntPtr(), 0x00000010);
            SP_DEVICE_INTERFACE_DATA diData = new SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = Marshal.SizeOf(diData);
            while (SetupDiEnumDeviceInterfaces(hDevInfo, new IntPtr(), ref guid, index, ref diData))
            {
                UInt32 size;
                SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, new IntPtr(), 0, out size, new IntPtr());
                SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                diDetail.cbSize = 5;
                if (SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, new IntPtr()))
                {
                    if ((diDetail.DevicePath.Contains(vendor_id) | diDetail.DevicePath.Contains(vendor_id_)) & (diDetail.DevicePath.Contains(product_r1) | diDetail.DevicePath.Contains(product_r2)))
                    {
                        WiimoteFound(diDetail.DevicePath);
                        WiimoteFound(diDetail.DevicePath);
                        WiimoteFound(diDetail.DevicePath);
                        return true;
                    }
                }
                index++;
            }
            return false;
        }
        private static void WiimoteFound(string path)
        {
            do
            {
                handle = CreateFile(path, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, (uint)EFileAttributes.Overlapped, IntPtr.Zero);
                WriteData(handle, IR, (int)REGISTER_IR, new byte[] { 0x08 }, 1);
                WriteData(handle, Type, (int)REGISTER_EXTENSION_INIT_1, new byte[] { 0x55 }, 1);
                WriteData(handle, Type, (int)REGISTER_EXTENSION_INIT_2, new byte[] { 0x00 }, 1);
                WriteData(handle, Type, (int)REGISTER_MOTIONPLUS_INIT, new byte[] { 0x04 }, 1);
                ReadData(handle, 0x0016, 7);
                ReadData(handle, (int)REGISTER_EXTENSION_TYPE, 6);
                ReadData(handle, (int)REGISTER_EXTENSION_CALIBRATION, 16);
                ReadData(handle, (int)REGISTER_EXTENSION_CALIBRATION, 32);
            }
            while (handle.IsInvalid);
            mStream = new FileStream(handle, FileAccess.ReadWrite, 22, true);
        }
        private static void ReadData(SafeFileHandle _hFile, int address, short size)
        {
            mBuff[0] = (byte)ReadMemory;
            mBuff[1] = (byte)((address & 0xff000000) >> 24);
            mBuff[2] = (byte)((address & 0x00ff0000) >> 16);
            mBuff[3] = (byte)((address & 0x0000ff00) >> 8);
            mBuff[4] = (byte)(address & 0x000000ff);
            mBuff[5] = (byte)((size & 0xff00) >> 8);
            mBuff[6] = (byte)(size & 0xff);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff, 22);
        }
        private static void WriteData(SafeFileHandle _hFile, byte mbuff, int address, byte[] buff, short size)
        {
            mBuff[0] = (byte)mbuff;
            mBuff[1] = (byte)(0x04);
            mBuff[2] = (byte)IRExtensionAccel;
            Array.Copy(buff, 0, mBuff, 3, 1);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff, 22);
            mBuff[0] = (byte)WriteMemory;
            mBuff[1] = (byte)(((address & 0xff000000) >> 24));
            mBuff[2] = (byte)((address & 0x00ff0000) >> 16);
            mBuff[3] = (byte)((address & 0x0000ff00) >> 8);
            mBuff[4] = (byte)((address & 0x000000ff) >> 0);
            mBuff[5] = (byte)size;
            Array.Copy(buff, 0, mBuff, 6, 1);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff, 22);
        }
        private static double[] CenterSticksLeft(UInt16[] vals)
        {
            double[] s = { 0, 0 };
            s[0] = ((int)((vals[0] - stick_calibrationLeft[0]) / 100f)) / 13f;
            s[1] = ((int)((vals[1] - stick_calibrationLeft[1]) / 100f)) / 13f;
            return s;
        }
        private static double[] stickLeft = { 0, 0 };
        private static SafeFileHandle handleLeft;
        private static Vector3 acc_gLeft = new Vector3();
        private static byte[] stick_rawLeft = { 0, 0, 0 };
        private static UInt16[] stick_calibrationLeft = { 0, 0 };
        private static UInt16[] stick_precalLeft = { 0, 0 };
        private const uint report_lenLeft = 25;
        private static bool LeftButtonSHOULDER_1, LeftButtonSHOULDER_2, LeftButtonSR, LeftButtonSL, LeftButtonDPAD_DOWN, LeftButtonDPAD_RIGHT, LeftButtonDPAD_UP, LeftButtonDPAD_LEFT, LeftButtonMINUS, LeftButtonSTICK, LeftButtonCAPTURE, ISLEFT;
        private static byte[] report_bufLeft = new byte[report_lenLeft];
        private static byte[] buf_Left = new byte[report_lenLeft];
        private static float acc_gcalibrationLeftX, acc_gcalibrationLeftY, acc_gcalibrationLeftZ;
        private static double[] GetStickLeft()
        {
            return stickLeft;
        }
        private static bool ScanLeft()
        {
            int index = 0;
            System.Guid guid;
            HidD_GetHidGuid(out guid);
            System.IntPtr hDevInfo = SetupDiGetClassDevs(ref guid, null, new System.IntPtr(), 0x00000010);
            SP_DEVICE_INTERFACE_DATA diData = new SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(diData);
            while (SetupDiEnumDeviceInterfaces(hDevInfo, new System.IntPtr(), ref guid, index, ref diData))
            {
                System.UInt32 size;
                SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, new System.IntPtr(), 0, out size, new System.IntPtr());
                SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                diDetail.cbSize = 5;
                if (SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, new System.IntPtr()))
                {
                    if ((diDetail.DevicePath.Contains(vendor_id) | diDetail.DevicePath.Contains(vendor_id_)) & diDetail.DevicePath.Contains(product_l))
                    {
                        ISLEFT = true;
                        AttachJoyLeft(diDetail.DevicePath);
                        AttachJoyLeft(diDetail.DevicePath);
                        AttachJoyLeft(diDetail.DevicePath);
                        return true;
                    }
                }
                index++;
            }
            return false;
        }
        private static void AttachJoyLeft(string path)
        {
            do
            {
                IntPtr handle = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleLeft = Lhid_open_path(handle);
                SubcommandLeft(0x3, new byte[] { 0x30 }, 1);
                SubcommandLeft(0x40, new byte[] { 0x1 }, 1);
                SubcommandLeft(0x60, new byte[] { 0x86 }, 1);
                SubcommandLeft(0x80, new byte[] { 0x34 }, 1);
            }
            while (handleLeft.IsInvalid);
        }
        private static void SubcommandLeft(byte sc, byte[] buf, uint len)
        {
            Array.Copy(buf, 0, buf_Left, 11, len);
            buf_Left[0] = 0x1;
            buf_Left[1] = 0;
            buf_Left[10] = sc;
            Lhid_write(handleLeft, buf_Left, (UIntPtr)(len + 11));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)report_lenLeft);
        }
    }
}
