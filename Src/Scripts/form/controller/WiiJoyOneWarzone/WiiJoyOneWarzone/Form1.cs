using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using controller;
using System.Windows.Forms;
namespace WiiJoyOneWarzone
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
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private const double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe;
        private static double irx0, iry0, irx1, iry1, irx, iry, irxc, iryc, mWSIRSensors0X, mWSIRSensors0Y, mWSIRSensors1X, mWSIRSensors1Y, mWSButtonStateIRX, mWSButtonStateIRY, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, calibrationinit, mousex, mousey, mWSIRSensors0Xcam, mWSIRSensors0Ycam, mWSIRSensors1Xcam, mWSIRSensors1Ycam, mWSIRSensorsXcam, mWSIRSensorsYcam, keys123456, viewpower05x, viewpower1x, viewpower2x = 10f, viewpower3x, viewpower05y, viewpower1y, viewpower2y = 10f, viewpower3y, dzx, dzy, lowsensx = 1f, lowsensy = 1f, slowingdown, switchcount, centery = 160f;
        private static bool mWSIR1found, mWSIR0found, mWSButtonStateA, mWSButtonStateB, mWSButtonStateMinus, mWSButtonStateHome, mWSButtonStatePlus, mWSButtonStateOne, mWSButtonStateTwo, mWSButtonStateUp, mWSButtonStateDown, mWSButtonStateLeft, mWSButtonStateRight, Getstate, running, firstdown, seconddown;
        private static byte[] buff = new byte[] { 0x55 }, mBuff = new byte[22], aBuffer = new byte[22];
        private const byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        private static uint CurrentResolution = 0;
        private static FileStream mStream;
        private static SafeFileHandle handle = null;
        private static ThreadStart threadstart;
        private static Thread thread;
        private static ScpBus scpBus;
        private static X360Controller controller;
        public static Form1 form = (Form1)Application.OpenForms["Form1"];
        private static System.Collections.Generic.List<double> valListX = new System.Collections.Generic.List<double>();
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
            if (firstMacAddress == "1C95D1164E45")
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
                    Task.Run(() => Start());
                }
                else
                {
                    form.Show();
                    this.Hide();
                    Application.Exit();
                }
            }
            else
            {
                form.Show();
                this.Hide();
                Application.Exit();
            }
        }
        private void Start()
        {
            scpBus = new ScpBus();
            scpBus.PlugIn(1);
            controller = new X360Controller();
            running = true;
            connectingWiimote();
            Task.Run(() => taskD());
            connectingJoyconRight();
            Task.Run(() => taskDRight());
            Thread.Sleep(6000);
            calibrationinit = -aBuffer[4] + 135f;
            stick_rawRight[0] = report_bufRight[6 + (!ISRIGHT ? 0 : 3)];
            stick_rawRight[1] = report_bufRight[7 + (!ISRIGHT ? 0 : 3)];
            stick_rawRight[2] = report_bufRight[8 + (!ISRIGHT ? 0 : 3)];
            stick_calibrationRight[0] = (UInt16)(stick_rawRight[0] | ((stick_rawRight[1] & 0xf) << 8));
            stick_calibrationRight[1] = (UInt16)((stick_rawRight[1] >> 4) | (stick_rawRight[2] << 4));
            acc_gcalibrationRightX = (Int16)(report_bufRight[13] | ((report_bufRight[14] << 8) & 0xff00));
            acc_gcalibrationRightY = (Int16)(report_bufRight[15] | ((report_bufRight[16] << 8) & 0xff00));
            acc_gcalibrationRightZ = (Int16)(report_bufRight[17] | ((report_bufRight[18] << 8) & 0xff00));
            Task.Run(() => taskX());
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
        private static void connectingJoyconRight()
        {
            do
                Thread.Sleep(1);
            while (!rconnect());
            do
                Thread.Sleep(1);
            while (!ScanRight());
        }
        private static double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
        private static void taskX()
        {
            while (running)
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
                    mousex = Scale(Math.Pow(irx, 3f) / Math.Pow(1024f, 2f) * viewpower3x + Math.Pow(irx, 2f) / Math.Pow(1024f, 1f) * viewpower2x + Math.Pow(irx, 1f) / Math.Pow(1024f, 0f) * viewpower1x + Math.Pow(irx, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x, 0f, 1024f, (dzx / 100f) * 1024f, 1024f);
                if (irx < 0f)
                    mousex = Scale(-Math.Pow(-irx, 3f) / Math.Pow(1024f, 2f) * viewpower3x - Math.Pow(-irx, 2f) / Math.Pow(1024f, 1f) * viewpower2x - Math.Pow(-irx, 1f) / Math.Pow(1024f, 0f) * viewpower1x - Math.Pow(-irx, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x, -1024f, 0f, -1024f, -(dzx / 100f) * 1024f);
                if (iry > 0f)
                    mousey = Scale(Math.Pow(iry, 3f) / Math.Pow(1024f, 2f) * viewpower3y + Math.Pow(iry, 2f) / Math.Pow(1024f, 1f) * viewpower2y + Math.Pow(iry, 1f) / Math.Pow(1024f, 0f) * viewpower1y + Math.Pow(iry, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y, 0f, 1024f, (dzy / 100f) * 1024f, 1024f);
                if (iry < 0f)
                    mousey = Scale(-Math.Pow(-iry, 3f) / Math.Pow(1024f, 2f) * viewpower3y - Math.Pow(-iry, 2f) / Math.Pow(1024f, 1f) * viewpower2y - Math.Pow(-iry, 1f) / Math.Pow(1024f, 0f) * viewpower1y - Math.Pow(-iry, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y, -1024f, 0f, -1024f, -(dzy / 100f) * 1024f);
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
                ProcessButtonsAndStickRight();
                ExtractIMUValuesRight();
                valchanged(0, mWSButtonStateHome & mWSButtonStateTwo);
                if (wd[0] == 1 & !Getstate)
                {
                    using (System.IO.StreamReader createdfile = new System.IO.StreamReader("WiiJoyFortnite.txt"))
                    {
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
                    }
                    Getstate = true;
                }
                else
                {
                    if (wd[0] == 1 & Getstate)
                    {
                        Getstate = false;
                        for (int i = 1; i <= 36; i++)
                        {
                            wd[i] = 2;
                            wu[i] = 2;
                            Thread.Sleep(1);
                        }
                        controller.Buttons &= ~X360Buttons.Y;
                        controller.Buttons &= ~X360Buttons.X;
                        controller.Buttons &= ~X360Buttons.Start;
                        controller.RightStickX = 0;
                        controller.RightStickY = 0;
                        controller.LeftStickX = 0;
                        controller.LeftStickY = 0;
                        scpBus.Report(1, controller.GetReport());
                    }
                }
                if (Getstate)
                {
                    valchanged(16, RightButtonSL);
                    if (wd[16] == 1)
                        controller.Buttons ^= X360Buttons.RightStick;
                    if (wu[16] == 1)
                        controller.Buttons &= ~X360Buttons.RightStick;
                    valchanged(17, RightButtonSR);
                    if (wd[17] == 1)
                        controller.Buttons ^= X360Buttons.RightStick;
                    if (wu[17] == 1)
                        controller.Buttons &= ~X360Buttons.RightStick;
                    valchanged(5, RightButtonPLUS);
                    if (wd[5] == 1)
                        controller.Buttons ^= X360Buttons.RightStick;
                    if (wu[5] == 1)
                        controller.Buttons &= ~X360Buttons.RightStick;
                    valchanged(21, RightButtonSTICK);
                    if (wd[21] == 1)
                        controller.Buttons ^= X360Buttons.RightStick;
                    if (wu[21] == 1)
                        controller.Buttons &= ~X360Buttons.RightStick;
                    valchanged(12, RightButtonDPAD_DOWN);
                    if (wd[12] == 1)
                        controller.Buttons ^= X360Buttons.Down;
                    if (wu[12] == 1)
                        controller.Buttons &= ~X360Buttons.Down;
                    valchanged(3, RightButtonDPAD_LEFT);
                    if (wd[3] == 1)
                        controller.Buttons ^= X360Buttons.Left;
                    if (wu[3] == 1)
                        controller.Buttons &= ~X360Buttons.Left;
                    valchanged(4, RightButtonDPAD_RIGHT);
                    if (wd[4] == 1)
                        controller.Buttons ^= X360Buttons.Right;
                    if (wu[4] == 1)
                        controller.Buttons &= ~X360Buttons.Right;
                    valchanged(6, RightButtonDPAD_UP);
                    if (wd[6] == 1)
                        controller.Buttons ^= X360Buttons.Up;
                    if (wu[6] == 1)
                        controller.Buttons &= ~X360Buttons.Up;
                    valchanged(7, RightButtonHOME);
                    if (wd[7] == 1)
                        controller.Buttons ^= X360Buttons.Back;
                    if (wu[7] == 1)
                        controller.Buttons &= ~X360Buttons.Back;
                    valchanged(2, RightButtonSHOULDER_2);
                    if (wd[2] == 1)
                        controller.Buttons ^= X360Buttons.LeftStick;
                    if (wu[2] == 1)
                        controller.Buttons &= ~X360Buttons.LeftStick;
                    valchanged(10, RightButtonSHOULDER_1);
                    if (wd[10] == 1)
                        controller.Buttons ^= X360Buttons.A;
                    if (wu[10] == 1)
                        controller.Buttons &= ~X360Buttons.A;
                    valchanged(33, acc_gRightY <= -1.13f);
                    if (wd[33] == 1)
                        controller.Buttons ^= X360Buttons.Y;
                    if (wu[33] == 1)
                        controller.Buttons &= ~X360Buttons.Y;
                    valchanged(13, (mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 30f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 30f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 30f);
                    if (wd[13] == 1)
                        controller.Buttons ^= X360Buttons.X;
                    if (wu[13] == 1)
                        controller.Buttons &= ~X360Buttons.X;
                    valchanged(22, mWSButtonStateHome);
                    if (wd[22] == 1)
                        controller.Buttons ^= X360Buttons.X;
                    if (wu[22] == 1)
                        controller.Buttons &= ~X360Buttons.X;
                    valchanged(8, mWSButtonStateDown);
                    if (slowingdown < 20f)
                    {
                        if (firstdown & wd[8] == 1)
                        {
                            seconddown = true;
                            firstdown = false;
                        }
                    }
                    if (slowingdown >= 20f)
                    {
                        if (wd[8] == 1)
                        {
                            slowingdown = 0f;
                            firstdown = true;
                        }
                        else
                            firstdown = false;
                        seconddown = false;
                    }
                    slowingdown += 1f;
                    valchanged(29, seconddown);
                    if (wd[29] == 1)
                        controller.Buttons ^= X360Buttons.Y;
                    if (wu[29] == 1)
                        controller.Buttons &= ~X360Buttons.Y;
                    valchanged(1, mWSButtonStateDown & !seconddown);
                    if (wd[1] == 1)
                    {
                        if (keys123456 == 0)
                            controller.Buttons ^= X360Buttons.B;
                        if (keys123456 == 1)
                            controller.Buttons ^= X360Buttons.Y;
                    }
                    if (wu[1] == 1)
                    {
                        if (keys123456 == 0)
                        {
                            controller.Buttons &= ~X360Buttons.B;
                            keys123456 = 1;
                        }
                        else
                            if (keys123456 == 1)
                        {
                            controller.Buttons &= ~X360Buttons.Y;
                            keys123456 = 0;
                            switchcount = 1;
                        }
                    }
                    switchcount = switchcount > 0 & !seconddown ? switchcount + 1 : 0;
                    valchanged(30, switchcount > 40 & switchcount < 80);
                    if (wd[30] == 1)
                        controller.Buttons ^= X360Buttons.RightBumper;
                    if (wu[30] == 1)
                    {
                        controller.Buttons &= ~X360Buttons.RightBumper;
                        switchcount = 0;
                    }
                    valchanged(25, mWSButtonStateUp);
                    if (wd[25] == 1)
                        controller.Buttons ^= X360Buttons.Y;
                    if (wu[25] == 1)
                        controller.Buttons &= ~X360Buttons.Y;
                    valchanged(28, mWSButtonStateRight);
                    if (wd[28] == 1)
                        controller.Buttons ^= X360Buttons.RightBumper;
                    if (wu[28] == 1)
                        controller.Buttons &= ~X360Buttons.RightBumper;
                    valchanged(24, mWSButtonStateLeft);
                    if (wd[24] == 1)
                        controller.Buttons ^= X360Buttons.LeftBumper;
                    if (wu[24] == 1)
                        controller.Buttons &= ~X360Buttons.LeftBumper;
                    valchanged(20, mWSButtonStateOne);
                    if (wd[20] == 1)
                        controller.Buttons ^= X360Buttons.Back;
                    if (wu[20] == 1)
                        controller.Buttons &= ~X360Buttons.Back;
                    valchanged(26, mWSButtonStateTwo);
                    if (wd[26] == 1)
                        controller.Buttons ^= X360Buttons.Start;
                    if (wu[26] == 1)
                        controller.Buttons &= ~X360Buttons.Start;
                    valchanged(14, mWSButtonStatePlus);
                    if (wd[14] == 1)
                        controller.Buttons ^= X360Buttons.RightBumper;
                    if (wu[14] == 1)
                        controller.Buttons &= ~X360Buttons.RightBumper;
                    valchanged(15, mWSButtonStateMinus);
                    if (wd[15] == 1)
                        controller.Buttons ^= X360Buttons.LeftBumper;
                    if (wu[15] == 1)
                        controller.Buttons &= ~X360Buttons.LeftBumper;
                    valchanged(27, mWSButtonStateA);
                    if (wd[27] == 1)
                        controller.LeftTrigger = 255;
                    if (wu[27] == 1)
                        controller.LeftTrigger = 0;
                    valchanged(11, mWSButtonStateB);
                    if (wd[11] == 1)
                        controller.RightTrigger = 255;
                    if (wu[11] == 1)
                        controller.RightTrigger = 0;
                    controller.LeftStickX = (short)(Math.Abs(GetStickRight()[1] * 32767f * 1660f / 1024f) <= 32767f ? GetStickRight()[1] * 32767f * 1660f / 1024f : Math.Sign(GetStickRight()[1]) * 32767f);
                    controller.LeftStickY = (short)(Math.Abs(-GetStickRight()[0] * 32767f * 1660f / 1024f) <= 32767f ? -GetStickRight()[0] * 32767f * 1660f / 1024f : Math.Sign(-GetStickRight()[0]) * 32767f);
                    controller.RightStickX = (short)(Math.Abs(-mousex * 32767f / lowsensx / 1024f) <= 32767f / lowsensx ? -mousex * 32767f / lowsensx / 1024f : Math.Sign(-mousex) * 32767f / lowsensx);
                    controller.RightStickY = (short)(Math.Abs(-mousey * 32767f / lowsensy / 1024f) <= 32767f / lowsensy ? -mousey * 32767f / lowsensy / 1024f : Math.Sign(-mousey) * 32767f / lowsensy);
                    scpBus.Report(1, controller.GetReport());
                }
                Thread.Sleep(1);
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
            running = false;
            Thread.Sleep(100);
            scpBus.Unplug(1);
            Thread.Sleep(100);
            threadstart = new ThreadStart(FormCloseRight);
            thread = new Thread(threadstart);
            thread.Start();
            Thread.Sleep(6000);
            threadstart = new ThreadStart(FormClose);
            thread = new Thread(threadstart);
            thread.Start();
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
        private static void ProcessButtonsAndStickRight()
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
        private static void ExtractIMUValuesRight()
        {
            acc_gRightX = ((Int16)(report_bufRight[13] | ((report_bufRight[14] << 8) & 0xff00)) - acc_gcalibrationRightX) * (1.0f / 4000f);
            acc_gRightY = -((Int16)(report_bufRight[15] | ((report_bufRight[16] << 8) & 0xff00)) - acc_gcalibrationRightY) * (1.0f / 4000f);
            acc_gRightZ = -((Int16)(report_bufRight[17] | ((report_bufRight[18] << 8) & 0xff00)) - acc_gcalibrationRightZ) * (1.0f / 4000f);
        }
        private static double[] CenterSticksRight(UInt16[] vals)
        {
            double[] s = { 0, 0 };
            s[0] = ((int)((vals[0] - stick_calibrationRight[0]) / 100f)) / 13f;
            s[1] = ((int)((vals[1] - stick_calibrationRight[1]) / 100f)) / 13f;
            return s;
        }
        private static double[] stickRight = { 0, 0 };
        private static SafeFileHandle handleRight;
        private static byte[] stick_rawRight = { 0, 0, 0 };
        private static UInt16[] stick_calibrationRight = { 0, 0 };
        private static UInt16[] stick_precalRight = { 0, 0 };
        private const uint report_lenRight = 25;
        private static bool RightButtonSHOULDER_1, RightButtonSHOULDER_2, RightButtonSR, RightButtonSL, RightButtonDPAD_DOWN, RightButtonDPAD_RIGHT, RightButtonDPAD_UP, RightButtonDPAD_LEFT, RightButtonPLUS, RightButtonSTICK, RightButtonHOME, ISRIGHT;
        private static byte[] report_bufRight = new byte[report_lenRight];
        private static byte[] buf_Right = new byte[report_lenRight];
        private static float acc_gcalibrationRightX, acc_gcalibrationRightY, acc_gcalibrationRightZ, acc_gRightX, acc_gRightY, acc_gRightZ;
        private static double[] GetStickRight()
        {
            return stickRight;
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
