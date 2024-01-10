using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Win32.SafeHandles;
using SharpDX.DirectInput;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms.VisualStyles;

namespace WiiJoyCoDMP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            ExtractResourceToFile("MotionInputPairing");
        }
        public System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) 
                return null;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return System.Reflection.Assembly.Load(bytes);
        }
        public void ExtractResourceToFile(string dllName)
        {
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            using (System.IO.FileStream fs = new System.IO.FileStream(@"C:\Windows\System32\" + dllName + ".dll", System.IO.FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
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
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static bool back, start, A, B, X, Y, up, left, down, right, leftstick, rightstick, leftbumper, rightbumper, lefttrigger, righttrigger;
        private static double leftstickx, leftsticky, rightstickx, rightsticky;
        private const double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe;
        private static double irx0, iry0, irx1, iry1, irx, iry, irxc, iryc, mWSIRSensors0X, mWSIRSensors0Y, mWSIRSensors1X, mWSIRSensors1Y, mWSButtonStateIRX, mWSButtonStateIRY, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, calibrationinit, mousex, mousey, mWSIRSensors0Xcam, mWSIRSensors0Ycam, mWSIRSensors1Xcam, mWSIRSensors1Ycam, mWSIRSensorsXcam, mWSIRSensorsYcam, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, dzx = 2.0f, dzy = 2.0f, centery = 140f;
        private static bool mWSIR1found, mWSIR0found, mWSButtonStateA, mWSButtonStateB, mWSButtonStateMinus, mWSButtonStateHome, mWSButtonStatePlus, mWSButtonStateOne, mWSButtonStateTwo, mWSButtonStateUp, mWSButtonStateDown, mWSButtonStateLeft, mWSButtonStateRight, getstate, running;
        private static byte[] buff = new byte[] { 0x55 }, mBuff = new byte[22], aBuffer = new byte[22];
        private const byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        private static uint CurrentResolution = 0;
        private static FileStream mStream;
        private static SafeFileHandle handle = null;
        private static DirectInput directInput = new DirectInput();
        private static int[] wd = { 2 };
        private static int[] wu = { 2 };
        public static void valchanged(int n, bool val)
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
        private void Start()
        {
            running = true;
            do
                Thread.Sleep(1);
            while (!wiimotejoyconleftconnect());
            Scan();
            Task.Run(() => taskD());
            Thread.Sleep(2000);
            calibrationinit = -aBuffer[4] + 135f;
            DirectInputHookConnect();
            ScpBus.LoadController();
            Task.Run(() => taskX());
            this.WindowState = FormWindowState.Minimized;
        }
        private void taskX()
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
                if (irx >= 0f)
                    mousex = Scale(irx * irx * irx / 1024f / 1024f * viewpower3x + irx * irx / 1024f * viewpower2x + irx * viewpower1x, 0f, 1024f, (dzx / 100f) * 1024f, 1024f);
                if (irx <= 0f)
                    mousex = Scale(-(-irx * -irx * -irx) / 1024f / 1024f * viewpower3x - (-irx * -irx) / 1024f * viewpower2x - (-irx) * viewpower1x, -1024f, 0f, -1024f, -(dzx / 100f) * 1024f);
                if (iry >= 0f)
                    mousey = Scale(iry * iry * iry / 1024f / 1024f * viewpower3y + iry * iry / 1024f * viewpower2y + iry * viewpower1y, 0f, 1024f, (dzy / 100f) * 1024f, 1024f);
                if (iry <= 0f)
                    mousey = Scale(-(-iry * -iry * -iry) / 1024f / 1024f * viewpower3y - (-iry * -iry) / 1024f * viewpower2y - (-iry) * viewpower1y, -1024f, 0f, -1024f, -(dzy / 100f) * 1024f);
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
                GamepadProcess();
                down = Joystick1Buttons1;
                left = Joystick1Buttons0;
                right = Joystick1Buttons3;
                up = Joystick1Buttons2;
                rightstick = Joystick1Buttons4 | Joystick1Buttons5 | Joystick1Buttons8;
                leftstick = Joystick1Buttons15;
                A = Joystick1Buttons14;
                back = mWSButtonStateOne;
                start = mWSButtonStateTwo;
                X = mWSButtonStateHome | ((mWSRawValuesZ > 0 ? mWSRawValuesZ : -mWSRawValuesZ) >= 30f & (mWSRawValuesY > 0 ? mWSRawValuesY : -mWSRawValuesY) >= 30f & (mWSRawValuesX > 0 ? mWSRawValuesX : -mWSRawValuesX) >= 30f);
                rightbumper = mWSButtonStatePlus | mWSButtonStateUp;
                leftbumper = mWSButtonStateMinus | mWSButtonStateUp;
                B = mWSButtonStateDown;
                Y = mWSButtonStateLeft | mWSButtonStateRight;
                righttrigger = mWSButtonStateB;
                valchanged(0, mWSButtonStateA);
                if (wd[0] == 1 & !getstate)
                {
                    getstate = true;
                }
                else
                {
                    if (wd[0] == 1 & getstate)
                    {
                        getstate = false;
                    }
                }
                if (X | Y | rightbumper | leftbumper | rightstick | leftstick | back | start)
                {
                    getstate = false;
                }
                lefttrigger = getstate;
                rightstickx = (short)(-mousex / 1024f * 32767f);
                rightsticky = (short)(-mousey / 1024f * 32767f);
                if (Joystick1PointOfViewControllers0 == 27000 | Joystick1PointOfViewControllers0 == 22500 | Joystick1PointOfViewControllers0 == 31500)
                    leftsticky = 32767;
                if (Joystick1PointOfViewControllers0 == 9000 | Joystick1PointOfViewControllers0 == 13500 | Joystick1PointOfViewControllers0 == 4500)
                    leftsticky = -32767;
                if (Joystick1PointOfViewControllers0 == -1 | (Joystick1PointOfViewControllers0 != 27000 & Joystick1PointOfViewControllers0 != 22500 & Joystick1PointOfViewControllers0 != 31500 & Joystick1PointOfViewControllers0 != 9000 & Joystick1PointOfViewControllers0 != 13500 & Joystick1PointOfViewControllers0 != 4500))
                    leftsticky = 0;
                if (Joystick1PointOfViewControllers0 == 18000 | Joystick1PointOfViewControllers0 == 13500 | Joystick1PointOfViewControllers0 == 22500)
                    leftstickx = -32767;
                if (Joystick1PointOfViewControllers0 == 0 | Joystick1PointOfViewControllers0 == 4500 | Joystick1PointOfViewControllers0 == 31500)
                    leftstickx = 32767;
                if (Joystick1PointOfViewControllers0 == -1 | (Joystick1PointOfViewControllers0 != 18000 & Joystick1PointOfViewControllers0 != 13500 & Joystick1PointOfViewControllers0 != 22500 & Joystick1PointOfViewControllers0 != 0 & Joystick1PointOfViewControllers0 != 4500 & Joystick1PointOfViewControllers0 != 31500))
                    leftstickx = 0;
                ScpBus.SetController(back, start, A, B, X, Y, up, left, down, right, leftstick, rightstick, leftbumper, rightbumper, lefttrigger, righttrigger, leftstickx, leftsticky, rightstickx, rightsticky);
                Thread.Sleep(1);
            }
        }
        private static double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
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
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                ScpBus.UnLoadController();
                Thread.Sleep(100);
                mStream.Close();
                handle.Close();
                wiimotedisconnect();
                joyconleftdisconnect();
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
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimotejoyconleftconnect")]
        public static extern bool wiimotejoyconleftconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "joyconleftdisconnect")]
        public static extern bool joyconleftdisconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimotedisconnect")]
        public static extern bool wiimotedisconnect();
        private static bool Scan()
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
                    if ((diDetail.DevicePath.Contains(vendor_id) | diDetail.DevicePath.Contains(vendor_id_)) & (diDetail.DevicePath.Contains(product_r1) | diDetail.DevicePath.Contains(product_r2)))
                    {
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
        private static Joystick[] joystick = new Joystick[] { null, null };
        private static Guid[] joystickGuid = new Guid[] { Guid.Empty, Guid.Empty };
        private static int dinum = 0;
        public static int Joystick1AxisX;
        public static int Joystick1AxisY;
        public static int Joystick1AxisZ;
        public static int Joystick1RotationX;
        public static int Joystick1RotationY;
        public static int Joystick1RotationZ;
        public static int Joystick1Sliders0;
        public static int Joystick1Sliders1;
        public static int Joystick1PointOfViewControllers0;
        public static int Joystick1PointOfViewControllers1;
        public static int Joystick1PointOfViewControllers2;
        public static int Joystick1PointOfViewControllers3;
        public static int Joystick1VelocityX;
        public static int Joystick1VelocityY;
        public static int Joystick1VelocityZ;
        public static int Joystick1AngularVelocityX;
        public static int Joystick1AngularVelocityY;
        public static int Joystick1AngularVelocityZ;
        public static int Joystick1VelocitySliders0;
        public static int Joystick1VelocitySliders1;
        public static int Joystick1AccelerationX;
        public static int Joystick1AccelerationY;
        public static int Joystick1AccelerationZ;
        public static int Joystick1AngularAccelerationX;
        public static int Joystick1AngularAccelerationY;
        public static int Joystick1AngularAccelerationZ;
        public static int Joystick1AccelerationSliders0;
        public static int Joystick1AccelerationSliders1;
        public static int Joystick1ForceX;
        public static int Joystick1ForceY;
        public static int Joystick1ForceZ;
        public static int Joystick1TorqueX;
        public static int Joystick1TorqueY;
        public static int Joystick1TorqueZ;
        public static int Joystick1ForceSliders0;
        public static int Joystick1ForceSliders1;
        public static bool Joystick1Buttons0, Joystick1Buttons1, Joystick1Buttons2, Joystick1Buttons3, Joystick1Buttons4, Joystick1Buttons5, Joystick1Buttons6, Joystick1Buttons7, Joystick1Buttons8, Joystick1Buttons9, Joystick1Buttons10, Joystick1Buttons11, Joystick1Buttons12, Joystick1Buttons13, Joystick1Buttons14, Joystick1Buttons15, Joystick1Buttons16, Joystick1Buttons17, Joystick1Buttons18, Joystick1Buttons19, Joystick1Buttons20, Joystick1Buttons21, Joystick1Buttons22, Joystick1Buttons23, Joystick1Buttons24, Joystick1Buttons25, Joystick1Buttons26, Joystick1Buttons27, Joystick1Buttons28, Joystick1Buttons29, Joystick1Buttons30, Joystick1Buttons31, Joystick1Buttons32, Joystick1Buttons33, Joystick1Buttons34, Joystick1Buttons35, Joystick1Buttons36, Joystick1Buttons37, Joystick1Buttons38, Joystick1Buttons39, Joystick1Buttons40, Joystick1Buttons41, Joystick1Buttons42, Joystick1Buttons43, Joystick1Buttons44, Joystick1Buttons45, Joystick1Buttons46, Joystick1Buttons47, Joystick1Buttons48, Joystick1Buttons49, Joystick1Buttons50, Joystick1Buttons51, Joystick1Buttons52, Joystick1Buttons53, Joystick1Buttons54, Joystick1Buttons55, Joystick1Buttons56, Joystick1Buttons57, Joystick1Buttons58, Joystick1Buttons59, Joystick1Buttons60, Joystick1Buttons61, Joystick1Buttons62, Joystick1Buttons63, Joystick1Buttons64, Joystick1Buttons65, Joystick1Buttons66, Joystick1Buttons67, Joystick1Buttons68, Joystick1Buttons69, Joystick1Buttons70, Joystick1Buttons71, Joystick1Buttons72, Joystick1Buttons73, Joystick1Buttons74, Joystick1Buttons75, Joystick1Buttons76, Joystick1Buttons77, Joystick1Buttons78, Joystick1Buttons79, Joystick1Buttons80, Joystick1Buttons81, Joystick1Buttons82, Joystick1Buttons83, Joystick1Buttons84, Joystick1Buttons85, Joystick1Buttons86, Joystick1Buttons87, Joystick1Buttons88, Joystick1Buttons89, Joystick1Buttons90, Joystick1Buttons91, Joystick1Buttons92, Joystick1Buttons93, Joystick1Buttons94, Joystick1Buttons95, Joystick1Buttons96, Joystick1Buttons97, Joystick1Buttons98, Joystick1Buttons99, Joystick1Buttons100, Joystick1Buttons101, Joystick1Buttons102, Joystick1Buttons103, Joystick1Buttons104, Joystick1Buttons105, Joystick1Buttons106, Joystick1Buttons107, Joystick1Buttons108, Joystick1Buttons109, Joystick1Buttons110, Joystick1Buttons111, Joystick1Buttons112, Joystick1Buttons113, Joystick1Buttons114, Joystick1Buttons115, Joystick1Buttons116, Joystick1Buttons117, Joystick1Buttons118, Joystick1Buttons119, Joystick1Buttons120, Joystick1Buttons121, Joystick1Buttons122, Joystick1Buttons123, Joystick1Buttons124, Joystick1Buttons125, Joystick1Buttons126, Joystick1Buttons127;
        public bool DirectInputHookConnect()
        {
            try
            {
                directInput = new DirectInput();
                joystick = new Joystick[] { null, null };
                joystickGuid = new Guid[] { Guid.Empty, Guid.Empty };
                dinum = 0;
                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid[dinum] = deviceInstance.InstanceGuid;
                    dinum++;
                    if (dinum >= 2)
                    {
                        break;
                    }
                }
                if (dinum < 2)
                {
                    foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                    {
                        joystickGuid[dinum] = deviceInstance.InstanceGuid;
                        dinum++;
                        if (dinum >= 1)
                        {
                            break;
                        }
                    }
                }
            }
            catch { }
            if (joystickGuid[0] == Guid.Empty)
            {
                return false;
            }
            else
            {
                for (int inc = 0; inc < dinum; inc++)
                {
                    joystick[inc] = new Joystick(directInput, joystickGuid[inc]);
                    joystick[inc].Properties.BufferSize = 128;
                    joystick[inc].Acquire();
                }
                return true;
            }
        }
        private void GamepadProcess()
        {
            for (int inc = 0; inc < dinum; inc++)
            {
                joystick[inc].Poll();
                var datas = joystick[inc].GetBufferedData();
                foreach (var state in datas)
                {
                    if (inc == 0 & state.Offset == JoystickOffset.X)
                        Joystick1AxisX = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.Y)
                        Joystick1AxisY = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.Z)
                        Joystick1AxisZ = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.RotationX)
                        Joystick1RotationX = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.RotationY)
                        Joystick1RotationY = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.RotationZ)
                        Joystick1RotationZ = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.Sliders0)
                        Joystick1Sliders0 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.Sliders1)
                        Joystick1Sliders1 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.PointOfViewControllers0)
                        Joystick1PointOfViewControllers0 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.PointOfViewControllers1)
                        Joystick1PointOfViewControllers1 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.PointOfViewControllers2)
                        Joystick1PointOfViewControllers2 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.PointOfViewControllers3)
                        Joystick1PointOfViewControllers3 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.VelocityX)
                        Joystick1VelocityX = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.VelocityY)
                        Joystick1VelocityY = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.VelocityZ)
                        Joystick1VelocityZ = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AngularVelocityX)
                        Joystick1AngularVelocityX = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AngularVelocityY)
                        Joystick1AngularVelocityY = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AngularVelocityZ)
                        Joystick1AngularVelocityZ = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.VelocitySliders0)
                        Joystick1VelocitySliders0 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.VelocitySliders1)
                        Joystick1VelocitySliders1 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AccelerationX)
                        Joystick1AccelerationX = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AccelerationY)
                        Joystick1AccelerationY = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AccelerationZ)
                        Joystick1AccelerationZ = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AngularAccelerationX)
                        Joystick1AngularAccelerationX = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AngularAccelerationY)
                        Joystick1AngularAccelerationY = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AngularAccelerationZ)
                        Joystick1AngularAccelerationZ = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AccelerationSliders0)
                        Joystick1AccelerationSliders0 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.AccelerationSliders1)
                        Joystick1AccelerationSliders1 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.ForceX)
                        Joystick1ForceX = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.ForceY)
                        Joystick1ForceY = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.ForceZ)
                        Joystick1ForceZ = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.TorqueX)
                        Joystick1TorqueX = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.TorqueY)
                        Joystick1TorqueY = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.TorqueZ)
                        Joystick1TorqueZ = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.ForceSliders0)
                        Joystick1ForceSliders0 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.ForceSliders1)
                        Joystick1ForceSliders1 = state.Value;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons0 & state.Value == 128)
                        Joystick1Buttons0 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons0 & state.Value == 0)
                        Joystick1Buttons0 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons1 & state.Value == 128)
                        Joystick1Buttons1 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons1 & state.Value == 0)
                        Joystick1Buttons1 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons2 & state.Value == 128)
                        Joystick1Buttons2 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons2 & state.Value == 0)
                        Joystick1Buttons2 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons3 & state.Value == 128)
                        Joystick1Buttons3 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons3 & state.Value == 0)
                        Joystick1Buttons3 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons4 & state.Value == 128)
                        Joystick1Buttons4 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons4 & state.Value == 0)
                        Joystick1Buttons4 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons5 & state.Value == 128)
                        Joystick1Buttons5 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons5 & state.Value == 0)
                        Joystick1Buttons5 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons6 & state.Value == 128)
                        Joystick1Buttons6 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons6 & state.Value == 0)
                        Joystick1Buttons6 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons7 & state.Value == 128)
                        Joystick1Buttons7 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons7 & state.Value == 0)
                        Joystick1Buttons7 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons8 & state.Value == 128)
                        Joystick1Buttons8 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons8 & state.Value == 0)
                        Joystick1Buttons8 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons9 & state.Value == 128)
                        Joystick1Buttons9 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons9 & state.Value == 0)
                        Joystick1Buttons9 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons10 & state.Value == 128)
                        Joystick1Buttons10 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons10 & state.Value == 0)
                        Joystick1Buttons10 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons11 & state.Value == 128)
                        Joystick1Buttons11 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons11 & state.Value == 0)
                        Joystick1Buttons11 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons12 & state.Value == 128)
                        Joystick1Buttons12 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons12 & state.Value == 0)
                        Joystick1Buttons12 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons13 & state.Value == 128)
                        Joystick1Buttons13 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons13 & state.Value == 0)
                        Joystick1Buttons13 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons14 & state.Value == 128)
                        Joystick1Buttons14 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons14 & state.Value == 0)
                        Joystick1Buttons14 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons15 & state.Value == 128)
                        Joystick1Buttons15 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons15 & state.Value == 0)
                        Joystick1Buttons15 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons16 & state.Value == 128)
                        Joystick1Buttons16 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons16 & state.Value == 0)
                        Joystick1Buttons16 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons17 & state.Value == 128)
                        Joystick1Buttons17 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons17 & state.Value == 0)
                        Joystick1Buttons17 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons18 & state.Value == 128)
                        Joystick1Buttons18 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons18 & state.Value == 0)
                        Joystick1Buttons18 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons19 & state.Value == 128)
                        Joystick1Buttons19 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons19 & state.Value == 0)
                        Joystick1Buttons19 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons20 & state.Value == 128)
                        Joystick1Buttons20 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons20 & state.Value == 0)
                        Joystick1Buttons20 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons21 & state.Value == 128)
                        Joystick1Buttons21 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons21 & state.Value == 0)
                        Joystick1Buttons21 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons22 & state.Value == 128)
                        Joystick1Buttons22 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons22 & state.Value == 0)
                        Joystick1Buttons22 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons23 & state.Value == 128)
                        Joystick1Buttons23 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons23 & state.Value == 0)
                        Joystick1Buttons23 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons24 & state.Value == 128)
                        Joystick1Buttons24 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons24 & state.Value == 0)
                        Joystick1Buttons24 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons25 & state.Value == 128)
                        Joystick1Buttons25 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons25 & state.Value == 0)
                        Joystick1Buttons25 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons26 & state.Value == 128)
                        Joystick1Buttons26 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons26 & state.Value == 0)
                        Joystick1Buttons26 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons27 & state.Value == 128)
                        Joystick1Buttons27 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons27 & state.Value == 0)
                        Joystick1Buttons27 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons28 & state.Value == 128)
                        Joystick1Buttons28 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons28 & state.Value == 0)
                        Joystick1Buttons28 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons29 & state.Value == 128)
                        Joystick1Buttons29 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons29 & state.Value == 0)
                        Joystick1Buttons29 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons30 & state.Value == 128)
                        Joystick1Buttons30 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons30 & state.Value == 0)
                        Joystick1Buttons30 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons31 & state.Value == 128)
                        Joystick1Buttons31 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons31 & state.Value == 0)
                        Joystick1Buttons31 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons32 & state.Value == 128)
                        Joystick1Buttons32 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons32 & state.Value == 0)
                        Joystick1Buttons32 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons33 & state.Value == 128)
                        Joystick1Buttons33 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons33 & state.Value == 0)
                        Joystick1Buttons33 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons34 & state.Value == 128)
                        Joystick1Buttons34 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons34 & state.Value == 0)
                        Joystick1Buttons34 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons35 & state.Value == 128)
                        Joystick1Buttons35 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons35 & state.Value == 0)
                        Joystick1Buttons35 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons36 & state.Value == 128)
                        Joystick1Buttons36 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons36 & state.Value == 0)
                        Joystick1Buttons36 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons37 & state.Value == 128)
                        Joystick1Buttons37 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons37 & state.Value == 0)
                        Joystick1Buttons37 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons38 & state.Value == 128)
                        Joystick1Buttons38 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons38 & state.Value == 0)
                        Joystick1Buttons38 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons39 & state.Value == 128)
                        Joystick1Buttons39 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons39 & state.Value == 0)
                        Joystick1Buttons39 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons40 & state.Value == 128)
                        Joystick1Buttons40 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons40 & state.Value == 0)
                        Joystick1Buttons40 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons41 & state.Value == 128)
                        Joystick1Buttons41 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons41 & state.Value == 0)
                        Joystick1Buttons41 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons42 & state.Value == 128)
                        Joystick1Buttons42 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons42 & state.Value == 0)
                        Joystick1Buttons42 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons43 & state.Value == 128)
                        Joystick1Buttons43 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons43 & state.Value == 0)
                        Joystick1Buttons43 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons44 & state.Value == 128)
                        Joystick1Buttons44 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons44 & state.Value == 0)
                        Joystick1Buttons44 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons45 & state.Value == 128)
                        Joystick1Buttons45 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons45 & state.Value == 0)
                        Joystick1Buttons45 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons46 & state.Value == 128)
                        Joystick1Buttons46 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons46 & state.Value == 0)
                        Joystick1Buttons46 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons47 & state.Value == 128)
                        Joystick1Buttons47 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons47 & state.Value == 0)
                        Joystick1Buttons47 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons48 & state.Value == 128)
                        Joystick1Buttons48 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons48 & state.Value == 0)
                        Joystick1Buttons48 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons49 & state.Value == 128)
                        Joystick1Buttons49 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons49 & state.Value == 0)
                        Joystick1Buttons49 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons50 & state.Value == 128)
                        Joystick1Buttons50 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons50 & state.Value == 0)
                        Joystick1Buttons50 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons51 & state.Value == 128)
                        Joystick1Buttons51 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons51 & state.Value == 0)
                        Joystick1Buttons51 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons52 & state.Value == 128)
                        Joystick1Buttons52 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons52 & state.Value == 0)
                        Joystick1Buttons52 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons53 & state.Value == 128)
                        Joystick1Buttons53 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons53 & state.Value == 0)
                        Joystick1Buttons53 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons54 & state.Value == 128)
                        Joystick1Buttons54 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons54 & state.Value == 0)
                        Joystick1Buttons54 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons55 & state.Value == 128)
                        Joystick1Buttons55 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons55 & state.Value == 0)
                        Joystick1Buttons55 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons56 & state.Value == 128)
                        Joystick1Buttons56 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons56 & state.Value == 0)
                        Joystick1Buttons56 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons57 & state.Value == 128)
                        Joystick1Buttons57 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons57 & state.Value == 0)
                        Joystick1Buttons57 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons58 & state.Value == 128)
                        Joystick1Buttons58 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons58 & state.Value == 0)
                        Joystick1Buttons58 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons59 & state.Value == 128)
                        Joystick1Buttons59 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons59 & state.Value == 0)
                        Joystick1Buttons59 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons60 & state.Value == 128)
                        Joystick1Buttons60 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons60 & state.Value == 0)
                        Joystick1Buttons60 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons61 & state.Value == 128)
                        Joystick1Buttons61 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons61 & state.Value == 0)
                        Joystick1Buttons61 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons62 & state.Value == 128)
                        Joystick1Buttons62 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons62 & state.Value == 0)
                        Joystick1Buttons62 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons63 & state.Value == 128)
                        Joystick1Buttons63 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons63 & state.Value == 0)
                        Joystick1Buttons63 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons64 & state.Value == 128)
                        Joystick1Buttons64 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons64 & state.Value == 0)
                        Joystick1Buttons64 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons65 & state.Value == 128)
                        Joystick1Buttons65 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons65 & state.Value == 0)
                        Joystick1Buttons65 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons66 & state.Value == 128)
                        Joystick1Buttons66 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons66 & state.Value == 0)
                        Joystick1Buttons66 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons67 & state.Value == 128)
                        Joystick1Buttons67 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons67 & state.Value == 0)
                        Joystick1Buttons67 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons68 & state.Value == 128)
                        Joystick1Buttons68 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons68 & state.Value == 0)
                        Joystick1Buttons68 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons69 & state.Value == 128)
                        Joystick1Buttons69 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons69 & state.Value == 0)
                        Joystick1Buttons69 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons70 & state.Value == 128)
                        Joystick1Buttons70 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons70 & state.Value == 0)
                        Joystick1Buttons70 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons71 & state.Value == 128)
                        Joystick1Buttons71 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons71 & state.Value == 0)
                        Joystick1Buttons71 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons72 & state.Value == 128)
                        Joystick1Buttons72 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons72 & state.Value == 0)
                        Joystick1Buttons72 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons73 & state.Value == 128)
                        Joystick1Buttons73 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons73 & state.Value == 0)
                        Joystick1Buttons73 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons74 & state.Value == 128)
                        Joystick1Buttons74 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons74 & state.Value == 0)
                        Joystick1Buttons74 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons75 & state.Value == 128)
                        Joystick1Buttons75 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons75 & state.Value == 0)
                        Joystick1Buttons75 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons76 & state.Value == 128)
                        Joystick1Buttons76 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons76 & state.Value == 0)
                        Joystick1Buttons76 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons77 & state.Value == 128)
                        Joystick1Buttons77 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons77 & state.Value == 0)
                        Joystick1Buttons77 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons78 & state.Value == 128)
                        Joystick1Buttons78 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons78 & state.Value == 0)
                        Joystick1Buttons78 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons79 & state.Value == 128)
                        Joystick1Buttons79 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons79 & state.Value == 0)
                        Joystick1Buttons79 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons80 & state.Value == 128)
                        Joystick1Buttons80 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons80 & state.Value == 0)
                        Joystick1Buttons80 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons81 & state.Value == 128)
                        Joystick1Buttons81 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons81 & state.Value == 0)
                        Joystick1Buttons81 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons82 & state.Value == 128)
                        Joystick1Buttons82 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons82 & state.Value == 0)
                        Joystick1Buttons82 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons83 & state.Value == 128)
                        Joystick1Buttons83 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons83 & state.Value == 0)
                        Joystick1Buttons83 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons84 & state.Value == 128)
                        Joystick1Buttons84 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons84 & state.Value == 0)
                        Joystick1Buttons84 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons85 & state.Value == 128)
                        Joystick1Buttons85 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons85 & state.Value == 0)
                        Joystick1Buttons85 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons86 & state.Value == 128)
                        Joystick1Buttons86 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons86 & state.Value == 0)
                        Joystick1Buttons86 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons87 & state.Value == 128)
                        Joystick1Buttons87 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons87 & state.Value == 0)
                        Joystick1Buttons87 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons88 & state.Value == 128)
                        Joystick1Buttons88 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons88 & state.Value == 0)
                        Joystick1Buttons88 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons89 & state.Value == 128)
                        Joystick1Buttons89 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons89 & state.Value == 0)
                        Joystick1Buttons89 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons90 & state.Value == 128)
                        Joystick1Buttons90 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons90 & state.Value == 0)
                        Joystick1Buttons90 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons91 & state.Value == 128)
                        Joystick1Buttons91 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons91 & state.Value == 0)
                        Joystick1Buttons91 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons92 & state.Value == 128)
                        Joystick1Buttons92 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons92 & state.Value == 0)
                        Joystick1Buttons92 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons93 & state.Value == 128)
                        Joystick1Buttons93 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons93 & state.Value == 0)
                        Joystick1Buttons93 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons94 & state.Value == 128)
                        Joystick1Buttons94 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons94 & state.Value == 0)
                        Joystick1Buttons94 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons95 & state.Value == 128)
                        Joystick1Buttons95 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons95 & state.Value == 0)
                        Joystick1Buttons95 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons96 & state.Value == 128)
                        Joystick1Buttons96 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons96 & state.Value == 0)
                        Joystick1Buttons96 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons97 & state.Value == 128)
                        Joystick1Buttons97 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons97 & state.Value == 0)
                        Joystick1Buttons97 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons98 & state.Value == 128)
                        Joystick1Buttons98 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons98 & state.Value == 0)
                        Joystick1Buttons98 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons99 & state.Value == 128)
                        Joystick1Buttons99 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons99 & state.Value == 0)
                        Joystick1Buttons99 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons100 & state.Value == 128)
                        Joystick1Buttons100 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons100 & state.Value == 0)
                        Joystick1Buttons100 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons101 & state.Value == 128)
                        Joystick1Buttons101 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons101 & state.Value == 0)
                        Joystick1Buttons101 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons102 & state.Value == 128)
                        Joystick1Buttons102 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons102 & state.Value == 0)
                        Joystick1Buttons102 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons103 & state.Value == 128)
                        Joystick1Buttons103 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons103 & state.Value == 0)
                        Joystick1Buttons103 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons104 & state.Value == 128)
                        Joystick1Buttons104 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons104 & state.Value == 0)
                        Joystick1Buttons104 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons105 & state.Value == 128)
                        Joystick1Buttons105 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons105 & state.Value == 0)
                        Joystick1Buttons105 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons106 & state.Value == 128)
                        Joystick1Buttons106 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons106 & state.Value == 0)
                        Joystick1Buttons106 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons107 & state.Value == 128)
                        Joystick1Buttons107 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons107 & state.Value == 0)
                        Joystick1Buttons107 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons108 & state.Value == 128)
                        Joystick1Buttons108 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons108 & state.Value == 0)
                        Joystick1Buttons108 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons109 & state.Value == 128)
                        Joystick1Buttons109 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons109 & state.Value == 0)
                        Joystick1Buttons109 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons110 & state.Value == 128)
                        Joystick1Buttons110 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons110 & state.Value == 0)
                        Joystick1Buttons110 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons111 & state.Value == 128)
                        Joystick1Buttons111 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons111 & state.Value == 0)
                        Joystick1Buttons111 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons112 & state.Value == 128)
                        Joystick1Buttons112 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons112 & state.Value == 0)
                        Joystick1Buttons112 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons113 & state.Value == 128)
                        Joystick1Buttons113 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons113 & state.Value == 0)
                        Joystick1Buttons113 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons114 & state.Value == 128)
                        Joystick1Buttons114 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons114 & state.Value == 0)
                        Joystick1Buttons114 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons115 & state.Value == 128)
                        Joystick1Buttons115 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons115 & state.Value == 0)
                        Joystick1Buttons115 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons116 & state.Value == 128)
                        Joystick1Buttons116 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons116 & state.Value == 0)
                        Joystick1Buttons116 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons117 & state.Value == 128)
                        Joystick1Buttons117 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons117 & state.Value == 0)
                        Joystick1Buttons117 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons118 & state.Value == 128)
                        Joystick1Buttons118 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons118 & state.Value == 0)
                        Joystick1Buttons118 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons119 & state.Value == 128)
                        Joystick1Buttons119 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons119 & state.Value == 0)
                        Joystick1Buttons119 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons120 & state.Value == 128)
                        Joystick1Buttons120 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons120 & state.Value == 0)
                        Joystick1Buttons120 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons121 & state.Value == 128)
                        Joystick1Buttons121 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons121 & state.Value == 0)
                        Joystick1Buttons121 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons122 & state.Value == 128)
                        Joystick1Buttons122 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons122 & state.Value == 0)
                        Joystick1Buttons122 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons123 & state.Value == 128)
                        Joystick1Buttons123 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons123 & state.Value == 0)
                        Joystick1Buttons123 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons124 & state.Value == 128)
                        Joystick1Buttons124 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons124 & state.Value == 0)
                        Joystick1Buttons124 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons125 & state.Value == 128)
                        Joystick1Buttons125 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons125 & state.Value == 0)
                        Joystick1Buttons125 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons126 & state.Value == 128)
                        Joystick1Buttons126 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons126 & state.Value == 0)
                        Joystick1Buttons126 = false;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons127 & state.Value == 128)
                        Joystick1Buttons127 = true;
                    if (inc == 0 & state.Offset == JoystickOffset.Buttons127 & state.Value == 0)
                        Joystick1Buttons127 = false;
                }
            }
        }
    }
    public class ScpBus : IDisposable
    {
        public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static void valchanged(int n, bool val)
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
        private static ScpBus scpBus;
        private static X360Controller controller;
        public static void LoadController()
        {
            scpBus = new ScpBus();
            scpBus.PlugIn(1);
            controller = new X360Controller();
        }
        public static void UnLoadController()
        {
            SetController(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, 0, 0, 0, 0);
            Thread.Sleep(100);
            scpBus.Unplug(1);
        }
        public static void SetController(bool back, bool start, bool A, bool B, bool X, bool Y, bool up, bool left, bool down, bool right, bool leftstick, bool rightstick, bool leftbumper, bool rightbumper, bool lefttrigger, bool righttrigger, double leftstickx, double leftsticky, double rightstickx, double rightsticky)
        {
            valchanged(1, back);
            if (wd[1] == 1)
                controller.Buttons ^= X360Buttons.Back;
            if (wu[1] == 1)
                controller.Buttons &= ~X360Buttons.Back;
            valchanged(2, start);
            if (wd[2] == 1)
                controller.Buttons ^= X360Buttons.Start;
            if (wu[2] == 1)
                controller.Buttons &= ~X360Buttons.Start;
            valchanged(3, A);
            if (wd[3] == 1)
                controller.Buttons ^= X360Buttons.A;
            if (wu[3] == 1)
                controller.Buttons &= ~X360Buttons.A;
            valchanged(4, B);
            if (wd[4] == 1)
                controller.Buttons ^= X360Buttons.B;
            if (wu[4] == 1)
                controller.Buttons &= ~X360Buttons.B;
            valchanged(5, X);
            if (wd[5] == 1)
                controller.Buttons ^= X360Buttons.X;
            if (wu[5] == 1)
                controller.Buttons &= ~X360Buttons.X;
            valchanged(6, Y);
            if (wd[6] == 1)
                controller.Buttons ^= X360Buttons.Y;
            if (wu[6] == 1)
                controller.Buttons &= ~X360Buttons.Y;
            valchanged(7, up);
            if (wd[7] == 1)
                controller.Buttons ^= X360Buttons.Up;
            if (wu[7] == 1)
                controller.Buttons &= ~X360Buttons.Up;
            valchanged(8, left);
            if (wd[8] == 1)
                controller.Buttons ^= X360Buttons.Left;
            if (wu[8] == 1)
                controller.Buttons &= ~X360Buttons.Left;
            valchanged(9, down);
            if (wd[9] == 1)
                controller.Buttons ^= X360Buttons.Down;
            if (wu[9] == 1)
                controller.Buttons &= ~X360Buttons.Down;
            valchanged(10, right);
            if (wd[10] == 1)
                controller.Buttons ^= X360Buttons.Right;
            if (wu[10] == 1)
                controller.Buttons &= ~X360Buttons.Right;
            valchanged(11, leftstick);
            if (wd[11] == 1)
                controller.Buttons ^= X360Buttons.LeftStick;
            if (wu[11] == 1)
                controller.Buttons &= ~X360Buttons.LeftStick;
            valchanged(12, rightstick);
            if (wd[12] == 1)
                controller.Buttons ^= X360Buttons.RightStick;
            if (wu[12] == 1)
                controller.Buttons &= ~X360Buttons.RightStick;
            valchanged(13, leftbumper);
            if (wd[13] == 1)
                controller.Buttons ^= X360Buttons.LeftBumper;
            if (wu[13] == 1)
                controller.Buttons &= ~X360Buttons.LeftBumper;
            valchanged(14, rightbumper);
            if (wd[14] == 1)
                controller.Buttons ^= X360Buttons.RightBumper;
            if (wu[14] == 1)
                controller.Buttons &= ~X360Buttons.RightBumper;
            if (lefttrigger)
                controller.LeftTrigger = 255;
            else
                controller.LeftTrigger = 0;
            if (righttrigger)
                controller.RightTrigger = 255;
            else
                controller.RightTrigger = 0;
            controller.LeftStickX = (short)leftstickx;
            controller.LeftStickY = (short)leftsticky;
            controller.RightStickX = (short)rightstickx;
            controller.RightStickY = (short)rightsticky;
            scpBus.Report(controller.GetReport());
        }
        private const string SCP_BUS_CLASS_GUID = "{F679F562-3164-42CE-A4DB-E7DDBE723909}";
        private const int ReportSize = 28;

        private readonly SafeFileHandle _deviceHandle;

        /// <summary>
        /// Creates a new ScpBus object, which will then try to get a handle to the SCP Virtual Bus device. If it is unable to get the handle, an IOException will be thrown.
        /// </summary>
        public ScpBus() : this(0) { }

        /// <summary>
        /// Creates a new ScpBus object, which will then try to get a handle to the SCP Virtual Bus device. If it is unable to get the handle, an IOException will be thrown.
        /// </summary>
        /// <param name="instance">Specifies which SCP Virtual Bus device to use. This is 0-based.</param>
        public ScpBus(int instance)
        {
            string devicePath = "";

            if (Find(new Guid(SCP_BUS_CLASS_GUID), ref devicePath, instance))
            {
                _deviceHandle = GetHandle(devicePath);
            }
            else
            {
                throw new IOException("SCP Virtual Bus Device not found");
            }
        }

        /// <summary>
        /// Creates a new ScpBus object, which will then try to get a handle to the specified SCP Virtual Bus device. If it is unable to get the handle, an IOException will be thrown.
        /// </summary>
        /// <param name="devicePath">The path to the SCP Virtual Bus device that you want to use.</param>
        public ScpBus(string devicePath)
        {
            _deviceHandle = GetHandle(devicePath);
        }

        /// <summary>
        /// Closes the handle to the SCP Virtual Bus device. Call this when you are done with your instance of ScpBus.
        /// 
        /// (This method does the same thing as the Dispose() method. Use one or the other.)
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Closes the handle to the SCP Virtual Bus device. Call this when you are done with your instance of ScpBus.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Internal disposer, called by either the finalizer or the Dispose() method.
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if called from finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_deviceHandle != null && !_deviceHandle.IsInvalid)
            {
                _deviceHandle.Dispose();
            }
        }

        /// <summary>
        /// Plugs in an emulated Xbox 360 controller.
        /// </summary>
        /// <param name="controllerNumber">Used to identify the controller. Give each controller you plug in a different number. Number must be non-zero.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool PlugIn(int controllerNumber)
        {
            if (_deviceHandle.IsInvalid)
                throw new ObjectDisposedException("SCP Virtual Bus device handle is closed");

            int transfered = 0;
            byte[] buffer = new byte[16];

            buffer[0] = 0x10;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            buffer[3] = 0x00;

            buffer[4] = (byte)((controllerNumber) & 0xFF);
            buffer[5] = (byte)((controllerNumber >> 8) & 0xFF);
            buffer[6] = (byte)((controllerNumber >> 16) & 0xFF);
            buffer[7] = (byte)((controllerNumber >> 24) & 0xFF);

            return NativeMethods.DeviceIoControl(_deviceHandle, 0x2A4000, buffer, buffer.Length, null, 0, ref transfered, IntPtr.Zero);
        }

        /// <summary>
        /// Unplugs an emulated Xbox 360 controller.
        /// </summary>
        /// <param name="controllerNumber">The controller you want to unplug.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool Unplug(int controllerNumber)
        {
            if (_deviceHandle.IsInvalid)
                throw new ObjectDisposedException("SCP Virtual Bus device handle is closed");

            int transfered = 0;
            byte[] buffer = new Byte[16];

            buffer[0] = 0x10;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            buffer[3] = 0x00;

            buffer[4] = (byte)((controllerNumber) & 0xFF);
            buffer[5] = (byte)((controllerNumber >> 8) & 0xFF);
            buffer[6] = (byte)((controllerNumber >> 16) & 0xFF);
            buffer[7] = (byte)((controllerNumber >> 24) & 0xFF);

            return NativeMethods.DeviceIoControl(_deviceHandle, 0x2A4004, buffer, buffer.Length, null, 0, ref transfered, IntPtr.Zero);
        }

        /// <summary>
        /// Unplugs all emulated Xbox 360 controllers.
        /// </summary>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool UnplugAll()
        {
            if (_deviceHandle.IsInvalid)
                throw new ObjectDisposedException("SCP Virtual Bus device handle is closed");

            int transfered = 0;
            byte[] buffer = new byte[16];

            buffer[0] = 0x10;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            buffer[3] = 0x00;

            return NativeMethods.DeviceIoControl(_deviceHandle, 0x2A4004, buffer, buffer.Length, null, 0, ref transfered, IntPtr.Zero);
        }
        int transferred;
        byte[] outputBuffer = null;
        /// <summary>
        /// Sends an input report for the current state of the specified emulated Xbox 360 controller. Note: Only use this if you don't care about rumble data, otherwise use the 3-parameter version of Report().
        /// </summary>
        /// <param name="controllerNumber">The controller to report.</param>
        /// <param name="controllerReport">The controller report. If using the included X360Controller class, this can be generated with the GetReport() method. Otherwise see http://free60.org/wiki/GamePad#Input_report for details.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool Report(byte[] controllerReport)
        {
            return NativeMethods.DeviceIoControl(_deviceHandle, 0x2A400C, controllerReport, controllerReport.Length, outputBuffer, outputBuffer?.Length ?? 0, ref transferred, IntPtr.Zero);
        }

        private static bool Find(Guid target, ref string path, int instance = 0)
        {
            IntPtr detailDataBuffer = IntPtr.Zero;
            IntPtr deviceInfoSet = IntPtr.Zero;

            try
            {
                NativeMethods.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = new NativeMethods.SP_DEVICE_INTERFACE_DATA(), da = new NativeMethods.SP_DEVICE_INTERFACE_DATA();
                int bufferSize = 0, memberIndex = 0;

                deviceInfoSet = NativeMethods.SetupDiGetClassDevs(ref target, IntPtr.Zero, IntPtr.Zero, NativeMethods.DIGCF_PRESENT | NativeMethods.DIGCF_DEVICEINTERFACE);

                DeviceInterfaceData.cbSize = da.cbSize = Marshal.SizeOf(DeviceInterfaceData);

                while (NativeMethods.SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref target, memberIndex, ref DeviceInterfaceData))
                {
                    NativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref DeviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, ref da);
                    detailDataBuffer = Marshal.AllocHGlobal(bufferSize);

                    Marshal.WriteInt32(detailDataBuffer, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);

                    if (NativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref DeviceInterfaceData, detailDataBuffer, bufferSize, ref bufferSize, ref da))
                    {
                        IntPtr pDevicePathName = detailDataBuffer + 4;

                        path = Marshal.PtrToStringAuto(pDevicePathName).ToUpper(CultureInfo.InvariantCulture);
                        Marshal.FreeHGlobal(detailDataBuffer);

                        if (memberIndex == instance) return true;
                    }
                    else Marshal.FreeHGlobal(detailDataBuffer);


                    memberIndex++;
                }
            }
            finally
            {
                if (deviceInfoSet != IntPtr.Zero)
                {
                    NativeMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
                }
            }

            return false;
        }

        private static SafeFileHandle GetHandle(string devicePath)
        {
            devicePath = devicePath.ToUpper(CultureInfo.InvariantCulture);

            SafeFileHandle handle = NativeMethods.CreateFile(devicePath, (NativeMethods.GENERIC_WRITE | NativeMethods.GENERIC_READ), NativeMethods.FILE_SHARE_READ | NativeMethods.FILE_SHARE_WRITE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, NativeMethods.FILE_ATTRIBUTE_NORMAL | NativeMethods.FILE_FLAG_OVERLAPPED, UIntPtr.Zero);

            if (handle == null || handle.IsInvalid)
            {
                throw new IOException("Unable to get SCP Virtual Bus Device handle");
            }

            return handle;
        }
    }

    internal static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVICE_INTERFACE_DATA
        {
            internal int cbSize;
            internal Guid InterfaceClassGuid;
            internal int Flags;
            internal IntPtr Reserved;
        }

        internal const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        internal const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        internal const uint FILE_SHARE_READ = 1;
        internal const uint FILE_SHARE_WRITE = 2;
        internal const uint GENERIC_READ = 0x80000000;
        internal const uint GENERIC_WRITE = 0x40000000;
        internal const uint OPEN_EXISTING = 3;
        internal const int DIGCF_PRESENT = 0x0002;
        internal const int DIGCF_DEVICEINTERFACE = 0x0010;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, UIntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeviceIoControl(SafeFileHandle hDevice, int dwIoControlCode, byte[] lpInBuffer, int nInBufferSize, byte[] lpOutBuffer, int nOutBufferSize, ref int lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, int memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, int flags);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, ref SP_DEVICE_INTERFACE_DATA deviceInfoData);
    }
    /// <summary>
    /// A virtual Xbox 360 Controller. After setting the desired values, use the GetReport() method to generate a controller report that can be used with ScpBus's Report() method.
    /// </summary>
    public class X360Controller
    {
        /// <summary>
        /// Generates a new X360Controller object with the default initial state (no buttons pressed, all analog inputs 0).
        /// </summary>
        public X360Controller()
        {
            Buttons = X360Buttons.None;
            LeftTrigger = 0;
            RightTrigger = 0;
            LeftStickX = 0;
            LeftStickY = 0;
            RightStickX = 0;
            RightStickY = 0;
        }

        /// <summary>
        /// Generates a new X360Controller object. Optionally, you can specify the initial state of the controller.
        /// </summary>
        /// <param name="buttons">The pressed buttons. Use like flags (i.e. (X360Buttons.A | X360Buttons.X) would be mean both A and X are pressed).</param>
        /// <param name="leftTrigger">Left trigger analog input. 0 to 255.</param>
        /// <param name="rightTrigger">Right trigger analog input. 0 to 255.</param>
        /// <param name="leftStickX">Left stick X-axis. -32,768 to 32,767.</param>
        /// <param name="leftStickY">Left stick Y-axis. -32,768 to 32,767.</param>
        /// <param name="rightStickX">Right stick X-axis. -32,768 to 32,767.</param>
        /// <param name="rightStickY">Right stick Y-axis. -32,768 to 32,767.</param>
        public X360Controller(X360Buttons buttons, byte leftTrigger, byte rightTrigger, short leftStickX, short leftStickY, short rightStickX, short rightStickY)
        {
            Buttons = buttons;
            LeftTrigger = leftTrigger;
            RightTrigger = rightTrigger;
            LeftStickX = leftStickX;
            LeftStickY = leftStickY;
            RightStickX = rightStickX;
            RightStickY = rightStickY;
        }

        /// <summary>
        /// Generates a new X360Controller object with the same values as the specified X360Controller object.
        /// </summary>
        /// <param name="controller">An X360Controller object to copy values from.</param>
        public X360Controller(X360Controller controller)
        {
            Buttons = controller.Buttons;
            LeftTrigger = controller.LeftTrigger;
            RightTrigger = controller.RightTrigger;
            LeftStickX = controller.LeftStickX;
            LeftStickY = controller.LeftStickY;
            RightStickX = controller.RightStickX;
            RightStickY = controller.RightStickY;
        }

        /// <summary>
        /// The controller's currently pressed buttons. Use the X360Button values like flags (i.e. (X360Buttons.A | X360Buttons.X) would be mean both A and X are pressed).
        /// </summary>
        public X360Buttons Buttons { get; set; }

        /// <summary>
        /// The controller's left trigger analog input. Value can range from 0 to 255.
        /// </summary>
        public byte LeftTrigger { get; set; }

        /// <summary>
        /// The controller's right trigger analog input. Value can range from 0 to 255.
        /// </summary>
        public byte RightTrigger { get; set; }

        /// <summary>
        /// The controller's left stick X-axis. Value can range from -32,768 to 32,767.
        /// </summary>
        public short LeftStickX { get; set; }

        /// <summary>
        /// The controller's left stick Y-axis. Value can range from -32,768 to 32,767.
        /// </summary>
        public short LeftStickY { get; set; }

        /// <summary>
        /// The controller's right stick X-axis. Value can range from -32,768 to 32,767.
        /// </summary>
        public short RightStickX { get; set; }

        /// <summary>
        /// The controller's right stick Y-axis. Value can range from -32,768 to 32,767.
        /// </summary>
        public short RightStickY { get; set; }

        byte[] bytes = new byte[20];
        byte[] fullReport = { 0x1C, 0, 0, 0, (byte)((1) & 0xFF), (byte)((1 >> 8) & 0xFF), (byte)((1 >> 16) & 0xFF), (byte)((1 >> 24) & 0xFF), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// Generates an Xbox 360 controller report as specified here: http://free60.org/wiki/GamePad#Input_report. This can be used with ScpBus's Report() method.
        /// </summary>
        /// <returns>A 20-byte Xbox 360 controller report.</returns>
        public byte[] GetReport()
        {
            bytes[0] = 0x00;                                 // Message type (input report)
            bytes[1] = 0x14;                                 // Message size (20 bytes)

            bytes[2] = (byte)((ushort)Buttons & 0xFF);       // Buttons low
            bytes[3] = (byte)((ushort)Buttons >> 8 & 0xFF);  // Buttons high

            bytes[4] = LeftTrigger;                          // Left trigger
            bytes[5] = RightTrigger;                         // Right trigger

            bytes[6] = (byte)(LeftStickX & 0xFF);            // Left stick X-axis low
            bytes[7] = (byte)(LeftStickX >> 8 & 0xFF);       // Left stick X-axis high
            bytes[8] = (byte)(LeftStickY & 0xFF);            // Left stick Y-axis low
            bytes[9] = (byte)(LeftStickY >> 8 & 0xFF);       // Left stick Y-axis high

            bytes[10] = (byte)(RightStickX & 0xFF);          // Right stick X-axis low
            bytes[11] = (byte)(RightStickX >> 8 & 0xFF);     // Right stick X-axis high
            bytes[12] = (byte)(RightStickY & 0xFF);          // Right stick Y-axis low
            bytes[13] = (byte)(RightStickY >> 8 & 0xFF);     // Right stick Y-axis high

            // Remaining bytes are unused

            Array.Copy(bytes, 0, fullReport, 8, 20);

            return fullReport;
        }
    }

    /// <summary>
    /// The buttons to be used with an X360Controller object.
    /// </summary>
    [Flags]
    public enum X360Buttons
    {
        None = 0,

        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,

        Start = 1 << 4,
        Back = 1 << 5,

        LeftStick = 1 << 6,
        RightStick = 1 << 7,

        LeftBumper = 1 << 8,
        RightBumper = 1 << 9,

        Logo = 1 << 10,

        A = 1 << 12,
        B = 1 << 13,
        X = 1 << 14,
        Y = 1 << 15,

    }
}