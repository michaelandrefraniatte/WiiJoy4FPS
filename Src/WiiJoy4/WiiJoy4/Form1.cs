using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Numerics;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;
using AForge;
using AForge.Imaging;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System.CodeDom;
using System.Reflection;
namespace WiiJoy4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("system32/user32.dll")]
        public static extern bool SwitchToThisWindow(IntPtr handle, bool fAltTab);
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
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(out int x, out int y);
        private int leftandright;
        public static double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe, camx, camy, irx0, iry0, irx1, iry1, irx, iry, irxc, iryc, mWSIRSensorsXcam, mWSIRSensorsYcam, mWSIRSensors0X, mWSIRSensors0Y, mWSIRSensors1X, mWSIRSensors1Y, mWSButtonStateIRX, mWSButtonStateIRY, mWSIRSensors0Xcam, mWSIRSensors1Xcam, mWSIRSensors0Ycam, mWSIRSensors1Ycam, MyAngle, mWSIR0notfound = 0, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, calibrationinit, watchM = 50, watchM1 = 2, watchM2 = 0, watchK = 50, watchK1 = 2, watchK2 = 0, backpointX, BackpointAnglesX, InitBackpointAnglesX, posRightX, backpointY, BackpointAnglesY, InitBackpointAnglesY, posRightY, stickviewxinit, stickviewyinit, mWSNunchuckStateRawValuesX, mWSNunchuckStateRawValuesY, mWSNunchuckStateRawValuesZ, mWSNunchuckStateRawJoystickX, mWSNunchuckStateRawJoystickY, mWSIRSensorsX, mWSIRSensorsY, irx2, iry2, irx3, iry3;
        public static bool mWSIR1foundcam, mWSIR0foundcam, mWSIR1found, mWSIR0found, mWSIRswitch, mWSButtonStateA, mWSButtonStateB, mWSButtonStateMinus, mWSButtonStateHome, mWSButtonStatePlus, mWSButtonStateOne, mWSButtonStateTwo, mWSButtonStateUp, mWSButtonStateDown, mWSButtonStateLeft, mWSButtonStateRight, Getstate, notpressing1and2, camenabled, mWSNunchuckStateC, mWSNunchuckStateZ, endinvoke;
        public static string processname;
        public static byte[] buff = new byte[] { 0x55 }, mBuff = new byte[22], aBuffer = new byte[22];
        public static byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        public static Guid guid = new System.Guid();
        public static uint hDevInfo, CurrentResolution = 0;
        public FilterInfoCollection CaptureDevice;
        public VideoCaptureDevice FinalFrame;
        public static Bitmap img, EditableImg;
        public static Form1 form = (Form1)Application.OpenForms["Form1"];
        private static BlobCounter blobCounter = new BlobCounter();
        public static BlobsFiltering blobfilter = new BlobsFiltering();
        public static ConnectedComponentsLabeling componentfilter = new ConnectedComponentsLabeling();
        public static Blob[] blobs;
        public static List<IntPoint> corners = new List<IntPoint>();
        public static AForge.Math.Geometry.SimpleShapeChecker shapeChecker = new AForge.Math.Geometry.SimpleShapeChecker();
        public static BrightnessCorrection brightnessfilter = new BrightnessCorrection(-50);
        public static ColorFiltering colorfilter = new ColorFiltering();
        public static Grayscale grayscalefilter = new Grayscale(1, 0, 0);
        public static EuclideanColorFiltering euclideanfilter = new EuclideanColorFiltering();
        public VideoCapabilities[] videoCapabilities;
        public static ThreadStart threadstart;
        public static Thread thread;
        private static bool ISLEFT, ISRIGHT;
        private static Task taskDReadWiimote, taskM, taskK, taskS;
        private static Stopwatch diffM = new Stopwatch(), diffK = new Stopwatch();
        private Type mouseprogram, keyboardprogram;
        private object mouseobj, keyboardobj, objdataReadLeft, objdataReadRight, objdataReadLeftRight, objdataReadWiimote, objdata;
        private Assembly assembly;
        private System.CodeDom.Compiler.CompilerResults results;
        private Microsoft.CSharp.CSharpCodeProvider provider;
        private System.CodeDom.Compiler.CompilerParameters parameters;
        private string addedcode, finalcode, code, mousecode, keyboardcode;
        MouseHook mouseHook = new MouseHook();
        public static IntPtr Param;
        public static int MouseDesktopHookX, MouseDesktopHookY, MouseHookX, MouseHookY, MouseHookWheelM, MouseHookWheelK;
        public static bool MouseHookLeftButton, MouseHookRightButton, MouseHookDoubleClick, MouseHookMiddleButton, getstate;
        private static bool notmouseover = true;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void OnKeyDown(Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                const string message = "• Author: Michaël André Franiatte.\n\r\n\r• Contact: michael.franiatte@gmail.com.\n\r\n\r• Publisher: https://github.com/michaelandrefraniatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• License: Not open source, not free of charge to use.";
                const string caption = "About";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (keyData == Keys.Escape)
            {
                this.Close();
            }
        }
        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            notmouseover = true;
        }
        private void richTextBox1_MouseHover(object sender, EventArgs e)
        {
            notmouseover = false;
        }
        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            notmouseover = false;
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            form.button1.Text = "Start";
            form.button1.Enabled = false;
            form.toolStripButton5.Enabled = false;
            form.toolStripButton6.Enabled = false;
            endinvoke = true;
            Getstate = false;
            Thread.Sleep(100);
            form.BackColor = System.Drawing.Color.Black;
            form.toolStripLabel1.Text = "Searching";
            leftandright = 0;
            endinvoke = false;
            form3.shown();
            taskS = new Task(FormStart);
            taskS.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!Getstate)
            {
                form.button1.Text = "Stop";
                Getstate = true;
                scriptUpdate();
            }
            else
            {
                if (Getstate)
                {
                    form.button1.Text = "Start";
                    Getstate = false;
                    form.BackColor = System.Drawing.Color.WhiteSmoke;
                    form.toolStripLabel1.Text = "Ready";
                }
            }
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            form.button1.Text = "Start";
            form.button1.Enabled = false;
            form.toolStripButton5.Enabled = false;
            form.toolStripButton6.Enabled = false;
            endinvoke = true;
            Getstate = false;
            Thread.Sleep(100);
            form.BackColor = System.Drawing.Color.Black;
            form.toolStripLabel1.Text = "Stoped";
            leftandright = 0;
            endinvoke = false;
            form3.closed();
            form.button1.Enabled = true;
            form.toolStripButton5.Enabled = true;
            form.toolStripButton6.Enabled = true;
            form.BackColor = System.Drawing.Color.WhiteSmoke;
            form.richTextBox1.AppendText("Disconnected" + "...\r\n");
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process processattached = System.Diagnostics.Process.Start("howto.txt");
            }
            catch { }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!getstate)
            {
                getstate = true;
                try
                {
                    form2.Visible = true;
                }
                catch { }
            }
            else
            {
                if (getstate)
                {
                    getstate = false;
                    try
                    {
                        form2.Hide();
                    }
                    catch { }
                }
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process processattached = System.Diagnostics.Process.Start("WiiJoyScripter.exe");
            }
            catch { }
        }
        public static Form2 form2 = new Form2();
        public static Form3 form3 = new Form3();
        public static int[] wd = { 2, 2 };
        public static int[] wu = { 2, 2 };
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
        private void initConfig()
        {
            System.IO.StreamReader file = new System.IO.StreamReader(processname + ".txt");
            string linesofcode = "";
            string emptylines = "";
            int i = 0;
            file.ReadLine();
            file.ReadLine();
            do
            {
                emptylines = file.ReadLine();
                if (emptylines != "")
                {
                    linesofcode += emptylines + " ";
                    i++;
                }
                else
                    break;
            }
            while (emptylines != "" | i == 0);
            file.Close();
            mousecode = linesofcode;
            file = new System.IO.StreamReader(processname + ".txt");
            linesofcode = "";
            emptylines = "";
            i = 0;
            file.ReadLine();
            file.ReadLine();
            do
            {
                emptylines = file.ReadLine();
                if (emptylines != "")
                {
                    linesofcode += emptylines + " ";
                    i++;
                }
                else
                    break;
            }
            while (emptylines != "" | i == 0);
            linesofcode = "";
            emptylines = "";
            i = 0;
            file.ReadLine();
            do
            {
                emptylines = file.ReadLine();
                if (emptylines != "")
                {
                    linesofcode += emptylines + " ";
                    i++;
                }
                else
                    break;
            }
            while (emptylines != "" | i == 0);
            file.Close();
            keyboardcode = linesofcode;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            form.button1.Enabled = false;
            form.toolStripButton5.Enabled = false;
            form.toolStripButton6.Enabled = false;
            form.Location = new System.Drawing.Point(50, 50);
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            System.Diagnostics.Process process = Process.GetCurrentProcess();
            process.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            processname = Process.GetCurrentProcess().ProcessName;
            form.Text = processname;
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(processname + ".txt");
                string imgpath = file.ReadLine().Replace(".txt", ".png");
                file.Close();
                System.Drawing.Image img = System.Drawing.Image.FromFile(imgpath);
                form.Size = new System.Drawing.Size(img.Width, img.Height);
                System.Drawing.Image myimage = new Bitmap(imgpath);
                form.BackgroundImage = myimage;
                form.button1.Visible = false;
                form.toolStrip1.Visible = false;
            }
            catch { }
            try
            {
                Start();
                camenabled = true;
            }
            catch { }
            mouseHook.Hook += new MouseHook.MouseHookCallback(mouseHook_Hook);
            mouseHook.Install();
            form3.shown();
            taskS = new Task(FormStart);
            taskS.Start();
        }
        private void scriptUpdate()
        {
            code = @"
                using System;
                using System.Runtime.InteropServices;
                namespace StringToCode
                {
                    public class FooClass 
                    { 
                        public static uint CurrentResolution = 0;
                        Input input = new Input();
                        funct_driver
                        public bool this[int i]
                        {
                            get { return _valuechanged[i]; }
                            set
                            {
                                if (_valuechanged[i] != value)
                                    _Valuechanged[i] = true;
                                else
                                    _Valuechanged[i] = false;
                                _valuechanged[i] = value;
                            }
                        }
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
                        public void Open()
                        { 
                            TimeBeginPeriod(1);
                            NtSetTimerResolution(1, true, ref CurrentResolution);
                            input.KeyboardFilterMode = KeyboardFilterMode.All;
                            input.MouseFilterMode = MouseFilterMode.All;
                            input.Load(); 
                        } 
                        public void Close()
                        { 
                            TimeEndPeriod(1);
                            input.Unload();
                        } 
                        [DllImport(""InputSending.dll"", EntryPoint = ""MoveMouseTo"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void MoveMouseTo(int x, int y);
                        [DllImport(""InputSending.dll"", EntryPoint = ""MoveMouseBy"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void MoveMouseBy(int x, int y);
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendKey"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendKey(UInt16 bVk, UInt16 bScan);
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendKeyF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendKeyF(UInt16 bVk, UInt16 bScan);
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendKeyArrows"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendKeyArrows(UInt16 bVk, UInt16 bScan);
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendKeyArrowsF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendKeyArrowsF(UInt16 bVk, UInt16 bScan);
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendMouseEventButtonLeft"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendMouseEventButtonLeft();
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendMouseEventButtonLeftF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendMouseEventButtonLeftF();
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendMouseEventButtonRight"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendMouseEventButtonRight();
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendMouseEventButtonRightF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendMouseEventButtonRightF();
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendMouseEventButtonMiddle"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendMouseEventButtonMiddle();
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendMouseEventButtonMiddleF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendMouseEventButtonMiddleF();
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendMouseEventButtonWheelUp"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendMouseEventButtonWheelUp();
                        [DllImport(""InputSending.dll"", EntryPoint = ""SendMouseEventButtonWheelDown"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SendMouseEventButtonWheelDown();
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""SimulateKeyDown"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SimulateKeyDown(UInt16 keyCode, UInt16 bScan);
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""SimulateKeyUp"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SimulateKeyUp(UInt16 keyCode, UInt16 bScan);
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""SimulateKeyDownArrows"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SimulateKeyDownArrows(UInt16 keyCode, UInt16 bScan);
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""SimulateKeyUpArrows"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SimulateKeyUpArrows(UInt16 keyCode, UInt16 bScan);
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""MouseMW3"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void MouseMW3(int x, int y);
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""MouseBrink"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void MouseBrink(int x, int y);
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""LeftClick"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void LeftClick();
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""LeftClickF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void LeftClickF();
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""RightClick"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void RightClick();
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""RightClickF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void RightClickF();
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""MiddleClick"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void MiddleClick();
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""MiddleClickF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void MiddleClickF();
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""WheelDownF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void WheelDownF();
                        [DllImport(""SendInputLibrary.dll"", EntryPoint = ""WheelUpF"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void WheelUpF();
                        [DllImport(""user32.dll"")]
                        public static extern void SetPhysicalCursorPos(int X, int Y);
                        [DllImport(""user32.dll"")]
                        public static extern void SetCaretPos(int X, int Y);
                        [DllImport(""user32.dll"")]
                        public static extern void SetCursorPos(int X, int Y);
                        [DllImport(""winmm.dll"", EntryPoint = ""timeBeginPeriod"")]
                        public static extern uint TimeBeginPeriod(uint ms);
                        [DllImport(""winmm.dll"", EntryPoint = ""timeEndPeriod"")]
                        public static extern uint TimeEndPeriod(uint ms);
                        [DllImport(""ntdll.dll"", EntryPoint = ""NtSetTimerResolution"")]
                        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
                        [DllImport(""system32/user32.dll"")]
                        public static extern uint MapVirtualKey(uint uCode, uint uMapType);
                        [DllImport(""user32.dll"")]
                        public static extern bool GetAsyncKeyState(System.Windows.Forms.Keys vKey);
                        [DllImport(""User32.dll"")]
                        private static extern bool GetCursorPos(out int x, out int y);
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
                    }
                    public class Input
                    {
                        private IntPtr context;
                        public KeyboardFilterMode KeyboardFilterMode { get; set; }
                        public MouseFilterMode MouseFilterMode { get; set; }
                        public bool IsLoaded { get; set; }
                        public Input()
                        {
                            context = IntPtr.Zero;
                            KeyboardFilterMode = KeyboardFilterMode.None;
                            MouseFilterMode = MouseFilterMode.None;
                        }
                        public bool Load()
                        {
                            context = InterceptionDriver.CreateContext();
                            return true;
                        }
                        public void Unload()
                        {
                            InterceptionDriver.DestroyContext(context);
                        }
                        public void SendKey(Keys key, KeyState state, int keyboardId)
                        {
                            Stroke stroke = new Stroke();
                            KeyStroke keyStroke = new KeyStroke();
                            keyStroke.Code = key;
                            keyStroke.State = state;
                            stroke.Key = keyStroke;
                            InterceptionDriver.Send(context, keyboardId, ref stroke, 1);
                        }
                        public void SendKey(Keys key, int keyboardId)
                        {
                            SendKey(key, KeyState.Down, keyboardId);
                        }
                        public void SendKeyF(Keys key, int keyboardId)
                        {
                            SendKey(key, KeyState.Up, keyboardId);
                        }
                        public void SendMouseEvent(MouseState state, int mouseId)
                        {
                            Stroke stroke = new Stroke();
                            MouseStroke mouseStroke = new MouseStroke();
                            mouseStroke.State = state;
                            if (state == MouseState.ScrollUp)
                            {
                                mouseStroke.Rolling = 120;
                            }
                            else if (state == MouseState.ScrollDown)
                            {
                                mouseStroke.Rolling = -120;
                            }
                            stroke.Mouse = mouseStroke;
                            InterceptionDriver.Send(context, mouseId, ref stroke, 1);
                        }
                        public void SendLeftClick(int mouseId)
                        {
                            SendMouseEvent(MouseState.LeftDown, mouseId);
                        }
                        public void SendRightClick(int mouseId)
                        {
                            SendMouseEvent(MouseState.RightDown, mouseId);
                        }
                        public void SendLeftClickF(int mouseId)
                        {
                            SendMouseEvent(MouseState.LeftUp, mouseId);
                        }
                        public void SendRightClickF(int mouseId)
                        {
                            SendMouseEvent(MouseState.RightUp, mouseId);
                        }
                        public void SendMiddleClick(int mouseId)
                        {
                            SendMouseEvent(MouseState.MiddleDown, mouseId);
                        }
                        public void SendMiddleClickF(int mouseId)
                        {
                            SendMouseEvent(MouseState.MiddleUp, mouseId);
                        }
                        public void SendWheelUp(int mouseId)
                        {
                            SendMouseEvent(MouseState.ScrollUp, mouseId);
                        }
                        public void SendWheelDown(int mouseId)
                        {
                            SendMouseEvent(MouseState.ScrollDown, mouseId);
                        }
                        public void MoveMouseBy(int deltaX, int deltaY, int mouseId)
                        {
                            Stroke stroke = new Stroke();
                            MouseStroke mouseStroke = new MouseStroke();
                            mouseStroke.X = deltaX;
                            mouseStroke.Y = deltaY;
                            stroke.Mouse = mouseStroke;
                            stroke.Mouse.Flags = MouseFlags.MoveRelative;
                            InterceptionDriver.Send(context, mouseId, ref stroke, 1);
                        }
                        public void MoveMouseTo(int x, int y, int mouseId)
                        {
                            Stroke stroke = new Stroke();
                            MouseStroke mouseStroke = new MouseStroke();
                            mouseStroke.X = x;
                            mouseStroke.Y = y;
                            stroke.Mouse = mouseStroke;
                            stroke.Mouse.Flags = MouseFlags.MoveAbsolute;
                            InterceptionDriver.Send(context, mouseId, ref stroke, 1);
                        }
                    }
                    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
                    public delegate int Predicate(int device);
                    [Flags]
                    public enum KeyState : ushort
                    {
                        Down = 0x00,
                        Up = 0x01,
                        E0 = 0x02,
                        E1 = 0x04,
                        TermsrvSetLED = 0x08,
                        TermsrvShadow = 0x10,
                        TermsrvVKPacket = 0x20
                    }
                    [Flags]
                    public enum KeyboardFilterMode : ushort
                    {
                        None = 0x0000,
                        All = 0xFFFF,
                        KeyDown = KeyState.Up,
                        KeyUp = KeyState.Up << 1,
                        KeyE0 = KeyState.E0 << 1,
                        KeyE1 = KeyState.E1 << 1,
                        KeyTermsrvSetLED = KeyState.TermsrvSetLED << 1,
                        KeyTermsrvShadow = KeyState.TermsrvShadow << 1,
                        KeyTermsrvVKPacket = KeyState.TermsrvVKPacket << 1
                    }
                    [Flags]
                    public enum MouseState : ushort
                    {
                        LeftDown = 0x01,
                        LeftUp = 0x02,
                        RightDown = 0x04,
                        RightUp = 0x08,
                        MiddleDown = 0x10,
                        MiddleUp = 0x20,
                        LeftExtraDown = 0x40,
                        LeftExtraUp = 0x80,
                        RightExtraDown = 0x100,
                        RightExtraUp = 0x200,
                        ScrollVertical = 0x400,
                        ScrollUp = 0x400,
                        ScrollDown = 0x400,
                        ScrollHorizontal = 0x800,
                        ScrollLeft = 0x800,
                        ScrollRight = 0x800,
                    }
                    [Flags]
                    public enum MouseFilterMode : ushort
                    {
                        None = 0x0000,
                        All = 0xFFFF,
                        LeftDown = 0x01,
                        LeftUp = 0x02,
                        RightDown = 0x04,
                        RightUp = 0x08,
                        MiddleDown = 0x10,
                        MiddleUp = 0x20,
                        LeftExtraDown = 0x40,
                        LeftExtraUp = 0x80,
                        RightExtraDown = 0x100,
                        RightExtraUp = 0x200,
                        MouseWheelVertical = 0x400,
                        MouseWheelHorizontal = 0x800,
                        MouseMove = 0x1000,
                    }
                    [Flags]
                    public enum MouseFlags : ushort
                    {
                        MoveRelative = 0x000,
                        MoveAbsolute = 0x001,
                        VirtualDesktop = 0x002,
                        AttributesChanged = 0x004,
                        MoveWithoutCoalescing = 0x008,
                        TerminalServicesSourceShadow = 0x100
                    }
                    [StructLayout(LayoutKind.Sequential)]
                    public struct MouseStroke
                    {
                        public MouseState State;
                        public MouseFlags Flags;
                        public Int16 Rolling;
                        public Int32 X;
                        public Int32 Y;
                        public UInt16 Information;
                    }
                    [StructLayout(LayoutKind.Sequential)]
                    public struct KeyStroke
                    {
                        public Keys Code;
                        public KeyState State;
                        public UInt32 Information;
                    }
                    [StructLayout(LayoutKind.Explicit)]
                    public struct Stroke
                    {
                        [FieldOffset(0)]
                        public MouseStroke Mouse;
                        [FieldOffset(0)]
                        public KeyStroke Key;
                    }
                    public static class InterceptionDriver
                    {
                        [DllImport(""interception.dll"", EntryPoint = ""interception_create_context"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern IntPtr CreateContext();
                        [DllImport(""interception.dll"", EntryPoint = ""interception_destroy_context"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void DestroyContext(IntPtr context);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_get_precedence"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void GetPrecedence(IntPtr context, Int32 device);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_set_precedence"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SetPrecedence(IntPtr context, Int32 device, Int32 Precedence);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_get_filter"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void GetFilter(IntPtr context, Int32 device);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_set_filter"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern void SetFilter(IntPtr context, Predicate predicate, Int32 keyboardFilterMode);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_wait"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern Int32 Wait(IntPtr context);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_wait_with_timeout"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern Int32 WaitWithTimeout(IntPtr context, UInt64 milliseconds);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_send"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern Int32 Send(IntPtr context, Int32 device, ref Stroke stroke, UInt32 numStrokes);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_receive"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern Int32 Receive(IntPtr context, Int32 device, ref Stroke stroke, UInt32 numStrokes);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_get_hardware_id"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern Int32 GetHardwareId(IntPtr context, Int32 device, String hardwareIdentifier, UInt32 sizeOfString);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_is_invalid"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern Int32 IsInvalid(Int32 device);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_is_keyboard"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern Int32 IsKeyboard(Int32 device);
                        [DllImport(""interception.dll"", EntryPoint = ""interception_is_mouse"", CallingConvention = CallingConvention.Cdecl)]
                        public static extern Int32 IsMouse(Int32 device);
                    }
                    public class KeyPressedEventArgs : EventArgs
                    {
                        public Keys Key { get; set; }
                        public KeyState State { get; set; }
                        public bool Handled { get; set; }
                    }
                    public enum Keys : ushort
                    {
                        CANCEL = 70,
                        BACK = 14,
                        TAB = 15,
                        CLEAR = 76,
                        RETURN = 28,
                        SHIFT = 42,
                        CONTROL = 29,
                        MENU = 56,
                        CAPITAL = 58,
                        ESCAPE = 1,
                        SPACE = 57,
                        PRIOR = 73,
                        NEXT = 81,
                        END = 79,
                        HOME = 71,
                        LEFT = 101,
                        UP = 100,
                        RIGHT = 103,
                        DOWN = 102,
                        SNAPSHOT = 84,
                        INSERT = 91,
                        NUMPADDEL = 83,
                        NUMPADINSERT = 82,
                        HELP = 99,
                        APOSTROPHE = 41,
                        BACKSPACE = 14,
                        PAGEDOWN = 97,
                        PAGEUP = 93,
                        FIN = 96,
                        MOUSE = 105,
                        A = 16,
                        B = 48,
                        C = 46,
                        D = 32,
                        E = 18,
                        F = 33,
                        G = 34,
                        H = 35,
                        I = 23,
                        J = 36,
                        K = 37,
                        L = 38,
                        M = 39,
                        N = 49,
                        O = 24,
                        P = 25,
                        Q = 30,
                        R = 19,
                        S = 31,
                        T = 20,
                        U = 22,
                        V = 47,
                        W = 44,
                        X = 45,
                        Y = 21,
                        Z = 17,
                        LWIN = 91,
                        RWIN = 92,
                        APPS = 93,
                        DELETE = 95,
                        NUMPAD0 = 82,
                        NUMPAD1 = 79,
                        NUMPAD2 = 80,
                        NUMPAD3 = 81,
                        NUMPAD4 = 75,
                        NUMPAD5 = 76,
                        NUMPAD6 = 77,
                        NUMPAD7 = 71,
                        NUMPAD8 = 72,
                        NUMPAD9 = 73,
                        MULTIPLY = 55,
                        ADD = 78,
                        SUBTRACT = 74,
                        DECIMAL = 83,
                        PRINTSCREEN = 84,
                        DIVIDE = 53,
                        F1 = 59,
                        F2 = 60,
                        F3 = 61,
                        F4 = 62,
                        F5 = 63,
                        F6 = 64,
                        F7 = 65,
                        F8 = 66,
                        F9 = 67,
                        F10 = 68,
                        F11 = 87,
                        F12 = 88,
                        NUMLOCK = 69,
                        SCROLLLOCK = 70,
                        LEFTSHIFT = 42,
                        RIGHTSHIFT = 54,
                        LEFTCONTROL = 29,
                        RIGHTCONTROL = 99,
                        LEFTALT = 56,
                        RIGHTALT = 98,
                        BROWSER_BACK = 106,
                        BROWSER_FORWARD = 105,
                        BROWSER_REFRESH = 103,
                        BROWSER_STOP = 104,
                        BROWSER_SEARCH = 101,
                        BROWSER_FAVORITES = 102,
                        BROWSER_HOME = 50,
                        VOLUME_MUTE = 32,
                        VOLUME_DOWN = 46,
                        VOLUME_UP = 48,
                        MEDIA_NEXT_TRACK = 25,
                        MEDIA_PREV_TRACK = 16,
                        MEDIA_STOP = 36,
                        MEDIA_PLAY_PAUSE = 34,
                        LAUNCH_MAIL = 108,
                        LAUNCH_MEDIA_SELECT = 109,
                        LAUNCH_APP1 = 107,
                        LAUNCH_APP2 = 33,
                        OEM_1 = 27,
                        OEM_PLUS = 13,
                        OEM_COMMA = 50,
                        OEM_MINUS = 0,
                        OEM_PERIOD = 51,
                        OEM_2 = 52,
                        OEM_3 = 40,
                        OEM_4 = 12,
                        OEM_5 = 43,
                        OEM_6 = 26,
                        OEM_7 = 41,
                        OEM_8 = 53,
                        OEM_102 = 86,
                        EREOF = 93,
                        ZOOM = 98,
                        Escape = 1,
                        One = 2,
                        Two = 3,
                        Three = 4,
                        Four = 5,
                        Five = 6,
                        Six = 7,
                        Seven = 8,
                        Eight = 9,
                        Nine = 10,
                        Zero = 11,
                        DashUnderscore = 12,
                        PlusEquals = 13,
                        Backspace = 14,
                        Tab = 15,
                        OpenBracketBrace = 26,
                        CloseBracketBrace = 27,
                        Enter = 28,
                        Control = 29,
                        SemicolonColon = 39,
                        SingleDoubleQuote = 40,
                        Tilde = 41,
                        LeftShift = 42,
                        BackslashPipe = 43,
                        CommaLeftArrow = 51,
                        PeriodRightArrow = 52,
                        ForwardSlashQuestionMark = 53,
                        RightShift = 54,
                        RightAlt = 56,
                        Space = 57,
                        CapsLock = 58,
                        Up = 72,
                        Down = 80,
                        Right = 77,
                        Left = 75,
                        Home = 71,
                        End = 79,
                        Delete = 83,
                        PageUp = 73,
                        PageDown = 81,
                        Insert = 82,
                        PrintScreen = 55,
                        NumLock = 69,
                        ScrollLock = 70,
                        Menu = 93,
                        WindowsKey = 91,
                        NumpadDivide = 53,
                        NumpadAsterisk = 55,
                        Numpad7 = 71,
                        Numpad8 = 72,
                        Numpad9 = 73,
                        Numpad4 = 75,
                        Numpad5 = 76,
                        Numpad6 = 77,
                        Numpad1 = 79,
                        Numpad2 = 80,
                        Numpad3 = 81,
                        Numpad0 = 82,
                        NumpadDelete = 83,
                        NumpadEnter = 28,
                        NumpadPlus = 78,
                        NumpadMinus = 74,
                    }
                    public class MousePressedEventArgs : EventArgs
                    {
                        public MouseState State { get; set; }
                        public bool Handled { get; set; }
                        public int X { get; set; }
                        public int Y { get; set; }
                        public short Rolling { get; set; }
                    }
                    public enum ScrollDirection
                    {
                        Down,
                        Up
                    }
                }
                ";
            addedcode = @"public static int[] wd = { 2 };
                public static int[] wu = { 2 };
                public static bool[] _Valuechanged = new bool[2], _valuechanged = new bool[2];
                public double WidthS = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2f;
	            public double HeightS = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2f;
                public double mousex, mousey, mousexp, mouseyp, irx, iry, deadzone = 30f, zoning = 250f, hardness = 330f, center = 200f, max = 1900f;
                public void Main(double mWSNunchuckStateRawJoystickX, double mWSNunchuckStateRawJoystickY, double mWSNunchuckStateRawValuesX, double mWSNunchuckStateRawValuesY, double mWSNunchuckStateRawValuesZ, bool mWSNunchuckStateC, bool mWSNunchuckStateZ, 
                double mWSButtonStateIRX, double mWSButtonStateIRY, bool mWSButtonStateA, bool mWSButtonStateB, bool mWSButtonStateMinus, bool mWSButtonStateHome, bool mWSButtonStatePlus, bool mWSButtonStateOne, bool mWSButtonStateTwo, bool mWSButtonStateUp, bool mWSButtonStateDown, bool mWSButtonStateLeft, bool mWSButtonStateRight, double mWSRawValuesX, double mWSRawValuesY, double mWSRawValuesZ, 
                float EulerAnglesX, float EulerAnglesY, float EulerAnglesZ, float DirectAnglesX, float DirectAnglesY, float DirectAnglesZ, double camx, double camy, float EulerAnglesLeftX, float EulerAnglesLeftY, float EulerAnglesLeftZ, float DirectAnglesLeftX, float DirectAnglesLeftY, float DirectAnglesLeftZ, float EulerAnglesRightX, float EulerAnglesRightY, float EulerAnglesRightZ, 
                float DirectAnglesRightX, float DirectAnglesRightY, float DirectAnglesRightZ, bool LeftButtonSHOULDER_1, bool LeftButtonMINUS, bool LeftButtonCAPTURE, bool LeftButtonDPAD_UP, bool LeftButtonDPAD_LEFT, bool LeftButtonDPAD_DOWN, bool LeftButtonDPAD_RIGHT, bool LeftButtonSTICK, bool RightButtonDPAD_DOWN, bool LeftButtonSL, bool LeftButtonSR, double GetStickLeftX, double GetStickLeftY, 
                bool RightButtonPLUS, bool RightButtonDPAD_RIGHT, bool RightButtonHOME, bool RightButtonSHOULDER_1, bool RightButtonDPAD_LEFT, bool RightButtonDPAD_UP, bool RightButtonSTICK, bool RightButtonSL, bool RightButtonSR, bool RightButtonSHOULDER_2, bool LeftButtonSHOULDER_2, double GetStickRightX, double GetStickRightY, float GetAccelX, float GetAccelY, float GetAccelZ, float GetAccelRightX, float GetAccelRightY, float GetAccelRightZ, float GetAccelLeftX, float GetAccelLeftY, float GetAccelLeftZ, 
                int MouseHookX, int MouseHookY, int MouseHookWheel, bool MouseHookLeftButton, bool MouseHookRightButton, bool MouseHookDoubleClick, bool MouseHookMiddleButton, double watchM)
                { 
                    irx = (mWSButtonStateIRX >= 0 ? Scale(mWSButtonStateIRX, 0f, 1360f, 0f, 1500f) : Scale(mWSButtonStateIRX, -1360f, 0f, -1500f, 0f));
                    iry = (mWSButtonStateIRY + center >= 0 ? Scale(mWSButtonStateIRY + center, 0f, 768f + center, 0f, max) : Scale(mWSButtonStateIRY + center, -768f + center, 0f, -max, 0f));
                    mousex = Math.Pow(irx > 0 ? irx : -irx, zoning / 100f) * (1500f / Math.Pow(1500f, zoning / 100f)) * (irx > 0 ? 1f : -1f) * (-deadzone / 100f + 1f);
                    mousey = Math.Pow(iry > 0 ? iry : -iry, zoning * (1500f / max) / 100f) * (max / Math.Pow(max, zoning * (1500f / max) / 100f)) * (iry > 0 ? 1f : -1f) * (-deadzone / 100f + 1f);
                    mousexp += mousex * watchM / 40f;
                    mouseyp += mousey * watchM / 40f;
                    MouseMW3((int)(32767.5f - (mousex * hardness) / 100f - mousexp), (int)((mousey * hardness) / 100f + mouseyp + 32767.5f));
	                System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)(WidthS - irx * WidthS / 1024f), (int)(HeightS + iry * HeightS / 1024f));
		            System.Threading.Thread.Sleep(1);
                } 
                private double Scale(double value, double min, double max, double minScale, double maxScale)
                {
                    double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
                    return scaled;
                }";
            string keyboardaddedcode = @"public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
                public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
                public static bool[] _Valuechanged = new bool[36], _valuechanged = new bool[36];
                public bool foraorcison, mWSButtonStateAio, randA, ApressIO = false, HomeFTG = false;
                public void Main(double mWSNunchuckStateRawJoystickX, double mWSNunchuckStateRawJoystickY, double mWSNunchuckStateRawValuesX, double mWSNunchuckStateRawValuesY, double mWSNunchuckStateRawValuesZ, bool mWSNunchuckStateC, bool mWSNunchuckStateZ, 
                double mWSButtonStateIRX, double mWSButtonStateIRY, bool mWSButtonStateA, bool mWSButtonStateB, bool mWSButtonStateMinus, bool mWSButtonStateHome, bool mWSButtonStatePlus, bool mWSButtonStateOne, bool mWSButtonStateTwo, bool mWSButtonStateUp, bool mWSButtonStateDown, bool mWSButtonStateLeft, bool mWSButtonStateRight, double mWSRawValuesX, double mWSRawValuesY, double mWSRawValuesZ, 
                float EulerAnglesX, float EulerAnglesY, float EulerAnglesZ, float DirectAnglesX, float DirectAnglesY, float DirectAnglesZ, double camx, double camy, float EulerAnglesLeftX, float EulerAnglesLeftY, float EulerAnglesLeftZ, float DirectAnglesLeftX, float DirectAnglesLeftY, float DirectAnglesLeftZ, float EulerAnglesRightX, float EulerAnglesRightY, float EulerAnglesRightZ, 
                float DirectAnglesRightX, float DirectAnglesRightY, float DirectAnglesRightZ, bool LeftButtonSHOULDER_1, bool LeftButtonMINUS, bool LeftButtonCAPTURE, bool LeftButtonDPAD_UP, bool LeftButtonDPAD_LEFT, bool LeftButtonDPAD_DOWN, bool LeftButtonDPAD_RIGHT, bool LeftButtonSTICK, bool RightButtonDPAD_DOWN, bool LeftButtonSL, bool LeftButtonSR, double GetStickLeftX, double GetStickLeftY, 
                bool RightButtonPLUS, bool RightButtonDPAD_RIGHT, bool RightButtonHOME, bool RightButtonSHOULDER_1, bool RightButtonDPAD_LEFT, bool RightButtonDPAD_UP, bool RightButtonSTICK, bool RightButtonSL, bool RightButtonSR, bool RightButtonSHOULDER_2, bool LeftButtonSHOULDER_2, double GetStickRightX, double GetStickRightY, float GetAccelX, float GetAccelY, float GetAccelZ, float GetAccelRightX, float GetAccelRightY, float GetAccelRightZ, float GetAccelLeftX, float GetAccelLeftY, float GetAccelLeftZ, 
                int MouseHookX, int MouseHookY, int MouseHookWheel, bool MouseHookLeftButton, bool MouseHookRightButton, bool MouseHookDoubleClick, bool MouseHookMiddleButton, double watchM)
                { 
                    valchanged(2, LeftButtonSHOULDER_2);
                    if (wd[2] == 1)
                        SimulateKeyDown(VK_LeftControl, S_LeftControl);
                    if (wu[2] == 1)
                        SimulateKeyUp(VK_LeftControl, S_LeftControl);
                    valchanged(3, LeftButtonMINUS);
                    if (wd[3] == 1)
                        SimulateKeyDown(VK_Return, S_Return);
                    if (wu[3] == 1)
                        SimulateKeyUp(VK_Return, S_Return);
                    valchanged(4, (GetAccelLeftX > 0.7f | GetAccelLeftX < -0.7f) & !((mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 40f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 40f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 40f));
                    if (wd[4] == 1)
                        SimulateKeyDown(VK_V, S_V);
                    if (wu[4] == 1)
                        SimulateKeyUp(VK_V, S_V);
                    valchanged(27, LeftButtonCAPTURE);
                    if (wd[27] == 1)
                        SimulateKeyDown(VK_P, S_P);
                    if (wu[27] == 1)
                        SimulateKeyUp(VK_P, S_P);
                    valchanged(5, LeftButtonDPAD_UP);
                    if (wd[5] == 1)
                        SimulateKeyDownArrows(VK_UP, S_UP);
                    if (wu[5] == 1)
                        SimulateKeyUpArrows(VK_UP, S_UP);
                    valchanged(6, LeftButtonDPAD_LEFT);
                    if (wd[6] == 1)
                        SimulateKeyDownArrows(VK_LEFT, S_LEFT);
                    if (wu[6] == 1)
                        SimulateKeyUpArrows(VK_LEFT, S_LEFT);
                    valchanged(7, LeftButtonDPAD_DOWN);
                    if (wd[7] == 1)
                        SimulateKeyDownArrows(VK_DOWN, S_DOWN);
                    if (wu[7] == 1)
                        SimulateKeyUpArrows(VK_DOWN, S_DOWN);
                    valchanged(8, LeftButtonDPAD_RIGHT);
                    if (wd[8] == 1)
                        SimulateKeyDownArrows(VK_RIGHT, S_RIGHT);
                    if (wu[8] == 1)
                        SimulateKeyUpArrows(VK_RIGHT, S_RIGHT);
                    valchanged(9, LeftButtonSTICK);
                    if (wd[9] == 1)
                        SimulateKeyDown(VK_LeftShift, S_LeftShift);
                    if (wu[9] == 1)
                        SimulateKeyUp(VK_LeftShift, S_LeftShift);
                    valchanged(10, LeftButtonSHOULDER_1);
                    if (wd[10] == 1)
                        SimulateKeyDown(VK_Space, S_Space);
                    if (wu[10] == 1)
                        SimulateKeyUp(VK_Space, S_Space);
                    valchanged(29, LeftButtonSL);
                    if (wd[29] == 1)
                        SimulateKeyDown(VK_B, S_B);
                    if (wu[29] == 1)
                        SimulateKeyUp(VK_B, S_B);
                    valchanged(28, LeftButtonSR);
                    if (wd[28] == 1)
                        SimulateKeyDown(VK_N, S_N);
                    if (wu[28] == 1)
                        SimulateKeyUp(VK_N, S_N);
                    valchanged(16, GetStickLeftX > 0.33f);
                    valchanged(17, GetStickLeftX < -0.33f);
                    valchanged(18, GetStickLeftY > 0.33f);
                    valchanged(19, GetStickLeftY < -0.33f);
                    if (wd[16] == 1)
                        SimulateKeyDown(VK_D, S_D);
                    if (wu[16] == 1)
                        SimulateKeyUp(VK_D, S_D);
                    if (wd[17] == 1)
                        SimulateKeyDown(VK_Q, S_Q);
                    if (wu[17] == 1)
                        SimulateKeyUp(VK_Q, S_Q);
                    if (wd[18] == 1)
                        SimulateKeyDown(VK_Z, S_Z);
                    if (wu[18] == 1)
                        SimulateKeyUp(VK_Z, S_Z);
                    if (wd[19] == 1)
                        SimulateKeyDown(VK_S, S_S);
                    if (wu[19] == 1)
                        SimulateKeyUp(VK_S, S_S);
                    valchanged(30, DirectAnglesLeftY <= -0.3f);
                    if (wd[30] == 1)
                        SimulateKeyDown(VK_A, S_A);
                    if (wu[30] == 1)
                        SimulateKeyUp(VK_A, S_A);
                    valchanged(31, DirectAnglesLeftY >= 0.3f);
                    if (wd[31] == 1)
                        SimulateKeyDown(VK_E, S_E);
                    if (wu[31] == 1)
                        SimulateKeyUp(VK_E, S_E);
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
                    valchanged(14, mWSButtonStatePlus | (HomeFTG & mWSButtonStateHome));
                    if (wd[14] == 1)
                        SimulateKeyDown(VK_G, S_G);
                    if (wu[14] == 1)
                        SimulateKeyUp(VK_G, S_G);
                    valchanged(15, mWSButtonStateMinus | (HomeFTG & mWSButtonStateHome));
                    if (wd[15] == 1)
                        SimulateKeyDown(VK_T, S_T);
                    if (wu[15] == 1)
                        SimulateKeyUp(VK_T, S_T);
                    valchanged(11, mWSButtonStateB);
                    if (wd[11] == 1)
                        LeftClick();
                    if (wu[11] == 1)
                        LeftClickF();
                    if (ApressIO)
                    {
                        foraorcison = (mWSButtonStateMinus | mWSButtonStatePlus | mWSButtonStateHome | ((mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 40f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 40f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 40f) | mWSButtonStateUp | mWSButtonStateDown | mWSButtonStateLeft | mWSButtonStateRight);
                        valchanged(32, mWSButtonStateA);
                        if (wd[32] == 1)
                            if (!randA)
                            {
                                mWSButtonStateAio = true;
                                randA = true;
                            }
                            else
                                if (randA)
                                {
                                    mWSButtonStateAio = false;
                                    randA = false;
                                }
                        if (mWSButtonStateAio & foraorcison)
                        {
                            mWSButtonStateAio = false;
                            randA = false;
                        }
                        valchanged(33, mWSButtonStateAio | (mWSButtonStateA & foraorcison));
                        if (wd[33] == 1)
                            RightClick();
                        if (wu[33] == 1)
                            RightClickF();
                    }
                    else
                    {
                        valchanged(34, mWSButtonStateA);
                        if (wd[34] == 1)
                            RightClick();
                        if (wu[34] == 1)
                            RightClickF();
                    }
		            System.Threading.Thread.Sleep(1);
                }";
            try { initConfig(); }
            catch
            {
                using (System.IO.StreamWriter createdfile = System.IO.File.AppendText(processname + ".txt"))
                {
                    createdfile.WriteLine(processname + ".txt");
                    createdfile.WriteLine("//mouse control");
                    createdfile.Write(addedcode);
                    createdfile.WriteLine("");
                    createdfile.WriteLine("");
                    createdfile.WriteLine("//keyboard control");
                    createdfile.Write(keyboardaddedcode);
                    createdfile.WriteLine("");
                    createdfile.WriteLine("");
                }
                initConfig();
            }
            finalcode = code.Replace("funct_driver", mousecode);
            parameters = new System.CodeDom.Compiler.CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            results = provider.CompileAssemblyFromSource(parameters, finalcode);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}) : {1}", error.ErrorNumber, error.ErrorText));
                }
                MessageBox.Show("mouse control :\n\r" + sb.ToString());
                Getstate = false;
                form.BackColor = System.Drawing.Color.Black;
                form.toolStripLabel1.Text = "Error";
                return;
            }
            assembly = results.CompiledAssembly;
            mouseprogram = assembly.GetType("StringToCode.FooClass");
            mouseobj = Activator.CreateInstance(mouseprogram);
            finalcode = code.Replace("funct_driver", keyboardcode);
            parameters = new System.CodeDom.Compiler.CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            results = provider.CompileAssemblyFromSource(parameters, finalcode);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}) : {1}", error.ErrorNumber, error.ErrorText));
                }
                MessageBox.Show("keyboard control :\n\r" + sb.ToString());
                Getstate = false;
                form.BackColor = System.Drawing.Color.Black;
                form.toolStripLabel1.Text = "Error";
                return;
            }
            assembly = results.CompiledAssembly;
            keyboardprogram = assembly.GetType("StringToCode.FooClass");
            keyboardobj = Activator.CreateInstance(keyboardprogram);
            taskM = new Task(WiiJoy_thrM);
            taskM.Start();
            taskK = new Task(WiiJoy_thrK);
            taskK.Start();
            assembly = null;
            results = null;
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            parameters = new System.CodeDom.Compiler.CompilerParameters();
            finalcode = "";
            code = "";
            assembly = null;
            results = null;
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            parameters = new System.CodeDom.Compiler.CompilerParameters();
            finalcode = "";
            code = "";
            mousecode = "";
            keyboardcode = "";
            form.BackColor = System.Drawing.Color.DarkGray;
            form.toolStripLabel1.Text = "Running";
        }
        private bool initConnected()
        {
            bool connected = false;
            System.IO.StreamReader file = new System.IO.StreamReader("initconnect.txt");
            connected = bool.Parse(file.ReadLine());
            leftandright = (int)Convert.ToDouble(file.ReadLine());
            file.Close();
            return connected;
        }
        private void FormStart()
        {
            bool connected = false;
            do
            {
                try { connected = initConnected(); }
                catch { }
                Thread.Sleep(1);
            }
            while (!connected & !notpressing1and2);
            System.IO.StreamWriter file = new System.IO.StreamWriter("initconnect.txt");
            file.WriteLine("False");
            file.WriteLine(leftandright.ToString());
            file.Close();
            if (!notpressing1and2)
            {
                ISLEFT = true;
                ISRIGHT = true;
                diffM.Start();
                diffK.Start();
                if (camenabled)
                {
                    objdata = Activator.CreateInstance(typeof(dataClass));
                    form.richTextBox1.AppendText("Camera found" + "...\r\n");
                }
                if (leftandright == 1 | leftandright == 2 | leftandright == 4 | leftandright == 7)
                {
                    objdataReadLeft = Activator.CreateInstance(typeof(dataReadClassLeft));
                    form.richTextBox1.AppendText("Joycon Left found" + "...\r\n");
                }
                if (leftandright == 4 | leftandright == 5 | leftandright == 6 | leftandright == 7)
                {
                    objdataReadRight = Activator.CreateInstance(typeof(dataReadClassRight));
                    form.richTextBox1.AppendText("Joycon Right found" + "...\r\n");
                }
                if (leftandright == 4 | leftandright == 7)
                {
                    objdataReadLeftRight = Activator.CreateInstance(typeof(dataReadClassLeftRight));
                }
                if (leftandright == 2 | leftandright == 3 | leftandright == 5 | leftandright == 7)
                {
                    objdataReadWiimote = Activator.CreateInstance(typeof(dataReadClassWiimote));
                    taskDReadWiimote = new Task(WiiJoy_thrDReadWiimote);
                    taskDReadWiimote.Start();
                    form.richTextBox1.AppendText("Wiimote found" + "...\r\n");
                }
                Thread.Sleep(2000);
                if (leftandright == 2 | leftandright == 3 | leftandright == 5 | leftandright == 7)
                {
                    calibrationinit = -aBuffer[4] + 135f;
                    stickviewxinit = -aBuffer[16] + 125f;
                    stickviewyinit = -aBuffer[17] + 125f;
                }
                if (leftandright == 1 | leftandright == 2 | leftandright == 4 | leftandright == 7)
                {
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
                }
                if (leftandright == 4 | leftandright == 5 | leftandright == 6 | leftandright == 7)
                {
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
                }
                if (leftandright == 4 | leftandright == 7)
                {
                    acc_gcalibrationX = (int)(avg((Int16)((report_bufLeft[13 + 0 * 12] | ((report_bufLeft[14 + 0 * 12] << 8) & 0xff00)) + (report_bufRight[13 + 0 * 12] | ((report_bufRight[14 + 0 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[13 + 1 * 12] | ((report_bufLeft[14 + 1 * 12] << 8) & 0xff00)) + (report_bufRight[13 + 1 * 12] | ((report_bufRight[14 + 1 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[13 + 2 * 12] | ((report_bufLeft[14 + 2 * 12] << 8) & 0xff00)) + (report_bufRight[13 + 2 * 12] | ((report_bufRight[14 + 2 * 12] << 8) & 0xff00))) / 2f)) * (1.0f / 4000f);
                    acc_gcalibrationY = (int)(avg((Int16)((report_bufLeft[15 + 0 * 12] | ((report_bufLeft[16 + 0 * 12] << 8) & 0xff00)) - (report_bufRight[15 + 0 * 12] | ((report_bufRight[16 + 0 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[15 + 1 * 12] | ((report_bufLeft[16 + 1 * 12] << 8) & 0xff00)) - (report_bufRight[15 + 1 * 12] | ((report_bufRight[16 + 1 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[15 + 2 * 12] | ((report_bufLeft[16 + 2 * 12] << 8) & 0xff00)) - (report_bufRight[15 + 2 * 12] | ((report_bufRight[16 + 2 * 12] << 8) & 0xff00))) / 2f)) * (1.0f / 4000f);
                    acc_gcalibrationZ = (int)(avg((Int16)((report_bufLeft[17 + 0 * 12] | ((report_bufLeft[18 + 0 * 12] << 8) & 0xff00)) - (report_bufRight[17 + 0 * 12] | ((report_bufRight[18 + 0 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[17 + 1 * 12] | ((report_bufLeft[18 + 1 * 12] << 8) & 0xff00)) - (report_bufRight[17 + 1 * 12] | ((report_bufRight[18 + 1 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[17 + 2 * 12] | ((report_bufLeft[18 + 2 * 12] << 8) & 0xff00)) - (report_bufRight[17 + 2 * 12] | ((report_bufRight[18 + 2 * 12] << 8) & 0xff00))) / 2f)) * (1.0f / 4000f);
                    gyr_gcalibrationX = (int)(avg((int)((Int16)((report_bufLeft[19 + 0 * 12] | ((report_bufLeft[20 + 0 * 12] << 8) & 0xff00)) + (report_bufRight[19 + 0 * 12] | ((report_bufRight[20 + 0 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[19 + 1 * 12] | ((report_bufLeft[20 + 1 * 12] << 8) & 0xff00)) + (report_bufRight[19 + 1 * 12] | ((report_bufRight[20 + 1 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[19 + 2 * 12] | ((report_bufLeft[20 + 2 * 12] << 8) & 0xff00)) + (report_bufRight[19 + 2 * 12] | ((report_bufRight[20 + 2 * 12] << 8) & 0xff00))) / (2f)))) * (1.0f / 600000f);
                    gyr_gcalibrationY = (int)(avg((int)((Int16)((report_bufLeft[21 + 0 * 12] | ((report_bufLeft[22 + 0 * 12] << 8) & 0xff00)) - (report_bufRight[21 + 0 * 12] | ((report_bufRight[22 + 0 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[21 + 1 * 12] | ((report_bufLeft[22 + 1 * 12] << 8) & 0xff00)) - (report_bufRight[21 + 1 * 12] | ((report_bufRight[22 + 1 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[21 + 2 * 12] | ((report_bufLeft[22 + 2 * 12] << 8) & 0xff00)) - (report_bufRight[21 + 2 * 12] | ((report_bufRight[22 + 2 * 12] << 8) & 0xff00))) / (2f)))) * (1.0f / 600000f);
                    gyr_gcalibrationZ = (int)(avg((int)((Int16)((report_bufLeft[23 + 0 * 12] | ((report_bufLeft[24 + 0 * 12] << 8) & 0xff00)) - (report_bufRight[23 + 0 * 12] | ((report_bufRight[24 + 0 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[23 + 1 * 12] | ((report_bufLeft[24 + 1 * 12] << 8) & 0xff00)) - (report_bufRight[23 + 1 * 12] | ((report_bufRight[24 + 1 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[23 + 2 * 12] | ((report_bufLeft[24 + 2 * 12] << 8) & 0xff00)) - (report_bufRight[23 + 2 * 12] | ((report_bufRight[24 + 2 * 12] << 8) & 0xff00))) / (2f)))) * (1.0f / 600000f);
                }
                form.BackColor = System.Drawing.Color.WhiteSmoke;
                form.toolStripLabel1.Text = "Ready";
                form.button1.Enabled = true;
                form.toolStripButton5.Enabled = true;
                form.toolStripButton6.Enabled = true;
            }
        }
        private double avg(double val1, double val2, double val3)
        {
            return (new double[] { val1, val2, val3 }).Average();
        }
        private void WiiJoy_thrM()
        {
            try
            {
                mouseprogram.InvokeMember("Open", BindingFlags.Default | BindingFlags.InvokeMethod, null, mouseobj, new object[] { });
            }
            catch { }
            for (; ; )
            {
                if (endinvoke | !Getstate)
                {
                    try
                    {
                        mouseprogram.InvokeMember("Close", BindingFlags.Default | BindingFlags.InvokeMethod, null, mouseobj, new object[] { });
                    }
                    catch { }
                    return;
                }
                watchM2 = (double)diffM.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
                watchM = (watchM2 - watchM1) / 1000f;
                watchM1 = watchM2;
                asynctaskM();
            }
        }
        private void WiiJoy_thrK()
        {
            try
            {
                keyboardprogram.InvokeMember("Open", BindingFlags.Default | BindingFlags.InvokeMethod, null, keyboardobj, new object[] { });
            }
            catch { }
            for (; ; )
            {
                if (endinvoke | !Getstate)
                {
                    try
                    {
                        keyboardprogram.InvokeMember("Close", BindingFlags.Default | BindingFlags.InvokeMethod, null, keyboardobj, new object[] { });
                    }
                    catch { }
                    return;
                }
                watchK2 = (double)diffK.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
                watchK = (watchK2 - watchK1) / 1000f;
                watchK1 = watchK2;
                asynctaskK();
            }
        }
        public void asynctaskM()
        {
            try
            {
                mouseprogram.InvokeMember("Main", BindingFlags.Default | BindingFlags.InvokeMethod, null, mouseobj, new object[] { mWSNunchuckStateRawJoystickX, mWSNunchuckStateRawJoystickY, mWSNunchuckStateRawValuesX, mWSNunchuckStateRawValuesY, mWSNunchuckStateRawValuesZ, mWSNunchuckStateC, mWSNunchuckStateZ, mWSButtonStateIRX, mWSButtonStateIRY, mWSButtonStateA, mWSButtonStateB, mWSButtonStateMinus, mWSButtonStateHome, mWSButtonStatePlus, mWSButtonStateOne, mWSButtonStateTwo, mWSButtonStateUp, mWSButtonStateDown, mWSButtonStateLeft, mWSButtonStateRight, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, EulerAngles.X, EulerAngles.Y, EulerAngles.Z, DirectAngles.X, DirectAngles.Y, DirectAngles.Z, camx, camy, EulerAnglesLeft.X, EulerAnglesLeft.Y, EulerAnglesLeft.Z, DirectAnglesLeft.X, DirectAnglesLeft.Y, DirectAnglesLeft.Z, EulerAnglesRight.X, EulerAnglesRight.Y, EulerAnglesRight.Z, DirectAnglesRight.X, DirectAnglesRight.Y, DirectAnglesRight.Z, LeftButtonSHOULDER_1, LeftButtonMINUS, LeftButtonCAPTURE, LeftButtonDPAD_UP, LeftButtonDPAD_LEFT, LeftButtonDPAD_DOWN, LeftButtonDPAD_RIGHT, LeftButtonSTICK, RightButtonDPAD_DOWN, LeftButtonSL, LeftButtonSR, GetStickLeft()[0], GetStickLeft()[1], RightButtonPLUS, RightButtonDPAD_RIGHT, RightButtonHOME, RightButtonSHOULDER_1, RightButtonDPAD_LEFT, RightButtonDPAD_UP, RightButtonSTICK, RightButtonSL, RightButtonSR, RightButtonSHOULDER_2, LeftButtonSHOULDER_2, GetStickRight()[0], GetStickRight()[1], GetAccel().X, GetAccel().Y, GetAccel().Z, GetAccelRight().X, GetAccelRight().Y, GetAccelRight().Z, GetAccelLeft().X, GetAccelLeft().Y, GetAccelLeft().Z, MouseHookX, MouseHookY, MouseHookWheelM, MouseHookLeftButton, MouseHookRightButton, MouseHookDoubleClick, MouseHookMiddleButton, watchM });
            }
            catch { }
        }
        public void asynctaskK()
        {
            try
            {
                keyboardprogram.InvokeMember("Main", BindingFlags.Default | BindingFlags.InvokeMethod, null, keyboardobj, new object[] { mWSNunchuckStateRawJoystickX, mWSNunchuckStateRawJoystickY, mWSNunchuckStateRawValuesX, mWSNunchuckStateRawValuesY, mWSNunchuckStateRawValuesZ, mWSNunchuckStateC, mWSNunchuckStateZ, mWSButtonStateIRX, mWSButtonStateIRY, mWSButtonStateA, mWSButtonStateB, mWSButtonStateMinus, mWSButtonStateHome, mWSButtonStatePlus, mWSButtonStateOne, mWSButtonStateTwo, mWSButtonStateUp, mWSButtonStateDown, mWSButtonStateLeft, mWSButtonStateRight, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, EulerAngles.X, EulerAngles.Y, EulerAngles.Z, DirectAngles.X, DirectAngles.Y, DirectAngles.Z, camx, camy, EulerAnglesLeft.X, EulerAnglesLeft.Y, EulerAnglesLeft.Z, DirectAnglesLeft.X, DirectAnglesLeft.Y, DirectAnglesLeft.Z, EulerAnglesRight.X, EulerAnglesRight.Y, EulerAnglesRight.Z, DirectAnglesRight.X, DirectAnglesRight.Y, DirectAnglesRight.Z, LeftButtonSHOULDER_1, LeftButtonMINUS, LeftButtonCAPTURE, LeftButtonDPAD_UP, LeftButtonDPAD_LEFT, LeftButtonDPAD_DOWN, LeftButtonDPAD_RIGHT, LeftButtonSTICK, RightButtonDPAD_DOWN, LeftButtonSL, LeftButtonSR, GetStickLeft()[0], GetStickLeft()[1], RightButtonPLUS, RightButtonDPAD_RIGHT, RightButtonHOME, RightButtonSHOULDER_1, RightButtonDPAD_LEFT, RightButtonDPAD_UP, RightButtonSTICK, RightButtonSL, RightButtonSR, RightButtonSHOULDER_2, LeftButtonSHOULDER_2, GetStickRight()[0], GetStickRight()[1], GetAccel().X, GetAccel().Y, GetAccel().Z, GetAccelRight().X, GetAccelRight().Y, GetAccelRight().Z, GetAccelLeft().X, GetAccelLeft().Y, GetAccelLeft().Z, MouseHookX, MouseHookY, MouseHookWheelK, MouseHookLeftButton, MouseHookRightButton, MouseHookDoubleClick, MouseHookMiddleButton, watchK });
            }
            catch { }
        }
        public void Start()
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            FinalFrame = new VideoCaptureDevice(CaptureDevice[0].MonikerString);
            videoCapabilities = FinalFrame.VideoCapabilities;
            FinalFrame.VideoResolution = videoCapabilities[videoCapabilities.Length - 1];
            FinalFrame.SetCameraProperty(CameraControlProperty.Zoom, 0, CameraControlFlags.Manual);
            FinalFrame.SetCameraProperty(CameraControlProperty.Focus, 0, CameraControlFlags.Manual);
            FinalFrame.SetCameraProperty(CameraControlProperty.Exposure, 0, CameraControlFlags.Manual);
            FinalFrame.SetCameraProperty(CameraControlProperty.Iris, 0, CameraControlFlags.Manual);
            FinalFrame.SetCameraProperty(CameraControlProperty.Pan, 0, CameraControlFlags.Manual);
            FinalFrame.NewFrame += FinalFrame_NewFrame;
            FinalFrame.Start();
        }
        void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            typeof(dataClass).InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objdata, new object[] { eventArgs });
        }
        public class dataClass
        {
            public void Data(NewFrameEventArgs eventArgs)
            {
                ReceiveRawRight(eventArgs);
            }
            private void ReceiveRawRight(NewFrameEventArgs eventArgs)
            {
                img = (Bitmap)eventArgs.Frame.Clone();
                brightnessfilter.ApplyInPlace(img);
                colorfilter.Red = new IntRange(0, 255);
                colorfilter.Green = new IntRange(205, 255);
                colorfilter.Blue = new IntRange(205, 255);
                colorfilter.ApplyInPlace(img);
                brightnessfilter.ApplyInPlace(img);
                euclideanfilter.CenterColor = new RGB(255, 255, 255);
                euclideanfilter.Radius = 175;
                euclideanfilter.ApplyInPlace(img);
                blobCounter.ProcessImage(img);
                blobs = blobCounter.GetObjectsInformation();
                for (int i = 0; i < blobs.Length; i++)
                {
                    shapeChecker.RelativeDistortionLimit = 100f;
                    shapeChecker.MinAcceptableDistortion = 20f;
                    if (shapeChecker.IsCircle(blobCounter.GetBlobsEdgePoints(blobs[i])))
                    {
                        backpointX = blobs[0].CenterOfGravity.X;
                        backpointY = blobs[0].CenterOfGravity.Y;
                    }
                }
                EditableImg = new Bitmap(img);
                EditableImg.MakeTransparent();
                DrawLines(ref EditableImg, new System.Drawing.Point((int)backpointX, (int)backpointY));
                try
                {
                    form2.SetPictureBox(EditableImg);
                }
                catch { }
                posRightX = backpointX - img.Width / 2;
                posRightY = backpointY - img.Height / 2;
                BackpointAnglesX = posRightX - InitBackpointAnglesX;
                BackpointAnglesY = posRightY - InitBackpointAnglesY;
                if (!Getstate)
                {
                    InitBackpointAnglesX = posRightX;
                    InitBackpointAnglesY = posRightY;
                }
                camx = BackpointAnglesX;
                camy = BackpointAnglesY;
            }
            public void DrawLines(ref Bitmap image, System.Drawing.Point p)
            {
                Graphics g = Graphics.FromImage(image);
                Pen p1 = new Pen(Color.Red, 2);
                System.Drawing.Point ph = new System.Drawing.Point(image.Width, p.Y);
                System.Drawing.Point ph2 = new System.Drawing.Point(0, p.Y);
                g.DrawLine(p1, p, ph);
                g.DrawLine(p1, p, ph2);
                ph = new System.Drawing.Point(p.X, 0);
                ph2 = new System.Drawing.Point(p.X, image.Height);
                g.DrawLine(p1, p, ph);
                g.DrawLine(p1, p, ph2);
                g.Dispose();
            }
        }
        public void Setgetstate()
        {
            getstate = false;
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            valchanged(1, GetAsyncKeyState(System.Windows.Forms.Keys.NumPad0));
            if (wd[1] == 1 & !getstate)
            {
                getstate = true;
                try
                {
                    form2.Visible = true;
                }
                catch { }
            }
            else
            {
                if (wd[1] == 1 & getstate)
                {
                    getstate = false;
                    try
                    {
                        form2.Hide();
                    }
                    catch { }
                }
            }
            string str = "mWSNunchuckStateRawJoystickX : " + mWSNunchuckStateRawJoystickX + Environment.NewLine;
            str += "mWSNunchuckStateRawJoystickY : " + mWSNunchuckStateRawJoystickY + Environment.NewLine;
            str += "mWSNunchuckStateRawValuesX : " + mWSNunchuckStateRawValuesX + Environment.NewLine;
            str += "mWSNunchuckStateRawValuesY : " + mWSNunchuckStateRawValuesY + Environment.NewLine;
            str += "mWSNunchuckStateRawValuesZ : " + mWSNunchuckStateRawValuesZ + Environment.NewLine;
            str += "mWSNunchuckStateC : " + mWSNunchuckStateC + Environment.NewLine;
            str += "mWSNunchuckStateZ : " + mWSNunchuckStateZ + Environment.NewLine;
            str += "mWSButtonStateIRX : " + mWSButtonStateIRX + Environment.NewLine;
            str += "mWSButtonStateIRY : " + mWSButtonStateIRY + Environment.NewLine;
            str += "mWSButtonStateA : " + mWSButtonStateA + Environment.NewLine;
            str += "mWSButtonStateB : " + mWSButtonStateB + Environment.NewLine;
            str += "mWSButtonStateMinus : " + mWSButtonStateMinus + Environment.NewLine;
            str += "mWSButtonStateHome : " + mWSButtonStateHome + Environment.NewLine;
            str += "mWSButtonStatePlus : " + mWSButtonStatePlus + Environment.NewLine;
            str += "mWSButtonStateOne : " + mWSButtonStateOne + Environment.NewLine;
            str += "mWSButtonStateTwo : " + mWSButtonStateTwo + Environment.NewLine;
            str += "mWSButtonStateUp : " + mWSButtonStateUp + Environment.NewLine;
            str += "mWSButtonStateDown : " + mWSButtonStateDown + Environment.NewLine;
            str += "mWSButtonStateLeft : " + mWSButtonStateLeft + Environment.NewLine;
            str += "mWSButtonStateRight : " + mWSButtonStateRight + Environment.NewLine;
            str += "mWSRawValuesX : " + mWSRawValuesX + Environment.NewLine;
            str += "mWSRawValuesY : " + mWSRawValuesY + Environment.NewLine;
            str += "mWSRawValuesZ : " + mWSRawValuesZ + Environment.NewLine;
            str += "EulerAnglesX : " + EulerAngles.X + Environment.NewLine;
            str += "EulerAnglesY : " + EulerAngles.Y + Environment.NewLine;
            str += "EulerAnglesZ : " + EulerAngles.Z + Environment.NewLine;
            str += "DirectAnglesX : " + DirectAngles.X + Environment.NewLine;
            str += "DirectAnglesY : " + DirectAngles.Y + Environment.NewLine;
            str += "DirectAnglesZ : " + DirectAngles.Z + Environment.NewLine;
            str += "camx : " + camx + Environment.NewLine;
            str += "camy : " + camy + Environment.NewLine;
            str += "EulerAnglesLeftX : " + EulerAnglesLeft.X + Environment.NewLine;
            str += "EulerAnglesLeftY : " + EulerAnglesLeft.Y + Environment.NewLine;
            str += "EulerAnglesLeftZ : " + EulerAnglesLeft.Z + Environment.NewLine;
            str += "DirectAnglesLeftX : " + DirectAnglesLeft.X + Environment.NewLine;
            str += "DirectAnglesLeftY : " + DirectAnglesLeft.Y + Environment.NewLine;
            str += "DirectAnglesLeftZ : " + DirectAnglesLeft.Z + Environment.NewLine;
            str += "EulerAnglesRightX : " + EulerAnglesRight.X + Environment.NewLine;
            str += "EulerAnglesRightY : " + EulerAnglesRight.Y + Environment.NewLine;
            str += "EulerAnglesRightZ : " + EulerAnglesRight.Z + Environment.NewLine;
            str += "DirectAnglesRightX : " + DirectAnglesRight.X + Environment.NewLine;
            str += "DirectAnglesRightY : " + DirectAnglesRight.Y + Environment.NewLine;
            str += "DirectAnglesRightZ : " + DirectAnglesRight.Z + Environment.NewLine;
            str += "LeftButtonSHOULDER_1 : " + LeftButtonSHOULDER_1 + Environment.NewLine;
            str += "LeftButtonMINUS : " + LeftButtonMINUS + Environment.NewLine;
            try
            {
                form2.SetLabel1Text(str);
            }
            catch { }
            str = "LeftButtonCAPTURE : " + LeftButtonCAPTURE + Environment.NewLine;
            str += "LeftButtonDPAD_UP : " + LeftButtonDPAD_UP + Environment.NewLine;
            str += "LeftButtonDPAD_LEFT : " + LeftButtonDPAD_LEFT + Environment.NewLine;
            str += "LeftButtonDPAD_DOWN : " + LeftButtonDPAD_DOWN + Environment.NewLine;
            str += "LeftButtonDPAD_RIGHT : " + LeftButtonDPAD_RIGHT + Environment.NewLine;
            str += "LeftButtonSTICK : " + LeftButtonSTICK + Environment.NewLine;
            str += "RightButtonDPAD_DOWN : " + RightButtonDPAD_DOWN + Environment.NewLine;
            str += "LeftButtonSL : " + LeftButtonSL + Environment.NewLine;
            str += "LeftButtonSR : " + LeftButtonSR + Environment.NewLine;
            str += "GetStickLeftX : " + GetStickLeft()[0] + Environment.NewLine;
            str += "GetStickLeftY : " + GetStickLeft()[1] + Environment.NewLine;
            str += "RightButtonPLUS : " + RightButtonPLUS + Environment.NewLine;
            str += "RightButtonDPAD_RIGHT : " + RightButtonDPAD_RIGHT + Environment.NewLine;
            str += "RightButtonHOME : " + RightButtonHOME + Environment.NewLine;
            str += "RightButtonSHOULDER_1 : " + RightButtonSHOULDER_1 + Environment.NewLine;
            str += "RightButtonDPAD_LEFT : " + RightButtonDPAD_LEFT + Environment.NewLine;
            str += "RightButtonDPAD_UP : " + RightButtonDPAD_UP + Environment.NewLine;
            str += "RightButtonSTICK : " + RightButtonSTICK + Environment.NewLine;
            str += "RightButtonSL : " + RightButtonSL + Environment.NewLine;
            str += "RightButtonSR : " + RightButtonSR + Environment.NewLine;
            str += "RightButtonSHOULDER_2 : " + RightButtonSHOULDER_2 + Environment.NewLine;
            str += "LeftButtonSHOULDER_2 : " + LeftButtonSHOULDER_2 + Environment.NewLine;
            str += "GetStickRightX : " + GetStickRight()[0] + Environment.NewLine;
            str += "GetStickRightY : " + GetStickRight()[1] + Environment.NewLine;
            str += "GetAccelX : " + GetAccel().X + Environment.NewLine;
            str += "GetAccelY : " + GetAccel().Y + Environment.NewLine;
            str += "GetAccelZ : " + GetAccel().Z + Environment.NewLine;
            str += "GetAccelRightX : " + GetAccelRight().X + Environment.NewLine;
            str += "GetAccelRight.Y : " + GetAccelRight().Y + Environment.NewLine;
            str += "GetAccelRightZ : " + GetAccelRight().Z + Environment.NewLine;
            str += "GetAccelLeftX : " + GetAccelLeft().X + Environment.NewLine;
            str += "GetAccelLeftY : " + GetAccelLeft().Y + Environment.NewLine;
            str += "GetAccelLeftZ : " + GetAccelLeft().Z + Environment.NewLine;
            str += "MouseHookX : " + MouseHookX + Environment.NewLine;
            str += "MouseHookY : " + MouseHookY + Environment.NewLine;
            str += "MouseHookWheel : " + (int)((MouseHookWheelM + MouseHookWheelK) / 2) + Environment.NewLine;
            str += "MouseHookLeftButton : " + MouseHookLeftButton + Environment.NewLine;
            str += "MouseHookRightButton : " + MouseHookRightButton + Environment.NewLine;
            str += "MouseHookDoubleClick : " + MouseHookDoubleClick + Environment.NewLine;
            str += "MouseHookMiddleButton : " + MouseHookMiddleButton + Environment.NewLine;
            str += "watchM : " + watchM + Environment.NewLine;
            str += "watchK : " + watchK + Environment.NewLine;
            GetCursorPos(out MouseDesktopHookX, out MouseDesktopHookY);
            str += "GetCursorPos out X : " + MouseDesktopHookX + Environment.NewLine;
            str += "GetCursorPos out Y : " + MouseDesktopHookY;
            try 
            { 
                form2.SetLabel2Text(str);
            }
            catch { }
            MouseHookWheelM = 0;
            MouseHookWheelK = 0;
        }
        private async void timer1_Tick(object sender, EventArgs e)
        {
            await asynctaskSelection();
            if (!endinvoke)
            {
                if (leftandright == 1 | leftandright == 2 | leftandright == 4 | leftandright == 7)
                    await asynctaskReadClassLeft();
                if (leftandright == 4 | leftandright == 5 | leftandright == 6 | leftandright == 7)
                    await asynctaskReadClassRight();
                if (leftandright == 4 | leftandright == 7)
                    await asynctaskReadClassLeftRight();
            }
            if (notmouseover)
                form.richTextBox1.ScrollToCaret();
            notmouseover = true;
        }
        public async Task asynctaskSelection()
        {
            valchanged(0, GetAsyncKeyState(System.Windows.Forms.Keys.Decimal));
            if (wd[0] == 1 & !Getstate)
            {
                form.button1.Text = "Stop";
                Getstate = true;
                scriptUpdate();
            }
            else
            {
                if (wd[0] == 1 & Getstate)
                {
                    form.button1.Text = "Start";
                    Getstate = false;
                    form.BackColor = System.Drawing.Color.WhiteSmoke;
                    form.toolStripLabel1.Text = "Ready";
                }
            }
            if (camenabled)
                try
                {
                    if (FinalFrame.IsRunning != true)
                        Start();
                }
                catch { }
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        public async Task asynctaskReadClassLeft()
        {
            try
            {
                typeof(dataReadClassLeft).InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objdataReadLeft, new object[] { });
            }
            catch { }
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        public async Task asynctaskReadClassRight()
        {
            try
            {
                typeof(dataReadClassRight).InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objdataReadRight, new object[] { });
            }
            catch { }
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        public async Task asynctaskReadClassLeftRight()
        {
            try
            {
                typeof(dataReadClassLeftRight).InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objdataReadLeftRight, new object[] { });
            }
            catch { }
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        private void WiiJoy_thrDReadWiimote()
        {
            for (; ; )
            {
                if (endinvoke)
                    return;
                typeof(dataReadClassWiimote).InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objdataReadWiimote, new object[] { });
                Thread.Sleep(1);
            }
        }
        public class dataReadClassLeft
        {
            public void Data()
            {
                ReceiveRawLeft();
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
            private void ReceiveRawLeft()
            {
                if (!endinvoke)
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream("dataLeft", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, bufferSize: 4096, useAsync: true))
                    {
                        fs.ReadAsync(report_bufLeft, 0, 49);
                    };
                    ProcessButtonsAndStickLeft();
                    ExtractIMUValuesLeft();
                }
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
        }
        public class dataReadClassRight
        {
            public void Data()
            {
                ReceiveRawRight();
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
            private void ReceiveRawRight()
            {
                if (!endinvoke)
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream("dataRight", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, bufferSize: 4096, useAsync: true))
                    {
                        fs.ReadAsync(report_bufRight, 0, 49);
                    };
                    ProcessButtonsAndStickRight();
                    ExtractIMUValuesRight();
                }
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
        }
        public class dataReadClassLeftRight
        {
            public void Data()
            {
                ReceiveRaw();
            }
            public Quaternion GetVectora()
            {
                Vector3 v1 = new Vector3(j_a.X, i_a.X, k_a.X);
                Vector3 v2 = -(new Vector3(j_a.Z, i_a.Z, k_a.Z));
                return QuaternionLookRotation(v1, v2);
            }
            public Quaternion GetVectorb()
            {
                Vector3 v1 = new Vector3(j_b.X, i_b.X, k_b.X);
                Vector3 v2 = -(new Vector3(j_b.Z, i_b.Z, k_b.Z));
                return QuaternionLookRotation(v1, v2);
            }
            public Quaternion GetVectorc()
            {
                Vector3 v1 = new Vector3(j_c.X, i_c.X, k_c.X);
                Vector3 v2 = -(new Vector3(j_c.Z, i_c.Z, k_c.Z));
                return QuaternionLookRotation(v1, v2);
            }
            private static Quaternion QuaternionLookRotation(Vector3 forward, Vector3 up)
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
            public static Vector3 ToEulerAngles(Quaternion q)
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
            private void ReceiveRaw()
            {
                if (!endinvoke)
                {
                    ExtractIMUValues();
                }
            }
            private void ExtractIMUValues()
            {
                acc_g.X = (int)(average((Int16)((report_bufLeft[13 + 0 * 12] | ((report_bufLeft[14 + 0 * 12] << 8) & 0xff00)) + (report_bufRight[13 + 0 * 12] | ((report_bufRight[14 + 0 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[13 + 1 * 12] | ((report_bufLeft[14 + 1 * 12] << 8) & 0xff00)) + (report_bufRight[13 + 1 * 12] | ((report_bufRight[14 + 1 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[13 + 2 * 12] | ((report_bufLeft[14 + 2 * 12] << 8) & 0xff00)) + (report_bufRight[13 + 2 * 12] | ((report_bufRight[14 + 2 * 12] << 8) & 0xff00))) / 2f)) * (1.0f / 4000f) - acc_gcalibrationX;
                acc_g.Y = (int)(average((Int16)((report_bufLeft[15 + 0 * 12] | ((report_bufLeft[16 + 0 * 12] << 8) & 0xff00)) - (report_bufRight[15 + 0 * 12] | ((report_bufRight[16 + 0 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[15 + 1 * 12] | ((report_bufLeft[16 + 1 * 12] << 8) & 0xff00)) - (report_bufRight[15 + 1 * 12] | ((report_bufRight[16 + 1 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[15 + 2 * 12] | ((report_bufLeft[16 + 2 * 12] << 8) & 0xff00)) - (report_bufRight[15 + 2 * 12] | ((report_bufRight[16 + 2 * 12] << 8) & 0xff00))) / 2f)) * (1.0f / 4000f) - acc_gcalibrationY;
                acc_g.Z = (int)(average((Int16)((report_bufLeft[17 + 0 * 12] | ((report_bufLeft[18 + 0 * 12] << 8) & 0xff00)) - (report_bufRight[17 + 0 * 12] | ((report_bufRight[18 + 0 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[17 + 1 * 12] | ((report_bufLeft[18 + 1 * 12] << 8) & 0xff00)) - (report_bufRight[17 + 1 * 12] | ((report_bufRight[18 + 1 * 12] << 8) & 0xff00))) / 2f, (Int16)((report_bufLeft[17 + 2 * 12] | ((report_bufLeft[18 + 2 * 12] << 8) & 0xff00)) - (report_bufRight[17 + 2 * 12] | ((report_bufRight[18 + 2 * 12] << 8) & 0xff00))) / 2f)) * (1.0f / 4000f) - acc_gcalibrationZ;
                gyr_g.X = (int)(average((int)((Int16)((report_bufLeft[19 + 0 * 12] | ((report_bufLeft[20 + 0 * 12] << 8) & 0xff00)) + (report_bufRight[19 + 0 * 12] | ((report_bufRight[20 + 0 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[19 + 1 * 12] | ((report_bufLeft[20 + 1 * 12] << 8) & 0xff00)) + (report_bufRight[19 + 1 * 12] | ((report_bufRight[20 + 1 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[19 + 2 * 12] | ((report_bufLeft[20 + 2 * 12] << 8) & 0xff00)) + (report_bufRight[19 + 2 * 12] | ((report_bufRight[20 + 2 * 12] << 8) & 0xff00))) / (2f)))) * (1.0f / 600000f) - gyr_gcalibrationX;
                gyr_g.Y = (int)(average((int)((Int16)((report_bufLeft[21 + 0 * 12] | ((report_bufLeft[22 + 0 * 12] << 8) & 0xff00)) - (report_bufRight[21 + 0 * 12] | ((report_bufRight[22 + 0 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[21 + 1 * 12] | ((report_bufLeft[22 + 1 * 12] << 8) & 0xff00)) - (report_bufRight[21 + 1 * 12] | ((report_bufRight[22 + 1 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[21 + 2 * 12] | ((report_bufLeft[22 + 2 * 12] << 8) & 0xff00)) - (report_bufRight[21 + 2 * 12] | ((report_bufRight[22 + 2 * 12] << 8) & 0xff00))) / (2f)))) * (1.0f / 600000f) - gyr_gcalibrationY;
                gyr_g.Z = (int)(average((int)((Int16)((report_bufLeft[23 + 0 * 12] | ((report_bufLeft[24 + 0 * 12] << 8) & 0xff00)) - (report_bufRight[23 + 0 * 12] | ((report_bufRight[24 + 0 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[23 + 1 * 12] | ((report_bufLeft[24 + 1 * 12] << 8) & 0xff00)) - (report_bufRight[23 + 1 * 12] | ((report_bufRight[24 + 1 * 12] << 8) & 0xff00))) / (2f)), (int)((Int16)((report_bufLeft[23 + 2 * 12] | ((report_bufLeft[24 + 2 * 12] << 8) & 0xff00)) - (report_bufRight[23 + 2 * 12] | ((report_bufRight[24 + 2 * 12] << 8) & 0xff00))) / (2f)))) * (1.0f / 600000f) - gyr_gcalibrationZ;
                if (!Getstate)
                    InitDirectAngles = acc_g;
                DirectAngles = acc_g - InitDirectAngles;
                i_c = new Vector3(1, 0, 0);
                k_c = new Vector3(0, 0, 1);
                j_c.X = 0f;
                j_c.Y = 1f;
                i_b = new Vector3(1, 0, 0);
                k_b = new Vector3(0, 0, 1);
                j_b.Y = 1f;
                j_b.Z = 0f;
                i_a = new Vector3(1, 0, 0);
                j_a = new Vector3(0, 1, 0);
                k_a.Y = 0f;
                k_a.Z = 1f;
                if (EulerAnglesc.Y == 0f)
                    j_c = new Vector3(0, 1, 0);
                if (EulerAnglesb.X == 0f)
                    j_b = new Vector3(0, 1, 0);
                if (EulerAnglesa.Z == 0f)
                    k_a = new Vector3(0, 0, 1);
                if (!Getstate | LeftButtonCAPTURE | RightButtonHOME)
                {
                    j_c = new Vector3(0, 1, 0);
                    InitEulerAnglesc = ToEulerAngles(GetVectorc());
                    j_b = new Vector3(0, 1, 0);
                    InitEulerAnglesb = ToEulerAngles(GetVectorb());
                    k_a = new Vector3(0, 0, 1);
                    InitEulerAnglesa = ToEulerAngles(GetVectora());
                }
                j_c += Vector3.Cross(Vector3.Negate(gyr_g), j_c);
                j_c = Vector3.Normalize(j_c);
                EulerAnglesc = ToEulerAngles(GetVectorc()) - InitEulerAnglesc;
                j_b += Vector3.Cross(Vector3.Negate(gyr_g), j_b);
                j_b = Vector3.Normalize(j_b);
                EulerAnglesb = ToEulerAngles(GetVectorb()) - InitEulerAnglesb;
                k_a += Vector3.Cross(Vector3.Negate(gyr_g), k_a);
                k_a = Vector3.Normalize(k_a);
                EulerAnglesa = ToEulerAngles(GetVectora()) - InitEulerAnglesa;
                EulerAngles = new Vector3(EulerAnglesb.X, EulerAnglesc.Y, EulerAnglesa.Z);
            }
            private double average(double val1, double val2, double val3)
            {
                array = new double[] { val1, val2, val3 };
                return array.Average();
            }
        }
        public class dataReadClassWiimote
        {
            public void Data()
            {
                ReceiveRawWiimote();
            }
            private void ReceiveRawWiimote()
            {
                if (!endinvoke)
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream("dataWiimote", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, bufferSize: 4096, useAsync: true))
                    {
                        fs.ReadAsync(aBuffer, 0, 22);
                    };
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
                    if (mWSIR0foundcam & mWSIR1foundcam)
                    {
                        mWSIRSensorsX = (mWSIRSensors0Xcam + mWSIRSensors1Xcam) / 2f;
                        mWSIRSensorsY = (mWSIRSensors0Ycam + mWSIRSensors1Ycam) / 2f;
                    }
                    if (mWSIR0foundcam)
                    {
                        irx0 = 2 * mWSIRSensors0Xcam - mWSIRSensorsX;
                        iry0 = 2 * mWSIRSensors0Ycam - mWSIRSensorsY;
                    }
                    if (mWSIR1foundcam)
                    {
                        irx1 = 2 * mWSIRSensors1Xcam - mWSIRSensorsX;
                        iry1 = 2 * mWSIRSensors1Ycam - mWSIRSensorsY;
                    }
                    irxc = irx0 + irx1;
                    iryc = iry0 + iry1;
                    MyAngle = (iry3 - iry2) * (irx3 - irx2 >= 0 ? 1 : -1);
                    mWSButtonStateIRX = irxc + iryc * MyAngle / 200f;
                    mWSButtonStateIRY = (iryc - irxc * MyAngle / 400f) * 2f;
                }
            }
        }
        private void mouseHook_Hook(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            MouseHookX = mouseStruct.pt.x;
            MouseHookY = mouseStruct.pt.y;
            if (MouseHook.MouseMessages.WM_LBUTTONDOWN == (MouseHook.MouseMessages)Param)
                MouseHookLeftButton = true;
            if (MouseHook.MouseMessages.WM_LBUTTONUP == (MouseHook.MouseMessages)Param)
                MouseHookLeftButton = false;
            if (MouseHook.MouseMessages.WM_RBUTTONDOWN == (MouseHook.MouseMessages)Param)
                MouseHookRightButton = true;
            if (MouseHook.MouseMessages.WM_RBUTTONUP == (MouseHook.MouseMessages)Param)
                MouseHookRightButton = false;
            if (MouseHook.MouseMessages.WM_MBUTTONDOWN == (MouseHook.MouseMessages)Param)
                MouseHookMiddleButton = true;
            if (MouseHook.MouseMessages.WM_MBUTTONUP == (MouseHook.MouseMessages)Param)
                MouseHookMiddleButton = false;
            if (MouseHook.MouseMessages.WM_LBUTTONDBLCLK == (MouseHook.MouseMessages)Param)
                MouseHookDoubleClick = true;
            else
                MouseHookDoubleClick = false;
            if (MouseHook.MouseMessages.WM_MOUSEWHEEL == (MouseHook.MouseMessages)Param)
            {
                MouseHookWheelM = (int)mouseStruct.mouseData;
                MouseHookWheelK = (int)mouseStruct.mouseData;
            }
            else
            {
                MouseHookWheelM = 0;
                MouseHookWheelK = 0;
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            endinvoke = true;
            notpressing1and2 = true;
            Thread.Sleep(100);
            TimeEndPeriod(1);
            try
            {
                if (FinalFrame.IsRunning == true)
                    FinalFrame.Stop();
            }
            catch { }
            form3.closed();
            mouseHook.Hook -= new MouseHook.MouseHookCallback(mouseHook_Hook);
            mouseHook.Uninstall();
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
        private static double[] stickLeft = { 0, 0 };
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
        private static double[] stickRight = { 0, 0 };
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
        private static Vector3 gyr_g = new Vector3();
        private static Vector3 acc_g = new Vector3();
        public static Vector3 i_a = new Vector3(1, 0, 0);
        public static Vector3 j_a = new Vector3(0, 1, 0);
        public static Vector3 k_a = new Vector3(0, 0, 1);
        public static Vector3 i_b = new Vector3(1, 0, 0);
        public static Vector3 j_b = new Vector3(0, 1, 0);
        public static Vector3 k_b = new Vector3(0, 0, 1);
        public static Vector3 i_c = new Vector3(1, 0, 0);
        public static Vector3 j_c = new Vector3(0, 1, 0);
        public static Vector3 k_c = new Vector3(0, 0, 1);
        private static Vector3 InitDirectAngles, DirectAngles;
        private static Vector3 InitEulerAnglesa, EulerAnglesa, InitEulerAnglesb, EulerAnglesb, InitEulerAnglesc, EulerAnglesc, EulerAngles;
        private static double[] array;
        public static float acc_gcalibrationX, acc_gcalibrationY, acc_gcalibrationZ, gyr_gcalibrationX, gyr_gcalibrationY, gyr_gcalibrationZ;
        public Vector3 GetAccel()
        {
            return acc_g;
        }
    }
    class MouseHook
    {
        private delegate IntPtr MouseHookHandler(int nCode, IntPtr wParam, IntPtr lParam);
        private MouseHookHandler hookHandler;
        public delegate void MouseHookCallback(MSLLHOOKSTRUCT mouseStruct);
        public event MouseHookCallback LeftButtonDown;
        public event MouseHookCallback LeftButtonUp;
        public event MouseHookCallback RightButtonDown;
        public event MouseHookCallback RightButtonUp;
        public event MouseHookCallback MouseMove;
        public event MouseHookCallback MouseWheel;
        public event MouseHookCallback DoubleClick;
        public event MouseHookCallback MiddleButtonDown;
        public event MouseHookCallback MiddleButtonUp;
        public event MouseHookCallback Hook;
        private IntPtr hookID = IntPtr.Zero;
        public void Install()
        {
            hookHandler = HookFunc;
            hookID = SetHook(hookHandler);
        }
        public void Uninstall()
        {
            if (hookID == IntPtr.Zero)
                return;
            UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
        }
        ~MouseHook()
        {
            Uninstall();
        }
        private IntPtr SetHook(MouseHookHandler proc)
        {
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(module.ModuleName), 0);
        }
        private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                Form1.Param = wParam;
                if (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
                    if (LeftButtonDown != null)
                        LeftButtonDown((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                if (MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
                    if (LeftButtonUp != null)
                        LeftButtonUp((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                if (MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
                    if (RightButtonDown != null)
                        RightButtonDown((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                if (MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam)
                    if (RightButtonUp != null)
                        RightButtonUp((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
                    if (MouseMove != null)
                        MouseMove((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                if (MouseMessages.WM_MOUSEWHEEL == (MouseMessages)wParam)
                    if (MouseWheel != null)
                        MouseWheel((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                if (MouseMessages.WM_LBUTTONDBLCLK == (MouseMessages)wParam)
                    if (DoubleClick != null)
                        DoubleClick((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                if (MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam)
                    if (MiddleButtonDown != null)
                        MiddleButtonDown((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                if (MouseMessages.WM_MBUTTONUP == (MouseMessages)wParam)
                    if (MiddleButtonUp != null)
                        MiddleButtonUp((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                Hook((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }
        private const int WH_MOUSE_LL = 14;
        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, MouseHookHandler lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}