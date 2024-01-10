using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using controller;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
namespace WiiJoyX
{
    public partial class Form1 : Form
    {
        [DllImport("advapi32.dll")]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        [DllImport("WiimotePairing.dll", EntryPoint = "connect")]
        private static extern bool connect();
        [DllImport("WiimotePairing.dll", EntryPoint = "disconnect")]
        private static extern bool disconnect();
        [DllImport("RightJoyconPairing.dll", EntryPoint = "rconnect")]
        private static extern bool rconnect();
        [DllImport("RightJoyconPairing.dll", EntryPoint = "disconnectRight")]
        private static extern bool disconnectRight();
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
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_read_timeout")]
        private static extern int Rhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_write")]
        private static extern int Rhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_open_path")]
        private static extern SafeFileHandle Rhid_open_path(IntPtr handle);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_close")]
        private static extern void Rhid_close(SafeFileHandle device);
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
        private const double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe;
        private static double irx0, iry0, irx1, iry1, irx, iry, irxc, iryc, mWSIRSensors0X, mWSIRSensors0Y, mWSIRSensors1X, mWSIRSensors1Y, mWSButtonStateIRX, mWSButtonStateIRY, mousex, mousey, mWSIRSensors0Xcam, mWSIRSensors0Ycam, mWSIRSensors1Xcam, mWSIRSensors1Ycam, mWSIRSensorsXcam, mWSIRSensorsYcam, viewpower05x, viewpower1x, viewpower2x = 1f, viewpower3x, viewpower05y, viewpower1y, viewpower2y = 1f, viewpower3y, dzx, dzy, lowsensx = 1f, lowsensy = 1f, switchviewpower05x, switchviewpower1x, switchviewpower2x = 1f, switchviewpower3x, switchviewpower05y, switchviewpower1y, switchviewpower2y = 1f, switchviewpower3y, switchdzx, switchdzy, switchlowsensx = 1f, switchlowsensy = 1f, centery = 160f, dzapressiox, dzapressioy, lowsensapressiox, lowsensapressioy, lowsenstime, lowsenstimeapressio, lowsensdiffx, lowsensdiffy, lowsensincx, lowsensincy, dzdiffx, dzdiffy, dzincx, dzincy, lowsensdiffapressiox, lowsensdiffapressioy, dzdiffapressiox, dzdiffapressioy, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, calibrationinit, stickviewxinit, stickviewyinit, mWSNunchuckStateRawValuesX, mWSNunchuckStateRawValuesY, mWSNunchuckStateRawValuesZ, mWSNunchuckStateRawJoystickX, mWSNunchuckStateRawJoystickY, rolling;
        private static bool mWSIR1found, mWSIR0found, mWSButtonStateA, mWSButtonStateB, mWSButtonStateMinus, mWSButtonStateHome, mWSButtonStatePlus, mWSButtonStateOne, mWSButtonStateTwo, mWSButtonStateUp, mWSButtonStateDown, mWSButtonStateLeft, mWSButtonStateRight, Getstate, running, apressio, foraorcison, mWSButtonStateAio, randA, RightButtonSPA, mWSRightButtonTH, LeftButtonSMA, mWSLeftButtonTC, mWSButtonStateHFront, mWSButtonStatePU, mWSButtonStateMU, mWSButtonStateLR, RightButtonACC, LeftButtonACC, mWSButtonStateFront, switchstate, rconnected, lconnected, connected, mWSNunchuckStateC, mWSNunchuckStateZ, LeftRollLeft, LeftRollRight, RightRollLeft, RightRollRight;
        private static byte[] buff = new byte[] { 0x55 }, mBuff = new byte[22], aBuffer = new byte[22];
        private const byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        private static uint CurrentResolution = 0;
        private static FileStream mStream;
        private static SafeFileHandle handle = null;
        private static ThreadStart threadstart;
        private static Thread thread;
        private static ScpBus scpBus;
        private static X360Controller controller;
        private static Form1 form = (Form1)Application.OpenForms["Form1"];
        private static System.Collections.Generic.List<double> valListX = new System.Collections.Generic.List<double>(), valListY = new System.Collections.Generic.List<double>(), valListZ = new System.Collections.Generic.List<double>(), LeftvalList = new System.Collections.Generic.List<double>(), RightvalList = new System.Collections.Generic.List<double>();
        private static Dictionary<string, bool> Fbool = new Dictionary<string, bool>(60);
        private static Dictionary<string, double> Fdouble = new Dictionary<string, double>(18);
        private static string xoutDown, xoutLeft, xoutRight, xoutUp, xoutRightStick, xoutLeftStick, xoutA, xoutBack, xoutStart, xoutX, xoutRightBumper, xoutLeftBumper, xoutB, xoutY, xoutRightTrigger, xoutLeftTrigger, xoutswitchDown, xoutswitchLeft, xoutswitchRight, xoutswitchUp, xoutswitchRightStick, xoutswitchLeftStick, xoutswitchA, xoutswitchBack, xoutswitchStart, xoutswitchX, xoutswitchRightBumper, xoutswitchLeftBumper, xoutswitchB, xoutswitchY, xoutswitchRightTrigger, xoutswitchLeftTrigger, xoutLeftStickRight, xoutLeftStickLeft, xoutLeftStickUp, xoutLeftStickDown, xoutRightStickRight, xoutRightStickLeft, xoutRightStickUp, xoutRightStickDown, xoutLeftStickX, xoutLeftStickY, xoutRightStickX, xoutRightStickY, xoutswitchLeftStickRight, xoutswitchLeftStickLeft, xoutswitchLeftStickUp, xoutswitchLeftStickDown, xoutswitchRightStickRight, xoutswitchRightStickLeft, xoutswitchRightStickUp, xoutswitchRightStickDown, xoutswitchLeftStickX, xoutswitchLeftStickY, xoutswitchRightStickX, xoutswitchRightStickY;
        private static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
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
            Process[] processes = Process.GetProcessesByName("WiiJoyX");
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
            Fbool.Add("RightButtonSHOULDER_1", false);
            Fbool.Add("RightButtonSHOULDER_2", false);
            Fbool.Add("RightButtonSR", false);
            Fbool.Add("RightButtonSL", false);
            Fbool.Add("RightButtonDPAD_DOWN", false);
            Fbool.Add("RightButtonDPAD_RIGHT", false);
            Fbool.Add("RightButtonDPAD_UP", false);
            Fbool.Add("RightButtonDPAD_LEFT", false);
            Fbool.Add("RightButtonPLUS", false);
            Fbool.Add("RightButtonHOME", false);
            Fbool.Add("RightButtonSTICK", false);
            Fbool.Add("RightButtonSPA", false);
            Fbool.Add("mWSRightButtonTH", false);
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
            Fbool.Add("RightButtonACC", false);
            Fbool.Add("RightButtonStickRight", false);
            Fbool.Add("RightButtonStickLeft", false);
            Fbool.Add("RightButtonStickUp", false);
            Fbool.Add("RightButtonStickDown", false);
            Fbool.Add("mWSNunchuckStateRollingLeft", false);
            Fbool.Add("mWSNunchuckStateRollingRight", false);
            Fbool.Add("mWSNunchuckStateRawValuesY", false);
            Fbool.Add("mWSNunchuckStateC", false);
            Fbool.Add("mWSNunchuckStateZ", false);
            Fbool.Add("mWSNunchuckStateStickRight", false);
            Fbool.Add("mWSNunchuckStateStickLeft", false);
            Fbool.Add("mWSNunchuckStateStickUp", false);
            Fbool.Add("mWSNunchuckStateStickDown", false);
            Fbool.Add("LeftRollLeft", false);
            Fbool.Add("LeftRollRight", false);
            Fbool.Add("RightRollLeft", false);
            Fbool.Add("RightRollRight", false);
            Fbool.Add("none", false);
            Fdouble.Add("mousex", 0);
            Fdouble.Add("mousey", 0);
            Fdouble.Add("leftstickx", 0);
            Fdouble.Add("leftsticky", 0);
            Fdouble.Add("rightstickx", 0);
            Fdouble.Add("rightsticky", 0);
            Fdouble.Add("stickx", 0);
            Fdouble.Add("sticky", 0);
            Fdouble.Add("none", 0);
        }
        private void Start()
        {
            scpBus = new ScpBus();
            scpBus.PlugIn(1);
            controller = new X360Controller();
            running = true;
            connectingWiiJoy();
            do 
            { 
                Thread.Sleep(1);
            } 
            while (!lconnected & !rconnected & !connected);
            Task.Run(() => taskX1());
            Task.Run(() => taskX2());
            this.WindowState = FormWindowState.Minimized;
        }
        private static void connectingWiiJoy()
        {
            Task.Run(() => connectingJoyconLeft());
            Task.Run(() => connectingJoyconRight());
            Task.Run(() => connectingWiimote());
        }
        private static void connectingJoyconLeft()
        {
            do
            {
                lconnected = lconnect();
                if (Getstate)
                    return;
                Thread.Sleep(1);
            }
            while (!lconnected);
            if (lconnected)
            {
                do
                    Thread.Sleep(1);
                while (!ScanLeft());
                Task.Run(() => taskDLeft());
                Thread.Sleep(3000);
                InitLeft();
            }
        }
        private static void connectingJoyconRight()
        {
            do
            {
                rconnected = rconnect();
                if (Getstate)
                    return;
                Thread.Sleep(1);
            }
            while (!rconnected);
            if (rconnected)
            {
                do
                    Thread.Sleep(1);
                while (!ScanRight());
                Task.Run(() => taskDRight());
                Thread.Sleep(3000);
                InitRight();
            }
        }
        private static void connectingWiimote()
        {
            do
            {
                connected = connect();
                if (Getstate)
                    return;
                Thread.Sleep(1);
            }
            while (!connected);
            if (connected)
            {
                do
                    Thread.Sleep(1);
                while (!ScanWiimote());
                Task.Run(() => taskD());
                Thread.Sleep(3000);
                Init();
            }
        }
        private static void Init()
        {
            calibrationinit = -aBuffer[4] + 135f;
            stickviewxinit = -aBuffer[16] + 125f;
            stickviewyinit = -aBuffer[17] + 125f;
        }
        private static void InitLeft()
        {
            stick_rawLeft[0] = report_bufLeft[6 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[1] = report_bufLeft[7 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[2] = report_bufLeft[8 + (ISLEFT ? 0 : 3)];
            stick_calibrationLeft[0] = (UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8));
            stick_calibrationLeft[1] = (UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4));
            acc_gcalibrationLeftX = (Int16)(report_bufLeft[13] | ((report_bufLeft[14] << 8) & 0xff00));
            acc_gcalibrationLeftY = (Int16)(report_bufLeft[15] | ((report_bufLeft[16] << 8) & 0xff00));
            acc_gcalibrationLeftZ = (Int16)(report_bufLeft[17] | ((report_bufLeft[18] << 8) & 0xff00));
            gyr_gcalibrationLeftX = (Int16)(report_bufLeft[19] | ((report_bufLeft[20] << 8) & 0xff00));
            gyr_gcalibrationLeftY = (Int16)(report_bufLeft[21] | ((report_bufLeft[22] << 8) & 0xff00));
            gyr_gcalibrationLeftZ = (Int16)(report_bufLeft[23] | ((report_bufLeft[24] << 8) & 0xff00));
        }
        private static void InitRight()
        {
            stick_rawRight[0] = report_bufRight[6 + (!ISRIGHT ? 0 : 3)];
            stick_rawRight[1] = report_bufRight[7 + (!ISRIGHT ? 0 : 3)];
            stick_rawRight[2] = report_bufRight[8 + (!ISRIGHT ? 0 : 3)];
            stick_calibrationRight[0] = (UInt16)(stick_rawRight[0] | ((stick_rawRight[1] & 0xf) << 8));
            stick_calibrationRight[1] = (UInt16)((stick_rawRight[1] >> 4) | (stick_rawRight[2] << 4));
            acc_gcalibrationRightX = (Int16)(report_bufRight[13] | ((report_bufRight[14] << 8) & 0xff00));
            acc_gcalibrationRightY = (Int16)(report_bufRight[15] | ((report_bufRight[16] << 8) & 0xff00));
            acc_gcalibrationRightZ = (Int16)(report_bufRight[17] | ((report_bufRight[18] << 8) & 0xff00));
            gyr_gcalibrationRightX = (Int16)(report_bufRight[19] | ((report_bufRight[20] << 8) & 0xff00));
            gyr_gcalibrationRightY = (Int16)(report_bufRight[21] | ((report_bufRight[22] << 8) & 0xff00));
            gyr_gcalibrationRightZ = (Int16)(report_bufRight[23] | ((report_bufRight[24] << 8) & 0xff00));
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
                    if (!switchstate)
                    {
                        if (connected)
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
                        }
                        if (lconnected & !rconnected & !connected)
                        {
                            if (valListX.Count >= 50)
                            {
                                valListX.RemoveAt(0);
                                valListX.Add(DirectAnglesLeft.X * 1350f);
                            }
                            else
                                valListX.Add(DirectAnglesLeft.X * 1350f);
                            if (valListY.Count >= 50)
                            {
                                valListY.RemoveAt(0);
                                valListY.Add(-(DirectAnglesLeft.Y * 1350f / 0.5f));
                            }
                            else
                                valListY.Add(-(DirectAnglesLeft.Y * 1350f / 0.5f));
                            irx = valListX.Average();
                            iry = valListY.Average();
                        }
                        if (rconnected & !lconnected & !connected)
                        {
                            if (valListX.Count >= 50)
                            {
                                valListX.RemoveAt(0);
                                valListX.Add(DirectAnglesRight.X * 1350f);
                            }
                            else
                                valListX.Add(DirectAnglesRight.X * 1350f);
                            if (valListY.Count >= 50)
                            {
                                valListY.RemoveAt(0);
                                valListY.Add(-(DirectAnglesRight.Y * 1350f / 0.5f));
                            }
                            else
                                valListY.Add(-(DirectAnglesRight.Y * 1350f / 0.5f));
                            irx = valListX.Average();
                            iry = valListY.Average();
                        }
                        if (rconnected & lconnected & !connected)
                        {
                            irx = -EulerAnglesRight.X * 1024f * 180f - GetStickRight()[0] * 1500f;
                            if (valListY.Count >= 30)
                            {
                                valListY.RemoveAt(0);
                                valListY.Add(-DirectAnglesRight.X * 4800f);
                            }
                            else
                                valListY.Add(-DirectAnglesRight.X * 4800f);
                            iry = valListY.Average() - GetStickRight()[1] * 1500f;
                        }
                        if (irx >= 1024f)
                            irx = 1024f;
                        if (irx <= -1024f)
                            irx = -1024f;
                        if (iry >= 1024f)
                            iry = 1024f;
                        if (iry <= -1024f)
                            iry = -1024f;
                        if (Fbool[xoutRightTrigger])
                            controller.RightTrigger = 255;
                        else
                            controller.RightTrigger = 0;
                        if (!apressio)
                        {
                            if (Fbool[xoutLeftTrigger])
                                controller.LeftTrigger = 255;
                            else
                                controller.LeftTrigger = 0;
                            changeScale(dzx, dzy);
                            changeView(lowsensx, lowsensy);
                        }
                        else
                        {
                            foraorcison = mWSButtonStateMinus | mWSButtonStatePlus | mWSButtonStateHome | ((mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 40f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 40f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 40f) | mWSButtonStateUp | mWSButtonStateDown | mWSButtonStateLeft | mWSButtonStateRight;
                            valchanged(17, Fbool[xoutLeftTrigger]);
                            if (wd[17] == 1)
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
                            if (mWSButtonStateAio | (Fbool[xoutLeftTrigger] & foraorcison))
                                controller.LeftTrigger = 255;
                            else
                                controller.LeftTrigger = 0;
                            if (mWSButtonStateAio)
                            {
                                if (lowsensincx > lowsensx)
                                    lowsensincx -= lowsensdiffx;
                                else
                                    lowsensincx = lowsensx;
                                if (lowsensincy > lowsensy)
                                    lowsensincy -= lowsensdiffy;
                                else
                                    lowsensincy = lowsensy;
                                if (dzincx > dzx)
                                    dzincx -= dzdiffx;
                                else
                                    dzincx = dzx;
                                if (dzincy > dzy)
                                    dzincy -= dzdiffy;
                                else
                                    dzincy = dzy;
                            }
                            else
                            {
                                if (lowsensincx < lowsensapressiox)
                                    lowsensincx += lowsensdiffapressiox;
                                else
                                    lowsensincx = lowsensapressiox;
                                if (lowsensincy < lowsensapressioy)
                                    lowsensincy += lowsensdiffapressioy;
                                else
                                    lowsensincy = lowsensapressioy;
                                if (dzincx < dzapressiox)
                                    dzincx += dzdiffapressiox;
                                else
                                    dzincx = dzapressiox;
                                if (dzincy < dzapressioy)
                                    dzincy += dzdiffapressioy;
                                else
                                    dzincy = dzapressioy;
                            }
                            changeScale(dzincx, dzincy);
                            changeView(lowsensincx, lowsensincy);
                        }
                    }
                    else
                    {
                        if (lconnected & !rconnected & connected)
                        {
                            acc_gLeft.X = ((Int16)(report_bufLeft[13] | ((report_bufLeft[14] << 8) & 0xff00)) - acc_gcalibrationLeftX) * (1.0f / 4000f);
                            acc_gLeft.Y = ((Int16)(report_bufLeft[15] | ((report_bufLeft[16] << 8) & 0xff00)) - acc_gcalibrationLeftY) * (1.0f / 4000f);
                            acc_gLeft.Z = ((Int16)(report_bufLeft[17] | ((report_bufLeft[18] << 8) & 0xff00)) - acc_gcalibrationLeftZ) * (1.0f / 4000f);
                            DirectAnglesLeft = acc_gLeft - InitDirectAnglesLeft;
                            if (valListX.Count >= 50)
                            {
                                valListX.RemoveAt(0);
                                valListX.Add(DirectAnglesLeft.X * 1350f);
                            }
                            else
                                valListX.Add(DirectAnglesLeft.X * 1350f);
                            if (valListY.Count >= 50)
                            {
                                valListY.RemoveAt(0);
                                valListY.Add(-(DirectAnglesLeft.Y * 1350f / 0.5f));
                            }
                            else
                                valListY.Add(-(DirectAnglesLeft.Y * 1350f / 0.5f));
                            irx = valListX.Average();
                            iry = valListY.Average();
                        }
                        if (rconnected & !lconnected & connected)
                        {
                            acc_gRight.X = ((Int16)(report_bufRight[13] | ((report_bufRight[14] << 8) & 0xff00)) - acc_gcalibrationRightX) * (1.0f / 4000f);
                            acc_gRight.Y = -((Int16)(report_bufRight[15] | ((report_bufRight[16] << 8) & 0xff00)) - acc_gcalibrationRightY) * (1.0f / 4000f);
                            acc_gRight.Z = -((Int16)(report_bufRight[17] | ((report_bufRight[18] << 8) & 0xff00)) - acc_gcalibrationRightZ) * (1.0f / 4000f);
                            DirectAnglesRight = acc_gRight - InitDirectAnglesRight;
                            if (valListX.Count >= 50)
                            {
                                valListX.RemoveAt(0);
                                valListX.Add(DirectAnglesRight.X * 1350f);
                            }
                            else
                                valListX.Add(DirectAnglesRight.X * 1350f);
                            if (valListY.Count >= 50)
                            {
                                valListY.RemoveAt(0);
                                valListY.Add(-(DirectAnglesRight.Y * 1350f / 0.5f));
                            }
                            else
                                valListY.Add(-(DirectAnglesRight.Y * 1350f / 0.5f));
                            irx = valListX.Average();
                            iry = valListY.Average();
                        }
                        if (!lconnected & !rconnected & connected)
                        {
                            if (valListX.Count >= 60)
                            {
                                valListX.RemoveAt(0);
                                valListX.Add(mWSRawValuesY * 45f);
                                irx = valListX.Average();
                            }
                            else
                            {
                                valListX.Add(mWSRawValuesY * 45f);
                            }
                            if (valListY.Count >= 60)
                            {
                                valListY.RemoveAt(0);
                                valListY.Add(mWSRawValuesX * 45f / 0.5f);
                                iry = valListY.Average();
                            }
                            else
                            {
                                valListY.Add(mWSRawValuesX * 45f / 0.5f);
                            }
                        }
                        if (irx >= 1024f)
                            irx = 1024f;
                        if (irx <= -1024f)
                            irx = -1024f;
                        if (iry >= 1024f)
                            iry = 1024f;
                        if (iry <= -1024f)
                            iry = -1024f;
                        if (irx > 0f)
                            mousex = Scale(Math.Pow(irx, 3f) / Math.Pow(1024f, 2f) * switchviewpower3x + Math.Pow(irx, 2f) / Math.Pow(1024f, 1f) * switchviewpower2x + Math.Pow(irx, 1f) / Math.Pow(1024f, 0f) * switchviewpower1x + Math.Pow(irx, 0.5f) * Math.Pow(1024f, 0.5f) * switchviewpower05x, 0f, 1024f, (switchdzx / 100f) * 1024f, 1024f);
                        if (irx < 0f)
                            mousex = Scale(-Math.Pow(-irx, 3f) / Math.Pow(1024f, 2f) * switchviewpower3x - Math.Pow(-irx, 2f) / Math.Pow(1024f, 1f) * switchviewpower2x - Math.Pow(-irx, 1f) / Math.Pow(1024f, 0f) * switchviewpower1x - Math.Pow(-irx, 0.5f) * Math.Pow(1024f, 0.5f) * switchviewpower05x, -1024f, 0f, -1024f, -(switchdzx / 100f) * 1024f);
                        if (iry > 0f)
                            mousey = Scale(Math.Pow(iry, 3f) / Math.Pow(1024f, 2f) * switchviewpower3y + Math.Pow(iry, 2f) / Math.Pow(1024f, 1f) * switchviewpower2y + Math.Pow(iry, 1f) / Math.Pow(1024f, 0f) * switchviewpower1y + Math.Pow(iry, 0.5f) * Math.Pow(1024f, 0.5f) * switchviewpower05y, 0f, 1024f, (switchdzy / 100f) * 1024f, 1024f);
                        if (iry < 0f)
                            mousey = Scale(-Math.Pow(-iry, 3f) / Math.Pow(1024f, 2f) * switchviewpower3y - Math.Pow(-iry, 2f) / Math.Pow(1024f, 1f) * switchviewpower2y - Math.Pow(-iry, 1f) / Math.Pow(1024f, 0f) * switchviewpower1y - Math.Pow(-iry, 0.5f) * Math.Pow(1024f, 0.5f) * switchviewpower05y, -1024f, 0f, -1024f, -(switchdzy / 100f) * 1024f);
                        if (connected)
                        {
                            Fdouble["mousex"] = mousex / 1024f / switchlowsensx;
                            Fdouble["mousey"] = mousey / 1024f / switchlowsensy;
                            Fdouble["stickx"] = Math.Abs(mWSNunchuckStateRawJoystickX * 600f / 32767f) <= 1f ? mWSNunchuckStateRawJoystickX * 600f / 32767f : Math.Sign(mWSNunchuckStateRawJoystickX * 600f / 32767f);
                            Fdouble["sticky"] = Math.Abs(mWSNunchuckStateRawJoystickY * 600f / 32767f) <= 1f ? mWSNunchuckStateRawJoystickY * 600f / 32767f : Math.Sign(mWSNunchuckStateRawJoystickY * 600f / 32767f);
                        }
                        if (lconnected & !rconnected & connected)
                        {
                            Fdouble["leftstickx"] = Math.Abs(-GetStickLeft()[1] / 1024f * 1660f) <= 1f ? -GetStickLeft()[1] / 1024f * 1660f : Math.Sign(-GetStickLeft()[1] / 1024f * 1660f);
                            Fdouble["leftsticky"] = Math.Abs(-GetStickLeft()[0] / 1024f * 1660f) <= 1f ? -GetStickLeft()[0] / 1024f * 1660f : Math.Sign(-GetStickLeft()[0] / 1024f * 1660f);
                        }
                        if (rconnected & !lconnected & connected)
                        {
                            Fdouble["rightstickx"] = Math.Abs(-GetStickRight()[1] / 1024f * 1660f) <= 1f ? -GetStickRight()[1] / 1024f * 1660f : Math.Sign(-GetStickRight()[1] / 1024f * 1660f);
                            Fdouble["rightsticky"] = Math.Abs(-GetStickRight()[0] / 1024f * 1660f) <= 1f ? -GetStickRight()[0] / 1024f * 1660f : Math.Sign(-GetStickRight()[0] / 1024f * 1660f);
                        }
                        if (xoutswitchLeftStickX != "none")
                            controller.LeftStickX = (short)(Fdouble[xoutswitchLeftStickX] * 32767f);
                        else
                        {
                            if (Fbool[xoutswitchLeftStickRight] & !Fbool[xoutswitchLeftStickLeft])
                                controller.LeftStickX = 32767;
                            else
                            if (!Fbool[xoutswitchLeftStickLeft])
                                controller.LeftStickX = 0;
                            if (Fbool[xoutswitchLeftStickLeft] & !Fbool[xoutswitchLeftStickRight])
                                controller.LeftStickX = -32767;
                            else
                            if (!Fbool[xoutswitchLeftStickRight])
                                controller.LeftStickX = 0;
                        }
                        if (xoutswitchLeftStickY != "none")
                            controller.LeftStickY = (short)(Fdouble[xoutswitchLeftStickY] * 32767f);
                        else
                        {
                            if (Fbool[xoutswitchLeftStickUp] & !Fbool[xoutswitchLeftStickDown])
                                controller.LeftStickY = 32767;
                            else
                            if (!Fbool[xoutswitchLeftStickDown])
                                controller.LeftStickY = 0;
                            if (Fbool[xoutswitchLeftStickDown] & !Fbool[xoutswitchLeftStickUp])
                                controller.LeftStickY = -32767;
                            else
                            if (!Fbool[xoutswitchLeftStickUp])
                                controller.LeftStickY = 0;
                        }
                        if (xoutswitchRightStickX != "none")
                            controller.RightStickX = (short)(Fdouble[xoutswitchRightStickX] * 32767f);
                        else
                        {
                            if (Fbool[xoutswitchRightStickRight] & !Fbool[xoutswitchRightStickLeft])
                                controller.RightStickX = 32767;
                            else
                            if (!Fbool[xoutswitchRightStickLeft])
                                controller.RightStickX = 0;
                            if (Fbool[xoutswitchRightStickLeft] & !Fbool[xoutswitchRightStickRight])
                                controller.RightStickX = -32767;
                            else
                            if (!Fbool[xoutswitchRightStickRight])
                                controller.RightStickX = 0;
                        }
                        if (xoutswitchRightStickY != "none")
                            controller.RightStickY = (short)(Fdouble[xoutswitchRightStickY] * 32767f);
                        else
                        {
                            if (Fbool[xoutswitchRightStickUp] & !Fbool[xoutswitchRightStickDown])
                                controller.RightStickY = 32767;
                            else
                            if (!Fbool[xoutswitchRightStickDown])
                                controller.RightStickY = 0;
                            if (Fbool[xoutswitchRightStickDown] & !Fbool[xoutswitchRightStickUp])
                                controller.RightStickY = -32767;
                            else
                            if (!Fbool[xoutswitchRightStickUp])
                                controller.RightStickY = 0;
                        }
                        if (Fbool[xoutswitchRightTrigger])
                            controller.RightTrigger = 255;
                        else
                            controller.RightTrigger = 0;
                        if (Fbool[xoutswitchLeftTrigger])
                            controller.LeftTrigger = 255;
                        else
                            controller.LeftTrigger = 0;
                    }
                    scpBus.Report(1, controller.GetReport());
                }
                else
                {
                    if (lconnected & !rconnected)
                    {
                        acc_gLeft.X = ((Int16)(report_bufLeft[13] | ((report_bufLeft[14] << 8) & 0xff00)) - acc_gcalibrationLeftX) * (1.0f / 4000f);
                        acc_gLeft.Y = ((Int16)(report_bufLeft[15] | ((report_bufLeft[16] << 8) & 0xff00)) - acc_gcalibrationLeftY) * (1.0f / 4000f);
                        acc_gLeft.Z = ((Int16)(report_bufLeft[17] | ((report_bufLeft[18] << 8) & 0xff00)) - acc_gcalibrationLeftZ) * (1.0f / 4000f);
                        InitDirectAnglesLeft = acc_gLeft;
                    }
                    if (rconnected & !lconnected)
                    {
                        acc_gRight.X = ((Int16)(report_bufRight[13] | ((report_bufRight[14] << 8) & 0xff00)) - acc_gcalibrationRightX) * (1.0f / 4000f);
                        acc_gRight.Y = -((Int16)(report_bufRight[15] | ((report_bufRight[16] << 8) & 0xff00)) - acc_gcalibrationRightY) * (1.0f / 4000f);
                        acc_gRight.Z = -((Int16)(report_bufRight[17] | ((report_bufRight[18] << 8) & 0xff00)) - acc_gcalibrationRightZ) * (1.0f / 4000f);
                        InitDirectAnglesRight = acc_gRight;
                    }
                    if (lconnected & rconnected)
                    {
                        acc_gLeft.X = ((Int16)(report_bufLeft[13] | ((report_bufLeft[14] << 8) & 0xff00)) - acc_gcalibrationLeftX) * (1.0f / 4000f);
                        acc_gLeft.Y = ((Int16)(report_bufLeft[15] | ((report_bufLeft[16] << 8) & 0xff00)) - acc_gcalibrationLeftY) * (1.0f / 4000f);
                        acc_gLeft.Z = ((Int16)(report_bufLeft[17] | ((report_bufLeft[18] << 8) & 0xff00)) - acc_gcalibrationLeftZ) * (1.0f / 4000f);
                        InitDirectAnglesLeft = acc_gLeft;
                        acc_gRight.X = ((Int16)(report_bufRight[13] | ((report_bufRight[14] << 8) & 0xff00)) - acc_gcalibrationRightX) * (1.0f / 4000f);
                        acc_gRight.Y = -((Int16)(report_bufRight[15] | ((report_bufRight[16] << 8) & 0xff00)) - acc_gcalibrationRightY) * (1.0f / 4000f);
                        acc_gRight.Z = -((Int16)(report_bufRight[17] | ((report_bufRight[18] << 8) & 0xff00)) - acc_gcalibrationRightZ) * (1.0f / 4000f);
                        InitDirectAnglesRight = acc_gRight;
                    }
                }
                Thread.Sleep(1);
            }
        }
        private static void changeScale(double dzchangedx, double dzchangedy)
        {
            if (irx > 0f)
                mousex = Scale(Math.Pow(irx, 3f) / Math.Pow(1024f, 2f) * viewpower3x + Math.Pow(irx, 2f) / Math.Pow(1024f, 1f) * viewpower2x + Math.Pow(irx, 1f) / Math.Pow(1024f, 0f) * viewpower1x + Math.Pow(irx, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x, 0f, 1024f, (dzchangedx / 100f) * 1024f, 1024f);
            if (irx < 0f)
                mousex = Scale(-Math.Pow(-irx, 3f) / Math.Pow(1024f, 2f) * viewpower3x - Math.Pow(-irx, 2f) / Math.Pow(1024f, 1f) * viewpower2x - Math.Pow(-irx, 1f) / Math.Pow(1024f, 0f) * viewpower1x - Math.Pow(-irx, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x, -1024f, 0f, -1024f, -(dzchangedx / 100f) * 1024f);
            if (iry > 0f)
                mousey = Scale(Math.Pow(iry, 3f) / Math.Pow(1024f, 2f) * viewpower3y + Math.Pow(iry, 2f) / Math.Pow(1024f, 1f) * viewpower2y + Math.Pow(iry, 1f) / Math.Pow(1024f, 0f) * viewpower1y + Math.Pow(iry, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y, 0f, 1024f, (dzchangedy / 100f) * 1024f, 1024f);
            if (iry < 0f)
                mousey = Scale(-Math.Pow(-iry, 3f) / Math.Pow(1024f, 2f) * viewpower3y - Math.Pow(-iry, 2f) / Math.Pow(1024f, 1f) * viewpower2y - Math.Pow(-iry, 1f) / Math.Pow(1024f, 0f) * viewpower1y - Math.Pow(-iry, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y, -1024f, 0f, -1024f, -(dzchangedy / 100f) * 1024f);
        }
        private static void changeView(double lowsenschangedx, double lowsenschangedy)
        {
            if (connected)
            {
                Fdouble["mousex"] = -mousex / 1024f / lowsenschangedx;
                Fdouble["mousey"] = -mousey / 1024f / lowsenschangedy;
                Fdouble["stickx"] = Math.Abs(mWSNunchuckStateRawJoystickX * 600f / 32767f) <= 1f ? mWSNunchuckStateRawJoystickX * 600f / 32767f : Math.Sign(mWSNunchuckStateRawJoystickX * 600f / 32767f);
                Fdouble["sticky"] = Math.Abs(mWSNunchuckStateRawJoystickY * 600f / 32767f) <= 1f ? mWSNunchuckStateRawJoystickY * 600f / 32767f : Math.Sign(mWSNunchuckStateRawJoystickY * 600f / 32767f);
            }
            if (lconnected & !rconnected & connected)
            {
                Fdouble["leftstickx"] = Math.Abs(GetStickLeft()[0] / 1024f * 1660f) <= 1f ? GetStickLeft()[0] / 1024f * 1660f : Math.Sign(GetStickLeft()[0] / 1024f * 1660f);
                Fdouble["leftsticky"] = Math.Abs(GetStickLeft()[1] / 1024f * 1660f) <= 1f ? GetStickLeft()[1] / 1024f * 1660f : Math.Sign(GetStickLeft()[1] / 1024f * 1660f);
            }
            if (rconnected & !lconnected & connected)
            {
                Fdouble["rightstickx"] = Math.Abs(-GetStickRight()[1] / 1024f * 1660f) <= 1f ? -GetStickRight()[1] / 1024f * 1660f : Math.Sign(-GetStickRight()[1] / 1024f * 1660f);
                Fdouble["rightsticky"] = Math.Abs(-GetStickRight()[0] / 1024f * 1660f) <= 1f ? -GetStickRight()[0] / 1024f * 1660f : Math.Sign(-GetStickRight()[0] / 1024f * 1660f);
            }
            if (lconnected & !rconnected & !connected)
            {
                Fdouble["mousex"] = mousex / 1024f / lowsensx;
                Fdouble["mousey"] = mousey / 1024f / lowsensy;
                Fdouble["leftstickx"] = Math.Abs(-GetStickLeft()[1] / 1024f * 1660f) <= 1f ? -GetStickLeft()[1] / 1024f * 1660f : Math.Sign(-GetStickLeft()[1] / 1024f * 1660f);
                Fdouble["leftsticky"] = Math.Abs(-GetStickLeft()[0] / 1024f * 1660f) <= 1f ? -GetStickLeft()[0] / 1024f * 1660f : Math.Sign(-GetStickLeft()[0] / 1024f * 1660f);
            }
            if (rconnected & !lconnected & !connected)
            {
                Fdouble["mousex"] = mousex / 1024f / lowsensx;
                Fdouble["mousey"] = -mousey / 1024f / lowsensy;
                Fdouble["rightstickx"] = Math.Abs(-GetStickRight()[1] / 1024f * 1660f) <= 1f ? -GetStickRight()[1] / 1024f * 1660f : Math.Sign(-GetStickRight()[1] / 1024f * 1660f);
                Fdouble["rightsticky"] = Math.Abs(-GetStickRight()[0] / 1024f * 1660f) <= 1f ? -GetStickRight()[0] / 1024f * 1660f : Math.Sign(-GetStickRight()[0] / 1024f * 1660f);
            }
            if (rconnected & lconnected & !connected)
            {
                Fdouble["mousex"] = -mousex / 1024f / lowsensx;
                Fdouble["mousex"] = -mousey / 1024f / lowsensy;
                Fdouble["leftstickx"] = Math.Abs(GetStickLeft()[0] / 1024f * 1660f) <= 1f ? GetStickLeft()[0] / 1024f * 1660f : Math.Sign(GetStickLeft()[0] / 1024f * 1660f);
                Fdouble["leftsticky"] = Math.Abs(GetStickLeft()[1] / 1024f * 1660f) <= 1f ? GetStickLeft()[1] / 1024f * 1660f : Math.Sign(GetStickLeft()[1] / 1024f * 1660f);
                Fdouble["rightstickx"] = Math.Abs(-GetStickRight()[0] / 1024f * 1660f) <= 1f ? -GetStickRight()[0] / 1024f * 1660f : Math.Sign(-GetStickRight()[0] / 1024f * 1660f);
                Fdouble["rightsticky"] = Math.Abs(-GetStickRight()[1] / 1024f * 1660f) <= 1f ? -GetStickRight()[1] / 1024f * 1660f : Math.Sign(-GetStickRight()[1] / 1024f * 1660f);
            }
            if (xoutRightStickX != "none")
                controller.RightStickX = (short)(Fdouble[xoutRightStickX] * 32767f);
            else
            {
                if (Fbool[xoutRightStickRight] & !Fbool[xoutRightStickLeft])
                    controller.RightStickX = 32767;
                else
                if (!Fbool[xoutRightStickLeft])
                    controller.RightStickX = 0;
                if (Fbool[xoutRightStickLeft] & !Fbool[xoutRightStickRight])
                    controller.RightStickX = -32767;
                else
                if (!Fbool[xoutRightStickRight])
                    controller.RightStickX = 0;
            }
            if (xoutRightStickY != "none")
                controller.RightStickY = (short)(Fdouble[xoutRightStickY] * 32767f);
            else
            {
                if (Fbool[xoutRightStickUp] & !Fbool[xoutRightStickDown])
                    controller.RightStickY = 32767;
                else
                if (!Fbool[xoutRightStickDown])
                    controller.RightStickY = 0;
                if (Fbool[xoutRightStickDown] & !Fbool[xoutRightStickUp])
                    controller.RightStickY = -32767;
                else
                if (!Fbool[xoutRightStickUp])
                    controller.RightStickY = 0;
            }
            if (xoutLeftStickX != "none")
                controller.LeftStickX = (short)(Fdouble[xoutLeftStickX] * 32767f);
            else
            {
                if (Fbool[xoutLeftStickRight] & !Fbool[xoutLeftStickLeft])
                    controller.LeftStickX = 32767;
                else
                if (!Fbool[xoutLeftStickLeft])
                    controller.LeftStickX = 0;
                if (Fbool[xoutLeftStickLeft] & !Fbool[xoutLeftStickRight])
                    controller.LeftStickX = -32767;
                else
                if (!Fbool[xoutLeftStickRight])
                    controller.LeftStickX = 0;
            }
            if (xoutLeftStickY != "none")
                controller.LeftStickY = (short)(Fdouble[xoutLeftStickY] * 32767f);
            else
            {
                if (Fbool[xoutLeftStickUp] & !Fbool[xoutLeftStickDown])
                    controller.LeftStickY = 32767;
                else
                if (!Fbool[xoutLeftStickDown])
                    controller.LeftStickY = 0;
                if (Fbool[xoutLeftStickDown] & !Fbool[xoutLeftStickUp])
                    controller.LeftStickY = -32767;
                else
                if (!Fbool[xoutLeftStickUp])
                    controller.LeftStickY = 0;
            }
        }
        private static void taskX2()
        {
            while (running)
            {
                if (lconnected)
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
                    Fbool["LeftButtonACC"] = LeftButtonACC;
                    Fbool["LeftButtonStickRight"] = GetStickLeft()[0] > 0.25f;
                    Fbool["LeftButtonStickLeft"] = GetStickLeft()[0] < -0.25f;
                    Fbool["LeftButtonStickUp"] = GetStickLeft()[1] > 0.25f;
                    Fbool["LeftButtonStickDown"] = GetStickLeft()[1] < -0.25f;
                    if (LeftvalList.Count >= 7)
                    {
                        LeftvalList.RemoveAt(0);
                        LeftvalList.Add(acc_gLeft.Y);
                    }
                    else
                        LeftvalList.Add(acc_gLeft.Y);
                    LeftRollLeft = LeftvalList.Average() <= -0.75f;
                    LeftRollRight = LeftvalList.Average() >= 0.75f;
                    Fbool["LeftRollLeft"] = LeftRollLeft;
                    Fbool["LeftRollRight"] = LeftRollRight;
                }
                if (rconnected)
                {
                    acc_gRight.X = ((Int16)(report_bufRight[13] | ((report_bufRight[14] << 8) & 0xff00)) - acc_gcalibrationRightX) * (1.0f / 4000f);
                    acc_gRight.Y = -((Int16)(report_bufRight[15] | ((report_bufRight[16] << 8) & 0xff00)) - acc_gcalibrationRightY) * (1.0f / 4000f);
                    acc_gRight.Z = -((Int16)(report_bufRight[17] | ((report_bufRight[18] << 8) & 0xff00)) - acc_gcalibrationRightZ) * (1.0f / 4000f);
                    stick_rawRight[0] = report_bufRight[6 + (!ISRIGHT ? 0 : 3)];
                    stick_rawRight[1] = report_bufRight[7 + (!ISRIGHT ? 0 : 3)];
                    stick_rawRight[2] = report_bufRight[8 + (!ISRIGHT ? 0 : 3)];
                    stick_precalRight[0] = (UInt16)(stick_rawRight[0] | ((stick_rawRight[1] & 0xf) << 8));
                    stick_precalRight[1] = (UInt16)((stick_rawRight[1] >> 4) | (stick_rawRight[2] << 4));
                    stickRight = CenterSticksRight(stick_precalRight);
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
                    RightButtonACC = acc_gRight.Y <= -1.13;
                    RightButtonSPA = RightButtonSL | RightButtonSR | RightButtonPLUS | RightButtonACC;
                    mWSRightButtonTH = mWSButtonStateTwo | RightButtonHOME;
                    Fbool["RightButtonSHOULDER_1"] = RightButtonSHOULDER_1;
                    Fbool["RightButtonSHOULDER_2"] = RightButtonSHOULDER_2;
                    Fbool["RightButtonSR"] = RightButtonSR;
                    Fbool["RightButtonSL"] = RightButtonSL;
                    Fbool["RightButtonDPAD_DOWN"] = RightButtonDPAD_DOWN;
                    Fbool["RightButtonDPAD_RIGHT"] = RightButtonDPAD_RIGHT;
                    Fbool["RightButtonDPAD_UP"] = RightButtonDPAD_UP;
                    Fbool["RightButtonDPAD_LEFT"] = RightButtonDPAD_LEFT;
                    Fbool["RightButtonPLUS"] = RightButtonPLUS;
                    Fbool["RightButtonHOME"] = RightButtonHOME;
                    Fbool["RightButtonSTICK"] = RightButtonSTICK;
                    Fbool["RightButtonSPA"] = RightButtonSPA;
                    Fbool["mWSRightButtonTH"] = mWSRightButtonTH;
                    Fbool["RightButtonACC"] = RightButtonACC;
                    Fbool["RightButtonStickRight"] = GetStickRight()[1] > 0.25f;
                    Fbool["RightButtonStickLeft"] = GetStickRight()[1] < -0.25f;
                    Fbool["RightButtonStickUp"] = GetStickRight()[0] > 0.25f;
                    Fbool["RightButtonStickDown"] = GetStickRight()[0] < -0.25f;
                    if (RightvalList.Count >= 7)
                    {
                        RightvalList.RemoveAt(0);
                        RightvalList.Add(acc_gRight.X);
                    }
                    else
                        RightvalList.Add(acc_gRight.X);
                    RightRollLeft = RightvalList.Average() <= -0.75f;
                    RightRollRight = RightvalList.Average() >= 0.75f;
                    Fbool["RightRollLeft"] = RightRollLeft;
                    Fbool["RightRollRight"] = RightRollRight;
                }
                if (connected)
                {
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
                    mWSButtonStateFront = (mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 30f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 30f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 30f;
                    mWSButtonStateHFront = mWSButtonStateHome | mWSButtonStateFront;
                    mWSButtonStatePU = mWSButtonStatePlus | mWSButtonStateUp;
                    mWSButtonStateMU = mWSButtonStateMinus | mWSButtonStateUp;
                    mWSButtonStateLR = mWSButtonStateLeft | mWSButtonStateRight;
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
                    Fbool["mWSButtonStateHFront"] = mWSButtonStateHFront;
                    Fbool["mWSButtonStatePU"] = mWSButtonStatePU;
                    Fbool["mWSButtonStateMU"] = mWSButtonStateMU;
                    Fbool["mWSButtonStateLR"] = mWSButtonStateLR;
                    Fbool["mWSButtonStateFront"] = mWSButtonStateFront;
                }
                if (!lconnected & !rconnected & connected)
                {
                    mWSNunchuckStateRawJoystickX = aBuffer[16] - 125f + stickviewxinit;
                    mWSNunchuckStateRawJoystickY = aBuffer[17] - 125f + stickviewyinit;
                    mWSNunchuckStateRawValuesX = aBuffer[18] - 125f;
                    mWSNunchuckStateRawValuesY = aBuffer[19] - 125f;
                    mWSNunchuckStateRawValuesZ = aBuffer[20] - 125f;
                    mWSNunchuckStateC = (aBuffer[21] & 0x02) == 0;
                    mWSNunchuckStateZ = (aBuffer[21] & 0x01) == 0;
                    if (valListZ.Count >= 7)
                    {
                        valListZ.RemoveAt(0);
                        valListZ.Add(mWSNunchuckStateRawValuesX);
                        rolling = valListZ.Average();
                    }
                    else
                        valListZ.Add(mWSNunchuckStateRawValuesX);
                    Fbool["mWSNunchuckStateRollingLeft"] = rolling >= 48f;
                    Fbool["mWSNunchuckStateRollingRight"] = rolling <= -48f;
                    Fbool["mWSNunchuckStateRawValuesY"] = (mWSNunchuckStateRawValuesY > 33f) & !((mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 40f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 40f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 40f);
                    Fbool["mWSNunchuckStateC"] = mWSNunchuckStateC;
                    Fbool["mWSNunchuckStateZ"] = mWSNunchuckStateZ;
                    Fbool["mWSNunchuckStateStickRight"] = mWSNunchuckStateRawJoystickX > 33f;
                    Fbool["mWSNunchuckStateStickLeft"] = mWSNunchuckStateRawJoystickX < -33f;
                    Fbool["mWSNunchuckStateStickUp"] = mWSNunchuckStateRawJoystickY > 33f;
                    Fbool["mWSNunchuckStateStickDown"] = mWSNunchuckStateRawJoystickY < -33f;
                }
                valchanged(0, (RightButtonDPAD_DOWN & RightButtonHOME) | (LeftButtonDPAD_DOWN & LeftButtonCAPTURE) | (mWSButtonStateHome & mWSButtonStateTwo));
                if (wd[0] == 1 & !Getstate)
                {
                    using (System.IO.StreamReader createdfile = new System.IO.StreamReader("WiiJoyX.txt"))
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
                        dzx = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        dzy = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsensx = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsensy = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        centery = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        apressio = bool.Parse(createdfile.ReadLine());
                        createdfile.ReadLine();
                        dzapressiox = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        dzapressioy = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsensapressiox = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsensapressioy = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsenstime = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        lowsenstimeapressio = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchviewpower05x = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchviewpower1x = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchviewpower2x = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchviewpower3x = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchviewpower05y = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchviewpower1y = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchviewpower2y = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchviewpower3y = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchdzx = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchdzy = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchlowsensx = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        switchlowsensy = Convert.ToDouble(createdfile.ReadLine());
                        createdfile.ReadLine();
                        xoutDown = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeft = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRight = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutUp = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRightStick = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeftStick = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutA = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutBack = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutStart = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutX = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRightBumper = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeftBumper = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutB = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutY = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRightTrigger = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeftTrigger = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchDown = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeft = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRight = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchUp = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRightStick = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeftStick = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchA = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchBack = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchStart = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchX = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRightBumper = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeftBumper = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchB = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchY = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRightTrigger = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeftTrigger = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeftStickRight = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeftStickLeft = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeftStickUp = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeftStickDown = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRightStickRight = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRightStickLeft = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRightStickUp = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRightStickDown = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeftStickX = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutLeftStickY = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRightStickX = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutRightStickY = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeftStickRight = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeftStickLeft = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeftStickUp = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeftStickDown = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRightStickRight = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRightStickLeft = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRightStickUp = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRightStickDown = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeftStickX = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchLeftStickY = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRightStickX = createdfile.ReadLine();
                        createdfile.ReadLine();
                        xoutswitchRightStickY = createdfile.ReadLine();
                    }
                    lowsensdiffx = (lowsensapressiox - lowsensx) / lowsenstime;
                    lowsensdiffy = (lowsensapressioy - lowsensy) / lowsenstime;
                    dzdiffx = (dzapressiox - dzx) / lowsenstime;
                    dzdiffy = (dzapressioy - dzy) / lowsenstime;
                    lowsensdiffapressiox = (lowsensapressiox - lowsensx) / lowsenstimeapressio;
                    lowsensdiffapressioy = (lowsensapressioy - lowsensy) / lowsenstimeapressio;
                    dzdiffapressiox = (dzapressiox - dzx) / lowsenstimeapressio;
                    dzdiffapressioy = (dzapressioy - dzy) / lowsenstimeapressio;
                    lowsensincx = lowsensapressiox;
                    lowsensincy = lowsensapressioy;
                    dzincx = dzapressiox;
                    dzincy = dzapressioy;
                    fixInit();
                    Getstate = true;
                }
                else
                {
                    if (wd[0] == 1 & Getstate)
                    {
                        Getstate = false;
                        fixStuck();
                    }
                }
                if (Getstate)
                {
                    valchanged(33, mWSButtonStateHome & mWSButtonStateDown & connected);
                    if (wd[33] == 1 & !switchstate)
                    {
                        fixInit();
                        fixStuck();
                        switchstate = true;
                    }
                    else
                    {
                        if (wd[33] == 1 & switchstate)
                        {
                            fixInit();
                            fixStuck();
                            switchstate = false;
                        }
                    }
                    if (!switchstate)
                    {
                        valchanged(1, Fbool[xoutDown]);
                        if (wd[1] == 1)
                            controller.Buttons ^= X360Buttons.Down;
                        if (wu[1] == 1)
                            controller.Buttons &= ~X360Buttons.Down;
                        valchanged(2, Fbool[xoutLeft]);
                        if (wd[2] == 1)
                            controller.Buttons ^= X360Buttons.Left;
                        if (wu[2] == 1)
                            controller.Buttons &= ~X360Buttons.Left;
                        valchanged(3, Fbool[xoutRight]);
                        if (wd[3] == 1)
                            controller.Buttons ^= X360Buttons.Right;
                        if (wu[3] == 1)
                            controller.Buttons &= ~X360Buttons.Right;
                        valchanged(4, Fbool[xoutUp]);
                        if (wd[4] == 1)
                            controller.Buttons ^= X360Buttons.Up;
                        if (wu[4] == 1)
                            controller.Buttons &= ~X360Buttons.Up;
                        valchanged(5, Fbool[xoutRightStick]);
                        if (wd[5] == 1)
                            controller.Buttons ^= X360Buttons.RightStick;
                        if (wu[5] == 1)
                            controller.Buttons &= ~X360Buttons.RightStick;
                        valchanged(6, Fbool[xoutLeftStick]);
                        if (wd[6] == 1)
                            controller.Buttons ^= X360Buttons.LeftStick;
                        if (wu[6] == 1)
                            controller.Buttons &= ~X360Buttons.LeftStick;
                        valchanged(7, Fbool[xoutA]);
                        if (wd[7] == 1)
                            controller.Buttons ^= X360Buttons.A;
                        if (wu[7] == 1)
                            controller.Buttons &= ~X360Buttons.A;
                        valchanged(8, Fbool[xoutBack]);
                        if (wd[8] == 1)
                            controller.Buttons ^= X360Buttons.Back;
                        if (wu[8] == 1)
                            controller.Buttons &= ~X360Buttons.Back;
                        valchanged(9, Fbool[xoutStart]);
                        if (wd[9] == 1)
                            controller.Buttons ^= X360Buttons.Start;
                        if (wu[9] == 1)
                            controller.Buttons &= ~X360Buttons.Start;
                        valchanged(10, Fbool[xoutX]);
                        if (wd[10] == 1)
                            controller.Buttons ^= X360Buttons.X;
                        if (wu[10] == 1)
                            controller.Buttons &= ~X360Buttons.X;
                        valchanged(11, Fbool[xoutRightBumper]);
                        if (wd[11] == 1)
                            controller.Buttons ^= X360Buttons.RightBumper;
                        if (wu[11] == 1)
                            controller.Buttons &= ~X360Buttons.RightBumper;
                        valchanged(12, Fbool[xoutLeftBumper]);
                        if (wd[12] == 1)
                            controller.Buttons ^= X360Buttons.LeftBumper;
                        if (wu[12] == 1)
                            controller.Buttons &= ~X360Buttons.LeftBumper;
                        valchanged(13, Fbool[xoutB]);
                        if (wd[13] == 1)
                            controller.Buttons ^= X360Buttons.B;
                        if (wu[13] == 1)
                            controller.Buttons &= ~X360Buttons.B;
                        valchanged(14, Fbool[xoutY]);
                        if (wd[14] == 1)
                            controller.Buttons ^= X360Buttons.Y;
                        if (wu[14] == 1)
                            controller.Buttons &= ~X360Buttons.Y;
                    }
                    else
                    {
                        valchanged(19, Fbool[xoutswitchDown]);
                        if (wd[19] == 1)
                            controller.Buttons ^= X360Buttons.Down;
                        if (wu[19] == 1)
                            controller.Buttons &= ~X360Buttons.Down;
                        valchanged(20, Fbool[xoutswitchLeft]);
                        if (wd[20] == 1)
                            controller.Buttons ^= X360Buttons.Left;
                        if (wu[20] == 1)
                            controller.Buttons &= ~X360Buttons.Left;
                        valchanged(21, Fbool[xoutswitchRight]);
                        if (wd[21] == 1)
                            controller.Buttons ^= X360Buttons.Right;
                        if (wu[21] == 1)
                            controller.Buttons &= ~X360Buttons.Right;
                        valchanged(22, Fbool[xoutswitchUp]);
                        if (wd[22] == 1)
                            controller.Buttons ^= X360Buttons.Up;
                        if (wu[22] == 1)
                            controller.Buttons &= ~X360Buttons.Up;
                        valchanged(23, Fbool[xoutswitchRightStick]);
                        if (wd[23] == 1)
                            controller.Buttons ^= X360Buttons.RightStick;
                        if (wu[23] == 1)
                            controller.Buttons &= ~X360Buttons.RightStick;
                        valchanged(24, Fbool[xoutswitchLeftStick]);
                        if (wd[24] == 1)
                            controller.Buttons ^= X360Buttons.LeftStick;
                        if (wu[24] == 1)
                            controller.Buttons &= ~X360Buttons.LeftStick;
                        valchanged(25, Fbool[xoutswitchA]);
                        if (wd[25] == 1)
                            controller.Buttons ^= X360Buttons.A;
                        if (wu[25] == 1)
                            controller.Buttons &= ~X360Buttons.A;
                        valchanged(26, Fbool[xoutswitchBack]);
                        if (wd[26] == 1)
                            controller.Buttons ^= X360Buttons.Back;
                        if (wu[26] == 1)
                            controller.Buttons &= ~X360Buttons.Back;
                        valchanged(27, Fbool[xoutswitchStart]);
                        if (wd[27] == 1)
                            controller.Buttons ^= X360Buttons.Start;
                        if (wu[27] == 1)
                            controller.Buttons &= ~X360Buttons.Start;
                        valchanged(28, Fbool[xoutswitchX]);
                        if (wd[28] == 1)
                            controller.Buttons ^= X360Buttons.X;
                        if (wu[28] == 1)
                            controller.Buttons &= ~X360Buttons.X;
                        valchanged(29, Fbool[xoutswitchRightBumper]);
                        if (wd[29] == 1)
                            controller.Buttons ^= X360Buttons.RightBumper;
                        if (wu[29] == 1)
                            controller.Buttons &= ~X360Buttons.RightBumper;
                        valchanged(30, Fbool[xoutswitchLeftBumper]);
                        if (wd[30] == 1)
                            controller.Buttons ^= X360Buttons.LeftBumper;
                        if (wu[30] == 1)
                            controller.Buttons &= ~X360Buttons.LeftBumper;
                        valchanged(31, Fbool[xoutswitchB]);
                        if (wd[31] == 1)
                            controller.Buttons ^= X360Buttons.B;
                        if (wu[31] == 1)
                            controller.Buttons &= ~X360Buttons.B;
                        valchanged(32, Fbool[xoutswitchY]);
                        if (wd[32] == 1)
                            controller.Buttons ^= X360Buttons.Y;
                        if (wu[32] == 1)
                            controller.Buttons &= ~X360Buttons.Y;
                    }
                    scpBus.Report(1, controller.GetReport());
                }
                Thread.Sleep(10);
            }
        }
        private static void fixInit()
        {
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
            mWSButtonStateAio = false;
            randA = false;
            for (int i = 1; i <= 36; i++)
            {
                wd[i] = 2;
                wu[i] = 2;
                Thread.Sleep(1);
            }
        }
        private static void fixStuck()
        {
            controller.Buttons &= ~X360Buttons.LeftBumper;
            controller.Buttons &= ~X360Buttons.RightBumper;
            controller.Buttons &= ~X360Buttons.Left;
            controller.Buttons &= ~X360Buttons.Right;
            controller.Buttons &= ~X360Buttons.Up;
            controller.Buttons &= ~X360Buttons.Down;
            controller.Buttons &= ~X360Buttons.X;
            controller.Buttons &= ~X360Buttons.Y;
            controller.Buttons &= ~X360Buttons.A;
            controller.Buttons &= ~X360Buttons.B;
            controller.Buttons &= ~X360Buttons.RightStick;
            controller.Buttons &= ~X360Buttons.LeftStick;
            controller.Buttons &= ~X360Buttons.Start;
            controller.Buttons &= ~X360Buttons.Back;
            controller.Buttons &= ~X360Buttons.Logo;
            controller.RightStickX = 0;
            controller.RightStickY = 0;
            controller.LeftStickX = 0;
            controller.LeftStickY = 0;
            controller.LeftTrigger = 0;
            controller.RightTrigger = 0;
            scpBus.Report(1, controller.GetReport());
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (running)
                {
                    if (lconnected)
                        ExtractIMUValuesLeft();
                    if (rconnected)
                        ExtractIMUValuesRight();
                }
            }
            catch { }
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
        private static void taskDRight()
        {
            while (running)
            {
                try
                {
                    Rhid_read_timeout(handleRight, report_bufRight, (UIntPtr)report_lenRight);
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
                    scpBus.Unplug(1);
                    Thread.Sleep(100);
                    if (lconnected)
                    {
                        threadstart = new ThreadStart(FormCloseLeft);
                        thread = new Thread(threadstart);
                        thread.Start();
                        Thread.Sleep(6000);
                    }
                    if (rconnected)
                    {
                        threadstart = new ThreadStart(FormCloseRight);
                        thread = new Thread(threadstart);
                        thread.Start();
                        Thread.Sleep(6000);
                    }
                    if (connected)
                    {
                        threadstart = new ThreadStart(FormClose);
                        thread = new Thread(threadstart);
                        thread.Start();
                    }
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
        private static void FormCloseRight()
        {
            try
            {
                Rhid_close(handleRight);
                handleRight.Close();
                disconnectRight();
            }
            catch { }
        }
        private const string vendor_id = "57e", vendor_id_ = "057e", product_r1 = "0330", product_r2 = "0306", product_l = "2006", product_r = "2007";
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
        private static Quaternion GetVectoraLeft()
        {
            Vector3 v1 = new Vector3(j_aLeft.X, i_aLeft.X, k_aLeft.X);
            Vector3 v2 = -(new Vector3(j_aLeft.Z, i_aLeft.Z, k_aLeft.Z));
            return QuaternionLookRotationLeft(v1, v2);
        }
        private static Quaternion GetVectorbLeft()
        {
            Vector3 v1 = new Vector3(j_bLeft.X, i_bLeft.X, k_bLeft.X);
            Vector3 v2 = -(new Vector3(j_bLeft.Z, i_bLeft.Z, k_bLeft.Z));
            return QuaternionLookRotationLeft(v1, v2);
        }
        private static Quaternion GetVectorcLeft()
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
        private static Vector3 ToEulerAnglesLeft(Quaternion q)
        {
            Vector3 pitchYawRoll = new Vector3();
            double sqw = q.W * q.W;
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;
            double unit = sqx + sqy + sqz + sqw;
            double test = q.X * q.Y + q.Z * q.W;
            if (test > 0.4999f * unit)
            {
                pitchYawRoll.Y = 2f * (float)Math.Atan2(q.X, q.W);
                pitchYawRoll.X = (float)Math.PI * 0.5f;
                pitchYawRoll.Z = 0f;
                return pitchYawRoll;
            }
            else if (test < -0.4999f * unit)
            {
                pitchYawRoll.Y = -2f * (float)Math.Atan2(q.X, q.W);
                pitchYawRoll.X = -(float)Math.PI * 0.5f;
                pitchYawRoll.Z = 0f;
                return pitchYawRoll;
            }
            else
            {
                pitchYawRoll.Y = (float)Math.Atan2(2f * q.Y * q.W - 2f * q.X * q.Z, sqx - sqy - sqz + sqw);
                pitchYawRoll.X = (float)Math.Asin(2f * test / unit);
                pitchYawRoll.Z = (float)Math.Atan2(2f * q.X * q.W - 2f * q.Y * q.Z, -sqx + sqy - sqz + sqw);
            }
            return pitchYawRoll;
        }
        private static void ExtractIMUValuesLeft()
        {
            gyr_gLeft.X = ((Int16)(report_bufLeft[19] | ((report_bufLeft[20] << 8) & 0xff00)) - gyr_gcalibrationLeftX) * (1.0f / 4000000f);
            gyr_gLeft.Y = ((Int16)(report_bufLeft[21] | ((report_bufLeft[22] << 8) & 0xff00)) - gyr_gcalibrationLeftY) * (1.0f / 4000000f);
            gyr_gLeft.Z = ((Int16)(report_bufLeft[23] | ((report_bufLeft[24] << 8) & 0xff00)) - gyr_gcalibrationLeftZ) * (1.0f / 4000000f);
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
            if (!Getstate | LeftButtonCAPTURE)
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
        private static double[] CenterSticksLeft(UInt16[] vals)
        {
            double[] s = { 0, 0 };
            s[0] = ((int)((vals[0] - stick_calibrationLeft[0]) / 100f)) / 13f;
            s[1] = ((int)((vals[1] - stick_calibrationLeft[1]) / 100f)) / 13f;
            return s;
        }
        private static Quaternion GetVectoraRight()
        {
            Vector3 v1 = new Vector3(j_aRight.X, i_aRight.X, k_aRight.X);
            Vector3 v2 = -(new Vector3(j_aRight.Z, i_aRight.Z, k_aRight.Z));
            return QuaternionLookRotationRight(v1, v2);
        }
        private static Quaternion GetVectorbRight()
        {
            Vector3 v1 = new Vector3(j_bRight.X, i_bRight.X, k_bRight.X);
            Vector3 v2 = -(new Vector3(j_bRight.Z, i_bRight.Z, k_bRight.Z));
            return QuaternionLookRotationRight(v1, v2);
        }
        private static Quaternion GetVectorcRight()
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
        private static Vector3 ToEulerAnglesRight(Quaternion q)
        {
            Vector3 pitchYawRoll = new Vector3();
            double sqw = q.W * q.W;
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;
            double unit = sqx + sqy + sqz + sqw;
            double test = q.X * q.Y + q.Z * q.W;
            if (test > 0.4999f * unit)
            {
                pitchYawRoll.Y = 2f * (float)Math.Atan2(q.X, q.W);
                pitchYawRoll.X = (float)Math.PI * 0.5f;
                pitchYawRoll.Z = 0f;
                return pitchYawRoll;
            }
            else if (test < -0.4999f * unit)
            {
                pitchYawRoll.Y = -2f * (float)Math.Atan2(q.X, q.W);
                pitchYawRoll.X = -(float)Math.PI * 0.5f;
                pitchYawRoll.Z = 0f;
                return pitchYawRoll;
            }
            else
            {
                pitchYawRoll.Y = (float)Math.Atan2(2f * q.Y * q.W - 2f * q.X * q.Z, sqx - sqy - sqz + sqw);
                pitchYawRoll.X = (float)Math.Asin(2f * test / unit);
                pitchYawRoll.Z = (float)Math.Atan2(2f * q.X * q.W - 2f * q.Y * q.Z, -sqx + sqy - sqz + sqw);
            }
            return pitchYawRoll;
        }
        private static void ExtractIMUValuesRight()
        {
            gyr_gRight.X = ((Int16)(report_bufRight[19] | ((report_bufRight[20] << 8) & 0xff00)) - gyr_gcalibrationRightX) * (1.0f / 4000000f);
            gyr_gRight.Y = -((Int16)(report_bufRight[21] | ((report_bufRight[22] << 8) & 0xff00)) - gyr_gcalibrationRightY) * (1.0f / 4000000f);
            gyr_gRight.Z = -((Int16)(report_bufRight[23] | ((report_bufRight[24] << 8) & 0xff00)) - gyr_gcalibrationRightZ) * (1.0f / 4000000f);
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
            if (!Getstate | RightButtonHOME)
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
        private static double[] CenterSticksRight(UInt16[] vals)
        {
            double[] s = { 0, 0 };
            s[0] = ((int)((vals[0] - stick_calibrationRight[0]) / 100f)) / 13f;
            s[1] = ((int)((vals[1] - stick_calibrationRight[1]) / 100f)) / 13f;
            return s;
        }
        private static double[] stickLeft = { 0, 0 };
        private static SafeFileHandle handleLeft;
        private static byte[] stick_rawLeft = { 0, 0, 0 };
        private static UInt16[] stick_calibrationLeft = { 0, 0 };
        private static UInt16[] stick_precalLeft = { 0, 0 };
        private static Vector3 gyr_gLeft = new Vector3();
        private static Vector3 acc_gLeft = new Vector3();
        private const uint report_lenLeft = 25;
        private static Vector3 i_aLeft = new Vector3(1, 0, 0);
        private static Vector3 j_aLeft = new Vector3(0, 1, 0);
        private static Vector3 k_aLeft = new Vector3(0, 0, 1);
        private static Vector3 i_bLeft = new Vector3(1, 0, 0);
        private static Vector3 j_bLeft = new Vector3(0, 1, 0);
        private static Vector3 k_bLeft = new Vector3(0, 0, 1);
        private static Vector3 i_cLeft = new Vector3(1, 0, 0);
        private static Vector3 j_cLeft = new Vector3(0, 1, 0);
        private static Vector3 k_cLeft = new Vector3(0, 0, 1);
        private static Vector3 InitDirectAnglesLeft, DirectAnglesLeft;
        private static Vector3 InitEulerAnglesaLeft, EulerAnglesaLeft, InitEulerAnglesbLeft, EulerAnglesbLeft, InitEulerAnglescLeft, EulerAnglescLeft, EulerAnglesLeft;
        private static bool LeftButtonSHOULDER_1, LeftButtonSHOULDER_2, LeftButtonSR, LeftButtonSL, LeftButtonDPAD_DOWN, LeftButtonDPAD_RIGHT, LeftButtonDPAD_UP, LeftButtonDPAD_LEFT, LeftButtonMINUS, LeftButtonSTICK, LeftButtonCAPTURE, ISLEFT;
        private static byte[] report_bufLeft = new byte[report_lenLeft];
        private static byte[] buf_Left = new byte[report_lenLeft];
        private static float acc_gcalibrationLeftX, acc_gcalibrationLeftY, acc_gcalibrationLeftZ, gyr_gcalibrationLeftX, gyr_gcalibrationLeftY, gyr_gcalibrationLeftZ;
        private static double[] GetStickLeft()
        {
            return stickLeft;
        }
        private static Vector3 GetAccelLeft()
        {
            return acc_gLeft;
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
        private static double[] stickRight = { 0, 0 };
        private static SafeFileHandle handleRight;
        private static byte[] stick_rawRight = { 0, 0, 0 };
        private static UInt16[] stick_calibrationRight = { 0, 0 };
        private static UInt16[] stick_precalRight = { 0, 0 };
        private static Vector3 acc_gRight = new Vector3();
        private static Vector3 gyr_gRight = new Vector3();
        private const uint report_lenRight = 25;
        private static Vector3 i_cRight = new Vector3(1, 0, 0);
        private static Vector3 j_cRight = new Vector3(0, 1, 0);
        private static Vector3 k_cRight = new Vector3(0, 0, 1);
        private static Vector3 i_bRight = new Vector3(1, 0, 0);
        private static Vector3 j_bRight = new Vector3(0, 1, 0);
        private static Vector3 k_bRight = new Vector3(0, 0, 1);
        private static Vector3 i_aRight = new Vector3(1, 0, 0);
        private static Vector3 j_aRight = new Vector3(0, 1, 0);
        private static Vector3 k_aRight = new Vector3(0, 0, 1);
        private static Vector3 InitDirectAnglesRight, DirectAnglesRight;
        private static Vector3 InitEulerAnglesaRight, EulerAnglesaRight, InitEulerAnglesbRight, EulerAnglesbRight, InitEulerAnglescRight, EulerAnglescRight, EulerAnglesRight;
        private static bool RightButtonSHOULDER_1, RightButtonSHOULDER_2, RightButtonSR, RightButtonSL, RightButtonDPAD_DOWN, RightButtonDPAD_RIGHT, RightButtonDPAD_UP, RightButtonDPAD_LEFT, RightButtonPLUS, RightButtonSTICK, RightButtonHOME, ISRIGHT;
        private static byte[] report_bufRight = new byte[report_lenRight];
        private static byte[] buf_Right = new byte[report_lenRight];
        private static float acc_gcalibrationRightX, acc_gcalibrationRightY, acc_gcalibrationRightZ, gyr_gcalibrationRightX, gyr_gcalibrationRightY, gyr_gcalibrationRightZ;
        private static double[] GetStickRight()
        {
            return stickRight;
        }
        private static Vector3 GetAccelRight()
        {
            return acc_gRight;
        }
        private static bool ScanRight()
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
        private static void AttachJoyRight(string path)
        {
            do
            {
                IntPtr handle = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleRight = Rhid_open_path(handle);
                SubcommandRight(0x3, new byte[] { 0x30 }, 1);
                SubcommandRight(0x40, new byte[] { 0x1 }, 1);
                SubcommandRight(0x60, new byte[] { 0x98 }, 1);
                SubcommandRight(0x80, new byte[] { 0x34 }, 1);
            }
            while (handleRight.IsInvalid);
        }
        private static void SubcommandRight(byte sc, byte[] buf, uint len)
        {
            Array.Copy(buf, 0, buf_Right, 11, len);
            buf_Right[0] = 0x1;
            buf_Right[1] = 0;
            buf_Right[10] = sc;
            Rhid_write(handleRight, buf_Right, (UIntPtr)(len + 11));
            Rhid_read_timeout(handleRight, buf_Right, (UIntPtr)report_lenRight);
        }
    }
}
