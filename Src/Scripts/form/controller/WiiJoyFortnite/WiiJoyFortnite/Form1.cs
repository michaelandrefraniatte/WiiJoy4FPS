using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using controller;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Security.Cryptography;
namespace WiiJoyFortnite
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
        private static unsafe extern int Lhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_write")]
        private static unsafe extern int Lhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_open_path")]
        private static unsafe extern SafeFileHandle Lhid_open_path(IntPtr handle);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_close")]
        private static unsafe extern void Lhid_close(SafeFileHandle device);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private const double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe;
        private static double irx0, iry0, irx1, iry1, irx, iry, irxc, iryc, mWSIRSensors0X, mWSIRSensors0Y, mWSIRSensors1X, mWSIRSensors1Y, mWSButtonStateIRX, mWSButtonStateIRY, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, calibrationinit, mousex, mousey, mWSIRSensors0Xcam, mWSIRSensors0Ycam, mWSIRSensors1Xcam, mWSIRSensors1Ycam, mWSIRSensorsXcam, mWSIRSensorsYcam, viewpower05x = 0f, viewpower1x = 0.25f, viewpower2x = 0f, viewpower3x = 0.75f, viewpower05y = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, dzx = 4.0f, dzy = 4.0f, lowsensx = 1f, lowsensy = 1f, centery = 140f, switchcount;
        private static bool mWSIR1found, mWSIR0found, mWSButtonStateA, mWSButtonStateB, mWSButtonStateMinus, mWSButtonStateHome, mWSButtonStatePlus, mWSButtonStateOne, mWSButtonStateTwo, mWSButtonStateUp, mWSButtonStateDown, mWSButtonStateLeft, mWSButtonStateRight, running, LeftButtonStickRight, LeftButtonStickLeft, LeftButtonStickUp, LeftButtonStickDown;
        private static byte[] buff = new byte[] { 0x55 }, mBuff = new byte[22], aBuffer = new byte[22];
        private const byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        private static uint CurrentResolution = 0;
        private static string username, hash, userchecksum;
        const string alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        private static FileStream mStream;
        private static SafeFileHandle handle = null;
        private static ThreadStart threadstart;
        private static Thread thread;
        private static ScpBus scpBus;
        private static X360Controller controller;
        public static Form1 form = (Form1)Application.OpenForms["Form1"];
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
            try
            {
                TimeBeginPeriod(1);
                NtSetTimerResolution(1, true, ref CurrentResolution);
                SerProcessPriority();
                using (System.IO.StreamReader file = new System.IO.StreamReader("auth.txt"))
                {
                    username = file.ReadLine();
                    hash = file.ReadLine();
                    file.Close();
                }
                String thisprocessname = Process.GetCurrentProcess().ProcessName;
                SHA1 sha1 = SHA1.Create();
                FileStream fs = new FileStream(thisprocessname + ".exe", FileMode.Open, FileAccess.Read);
                string checksum = BitConverter.ToString(sha1.ComputeHash(fs)).Replace("-", "");
                fs.Close();
                userchecksum = username + checksum;
                string salt = GetSalt(10);
                string hashedPass = HashPassword(salt, userchecksum);
                if (hash != hashedPass)
                    Application.Exit();
                if (getUniqueId() == "80158F43-BFEBFBFF000906EA-3A7A691C-5826-2020-0118-164738000000" & !AlreadyRunning())
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
            catch
            {
                Application.Exit();
            }
        }
        public static string GetSalt(int saltSize)
        {
            float key = 0.6f;
            StringBuilder strB = new StringBuilder("");
            while ((saltSize--) > 0)
                strB.Append(alphanumeric[(int)(key * alphanumeric.Length)]);
            return strB.ToString();
        }
        public static string HashPassword(string salt, string password)
        {
            string mergedPass = string.Concat(salt, password);
            return EncryptUsingMD5(mergedPass);
        }
        public static string EncryptUsingMD5(string inputStr)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(inputStr));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                    sBuilder.Append(data[i].ToString("x2"));
                return sBuilder.ToString();
            }
        }
        public static string getUniqueId()
        {
            try
            {
                string cpuInfo = string.Empty;
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
                string drive = "C";
                ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
                dsk.Get();
                string volumeSerial = dsk["VolumeSerialNumber"].ToString();
                string uuidInfo = string.Empty;
                ManagementClass mcu = new ManagementClass("Win32_ComputerSystemProduct");
                ManagementObjectCollection mocu = mcu.GetInstances();
                foreach (ManagementObject mou in mocu)
                {
                    uuidInfo = mou.Properties["UUID"].Value.ToString();
                    break;
                }
                if (volumeSerial != null & volumeSerial != "" & cpuInfo != null & cpuInfo != "" & uuidInfo != null & uuidInfo != "")
                    return volumeSerial + "-" + cpuInfo + "-" + uuidInfo;
                else
                    return null;
            }
            catch
            {
                Application.Exit();
                return null;
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
        private static void SerProcessPriority()
        {
            using (Process p = Process.GetCurrentProcess())
            {
                p.PriorityClass = ProcessPriorityClass.RealTime;
            }
        }
        private static bool AlreadyRunning()
        {
            String thisprocessname = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(thisprocessname);
            if (processes.Length > 1)
                return true;
            else
                return false;
        }
        private void Start()
        {
            scpBus = new ScpBus();
            scpBus.PlugIn(1);
            controller = new X360Controller();
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
        private static void connectingJoyconLeft()
        {
            do
                Thread.Sleep(1);
            while (!lconnect());
            do
                Thread.Sleep(1);
            while (!ScanLeft());
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
                ProcessButtonsAndStickLeft();
                ExtractIMUValuesLeft();
                valchanged(1, LeftButtonDPAD_DOWN);
                if (wd[1] == 1)
                    controller.Buttons ^= X360Buttons.Down;
                if (wu[1] == 1)
                    controller.Buttons &= ~X360Buttons.Down;
                valchanged(2, LeftButtonDPAD_LEFT);
                if (wd[2] == 1)
                    controller.Buttons ^= X360Buttons.Left;
                if (wu[2] == 1)
                    controller.Buttons &= ~X360Buttons.Left;
                valchanged(3, LeftButtonDPAD_RIGHT);
                if (wd[3] == 1)
                    controller.Buttons ^= X360Buttons.Right;
                if (wu[3] == 1)
                    controller.Buttons &= ~X360Buttons.Right;
                valchanged(4, LeftButtonDPAD_UP);
                if (wd[4] == 1)
                    controller.Buttons ^= X360Buttons.Up;
                if (wu[4] == 1)
                    controller.Buttons &= ~X360Buttons.Up;
                valchanged(5, LeftButtonSL | LeftButtonSR | LeftButtonMINUS);
                if (wd[5] == 1)
                    controller.Buttons ^= X360Buttons.RightStick;
                if (wu[5] == 1)
                    controller.Buttons &= ~X360Buttons.RightStick;
                valchanged(6, LeftButtonSHOULDER_2);
                if (wd[6] == 1)
                    controller.Buttons ^= X360Buttons.LeftStick;
                if (wu[6] == 1)
                    controller.Buttons &= ~X360Buttons.LeftStick;
                valchanged(7, LeftButtonSHOULDER_1);
                if (wd[7] == 1)
                    controller.Buttons ^= X360Buttons.A;
                if (wu[7] == 1)
                    controller.Buttons &= ~X360Buttons.A;
                valchanged(8, mWSButtonStateOne);
                if (wd[8] == 1)
                    controller.Buttons ^= X360Buttons.Back;
                if (wu[8] == 1)
                    controller.Buttons &= ~X360Buttons.Back;
                valchanged(9, mWSButtonStateTwo | LeftButtonCAPTURE);
                if (wd[9] == 1)
                    controller.Buttons ^= X360Buttons.Start;
                if (wu[9] == 1)
                    controller.Buttons &= ~X360Buttons.Start;
                valchanged(10, mWSButtonStateHome | ((mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 30f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 30f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 30f));
                if (wd[10] == 1)
                    controller.Buttons ^= X360Buttons.X;
                if (wu[10] == 1)
                    controller.Buttons &= ~X360Buttons.X;
                valchanged(11, mWSButtonStatePlus);
                if (wd[11] == 1)
                    controller.Buttons ^= X360Buttons.RightBumper;
                if (wu[11] == 1)
                    controller.Buttons &= ~X360Buttons.RightBumper;
                valchanged(12, mWSButtonStateMinus);
                if (wd[12] == 1)
                    controller.Buttons ^= X360Buttons.LeftBumper;
                if (wu[12] == 1)
                    controller.Buttons &= ~X360Buttons.LeftBumper;
                valchanged(13, mWSButtonStateDown);
                if (wd[13] == 1)
                    controller.Buttons ^= X360Buttons.B;
                if (wu[13] == 1)
                    controller.Buttons &= ~X360Buttons.B;
                valchanged(14, acc_gLeftY <= -1.13 | mWSButtonStateUp);
                if (wd[14] == 1)
                    controller.Buttons ^= X360Buttons.Y;
                if (wu[14] == 1)
                    controller.Buttons &= ~X360Buttons.Y;
                valchanged(15, mWSButtonStateLeft | mWSButtonStateRight);
                if (wd[15] == 1)
                    controller.Buttons ^= X360Buttons.Y;
                if (wu[15] == 1)
                {
                    controller.Buttons &= ~X360Buttons.Y;
                    switchcount = 1;
                }
                switchcount = switchcount > 0 ? switchcount + 1 : 0;
                valchanged(16, switchcount > 40 & switchcount < 80);
                if (wd[16] == 1)
                    controller.Buttons ^= X360Buttons.RightBumper;
                if (wu[16] == 1)
                {
                    controller.Buttons &= ~X360Buttons.RightBumper;
                    switchcount = 0;
                }
                valchanged(17, mWSButtonStateA);
                if (wd[17] == 1)
                    controller.LeftTrigger = 255;
                if (wu[17] == 1)
                    controller.LeftTrigger = 0;
                valchanged(18, mWSButtonStateB);
                if (wd[18] == 1)
                    controller.RightTrigger = 255;
                if (wu[18] == 1)
                    controller.RightTrigger = 0;
                if (mWSButtonStateB)
                    controller.RightTrigger = 255;
                else
                    controller.RightTrigger = 0;
                if (mWSButtonStateA)
                    controller.LeftTrigger = 255;
                else
                    controller.LeftTrigger = 0;
                changeScale(dzx, dzy);
                changeView(lowsensx, lowsensy);
                LeftButtonStickUp = GetStickLeft()[1] > 0.25f;
                LeftButtonStickDown = GetStickLeft()[1] < -0.25f;
                if (LeftButtonStickUp & !LeftButtonStickDown)
                    controller.LeftStickY = 32767;
                else
                if (!LeftButtonStickDown)
                    controller.LeftStickY = 0;
                if (LeftButtonStickDown & !LeftButtonStickUp)
                    controller.LeftStickY = -32767;
                else
                if (!LeftButtonStickUp)
                    controller.LeftStickY = 0;
                LeftButtonStickRight = GetStickLeft()[0] > 0.25f;
                LeftButtonStickLeft = GetStickLeft()[0] < -0.25f;
                if (LeftButtonStickRight & !LeftButtonStickLeft)
                    controller.LeftStickX = 32767;
                else
                if (!LeftButtonStickLeft)
                    controller.LeftStickX = 0;
                if (LeftButtonStickLeft & !LeftButtonStickRight)
                    controller.LeftStickX = -32767;
                else
                if (!LeftButtonStickRight)
                    controller.LeftStickX = 0;
                scpBus.Report(controller.GetReport());
                Thread.Sleep(1);
            }
        }
        private static void changeScale(double dzchangedx, double dzchangedy)
        {
            if (irx >= 0f)
                mousex = Scale(Math.Pow(irx, 3f) / Math.Pow(1024f, 2f) * viewpower3x + Math.Pow(irx, 2f) / Math.Pow(1024f, 1f) * viewpower2x + Math.Pow(irx, 1f) / Math.Pow(1024f, 0f) * viewpower1x + Math.Pow(irx, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x, 0f, 1024f, (dzchangedx / 100f) * 1024f, 1024f);
            if (irx <= 0f)
                mousex = Scale(-Math.Pow(-irx, 3f) / Math.Pow(1024f, 2f) * viewpower3x - Math.Pow(-irx, 2f) / Math.Pow(1024f, 1f) * viewpower2x - Math.Pow(-irx, 1f) / Math.Pow(1024f, 0f) * viewpower1x - Math.Pow(-irx, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x, -1024f, 0f, -1024f, -(dzchangedx / 100f) * 1024f);
            if (iry >= 0f)
                mousey = Scale(Math.Pow(iry, 3f) / Math.Pow(1024f, 2f) * viewpower3y + Math.Pow(iry, 2f) / Math.Pow(1024f, 1f) * viewpower2y + Math.Pow(iry, 1f) / Math.Pow(1024f, 0f) * viewpower1y + Math.Pow(iry, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y, 0f, 1024f, (dzchangedy / 100f) * 1024f, 1024f);
            if (iry <= 0f)
                mousey = Scale(-Math.Pow(-iry, 3f) / Math.Pow(1024f, 2f) * viewpower3y - Math.Pow(-iry, 2f) / Math.Pow(1024f, 1f) * viewpower2y - Math.Pow(-iry, 1f) / Math.Pow(1024f, 0f) * viewpower1y - Math.Pow(-iry, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y, -1024f, 0f, -1024f, -(dzchangedy / 100f) * 1024f);
        }
        private static void changeView(double lowsenschangedx, double lowsenschangedy)
        {
            controller.RightStickX = (short)(-mousex / 1024f / lowsenschangedx * 32767f);
            controller.RightStickY = (short)(-mousey / 1024f / lowsenschangedy * 32767f);
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
                    scpBus.Unplug(1);
                    Thread.Sleep(100);
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
        private static void ProcessButtonsAndStickLeft()
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
        private static void ExtractIMUValuesLeft()
        {
            acc_gLeftX = ((Int16)(report_bufLeft[13] | ((report_bufLeft[14] << 8) & 0xff00)) - acc_gcalibrationLeftX) * (1.0f / 4000f);
            acc_gLeftY = ((Int16)(report_bufLeft[15] | ((report_bufLeft[16] << 8) & 0xff00)) - acc_gcalibrationLeftY) * (1.0f / 4000f);
            acc_gLeftZ = ((Int16)(report_bufLeft[17] | ((report_bufLeft[18] << 8) & 0xff00)) - acc_gcalibrationLeftZ) * (1.0f / 4000f);
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
        private static byte[] stick_rawLeft = { 0, 0, 0 };
        private static UInt16[] stick_calibrationLeft = { 0, 0 };
        private static UInt16[] stick_precalLeft = { 0, 0 };
        private const uint report_lenLeft = 25;
        private static bool LeftButtonSHOULDER_1, LeftButtonSHOULDER_2, LeftButtonSR, LeftButtonSL, LeftButtonDPAD_DOWN, LeftButtonDPAD_RIGHT, LeftButtonDPAD_UP, LeftButtonDPAD_LEFT, LeftButtonMINUS, LeftButtonSTICK, LeftButtonCAPTURE, ISLEFT;
        private static byte[] report_bufLeft = new byte[report_lenLeft];
        private static byte[] buf_Left = new byte[report_lenLeft];
        private static float acc_gLeftX, acc_gLeftY, acc_gLeftZ, acc_gcalibrationLeftX, acc_gcalibrationLeftY, acc_gcalibrationLeftZ;
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
                SubcommandLeft(0x60, new byte[] { 0x98 }, 1);
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
