using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace WiiTrol
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("WiimotePairing.dll", EntryPoint = "connect")]
        public static unsafe extern bool connect();
        [DllImport("WiimotePairing.dll", EntryPoint = "disconnect")]
        public static unsafe extern bool disconnect();
        [DllImport("hid.dll")]
        public static unsafe extern void HidD_GetHidGuid(out Guid gHid);
        [DllImport("hid.dll")]
        public extern unsafe static bool HidD_SetOutputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);
        [DllImport("setupapi.dll")]
        public static unsafe extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string Enumerator, IntPtr hwndParent, UInt32 Flags);
        [DllImport("setupapi.dll")]
        public static unsafe extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInvo, ref Guid interfaceClassGuid, Int32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);
        [DllImport("setupapi.dll")]
        public static unsafe extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("setupapi.dll")]
        public static unsafe extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("Kernel32.dll")]
        public static unsafe extern SafeFileHandle CreateFile(string fileName, [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess, [MarshalAs(UnmanagedType.U4)] FileShare fileShare, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] uint flags, IntPtr template);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        [DllImport("SendInputLibrary.dll", EntryPoint = "SimulateKeyDown", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyDown(UInt16 keyCode, UInt16 bScan);
        [DllImport("SendInputLibrary.dll", EntryPoint = "SimulateKeyUp", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyUp(UInt16 keyCode, UInt16 bScan);
        [DllImport("SendInputLibrary.dll", EntryPoint = "SimulateKeyDownArrows", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyDownArrows(UInt16 keyCode, UInt16 bScan);
        [DllImport("SendInputLibrary.dll", EntryPoint = "SimulateKeyUpArrows", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyUpArrows(UInt16 keyCode, UInt16 bScan);
        [DllImport("SendInputLibrary.dll", EntryPoint = "MouseMW3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseMW3(int x, int y);
        [DllImport("SendInputLibrary.dll", EntryPoint = "MouseBrink", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseBrink(int x, int y);
        [DllImport("SendInputLibrary.dll", EntryPoint = "LeftClick", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LeftClick();
        [DllImport("SendInputLibrary.dll", EntryPoint = "LeftClickF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LeftClickF();
        [DllImport("SendInputLibrary.dll", EntryPoint = "RightClick", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RightClick();
        [DllImport("SendInputLibrary.dll", EntryPoint = "RightClickF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RightClickF();
        [DllImport("SendInputLibrary.dll", EntryPoint = "MiddleClick", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiddleClick();
        [DllImport("SendInputLibrary.dll", EntryPoint = "MiddleClickF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiddleClickF();
        [DllImport("SendInputLibrary.dll", EntryPoint = "WheelDownF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WheelDownF();
        [DllImport("SendInputLibrary.dll", EntryPoint = "WheelUpF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WheelUpF();
        [DllImport("user32.dll")]
        public static extern void SetPhysicalCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern void SetCaretPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);
        public static ushort VK_LBUTTON = (ushort)0x01;
        public static ushort VK_RBUTTON = (ushort)0x02;
        public static ushort VK_CANCEL = (ushort)0x03;
        public static ushort VK_MBUTTON = (ushort)0x04;
        public static ushort VK_XBUTTON1 = (ushort)0x05;
        public static ushort VK_XBUTTON2 = (ushort)0x06;
        public static ushort VK_BACK = (ushort)0x08;
        public static ushort VK_Tab = (ushort)0x09;
        public static ushort VK_CLEAR = (ushort)0x0C;
        public static ushort VK_Return = (ushort)0x0D;
        public static ushort VK_SHIFT = (ushort)0x10;
        public static ushort VK_CONTROL = (ushort)0x11;
        public static ushort VK_MENU = (ushort)0x12;
        public static ushort VK_PAUSE = (ushort)0x13;
        public static ushort VK_CAPITAL = (ushort)0x14;
        public static ushort VK_KANA = (ushort)0x15;
        public static ushort VK_HANGEUL = (ushort)0x15;
        public static ushort VK_HANGUL = (ushort)0x15;
        public static ushort VK_JUNJA = (ushort)0x17;
        public static ushort VK_FINAL = (ushort)0x18;
        public static ushort VK_HANJA = (ushort)0x19;
        public static ushort VK_KANJI = (ushort)0x19;
        public static ushort VK_Escape = (ushort)0x1B;
        public static ushort VK_CONVERT = (ushort)0x1C;
        public static ushort VK_NONCONVERT = (ushort)0x1D;
        public static ushort VK_ACCEPT = (ushort)0x1E;
        public static ushort VK_MODECHANGE = (ushort)0x1F;
        public static ushort VK_Space = (ushort)0x20;
        public static ushort VK_PRIOR = (ushort)0x21;
        public static ushort VK_NEXT = (ushort)0x22;
        public static ushort VK_END = (ushort)0x23;
        public static ushort VK_HOME = (ushort)0x24;
        public static ushort VK_LEFT = (ushort)0x25;
        public static ushort VK_UP = (ushort)0x26;
        public static ushort VK_RIGHT = (ushort)0x27;
        public static ushort VK_DOWN = (ushort)0x28;
        public static ushort VK_SELECT = (ushort)0x29;
        public static ushort VK_PRINT = (ushort)0x2A;
        public static ushort VK_EXECUTE = (ushort)0x2B;
        public static ushort VK_SNAPSHOT = (ushort)0x2C;
        public static ushort VK_INSERT = (ushort)0x2D;
        public static ushort VK_DELETE = (ushort)0x2E;
        public static ushort VK_HELP = (ushort)0x2F;
        public static ushort VK_APOSTROPHE = (ushort)0xDE;
        public static ushort VK_0 = (ushort)0x30;
        public static ushort VK_1 = (ushort)0x31;
        public static ushort VK_2 = (ushort)0x32;
        public static ushort VK_3 = (ushort)0x33;
        public static ushort VK_4 = (ushort)0x34;
        public static ushort VK_5 = (ushort)0x35;
        public static ushort VK_6 = (ushort)0x36;
        public static ushort VK_7 = (ushort)0x37;
        public static ushort VK_8 = (ushort)0x38;
        public static ushort VK_9 = (ushort)0x39;
        public static ushort VK_A = (ushort)0x41;
        public static ushort VK_B = (ushort)0x42;
        public static ushort VK_C = (ushort)0x43;
        public static ushort VK_D = (ushort)0x44;
        public static ushort VK_E = (ushort)0x45;
        public static ushort VK_F = (ushort)0x46;
        public static ushort VK_G = (ushort)0x47;
        public static ushort VK_H = (ushort)0x48;
        public static ushort VK_I = (ushort)0x49;
        public static ushort VK_J = (ushort)0x4A;
        public static ushort VK_K = (ushort)0x4B;
        public static ushort VK_L = (ushort)0x4C;
        public static ushort VK_M = (ushort)0x4D;
        public static ushort VK_N = (ushort)0x4E;
        public static ushort VK_O = (ushort)0x4F;
        public static ushort VK_P = (ushort)0x50;
        public static ushort VK_Q = (ushort)0x51;
        public static ushort VK_R = (ushort)0x52;
        public static ushort VK_S = (ushort)0x53;
        public static ushort VK_T = (ushort)0x54;
        public static ushort VK_U = (ushort)0x55;
        public static ushort VK_V = (ushort)0x56;
        public static ushort VK_W = (ushort)0x57;
        public static ushort VK_X = (ushort)0x58;
        public static ushort VK_Y = (ushort)0x59;
        public static ushort VK_Z = (ushort)0x5A;
        public static ushort VK_LWIN = (ushort)0x5B;
        public static ushort VK_RWIN = (ushort)0x5C;
        public static ushort VK_APPS = (ushort)0x5D;
        public static ushort VK_SLEEP = (ushort)0x5F;
        public static ushort VK_NUMPAD0 = (ushort)0x60;
        public static ushort VK_NUMPAD1 = (ushort)0x61;
        public static ushort VK_NUMPAD2 = (ushort)0x62;
        public static ushort VK_NUMPAD3 = (ushort)0x63;
        public static ushort VK_NUMPAD4 = (ushort)0x64;
        public static ushort VK_NUMPAD5 = (ushort)0x65;
        public static ushort VK_NUMPAD6 = (ushort)0x66;
        public static ushort VK_NUMPAD7 = (ushort)0x67;
        public static ushort VK_NUMPAD8 = (ushort)0x68;
        public static ushort VK_NUMPAD9 = (ushort)0x69;
        public static ushort VK_MULTIPLY = (ushort)0x6A;
        public static ushort VK_ADD = (ushort)0x6B;
        public static ushort VK_SEPARATOR = (ushort)0x6C;
        public static ushort VK_SUBTRACT = (ushort)0x6D;
        public static ushort VK_DECIMAL = (ushort)0x6E;
        public static ushort VK_DIVIDE = (ushort)0x6F;
        public static ushort VK_F1 = (ushort)0x70;
        public static ushort VK_F2 = (ushort)0x71;
        public static ushort VK_F3 = (ushort)0x72;
        public static ushort VK_F4 = (ushort)0x73;
        public static ushort VK_F5 = (ushort)0x74;
        public static ushort VK_F6 = (ushort)0x75;
        public static ushort VK_F7 = (ushort)0x76;
        public static ushort VK_F8 = (ushort)0x77;
        public static ushort VK_F9 = (ushort)0x78;
        public static ushort VK_F10 = (ushort)0x79;
        public static ushort VK_F11 = (ushort)0x7A;
        public static ushort VK_F12 = (ushort)0x7B;
        public static ushort VK_F13 = (ushort)0x7C;
        public static ushort VK_F14 = (ushort)0x7D;
        public static ushort VK_F15 = (ushort)0x7E;
        public static ushort VK_F16 = (ushort)0x7F;
        public static ushort VK_F17 = (ushort)0x80;
        public static ushort VK_F18 = (ushort)0x81;
        public static ushort VK_F19 = (ushort)0x82;
        public static ushort VK_F20 = (ushort)0x83;
        public static ushort VK_F21 = (ushort)0x84;
        public static ushort VK_F22 = (ushort)0x85;
        public static ushort VK_F23 = (ushort)0x86;
        public static ushort VK_F24 = (ushort)0x87;
        public static ushort VK_NUMLOCK = (ushort)0x90;
        public static ushort VK_SCROLL = (ushort)0x91;
        public static ushort VK_LeftShift = (ushort)0xA0;
        public static ushort VK_RightShift = (ushort)0xA1;
        public static ushort VK_LeftControl = (ushort)0xA2;
        public static ushort VK_RightControl = (ushort)0xA3;
        public static ushort VK_LMENU = (ushort)0xA4;
        public static ushort VK_RMENU = (ushort)0xA5;
        public static ushort VK_BROWSER_BACK = (ushort)0xA6;
        public static ushort VK_BROWSER_FORWARD = (ushort)0xA7;
        public static ushort VK_BROWSER_REFRESH = (ushort)0xA8;
        public static ushort VK_BROWSER_STOP = (ushort)0xA9;
        public static ushort VK_BROWSER_SEARCH = (ushort)0xAA;
        public static ushort VK_BROWSER_FAVORITES = (ushort)0xAB;
        public static ushort VK_BROWSER_HOME = (ushort)0xAC;
        public static ushort VK_VOLUME_MUTE = (ushort)0xAD;
        public static ushort VK_VOLUME_DOWN = (ushort)0xAE;
        public static ushort VK_VOLUME_UP = (ushort)0xAF;
        public static ushort VK_MEDIA_NEXT_TRACK = (ushort)0xB0;
        public static ushort VK_MEDIA_PREV_TRACK = (ushort)0xB1;
        public static ushort VK_MEDIA_STOP = (ushort)0xB2;
        public static ushort VK_MEDIA_PLAY_PAUSE = (ushort)0xB3;
        public static ushort VK_LAUNCH_MAIL = (ushort)0xB4;
        public static ushort VK_LAUNCH_MEDIA_SELECT = (ushort)0xB5;
        public static ushort VK_LAUNCH_APP1 = (ushort)0xB6;
        public static ushort VK_LAUNCH_APP2 = (ushort)0xB7;
        public static ushort VK_OEM_1 = (ushort)0xBA;
        public static ushort VK_OEM_PLUS = (ushort)0xBB;
        public static ushort VK_OEM_COMMA = (ushort)0xBC;
        public static ushort VK_OEM_MINUS = (ushort)0xBD;
        public static ushort VK_OEM_PERIOD = (ushort)0xBE;
        public static ushort VK_OEM_2 = (ushort)0xBF;
        public static ushort VK_OEM_3 = (ushort)0xC0;
        public static ushort VK_OEM_4 = (ushort)0xDB;
        public static ushort VK_OEM_5 = (ushort)0xDC;
        public static ushort VK_OEM_6 = (ushort)0xDD;
        public static ushort VK_OEM_7 = (ushort)0xDE;
        public static ushort VK_OEM_8 = (ushort)0xDF;
        public static ushort VK_OEM_102 = (ushort)0xE2;
        public static ushort VK_PROCESSKEY = (ushort)0xE5;
        public static ushort VK_PACKET = (ushort)0xE7;
        public static ushort VK_ATTN = (ushort)0xF6;
        public static ushort VK_CRSEL = (ushort)0xF7;
        public static ushort VK_EXSEL = (ushort)0xF8;
        public static ushort VK_EREOF = (ushort)0xF9;
        public static ushort VK_PLAY = (ushort)0xFA;
        public static ushort VK_ZOOM = (ushort)0xFB;
        public static ushort VK_NONAME = (ushort)0xFC;
        public static ushort VK_PA1 = (ushort)0xFD;
        public static ushort VK_OEM_CLEAR = (ushort)0xFE;
        public static ushort S_LBUTTON = (ushort)MapVirtualKey(0x01, 0);
        public static ushort S_RBUTTON = (ushort)MapVirtualKey(0x02, 0);
        public static ushort S_CANCEL = (ushort)MapVirtualKey(0x03, 0);
        public static ushort S_MBUTTON = (ushort)MapVirtualKey(0x04, 0);
        public static ushort S_XBUTTON1 = (ushort)MapVirtualKey(0x05, 0);
        public static ushort S_XBUTTON2 = (ushort)MapVirtualKey(0x06, 0);
        public static ushort S_BACK = (ushort)MapVirtualKey(0x08, 0);
        public static ushort S_Tab = (ushort)MapVirtualKey(0x09, 0);
        public static ushort S_CLEAR = (ushort)MapVirtualKey(0x0C, 0);
        public static ushort S_Return = (ushort)MapVirtualKey(0x0D, 0);
        public static ushort S_SHIFT = (ushort)MapVirtualKey(0x10, 0);
        public static ushort S_CONTROL = (ushort)MapVirtualKey(0x11, 0);
        public static ushort S_MENU = (ushort)MapVirtualKey(0x12, 0);
        public static ushort S_PAUSE = (ushort)MapVirtualKey(0x13, 0);
        public static ushort S_CAPITAL = (ushort)MapVirtualKey(0x14, 0);
        public static ushort S_KANA = (ushort)MapVirtualKey(0x15, 0);
        public static ushort S_HANGEUL = (ushort)MapVirtualKey(0x15, 0);
        public static ushort S_HANGUL = (ushort)MapVirtualKey(0x15, 0);
        public static ushort S_JUNJA = (ushort)MapVirtualKey(0x17, 0);
        public static ushort S_FINAL = (ushort)MapVirtualKey(0x18, 0);
        public static ushort S_HANJA = (ushort)MapVirtualKey(0x19, 0);
        public static ushort S_KANJI = (ushort)MapVirtualKey(0x19, 0);
        public static ushort S_Escape = (ushort)MapVirtualKey(0x1B, 0);
        public static ushort S_CONVERT = (ushort)MapVirtualKey(0x1C, 0);
        public static ushort S_NONCONVERT = (ushort)MapVirtualKey(0x1D, 0);
        public static ushort S_ACCEPT = (ushort)MapVirtualKey(0x1E, 0);
        public static ushort S_MODECHANGE = (ushort)MapVirtualKey(0x1F, 0);
        public static ushort S_Space = (ushort)MapVirtualKey(0x20, 0);
        public static ushort S_PRIOR = (ushort)MapVirtualKey(0x21, 0);
        public static ushort S_NEXT = (ushort)MapVirtualKey(0x22, 0);
        public static ushort S_END = (ushort)MapVirtualKey(0x23, 0);
        public static ushort S_HOME = (ushort)MapVirtualKey(0x24, 0);
        public static ushort S_LEFT = (ushort)MapVirtualKey(0x25, 0);
        public static ushort S_UP = (ushort)MapVirtualKey(0x26, 0);
        public static ushort S_RIGHT = (ushort)MapVirtualKey(0x27, 0);
        public static ushort S_DOWN = (ushort)MapVirtualKey(0x28, 0);
        public static ushort S_SELECT = (ushort)MapVirtualKey(0x29, 0);
        public static ushort S_PRINT = (ushort)MapVirtualKey(0x2A, 0);
        public static ushort S_EXECUTE = (ushort)MapVirtualKey(0x2B, 0);
        public static ushort S_SNAPSHOT = (ushort)MapVirtualKey(0x2C, 0);
        public static ushort S_INSERT = (ushort)MapVirtualKey(0x2D, 0);
        public static ushort S_DELETE = (ushort)MapVirtualKey(0x2E, 0);
        public static ushort S_HELP = (ushort)MapVirtualKey(0x2F, 0);
        public static ushort S_APOSTROPHE = (ushort)MapVirtualKey(0xDE, 0);
        public static ushort S_0 = (ushort)MapVirtualKey(0x30, 0);
        public static ushort S_1 = (ushort)MapVirtualKey(0x31, 0);
        public static ushort S_2 = (ushort)MapVirtualKey(0x32, 0);
        public static ushort S_3 = (ushort)MapVirtualKey(0x33, 0);
        public static ushort S_4 = (ushort)MapVirtualKey(0x34, 0);
        public static ushort S_5 = (ushort)MapVirtualKey(0x35, 0);
        public static ushort S_6 = (ushort)MapVirtualKey(0x36, 0);
        public static ushort S_7 = (ushort)MapVirtualKey(0x37, 0);
        public static ushort S_8 = (ushort)MapVirtualKey(0x38, 0);
        public static ushort S_9 = (ushort)MapVirtualKey(0x39, 0);
        public static ushort S_A = (ushort)MapVirtualKey(0x41, 0);
        public static ushort S_B = (ushort)MapVirtualKey(0x42, 0);
        public static ushort S_C = (ushort)MapVirtualKey(0x43, 0);
        public static ushort S_D = (ushort)MapVirtualKey(0x44, 0);
        public static ushort S_E = (ushort)MapVirtualKey(0x45, 0);
        public static ushort S_F = (ushort)MapVirtualKey(0x46, 0);
        public static ushort S_G = (ushort)MapVirtualKey(0x47, 0);
        public static ushort S_H = (ushort)MapVirtualKey(0x48, 0);
        public static ushort S_I = (ushort)MapVirtualKey(0x49, 0);
        public static ushort S_J = (ushort)MapVirtualKey(0x4A, 0);
        public static ushort S_K = (ushort)MapVirtualKey(0x4B, 0);
        public static ushort S_L = (ushort)MapVirtualKey(0x4C, 0);
        public static ushort S_M = (ushort)MapVirtualKey(0x4D, 0);
        public static ushort S_N = (ushort)MapVirtualKey(0x4E, 0);
        public static ushort S_O = (ushort)MapVirtualKey(0x4F, 0);
        public static ushort S_P = (ushort)MapVirtualKey(0x50, 0);
        public static ushort S_Q = (ushort)MapVirtualKey(0x51, 0);
        public static ushort S_R = (ushort)MapVirtualKey(0x52, 0);
        public static ushort S_S = (ushort)MapVirtualKey(0x53, 0);
        public static ushort S_T = (ushort)MapVirtualKey(0x54, 0);
        public static ushort S_U = (ushort)MapVirtualKey(0x55, 0);
        public static ushort S_V = (ushort)MapVirtualKey(0x56, 0);
        public static ushort S_W = (ushort)MapVirtualKey(0x57, 0);
        public static ushort S_X = (ushort)MapVirtualKey(0x58, 0);
        public static ushort S_Y = (ushort)MapVirtualKey(0x59, 0);
        public static ushort S_Z = (ushort)MapVirtualKey(0x5A, 0);
        public static ushort S_LWIN = (ushort)MapVirtualKey(0x5B, 0);
        public static ushort S_RWIN = (ushort)MapVirtualKey(0x5C, 0);
        public static ushort S_APPS = (ushort)MapVirtualKey(0x5D, 0);
        public static ushort S_SLEEP = (ushort)MapVirtualKey(0x5F, 0);
        public static ushort S_NUMPAD0 = (ushort)MapVirtualKey(0x60, 0);
        public static ushort S_NUMPAD1 = (ushort)MapVirtualKey(0x61, 0);
        public static ushort S_NUMPAD2 = (ushort)MapVirtualKey(0x62, 0);
        public static ushort S_NUMPAD3 = (ushort)MapVirtualKey(0x63, 0);
        public static ushort S_NUMPAD4 = (ushort)MapVirtualKey(0x64, 0);
        public static ushort S_NUMPAD5 = (ushort)MapVirtualKey(0x65, 0);
        public static ushort S_NUMPAD6 = (ushort)MapVirtualKey(0x66, 0);
        public static ushort S_NUMPAD7 = (ushort)MapVirtualKey(0x67, 0);
        public static ushort S_NUMPAD8 = (ushort)MapVirtualKey(0x68, 0);
        public static ushort S_NUMPAD9 = (ushort)MapVirtualKey(0x69, 0);
        public static ushort S_MULTIPLY = (ushort)MapVirtualKey(0x6A, 0);
        public static ushort S_ADD = (ushort)MapVirtualKey(0x6B, 0);
        public static ushort S_SEPARATOR = (ushort)MapVirtualKey(0x6C, 0);
        public static ushort S_SUBTRACT = (ushort)MapVirtualKey(0x6D, 0);
        public static ushort S_DECIMAL = (ushort)MapVirtualKey(0x6E, 0);
        public static ushort S_DIVIDE = (ushort)MapVirtualKey(0x6F, 0);
        public static ushort S_F1 = (ushort)MapVirtualKey(0x70, 0);
        public static ushort S_F2 = (ushort)MapVirtualKey(0x71, 0);
        public static ushort S_F3 = (ushort)MapVirtualKey(0x72, 0);
        public static ushort S_F4 = (ushort)MapVirtualKey(0x73, 0);
        public static ushort S_F5 = (ushort)MapVirtualKey(0x74, 0);
        public static ushort S_F6 = (ushort)MapVirtualKey(0x75, 0);
        public static ushort S_F7 = (ushort)MapVirtualKey(0x76, 0);
        public static ushort S_F8 = (ushort)MapVirtualKey(0x77, 0);
        public static ushort S_F9 = (ushort)MapVirtualKey(0x78, 0);
        public static ushort S_F10 = (ushort)MapVirtualKey(0x79, 0);
        public static ushort S_F11 = (ushort)MapVirtualKey(0x7A, 0);
        public static ushort S_F12 = (ushort)MapVirtualKey(0x7B, 0);
        public static ushort S_F13 = (ushort)MapVirtualKey(0x7C, 0);
        public static ushort S_F14 = (ushort)MapVirtualKey(0x7D, 0);
        public static ushort S_F15 = (ushort)MapVirtualKey(0x7E, 0);
        public static ushort S_F16 = (ushort)MapVirtualKey(0x7F, 0);
        public static ushort S_F17 = (ushort)MapVirtualKey(0x80, 0);
        public static ushort S_F18 = (ushort)MapVirtualKey(0x81, 0);
        public static ushort S_F19 = (ushort)MapVirtualKey(0x82, 0);
        public static ushort S_F20 = (ushort)MapVirtualKey(0x83, 0);
        public static ushort S_F21 = (ushort)MapVirtualKey(0x84, 0);
        public static ushort S_F22 = (ushort)MapVirtualKey(0x85, 0);
        public static ushort S_F23 = (ushort)MapVirtualKey(0x86, 0);
        public static ushort S_F24 = (ushort)MapVirtualKey(0x87, 0);
        public static ushort S_NUMLOCK = (ushort)MapVirtualKey(0x90, 0);
        public static ushort S_SCROLL = (ushort)MapVirtualKey(0x91, 0);
        public static ushort S_LeftShift = (ushort)MapVirtualKey(0xA0, 0);
        public static ushort S_RightShift = (ushort)MapVirtualKey(0xA1, 0);
        public static ushort S_LeftControl = (ushort)MapVirtualKey(0xA2, 0);
        public static ushort S_RightControl = (ushort)MapVirtualKey(0xA3, 0);
        public static ushort S_LMENU = (ushort)MapVirtualKey(0xA4, 0);
        public static ushort S_RMENU = (ushort)MapVirtualKey(0xA5, 0);
        public static ushort S_BROWSER_BACK = (ushort)MapVirtualKey(0xA6, 0);
        public static ushort S_BROWSER_FORWARD = (ushort)MapVirtualKey(0xA7, 0);
        public static ushort S_BROWSER_REFRESH = (ushort)MapVirtualKey(0xA8, 0);
        public static ushort S_BROWSER_STOP = (ushort)MapVirtualKey(0xA9, 0);
        public static ushort S_BROWSER_SEARCH = (ushort)MapVirtualKey(0xAA, 0);
        public static ushort S_BROWSER_FAVORITES = (ushort)MapVirtualKey(0xAB, 0);
        public static ushort S_BROWSER_HOME = (ushort)MapVirtualKey(0xAC, 0);
        public static ushort S_VOLUME_MUTE = (ushort)MapVirtualKey(0xAD, 0);
        public static ushort S_VOLUME_DOWN = (ushort)MapVirtualKey(0xAE, 0);
        public static ushort S_VOLUME_UP = (ushort)MapVirtualKey(0xAF, 0);
        public static ushort S_MEDIA_NEXT_TRACK = (ushort)MapVirtualKey(0xB0, 0);
        public static ushort S_MEDIA_PREV_TRACK = (ushort)MapVirtualKey(0xB1, 0);
        public static ushort S_MEDIA_STOP = (ushort)MapVirtualKey(0xB2, 0);
        public static ushort S_MEDIA_PLAY_PAUSE = (ushort)MapVirtualKey(0xB3, 0);
        public static ushort S_LAUNCH_MAIL = (ushort)MapVirtualKey(0xB4, 0);
        public static ushort S_LAUNCH_MEDIA_SELECT = (ushort)MapVirtualKey(0xB5, 0);
        public static ushort S_LAUNCH_APP1 = (ushort)MapVirtualKey(0xB6, 0);
        public static ushort S_LAUNCH_APP2 = (ushort)MapVirtualKey(0xB7, 0);
        public static ushort S_OEM_1 = (ushort)MapVirtualKey(0xBA, 0);
        public static ushort S_OEM_PLUS = (ushort)MapVirtualKey(0xBB, 0);
        public static ushort S_OEM_COMMA = (ushort)MapVirtualKey(0xBC, 0);
        public static ushort S_OEM_MINUS = (ushort)MapVirtualKey(0xBD, 0);
        public static ushort S_OEM_PERIOD = (ushort)MapVirtualKey(0xBE, 0);
        public static ushort S_OEM_2 = (ushort)MapVirtualKey(0xBF, 0);
        public static ushort S_OEM_3 = (ushort)MapVirtualKey(0xC0, 0);
        public static ushort S_OEM_4 = (ushort)MapVirtualKey(0xDB, 0);
        public static ushort S_OEM_5 = (ushort)MapVirtualKey(0xDC, 0);
        public static ushort S_OEM_6 = (ushort)MapVirtualKey(0xDD, 0);
        public static ushort S_OEM_7 = (ushort)MapVirtualKey(0xDE, 0);
        public static ushort S_OEM_8 = (ushort)MapVirtualKey(0xDF, 0);
        public static ushort S_OEM_102 = (ushort)MapVirtualKey(0xE2, 0);
        public static ushort S_PROCESSKEY = (ushort)MapVirtualKey(0xE5, 0);
        public static ushort S_PACKET = (ushort)MapVirtualKey(0xE7, 0);
        public static ushort S_ATTN = (ushort)MapVirtualKey(0xF6, 0);
        public static ushort S_CRSEL = (ushort)MapVirtualKey(0xF7, 0);
        public static ushort S_EXSEL = (ushort)MapVirtualKey(0xF8, 0);
        public static ushort S_EREOF = (ushort)MapVirtualKey(0xF9, 0);
        public static ushort S_PLAY = (ushort)MapVirtualKey(0xFA, 0);
        public static ushort S_ZOOM = (ushort)MapVirtualKey(0xFB, 0);
        public static ushort S_NONAME = (ushort)MapVirtualKey(0xFC, 0);
        public static ushort S_PA1 = (ushort)MapVirtualKey(0xFD, 0);
        public static ushort S_OEM_CLEAR = (ushort)MapVirtualKey(0xFE, 0);
        public double WidthS = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2f;
        public double HeightS = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2f;
        public static double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe, irx2e, iry2e, irx3e, iry3e, irx, iry, irxc, iryc, mWSIRSensorsXcam, mWSIRSensorsYcam, mWSIRSensors0X, mWSIRSensors0Y, mWSIRSensors1X, mWSIRSensors1Y, mWSButtonStateIRX, mWSButtonStateIRY, mWSIRSensors0Xcam, mWSIRSensors1Xcam, mWSIRSensors0Ycam, mWSIRSensors1Ycam, MyAngle, mWSIR0notfound = 0, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, calibrationinit, stickviewxinit, stickviewyinit, mWSNunchuckStateRawValuesX, mWSNunchuckStateRawValuesY, mWSNunchuckStateRawValuesZ, mWSNunchuckStateRawJoystickX, mWSNunchuckStateRawJoystickY, mousex, mousey, mousexp, mouseyp, center = 100f, keys123, keys456, readingfilecount, mWSIRSensorsX, mWSIRSensorsY, irx2, iry2, irx3, iry3;
        public static bool mWSIR1foundcam, mWSIR0foundcam, mWSIR1found, mWSIR0found, mWSIRswitch, mWSButtonStateA, mWSButtonStateB, mWSButtonStateMinus, mWSButtonStateHome, mWSButtonStatePlus, mWSButtonStateOne, mWSButtonStateTwo, mWSButtonStateUp, mWSButtonStateDown, mWSButtonStateLeft, mWSButtonStateRight, Getstate, mWSNunchuckStateC, mWSNunchuckStateZ, readingfile;
        public static byte[] buff = new byte[] { 0x55 }, mBuff = new byte[22], aBuffer = new byte[22];
        public static byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        public static Guid guid = new Guid();
        public static uint hDevInfo, CurrentResolution = 0;
        private static FileStream mStream;
        private static string path;
        public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        static void valchanged(int n, bool val)
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
            Task.Run(() => Start());
        }
        public void Start()
        {
            do
                Thread.Sleep(1);
            while (!connect());
            do
                Thread.Sleep(1);
            while (!ScanWiimote());
            Task.Run(() => taskD());
            Thread.Sleep(1000);
            calibrationinit = -aBuffer[4] + 135f;
            stickviewxinit = -aBuffer[16] + 125f;
            stickviewyinit = -aBuffer[17] + 125f;
            Task.Run(() => taskM());
            Task.Run(() => taskK());
            this.WindowState = FormWindowState.Minimized;
        }
        public void taskM()
        {
            for (; ; )
            {
                if (Getstate)
                {
                    mWSIR0found = (aBuffer[6] | ((aBuffer[8] >> 4) & 0x03) << 8) > 1 & (aBuffer[6] | ((aBuffer[8] >> 4) & 0x03) << 8) < 1023;
                    mWSIR1found = (aBuffer[9] | ((aBuffer[8] >> 0) & 0x03) << 8) > 1 & (aBuffer[9] | ((aBuffer[8] >> 0) & 0x03) << 8) < 1023;
                    if (mWSIR0notfound == 0 & mWSIR1found)
                        mWSIR0notfound = 1;
                    if (mWSIR0notfound == 1 & !mWSIR0found & !mWSIR1found)
                        mWSIR0notfound = 2;
                    if (mWSIR0notfound == 2 & mWSIR0found)
                    {
                        mWSIR0notfound = 0;
                        if (!mWSIRswitch)
                            mWSIRswitch = true;
                        else
                            mWSIRswitch = false;
                    }
                    if (mWSIR0notfound == 0 & mWSIR0found)
                        mWSIR0notfound = 0;
                    if (mWSIR0notfound == 0 & !mWSIR0found & !mWSIR1found)
                        mWSIR0notfound = 0;
                    if (mWSIR0notfound == 1 & mWSIR0found)
                        mWSIR0notfound = 0;
                    if (mWSIR0found)
                    {
                        mWSIRSensors0X = (aBuffer[6] | ((aBuffer[8] >> 4) & 0x03) << 8);
                        mWSIRSensors0Y = (aBuffer[7] | ((aBuffer[8] >> 6) & 0x03) << 8);
                    }
                    if (mWSIR1found)
                    {
                        mWSIRSensors1X = (aBuffer[9] | ((aBuffer[8] >> 0) & 0x03) << 8);
                        mWSIRSensors1Y = (aBuffer[10] | ((aBuffer[8] >> 2) & 0x03) << 8);
                    }
                    if (mWSIRswitch)
                    {
                        mWSIR0foundcam = mWSIR0found;
                        mWSIR1foundcam = mWSIR1found;
                        mWSIRSensors0Xcam = mWSIRSensors0X - 512f;
                        mWSIRSensors0Ycam = mWSIRSensors0Y - 384f;
                        mWSIRSensors1Xcam = mWSIRSensors1X - 512f;
                        mWSIRSensors1Ycam = mWSIRSensors1Y - 384f;
                    }
                    else
                    {
                        mWSIR1foundcam = mWSIR0found;
                        mWSIR0foundcam = mWSIR1found;
                        mWSIRSensors1Xcam = mWSIRSensors0X - 512f;
                        mWSIRSensors1Ycam = mWSIRSensors0Y - 384f;
                        mWSIRSensors0Xcam = mWSIRSensors1X - 512f;
                        mWSIRSensors0Ycam = mWSIRSensors1Y - 384f;
                    }
                    if (mWSIR0foundcam & mWSIR1foundcam)
                    {
                        irx2 = mWSIRSensors0Xcam;
                        iry2 = mWSIRSensors0Ycam;
                        irx3 = mWSIRSensors1Xcam;
                        iry3 = mWSIRSensors1Ycam;
                        mWSIRSensorsXcam = mWSIRSensors0Xcam - mWSIRSensors1Xcam;
                        mWSIRSensorsYcam = mWSIRSensors0Ycam - mWSIRSensors1Ycam;
                    }
                    if (mWSIR0foundcam & !mWSIR1foundcam)
                    {
                        irx2 = mWSIRSensors0Xcam;
                        iry2 = mWSIRSensors0Ycam;
                        irx3 = mWSIRSensors0Xcam - mWSIRSensorsXcam;
                        iry3 = mWSIRSensors0Ycam - mWSIRSensorsYcam;
                    }
                    if (mWSIR1foundcam & !mWSIR0foundcam)
                    {
                        irx3 = mWSIRSensors1Xcam;
                        iry3 = mWSIRSensors1Ycam;
                        irx2 = mWSIRSensors1Xcam + mWSIRSensorsXcam;
                        iry2 = mWSIRSensors1Ycam + mWSIRSensorsYcam;
                    }
                    MyAngle = (irx2 - irx3 > 0 ? 1f : -1f) * (iry2 - iry3);
                    if (mWSIR0foundcam & mWSIR1foundcam)
                    {
                        mWSIRSensorsX = (mWSIRSensors0Xcam + mWSIRSensors1Xcam) / 2f;
                        mWSIRSensorsY = (mWSIRSensors0Ycam + mWSIRSensors1Ycam) / 2f;
                    }
                    if (mWSIR0foundcam)
                    {
                        irx2e = 2 * mWSIRSensors0Xcam - mWSIRSensorsX;
                        iry2e = 2 * mWSIRSensors0Ycam - mWSIRSensorsY;
                    }
                    if (mWSIR1foundcam)
                    {
                        irx3e = 2 * mWSIRSensors1Xcam - mWSIRSensorsX;
                        iry3e = 2 * mWSIRSensors1Ycam - mWSIRSensorsY;
                    }
                    irxc = irx2e + irx3e;
                    iryc = iry2e + iry3e;
                    mWSButtonStateIRX = irxc * (1f - (MyAngle > 0 ? MyAngle : -MyAngle) / 5000f) + MyAngle / 5000f * iryc;
                    mWSButtonStateIRY = (iryc * (1f - (MyAngle > 0 ? MyAngle : -MyAngle) / 10000f) - MyAngle / 10000f * irxc) * 2f;
                    irx = mWSButtonStateIRX;
                    iry = (mWSButtonStateIRY + center >= 0 ? Scale(mWSButtonStateIRY + center, 0f, 1200f + center, 0f, 1200f) : Scale(mWSButtonStateIRY + center, -1200f + center, 0f, -1200f, 0f));
                    mousex = (Math.Pow(irx > 0 ? irx : -irx, 3.5f) / Math.Pow(1200f, 2.5f) + 48f) * (irx > 0 ? 1f : -1f);
                    mousey = (Math.Pow(iry > 0 ? iry : -iry, 2f) / Math.Pow(1200f, 1f) + 85f) * (iry > 0 ? 1f : -1f);
                    mousexp += mousex;
                    mouseyp += mousey;
                    MouseMW3((int)(32767.5 - mousex * 2f - mousexp), (int)(mousey * 2f + mouseyp + 32767.5));
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)(WidthS - irx * WidthS / 1024f), (int)(HeightS + iry * HeightS / 1024f));
                }
                Thread.Sleep(1);
            }
        }
        private double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
        public void taskK()
        {
            for (; ; )
            {
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
                mWSRawValuesX = aBuffer[3] - 135f + calibrationinit;
                mWSRawValuesY = aBuffer[4] - 135f + calibrationinit;
                mWSRawValuesZ = aBuffer[5] - 135f + calibrationinit;
                mWSNunchuckStateRawJoystickX = aBuffer[16] - 125f + stickviewxinit;
                mWSNunchuckStateRawJoystickY = aBuffer[17] - 125f + stickviewyinit;
                mWSNunchuckStateRawValuesX = aBuffer[18] - 125f;
                mWSNunchuckStateRawValuesY = aBuffer[19] - 125f;
                mWSNunchuckStateRawValuesZ = aBuffer[20] - 125f;
                mWSNunchuckStateC = (aBuffer[21] & 0x02) == 0;
                mWSNunchuckStateZ = (aBuffer[21] & 0x01) == 0;
                valchanged(0, mWSButtonStateHome & mWSButtonStateTwo);
                if (wd[0] == 1 & !Getstate)
                    Getstate = true;
                else
                {
                    if (wd[0] == 1 & Getstate)
                        Getstate = false;
                }
                if (Getstate)
                {
                    valchanged(5, mWSNunchuckStateRawValuesX >= 41);
                    if (wd[5] == 1)
                        SimulateKeyDown(VK_E, S_E);
                    if (wu[5] == 1)
                        SimulateKeyUp(VK_E, S_E);
                    valchanged(6, mWSNunchuckStateRawValuesX <= -41);
                    if (wd[6] == 1)
                        SimulateKeyDown(VK_Q, S_Q);
                    if (wu[6] == 1)
                        SimulateKeyUp(VK_Q, S_Q);
                    valchanged(2, mWSNunchuckStateZ);
                    if (wd[2] == 1)
                        SimulateKeyDown(VK_LeftShift, S_LeftShift);
                    if (wu[2] == 1)
                        SimulateKeyUp(VK_LeftShift, S_LeftShift);
                    valchanged(3, mWSNunchuckStateZ & mWSNunchuckStateC);
                    if (wd[3] == 1)
                        SimulateKeyDown(VK_LMENU, S_LMENU);
                    if (wu[3] == 1)
                        SimulateKeyUp(VK_LMENU, S_LMENU);
                    valchanged(4, (mWSNunchuckStateRawValuesY > 33f) & !((mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 40f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 40f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 40f));
                    if (wd[4] == 1)
                        SimulateKeyDown(VK_V, S_V);
                    if (wu[4] == 1)
                        SimulateKeyUp(VK_V, S_V);
                    valchanged(10, mWSNunchuckStateC & !mWSNunchuckStateZ);
                    if (wd[10] == 1)
                        SimulateKeyDown(VK_Space, S_Space);
                    if (wu[10] == 1)
                        SimulateKeyUp(VK_Space, S_Space);
                    valchanged(16, mWSNunchuckStateRawJoystickX > 33f);
                    valchanged(17, mWSNunchuckStateRawJoystickX < -33f);
                    valchanged(18, mWSNunchuckStateRawJoystickY > 33f);
                    valchanged(19, mWSNunchuckStateRawJoystickY < -33f);
                    if (wd[16] == 1)
                        SimulateKeyDown(VK_D, S_D);
                    if (wu[16] == 1)
                        SimulateKeyUp(VK_D, S_D);
                    if (wd[17] == 1)
                        SimulateKeyDown(VK_A, S_A);
                    if (wu[17] == 1)
                        SimulateKeyUp(VK_A, S_A);
                    if (wd[18] == 1)
                        SimulateKeyDown(VK_W, S_W);
                    if (wu[18] == 1)
                        SimulateKeyUp(VK_W, S_W);
                    if (wd[19] == 1)
                        SimulateKeyDown(VK_S, S_S);
                    if (wu[19] == 1)
                        SimulateKeyUp(VK_S, S_S);
                    valchanged(13, (mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 40f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 40f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 40f);
                    if (wd[13] == 1)
                        SimulateKeyDown(VK_R, S_R);
                    if (wu[13] == 1)
                        SimulateKeyUp(VK_R, S_R);
                    valchanged(20, mWSButtonStateOne);
                    if (wd[20] == 1)
                        SimulateKeyDown(VK_Tab, S_Tab);
                    if (wu[20] == 1)
                        SimulateKeyUp(VK_Tab, S_Tab);
                    valchanged(21, mWSButtonStateDown);
                    if (wd[21] == 1)
                        SimulateKeyDown(VK_C, S_C);
                    if (wu[21] == 1)
                        SimulateKeyUp(VK_C, S_C);
                    valchanged(22, mWSButtonStateHome);
                    if (wd[22] == 1)
                        SimulateKeyDown(VK_F, S_F);
                    if (wu[22] == 1)
                        SimulateKeyUp(VK_F, S_F);
                    valchanged(23, mWSButtonStateRight);
                    if (wd[23] == 1)
                        SimulateKeyDown(VK_U, S_U);
                    if (wu[23] == 1)
                        SimulateKeyUp(VK_U, S_U);
                    valchanged(24, mWSButtonStateLeft);
                    if (wd[24] == 1)
                        SimulateKeyDown(VK_Y, S_Y);
                    if (wu[24] == 1)
                        SimulateKeyUp(VK_Y, S_Y);
                    valchanged(25, mWSButtonStateUp);
                    if (wd[25] == 1)
                        SimulateKeyDown(VK_X, S_X);
                    if (wu[25] == 1)
                        SimulateKeyUp(VK_X, S_X);
                    valchanged(26, mWSButtonStateTwo);
                    if (wd[26] == 1)
                        SimulateKeyDown(VK_Escape, S_Escape);
                    if (wu[26] == 1)
                        SimulateKeyUp(VK_Escape, S_Escape);
                    valchanged(14, mWSButtonStatePlus);
                    if (wd[14] == 1)
                        SimulateKeyDown(VK_G, S_G);
                    if (wu[14] == 1)
                        SimulateKeyUp(VK_G, S_G);
                    valchanged(15, mWSButtonStateMinus);
                    if (wd[15] == 1)
                        SimulateKeyDown(VK_T, S_T);
                    if (wu[15] == 1)
                        SimulateKeyUp(VK_T, S_T);
                    valchanged(34, mWSButtonStateA);
                    if (wd[34] == 1)
                        RightClick();
                    if (wu[34] == 1)
                        RightClickF();
                    valchanged(11, mWSButtonStateB);
                    if (wd[11] == 1)
                        LeftClick();
                    if (wu[11] == 1)
                        LeftClickF();
                }
                if (readingfilecount == 0)
                    readingfile = false;
                readingfilecount++;
                if (readingfilecount > 100)
                {
                    if (!readingfile)
                        WiimoteFound(path);
                    readingfilecount = 0;
                }
                Thread.Sleep(1);
            }
        }
        public void taskD()
        {
            for (; ; )
            {
                try
                {
                    mStream.Read(aBuffer, 0, 22);
                    readingfile = true;
                }
                catch
                {
                    readingfile = false;
                }
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Thread.Sleep(100);
            TimeEndPeriod(1);
            disconnect();
        }
        private const string vendor_id = "57e", vendor_id_ = "057e", product_r1 = "0330", product_r2 = "0306";
        public enum EFileAttributes : uint
        {
            Overlapped = 0x40000000,
            Normal = 0x80
        };
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr RESERVED;
        }
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
        private bool ScanWiimote()
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
                        path = diDetail.DevicePath;
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
        public static void WiimoteFound(string path)
        {
            SafeFileHandle handle = null;
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
        public static void ReadData(SafeFileHandle _hFile, int address, short size)
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
        public static void WriteData(SafeFileHandle _hFile, byte mbuff, int address, byte[] buff, short size)
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
    }
}