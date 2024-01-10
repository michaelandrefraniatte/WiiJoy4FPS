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
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System.CodeDom;
using System.Reflection;
namespace WiiJoy4
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        [DllImport("WiiJoyPairing.dll", EntryPoint = "connect")]
        public static unsafe extern int connect();
        [DllImport("WiiJoyPairing.dll", EntryPoint = "disconnect")]
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
        [DllImport("Kernel32.dll")]
        public static unsafe extern SafeFileHandle CreateFile(string fileName, [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess, [MarshalAs(UnmanagedType.U4)] FileShare fileShare, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] uint flags, IntPtr template);
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
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        private int leftandright;
        public static double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe, irx2e, iry2e, irx3e, iry3e, irx, iry, irxc, iryc, mWSIRSensorsXcam, mWSIRSensorsYcam, mWSIRSensors0X, mWSIRSensors0Y, mWSIRSensors1X, mWSIRSensors1Y, mWSButtonStateIRX, mWSButtonStateIRY, mWSIRSensors0Xcam, mWSIRSensors1Xcam, mWSIRSensors0Ycam, mWSIRSensors1Ycam, MyAngle, mWSIR0notfound = 0, mWSRawValuesX, mWSRawValuesY, mWSRawValuesZ, calibrationinit, camx, camy, watchM = 50, watchM1 = 2, watchM2 = 0, stickviewxinit, stickviewyinit, mWSNunchuckStateRawValuesX, mWSNunchuckStateRawValuesY, mWSNunchuckStateRawValuesZ, mWSNunchuckStateRawJoystickX, mWSNunchuckStateRawJoystickY;
        public static bool readingfile, endinvoke;
        public static int readingfilecount;
        public static string processname, path;
        public static byte[] buff = new byte[] { 0x55 }, mBuff = new byte[22], aBuffer = new byte[22], bBuffer = new byte[22];
        public static byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        public static Guid guid = new System.Guid();
        public static uint hDevInfo, CurrentResolution = 0;
        public static ThreadStart threadstart;
        public static Thread thread;
        public static Task taskDHidReadWiimote, taskDWriteWiimote, taskDHidReadLeft, taskDWriteLeft, taskDHidReadRight, taskDWriteRight, taskS;
        private static Stopwatch diffM = new Stopwatch();
        private Type programwiimote, programjoyconleft, programjoyconright;
        private object objwiimote, objjoyconleft, objjoyconright, objdataWriteWiimote, objdataWriteLeft, objdataWriteRight;
        private Assembly assemblywiimote, assemblyjoyconleft, assemblyjoyconright;
        private System.CodeDom.Compiler.CompilerResults resultswiimote, resultsjoyconleft, resultsjoyconright;
        private Microsoft.CSharp.CSharpCodeProvider providerwiimote, providerjoyconleft, providerjoyconright;
        private System.CodeDom.Compiler.CompilerParameters parameterswiimote, parametersjoyconleft, parametersjoyconright;
        private string joyconleftcode, joyconrightcode, wiimotecode;
        private static System.IO.FileStream mStream;
        public void shown()
        {
            endinvoke = true;
            Thread.Sleep(100);
            endinvoke = false;
            this.Location = new System.Drawing.Point(50, 50);
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            System.Diagnostics.Process process = Process.GetCurrentProcess();
            process.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            taskS = new Task(FormStart);
            taskS.Start();
        }
        private void initConnect()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("initconnect.txt");
            file.WriteLine("True");
            file.WriteLine(leftandright.ToString());
            file.Close();
        }
        private void FormStart()
        {
            leftandright = connect();
            try { initConnect(); }
            catch
            {
                using (System.IO.StreamWriter createdfile = System.IO.File.AppendText("initconnect.txt"))
                {
                    createdfile.WriteLine("True");
                    createdfile.WriteLine(leftandright.ToString());
                }
            }
            if (leftandright == 4 | leftandright == 5 | leftandright == 6 | leftandright == 7)
                do
                    Thread.Sleep(1);
                while (!ScanRight());
            if (leftandright == 1 | leftandright == 2 | leftandright == 4 | leftandright == 7)
                do
                    Thread.Sleep(1);
                while (!ScanLeft());
            if (leftandright == 2 | leftandright == 3 | leftandright == 5 | leftandright == 7)
                do
                    Thread.Sleep(1);
                while (!ScanWiimote());
            if (leftandright == 1 | leftandright == 2 | leftandright == 4 | leftandright == 7)
            {
                joyconleftcode = @"using System;
                using System.Runtime.InteropServices;
                using Microsoft.Win32.SafeHandles;
                namespace StringToCode
                {
                    public class dataHidReadClassLeft
                    {
                        [DllImport(""winmm.dll"", EntryPoint = ""timeBeginPeriod"")]
                        public static extern uint TimeBeginPeriod(uint ms);
                        [DllImport(""winmm.dll"", EntryPoint = ""timeEndPeriod"")]
                        public static extern uint TimeEndPeriod(uint ms);
                        [DllImport(""ntdll.dll"", EntryPoint = ""NtSetTimerResolution"")]
                        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
                        public static uint CurrentResolution = 0;
                        public void Open()
                        {
                            TimeBeginPeriod(1);
                            NtSetTimerResolution(1, true, ref CurrentResolution);
                        }
                        public void Close()
                        {
                            TimeEndPeriod(1);
                        }
                        [DllImport(""lhidread.dll"", CallingConvention = CallingConvention.Cdecl, EntryPoint = ""Lhid_read_timeout"")]
                        public static extern int Lhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
                        private static byte[] report_bufaLeft = new byte[49];
                        public object[] Data(SafeFileHandle handleLeft)
                        {
                            Lhid_read_timeout(handleLeft, report_bufaLeft, (UIntPtr)49);
                            return new object[] { report_bufaLeft };
                        }
                    }
                }";
                parametersjoyconleft = new System.CodeDom.Compiler.CompilerParameters();
                parametersjoyconleft.GenerateExecutable = false;
                parametersjoyconleft.GenerateInMemory = true;
                parametersjoyconleft.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                parametersjoyconleft.ReferencedAssemblies.Add("System.Drawing.dll");
                providerjoyconleft = new Microsoft.CSharp.CSharpCodeProvider();
                resultsjoyconleft = providerjoyconleft.CompileAssemblyFromSource(parametersjoyconleft, joyconleftcode);
                assemblyjoyconleft = resultsjoyconleft.CompiledAssembly;
                programjoyconleft = assemblyjoyconleft.GetType("StringToCode.dataHidReadClassLeft");
                objjoyconleft = Activator.CreateInstance(programjoyconleft);
                objdataWriteLeft = Activator.CreateInstance(typeof(dataWriteClassLeft));
                taskDHidReadLeft = new Task(WiiJoy_thrDHidReadLeft);
                taskDHidReadLeft.Start();
                taskDWriteLeft = new Task(WiiJoy_thrDWriteLeft);
                taskDWriteLeft.Start();
            }
            if (leftandright == 4 | leftandright == 5 | leftandright == 6 | leftandright == 7)
            {
                joyconrightcode = @"using System;
                using System.Runtime.InteropServices;
                using Microsoft.Win32.SafeHandles;
                namespace StringToCode
                {
                    public class dataHidReadClassRight
                    {
                        [DllImport(""winmm.dll"", EntryPoint = ""timeBeginPeriod"")]
                        public static extern uint TimeBeginPeriod(uint ms);
                        [DllImport(""winmm.dll"", EntryPoint = ""timeEndPeriod"")]
                        public static extern uint TimeEndPeriod(uint ms);
                        [DllImport(""ntdll.dll"", EntryPoint = ""NtSetTimerResolution"")]
                        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
                        public static uint CurrentResolution = 0;
                        public void Open()
                        {
                            TimeBeginPeriod(1);
                            NtSetTimerResolution(1, true, ref CurrentResolution);
                        }
                        public void Close()
                        {
                            TimeEndPeriod(1);
                        }
                        [DllImport(""rhidread.dll"", CallingConvention = CallingConvention.Cdecl, EntryPoint = ""Rhid_read_timeout"")]
                        public static extern int Rhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
                        private static byte[] report_bufaRight = new byte[49];
                        public object[] Data(SafeFileHandle handleRight)
                        {
                            Rhid_read_timeout(handleRight, report_bufaRight, (UIntPtr)49);
                            return new object[] { report_bufaRight };
                        }
                    }
                }";
                parametersjoyconright = new System.CodeDom.Compiler.CompilerParameters();
                parametersjoyconright.GenerateExecutable = false;
                parametersjoyconright.GenerateInMemory = true;
                parametersjoyconright.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                parametersjoyconright.ReferencedAssemblies.Add("System.Drawing.dll");
                providerjoyconright = new Microsoft.CSharp.CSharpCodeProvider();
                resultsjoyconright = providerjoyconright.CompileAssemblyFromSource(parametersjoyconright, joyconrightcode);
                assemblyjoyconright = resultsjoyconright.CompiledAssembly;
                programjoyconright = assemblyjoyconright.GetType("StringToCode.dataHidReadClassRight");
                objjoyconright = Activator.CreateInstance(programjoyconright);
                objdataWriteRight = Activator.CreateInstance(typeof(dataWriteClassRight));
                taskDHidReadRight = new Task(WiiJoy_thrDHidReadRight);
                taskDHidReadRight.Start();
                taskDWriteRight = new Task(WiiJoy_thrDWriteRight);
                taskDWriteRight.Start();
            }
            if (leftandright == 2 | leftandright == 3 | leftandright == 5 | leftandright == 7)
            {
                wiimotecode = @"using System;
                using System.Runtime.InteropServices;
                namespace StringToCode
                {
                    public class dataHidReadClassWiimote
                    {
                        [DllImport(""winmm.dll"", EntryPoint = ""timeBeginPeriod"")]
                        public static extern uint TimeBeginPeriod(uint ms);
                        [DllImport(""winmm.dll"", EntryPoint = ""timeEndPeriod"")]
                        public static extern uint TimeEndPeriod(uint ms);
                        [DllImport(""ntdll.dll"", EntryPoint = ""NtSetTimerResolution"")]
                        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
                        public static uint CurrentResolution = 0;
                        public void Open()
                        {
                            TimeBeginPeriod(1);
                            NtSetTimerResolution(1, true, ref CurrentResolution);
                        }
                        public void Close()
                        {
                            TimeEndPeriod(1);
                        }
                        public static byte[] aBuffer = new byte[22];
                        public static bool readingfile;
                        public object[] Data(System.IO.FileStream mStream)
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
                            return new object[] { aBuffer, readingfile };
                        }
                    }
                }";
                parameterswiimote = new System.CodeDom.Compiler.CompilerParameters();
                parameterswiimote.GenerateExecutable = false;
                parameterswiimote.GenerateInMemory = true;
                parameterswiimote.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                parameterswiimote.ReferencedAssemblies.Add("System.Drawing.dll");
                providerwiimote = new Microsoft.CSharp.CSharpCodeProvider();
                resultswiimote = providerwiimote.CompileAssemblyFromSource(parameterswiimote, wiimotecode);
                assemblywiimote = resultswiimote.CompiledAssembly;
                programwiimote = assemblywiimote.GetType("StringToCode.dataHidReadClassWiimote");
                objwiimote = Activator.CreateInstance(programwiimote);
                objdataWriteWiimote = Activator.CreateInstance(typeof(dataWriteClassWiimote));
                taskDHidReadWiimote = new Task(WiiJoy_thrDHidReadWiimote);
                taskDHidReadWiimote.Start();
                taskDWriteWiimote = new Task(WiiJoy_thrDWriteWiimote);
                taskDWriteWiimote.Start();
            }
            assemblywiimote = null;
            assemblyjoyconleft = null;
            assemblyjoyconright = null;
            resultswiimote = null;
            resultsjoyconleft = null;
            resultsjoyconright = null;
            providerwiimote = new Microsoft.CSharp.CSharpCodeProvider();
            providerjoyconleft = new Microsoft.CSharp.CSharpCodeProvider();
            providerjoyconright = new Microsoft.CSharp.CSharpCodeProvider();
            parameterswiimote = new System.CodeDom.Compiler.CompilerParameters();
            parametersjoyconleft = new System.CodeDom.Compiler.CompilerParameters();
            parametersjoyconright = new System.CodeDom.Compiler.CompilerParameters();
            joyconleftcode = "";
            joyconrightcode = "";
            wiimotecode = "";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
        }
        private async void WiiJoy_thrDHidReadLeft()
        {
            programjoyconleft.InvokeMember("Open", BindingFlags.Default | BindingFlags.InvokeMethod, null, objjoyconleft, new object[] { });
            for (; ; )
            {
                if (endinvoke)
                {
                    programjoyconleft.InvokeMember("Close", BindingFlags.Default | BindingFlags.InvokeMethod, null, objjoyconleft, new object[] { });
                    return;
                }
                await asynctaskDHidReadLeft();
            }
        }
        public async Task asynctaskDHidReadLeft()
        {
            object[] val = (object[])programjoyconleft.InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objjoyconleft, new object[] { handleLeft });
            report_bufaLeft = (byte[])val[0];
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        public async void WiiJoy_thrDWriteLeft()
        {
            for (; ; )
            {
                if (endinvoke)
                    return;
                await asynctaskDWriteLeft();
            }
        }
        public async Task asynctaskDWriteLeft()
        {
            try
            {
                typeof(dataWriteClassLeft).InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objdataWriteLeft, new object[] { });
            }
            catch { }
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(1));
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        public class dataWriteClassLeft
        {
            public void Data()
            {
                using (System.IO.FileStream fs = new System.IO.FileStream("dataLeft", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, bufferSize: 4096, useAsync: true))
                {
                    fs.WriteAsync(report_bufaLeft, 0, 49);
                };
            }
        }
        private async void WiiJoy_thrDHidReadRight()
        {
            programjoyconright.InvokeMember("Open", BindingFlags.Default | BindingFlags.InvokeMethod, null, objjoyconright, new object[] { });
            for (; ; )
            {
                if (endinvoke)
                {
                    programjoyconright.InvokeMember("Close", BindingFlags.Default | BindingFlags.InvokeMethod, null, objjoyconright, new object[] { });
                    return;
                }
                await asynctaskDHidReadRight();
            }
        }
        public async Task asynctaskDHidReadRight()
        {
            object[] val = (object[])programjoyconright.InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objjoyconright, new object[] { handleRight });
            report_bufaRight = (byte[])val[0];
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        public async void WiiJoy_thrDWriteRight()
        {
            for (; ; )
            {
                if (endinvoke)
                    return;
                await asynctaskDWriteRight();
            }
        }
        public async Task asynctaskDWriteRight()
        {
            try
            {
                typeof(dataWriteClassRight).InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objdataWriteRight, new object[] { });
            }
            catch { }
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(1));
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        public class dataWriteClassRight
        {
            public void Data()
            {
                using (System.IO.FileStream fs = new System.IO.FileStream("dataRight", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, bufferSize: 4096, useAsync: true))
                {
                    fs.WriteAsync(report_bufaRight, 0, 49);
                };
            }
        }
        private async void WiiJoy_thrDHidReadWiimote()
        {
            programwiimote.InvokeMember("Open", BindingFlags.Default | BindingFlags.InvokeMethod, null, objwiimote, new object[] { });
            for (; ; )
            {
                if (endinvoke)
                {
                    programwiimote.InvokeMember("Close", BindingFlags.Default | BindingFlags.InvokeMethod, null, objwiimote, new object[] { });
                    return;
                }
                await asynctaskDHidReadWiimote();
            }
        }
        public async Task asynctaskDHidReadWiimote()
        {
            object[] val = (object[])programwiimote.InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objwiimote, new object[] { mStream });
            aBuffer = (byte[])val[0];
            readingfile = (bool)val[1];
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        public async void WiiJoy_thrDWriteWiimote()
        {
            for (; ; )
            {
                if (endinvoke)
                    return;
                await asynctaskDWriteWiimote();
            }
        }
        public async Task asynctaskDWriteWiimote()
        {
            try
            {
                typeof(dataWriteClassWiimote).InvokeMember("Data", BindingFlags.Default | BindingFlags.InvokeMethod, null, objdataWriteWiimote, new object[] { });
            }
            catch { }
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(1));
            await Task.Delay(TimeSpan.FromMilliseconds(0));
        }
        public class dataWriteClassWiimote
        {
            public void Data()
            {
                using (System.IO.FileStream fs = new System.IO.FileStream("dataWiimote", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, bufferSize: 4096, useAsync: true))
                {
                    fs.WriteAsync(aBuffer, 0, 22);
                };
                if (readingfilecount == 0)
                    readingfile = false;
                readingfilecount++;
                if (readingfilecount > 100)
                {
                    if (!readingfile & !endinvoke)
                        WiimoteFound(path);
                    readingfilecount = 0;
                }
            }
        }
        private void FormClose()
        {
            disconnect();
        }
        public void closed()
        {
            endinvoke = true;
            Thread.Sleep(100);
            endinvoke = false;
            TimeEndPeriod(1);
            try
            {
                Lhid_close(handleLeft);
            }
            catch { }
            try
            {
                Rhid_close(handleRight);
            }
            catch { }
            threadstart = new ThreadStart(FormClose);
            thread = new Thread(threadstart);
            thread.Start();
        }
        private const string vendor_id = "57e", vendor_id_ = "057e", product_l = "2006", product_r = "2007", product_r1 = "0330", product_r2 = "0306";
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
        private static SafeFileHandle handleLeft;
        private const uint report_lenLeft = 49;
        private static byte[] report_bufaLeft = new byte[report_lenLeft];
        private static byte[] buf_Left = new byte[report_lenLeft];
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
        private static SafeFileHandle handleRight;
        private const uint report_lenRight = 49;
        private static byte[] report_bufaRight = new byte[report_lenRight];
        private static byte[] buf_Right = new byte[report_lenRight];
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
        private bool ScanWiimote()
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
            mStream = new System.IO.FileStream(handle, System.IO.FileAccess.ReadWrite, 22, true);
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
