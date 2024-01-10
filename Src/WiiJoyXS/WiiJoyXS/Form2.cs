using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace WiiJoyXS
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private static double viewpower05x, viewpower1x, viewpower2x = 1f, viewpower3x, viewpower05y, viewpower1y, viewpower2y = 1f, viewpower3y, dzx, dzy, lowsensx = 1f, lowsensy = 1f, switchviewpower05x, switchviewpower1x, switchviewpower2x = 1f, switchviewpower3x, switchviewpower05y, switchviewpower1y, switchviewpower2y = 1f, switchviewpower3y, switchdzx, switchdzy, switchlowsensx = 1f, switchlowsensy = 1f, centery = 160f, dzapressiox, dzapressioy, lowsensapressiox, lowsensapressioy, lowsenstime, lowsenstimeapressio, pollrate = 1, viewpower05xs, viewpower1xs, viewpower2xs = 1f, viewpower3xs, viewpower05ys, viewpower1ys, viewpower2ys = 1f, viewpower3ys, dzxs, dzys, lowsensxs = 1f, lowsensys = 1f, centerys, lowsensbrinkaex, lowsensbrinkaey;
        private static bool apressio, brink, mw3, mw3ae, brinkae, desktop;
        private static string xoutDown, xoutLeft, xoutRight, xoutUp, xoutRightStick, xoutLeftStick, xoutA, xoutBack, xoutStart, xoutX, xoutRightBumper, xoutLeftBumper, xoutB, xoutY, xoutRightTrigger, xoutLeftTrigger, xoutswitchDown, xoutswitchLeft, xoutswitchRight, xoutswitchUp, xoutswitchRightStick, xoutswitchLeftStick, xoutswitchA, xoutswitchBack, xoutswitchStart, xoutswitchX, xoutswitchRightBumper, xoutswitchLeftBumper, xoutswitchB, xoutswitchY, xoutswitchRightTrigger, xoutswitchLeftTrigger, xoutLeftStickRight, xoutLeftStickLeft, xoutLeftStickUp, xoutLeftStickDown, xoutRightStickRight, xoutRightStickLeft, xoutRightStickUp, xoutRightStickDown, xoutLeftStickX, xoutLeftStickY, xoutRightStickX, xoutRightStickY, xoutswitchLeftStickRight, xoutswitchLeftStickLeft, xoutswitchLeftStickUp, xoutswitchLeftStickDown, xoutswitchRightStickRight, xoutswitchRightStickLeft, xoutswitchRightStickUp, xoutswitchRightStickDown, xoutswitchLeftStickX, xoutswitchLeftStickY, xoutswitchRightStickX, xoutswitchRightStickY;
        private static string games, game, hidtype, drivertype, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15, s16, s17, s18, s19, s20, s21, s22, s23, s24, s25, s26, s27, s28, s29, s30, s31, s32, s33, s34, s35, s36, s37, s38, s39, s40, s41, s42, s43, s44, s45, s46, s47, s48, s49, s50, s51, s52, s53, s54, s55, s56, s57, s58, s59, s60, s61, s62, s63, s64, s65, s66, s67, s68, s69, s70, s71, s72, s73, s74, s75, s76, s77, s78, s79, s80, s81, s82, s83, s84, s85, s86, s87, s88, s89, s90, s91, s92, s93, s94, s95, s96, s97, s98, s99, s100, s101, s102, s103, s104, s105, s106, s107, s108, s109, s110, s111, s112, s113, s114, s115, s116, s117, s118, s119, s120, s121, s122, s123, s124, s125, s126, s127, s128, s129, s130, s131, s132, s133, s134, s135, s136, s137, s138, s139, s140, s141, filename = "";
        string[] itemsbuttons = new string[] { "mWSButtonStateLR",
                "mWSButtonStateMU",
                "mWSButtonStatePU",
                "mWSButtonStateHFront",
                "mWSLeftButtonTC",
                "LeftButtonSMA",
                "mWSButtonStateA",
                "mWSButtonStateB",
                "mWSButtonStateMinus",
                "mWSButtonStateHome",
                "mWSButtonStatePlus",
                "mWSButtonStateOne",
                "mWSButtonStateTwo",
                "mWSButtonStateUp",
                "mWSButtonStateDown",
                "mWSButtonStateLeft",
                "mWSButtonStateRight",
                "mWSButtonStateFront",
                "LeftButtonSHOULDER_1",
                "LeftButtonSHOULDER_2",
                "LeftButtonSR",
                "LeftButtonSL",
                "LeftButtonDPAD_DOWN",
                "LeftButtonDPAD_RIGHT",
                "LeftButtonDPAD_UP",
                "LeftButtonDPAD_LEFT",
                "LeftButtonMINUS",
                "LeftButtonCAPTURE",
                "LeftButtonSTICK",
                "LeftButtonACC",
                "LeftButtonStickRight",
                "LeftButtonStickLeft",
                "LeftButtonStickUp",
                "LeftButtonStickDown",
                "RightButtonSHOULDER_1",
                "RightButtonSHOULDER_2",
                "RightButtonSR",
                "RightButtonSL",
                "RightButtonDPAD_DOWN",
                "RightButtonDPAD_RIGHT",
                "RightButtonDPAD_UP",
                "RightButtonDPAD_LEFT",
                "RightButtonPLUS",
                "RightButtonHOME",
                "RightButtonSTICK",
                "RightButtonSPA",
                "mWSRightButtonTH",
                "RightButtonACC",
                "RightButtonStickRight",
                "RightButtonStickLeft",
                "RightButtonStickUp",
                "RightButtonStickDown",
                "mWSNunchuckStateRollingLeft",
                "mWSNunchuckStateRollingRight",
                "mWSNunchuckStateRawValuesY",
                "mWSNunchuckStateC",
                "mWSNunchuckStateZ",
                "mWSNunchuckStateStickRight",
                "mWSNunchuckStateStickLeft",
                "mWSNunchuckStateStickUp",
                "mWSNunchuckStateStickDown",
                "LeftRollLeft",
                "LeftRollRight",
                "RightRollLeft",
                "RightRollRight",
                "none" };
        string[] itemsaxis = new string[] {
                "irx",
                "iry",
                "accx",
                "accy",
                "leftgyrox",
                "leftgyroy",
                "rightgyrox",
                "rightgyroy",
                "leftaccx",
                "leftaccy",
                "rightaccx",
                "rightaccy",
                "stickx",
                "sticky",
                "rightstickx",
                "rightsticky",
                "leftstickx",
                "leftsticky",
                "none" };
        string[] itemsbools = new string[] {
                "False",
                "True" };
        string[] itemsdrivertypes = new string[] {
                "sendinput", "kmevent", "interception" };
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (filename != "")
            {
                using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter("tempsave"))
                {
                    createdfile.WriteLine(filename);
                }
            }
        }
        private void openConf(bool shown)
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Outputs";
            dataGridView1.Columns[1].Name = "Inputs";
            if (!shown)
            {
                if (hidtype == "controller")
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
            }
            dataGridView1.Rows.Add("Game", shown ? "Call of Duty Black Ops Cold War" : game); //0
            dataGridView1.Rows.Add("viewpower05x", shown ? "0" : viewpower05x.ToString()); //1
            dataGridView1.Rows.Add("viewpower1x", shown ? "0" : viewpower1x.ToString()); //2
            dataGridView1.Rows.Add("viewpower2x", shown ? "1" : viewpower2x.ToString()); //3
            dataGridView1.Rows.Add("viewpower3x", shown ? "0" : viewpower3x.ToString()); //4
            dataGridView1.Rows.Add("viewpower05y", shown ? "0" : viewpower05y.ToString()); //5
            dataGridView1.Rows.Add("viewpower1y", shown ? "0" : viewpower1y.ToString()); //6
            dataGridView1.Rows.Add("viewpower2y", shown ? "1" : viewpower2y.ToString()); //7
            dataGridView1.Rows.Add("viewpower3y", shown ? "0" : viewpower3y.ToString()); //8
            dataGridView1.Rows.Add("dzx", shown ? "3.0" : dzx.ToString()); //9
            dataGridView1.Rows.Add("dzy", shown ? "3.0" : dzy.ToString()); //10
            dataGridView1.Rows.Add("lowsensx", shown ? "1.0" : lowsensx.ToString()); //11
            dataGridView1.Rows.Add("lowsensy", shown ? "1.0" : lowsensy.ToString()); //12
            dataGridView1.Rows.Add("centery", shown ? "140" : centery.ToString()); //13
            dataGridView1.Rows.Add("apressio"); //14
            DataGridViewComboBoxCell cmb14 = new DataGridViewComboBoxCell();
            cmb14.DataSource = itemsbools;
            dataGridView1[1, 14] = cmb14;
            dataGridView1[1, 14].Value = shown ? "False" : apressio.ToString();
            dataGridView1.Rows.Add("dzapressiox", shown ? "3.0" : dzapressiox.ToString()); //15
            dataGridView1.Rows.Add("dzapressioy", shown ? "3.0" : dzapressioy.ToString()); //16
            dataGridView1.Rows.Add("lowsensapressiox", shown ? "2.0" : lowsensapressiox.ToString()); //17
            dataGridView1.Rows.Add("lowsensapressioy", shown ? "2.0" : lowsensapressioy.ToString()); //18
            dataGridView1.Rows.Add("lowsenstime", shown ? "290" : lowsenstime.ToString()); //19
            dataGridView1.Rows.Add("lowsenstimeapressio", shown ? "145" : lowsenstimeapressio.ToString()); //20
            dataGridView1.Rows.Add("switch viewpower05x", shown ? "0" : switchviewpower05x.ToString()); //21
            dataGridView1.Rows.Add("switch viewpower1x", shown ? "0" : switchviewpower1x.ToString()); //22
            dataGridView1.Rows.Add("switch viewpower2x", shown ? "1" : switchviewpower2x.ToString()); //23
            dataGridView1.Rows.Add("switch viewpower3x", shown ? "0" : switchviewpower3x.ToString()); //24
            dataGridView1.Rows.Add("switch viewpower05y", shown ? "0" : switchviewpower05y.ToString()); //25
            dataGridView1.Rows.Add("switch viewpower1y", shown ? "0" : switchviewpower1y.ToString()); //26
            dataGridView1.Rows.Add("switch viewpower2y", shown ? "1" : switchviewpower2y.ToString()); //27
            dataGridView1.Rows.Add("switch viewpower3y", shown ? "0" : switchviewpower3y.ToString()); //28
            dataGridView1.Rows.Add("switch dzx", shown ? "3.0" : switchdzx.ToString()); //29
            dataGridView1.Rows.Add("switch dzy", shown ? "3.0" : switchdzy.ToString()); //30
            dataGridView1.Rows.Add("switch lowsensx", shown ? "1.0" : switchlowsensx.ToString()); //31
            dataGridView1.Rows.Add("switch lowsensy", shown ? "1.0" : switchlowsensy.ToString()); //32
            dataGridView1.Rows.Add("Down"); //33
            DataGridViewComboBoxCell cmb33 = new DataGridViewComboBoxCell();
            cmb33.DataSource = itemsbuttons;
            dataGridView1[1, 33] = cmb33;
            dataGridView1[1, 33].Value = shown ? "LeftButtonDPAD_DOWN" : xoutDown.ToString();
            dataGridView1.Rows.Add("Left"); //34
            DataGridViewComboBoxCell cmb34 = new DataGridViewComboBoxCell();
            cmb34.DataSource = itemsbuttons;
            dataGridView1[1, 34] = cmb34;
            dataGridView1[1, 34].Value = shown ? "LeftButtonDPAD_LEFT" : xoutLeft.ToString();
            dataGridView1.Rows.Add("Right"); //35
            DataGridViewComboBoxCell cmb35 = new DataGridViewComboBoxCell();
            cmb35.DataSource = itemsbuttons;
            dataGridView1[1, 35] = cmb35;
            dataGridView1[1, 35].Value = shown ? "LeftButtonDPAD_RIGHT" : xoutRight.ToString();
            dataGridView1.Rows.Add("Up"); //36
            DataGridViewComboBoxCell cmb36 = new DataGridViewComboBoxCell();
            cmb36.DataSource = itemsbuttons;
            dataGridView1[1, 36] = cmb36;
            dataGridView1[1, 36].Value = shown ? "LeftButtonDPAD_UP" : xoutUp.ToString();
            dataGridView1.Rows.Add("RightStick"); //37
            DataGridViewComboBoxCell cmb37 = new DataGridViewComboBoxCell();
            cmb37.DataSource = itemsbuttons;
            dataGridView1[1, 37] = cmb37;
            dataGridView1[1, 37].Value = shown ? "LeftButtonSMA" : xoutRightStick.ToString();
            dataGridView1.Rows.Add("LeftStick"); //38
            DataGridViewComboBoxCell cmb38 = new DataGridViewComboBoxCell();
            cmb38.DataSource = itemsbuttons;
            dataGridView1[1, 38] = cmb38;
            dataGridView1[1, 38].Value = shown ? "LeftButtonSHOULDER_2" : xoutLeftStick.ToString();
            dataGridView1.Rows.Add("A"); //39
            DataGridViewComboBoxCell cmb39 = new DataGridViewComboBoxCell();
            cmb39.DataSource = itemsbuttons;
            dataGridView1[1, 39] = cmb39;
            dataGridView1[1, 39].Value = shown ? "LeftButtonSHOULDER_1" : xoutA.ToString();
            dataGridView1.Rows.Add("Back"); //40
            DataGridViewComboBoxCell cmb40 = new DataGridViewComboBoxCell();
            cmb40.DataSource = itemsbuttons;
            dataGridView1[1, 40] = cmb40;
            dataGridView1[1, 40].Value = shown ? "mWSButtonStateOne" : xoutBack.ToString();
            dataGridView1.Rows.Add("Start"); //41
            DataGridViewComboBoxCell cmb41 = new DataGridViewComboBoxCell();
            cmb41.DataSource = itemsbuttons;
            dataGridView1[1, 41] = cmb41;
            dataGridView1[1, 41].Value = shown ? "mWSLeftButtonTC" : xoutStart.ToString();
            dataGridView1.Rows.Add("X"); //42
            DataGridViewComboBoxCell cmb42 = new DataGridViewComboBoxCell();
            cmb42.DataSource = itemsbuttons;
            dataGridView1[1, 42] = cmb42;
            dataGridView1[1, 42].Value = shown ? "mWSButtonStateHFront" : xoutX.ToString();
            dataGridView1.Rows.Add("RightBumper"); //43
            DataGridViewComboBoxCell cmb43 = new DataGridViewComboBoxCell();
            cmb43.DataSource = itemsbuttons;
            dataGridView1[1, 43] = cmb43;
            dataGridView1[1, 43].Value = shown ? "mWSButtonStatePU" : xoutRightBumper.ToString();
            dataGridView1.Rows.Add("LeftBumper"); //44
            DataGridViewComboBoxCell cmb44 = new DataGridViewComboBoxCell();
            cmb44.DataSource = itemsbuttons;
            dataGridView1[1, 44] = cmb44;
            dataGridView1[1, 44].Value = shown ? "mWSButtonStateMU" : xoutLeftBumper.ToString();
            dataGridView1.Rows.Add("B"); //45
            DataGridViewComboBoxCell cmb45 = new DataGridViewComboBoxCell();
            cmb45.DataSource = itemsbuttons;
            dataGridView1[1, 45] = cmb45;
            dataGridView1[1, 45].Value = shown ? "mWSButtonStateDown" : xoutB.ToString();
            dataGridView1.Rows.Add("Y"); //46
            DataGridViewComboBoxCell cmb46 = new DataGridViewComboBoxCell();
            cmb46.DataSource = itemsbuttons;
            dataGridView1[1, 46] = cmb46;
            dataGridView1[1, 46].Value = shown ? "mWSButtonStateLR" : xoutY.ToString();
            dataGridView1.Rows.Add("RightTrigger"); //47
            DataGridViewComboBoxCell cmb47 = new DataGridViewComboBoxCell();
            cmb47.DataSource = itemsbuttons;
            dataGridView1[1, 47] = cmb47;
            dataGridView1[1, 47].Value = shown ? "mWSButtonStateB" : xoutRightTrigger.ToString();
            dataGridView1.Rows.Add("LeftTrigger"); //48
            DataGridViewComboBoxCell cmb48 = new DataGridViewComboBoxCell();
            cmb48.DataSource = itemsbuttons;
            dataGridView1[1, 48] = cmb48;
            dataGridView1[1, 48].Value = shown ? "mWSButtonStateA" : xoutLeftTrigger.ToString();
            dataGridView1.Rows.Add("Switch Down"); //49
            DataGridViewComboBoxCell cmb49 = new DataGridViewComboBoxCell();
            cmb49.DataSource = itemsbuttons;
            dataGridView1[1, 49] = cmb49;
            dataGridView1[1, 49].Value = shown ? "LeftButtonDPAD_DOWN" : xoutswitchDown.ToString();
            dataGridView1.Rows.Add("Switch Left"); //50
            DataGridViewComboBoxCell cmb50 = new DataGridViewComboBoxCell();
            cmb50.DataSource = itemsbuttons;
            dataGridView1[1, 50] = cmb50;
            dataGridView1[1, 50].Value = shown ? "LeftButtonDPAD_LEFT" : xoutswitchLeft.ToString();
            dataGridView1.Rows.Add("Switch Right"); //51
            DataGridViewComboBoxCell cmb51 = new DataGridViewComboBoxCell();
            cmb51.DataSource = itemsbuttons;
            dataGridView1[1, 51] = cmb51;
            dataGridView1[1, 51].Value = shown ? "LeftButtonDPAD_RIGHT" : xoutswitchRight.ToString();
            dataGridView1.Rows.Add("Switch Up"); //52
            DataGridViewComboBoxCell cmb52 = new DataGridViewComboBoxCell();
            cmb52.DataSource = itemsbuttons;
            dataGridView1[1, 52] = cmb52;
            dataGridView1[1, 52].Value = shown ? "LeftButtonDPAD_UP" : xoutswitchUp.ToString();
            dataGridView1.Rows.Add("Switch RightStick"); //53
            DataGridViewComboBoxCell cmb53 = new DataGridViewComboBoxCell();
            cmb53.DataSource = itemsbuttons;
            dataGridView1[1, 53] = cmb53;
            dataGridView1[1, 53].Value = shown ? "LeftButtonSMA" : xoutswitchRightStick.ToString();
            dataGridView1.Rows.Add("Switch LeftStick"); //54
            DataGridViewComboBoxCell cmb54 = new DataGridViewComboBoxCell();
            cmb54.DataSource = itemsbuttons;
            dataGridView1[1, 54] = cmb54;
            dataGridView1[1, 54].Value = shown ? "LeftButtonSHOULDER_2" : xoutswitchLeftStick.ToString();
            dataGridView1.Rows.Add("Switch A"); //55
            DataGridViewComboBoxCell cmb55 = new DataGridViewComboBoxCell();
            cmb55.DataSource = itemsbuttons;
            dataGridView1[1, 55] = cmb55;
            dataGridView1[1, 55].Value = shown ? "LeftButtonSHOULDER_1" : xoutswitchA.ToString();
            dataGridView1.Rows.Add("Switch Back"); //56
            DataGridViewComboBoxCell cmb56 = new DataGridViewComboBoxCell();
            cmb56.DataSource = itemsbuttons;
            dataGridView1[1, 56] = cmb56;
            dataGridView1[1, 56].Value = shown ? "mWSButtonStateOne" : xoutswitchBack.ToString();
            dataGridView1.Rows.Add("Switch Start"); //57
            DataGridViewComboBoxCell cmb57 = new DataGridViewComboBoxCell();
            cmb57.DataSource = itemsbuttons;
            dataGridView1[1, 57] = cmb57;
            dataGridView1[1, 57].Value = shown ? "mWSLeftButtonTC" : xoutswitchStart.ToString();
            dataGridView1.Rows.Add("Switch X"); //58
            DataGridViewComboBoxCell cmb58 = new DataGridViewComboBoxCell();
            cmb58.DataSource = itemsbuttons;
            dataGridView1[1, 58] = cmb58;
            dataGridView1[1, 58].Value = shown ? "mWSButtonStateHFront" : xoutswitchX.ToString();
            dataGridView1.Rows.Add("Switch RightBumper"); //59
            DataGridViewComboBoxCell cmb59 = new DataGridViewComboBoxCell();
            cmb59.DataSource = itemsbuttons;
            dataGridView1[1, 59] = cmb59;
            dataGridView1[1, 59].Value = shown ? "mWSButtonStatePU" : xoutswitchRightBumper.ToString();
            dataGridView1.Rows.Add("Switch LeftBumper"); //60
            DataGridViewComboBoxCell cmb60 = new DataGridViewComboBoxCell();
            cmb60.DataSource = itemsbuttons;
            dataGridView1[1, 60] = cmb60;
            dataGridView1[1, 60].Value = shown ? "mWSButtonStateMU" : xoutswitchLeftBumper.ToString();
            dataGridView1.Rows.Add("Switch B"); //61
            DataGridViewComboBoxCell cmb61 = new DataGridViewComboBoxCell();
            cmb61.DataSource = itemsbuttons;
            dataGridView1[1, 61] = cmb61;
            dataGridView1[1, 61].Value = shown ? "mWSButtonStateDown" : xoutswitchB.ToString();
            dataGridView1.Rows.Add("Switch Y"); //62
            DataGridViewComboBoxCell cmb62 = new DataGridViewComboBoxCell();
            cmb62.DataSource = itemsbuttons;
            dataGridView1[1, 62] = cmb62;
            dataGridView1[1, 62].Value = shown ? "mWSButtonStateLR" : xoutswitchY.ToString();
            dataGridView1.Rows.Add("Switch RightTrigger"); //63
            DataGridViewComboBoxCell cmb63 = new DataGridViewComboBoxCell();
            cmb63.DataSource = itemsbuttons;
            dataGridView1[1, 63] = cmb63;
            dataGridView1[1, 63].Value = shown ? "mWSButtonStateB" : xoutswitchRightTrigger.ToString();
            dataGridView1.Rows.Add("Switch LeftTrigger"); //64
            DataGridViewComboBoxCell cmb64 = new DataGridViewComboBoxCell();
            cmb64.DataSource = itemsbuttons;
            dataGridView1[1, 64] = cmb64;
            dataGridView1[1, 64].Value = shown ? "mWSButtonStateA" : xoutswitchLeftTrigger.ToString();
            dataGridView1.Rows.Add("left stick right"); //65
            DataGridViewComboBoxCell cmb65 = new DataGridViewComboBoxCell();
            cmb65.DataSource = itemsbuttons;
            dataGridView1[1, 65] = cmb65;
            dataGridView1[1, 65].Value = shown ? "none" : xoutLeftStickRight.ToString();
            dataGridView1.Rows.Add("left stick left"); //66
            DataGridViewComboBoxCell cmb66 = new DataGridViewComboBoxCell();
            cmb66.DataSource = itemsbuttons;
            dataGridView1[1, 66] = cmb66;
            dataGridView1[1, 66].Value = shown ? "none" : xoutLeftStickLeft.ToString();
            dataGridView1.Rows.Add("left stick up"); //67
            DataGridViewComboBoxCell cmb67 = new DataGridViewComboBoxCell();
            cmb67.DataSource = itemsbuttons;
            dataGridView1[1, 67] = cmb67;
            dataGridView1[1, 67].Value = shown ? "none" : xoutLeftStickUp.ToString();
            dataGridView1.Rows.Add("left stick down"); //68
            DataGridViewComboBoxCell cmb68 = new DataGridViewComboBoxCell();
            cmb68.DataSource = itemsbuttons;
            dataGridView1[1, 68] = cmb68;
            dataGridView1[1, 68].Value = shown ? "none" : xoutLeftStickDown.ToString();
            dataGridView1.Rows.Add("right stick right"); //69
            DataGridViewComboBoxCell cmb69 = new DataGridViewComboBoxCell();
            cmb69.DataSource = itemsbuttons;
            dataGridView1[1, 69] = cmb69;
            dataGridView1[1, 69].Value = shown ? "none" : xoutRightStickRight.ToString();
            dataGridView1.Rows.Add("right stick left"); //70
            DataGridViewComboBoxCell cmb70 = new DataGridViewComboBoxCell();
            cmb70.DataSource = itemsbuttons;
            dataGridView1[1, 70] = cmb70;
            dataGridView1[1, 70].Value = shown ? "none" : xoutRightStickLeft.ToString();
            dataGridView1.Rows.Add("right stick up"); //71
            DataGridViewComboBoxCell cmb71 = new DataGridViewComboBoxCell();
            cmb71.DataSource = itemsbuttons;
            dataGridView1[1, 71] = cmb71;
            dataGridView1[1, 71].Value = shown ? "none" : xoutRightStickUp.ToString();
            dataGridView1.Rows.Add("right stick down"); //72
            DataGridViewComboBoxCell cmb72 = new DataGridViewComboBoxCell();
            cmb72.DataSource = itemsbuttons;
            dataGridView1[1, 72] = cmb72;
            dataGridView1[1, 72].Value = shown ? "none" : xoutRightStickDown.ToString();
            dataGridView1.Rows.Add("left stick x"); //73
            DataGridViewComboBoxCell cmb73 = new DataGridViewComboBoxCell();
            cmb73.DataSource = itemsaxis;
            dataGridView1[1, 73] = cmb73;
            dataGridView1[1, 73].Value = shown ? "leftstickx" : xoutLeftStickX.ToString();
            dataGridView1.Rows.Add("left stick y"); //74
            DataGridViewComboBoxCell cmb74 = new DataGridViewComboBoxCell();
            cmb74.DataSource = itemsaxis;
            dataGridView1[1, 74] = cmb74;
            dataGridView1[1, 74].Value = shown ? "leftsticky" : xoutLeftStickY.ToString();
            dataGridView1.Rows.Add("right stick x"); //75
            DataGridViewComboBoxCell cmb75 = new DataGridViewComboBoxCell();
            cmb75.DataSource = itemsaxis;
            dataGridView1[1, 75] = cmb75;
            dataGridView1[1, 75].Value = shown ? "irx" : xoutRightStickX.ToString();
            dataGridView1.Rows.Add("right stick y"); //76
            DataGridViewComboBoxCell cmb76 = new DataGridViewComboBoxCell();
            cmb76.DataSource = itemsaxis;
            dataGridView1[1, 76] = cmb76;
            dataGridView1[1, 76].Value = shown ? "iry" : xoutRightStickY.ToString();
            dataGridView1.Rows.Add("switch left stick right"); //77
            DataGridViewComboBoxCell cmb77 = new DataGridViewComboBoxCell();
            cmb77.DataSource = itemsbuttons;
            dataGridView1[1, 77] = cmb77;
            dataGridView1[1, 77].Value = shown ? "none" : xoutswitchLeftStickRight.ToString();
            dataGridView1.Rows.Add("switch left stick left"); //78
            DataGridViewComboBoxCell cmb78 = new DataGridViewComboBoxCell();
            cmb78.DataSource = itemsbuttons;
            dataGridView1[1, 78] = cmb78;
            dataGridView1[1, 78].Value = shown ? "none" : xoutswitchLeftStickLeft.ToString();
            dataGridView1.Rows.Add("switch left stick up"); //79
            DataGridViewComboBoxCell cmb79 = new DataGridViewComboBoxCell();
            cmb79.DataSource = itemsbuttons;
            dataGridView1[1, 79] = cmb79;
            dataGridView1[1, 79].Value = shown ? "none" : xoutswitchLeftStickUp.ToString();
            dataGridView1.Rows.Add("switch left stick down"); //80
            DataGridViewComboBoxCell cmb80 = new DataGridViewComboBoxCell();
            cmb80.DataSource = itemsbuttons;
            dataGridView1[1, 80] = cmb80;
            dataGridView1[1, 80].Value = shown ? "none" : xoutswitchLeftStickDown.ToString();
            dataGridView1.Rows.Add("switch right stick right"); //81
            DataGridViewComboBoxCell cmb81 = new DataGridViewComboBoxCell();
            cmb81.DataSource = itemsbuttons;
            dataGridView1[1, 81] = cmb81;
            dataGridView1[1, 81].Value = shown ? "none" : xoutswitchRightStickRight.ToString();
            dataGridView1.Rows.Add("switch right stick left"); //82
            DataGridViewComboBoxCell cmb82 = new DataGridViewComboBoxCell();
            cmb82.DataSource = itemsbuttons;
            dataGridView1[1, 82] = cmb82;
            dataGridView1[1, 82].Value = shown ? "none" : xoutswitchRightStickLeft.ToString();
            dataGridView1.Rows.Add("switch right stick up"); //83
            DataGridViewComboBoxCell cmb83 = new DataGridViewComboBoxCell();
            cmb83.DataSource = itemsbuttons;
            dataGridView1[1, 83] = cmb83;
            dataGridView1[1, 83].Value = shown ? "none" : xoutswitchRightStickUp.ToString();
            dataGridView1.Rows.Add("switch right stick down"); //84
            DataGridViewComboBoxCell cmb84 = new DataGridViewComboBoxCell();
            cmb84.DataSource = itemsbuttons;
            dataGridView1[1, 84] = cmb84;
            dataGridView1[1, 84].Value = shown ? "none" : xoutswitchRightStickDown.ToString();
            dataGridView1.Rows.Add("switch left stick x"); //85
            DataGridViewComboBoxCell cmb85 = new DataGridViewComboBoxCell();
            cmb85.DataSource = itemsaxis;
            dataGridView1[1, 85] = cmb85;
            dataGridView1[1, 85].Value = shown ? "irx" : xoutswitchLeftStickX.ToString();
            dataGridView1.Rows.Add("switch left stick y"); //86
            DataGridViewComboBoxCell cmb86 = new DataGridViewComboBoxCell();
            cmb86.DataSource = itemsaxis;
            dataGridView1[1, 86] = cmb86;
            dataGridView1[1, 86].Value = shown ? "iry" : xoutswitchLeftStickY.ToString();
            dataGridView1.Rows.Add("switch right stick x"); //87
            DataGridViewComboBoxCell cmb87 = new DataGridViewComboBoxCell();
            cmb87.DataSource = itemsaxis;
            dataGridView1[1, 87] = cmb87;
            dataGridView1[1, 87].Value = shown ? "leftstickx" : xoutswitchRightStickX.ToString();
            dataGridView1.Rows.Add("switch right stick y"); //88
            DataGridViewComboBoxCell cmb88 = new DataGridViewComboBoxCell();
            cmb88.DataSource = itemsaxis;
            dataGridView1[1, 88] = cmb88;
            dataGridView1[1, 88].Value = shown ? "leftsticky" : xoutswitchRightStickY.ToString();
            dataGridView2.AutoResizeColumns();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ColumnCount = 2;
            dataGridView2.Columns[0].Name = "Outputs";
            dataGridView2.Columns[1].Name = "Inputs";
            dataGridView2.Rows.Add("Game", shown ? "Control" : games.ToString()); //0
            dataGridView2.Rows.Add("viewpower05x", shown ? "0" : viewpower05xs.ToString()); //1
            dataGridView2.Rows.Add("viewpower1x", shown ? "0" : viewpower1xs.ToString()); //2
            dataGridView2.Rows.Add("viewpower2x", shown ? "0.15" : viewpower2xs.ToString()); //3
            dataGridView2.Rows.Add("viewpower3x", shown ? "0.85" : viewpower3xs.ToString()); //4
            dataGridView2.Rows.Add("viewpower05y", shown ? "0" : viewpower05ys.ToString()); //5
            dataGridView2.Rows.Add("viewpower1y", shown ? "0" : viewpower1ys.ToString()); //6
            dataGridView2.Rows.Add("viewpower2y", shown ? "0.15" : viewpower2ys.ToString()); //7
            dataGridView2.Rows.Add("viewpower3y", shown ? "0.85" : viewpower3ys.ToString()); //8
            dataGridView2.Rows.Add("pollrate", shown ? "10" : pollrate.ToString()); //9
            dataGridView2.Rows.Add("dzx", shown ? "0.0" : dzxs.ToString()); //10
            dataGridView2.Rows.Add("dzy", shown ? "0.0" : dzys.ToString()); //11
            dataGridView2.Rows.Add("lowsensx", shown ? "1.0" : lowsensxs.ToString()); //12
            dataGridView2.Rows.Add("lowsensy", shown ? "1.0" : lowsensys.ToString()); //13
            dataGridView2.Rows.Add("lowsensbrinkaex", shown ? "lowsensbrinkaex" : lowsensbrinkaex.ToString()); //14
            dataGridView2.Rows.Add("lowsensbrinkaey", shown ? "lowsensbrinkaey" : lowsensbrinkaey.ToString()); //15
            dataGridView2.Rows.Add("brink"); //16
            DataGridViewComboBoxCell cmbs16 = new DataGridViewComboBoxCell();
            cmbs16.DataSource = itemsbools;
            dataGridView2[1, 16] = cmbs16;
            dataGridView2[1, 16].Value = shown ? "True" : brink.ToString();
            dataGridView2.Rows.Add("mw3"); //17
            DataGridViewComboBoxCell cmbs17 = new DataGridViewComboBoxCell();
            cmbs17.DataSource = itemsbools;
            dataGridView2[1, 17] = cmbs17;
            dataGridView2[1, 17].Value = shown ? "True" : mw3.ToString();
            dataGridView2.Rows.Add("mw3ae"); //18
            DataGridViewComboBoxCell cmbs18 = new DataGridViewComboBoxCell();
            cmbs18.DataSource = itemsbools;
            dataGridView2[1, 18] = cmbs18;
            dataGridView2[1, 18].Value = shown ? "False" : mw3ae.ToString();
            dataGridView2.Rows.Add("brinkae"); //19
            DataGridViewComboBoxCell cmbs19 = new DataGridViewComboBoxCell();
            cmbs19.DataSource = itemsbools;
            dataGridView2[1, 19] = cmbs19;
            dataGridView2[1, 19].Value = shown ? "False" : brinkae.ToString();
            dataGridView2.Rows.Add("desktop"); //20
            DataGridViewComboBoxCell cmbs20 = new DataGridViewComboBoxCell();
            cmbs20.DataSource = itemsbools;
            dataGridView2[1, 20] = cmbs20;
            dataGridView2[1, 20].Value = shown ? "True" : desktop.ToString();
            dataGridView2.Rows.Add("centery", shown ? "140" : centerys.ToString()); //21
            dataGridView2.Rows.Add("driver type"); //22
            DataGridViewComboBoxCell cmbs22 = new DataGridViewComboBoxCell();
            cmbs22.DataSource = itemsdrivertypes;
            dataGridView2[1, 22] = cmbs22;
            dataGridView2[1, 22].Value = shown ? "sendinput" : drivertype.ToString();
            dataGridView2.Rows.Add("SendMouseEventButtonLeft"); //23
            DataGridViewComboBoxCell cmbs23 = new DataGridViewComboBoxCell();
            cmbs23.DataSource = itemsbuttons;
            dataGridView2[1, 23] = cmbs23;
            dataGridView2[1, 23].Value = shown ? "mWSButtonStateB" : s1.ToString();
            dataGridView2.Rows.Add("SendMouseEventButtonRight"); //24
            DataGridViewComboBoxCell cmbs24 = new DataGridViewComboBoxCell();
            cmbs24.DataSource = itemsbuttons;
            dataGridView2[1, 24] = cmbs24;
            dataGridView2[1, 24].Value = shown ? "mWSButtonStateA" : s2.ToString();
            dataGridView2.Rows.Add("SendMouseEventButtonMiddle"); //25
            DataGridViewComboBoxCell cmbs25 = new DataGridViewComboBoxCell();
            cmbs25.DataSource = itemsbuttons;
            dataGridView2[1, 25] = cmbs25;
            dataGridView2[1, 25].Value = shown ? "none" : s3.ToString();
            dataGridView2.Rows.Add("SendMouseEventButtonWheelUp"); //26
            DataGridViewComboBoxCell cmbs26 = new DataGridViewComboBoxCell();
            cmbs26.DataSource = itemsbuttons;
            dataGridView2[1, 26] = cmbs26;
            dataGridView2[1, 26].Value = shown ? "none" : s4.ToString();
            dataGridView2.Rows.Add("SendMouseEventButtonWheelDown"); //27
            DataGridViewComboBoxCell cmbs27 = new DataGridViewComboBoxCell();
            cmbs27.DataSource = itemsbuttons;
            dataGridView2[1, 27] = cmbs27;
            dataGridView2[1, 27].Value = shown ? "none" : s5.ToString();
            dataGridView2.Rows.Add("VK_LEFT"); //28
            DataGridViewComboBoxCell cmbs28 = new DataGridViewComboBoxCell();
            cmbs28.DataSource = itemsbuttons;
            dataGridView2[1, 28] = cmbs28;
            dataGridView2[1, 28].Value = shown ? "none" : s6.ToString();
            dataGridView2.Rows.Add("VK_RIGHT"); //29
            DataGridViewComboBoxCell cmbs29 = new DataGridViewComboBoxCell();
            cmbs29.DataSource = itemsbuttons;
            dataGridView2[1, 29] = cmbs29;
            dataGridView2[1, 29].Value = shown ? "none" : s7.ToString();
            dataGridView2.Rows.Add("VK_UP"); //30
            DataGridViewComboBoxCell cmbs30 = new DataGridViewComboBoxCell();
            cmbs30.DataSource = itemsbuttons;
            dataGridView2[1, 30] = cmbs30;
            dataGridView2[1, 30].Value = shown ? "none" : s8.ToString();
            dataGridView2.Rows.Add("VK_DOWN"); //31
            DataGridViewComboBoxCell cmbs31 = new DataGridViewComboBoxCell();
            cmbs31.DataSource = itemsbuttons;
            dataGridView2[1, 31] = cmbs31;
            dataGridView2[1, 31].Value = shown ? "none" : s9.ToString();
            dataGridView2.Rows.Add("VK_LBUTTON"); //32
            DataGridViewComboBoxCell cmbs32 = new DataGridViewComboBoxCell();
            cmbs32.DataSource = itemsbuttons;
            dataGridView2[1, 32] = cmbs32;
            dataGridView2[1, 32].Value = shown ? "none" : s10.ToString();
            dataGridView2.Rows.Add("VK_RBUTTON"); //33
            DataGridViewComboBoxCell cmbs33 = new DataGridViewComboBoxCell();
            cmbs33.DataSource = itemsbuttons;
            dataGridView2[1, 33] = cmbs33;
            dataGridView2[1, 33].Value = shown ? "none" : s11.ToString();
            dataGridView2.Rows.Add("VK_CANCEL"); //34
            DataGridViewComboBoxCell cmbs34 = new DataGridViewComboBoxCell();
            cmbs34.DataSource = itemsbuttons;
            dataGridView2[1, 34] = cmbs34;
            dataGridView2[1, 34].Value = shown ? "none" : s12.ToString();
            dataGridView2.Rows.Add("VK_MBUTTON"); //35
            DataGridViewComboBoxCell cmbs35 = new DataGridViewComboBoxCell();
            cmbs35.DataSource = itemsbuttons;
            dataGridView2[1, 35] = cmbs35;
            dataGridView2[1, 35].Value = shown ? "none" : s13.ToString();
            dataGridView2.Rows.Add("VK_XBUTTON1"); //36
            DataGridViewComboBoxCell cmbs36 = new DataGridViewComboBoxCell();
            cmbs36.DataSource = itemsbuttons;
            dataGridView2[1, 36] = cmbs36;
            dataGridView2[1, 36].Value = shown ? "none" : s14.ToString();
            dataGridView2.Rows.Add("VK_XBUTTON2"); //37
            DataGridViewComboBoxCell cmbs37 = new DataGridViewComboBoxCell();
            cmbs37.DataSource = itemsbuttons;
            dataGridView2[1, 37] = cmbs37;
            dataGridView2[1, 37].Value = shown ? "none" : s15.ToString();
            dataGridView2.Rows.Add("VK_BACK"); //38
            DataGridViewComboBoxCell cmbs38 = new DataGridViewComboBoxCell();
            cmbs38.DataSource = itemsbuttons;
            dataGridView2[1, 38] = cmbs38;
            dataGridView2[1, 38].Value = shown ? "none" : s16.ToString();
            dataGridView2.Rows.Add("VK_Tab"); //39
            DataGridViewComboBoxCell cmbs39 = new DataGridViewComboBoxCell();
            cmbs39.DataSource = itemsbuttons;
            dataGridView2[1, 39] = cmbs39;
            dataGridView2[1, 39].Value = shown ? "none" : s17.ToString();
            dataGridView2.Rows.Add("VK_CLEAR"); //40
            DataGridViewComboBoxCell cmbs40 = new DataGridViewComboBoxCell();
            cmbs40.DataSource = itemsbuttons;
            dataGridView2[1, 40] = cmbs40;
            dataGridView2[1, 40].Value = shown ? "none" : s18.ToString();
            dataGridView2.Rows.Add("VK_Return"); //41
            DataGridViewComboBoxCell cmbs41 = new DataGridViewComboBoxCell();
            cmbs41.DataSource = itemsbuttons;
            dataGridView2[1, 41] = cmbs41;
            dataGridView2[1, 41].Value = shown ? "mWSButtonStateOne" : s19.ToString();
            dataGridView2.Rows.Add("VK_SHIFT"); //42
            DataGridViewComboBoxCell cmbs42 = new DataGridViewComboBoxCell();
            cmbs42.DataSource = itemsbuttons;
            dataGridView2[1, 42] = cmbs42;
            dataGridView2[1, 42].Value = shown ? "none" : s20.ToString();
            dataGridView2.Rows.Add("VK_CONTROL"); //43
            DataGridViewComboBoxCell cmbs43 = new DataGridViewComboBoxCell();
            cmbs43.DataSource = itemsbuttons;
            dataGridView2[1, 43] = cmbs43;
            dataGridView2[1, 43].Value = shown ? "none" : s21.ToString();
            dataGridView2.Rows.Add("VK_MENU"); //44
            DataGridViewComboBoxCell cmbs44 = new DataGridViewComboBoxCell();
            cmbs44.DataSource = itemsbuttons;
            dataGridView2[1, 44] = cmbs44;
            dataGridView2[1, 44].Value = shown ? "none" : s22.ToString();
            dataGridView2.Rows.Add("VK_PAUSE"); //45
            DataGridViewComboBoxCell cmbs45 = new DataGridViewComboBoxCell();
            cmbs45.DataSource = itemsbuttons;
            dataGridView2[1, 45] = cmbs45;
            dataGridView2[1, 45].Value = shown ? "none" : s23.ToString();
            dataGridView2.Rows.Add("VK_CAPITAL"); //46
            DataGridViewComboBoxCell cmbs46 = new DataGridViewComboBoxCell();
            cmbs46.DataSource = itemsbuttons;
            dataGridView2[1, 46] = cmbs46;
            dataGridView2[1, 46].Value = shown ? "none" : s24.ToString();
            dataGridView2.Rows.Add("VK_KANA"); //47
            DataGridViewComboBoxCell cmbs47 = new DataGridViewComboBoxCell();
            cmbs47.DataSource = itemsbuttons;
            dataGridView2[1, 47] = cmbs47;
            dataGridView2[1, 47].Value = shown ? "none" : s25.ToString();
            dataGridView2.Rows.Add("VK_HANGEUL"); //48
            DataGridViewComboBoxCell cmbs48 = new DataGridViewComboBoxCell();
            cmbs48.DataSource = itemsbuttons;
            dataGridView2[1, 48] = cmbs48;
            dataGridView2[1, 48].Value = shown ? "none" : s26.ToString();
            dataGridView2.Rows.Add("VK_HANGUL"); //49
            DataGridViewComboBoxCell cmbs49 = new DataGridViewComboBoxCell();
            cmbs49.DataSource = itemsbuttons;
            dataGridView2[1, 49] = cmbs49;
            dataGridView2[1, 49].Value = shown ? "none" : s27.ToString();
            dataGridView2.Rows.Add("VK_JUNJA"); //50
            DataGridViewComboBoxCell cmbs50 = new DataGridViewComboBoxCell();
            cmbs50.DataSource = itemsbuttons;
            dataGridView2[1, 50] = cmbs50;
            dataGridView2[1, 50].Value = shown ? "none" : s28.ToString();
            dataGridView2.Rows.Add("VK_FINAL"); //51
            DataGridViewComboBoxCell cmbs51 = new DataGridViewComboBoxCell();
            cmbs51.DataSource = itemsbuttons;
            dataGridView2[1, 51] = cmbs51;
            dataGridView2[1, 51].Value = shown ? "none" : s29.ToString();
            dataGridView2.Rows.Add("VK_HANJA"); //52
            DataGridViewComboBoxCell cmbs52 = new DataGridViewComboBoxCell();
            cmbs52.DataSource = itemsbuttons;
            dataGridView2[1, 52] = cmbs52;
            dataGridView2[1, 52].Value = shown ? "none" : s30.ToString();
            dataGridView2.Rows.Add("VK_KANJI"); //53
            DataGridViewComboBoxCell cmbs53 = new DataGridViewComboBoxCell();
            cmbs53.DataSource = itemsbuttons;
            dataGridView2[1, 53] = cmbs53;
            dataGridView2[1, 53].Value = shown ? "none" : s31.ToString();
            dataGridView2.Rows.Add("VK_Escape"); //54
            DataGridViewComboBoxCell cmbs54 = new DataGridViewComboBoxCell();
            cmbs54.DataSource = itemsbuttons;
            dataGridView2[1, 54] = cmbs54;
            dataGridView2[1, 54].Value = shown ? "mWSButtonStateTwo" : s32.ToString();
            dataGridView2.Rows.Add("VK_CONVERT"); //55
            DataGridViewComboBoxCell cmbs55 = new DataGridViewComboBoxCell();
            cmbs55.DataSource = itemsbuttons;
            dataGridView2[1, 55] = cmbs55;
            dataGridView2[1, 55].Value = shown ? "none" : s33.ToString();
            dataGridView2.Rows.Add("VK_NONCONVERT"); //56
            DataGridViewComboBoxCell cmbs56 = new DataGridViewComboBoxCell();
            cmbs56.DataSource = itemsbuttons;
            dataGridView2[1, 56] = cmbs56;
            dataGridView2[1, 56].Value = shown ? "none" : s34.ToString();
            dataGridView2.Rows.Add("VK_ACCEPT"); //57
            DataGridViewComboBoxCell cmbs57 = new DataGridViewComboBoxCell();
            cmbs57.DataSource = itemsbuttons;
            dataGridView2[1, 57] = cmbs57;
            dataGridView2[1, 57].Value = shown ? "none" : s35.ToString();
            dataGridView2.Rows.Add("VK_MODECHANGE"); //58
            DataGridViewComboBoxCell cmbs58 = new DataGridViewComboBoxCell();
            cmbs58.DataSource = itemsbuttons;
            dataGridView2[1, 58] = cmbs58;
            dataGridView2[1, 58].Value = shown ? "none" : s36.ToString();
            dataGridView2.Rows.Add("VK_Space"); //59
            DataGridViewComboBoxCell cmbs59 = new DataGridViewComboBoxCell();
            cmbs59.DataSource = itemsbuttons;
            dataGridView2[1, 59] = cmbs59;
            dataGridView2[1, 59].Value = shown ? "LeftButtonSHOULDER_1" : s37.ToString();
            dataGridView2.Rows.Add("VK_PRIOR"); //60
            DataGridViewComboBoxCell cmbs60 = new DataGridViewComboBoxCell();
            cmbs60.DataSource = itemsbuttons;
            dataGridView2[1, 60] = cmbs60;
            dataGridView2[1, 60].Value = shown ? "none" : s38.ToString();
            dataGridView2.Rows.Add("VK_NEXT"); //61
            DataGridViewComboBoxCell cmbs61 = new DataGridViewComboBoxCell();
            cmbs61.DataSource = itemsbuttons;
            dataGridView2[1, 61] = cmbs61;
            dataGridView2[1, 61].Value = shown ? "none" : s39.ToString();
            dataGridView2.Rows.Add("VK_END"); //62
            DataGridViewComboBoxCell cmbs62 = new DataGridViewComboBoxCell();
            cmbs62.DataSource = itemsbuttons;
            dataGridView2[1, 62] = cmbs62;
            dataGridView2[1, 62].Value = shown ? "none" : s40.ToString();
            dataGridView2.Rows.Add("VK_HOME"); //63
            DataGridViewComboBoxCell cmbs63 = new DataGridViewComboBoxCell();
            cmbs63.DataSource = itemsbuttons;
            dataGridView2[1, 63] = cmbs63;
            dataGridView2[1, 63].Value = shown ? "none" : s41.ToString();
            dataGridView2.Rows.Add("VK_LEFT"); //64
            DataGridViewComboBoxCell cmbs64 = new DataGridViewComboBoxCell();
            cmbs64.DataSource = itemsbuttons;
            dataGridView2[1, 64] = cmbs64;
            dataGridView2[1, 64].Value = shown ? "none" : s42.ToString();
            dataGridView2.Rows.Add("VK_UP"); //65
            DataGridViewComboBoxCell cmbs65 = new DataGridViewComboBoxCell();
            cmbs65.DataSource = itemsbuttons;
            dataGridView2[1, 65] = cmbs65;
            dataGridView2[1, 65].Value = shown ? "none" : s43.ToString();
            dataGridView2.Rows.Add("VK_RIGHT"); //66
            DataGridViewComboBoxCell cmbs66 = new DataGridViewComboBoxCell();
            cmbs66.DataSource = itemsbuttons;
            dataGridView2[1, 66] = cmbs66;
            dataGridView2[1, 66].Value = shown ? "none" : s44.ToString();
            dataGridView2.Rows.Add("VK_DOWN"); //67
            DataGridViewComboBoxCell cmbs67 = new DataGridViewComboBoxCell();
            cmbs67.DataSource = itemsbuttons;
            dataGridView2[1, 67] = cmbs67;
            dataGridView2[1, 67].Value = shown ? "none" : s45.ToString();
            dataGridView2.Rows.Add("VK_SELECT"); //68
            DataGridViewComboBoxCell cmbs68 = new DataGridViewComboBoxCell();
            cmbs68.DataSource = itemsbuttons;
            dataGridView2[1, 68] = cmbs68;
            dataGridView2[1, 68].Value = shown ? "none" : s46.ToString();
            dataGridView2.Rows.Add("VK_PRINT"); //69
            DataGridViewComboBoxCell cmbs69 = new DataGridViewComboBoxCell();
            cmbs69.DataSource = itemsbuttons;
            dataGridView2[1, 69] = cmbs69;
            dataGridView2[1, 69].Value = shown ? "none" : s47.ToString();
            dataGridView2.Rows.Add("VK_EXECUTE"); //70
            DataGridViewComboBoxCell cmbs70 = new DataGridViewComboBoxCell();
            cmbs70.DataSource = itemsbuttons;
            dataGridView2[1, 70] = cmbs70;
            dataGridView2[1, 70].Value = shown ? "none" : s48.ToString();
            dataGridView2.Rows.Add("VK_SNAPSHOT"); //71
            DataGridViewComboBoxCell cmbs71 = new DataGridViewComboBoxCell();
            cmbs71.DataSource = itemsbuttons;
            dataGridView2[1, 71] = cmbs71;
            dataGridView2[1, 71].Value = shown ? "none" : s49.ToString();
            dataGridView2.Rows.Add("VK_INSERT"); //72
            DataGridViewComboBoxCell cmbs72 = new DataGridViewComboBoxCell();
            cmbs72.DataSource = itemsbuttons;
            dataGridView2[1, 72] = cmbs72;
            dataGridView2[1, 72].Value = shown ? "none" : s50.ToString();
            dataGridView2.Rows.Add("VK_DELETE"); //73
            DataGridViewComboBoxCell cmbs73 = new DataGridViewComboBoxCell();
            cmbs73.DataSource = itemsbuttons;
            dataGridView2[1, 73] = cmbs73;
            dataGridView2[1, 73].Value = shown ? "none" : s51.ToString();
            dataGridView2.Rows.Add("VK_HELP"); //74
            DataGridViewComboBoxCell cmbs74 = new DataGridViewComboBoxCell();
            cmbs74.DataSource = itemsbuttons;
            dataGridView2[1, 74] = cmbs74;
            dataGridView2[1, 74].Value = shown ? "none" : s52.ToString();
            dataGridView2.Rows.Add("VK_APOSTROPHE"); //75
            DataGridViewComboBoxCell cmbs75 = new DataGridViewComboBoxCell();
            cmbs75.DataSource = itemsbuttons;
            dataGridView2[1, 75] = cmbs75;
            dataGridView2[1, 75].Value = shown ? "none" : s53.ToString();
            dataGridView2.Rows.Add("VK_0"); //76
            DataGridViewComboBoxCell cmbs76 = new DataGridViewComboBoxCell();
            cmbs76.DataSource = itemsbuttons;
            dataGridView2[1, 76] = cmbs76;
            dataGridView2[1, 76].Value = shown ? "none" : s54.ToString();
            dataGridView2.Rows.Add("VK_1"); //77
            DataGridViewComboBoxCell cmbs77 = new DataGridViewComboBoxCell();
            cmbs77.DataSource = itemsbuttons;
            dataGridView2[1, 77] = cmbs77;
            dataGridView2[1, 77].Value = shown ? "none" : s55.ToString();
            dataGridView2.Rows.Add("VK_2"); //78
            DataGridViewComboBoxCell cmbs78 = new DataGridViewComboBoxCell();
            cmbs78.DataSource = itemsbuttons;
            dataGridView2[1, 78] = cmbs78;
            dataGridView2[1, 78].Value = shown ? "none" : s56.ToString();
            dataGridView2.Rows.Add("VK_3"); //79
            DataGridViewComboBoxCell cmbs79 = new DataGridViewComboBoxCell();
            cmbs79.DataSource = itemsbuttons;
            dataGridView2[1, 79] = cmbs79;
            dataGridView2[1, 79].Value = shown ? "none" : s57.ToString();
            dataGridView2.Rows.Add("VK_4"); //80
            DataGridViewComboBoxCell cmbs80 = new DataGridViewComboBoxCell();
            cmbs80.DataSource = itemsbuttons;
            dataGridView2[1, 80] = cmbs80;
            dataGridView2[1, 80].Value = shown ? "none" : s58.ToString();
            dataGridView2.Rows.Add("VK_5"); //81
            DataGridViewComboBoxCell cmbs81 = new DataGridViewComboBoxCell();
            cmbs81.DataSource = itemsbuttons;
            dataGridView2[1, 81] = cmbs81;
            dataGridView2[1, 81].Value = shown ? "none" : s59.ToString();
            dataGridView2.Rows.Add("VK_6"); //82
            DataGridViewComboBoxCell cmbs82 = new DataGridViewComboBoxCell();
            cmbs82.DataSource = itemsbuttons;
            dataGridView2[1, 82] = cmbs82;
            dataGridView2[1, 82].Value = shown ? "none" : s60.ToString();
            dataGridView2.Rows.Add("VK_7"); //83
            DataGridViewComboBoxCell cmbs83 = new DataGridViewComboBoxCell();
            cmbs83.DataSource = itemsbuttons;
            dataGridView2[1, 83] = cmbs83;
            dataGridView2[1, 83].Value = shown ? "none" : s61.ToString();
            dataGridView2.Rows.Add("VK_8"); //84
            DataGridViewComboBoxCell cmbs84 = new DataGridViewComboBoxCell();
            cmbs84.DataSource = itemsbuttons;
            dataGridView2[1, 84] = cmbs84;
            dataGridView2[1, 84].Value = shown ? "none" : s62.ToString();
            dataGridView2.Rows.Add("VK_9"); //85
            DataGridViewComboBoxCell cmbs85 = new DataGridViewComboBoxCell();
            cmbs85.DataSource = itemsbuttons;
            dataGridView2[1, 85] = cmbs85;
            dataGridView2[1, 85].Value = shown ? "none" : s63.ToString();
            dataGridView2.Rows.Add("VK_A"); //86
            DataGridViewComboBoxCell cmbs86 = new DataGridViewComboBoxCell();
            cmbs86.DataSource = itemsbuttons;
            dataGridView2[1, 86] = cmbs86;
            dataGridView2[1, 86].Value = shown ? "none" : s64.ToString();
            dataGridView2.Rows.Add("VK_B"); //87
            DataGridViewComboBoxCell cmbs87 = new DataGridViewComboBoxCell();
            cmbs87.DataSource = itemsbuttons;
            dataGridView2[1, 87] = cmbs87;
            dataGridView2[1, 87].Value = shown ? "none" : s65.ToString();
            dataGridView2.Rows.Add("VK_C"); //88
            DataGridViewComboBoxCell cmbs88 = new DataGridViewComboBoxCell();
            cmbs88.DataSource = itemsbuttons;
            dataGridView2[1, 88] = cmbs88;
            dataGridView2[1, 88].Value = shown ? "mWSButtonStateDown" : s66.ToString();
            dataGridView2.Rows.Add("VK_D"); //89
            DataGridViewComboBoxCell cmbs89 = new DataGridViewComboBoxCell();
            cmbs89.DataSource = itemsbuttons;
            dataGridView2[1, 89] = cmbs89;
            dataGridView2[1, 89].Value = shown ? "LeftButtonStickRight" : s67.ToString();
            dataGridView2.Rows.Add("VK_E"); //90
            DataGridViewComboBoxCell cmbs90 = new DataGridViewComboBoxCell();
            cmbs90.DataSource = itemsbuttons;
            dataGridView2[1, 90] = cmbs90;
            dataGridView2[1, 90].Value = shown ? "none" : s68.ToString();
            dataGridView2.Rows.Add("VK_F"); //91
            DataGridViewComboBoxCell cmbs91 = new DataGridViewComboBoxCell();
            cmbs91.DataSource = itemsbuttons;
            dataGridView2[1, 91] = cmbs91;
            dataGridView2[1, 91].Value = shown ? "mWSButtonStateHome" : s69.ToString();
            dataGridView2.Rows.Add("VK_G"); //92
            DataGridViewComboBoxCell cmbs92 = new DataGridViewComboBoxCell();
            cmbs92.DataSource = itemsbuttons;
            dataGridView2[1, 92] = cmbs92;
            dataGridView2[1, 92].Value = shown ? "mWSButtonStatePlus" : s70.ToString();
            dataGridView2.Rows.Add("VK_H"); //93
            DataGridViewComboBoxCell cmbs93 = new DataGridViewComboBoxCell();
            cmbs93.DataSource = itemsbuttons;
            dataGridView2[1, 93] = cmbs93;
            dataGridView2[1, 93].Value = shown ? "none" : s71.ToString();
            dataGridView2.Rows.Add("VK_I"); //94
            DataGridViewComboBoxCell cmbs94 = new DataGridViewComboBoxCell();
            cmbs94.DataSource = itemsbuttons;
            dataGridView2[1, 94] = cmbs94;
            dataGridView2[1, 94].Value = shown ? "none" : s72.ToString();
            dataGridView2.Rows.Add("VK_J"); //95
            DataGridViewComboBoxCell cmbs95 = new DataGridViewComboBoxCell();
            cmbs95.DataSource = itemsbuttons;
            dataGridView2[1, 95] = cmbs95;
            dataGridView2[1, 95].Value = shown ? "none" : s73.ToString();
            dataGridView2.Rows.Add("VK_K"); //96
            DataGridViewComboBoxCell cmbs96 = new DataGridViewComboBoxCell();
            cmbs96.DataSource = itemsbuttons;
            dataGridView2[1, 96] = cmbs96;
            dataGridView2[1, 96].Value = shown ? "none" : s74.ToString();
            dataGridView2.Rows.Add("VK_L"); //97
            DataGridViewComboBoxCell cmbs97 = new DataGridViewComboBoxCell();
            cmbs97.DataSource = itemsbuttons;
            dataGridView2[1, 97] = cmbs97;
            dataGridView2[1, 97].Value = shown ? "none" : s75.ToString();
            dataGridView2.Rows.Add("VK_M"); //98
            DataGridViewComboBoxCell cmbs98 = new DataGridViewComboBoxCell();
            cmbs98.DataSource = itemsbuttons;
            dataGridView2[1, 98] = cmbs98;
            dataGridView2[1, 98].Value = shown ? "none" : s76.ToString();
            dataGridView2.Rows.Add("VK_N"); //99
            DataGridViewComboBoxCell cmbs99 = new DataGridViewComboBoxCell();
            cmbs99.DataSource = itemsbuttons;
            dataGridView2[1, 99] = cmbs99;
            dataGridView2[1, 99].Value = shown ? "none" : s77.ToString();
            dataGridView2.Rows.Add("VK_O"); //100
            DataGridViewComboBoxCell cmbs100 = new DataGridViewComboBoxCell();
            cmbs100.DataSource = itemsbuttons;
            dataGridView2[1, 100] = cmbs100;
            dataGridView2[1, 100].Value = shown ? "none" : s78.ToString();
            dataGridView2.Rows.Add("VK_P"); //101
            DataGridViewComboBoxCell cmbs101 = new DataGridViewComboBoxCell();
            cmbs101.DataSource = itemsbuttons;
            dataGridView2[1, 101] = cmbs101;
            dataGridView2[1, 101].Value = shown ? "none" : s79.ToString();
            dataGridView2.Rows.Add("VK_Q"); //102
            DataGridViewComboBoxCell cmbs102 = new DataGridViewComboBoxCell();
            cmbs102.DataSource = itemsbuttons;
            dataGridView2[1, 102] = cmbs102;
            dataGridView2[1, 102].Value = shown ? "LeftButtonStickLeft" : s80.ToString();
            dataGridView2.Rows.Add("VK_R"); //103
            DataGridViewComboBoxCell cmbs103 = new DataGridViewComboBoxCell();
            cmbs103.DataSource = itemsbuttons;
            dataGridView2[1, 103] = cmbs103;
            dataGridView2[1, 103].Value = shown ? "mWSButtonStateFront" : s81.ToString();
            dataGridView2.Rows.Add("VK_S"); //104
            DataGridViewComboBoxCell cmbs104 = new DataGridViewComboBoxCell();
            cmbs104.DataSource = itemsbuttons;
            dataGridView2[1, 104] = cmbs104;
            dataGridView2[1, 104].Value = shown ? "LeftButtonStickDown" : s82.ToString();
            dataGridView2.Rows.Add("VK_T"); //105
            DataGridViewComboBoxCell cmbs105 = new DataGridViewComboBoxCell();
            cmbs105.DataSource = itemsbuttons;
            dataGridView2[1, 105] = cmbs105;
            dataGridView2[1, 105].Value = shown ? "mWSButtonStateMinus" : s83.ToString();
            dataGridView2.Rows.Add("VK_U"); //106
            DataGridViewComboBoxCell cmbs106 = new DataGridViewComboBoxCell();
            cmbs106.DataSource = itemsbuttons;
            dataGridView2[1, 106] = cmbs106;
            dataGridView2[1, 106].Value = shown ? "mWSButtonStateRight" : s84.ToString();
            dataGridView2.Rows.Add("VK_V"); //107
            DataGridViewComboBoxCell cmbs107 = new DataGridViewComboBoxCell();
            cmbs107.DataSource = itemsbuttons;
            dataGridView2[1, 107] = cmbs107;
            dataGridView2[1, 107].Value = shown ? "LeftButtonSMA" : s85.ToString();
            dataGridView2.Rows.Add("VK_W"); //108
            DataGridViewComboBoxCell cmbs108 = new DataGridViewComboBoxCell();
            cmbs108.DataSource = itemsbuttons;
            dataGridView2[1, 108] = cmbs108;
            dataGridView2[1, 108].Value = shown ? "none" : s86.ToString();
            dataGridView2.Rows.Add("VK_X"); //109
            DataGridViewComboBoxCell cmbs109 = new DataGridViewComboBoxCell();
            cmbs109.DataSource = itemsbuttons;
            dataGridView2[1, 109] = cmbs109;
            dataGridView2[1, 109].Value = shown ? "mWSButtonStateUp" : s87.ToString();
            dataGridView2.Rows.Add("VK_Y"); //110
            DataGridViewComboBoxCell cmbs110 = new DataGridViewComboBoxCell();
            cmbs110.DataSource = itemsbuttons;
            dataGridView2[1, 110] = cmbs110;
            dataGridView2[1, 110].Value = shown ? "mWSButtonStateLeft" : s88.ToString();
            dataGridView2.Rows.Add("VK_Z"); //111
            DataGridViewComboBoxCell cmbs111 = new DataGridViewComboBoxCell();
            cmbs111.DataSource = itemsbuttons;
            dataGridView2[1, 111] = cmbs111;
            dataGridView2[1, 111].Value = shown ? "LeftButtonStickUp" : s89.ToString();
            dataGridView2.Rows.Add("VK_LWIN"); //112
            DataGridViewComboBoxCell cmbs112 = new DataGridViewComboBoxCell();
            cmbs112.DataSource = itemsbuttons;
            dataGridView2[1, 112] = cmbs112;
            dataGridView2[1, 112].Value = shown ? "none" : s90.ToString();
            dataGridView2.Rows.Add("VK_RWIN"); //113
            DataGridViewComboBoxCell cmbs113 = new DataGridViewComboBoxCell();
            cmbs113.DataSource = itemsbuttons;
            dataGridView2[1, 113] = cmbs113;
            dataGridView2[1, 113].Value = shown ? "none" : s91.ToString();
            dataGridView2.Rows.Add("VK_APPS"); //114
            DataGridViewComboBoxCell cmbs114 = new DataGridViewComboBoxCell();
            cmbs114.DataSource = itemsbuttons;
            dataGridView2[1, 114] = cmbs114;
            dataGridView2[1, 114].Value = shown ? "none" : s92.ToString();
            dataGridView2.Rows.Add("VK_SLEEP"); //115
            DataGridViewComboBoxCell cmbs115 = new DataGridViewComboBoxCell();
            cmbs115.DataSource = itemsbuttons;
            dataGridView2[1, 115] = cmbs115;
            dataGridView2[1, 115].Value = shown ? "none" : s93.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD0"); //116
            DataGridViewComboBoxCell cmbs116 = new DataGridViewComboBoxCell();
            cmbs116.DataSource = itemsbuttons;
            dataGridView2[1, 116] = cmbs116;
            dataGridView2[1, 116].Value = shown ? "none" : s94.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD1"); //117
            DataGridViewComboBoxCell cmbs117 = new DataGridViewComboBoxCell();
            cmbs117.DataSource = itemsbuttons;
            dataGridView2[1, 117] = cmbs117;
            dataGridView2[1, 117].Value = shown ? "none" : s95.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD2"); //118
            DataGridViewComboBoxCell cmbs118 = new DataGridViewComboBoxCell();
            cmbs118.DataSource = itemsbuttons;
            dataGridView2[1, 118] = cmbs118;
            dataGridView2[1, 118].Value = shown ? "none" : s96.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD3"); //119
            DataGridViewComboBoxCell cmbs119 = new DataGridViewComboBoxCell();
            cmbs119.DataSource = itemsbuttons;
            dataGridView2[1, 119] = cmbs119;
            dataGridView2[1, 119].Value = shown ? "none" : s97.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD4"); //120
            DataGridViewComboBoxCell cmbs120 = new DataGridViewComboBoxCell();
            cmbs120.DataSource = itemsbuttons;
            dataGridView2[1, 120] = cmbs120;
            dataGridView2[1, 120].Value = shown ? "none" : s98.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD5"); //121
            DataGridViewComboBoxCell cmbs121 = new DataGridViewComboBoxCell();
            cmbs121.DataSource = itemsbuttons;
            dataGridView2[1, 121] = cmbs121;
            dataGridView2[1, 121].Value = shown ? "none" : s99.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD6"); //122
            DataGridViewComboBoxCell cmbs122 = new DataGridViewComboBoxCell();
            cmbs122.DataSource = itemsbuttons;
            dataGridView2[1, 122] = cmbs122;
            dataGridView2[1, 122].Value = shown ? "none" : s100.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD7"); //123
            DataGridViewComboBoxCell cmbs123 = new DataGridViewComboBoxCell();
            cmbs123.DataSource = itemsbuttons;
            dataGridView2[1, 123] = cmbs123;
            dataGridView2[1, 123].Value = shown ? "none" : s101.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD8"); //124
            DataGridViewComboBoxCell cmbs124 = new DataGridViewComboBoxCell();
            cmbs124.DataSource = itemsbuttons;
            dataGridView2[1, 124] = cmbs124;
            dataGridView2[1, 124].Value = shown ? "none" : s102.ToString();
            dataGridView2.Rows.Add("VK_NUMPAD9"); //125
            DataGridViewComboBoxCell cmbs125 = new DataGridViewComboBoxCell();
            cmbs125.DataSource = itemsbuttons;
            dataGridView2[1, 125] = cmbs125;
            dataGridView2[1, 125].Value = shown ? "none" : s103.ToString();
            dataGridView2.Rows.Add("VK_MULTIPLY"); //126
            DataGridViewComboBoxCell cmbs126 = new DataGridViewComboBoxCell();
            cmbs126.DataSource = itemsbuttons;
            dataGridView2[1, 126] = cmbs126;
            dataGridView2[1, 126].Value = shown ? "none" : s104.ToString();
            dataGridView2.Rows.Add("VK_ADD"); //127
            DataGridViewComboBoxCell cmbs127 = new DataGridViewComboBoxCell();
            cmbs127.DataSource = itemsbuttons;
            dataGridView2[1, 127] = cmbs127;
            dataGridView2[1, 127].Value = shown ? "none" : s105.ToString();
            dataGridView2.Rows.Add("VK_SEPARATOR"); //128
            DataGridViewComboBoxCell cmbs128 = new DataGridViewComboBoxCell();
            cmbs128.DataSource = itemsbuttons;
            dataGridView2[1, 128] = cmbs128;
            dataGridView2[1, 128].Value = shown ? "none" : s106.ToString();
            dataGridView2.Rows.Add("VK_SUBTRACT"); //129
            DataGridViewComboBoxCell cmbs129 = new DataGridViewComboBoxCell();
            cmbs129.DataSource = itemsbuttons;
            dataGridView2[1, 129] = cmbs129;
            dataGridView2[1, 129].Value = shown ? "none" : s107.ToString();
            dataGridView2.Rows.Add("VK_DECIMAL"); //130
            DataGridViewComboBoxCell cmbs130 = new DataGridViewComboBoxCell();
            cmbs130.DataSource = itemsbuttons;
            dataGridView2[1, 130] = cmbs130;
            dataGridView2[1, 130].Value = shown ? "none" : s108.ToString();
            dataGridView2.Rows.Add("VK_DIVIDE"); //131
            DataGridViewComboBoxCell cmbs131 = new DataGridViewComboBoxCell();
            cmbs131.DataSource = itemsbuttons;
            dataGridView2[1, 131] = cmbs131;
            dataGridView2[1, 131].Value = shown ? "none" : s109.ToString();
            dataGridView2.Rows.Add("VK_F1"); //132
            DataGridViewComboBoxCell cmbs132 = new DataGridViewComboBoxCell();
            cmbs132.DataSource = itemsbuttons;
            dataGridView2[1, 132] = cmbs132;
            dataGridView2[1, 132].Value = shown ? "none" : s110.ToString();
            dataGridView2.Rows.Add("VK_F2"); //133
            DataGridViewComboBoxCell cmbs133 = new DataGridViewComboBoxCell();
            cmbs133.DataSource = itemsbuttons;
            dataGridView2[1, 133] = cmbs133;
            dataGridView2[1, 133].Value = shown ? "none" : s111.ToString();
            dataGridView2.Rows.Add("VK_F3"); //134
            DataGridViewComboBoxCell cmbs134 = new DataGridViewComboBoxCell();
            cmbs134.DataSource = itemsbuttons;
            dataGridView2[1, 134] = cmbs134;
            dataGridView2[1, 134].Value = shown ? "none" : s112.ToString();
            dataGridView2.Rows.Add("VK_F4"); //135
            DataGridViewComboBoxCell cmbs135 = new DataGridViewComboBoxCell();
            cmbs135.DataSource = itemsbuttons;
            dataGridView2[1, 135] = cmbs135;
            dataGridView2[1, 135].Value = shown ? "none" : s113.ToString();
            dataGridView2.Rows.Add("VK_F5"); //136
            DataGridViewComboBoxCell cmbs136 = new DataGridViewComboBoxCell();
            cmbs136.DataSource = itemsbuttons;
            dataGridView2[1, 136] = cmbs136;
            dataGridView2[1, 136].Value = shown ? "none" : s114.ToString();
            dataGridView2.Rows.Add("VK_F6"); //137
            DataGridViewComboBoxCell cmbs137 = new DataGridViewComboBoxCell();
            cmbs137.DataSource = itemsbuttons;
            dataGridView2[1, 137] = cmbs137;
            dataGridView2[1, 137].Value = shown ? "none" : s115.ToString();
            dataGridView2.Rows.Add("VK_F7"); //138
            DataGridViewComboBoxCell cmbs138 = new DataGridViewComboBoxCell();
            cmbs138.DataSource = itemsbuttons;
            dataGridView2[1, 138] = cmbs138;
            dataGridView2[1, 138].Value = shown ? "none" : s116.ToString();
            dataGridView2.Rows.Add("VK_F8"); //139
            DataGridViewComboBoxCell cmbs139 = new DataGridViewComboBoxCell();
            cmbs139.DataSource = itemsbuttons;
            dataGridView2[1, 139] = cmbs139;
            dataGridView2[1, 139].Value = shown ? "none" : s117.ToString();
            dataGridView2.Rows.Add("VK_F9"); //140
            DataGridViewComboBoxCell cmbs140 = new DataGridViewComboBoxCell();
            cmbs140.DataSource = itemsbuttons;
            dataGridView2[1, 140] = cmbs140;
            dataGridView2[1, 140].Value = shown ? "none" : s118.ToString();
            dataGridView2.Rows.Add("VK_F10"); //141
            DataGridViewComboBoxCell cmbs141 = new DataGridViewComboBoxCell();
            cmbs141.DataSource = itemsbuttons;
            dataGridView2[1, 141] = cmbs141;
            dataGridView2[1, 141].Value = shown ? "none" : s119.ToString();
            dataGridView2.Rows.Add("VK_F11"); //142
            DataGridViewComboBoxCell cmbs142 = new DataGridViewComboBoxCell();
            cmbs142.DataSource = itemsbuttons;
            dataGridView2[1, 142] = cmbs142;
            dataGridView2[1, 142].Value = shown ? "none" : s120.ToString();
            dataGridView2.Rows.Add("VK_F12"); //143
            DataGridViewComboBoxCell cmbs143 = new DataGridViewComboBoxCell();
            cmbs143.DataSource = itemsbuttons;
            dataGridView2[1, 143] = cmbs143;
            dataGridView2[1, 143].Value = shown ? "none" : s121.ToString();
            dataGridView2.Rows.Add("VK_F13"); //144
            DataGridViewComboBoxCell cmbs144 = new DataGridViewComboBoxCell();
            cmbs144.DataSource = itemsbuttons;
            dataGridView2[1, 144] = cmbs144;
            dataGridView2[1, 144].Value = shown ? "none" : s122.ToString();
            dataGridView2.Rows.Add("VK_F14"); //145
            DataGridViewComboBoxCell cmbs145 = new DataGridViewComboBoxCell();
            cmbs145.DataSource = itemsbuttons;
            dataGridView2[1, 145] = cmbs145;
            dataGridView2[1, 145].Value = shown ? "none" : s123.ToString();
            dataGridView2.Rows.Add("VK_F15"); //146
            DataGridViewComboBoxCell cmbs146 = new DataGridViewComboBoxCell();
            cmbs146.DataSource = itemsbuttons;
            dataGridView2[1, 146] = cmbs146;
            dataGridView2[1, 146].Value = shown ? "none" : s124.ToString();
            dataGridView2.Rows.Add("VK_F16"); //147
            DataGridViewComboBoxCell cmbs147 = new DataGridViewComboBoxCell();
            cmbs147.DataSource = itemsbuttons;
            dataGridView2[1, 147] = cmbs147;
            dataGridView2[1, 147].Value = shown ? "none" : s125.ToString();
            dataGridView2.Rows.Add("VK_F17"); //148
            DataGridViewComboBoxCell cmbs148 = new DataGridViewComboBoxCell();
            cmbs148.DataSource = itemsbuttons;
            dataGridView2[1, 148] = cmbs148;
            dataGridView2[1, 148].Value = shown ? "none" : s126.ToString();
            dataGridView2.Rows.Add("VK_F18"); //149
            DataGridViewComboBoxCell cmbs149 = new DataGridViewComboBoxCell();
            cmbs149.DataSource = itemsbuttons;
            dataGridView2[1, 149] = cmbs149;
            dataGridView2[1, 149].Value = shown ? "none" : s127.ToString();
            dataGridView2.Rows.Add("VK_F19"); //150
            DataGridViewComboBoxCell cmbs150 = new DataGridViewComboBoxCell();
            cmbs150.DataSource = itemsbuttons;
            dataGridView2[1, 150] = cmbs150;
            dataGridView2[1, 150].Value = shown ? "none" : s128.ToString();
            dataGridView2.Rows.Add("VK_F20"); //151
            DataGridViewComboBoxCell cmbs151 = new DataGridViewComboBoxCell();
            cmbs151.DataSource = itemsbuttons;
            dataGridView2[1, 151] = cmbs151;
            dataGridView2[1, 151].Value = shown ? "none" : s129.ToString();
            dataGridView2.Rows.Add("VK_F21"); //152
            DataGridViewComboBoxCell cmbs152 = new DataGridViewComboBoxCell();
            cmbs152.DataSource = itemsbuttons;
            dataGridView2[1, 152] = cmbs152;
            dataGridView2[1, 152].Value = shown ? "none" : s130.ToString();
            dataGridView2.Rows.Add("VK_F22"); //153
            DataGridViewComboBoxCell cmbs153 = new DataGridViewComboBoxCell();
            cmbs153.DataSource = itemsbuttons;
            dataGridView2[1, 153] = cmbs153;
            dataGridView2[1, 153].Value = shown ? "none" : s131.ToString();
            dataGridView2.Rows.Add("VK_F23"); //154
            DataGridViewComboBoxCell cmbs154 = new DataGridViewComboBoxCell();
            cmbs154.DataSource = itemsbuttons;
            dataGridView2[1, 154] = cmbs154;
            dataGridView2[1, 154].Value = shown ? "none" : s132.ToString();
            dataGridView2.Rows.Add("VK_F24"); //155
            DataGridViewComboBoxCell cmbs155 = new DataGridViewComboBoxCell();
            cmbs155.DataSource = itemsbuttons;
            dataGridView2[1, 155] = cmbs155;
            dataGridView2[1, 155].Value = shown ? "none" : s133.ToString();
            dataGridView2.Rows.Add("VK_NUMLOCK"); //156
            DataGridViewComboBoxCell cmbs156 = new DataGridViewComboBoxCell();
            cmbs156.DataSource = itemsbuttons;
            dataGridView2[1, 156] = cmbs156;
            dataGridView2[1, 156].Value = shown ? "none" : s134.ToString();
            dataGridView2.Rows.Add("VK_SCROLL"); //157
            DataGridViewComboBoxCell cmbs157 = new DataGridViewComboBoxCell();
            cmbs157.DataSource = itemsbuttons;
            dataGridView2[1, 157] = cmbs157;
            dataGridView2[1, 157].Value = shown ? "none" : s135.ToString();
            dataGridView2.Rows.Add("VK_LeftShift"); //158
            DataGridViewComboBoxCell cmbs158 = new DataGridViewComboBoxCell();
            cmbs158.DataSource = itemsbuttons;
            dataGridView2[1, 158] = cmbs158;
            dataGridView2[1, 158].Value = shown ? "LeftButtonSHOULDER_2" : s136.ToString();
            dataGridView2.Rows.Add("VK_RightShift"); //159
            DataGridViewComboBoxCell cmbs159 = new DataGridViewComboBoxCell();
            cmbs159.DataSource = itemsbuttons;
            dataGridView2[1, 159] = cmbs159;
            dataGridView2[1, 159].Value = shown ? "none" : s137.ToString();
            dataGridView2.Rows.Add("VK_LeftControl"); //160
            DataGridViewComboBoxCell cmbs160 = new DataGridViewComboBoxCell();
            cmbs160.DataSource = itemsbuttons;
            dataGridView2[1, 160] = cmbs160;
            dataGridView2[1, 160].Value = shown ? "none" : s138.ToString();
            dataGridView2.Rows.Add("VK_RightControl"); //161
            DataGridViewComboBoxCell cmbs161 = new DataGridViewComboBoxCell();
            cmbs161.DataSource = itemsbuttons;
            dataGridView2[1, 161] = cmbs161;
            dataGridView2[1, 161].Value = shown ? "none" : s139.ToString();
            dataGridView2.Rows.Add("VK_LMENU"); //162
            DataGridViewComboBoxCell cmbs162 = new DataGridViewComboBoxCell();
            cmbs162.DataSource = itemsbuttons;
            dataGridView2[1, 162] = cmbs162;
            dataGridView2[1, 162].Value = shown ? "none" : s140.ToString();
            dataGridView2.Rows.Add("VK_RMENU"); //163
            DataGridViewComboBoxCell cmbs163 = new DataGridViewComboBoxCell();
            cmbs163.DataSource = itemsbuttons;
            dataGridView2[1, 163] = cmbs163;
            dataGridView2[1, 163].Value = shown ? "none" : s141.ToString();
        }
        private void Form2_Shown(object sender, EventArgs e)
        {
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("tempsave"))
                {
                    filename = file.ReadLine();
                }
                if (filename != "")
                {
                    this.Text = "WiiJoyXSConfigHelper - " + filename;
                    openAs(filename);
                    openConf(false);
                }
            }
            catch
            {
                openConf(true);
            }
        }
        private void openAs(string filepath)
        {
            using (StreamReader createdfile = new StreamReader(filepath))
            {
                do
                {
                    createdfile.ReadLine();
                    hidtype = createdfile.ReadLine();
                    createdfile.ReadLine();
                    game = createdfile.ReadLine();
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
                    createdfile.ReadLine();
                    games = createdfile.ReadLine();
                    createdfile.ReadLine();
                    viewpower05xs = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    viewpower1xs = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    viewpower2xs = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    viewpower3xs = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    viewpower05ys = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    viewpower1ys = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    viewpower2ys = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    viewpower3ys = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    pollrate = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    dzxs = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    dzys = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    lowsensxs = Convert.ToDouble(createdfile.ReadLine());
                    createdfile.ReadLine();
                    lowsensys = Convert.ToDouble(createdfile.ReadLine());
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
                    centerys = Convert.ToDouble(createdfile.ReadLine());
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
                while (createdfile.EndOfStream);
                createdfile.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                filename = op.FileName;
                this.Text = "WiiJoyXSConfigHelper - " + filename;
                openAs(op.FileName);
                openConf(false);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (filename == "")
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "All Files(*.*)|*.*";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    filename = sf.FileName;
                    this.Text = "WiiJoyXSConfigHelper - " + filename;
                    saveAs(sf.FileName);
                }
            }
            else
                saveAs(filename);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "All Files(*.*)|*.*";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                filename = sf.FileName;
                this.Text = "WiiJoyXSConfigHelper - " + filename;
                saveAs(sf.FileName);
            }
        }
        private void saveAs(string filepath)
        {
            using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter(filepath))
            {
                createdfile.WriteLine("// hid type");
                if (radioButton1.Checked)
                    createdfile.WriteLine("controller");
                else
                    createdfile.WriteLine("K&M");
                createdfile.WriteLine("// " + dataGridView1[1, 0].Value);
                createdfile.WriteLine("X");
                createdfile.WriteLine("// viewpower05x");
                createdfile.WriteLine(dataGridView1[1, 1].Value);
                createdfile.WriteLine("// viewpower1x");
                createdfile.WriteLine(dataGridView1[1, 2].Value);
                createdfile.WriteLine("// viewpower2x");
                createdfile.WriteLine(dataGridView1[1, 3].Value);
                createdfile.WriteLine("// viewpower3x");
                createdfile.WriteLine(dataGridView1[1, 4].Value);
                createdfile.WriteLine("// viewpower05y");
                createdfile.WriteLine(dataGridView1[1, 5].Value);
                createdfile.WriteLine("// viewpower1y");
                createdfile.WriteLine(dataGridView1[1, 6].Value);
                createdfile.WriteLine("// viewpower2y");
                createdfile.WriteLine(dataGridView1[1, 7].Value);
                createdfile.WriteLine("// viewpower3y");
                createdfile.WriteLine(dataGridView1[1, 8].Value);
                createdfile.WriteLine("// dzx");
                createdfile.WriteLine(dataGridView1[1, 9].Value);
                createdfile.WriteLine("// dzy");
                createdfile.WriteLine(dataGridView1[1, 10].Value);
                createdfile.WriteLine("// lowsensx");
                createdfile.WriteLine(dataGridView1[1, 11].Value);
                createdfile.WriteLine("// lowsensy");
                createdfile.WriteLine(dataGridView1[1, 12].Value);
                createdfile.WriteLine("// centery");
                createdfile.WriteLine(dataGridView1[1, 13].Value);
                createdfile.WriteLine("// apressio");
                createdfile.WriteLine(dataGridView1[1, 14].Value);
                createdfile.WriteLine("// dzapressiox");
                createdfile.WriteLine(dataGridView1[1, 15].Value);
                createdfile.WriteLine("// dzapressioy");
                createdfile.WriteLine(dataGridView1[1, 16].Value);
                createdfile.WriteLine("// lowsensapressiox");
                createdfile.WriteLine(dataGridView1[1, 17].Value);
                createdfile.WriteLine("// lowsensapressioy");
                createdfile.WriteLine(dataGridView1[1, 18].Value);
                createdfile.WriteLine("// lowsenstime");
                createdfile.WriteLine(dataGridView1[1, 19].Value);
                createdfile.WriteLine("// lowsenstimeapressio");
                createdfile.WriteLine(dataGridView1[1, 20].Value);
                createdfile.WriteLine("// switch viewpower05x");
                createdfile.WriteLine(dataGridView1[1, 21].Value);
                createdfile.WriteLine("// switch viewpower1x");
                createdfile.WriteLine(dataGridView1[1, 22].Value);
                createdfile.WriteLine("// switch viewpower2x");
                createdfile.WriteLine(dataGridView1[1, 23].Value);
                createdfile.WriteLine("// switch viewpower3x");
                createdfile.WriteLine(dataGridView1[1, 24].Value);
                createdfile.WriteLine("// switch viewpower05y");
                createdfile.WriteLine(dataGridView1[1, 25].Value);
                createdfile.WriteLine("// switch viewpower1y");
                createdfile.WriteLine(dataGridView1[1, 26].Value);
                createdfile.WriteLine("// switch viewpower2y");
                createdfile.WriteLine(dataGridView1[1, 27].Value);
                createdfile.WriteLine("// switch viewpower3y");
                createdfile.WriteLine(dataGridView1[1, 28].Value);
                createdfile.WriteLine("// switch dzx");
                createdfile.WriteLine(dataGridView1[1, 29].Value);
                createdfile.WriteLine("// switch dzy");
                createdfile.WriteLine(dataGridView1[1, 30].Value);
                createdfile.WriteLine("// switch lowsensx");
                createdfile.WriteLine(dataGridView1[1, 31].Value);
                createdfile.WriteLine("// switch lowsensy");
                createdfile.WriteLine(dataGridView1[1, 32].Value);
                createdfile.WriteLine("// Down");
                createdfile.WriteLine(dataGridView1[1, 33].Value);
                createdfile.WriteLine("// Left");
                createdfile.WriteLine(dataGridView1[1, 34].Value);
                createdfile.WriteLine("// Right");
                createdfile.WriteLine(dataGridView1[1, 35].Value);
                createdfile.WriteLine("// Up");
                createdfile.WriteLine(dataGridView1[1, 36].Value);
                createdfile.WriteLine("// RightStick");
                createdfile.WriteLine(dataGridView1[1, 37].Value);
                createdfile.WriteLine("// LeftStick");
                createdfile.WriteLine(dataGridView1[1, 38].Value);
                createdfile.WriteLine("// A");
                createdfile.WriteLine(dataGridView1[1, 39].Value);
                createdfile.WriteLine("// Back");
                createdfile.WriteLine(dataGridView1[1, 40].Value);
                createdfile.WriteLine("// Start");
                createdfile.WriteLine(dataGridView1[1, 41].Value);
                createdfile.WriteLine("// X");
                createdfile.WriteLine(dataGridView1[1, 42].Value);
                createdfile.WriteLine("// RightBumper");
                createdfile.WriteLine(dataGridView1[1, 43].Value);
                createdfile.WriteLine("// LeftBumper");
                createdfile.WriteLine(dataGridView1[1, 44].Value);
                createdfile.WriteLine("// B");
                createdfile.WriteLine(dataGridView1[1, 45].Value);
                createdfile.WriteLine("// Y");
                createdfile.WriteLine(dataGridView1[1, 46].Value);
                createdfile.WriteLine("// RightTrigger");
                createdfile.WriteLine(dataGridView1[1, 47].Value);
                createdfile.WriteLine("// LeftTrigger");
                createdfile.WriteLine(dataGridView1[1, 48].Value);
                createdfile.WriteLine("// Switch Down");
                createdfile.WriteLine(dataGridView1[1, 49].Value);
                createdfile.WriteLine("// Switch Left");
                createdfile.WriteLine(dataGridView1[1, 50].Value);
                createdfile.WriteLine("// Switch Right");
                createdfile.WriteLine(dataGridView1[1, 51].Value);
                createdfile.WriteLine("// Switch Up");
                createdfile.WriteLine(dataGridView1[1, 52].Value);
                createdfile.WriteLine("// Switch RightStick");
                createdfile.WriteLine(dataGridView1[1, 53].Value);
                createdfile.WriteLine("// Switch LeftStick");
                createdfile.WriteLine(dataGridView1[1, 54].Value);
                createdfile.WriteLine("// Switch A");
                createdfile.WriteLine(dataGridView1[1, 55].Value);
                createdfile.WriteLine("// Switch Back");
                createdfile.WriteLine(dataGridView1[1, 56].Value);
                createdfile.WriteLine("// Switch Start");
                createdfile.WriteLine(dataGridView1[1, 57].Value);
                createdfile.WriteLine("// Switch X");
                createdfile.WriteLine(dataGridView1[1, 58].Value);
                createdfile.WriteLine("// Switch RightBumper");
                createdfile.WriteLine(dataGridView1[1, 59].Value);
                createdfile.WriteLine("// Switch LeftBumper");
                createdfile.WriteLine(dataGridView1[1, 60].Value);
                createdfile.WriteLine("// Switch B");
                createdfile.WriteLine(dataGridView1[1, 61].Value);
                createdfile.WriteLine("// Switch Y");
                createdfile.WriteLine(dataGridView1[1, 62].Value);
                createdfile.WriteLine("// Switch RightTrigger");
                createdfile.WriteLine(dataGridView1[1, 63].Value);
                createdfile.WriteLine("// Switch LeftTrigger");
                createdfile.WriteLine(dataGridView1[1, 64].Value);
                createdfile.WriteLine("// left stick right");
                createdfile.WriteLine(dataGridView1[1, 65].Value);
                createdfile.WriteLine("// left stick left");
                createdfile.WriteLine(dataGridView1[1, 66].Value);
                createdfile.WriteLine("// left stick up");
                createdfile.WriteLine(dataGridView1[1, 67].Value);
                createdfile.WriteLine("// left stick down");
                createdfile.WriteLine(dataGridView1[1, 68].Value);
                createdfile.WriteLine("// right stick right");
                createdfile.WriteLine(dataGridView1[1, 69].Value);
                createdfile.WriteLine("// right stick left");
                createdfile.WriteLine(dataGridView1[1, 70].Value);
                createdfile.WriteLine("// right stick up");
                createdfile.WriteLine(dataGridView1[1, 71].Value);
                createdfile.WriteLine("// right stick down");
                createdfile.WriteLine(dataGridView1[1, 72].Value);
                createdfile.WriteLine("// left stick x");
                createdfile.WriteLine(dataGridView1[1, 73].Value);
                createdfile.WriteLine("// left stick y");
                createdfile.WriteLine(dataGridView1[1, 74].Value);
                createdfile.WriteLine("// right stick x");
                createdfile.WriteLine(dataGridView1[1, 75].Value);
                createdfile.WriteLine("// right stick y");
                createdfile.WriteLine(dataGridView1[1, 76].Value);
                createdfile.WriteLine("// switch left stick right");
                createdfile.WriteLine(dataGridView1[1, 77].Value);
                createdfile.WriteLine("// switch left stick left");
                createdfile.WriteLine(dataGridView1[1, 78].Value);
                createdfile.WriteLine("// switch left stick up");
                createdfile.WriteLine(dataGridView1[1, 79].Value);
                createdfile.WriteLine("// switch left stick down");
                createdfile.WriteLine(dataGridView1[1, 80].Value);
                createdfile.WriteLine("// switch right stick right");
                createdfile.WriteLine(dataGridView1[1, 81].Value);
                createdfile.WriteLine("// switch right stick left");
                createdfile.WriteLine(dataGridView1[1, 82].Value);
                createdfile.WriteLine("// switch right stick up");
                createdfile.WriteLine(dataGridView1[1, 83].Value);
                createdfile.WriteLine("// switch right stick down");
                createdfile.WriteLine(dataGridView1[1, 84].Value);
                createdfile.WriteLine("// switch left stick x");
                createdfile.WriteLine(dataGridView1[1, 85].Value);
                createdfile.WriteLine("// switch left stick y");
                createdfile.WriteLine(dataGridView1[1, 86].Value);
                createdfile.WriteLine("// switch right stick x");
                createdfile.WriteLine(dataGridView1[1, 87].Value);
                createdfile.WriteLine("// switch right stick y");
                createdfile.WriteLine(dataGridView1[1, 88].Value);
                createdfile.WriteLine("// " + dataGridView2[1, 0].Value);
                createdfile.WriteLine("S");
                createdfile.WriteLine("// viewpower05x");
                createdfile.WriteLine(dataGridView2[1, 1].Value);
                createdfile.WriteLine("// viewpower1x");
                createdfile.WriteLine(dataGridView2[1, 2].Value);
                createdfile.WriteLine("// viewpower2x");
                createdfile.WriteLine(dataGridView2[1, 3].Value);
                createdfile.WriteLine("// viewpower3x");
                createdfile.WriteLine(dataGridView2[1, 4].Value);
                createdfile.WriteLine("// viewpower05y");
                createdfile.WriteLine(dataGridView2[1, 5].Value);
                createdfile.WriteLine("// viewpower1y");
                createdfile.WriteLine(dataGridView2[1, 6].Value);
                createdfile.WriteLine("// viewpower2y");
                createdfile.WriteLine(dataGridView2[1, 7].Value);
                createdfile.WriteLine("// viewpower3y");
                createdfile.WriteLine(dataGridView2[1, 8].Value);
                createdfile.WriteLine("// pollrate");
                createdfile.WriteLine(dataGridView2[1, 9].Value);
                createdfile.WriteLine("// dzx");
                createdfile.WriteLine(dataGridView2[1, 10].Value);
                createdfile.WriteLine("// dzy");
                createdfile.WriteLine(dataGridView2[1, 11].Value);
                createdfile.WriteLine("// lowsensx");
                createdfile.WriteLine(dataGridView2[1, 12].Value);
                createdfile.WriteLine("// lowsensy");
                createdfile.WriteLine(dataGridView2[1, 13].Value);
                createdfile.WriteLine("// lowsensbrinkaex");
                createdfile.WriteLine(dataGridView2[1, 14].Value);
                createdfile.WriteLine("// lowsensbrinkaey");
                createdfile.WriteLine(dataGridView2[1, 15].Value);
                createdfile.WriteLine("// brink");
                createdfile.WriteLine(dataGridView2[1, 16].Value);
                createdfile.WriteLine("// mw3");
                createdfile.WriteLine(dataGridView2[1, 17].Value);
                createdfile.WriteLine("// mw3ae");
                createdfile.WriteLine(dataGridView2[1, 18].Value);
                createdfile.WriteLine("// brinkae");
                createdfile.WriteLine(dataGridView2[1, 19].Value);
                createdfile.WriteLine("// desktop");
                createdfile.WriteLine(dataGridView2[1, 20].Value);
                createdfile.WriteLine("// centery");
                createdfile.WriteLine(dataGridView2[1, 21].Value);
                createdfile.WriteLine("// driver type");
                createdfile.WriteLine(dataGridView2[1, 22].Value);
                createdfile.WriteLine("// SendMouseEventButtonLeft");
                createdfile.WriteLine(dataGridView2[1, 23].Value);
                createdfile.WriteLine("// SendMouseEventButtonRight");
                createdfile.WriteLine(dataGridView2[1, 24].Value);
                createdfile.WriteLine("// SendMouseEventButtonMiddle");
                createdfile.WriteLine(dataGridView2[1, 25].Value);
                createdfile.WriteLine("// SendMouseEventButtonWheelUp");
                createdfile.WriteLine(dataGridView2[1, 26].Value);
                createdfile.WriteLine("// SendMouseEventButtonWheelDown");
                createdfile.WriteLine(dataGridView2[1, 27].Value);
                createdfile.WriteLine("// VK_LEFT");
                createdfile.WriteLine(dataGridView2[1, 28].Value);
                createdfile.WriteLine("// VK_RIGHT");
                createdfile.WriteLine(dataGridView2[1, 29].Value);
                createdfile.WriteLine("// VK_UP");
                createdfile.WriteLine(dataGridView2[1, 30].Value);
                createdfile.WriteLine("// VK_DOWN");
                createdfile.WriteLine(dataGridView2[1, 31].Value);
                createdfile.WriteLine("// VK_LBUTTON");
                createdfile.WriteLine(dataGridView2[1, 32].Value);
                createdfile.WriteLine("// VK_RBUTTON");
                createdfile.WriteLine(dataGridView2[1, 33].Value);
                createdfile.WriteLine("// VK_CANCEL");
                createdfile.WriteLine(dataGridView2[1, 34].Value);
                createdfile.WriteLine("// VK_MBUTTON");
                createdfile.WriteLine(dataGridView2[1, 35].Value);
                createdfile.WriteLine("// VK_XBUTTON1");
                createdfile.WriteLine(dataGridView2[1, 36].Value);
                createdfile.WriteLine("// VK_XBUTTON2");
                createdfile.WriteLine(dataGridView2[1, 37].Value);
                createdfile.WriteLine("// VK_BACK");
                createdfile.WriteLine(dataGridView2[1, 38].Value);
                createdfile.WriteLine("// VK_Tab");
                createdfile.WriteLine(dataGridView2[1, 39].Value);
                createdfile.WriteLine("// VK_CLEAR");
                createdfile.WriteLine(dataGridView2[1, 40].Value);
                createdfile.WriteLine("// VK_Return");
                createdfile.WriteLine(dataGridView2[1, 41].Value);
                createdfile.WriteLine("// VK_SHIFT");
                createdfile.WriteLine(dataGridView2[1, 42].Value);
                createdfile.WriteLine("// VK_CONTROL");
                createdfile.WriteLine(dataGridView2[1, 43].Value);
                createdfile.WriteLine("// VK_MENU");
                createdfile.WriteLine(dataGridView2[1, 44].Value);
                createdfile.WriteLine("// VK_PAUSE");
                createdfile.WriteLine(dataGridView2[1, 45].Value);
                createdfile.WriteLine("// VK_CAPITAL");
                createdfile.WriteLine(dataGridView2[1, 46].Value);
                createdfile.WriteLine("// VK_KANA");
                createdfile.WriteLine(dataGridView2[1, 47].Value);
                createdfile.WriteLine("// VK_HANGEUL");
                createdfile.WriteLine(dataGridView2[1, 48].Value);
                createdfile.WriteLine("// VK_HANGUL");
                createdfile.WriteLine(dataGridView2[1, 49].Value);
                createdfile.WriteLine("// VK_JUNJA");
                createdfile.WriteLine(dataGridView2[1, 50].Value);
                createdfile.WriteLine("// VK_FINAL");
                createdfile.WriteLine(dataGridView2[1, 51].Value);
                createdfile.WriteLine("// VK_HANJA");
                createdfile.WriteLine(dataGridView2[1, 52].Value);
                createdfile.WriteLine("// VK_KANJI");
                createdfile.WriteLine(dataGridView2[1, 53].Value);
                createdfile.WriteLine("// VK_Escape");
                createdfile.WriteLine(dataGridView2[1, 54].Value);
                createdfile.WriteLine("// VK_CONVERT");
                createdfile.WriteLine(dataGridView2[1, 55].Value);
                createdfile.WriteLine("// VK_NONCONVERT");
                createdfile.WriteLine(dataGridView2[1, 56].Value);
                createdfile.WriteLine("// VK_ACCEPT");
                createdfile.WriteLine(dataGridView2[1, 57].Value);
                createdfile.WriteLine("// VK_MODECHANGE");
                createdfile.WriteLine(dataGridView2[1, 58].Value);
                createdfile.WriteLine("// VK_Space");
                createdfile.WriteLine(dataGridView2[1, 59].Value);
                createdfile.WriteLine("// VK_PRIOR");
                createdfile.WriteLine(dataGridView2[1, 60].Value);
                createdfile.WriteLine("// VK_NEXT");
                createdfile.WriteLine(dataGridView2[1, 61].Value);
                createdfile.WriteLine("// VK_END");
                createdfile.WriteLine(dataGridView2[1, 62].Value);
                createdfile.WriteLine("// VK_HOME");
                createdfile.WriteLine(dataGridView2[1, 63].Value);
                createdfile.WriteLine("// VK_LEFT");
                createdfile.WriteLine(dataGridView2[1, 64].Value);
                createdfile.WriteLine("// VK_UP");
                createdfile.WriteLine(dataGridView2[1, 65].Value);
                createdfile.WriteLine("// VK_RIGHT");
                createdfile.WriteLine(dataGridView2[1, 66].Value);
                createdfile.WriteLine("// VK_DOWN");
                createdfile.WriteLine(dataGridView2[1, 67].Value);
                createdfile.WriteLine("// VK_SELECT");
                createdfile.WriteLine(dataGridView2[1, 68].Value);
                createdfile.WriteLine("// VK_PRINT");
                createdfile.WriteLine(dataGridView2[1, 69].Value);
                createdfile.WriteLine("// VK_EXECUTE");
                createdfile.WriteLine(dataGridView2[1, 70].Value);
                createdfile.WriteLine("// VK_SNAPSHOT");
                createdfile.WriteLine(dataGridView2[1, 71].Value);
                createdfile.WriteLine("// VK_INSERT");
                createdfile.WriteLine(dataGridView2[1, 72].Value);
                createdfile.WriteLine("// VK_DELETE");
                createdfile.WriteLine(dataGridView2[1, 73].Value);
                createdfile.WriteLine("// VK_HELP");
                createdfile.WriteLine(dataGridView2[1, 74].Value);
                createdfile.WriteLine("// VK_APOSTROPHE");
                createdfile.WriteLine(dataGridView2[1, 75].Value);
                createdfile.WriteLine("// VK_0");
                createdfile.WriteLine(dataGridView2[1, 76].Value);
                createdfile.WriteLine("// VK_1");
                createdfile.WriteLine(dataGridView2[1, 77].Value);
                createdfile.WriteLine("// VK_2");
                createdfile.WriteLine(dataGridView2[1, 78].Value);
                createdfile.WriteLine("// VK_3");
                createdfile.WriteLine(dataGridView2[1, 79].Value);
                createdfile.WriteLine("// VK_4");
                createdfile.WriteLine(dataGridView2[1, 80].Value);
                createdfile.WriteLine("// VK_5");
                createdfile.WriteLine(dataGridView2[1, 81].Value);
                createdfile.WriteLine("// VK_6");
                createdfile.WriteLine(dataGridView2[1, 82].Value);
                createdfile.WriteLine("// VK_7");
                createdfile.WriteLine(dataGridView2[1, 83].Value);
                createdfile.WriteLine("// VK_8");
                createdfile.WriteLine(dataGridView2[1, 84].Value);
                createdfile.WriteLine("// VK_9");
                createdfile.WriteLine(dataGridView2[1, 85].Value);
                createdfile.WriteLine("// VK_A");
                createdfile.WriteLine(dataGridView2[1, 86].Value);
                createdfile.WriteLine("// VK_B");
                createdfile.WriteLine(dataGridView2[1, 87].Value);
                createdfile.WriteLine("// VK_C");
                createdfile.WriteLine(dataGridView2[1, 88].Value);
                createdfile.WriteLine("// VK_D");
                createdfile.WriteLine(dataGridView2[1, 89].Value);
                createdfile.WriteLine("// VK_E");
                createdfile.WriteLine(dataGridView2[1, 90].Value);
                createdfile.WriteLine("// VK_F");
                createdfile.WriteLine(dataGridView2[1, 91].Value);
                createdfile.WriteLine("// VK_G");
                createdfile.WriteLine(dataGridView2[1, 92].Value);
                createdfile.WriteLine("// VK_H");
                createdfile.WriteLine(dataGridView2[1, 93].Value);
                createdfile.WriteLine("// VK_I");
                createdfile.WriteLine(dataGridView2[1, 94].Value);
                createdfile.WriteLine("// VK_J");
                createdfile.WriteLine(dataGridView2[1, 95].Value);
                createdfile.WriteLine("// VK_K");
                createdfile.WriteLine(dataGridView2[1, 96].Value);
                createdfile.WriteLine("// VK_L");
                createdfile.WriteLine(dataGridView2[1, 97].Value);
                createdfile.WriteLine("// VK_M");
                createdfile.WriteLine(dataGridView2[1, 98].Value);
                createdfile.WriteLine("// VK_N");
                createdfile.WriteLine(dataGridView2[1, 99].Value);
                createdfile.WriteLine("// VK_O");
                createdfile.WriteLine(dataGridView2[1, 100].Value);
                createdfile.WriteLine("// VK_P");
                createdfile.WriteLine(dataGridView2[1, 101].Value);
                createdfile.WriteLine("// VK_Q");
                createdfile.WriteLine(dataGridView2[1, 102].Value);
                createdfile.WriteLine("// VK_R");
                createdfile.WriteLine(dataGridView2[1, 103].Value);
                createdfile.WriteLine("// VK_S");
                createdfile.WriteLine(dataGridView2[1, 104].Value);
                createdfile.WriteLine("// VK_T");
                createdfile.WriteLine(dataGridView2[1, 105].Value);
                createdfile.WriteLine("// VK_U");
                createdfile.WriteLine(dataGridView2[1, 106].Value);
                createdfile.WriteLine("// VK_V");
                createdfile.WriteLine(dataGridView2[1, 107].Value);
                createdfile.WriteLine("// VK_W");
                createdfile.WriteLine(dataGridView2[1, 108].Value);
                createdfile.WriteLine("// VK_X");
                createdfile.WriteLine(dataGridView2[1, 109].Value);
                createdfile.WriteLine("// VK_Y");
                createdfile.WriteLine(dataGridView2[1, 110].Value);
                createdfile.WriteLine("// VK_Z");
                createdfile.WriteLine(dataGridView2[1, 111].Value);
                createdfile.WriteLine("// VK_LWIN");
                createdfile.WriteLine(dataGridView2[1, 112].Value);
                createdfile.WriteLine("// VK_RWIN");
                createdfile.WriteLine(dataGridView2[1, 113].Value);
                createdfile.WriteLine("// VK_APPS");
                createdfile.WriteLine(dataGridView2[1, 114].Value);
                createdfile.WriteLine("// VK_SLEEP");
                createdfile.WriteLine(dataGridView2[1, 115].Value);
                createdfile.WriteLine("// VK_NUMPAD0");
                createdfile.WriteLine(dataGridView2[1, 116].Value);
                createdfile.WriteLine("// VK_NUMPAD1");
                createdfile.WriteLine(dataGridView2[1, 117].Value);
                createdfile.WriteLine("// VK_NUMPAD2");
                createdfile.WriteLine(dataGridView2[1, 118].Value);
                createdfile.WriteLine("// VK_NUMPAD3");
                createdfile.WriteLine(dataGridView2[1, 119].Value);
                createdfile.WriteLine("// VK_NUMPAD4");
                createdfile.WriteLine(dataGridView2[1, 120].Value);
                createdfile.WriteLine("// VK_NUMPAD5");
                createdfile.WriteLine(dataGridView2[1, 121].Value);
                createdfile.WriteLine("// VK_NUMPAD6");
                createdfile.WriteLine(dataGridView2[1, 122].Value);
                createdfile.WriteLine("// VK_NUMPAD7");
                createdfile.WriteLine(dataGridView2[1, 123].Value);
                createdfile.WriteLine("// VK_NUMPAD8");
                createdfile.WriteLine(dataGridView2[1, 124].Value);
                createdfile.WriteLine("// VK_NUMPAD9");
                createdfile.WriteLine(dataGridView2[1, 125].Value);
                createdfile.WriteLine("// VK_MULTIPLY");
                createdfile.WriteLine(dataGridView2[1, 126].Value);
                createdfile.WriteLine("// VK_ADD");
                createdfile.WriteLine(dataGridView2[1, 127].Value);
                createdfile.WriteLine("// VK_SEPARATOR");
                createdfile.WriteLine(dataGridView2[1, 128].Value);
                createdfile.WriteLine("// VK_SUBTRACT");
                createdfile.WriteLine(dataGridView2[1, 129].Value);
                createdfile.WriteLine("// VK_DECIMAL");
                createdfile.WriteLine(dataGridView2[1, 130].Value);
                createdfile.WriteLine("// VK_DIVIDE");
                createdfile.WriteLine(dataGridView2[1, 131].Value);
                createdfile.WriteLine("// VK_F1");
                createdfile.WriteLine(dataGridView2[1, 132].Value);
                createdfile.WriteLine("// VK_F2");
                createdfile.WriteLine(dataGridView2[1, 133].Value);
                createdfile.WriteLine("// VK_F3");
                createdfile.WriteLine(dataGridView2[1, 134].Value);
                createdfile.WriteLine("// VK_F4");
                createdfile.WriteLine(dataGridView2[1, 135].Value);
                createdfile.WriteLine("// VK_F5");
                createdfile.WriteLine(dataGridView2[1, 136].Value);
                createdfile.WriteLine("// VK_F6");
                createdfile.WriteLine(dataGridView2[1, 137].Value);
                createdfile.WriteLine("// VK_F7");
                createdfile.WriteLine(dataGridView2[1, 138].Value);
                createdfile.WriteLine("// VK_F8");
                createdfile.WriteLine(dataGridView2[1, 139].Value);
                createdfile.WriteLine("// VK_F9");
                createdfile.WriteLine(dataGridView2[1, 140].Value);
                createdfile.WriteLine("// VK_F10");
                createdfile.WriteLine(dataGridView2[1, 141].Value);
                createdfile.WriteLine("// VK_F11");
                createdfile.WriteLine(dataGridView2[1, 142].Value);
                createdfile.WriteLine("// VK_F12");
                createdfile.WriteLine(dataGridView2[1, 143].Value);
                createdfile.WriteLine("// VK_F13");
                createdfile.WriteLine(dataGridView2[1, 144].Value);
                createdfile.WriteLine("// VK_F14");
                createdfile.WriteLine(dataGridView2[1, 145].Value);
                createdfile.WriteLine("// VK_F15");
                createdfile.WriteLine(dataGridView2[1, 146].Value);
                createdfile.WriteLine("// VK_F16");
                createdfile.WriteLine(dataGridView2[1, 147].Value);
                createdfile.WriteLine("// VK_F17");
                createdfile.WriteLine(dataGridView2[1, 148].Value);
                createdfile.WriteLine("// VK_F18");
                createdfile.WriteLine(dataGridView2[1, 149].Value);
                createdfile.WriteLine("// VK_F19");
                createdfile.WriteLine(dataGridView2[1, 150].Value);
                createdfile.WriteLine("// VK_F20");
                createdfile.WriteLine(dataGridView2[1, 151].Value);
                createdfile.WriteLine("// VK_F21");
                createdfile.WriteLine(dataGridView2[1, 152].Value);
                createdfile.WriteLine("// VK_F22");
                createdfile.WriteLine(dataGridView2[1, 153].Value);
                createdfile.WriteLine("// VK_F23");
                createdfile.WriteLine(dataGridView2[1, 154].Value);
                createdfile.WriteLine("// VK_F24");
                createdfile.WriteLine(dataGridView2[1, 155].Value);
                createdfile.WriteLine("// VK_NUMLOCK");
                createdfile.WriteLine(dataGridView2[1, 156].Value);
                createdfile.WriteLine("// VK_SCROLL");
                createdfile.WriteLine(dataGridView2[1, 157].Value);
                createdfile.WriteLine("// VK_LeftShift");
                createdfile.WriteLine(dataGridView2[1, 158].Value);
                createdfile.WriteLine("// VK_RightShift");
                createdfile.WriteLine(dataGridView2[1, 159].Value);
                createdfile.WriteLine("// VK_LeftControl");
                createdfile.WriteLine(dataGridView2[1, 160].Value);
                createdfile.WriteLine("// VK_RightControl");
                createdfile.WriteLine(dataGridView2[1, 161].Value);
                createdfile.WriteLine("// VK_LMENU");
                createdfile.WriteLine(dataGridView2[1, 162].Value);
                createdfile.WriteLine("// VK_RMENU");
                createdfile.WriteLine(dataGridView2[1, 163].Value);
                createdfile.Close();
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                radioButton2.Checked = false;
            else
                radioButton2.Checked = true;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                radioButton1.Checked = false;
            else
                radioButton1.Checked = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            const string message = "• Start the program with Wiimote and Joycons couch and sticks free for calibration.\n\r• Press 1 and 2 buttons for pairing Wiimote, press button on the edge of Joycons during 2 seconds for pairing it.\n\r• Let 1 meter between sensor barre and Wiimote.\n\r• Free sensor barre all around Wiimote.\n\r• Adapt the mouse sensitivities and DPI in game options.\n\r• Prefer a PC with bluetooth integrate because it's more powerfull, and so normally there isn't any weird latency.\n\r• Press Win+R and type joy.cpl, and then press enter to test controller.\n\r• The form scripts use setting files needing to set decimal instead of comma for number separator in regional advanced options under control panel.\n\r• When you can choose to lower deadzone of gamepad from game options, set it to lowest values, it's better for vertical/horizontal flawless view control, but not for walk/run control.\n\r• You can switch between two configurations with Wiimote Home and Down Buttons, but switch is enable only when Wiimote is connected alone or with one Joycon, and with driver type as controller only.\n\r• To enable configuration change and input sendings, press Joycon Right Button DPAD_DOWN and Joycon Right Button HOME, or Joycon Left Button DPAD_DOWN and Joycon Left Button CAPTURE, or Wiimote Button Home and  Wiimote Button Two.\n\r• mWSButtonStateLR for Wiimote Left or Right.\n\r• mWSButtonStateMU for Wiimote Minus or Up.\n\r• mWSButtonStatePU for Wiimote Plus or Up.\n\r• mWSButtonStateHFront for Wiimote Home or to Front.\n\r• mWSLeftButtonTC for Wiimote Two or Joycon Left Capture.\n\r• LeftButtonSMA for Joycon Left SL, SR, Minus or Acceleration.\n\r• RightButtonSPA for Joycon Right SL, SR, Plus or Acceleration.\n\r• To enable change, config must be save in WiiJoyXS.txt file.";
            const string caption = "WiiJoyXS Legend";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
