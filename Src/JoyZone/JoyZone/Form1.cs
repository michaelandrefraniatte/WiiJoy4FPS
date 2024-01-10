using Microsoft.Win32.SafeHandles;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Numerics;
namespace JoyZone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("JoyconsPairing.dll", EntryPoint = "connect")]
        public static unsafe extern bool connect();
        [DllImport("JoyconsPairing.dll", EntryPoint = "disconnect")]
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
        public static unsafe extern IntPtr CreateFile(string fileName, System.IO.FileAccess fileAccess, System.IO.FileShare fileShare, IntPtr securityAttributes, System.IO.FileMode creationDisposition, EFileAttributes flags, IntPtr template);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_read_timeout")]
        public static extern int Lhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_write")]
        public static extern int Lhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_open_path")]
        public static extern SafeFileHandle Lhid_open_path(IntPtr handle);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_close")]
        public static unsafe extern void Lhid_close(SafeFileHandle device);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_read_timeout")]
        public static extern int Rhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_write")]
        public static extern int Rhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_open_path")]
        public static extern SafeFileHandle Rhid_open_path(IntPtr handle);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_close")]
        public static unsafe extern void Rhid_close(SafeFileHandle device);
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
        public static double mousex, mousey, irx, iry, zoningx = 100f, zoningy = 100f, maxx = 1600f, maxy = 1200f;
        public static bool Getstate, booliniton, ISLEFT, ISRIGHT, foraorcison, LeftButtonSHOULDER_2io, randA, ApressIO = false, HomeFTG = false;
        public static Guid guid = new Guid();
        public static uint hDevInfo, CurrentResolution = 0;
        public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
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
            while (!ScanLeft());
            do
                Thread.Sleep(1);
            while (!ScanRight());
            Task.Run(() => taskM());
            Task.Run(() => taskK());
            Task.Run(() => taskDLeft());
            Task.Run(() => taskDRight());
            Thread.Sleep(2000);
            stick_rawLeft[0] = report_bufLeft[6 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[1] = report_bufLeft[7 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[2] = report_bufLeft[8 + (ISLEFT ? 0 : 3)];
            stick_calibrationLeft[0] = (UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8));
            stick_calibrationLeft[1] = (UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4));
            acc_gcalibrationLeftX = (int)(avg((Int16)(report_bufLeft[13 + 0 * 12] | ((report_bufLeft[14 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[13 + 1 * 12] | ((report_bufLeft[14 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[13 + 2 * 12] | ((report_bufLeft[14 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f);
            acc_gcalibrationLeftY = (int)(avg((Int16)(report_bufLeft[15 + 0 * 12] | ((report_bufLeft[16 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[15 + 1 * 12] | ((report_bufLeft[16 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[15 + 2 * 12] | ((report_bufLeft[16 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f);
            acc_gcalibrationLeftZ = (int)(avg((Int16)(report_bufLeft[17 + 0 * 12] | ((report_bufLeft[18 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[17 + 1 * 12] | ((report_bufLeft[18 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[17 + 2 * 12] | ((report_bufLeft[18 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f);
            gyr_gcalibrationLeftX = (int)(avg((int)((Int16)((report_bufLeft[19 + 0 * 12] | ((report_bufLeft[20 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[19 + 1 * 12] | ((report_bufLeft[20 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[19 + 2 * 12] | ((report_bufLeft[20 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f);
            gyr_gcalibrationLeftY = (int)(avg((int)((Int16)((report_bufLeft[21 + 0 * 12] | ((report_bufLeft[22 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[21 + 1 * 12] | ((report_bufLeft[22 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[21 + 2 * 12] | ((report_bufLeft[22 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f);
            gyr_gcalibrationLeftZ = (int)(avg((int)((Int16)((report_bufLeft[23 + 0 * 12] | ((report_bufLeft[24 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[23 + 1 * 12] | ((report_bufLeft[24 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[23 + 2 * 12] | ((report_bufLeft[24 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f);
            stick_rawRight[0] = report_bufRight[6 + (!ISRIGHT ? 0 : 3)];
            stick_rawRight[1] = report_bufRight[7 + (!ISRIGHT ? 0 : 3)];
            stick_rawRight[2] = report_bufRight[8 + (!ISRIGHT ? 0 : 3)];
            stick_calibrationRight[0] = (UInt16)(stick_rawRight[0] | ((stick_rawRight[1] & 0xf) << 8));
            stick_calibrationRight[1] = (UInt16)((stick_rawRight[1] >> 4) | (stick_rawRight[2] << 4));
            acc_gcalibrationRightX = (int)(avg((Int16)(report_bufRight[13 + 0 * 12] | ((report_bufRight[14 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[13 + 1 * 12] | ((report_bufRight[14 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[13 + 2 * 12] | ((report_bufRight[14 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f);
            acc_gcalibrationRightY = -(int)(avg((Int16)(report_bufRight[15 + 0 * 12] | ((report_bufRight[16 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[15 + 1 * 12] | ((report_bufRight[16 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[15 + 2 * 12] | ((report_bufRight[16 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f);
            acc_gcalibrationRightZ = -(int)(avg((Int16)(report_bufRight[17 + 0 * 12] | ((report_bufRight[18 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[17 + 1 * 12] | ((report_bufRight[18 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[17 + 2 * 12] | ((report_bufRight[18 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f);
            gyr_gcalibrationRightX = (int)(avg((int)((Int16)((report_bufRight[19 + 0 * 12] | ((report_bufRight[20 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[19 + 1 * 12] | ((report_bufRight[20 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[19 + 2 * 12] | ((report_bufRight[20 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f);
            gyr_gcalibrationRightY = -(int)(avg((int)((Int16)((report_bufRight[21 + 0 * 12] | ((report_bufRight[22 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[21 + 1 * 12] | ((report_bufRight[22 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[21 + 2 * 12] | ((report_bufRight[22 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f);
            gyr_gcalibrationRightZ = -(int)(avg((int)((Int16)((report_bufRight[23 + 0 * 12] | ((report_bufRight[24 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[23 + 1 * 12] | ((report_bufRight[24 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[23 + 2 * 12] | ((report_bufRight[24 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f);
            booliniton = true;
            this.WindowState = FormWindowState.Minimized;
        }
        public void taskM()
        {
            for (; ; )
            {
                if (Getstate)
                {
                    irx = -EulerAnglesRight.X * 40000f;
                    iry = -EulerAnglesRight.Z * 40000f;
                    mousex = (Math.Pow(irx > 0 ? irx : -irx, 3.33f) / Math.Pow(maxx, 2.33f) + 1.49) * (irx > 0 ? 1f : -1f);
                    mousey = (Math.Pow(iry > 0 ? iry : -iry, 3.33f) / Math.Pow(maxy, 2.33f) + 0.99) * (iry > 0 ? 1f : -1f);
                    MouseMW3((int)(32767.5 - mousex * 32f), (int)(mousey * 32f + 32767.5));
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                    SetPhysicalCursorPos((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                    SetCaretPos((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                    SetCursorPos((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                }
                Thread.Sleep(1);
            }
        }
        public void taskK()
        {
            for (; ; )
            {
                valchanged(0, LeftButtonCAPTURE & RightButtonHOME);
                if (wd[0] == 1 & !Getstate)
                    Getstate = true;
                else
                {
                    if (wd[0] == 1 & Getstate)
                        Getstate = false;
                }
                if (Getstate)
                {
                    valchanged(5, LeftButtonDPAD_UP);
                    if (wd[5] == 1)
                        SimulateKeyDown(VK_7, S_7);
                    if (wu[5] == 1)
                        SimulateKeyUp(VK_7, S_7);
                    valchanged(6, LeftButtonDPAD_LEFT);
                    if (wd[6] == 1)
                        SimulateKeyDown(VK_8, S_8);
                    if (wu[6] == 1)
                        SimulateKeyUp(VK_8, S_8);
                    valchanged(7, LeftButtonDPAD_DOWN);
                    if (wd[7] == 1)
                        SimulateKeyDown(VK_9, S_9);
                    if (wu[7] == 1)
                        SimulateKeyUp(VK_9, S_9);
                    valchanged(8, LeftButtonDPAD_RIGHT);
                    if (wd[8] == 1)
                        SimulateKeyDown(VK_0, S_0);
                    if (wu[8] == 1)
                        SimulateKeyUp(VK_0, S_0);
                    valchanged(10, RightButtonDPAD_DOWN);
                    if (wd[10] == 1)
                        SimulateKeyDown(VK_C, S_C);
                    if (wu[10] == 1)
                        SimulateKeyUp(VK_C, S_C);
                    valchanged(21, RightButtonDPAD_RIGHT);
                    if (wd[21] == 1)
                        SimulateKeyDown(VK_2, S_2);
                    if (wu[21] == 1)
                        SimulateKeyUp(VK_2, S_2);
                    valchanged(24, RightButtonDPAD_LEFT);
                    if (wd[24] == 1)
                        SimulateKeyDown(VK_1, S_1);
                    if (wu[24] == 1)
                        SimulateKeyUp(VK_1, S_1);
                    valchanged(25, RightButtonDPAD_UP);
                    if (wd[25] == 1)
                        SimulateKeyDown(VK_X, S_X);
                    if (wu[25] == 1)
                        SimulateKeyUp(VK_X, S_X);
                    valchanged(3, LeftButtonMINUS | (HomeFTG & RightButtonHOME));
                    if (wd[3] == 1)
                        SimulateKeyDown(VK_T, S_T);
                    if (wu[3] == 1)
                        SimulateKeyUp(VK_T, S_T);
                    valchanged(20, RightButtonPLUS | (HomeFTG & RightButtonHOME));
                    if (wd[20] == 1)
                        SimulateKeyDown(VK_G, S_G);
                    if (wu[20] == 1)
                        SimulateKeyUp(VK_G, S_G);
                    valchanged(27, LeftButtonCAPTURE);
                    if (wd[27] == 1)
                        SimulateKeyDown(VK_LMENU, S_LMENU);
                    if (wu[27] == 1)
                        SimulateKeyUp(VK_LMENU, S_LMENU);
                    valchanged(22, RightButtonHOME);
                    if (wd[22] == 1)
                        SimulateKeyDown(VK_F, S_F);
                    if (wu[22] == 1)
                        SimulateKeyUp(VK_F, S_F);
                    valchanged(9, LeftButtonSTICK);
                    if (wd[9] == 1)
                        SimulateKeyDown(VK_LeftShift, S_LeftShift);
                    if (wu[9] == 1)
                        SimulateKeyUp(VK_LeftShift, S_LeftShift);
                    valchanged(26, RightButtonSTICK);
                    if (wd[26] == 1)
                        SimulateKeyDown(VK_V, S_V);
                    if (wu[26] == 1)
                        SimulateKeyUp(VK_V, S_V);
                    valchanged(29, LeftButtonSL);
                    if (wd[29] == 1)
                        SimulateKeyDown(VK_Q, S_Q);
                    if (wu[29] == 1)
                        SimulateKeyUp(VK_Q, S_Q);
                    valchanged(28, LeftButtonSR);
                    if (wd[28] == 1)
                        SimulateKeyDown(VK_Escape, S_Escape);
                    if (wu[28] == 1)
                        SimulateKeyUp(VK_Escape, S_Escape);
                    valchanged(14, RightButtonSL);
                    if (wd[14] == 1)
                        SimulateKeyDown(VK_Tab, S_Tab);
                    if (wu[14] == 1)
                        SimulateKeyUp(VK_Tab, S_Tab);
                    valchanged(15, RightButtonSR);
                    if (wd[15] == 1)
                        SimulateKeyDown(VK_E, S_E);
                    if (wu[15] == 1)
                        SimulateKeyUp(VK_E, S_E);
                    valchanged(23, RightButtonSHOULDER_1);
                    if (wd[23] == 1)
                        SimulateKeyDown(VK_V, S_V);
                    if (wu[23] == 1)
                        SimulateKeyUp(VK_V, S_V);
                    valchanged(2, LeftButtonSHOULDER_1);
                    if (wd[2] == 1)
                        SimulateKeyDown(VK_Space, S_Space);
                    if (wu[2] == 1)
                        SimulateKeyUp(VK_Space, S_Space);
                    valchanged(11, RightButtonSHOULDER_2);
                    if (wd[11] == 1)
                        LeftClick();
                    if (wu[11] == 1)
                        LeftClickF();
                    if (ApressIO)
                    {
                        foraorcison = (LeftButtonMINUS | RightButtonPLUS | RightButtonHOME | (GetStickLeft()[1] > 0.33f & LeftButtonSTICK) | LeftButtonDPAD_UP | LeftButtonDPAD_DOWN | LeftButtonDPAD_LEFT | LeftButtonDPAD_RIGHT);
                        valchanged(32, LeftButtonSHOULDER_2);
                        if (wd[32] == 1)
                            if (!randA)
                            {
                                LeftButtonSHOULDER_2io = true;
                                randA = true;
                            }
                            else
                                if (randA)
                            {
                                LeftButtonSHOULDER_2io = false;
                                randA = false;
                            }
                        if (LeftButtonSHOULDER_2io & foraorcison)
                        {
                            LeftButtonSHOULDER_2io = false;
                            randA = false;
                        }
                        valchanged(33, LeftButtonSHOULDER_2io | (LeftButtonSHOULDER_2 & foraorcison));
                        if (wd[33] == 1)
                            RightClick();
                        if (wu[33] == 1)
                            RightClickF();
                    }
                    else
                    {
                        valchanged(34, LeftButtonSHOULDER_2);
                        if (wd[34] == 1)
                            RightClick();
                        if (wu[34] == 1)
                            RightClickF();
                    }
                    valchanged(16, GetStickLeft()[0] > 0.25f);
                    valchanged(17, GetStickLeft()[0] < -0.25f);
                    valchanged(18, GetStickLeft()[1] > 0.25f);
                    valchanged(19, GetStickLeft()[1] < -0.25f);
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
                    valchanged(35, GetStickRight()[0] > 0.25f);
                    valchanged(36, GetStickRight()[0] < -0.25f);
                    valchanged(37, GetStickRight()[1] > 0.25f);
                    valchanged(38, GetStickRight()[1] < -0.25f);
                    if (wd[35] == 1)
                        SimulateKeyDown(VK_5, S_5);
                    if (wu[35] == 1)
                        SimulateKeyUp(VK_5, S_5);
                    if (wd[36] == 1)
                        SimulateKeyDown(VK_6, S_6);
                    if (wu[36] == 1)
                        SimulateKeyUp(VK_6, S_6);
                    if (wd[37] == 1)
                        SimulateKeyDown(VK_3, S_3);
                    if (wu[37] == 1)
                        SimulateKeyUp(VK_3, S_3);
                    if (wd[38] == 1)
                        SimulateKeyDown(VK_4, S_4);
                    if (wu[38] == 1)
                        SimulateKeyUp(VK_4, S_4);
                }
                Thread.Sleep(1);
            }
        }
        public void taskDLeft()
        {
            for (; ; )
            {
                Lhid_read_timeout(handleLeft, report_bufLeft, (UIntPtr)49);
                if (booliniton)
                    DataLeft();
            }
        }
        public void taskDRight()
        {
            for (; ; )
            {
                Rhid_read_timeout(handleRight, report_bufRight, (UIntPtr)49);
                if (booliniton)
                    DataRight();
            }
        }
        private double avg(double val1, double val2, double val3)
        {
            return (new double[] { val1, val2, val3 }).Average();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            booliniton = false;
            Thread.Sleep(100);
            TimeEndPeriod(1);
            disconnect();
        }
        public void DataLeft()
        {
            ProcessButtonsAndStickLeft();
            ExtractIMUValuesLeft();
        }
        public Quaternion GetVectoraLeft()
        {
            Vector3 v1 = new Vector3(j_aLeft.X, i_aLeft.X, k_aLeft.X);
            Vector3 v2 = -(new Vector3(j_aLeft.Z, i_aLeft.Z, k_aLeft.Z));
            return QuaternionLookRotationLeft(v1, v2);
        }
        public Quaternion GetVectorbLeft()
        {
            Vector3 v1 = new Vector3(j_bLeft.X, i_bLeft.X, k_bLeft.X);
            Vector3 v2 = -(new Vector3(j_bLeft.Z, i_bLeft.Z, k_bLeft.Z));
            return QuaternionLookRotationLeft(v1, v2);
        }
        public Quaternion GetVectorcLeft()
        {
            Vector3 v1 = new Vector3(j_cLeft.X, i_cLeft.X, k_cLeft.X);
            Vector3 v2 = -(new Vector3(j_cLeft.Z, i_cLeft.Z, k_cLeft.Z));
            return QuaternionLookRotationLeft(v1, v2);
        }
        private static Quaternion QuaternionLookRotationLeft(Vector3 forward, Vector3 up)
        {
            Vector3 vector = Vector3.Normalize(forward);
            Vector3 vector2 = Vector3.Normalize(Vector3.Cross(up, vector));
            Vector3 vector3 = Vector3.Cross(vector, vector2);
            var m00 = vector2.X;
            var m01 = vector2.Y;
            var m02 = vector2.Z;
            var m10 = vector3.X;
            var m11 = vector3.Y;
            var m12 = vector3.Z;
            var m20 = vector.X;
            var m21 = vector.Y;
            var m22 = vector.Z;
            double num8 = (m00 + m11) + m22;
            var quaternion = new Quaternion();
            if (num8 > 0f)
            {
                var num = (double)Math.Sqrt(num8 + 1f);
                quaternion.W = (float)num * 0.5f;
                num = 0.5f / num;
                quaternion.X = (float)(m12 - m21) * (float)num;
                quaternion.Y = (float)(m20 - m02) * (float)num;
                quaternion.Z = (float)(m01 - m10) * (float)num;
                return quaternion;
            }
            if ((m00 >= m11) && (m00 >= m22))
            {
                var num7 = (double)Math.Sqrt(((1f + m00) - m11) - m22);
                var num4 = 0.5f / num7;
                quaternion.X = 0.5f * (float)num7;
                quaternion.Y = (float)(m01 + m10) * (float)num4;
                quaternion.Z = (float)(m02 + m20) * (float)num4;
                quaternion.W = (float)(m12 - m21) * (float)num4;
                return quaternion;
            }
            if (m11 > m22)
            {
                var num6 = (double)Math.Sqrt(((1f + m11) - m00) - m22);
                var num3 = 0.5f / num6;
                quaternion.X = (float)(m10 + m01) * (float)num3;
                quaternion.Y = 0.5f * (float)num6;
                quaternion.Z = (float)(m21 + m12) * (float)num3;
                quaternion.W = (float)(m20 - m02) * (float)num3;
                return quaternion;
            }
            var num5 = (double)Math.Sqrt(((1f + m22) - m00) - m11);
            var num2 = 0.5f / num5;
            quaternion.X = (float)(m20 + m02) * (float)num2;
            quaternion.Y = (float)(m21 + m12) * (float)num2;
            quaternion.Z = 0.5f * (float)num5;
            quaternion.W = (float)(m01 - m10) * (float)num2;
            return quaternion;
        }
        public static Vector3 ToEulerAnglesLeft(Quaternion q)
        {
            Vector3 pitchYawRoll = new Vector3();
            double sqw = q.W * q.W;
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;
            double unit = sqx + sqy + sqz + sqw;
            double test = q.X * q.Y + q.Z * q.W;
            if (test > 0.4999f * unit)                              // 0.4999f OR 0.5f - EPSILON
            {
                pitchYawRoll.Y = 2f * (float)Math.Atan2(q.X, q.W);  // Yaw
                pitchYawRoll.X = (float)Math.PI * 0.5f;                         // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else if (test < -0.4999f * unit)                        // -0.4999f OR -0.5f + EPSILON
            {
                pitchYawRoll.Y = -2f * (float)Math.Atan2(q.X, q.W); // Yaw
                pitchYawRoll.X = -(float)Math.PI * 0.5f;                        // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else
            {
                pitchYawRoll.Y = (float)Math.Atan2(2f * q.Y * q.W - 2f * q.X * q.Z, sqx - sqy - sqz + sqw);       // Yaw
                pitchYawRoll.X = (float)Math.Asin(2f * test / unit);                                             // Pitch
                pitchYawRoll.Z = (float)Math.Atan2(2f * q.X * q.W - 2f * q.Y * q.Z, -sqx + sqy - sqz + sqw);      // Roll
            }
            return pitchYawRoll;
        }
        private void ProcessButtonsAndStickLeft()
        {
            LeftButtonSHOULDER_1 = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x40) != 0;
            LeftButtonSHOULDER_2 = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x80) != 0;
            LeftButtonSR = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x10) != 0;
            LeftButtonSL = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x20) != 0;
            LeftButtonDPAD_DOWN = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x01 : 0x04)) != 0;
            LeftButtonDPAD_RIGHT = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x04 : 0x08)) != 0;
            LeftButtonDPAD_UP = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x02 : 0x02)) != 0;
            LeftButtonDPAD_LEFT = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x08 : 0x01)) != 0;
            LeftButtonMINUS = ((report_bufLeft[4] & 0x01) != 0);
            LeftButtonCAPTURE = ((report_bufLeft[4] & 0x20) != 0);
            LeftButtonSTICK = ((report_bufLeft[4] & (ISLEFT ? 0x08 : 0x04)) != 0);
            stick_rawLeft[0] = report_bufLeft[6 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[1] = report_bufLeft[7 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[2] = report_bufLeft[8 + (ISLEFT ? 0 : 3)];
            stick_precalLeft[0] = (UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8));
            stick_precalLeft[1] = (UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4));
            stickLeft = CenterSticksLeft(stick_precalLeft);
        }
        private void ExtractIMUValuesLeft()
        {
            acc_gLeft.X = (int)(averageLeft((Int16)(report_bufLeft[13 + 0 * 12] | ((report_bufLeft[14 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[13 + 1 * 12] | ((report_bufLeft[14 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[13 + 2 * 12] | ((report_bufLeft[14 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f) - acc_gcalibrationLeftX;
            acc_gLeft.Y = (int)(averageLeft((Int16)(report_bufLeft[15 + 0 * 12] | ((report_bufLeft[16 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[15 + 1 * 12] | ((report_bufLeft[16 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[15 + 2 * 12] | ((report_bufLeft[16 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f) - acc_gcalibrationLeftY;
            acc_gLeft.Z = (int)(averageLeft((Int16)(report_bufLeft[17 + 0 * 12] | ((report_bufLeft[18 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[17 + 1 * 12] | ((report_bufLeft[18 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufLeft[17 + 2 * 12] | ((report_bufLeft[18 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f) - acc_gcalibrationLeftZ;
            gyr_gLeft.X = (int)(averageLeft((int)((Int16)((report_bufLeft[19 + 0 * 12] | ((report_bufLeft[20 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[19 + 1 * 12] | ((report_bufLeft[20 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[19 + 2 * 12] | ((report_bufLeft[20 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f) - gyr_gcalibrationLeftX;
            gyr_gLeft.Y = (int)(averageLeft((int)((Int16)((report_bufLeft[21 + 0 * 12] | ((report_bufLeft[22 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[21 + 1 * 12] | ((report_bufLeft[22 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[21 + 2 * 12] | ((report_bufLeft[22 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f) - gyr_gcalibrationLeftY;
            gyr_gLeft.Z = (int)(averageLeft((int)((Int16)((report_bufLeft[23 + 0 * 12] | ((report_bufLeft[24 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[23 + 1 * 12] | ((report_bufLeft[24 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufLeft[23 + 2 * 12] | ((report_bufLeft[24 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f) - gyr_gcalibrationLeftZ;
            if (!Getstate)
                InitDirectAnglesLeft = acc_gLeft;
            DirectAnglesLeft = acc_gLeft - InitDirectAnglesLeft;
            i_cLeft = new Vector3(1, 0, 0);
            k_cLeft = new Vector3(0, 0, 1);
            j_cLeft.X = 0f;
            j_cLeft.Y = 1f;
            i_bLeft = new Vector3(1, 0, 0);
            k_bLeft = new Vector3(0, 0, 1);
            j_bLeft.Y = 1f;
            j_bLeft.Z = 0f;
            i_aLeft = new Vector3(1, 0, 0);
            j_aLeft = new Vector3(0, 1, 0);
            k_aLeft.Y = 0f;
            k_aLeft.Z = 1f;
            if (EulerAnglescLeft.Y == 0f)
                j_cLeft = new Vector3(0, 1, 0);
            if (EulerAnglesbLeft.X == 0f)
                j_bLeft = new Vector3(0, 1, 0);
            if (EulerAnglesaLeft.Z == 0f)
                k_aLeft = new Vector3(0, 0, 1);
            if (!Getstate | LeftButtonCAPTURE | RightButtonHOME)
            {
                j_cLeft = new Vector3(0, 1, 0);
                InitEulerAnglescLeft = ToEulerAnglesLeft(GetVectorcLeft());
                j_bLeft = new Vector3(0, 1, 0);
                InitEulerAnglesbLeft = ToEulerAnglesLeft(GetVectorbLeft());
                k_aLeft = new Vector3(0, 0, 1);
                InitEulerAnglesaLeft = ToEulerAnglesLeft(GetVectoraLeft());
            }
            j_cLeft += Vector3.Cross(Vector3.Negate(gyr_gLeft), j_cLeft);
            j_cLeft = Vector3.Normalize(j_cLeft);
            EulerAnglescLeft = ToEulerAnglesLeft(GetVectorcLeft()) - InitEulerAnglescLeft;
            j_bLeft += Vector3.Cross(Vector3.Negate(gyr_gLeft), j_bLeft);
            j_bLeft = Vector3.Normalize(j_bLeft);
            EulerAnglesbLeft = ToEulerAnglesLeft(GetVectorbLeft()) - InitEulerAnglesbLeft;
            k_aLeft += Vector3.Cross(Vector3.Negate(gyr_gLeft), k_aLeft);
            k_aLeft = Vector3.Normalize(k_aLeft);
            EulerAnglesaLeft = ToEulerAnglesLeft(GetVectoraLeft()) - InitEulerAnglesaLeft;
            EulerAnglesLeft = new Vector3(EulerAnglesbLeft.X, EulerAnglescLeft.Y, EulerAnglesaLeft.Z);
        }
        private double averageLeft(double val1, double val2, double val3)
        {
            arrayLeft = new double[] { val1, val2, val3 };
            return arrayLeft.Average();
        }
        private double[] CenterSticksLeft(UInt16[] vals)
        {
            double[] s = { 0, 0 };
            s[0] = ((int)((vals[0] - stick_calibrationLeft[0]) / 100f)) / 13f;
            s[1] = ((int)((vals[1] - stick_calibrationLeft[1]) / 100f)) / 13f;
            return s;
        }
        public void DataRight()
        {
            ProcessButtonsAndStickRight();
            ExtractIMUValuesRight();
        }
        public Quaternion GetVectoraRight()
        {
            Vector3 v1 = new Vector3(j_aRight.X, i_aRight.X, k_aRight.X);
            Vector3 v2 = -(new Vector3(j_aRight.Z, i_aRight.Z, k_aRight.Z));
            return QuaternionLookRotationRight(v1, v2);
        }
        public Quaternion GetVectorbRight()
        {
            Vector3 v1 = new Vector3(j_bRight.X, i_bRight.X, k_bRight.X);
            Vector3 v2 = -(new Vector3(j_bRight.Z, i_bRight.Z, k_bRight.Z));
            return QuaternionLookRotationRight(v1, v2);
        }
        public Quaternion GetVectorcRight()
        {
            Vector3 v1 = new Vector3(j_cRight.X, i_cRight.X, k_cRight.X);
            Vector3 v2 = -(new Vector3(j_cRight.Z, i_cRight.Z, k_cRight.Z));
            return QuaternionLookRotationRight(v1, v2);
        }
        private static Quaternion QuaternionLookRotationRight(Vector3 forward, Vector3 up)
        {
            Vector3 vector = Vector3.Normalize(forward);
            Vector3 vector2 = Vector3.Normalize(Vector3.Cross(up, vector));
            Vector3 vector3 = Vector3.Cross(vector, vector2);
            var m00 = vector2.X;
            var m01 = vector2.Y;
            var m02 = vector2.Z;
            var m10 = vector3.X;
            var m11 = vector3.Y;
            var m12 = vector3.Z;
            var m20 = vector.X;
            var m21 = vector.Y;
            var m22 = vector.Z;
            double num8 = (m00 + m11) + m22;
            var quaternion = new Quaternion();
            if (num8 > 0f)
            {
                var num = (double)Math.Sqrt(num8 + 1f);
                quaternion.W = (float)num * 0.5f;
                num = 0.5f / num;
                quaternion.X = (float)(m12 - m21) * (float)num;
                quaternion.Y = (float)(m20 - m02) * (float)num;
                quaternion.Z = (float)(m01 - m10) * (float)num;
                return quaternion;
            }
            if ((m00 >= m11) && (m00 >= m22))
            {
                var num7 = (double)Math.Sqrt(((1f + m00) - m11) - m22);
                var num4 = 0.5f / num7;
                quaternion.X = 0.5f * (float)num7;
                quaternion.Y = (float)(m01 + m10) * (float)num4;
                quaternion.Z = (float)(m02 + m20) * (float)num4;
                quaternion.W = (float)(m12 - m21) * (float)num4;
                return quaternion;
            }
            if (m11 > m22)
            {
                var num6 = (double)Math.Sqrt(((1f + m11) - m00) - m22);
                var num3 = 0.5f / num6;
                quaternion.X = (float)(m10 + m01) * (float)num3;
                quaternion.Y = 0.5f * (float)num6;
                quaternion.Z = (float)(m21 + m12) * (float)num3;
                quaternion.W = (float)(m20 - m02) * (float)num3;
                return quaternion;
            }
            var num5 = (double)Math.Sqrt(((1f + m22) - m00) - m11);
            var num2 = 0.5f / num5;
            quaternion.X = (float)(m20 + m02) * (float)num2;
            quaternion.Y = (float)(m21 + m12) * (float)num2;
            quaternion.Z = 0.5f * (float)num5;
            quaternion.W = (float)(m01 - m10) * (float)num2;
            return quaternion;
        }
        public static Vector3 ToEulerAnglesRight(Quaternion q)
        {
            Vector3 pitchYawRoll = new Vector3();
            double sqw = q.W * q.W;
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;
            double unit = sqx + sqy + sqz + sqw;
            double test = q.X * q.Y + q.Z * q.W;
            if (test > 0.4999f * unit)                              // 0.4999f OR 0.5f - EPSILON
            {
                pitchYawRoll.Y = 2f * (float)Math.Atan2(q.X, q.W);  // Yaw
                pitchYawRoll.X = (float)Math.PI * 0.5f;                         // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else if (test < -0.4999f * unit)                        // -0.4999f OR -0.5f + EPSILON
            {
                pitchYawRoll.Y = -2f * (float)Math.Atan2(q.X, q.W); // Yaw
                pitchYawRoll.X = -(float)Math.PI * 0.5f;                        // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else
            {
                pitchYawRoll.Y = (float)Math.Atan2(2f * q.Y * q.W - 2f * q.X * q.Z, sqx - sqy - sqz + sqw);       // Yaw
                pitchYawRoll.X = (float)Math.Asin(2f * test / unit);                                             // Pitch
                pitchYawRoll.Z = (float)Math.Atan2(2f * q.X * q.W - 2f * q.Y * q.Z, -sqx + sqy - sqz + sqw);      // Roll
            }
            return pitchYawRoll;
        }
        public void ProcessButtonsAndStickRight()
        {
            RightButtonSHOULDER_1 = (report_bufRight[3 + (!ISRIGHT ? 2 : 0)] & 0x40) != 0;
            RightButtonSHOULDER_2 = (report_bufRight[3 + (!ISRIGHT ? 2 : 0)] & 0x80) != 0;
            RightButtonSR = (report_bufRight[3 + (!ISRIGHT ? 2 : 0)] & 0x10) != 0;
            RightButtonSL = (report_bufRight[3 + (!ISRIGHT ? 2 : 0)] & 0x20) != 0;
            RightButtonDPAD_DOWN = (report_bufRight[3 + (!ISRIGHT ? 2 : 0)] & (!ISRIGHT ? 0x01 : 0x04)) != 0;
            RightButtonDPAD_RIGHT = (report_bufRight[3 + (!ISRIGHT ? 2 : 0)] & (!ISRIGHT ? 0x04 : 0x08)) != 0;
            RightButtonDPAD_UP = (report_bufRight[3 + (!ISRIGHT ? 2 : 0)] & (!ISRIGHT ? 0x02 : 0x02)) != 0;
            RightButtonDPAD_LEFT = (report_bufRight[3 + (!ISRIGHT ? 2 : 0)] & (!ISRIGHT ? 0x08 : 0x01)) != 0;
            RightButtonPLUS = ((report_bufRight[4] & 0x02) != 0);
            RightButtonHOME = ((report_bufRight[4] & 0x10) != 0);
            RightButtonSTICK = ((report_bufRight[4] & (!ISRIGHT ? 0x08 : 0x04)) != 0);
            stick_rawRight[0] = report_bufRight[6 + (!ISRIGHT ? 0 : 3)];
            stick_rawRight[1] = report_bufRight[7 + (!ISRIGHT ? 0 : 3)];
            stick_rawRight[2] = report_bufRight[8 + (!ISRIGHT ? 0 : 3)];
            stick_precalRight[0] = (UInt16)(stick_rawRight[0] | ((stick_rawRight[1] & 0xf) << 8));
            stick_precalRight[1] = (UInt16)((stick_rawRight[1] >> 4) | (stick_rawRight[2] << 4));
            stickRight = CenterSticksRight(stick_precalRight);
        }
        private void ExtractIMUValuesRight()
        {
            acc_gRight.X = (int)(averageRight((Int16)(report_bufRight[13 + 0 * 12] | ((report_bufRight[14 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[13 + 1 * 12] | ((report_bufRight[14 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[13 + 2 * 12] | ((report_bufRight[14 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f) - acc_gcalibrationRightX;
            acc_gRight.Y = -(int)(averageRight((Int16)(report_bufRight[15 + 0 * 12] | ((report_bufRight[16 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[15 + 1 * 12] | ((report_bufRight[16 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[15 + 2 * 12] | ((report_bufRight[16 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f) - acc_gcalibrationRightY;
            acc_gRight.Z = -(int)(averageRight((Int16)(report_bufRight[17 + 0 * 12] | ((report_bufRight[18 + 0 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[17 + 1 * 12] | ((report_bufRight[18 + 1 * 12] << 8) & 0xff00)), (Int16)(report_bufRight[17 + 2 * 12] | ((report_bufRight[18 + 2 * 12] << 8) & 0xff00)))) * (1.0f / 4000f) - acc_gcalibrationRightZ;
            gyr_gRight.X = (int)(averageRight((int)((Int16)((report_bufRight[19 + 0 * 12] | ((report_bufRight[20 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[19 + 1 * 12] | ((report_bufRight[20 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[19 + 2 * 12] | ((report_bufRight[20 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f) - gyr_gcalibrationRightX;
            gyr_gRight.Y = -(int)(averageRight((int)((Int16)((report_bufRight[21 + 0 * 12] | ((report_bufRight[22 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[21 + 1 * 12] | ((report_bufRight[22 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[21 + 2 * 12] | ((report_bufRight[22 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f) - gyr_gcalibrationRightY;
            gyr_gRight.Z = -(int)(averageRight((int)((Int16)((report_bufRight[23 + 0 * 12] | ((report_bufRight[24 + 0 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[23 + 1 * 12] | ((report_bufRight[24 + 1 * 12] << 8) & 0xff00)))), (int)((Int16)((report_bufRight[23 + 2 * 12] | ((report_bufRight[24 + 2 * 12] << 8) & 0xff00)))))) * (1.0f / 600000f) - gyr_gcalibrationRightZ;
            if (!Getstate)
                InitDirectAnglesRight = acc_gRight;
            DirectAnglesRight = acc_gRight - InitDirectAnglesRight;
            i_cRight = new Vector3(1, 0, 0);
            k_cRight = new Vector3(0, 0, 1);
            j_cRight.X = 0f;
            j_cRight.Y = 1f;
            i_bRight = new Vector3(1, 0, 0);
            k_bRight = new Vector3(0, 0, 1);
            j_bRight.Y = 1f;
            j_bRight.Z = 0f;
            i_aRight = new Vector3(1, 0, 0);
            j_aRight = new Vector3(0, 1, 0);
            k_aRight.Y = 0f;
            k_aRight.Z = 1f;
            if (EulerAnglescRight.Y == 0f)
                j_cRight = new Vector3(0, 1, 0);
            if (EulerAnglesbRight.X == 0f)
                j_bRight = new Vector3(0, 1, 0);
            if (EulerAnglesaRight.Z == 0f)
                k_aRight = new Vector3(0, 0, 1);
            if (!Getstate | LeftButtonCAPTURE | RightButtonHOME)
            {
                j_cRight = new Vector3(0, 1, 0);
                InitEulerAnglescRight = ToEulerAnglesRight(GetVectorcRight());
                j_bRight = new Vector3(0, 1, 0);
                InitEulerAnglesbRight = ToEulerAnglesRight(GetVectorbRight());
                k_aRight = new Vector3(0, 0, 1);
                InitEulerAnglesaRight = ToEulerAnglesRight(GetVectoraRight());
            }
            j_cRight += Vector3.Cross(Vector3.Negate(gyr_gRight), j_cRight);
            j_cRight = Vector3.Normalize(j_cRight);
            EulerAnglescRight = ToEulerAnglesRight(GetVectorcRight()) - InitEulerAnglescRight;
            j_bRight += Vector3.Cross(Vector3.Negate(gyr_gRight), j_bRight);
            j_bRight = Vector3.Normalize(j_bRight);
            EulerAnglesbRight = ToEulerAnglesRight(GetVectorbRight()) - InitEulerAnglesbRight;
            k_aRight += Vector3.Cross(Vector3.Negate(gyr_gRight), k_aRight);
            k_aRight = Vector3.Normalize(k_aRight);
            EulerAnglesaRight = ToEulerAnglesRight(GetVectoraRight()) - InitEulerAnglesaRight;
            EulerAnglesRight = new Vector3(EulerAnglesbRight.X, EulerAnglescRight.Y, EulerAnglesaRight.Z);
        }
        private double averageRight(double val1, double val2, double val3)
        {
            arrayRight = new double[] { val1, val2, val3 };
            return arrayRight.Average();
        }
        private double[] CenterSticksRight(UInt16[] vals)
        {
            double[] s = { 0, 0 };
            s[0] = ((int)((vals[0] - stick_calibrationRight[0]) / 100f)) / 13f;
            s[1] = ((int)((vals[1] - stick_calibrationRight[1]) / 100f)) / 13f;
            return s;
        }
        private const string vendor_id = "57e", vendor_id_ = "057e", product_l = "2006", product_r = "2007";
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
        private static double[] stickLeft = { 0, 0 };
        private static SafeFileHandle handleLeft;
        private static byte[] stick_rawLeft = { 0, 0, 0 };
        private static UInt16[] stick_calibrationLeft = { 0, 0 };
        private static UInt16[] stick_precalLeft = { 0, 0 };
        private static Vector3 gyr_gLeft = new Vector3();
        private static Vector3 acc_gLeft = new Vector3();
        private const uint report_lenLeft = 49;
        public static Vector3 i_aLeft = new Vector3(1, 0, 0);
        public static Vector3 j_aLeft = new Vector3(0, 1, 0);
        public static Vector3 k_aLeft = new Vector3(0, 0, 1);
        public static Vector3 i_bLeft = new Vector3(1, 0, 0);
        public static Vector3 j_bLeft = new Vector3(0, 1, 0);
        public static Vector3 k_bLeft = new Vector3(0, 0, 1);
        public static Vector3 i_cLeft = new Vector3(1, 0, 0);
        public static Vector3 j_cLeft = new Vector3(0, 1, 0);
        public static Vector3 k_cLeft = new Vector3(0, 0, 1);
        private static Vector3 InitDirectAnglesLeft, DirectAnglesLeft;
        private static Vector3 InitEulerAnglesaLeft, EulerAnglesaLeft, InitEulerAnglesbLeft, EulerAnglesbLeft, InitEulerAnglescLeft, EulerAnglescLeft, EulerAnglesLeft;
        private static bool LeftButtonSHOULDER_1, LeftButtonSHOULDER_2, LeftButtonSR, LeftButtonSL, LeftButtonDPAD_DOWN, LeftButtonDPAD_RIGHT, LeftButtonDPAD_UP, LeftButtonDPAD_LEFT, LeftButtonMINUS, LeftButtonSTICK, LeftButtonCAPTURE;
        private static byte[] report_bufLeft = new byte[report_lenLeft];
        private static double[] arrayLeft;
        private static byte[] buf_Left = new byte[report_lenLeft];
        public static uint hDevInfoLeft;
        public static double indexLeft = 0;
        public static float acc_gcalibrationLeftX, acc_gcalibrationLeftY, acc_gcalibrationLeftZ, gyr_gcalibrationLeftX, gyr_gcalibrationLeftY, gyr_gcalibrationLeftZ;
        public double[] GetStickLeft()
        {
            return stickLeft;
        }
        public Vector3 GetAccelLeft()
        {
            return acc_gLeft;
        }
        private bool ScanLeft()
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
        public void AttachJoyLeft(string path)
        {
            do
            {
                IntPtr handle = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleLeft = Lhid_open_path(handle);
                SubcommandLeft(0x3, new byte[] { 0x30 }, 1);
                SubcommandLeft(0x40, new byte[] { 0x1 }, 1);
            }
            while (handleLeft.IsInvalid);
        }
        private void SubcommandLeft(byte sc, byte[] buf, uint len)
        {
            Array.Copy(buf, 0, buf_Left, 11, len);
            buf_Left[0] = 0x1;
            buf_Left[1] = 0;
            buf_Left[10] = sc;
            Lhid_write(handleLeft, buf_Left, (UIntPtr)(len + 11));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)49);
        }
        private static double[] stickRight = { 0, 0 };
        private static SafeFileHandle handleRight;
        private static byte[] stick_rawRight = { 0, 0, 0 };
        private static UInt16[] stick_calibrationRight = { 0, 0 };
        private static UInt16[] stick_precalRight = { 0, 0 };
        private static Vector3 acc_gRight = new Vector3();
        private static Vector3 gyr_gRight = new Vector3();
        private const uint report_lenRight = 49;
        public static Vector3 i_cRight = new Vector3(1, 0, 0);
        public static Vector3 j_cRight = new Vector3(0, 1, 0);
        public static Vector3 k_cRight = new Vector3(0, 0, 1);
        public static Vector3 i_bRight = new Vector3(1, 0, 0);
        public static Vector3 j_bRight = new Vector3(0, 1, 0);
        public static Vector3 k_bRight = new Vector3(0, 0, 1);
        public static Vector3 i_aRight = new Vector3(1, 0, 0);
        public static Vector3 j_aRight = new Vector3(0, 1, 0);
        public static Vector3 k_aRight = new Vector3(0, 0, 1);
        private static Vector3 InitDirectAnglesRight, DirectAnglesRight;
        private static Vector3 InitEulerAnglesaRight, EulerAnglesaRight, InitEulerAnglesbRight, EulerAnglesbRight, InitEulerAnglescRight, EulerAnglescRight, EulerAnglesRight;
        private static bool RightButtonSHOULDER_1, RightButtonSHOULDER_2, RightButtonSR, RightButtonSL, RightButtonDPAD_DOWN, RightButtonDPAD_RIGHT, RightButtonDPAD_UP, RightButtonDPAD_LEFT, RightButtonPLUS, RightButtonSTICK, RightButtonHOME;
        private static byte[] report_bufRight = new byte[report_lenRight];
        private static double[] arrayRight;
        private static byte[] buf_Right = new byte[report_lenRight];
        public static uint hDevInfoRight;
        public static double indexRight = 0;
        public static float acc_gcalibrationRightX, acc_gcalibrationRightY, acc_gcalibrationRightZ, gyr_gcalibrationRightX, gyr_gcalibrationRightY, gyr_gcalibrationRightZ;
        public double[] GetStickRight()
        {
            return stickRight;
        }
        public Vector3 GetAccelRight()
        {
            return acc_gRight;
        }
        private bool ScanRight()
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
                    if ((diDetail.DevicePath.Contains(vendor_id) | diDetail.DevicePath.Contains(vendor_id_)) & diDetail.DevicePath.Contains(product_r))
                    {
                        ISRIGHT = true;
                        AttachJoyRight(diDetail.DevicePath);
                        AttachJoyRight(diDetail.DevicePath);
                        AttachJoyRight(diDetail.DevicePath);
                        return true;
                    }
                }
                index++;
            }
            return false;
        }
        public void AttachJoyRight(string path)
        {
            do
            {
                IntPtr handle = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleRight = Rhid_open_path(handle);
                SubcommandRight(0x3, new byte[] { 0x30 }, 1);
                SubcommandRight(0x40, new byte[] { 0x1 }, 1);
            }
            while (handleRight.IsInvalid);
        }
        private void SubcommandRight(byte sc, byte[] buf, uint len)
        {
            Array.Copy(buf, 0, buf_Right, 11, len);
            buf_Right[0] = 0x1;
            buf_Right[1] = 0;
            buf_Right[10] = sc;
            Rhid_write(handleRight, buf_Right, (UIntPtr)(len + 11));
            Rhid_read_timeout(handleRight, buf_Right, (UIntPtr)49);
        }
    }
}