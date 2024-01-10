using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using AutocompleteMenuNS;
namespace WiiJoyScripter
{
    public partial class Form1 : Form
    {
        string openFilePath = null;
        bool justSaved = true;
        bool justSavedbefore = true;
        DialogResult result;
        string code, addedcode, finalcode;
        ContextMenu contextMenu = new ContextMenu();
        MenuItem menuItem;
        public string wordsearch;
        public string wordreplace;
        public int pos = -1;
        public string filename = "";
        string[] keywords = { "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while", "add", "alias", "ascending", "descending", "dynamic", "from", "get", "global", "group", "into", "join", "let", "orderby", "partial", "remove", "select", "set", "value", "var", "where", "yield",
            "void",
            "bool",
            "int",
            "float",
            "double",
            "Double",
            "List",
            "[",
            "]",
            "if",
            "input",
            "/* */",
            "this",
            "input",
            "mWSNunchuckStateRawJoystickX", 
            "mWSNunchuckStateRawJoystickY", 
            "mWSNunchuckStateRawValuesX",
            "mWSNunchuckStateRawValuesY", 
            "mWSNunchuckStateRawValuesZ", 
            "mWSNunchuckStateC", 
            "mWSNunchuckStateZ",
            "mWSButtonStateIRX",
            "mWSButtonStateIRY",
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
            "mWSRawValuesX",
            "mWSRawValuesY",
            "mWSRawValuesZ",
            "EulerAnglesX",
            "EulerAnglesY",
            "EulerAnglesZ",
            "DirectAnglesX",
            "DirectAnglesY",
            "DirectAnglesZ",
            "camx",
            "camy",
            "EulerAnglesLeftX",
            "EulerAnglesLeftY",
            "EulerAnglesLeftZ",
            "DirectAnglesLeftX",
            "DirectAnglesLeftY",
            "DirectAnglesLeftZ",
            "EulerAnglesRightX",
            "EulerAnglesRightY",
            "EulerAnglesRightZ",
            "DirectAnglesRightX",
            "DirectAnglesRightY",
            "DirectAnglesRightZ",
            "LeftButtonSHOULDER_1",
            "LeftButtonMINUS",
            "LeftButtonCAPTURE",
            "LeftButtonDPAD_UP",
            "LeftButtonDPAD_LEFT",
            "LeftButtonDPAD_DOWN",
            "LeftButtonDPAD_RIGHT",
            "LeftButtonSTICK",
            "RightButtonDPAD_DOWN",
            "LeftButtonSL",
            "LeftButtonSR",
            "GetStickLeftX",
            "GetStickLeftY",
            "RightButtonPLUS",
            "RightButtonDPAD_RIGHT",
            "RightButtonHOME",
            "RightButtonSHOULDER_1",
            "RightButtonDPAD_LEFT",
            "RightButtonDPAD_UP",
            "RightButtonSTICK",
            "RightButtonSL",
            "RightButtonSR",
            "RightButtonSHOULDER_2",
            "LeftButtonSHOULDER_2",
            "GetStickRightX",
            "GetStickRightY",
            "GetAccelX",
            "GetAccelY",
            "GetAccelZ",
            "GetAccelRightX",
            "GetAccelRightY",
            "GetAccelRightZ",
            "GetAccelLeftX",
            "GetAccelLeftY",
            "GetAccelLeftZ",
            "watchM",
            "MouseHookX",
            "MouseHookY",
            "MouseHookWheel",
            "MouseHookLeftButton",
            "MouseHookRightButton",
            "MouseHookDoubleClick",
            "MouseHookMiddleButton",
            "GetAsyncKeyState()",
            "System.Windows.Forms.Keys",
            "GetCursorPos()",
            "out x",
            "out y",
            "_Valuechanged[]",
            "valchanged()",
            "System.Collections.Generic.List<double>",
            "foreach",
            "return ",
            "Keys",
            "(Keys)",
            "System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width",
            "System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height",
            "System.Windows.Forms.Cursor.Position",
            "System.Drawing.Point",
            "SetPhysicalCursorPos",
            "SetCaretPos",
            "SetCursorPos",
            "(int)",
            "MouseMW3",
            "MouseBrink",
            "LeftClick()",
            "LeftClickF()",
            "RightClick()",
            "RightClickF()",
            "MiddleClick()",
            "MiddleClickF()",
            "WheelUpF()",
            "WheelDownF()",
            "SimulateKeyDown",
            "SimulateKeyUp",
            "SimulateKeyDownArrows",
            "SimulateKeyUpArrows",
            "MoveMouseTo",
            "MoveMouseBy",
            "SendMouseEventButtonLeft()",
            "SendMouseEventButtonLeftF()",
            "SendMouseEventButtonRight()",
            "SendMouseEventButtonRightF()",
            "SendMouseEventButtonMiddle()",
            "SendMouseEventButtonMiddleF()",
            "SendMouseEventButtonWheelUp()",
            "SendMouseEventButtonWheelDown()",
            "SendKey",
            "SendKeyF",
            "SendKeyArrows",
            "SendKeyArrowsF",
            "VK_LBUTTON", "S_LBUTTON",
            "VK_RBUTTON", "S_RBUTTON",
            "VK_CANCEL", "S_CANCEL",
            "VK_MBUTTON", "S_MBUTTON",
            "VK_XBUTTON1", "S_XBUTTON1",
            "VK_XBUTTON2", "S_XBUTTON2",
            "VK_BACK", "S_BACK",
            "VK_Tab", "S_Tab",
            "VK_CLEAR", "S_CLEAR",
            "VK_Return", "S_Return",
            "VK_SHIFT", "S_SHIFT",
            "VK_CONTROL", "S_CONTROL",
            "VK_MENU", "S_MENU",
            "VK_PAUSE", "S_PAUSE",
            "VK_CAPITAL", "S_CAPITAL",
            "VK_KANA", "S_KANA",
            "VK_HANGEUL", "S_HANGEUL",
            "VK_HANGUL", "S_HANGUL",
            "VK_JUNJA", "S_JUNJA",
            "VK_FINAL", "S_FINAL",
            "VK_HANJA", "S_HANJA",
            "VK_KANJI", "S_KANJI",
            "VK_Escape", "S_Escape",
            "VK_CONVERT", "S_CONVERT",
            "VK_NONCONVERT", "S_NONCONVERT",
            "VK_ACCEPT", "S_ACCEPT",
            "VK_MODECHANGE", "S_MODECHANGE",
            "VK_Space", "S_Space",
            "VK_PRIOR", "S_PRIOR",
            "VK_NEXT", "S_NEXT",
            "VK_END", "S_END",
            "VK_HOME", "S_HOME",
            "VK_LEFT", "S_LEFT",
            "VK_UP", "S_UP",
            "VK_RIGHT", "S_RIGHT",
            "VK_DOWN", "S_DOWN",
            "VK_SELECT", "S_SELECT",
            "VK_PRINT", "S_PRINT",
            "VK_EXECUTE", "S_EXECUTE",
            "VK_SNAPSHOT", "S_SNAPSHOT",
            "VK_INSERT", "S_INSERT",
            "VK_DELETE", "S_DELETE",
            "VK_HELP", "S_HELP",
            "VK_APOSTROPHE", "S_APOSTROPHE",
            "VK_0", "S_0",
            "VK_1", "S_1",
            "VK_2", "S_2",
            "VK_3", "S_3",
            "VK_4", "S_4",
            "VK_5", "S_5",
            "VK_6", "S_6",
            "VK_7", "S_7",
            "VK_8", "S_8",
            "VK_9", "S_9",
            "VK_A", "S_A",
            "VK_B", "S_B",
            "VK_C", "S_C",
            "VK_D", "S_D",
            "VK_E", "S_E",
            "VK_F", "S_F",
            "VK_G", "S_G",
            "VK_H", "S_H",
            "VK_I", "S_I",
            "VK_J", "S_J",
            "VK_K", "S_K",
            "VK_L", "S_L",
            "VK_M", "S_M",
            "VK_N", "S_N",
            "VK_O", "S_O",
            "VK_P", "S_P",
            "VK_Q", "S_Q",
            "VK_R", "S_R",
            "VK_S", "S_S",
            "VK_T", "S_T",
            "VK_U", "S_U",
            "VK_V", "S_V",
            "VK_W", "S_W",
            "VK_X", "S_X",
            "VK_Y", "S_Y",
            "VK_Z", "S_Z",
            "VK_LWIN", "S_LWIN",
            "VK_RWIN", "S_RWIN",
            "VK_APPS", "S_APPS",
            "VK_SLEEP", "S_SLEEP",
            "VK_NUMPAD0", "S_NUMPAD0",
            "VK_NUMPAD1", "S_NUMPAD1",
            "VK_NUMPAD2", "S_NUMPAD2",
            "VK_NUMPAD3", "S_NUMPAD3",
            "VK_NUMPAD4", "S_NUMPAD4",
            "VK_NUMPAD5", "S_NUMPAD5",
            "VK_NUMPAD6", "S_NUMPAD6",
            "VK_NUMPAD7", "S_NUMPAD7",
            "VK_NUMPAD8", "S_NUMPAD8",
            "VK_NUMPAD9", "S_NUMPAD9",
            "VK_MULTIPLY", "S_MULTIPLY",
            "VK_ADD", "S_ADD",
            "VK_SEPARATOR", "S_SEPARATOR",
            "VK_SUBTRACT", "S_SUBTRACT",
            "VK_DECIMAL", "S_DECIMAL",
            "VK_DIVIDE", "S_DIVIDE",
            "VK_F1", "S_F1",
            "VK_F2", "S_F2",
            "VK_F3", "S_F3",
            "VK_F4", "S_F4",
            "VK_F5", "S_F5",
            "VK_F6", "S_F6",
            "VK_F7", "S_F7",
            "VK_F8", "S_F8",
            "VK_F9", "S_F9",
            "VK_F10", "S_F10",
            "VK_F11", "S_F11",
            "VK_F12", "S_F12",
            "VK_F13", "S_F13",
            "VK_F14", "S_F14",
            "VK_F15", "S_F15",
            "VK_F16", "S_F16",
            "VK_F17", "S_F17",
            "VK_F18", "S_F18",
            "VK_F19", "S_F19",
            "VK_F20", "S_F20",
            "VK_F21", "S_F21",
            "VK_F22", "S_F22",
            "VK_F23", "S_F23",
            "VK_F24", "S_F24",
            "VK_NUMLOCK", "S_NUMLOCK",
            "VK_SCROLL", "S_SCROLL",
            "VK_LeftShift", "S_LeftShift",
            "VK_RightShift", "S_RightShift",
            "VK_LeftControl", "S_LeftControl",
            "VK_RightControl", "S_RightControl",
            "VK_LMENU", "S_LMENU",
            "VK_RMENU", "S_RMENU",
            "VK_BROWSER_BACK", "S_BROWSER_BACK",
            "VK_BROWSER_FORWARD", "S_BROWSER_FORWARD",
            "VK_BROWSER_REFRESH", "S_BROWSER_REFRESH",
            "VK_BROWSER_STOP", "S_BROWSER_STOP",
            "VK_BROWSER_SEARCH", "S_BROWSER_SEARCH",
            "VK_BROWSER_FAVORITES", "S_BROWSER_FAVORITES",
            "VK_BROWSER_HOME", "S_BROWSER_HOME",
            "VK_VOLUME_MUTE", "S_VOLUME_MUTE",
            "VK_VOLUME_DOWN", "S_VOLUME_DOWN",
            "VK_VOLUME_UP", "S_VOLUME_UP",
            "VK_MEDIA_NEXT_TRACK", "S_MEDIA_NEXT_TRACK",
            "VK_MEDIA_PREV_TRACK", "S_MEDIA_PREV_TRACK",
            "VK_MEDIA_STOP", "S_MEDIA_STOP",
            "VK_MEDIA_PLAY_PAUSE", "S_MEDIA_PLAY_PAUSE",
            "VK_LAUNCH_MAIL", "S_LAUNCH_MAIL",
            "VK_LAUNCH_MEDIA_SELECT", "S_LAUNCH_MEDIA_SELECT",
            "VK_LAUNCH_APP1", "S_LAUNCH_APP1",
            "VK_LAUNCH_APP2", "S_LAUNCH_APP2",
            "VK_OEM_1", "S_OEM_1",
            "VK_OEM_PLUS", "S_OEM_PLUS",
            "VK_OEM_COMMA", "S_OEM_COMMA",
            "VK_OEM_MINUS", "S_OEM_MINUS",
            "VK_OEM_PERIOD", "S_OEM_PERIOD",
            "VK_OEM_2", "S_OEM_2",
            "VK_OEM_3", "S_OEM_3",
            "VK_OEM_4", "S_OEM_4",
            "VK_OEM_5", "S_OEM_5",
            "VK_OEM_6", "S_OEM_6",
            "VK_OEM_7", "S_OEM_7",
            "VK_OEM_8", "S_OEM_8",
            "VK_OEM_102", "S_OEM_102",
            "VK_PROCESSKEY", "S_PROCESSKEY",
            "VK_PACKET", "S_PACKET",
            "VK_ATTN", "S_ATTN",
            "VK_CRSEL", "S_CRSEL",
            "VK_EXSEL", "S_EXSEL",
            "VK_EREOF", "S_EREOF",
            "VK_PLAY", "S_PLAY",
            "VK_ZOOM", "S_ZOOM",
            "VK_NONAME", "S_NONAME",
            "VK_PA1", "S_PA1",
            "VK_OEM_CLEAR", "S_OEM_CLEAR"
            };
        string[] methods = { "ToString()",
                "RemoveAt(0)",
                "Add",
                "MoveMouseTo",
                "MoveMouseBy",
                "SendLeftClick",
                "SendLeftClickF",
                "SendRightClick",
                "SendRightClickF",
                "SendMiddleClick",
                "SendMiddleClickF",
                "SendWheelUp",
                "SendWheelDown",
                "SendKey",
                "SendKeyF",
                "CANCEL",
                "BACK",
                "TAB",
                "CLEAR",
                "RETURN",
                "SHIFT",
                "CONTROL",
                "MENU",
                "CAPITAL",
                "ESCAPE",
                "SPACE",
                "PRIOR",
                "NEXT",
                "END",
                "HOME",
                "LEFT",
                "UP",
                "RIGHT",
                "DOWN",
                "SNAPSHOT",
                "INSERT",
                "NUMPADDEL",
                "NUMPADINSERT",
                "HELP",
                "APOSTROPHE",
                "BACKSPACE",
                "PAGEDOWN",
                "PAGEUP",
                "FIN",
                "MOUSE",
                "A",
                "B",
                "C",
                "D",
                "E",
                "F",
                "G",
                "H",
                "I",
                "J",
                "K",
                "L",
                "M",
                "N",
                "O",
                "P",
                "Q",
                "R",
                "S",
                "T",
                "U",
                "V",
                "W",
                "X",
                "Y",
                "Z",
                "LWIN",
                "RWIN",
                "APPS",
                "DELETE",
                "NUMPAD0",
                "NUMPAD1",
                "NUMPAD2",
                "NUMPAD3",
                "NUMPAD4",
                "NUMPAD5",
                "NUMPAD6",
                "NUMPAD7",
                "NUMPAD8",
                "NUMPAD9",
                "MULTIPLY",
                "ADD",
                "SUBTRACT",
                "DECIMAL",
                "PRINTSCREEN",
                "DIVIDE",
                "F1",
                "F2",
                "F3",
                "F4",
                "F5",
                "F6",
                "F7",
                "F8",
                "F9",
                "F10",
                "F11",
                "F12",
                "NUMLOCK",
                "SCROLLLOCK",
                "LEFTSHIFT",
                "RIGHTSHIFT",
                "LEFTCONTROL",
                "RIGHTCONTROL",
                "LEFTALT",
                "RIGHTALT",
                "BROWSER_BACK",
                "BROWSER_FORWARD",
                "BROWSER_REFRESH",
                "BROWSER_STOP",
                "BROWSER_SEARCH",
                "BROWSER_FAVORITES",
                "BROWSER_HOME",
                "VOLUME_MUTE",
                "VOLUME_DOWN",
                "VOLUME_UP",
                "MEDIA_NEXT_TRACK",
                "MEDIA_PREV_TRACK",
                "MEDIA_STOP",
                "MEDIA_PLAY_PAUSE",
                "LAUNCH_MAIL",
                "LAUNCH_MEDIA_SELECT",
                "LAUNCH_APP1",
                "LAUNCH_APP2",
                "OEM_1",
                "OEM_PLUS",
                "OEM_COMMA",
                "OEM_MINUS",
                "OEM_PERIOD",
                "OEM_2",
                "OEM_3",
                "OEM_4",
                "OEM_5",
                "OEM_6",
                "OEM_7",
                "OEM_8",
                "OEM_102",
                "EREOF",
                "ZOOM",
                "Escape",
                "One",
                "Two",
                "Three",
                "Four",
                "Five",
                "Six",
                "Seven",
                "Eight",
                "Nine",
                "Zero",
                "DashUnderscore",
                "PlusEquals",
                "Backspace",
                "Tab",
                "OpenBracketBrace",
                "CloseBracketBrace",
                "Enter",
                "Control",
                "SemicolonColon",
                "SingleDoubleQuote",
                "Tilde",
                "LeftShift",
                "BackslashPipe",
                "CommaLeftArrow",
                "PeriodRightArrow",
                "ForwardSlashQuestionMark",
                "RightShift",
                "RightAlt",
                "Space",
                "CapsLock",
                "Up",
                "Down",
                "Right",
                "Left",
                "Home",
                "End",
                "Delete",
                "PageUp",
                "PageDown",
                "Insert",
                "PrintScreen",
                "NumLock",
                "ScrollLock",
                "Menu",
                "WindowsKey",
                "NumpadDivide",
                "NumpadAsterisk",
                "Numpad7",
                "Numpad8",
                "Numpad9",
                "Numpad4",
                "Numpad5",
                "Numpad6",
                "Numpad1",
                "Numpad2",
                "Numpad3",
                "Numpad0",
                "NumpadDelete",
                "NumpadEnter",
                "NumpadPlus",
                "NumpadMinus",
                };
        string[] snippets = { "if(^)\n{\n}", "if(^)\n{\n}\nelse\n{\n}", "for(^;;)\n{\n}", "while(^)\n{\n}", "do${\n^}while();", "switch(^)\n{\n\tcase : break;\n}" };
        string[] declarationSnippets = {
               "public class ^\n{\n}", "private class ^\n{\n}", "internal class ^\n{\n}",
               "public struct ^\n{\n}", "private struct ^\n{\n}", "internal struct ^\n{\n}",
               "public void ^()\n{\n}", "private void ^()\n{\n}", "internal void ^()\n{\n}", "protected void ^()\n{\n}",
               "public ^{ get; set; }", "private ^{ get; set; }", "internal ^{ get; set; }", "protected ^{ get; set; }",
               "public void ^()\n{\n}", "private void ^()\n{\n}", "public string ^()\n{\n}", "private string ^()\n{\n}",
               "public bool ^()\n{\n}", "private bool ^()\n{\n}", "public int ^()\n{\n}", "private int ^()\n{\n}",
               "public double ^()\n{\n}", "private double ^()\n{\n}", "public float ^()\n{\n}", "private float ^()\n{\n}"
               };
        public Form1()
        {
            InitializeComponent();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "New", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    fileText.Clear();
                    this.Text = "WiiJoyScripter";
                    openFilePath = null;
                }
            }
            else
            {
                fileText.Clear();
                this.Text = "WiiJoyScripter";
                openFilePath = null;
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Open", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    OpenFileDialog op = new OpenFileDialog();
                    op.Filter = "Text Document(*.txt)|*.txt|All Files(*.*)|";
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        fileText.LoadFile(op.FileName, RichTextBoxStreamType.PlainText);
                        justSaved = true;
                        filename = op.FileName;
                        openFilePath = op.FileName;
                        this.Text = op.FileName;
                    }
                }
            }
            else
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Text Document(*.txt)|*.txt|All Files(*.*)|";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    fileText.LoadFile(op.FileName, RichTextBoxStreamType.PlainText);
                    justSaved = true;
                    filename = op.FileName;
                    openFilePath = op.FileName;
                    this.Text = op.FileName;
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFilePath == null)
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                fileText.SaveFile(openFilePath, RichTextBoxStreamType.PlainText);
                justSaved = true;
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Text Document(*.txt)|*.txt|All Files(*.*)|";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                fileText.SaveFile(sf.FileName, RichTextBoxStreamType.PlainText);
                justSaved = true;
                filename = sf.FileName;
                openFilePath = sf.FileName;
                this.Text = sf.FileName;
            }
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Cut();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Copy();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Paste();
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Undo();
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Redo();
        }
        private void colorWords()
        {
            fileText.ForeColor = Color.Black;
            string tempTxt = fileText.Text;
            fileText.Clear();
            fileText.Text = tempTxt;
            for (int i = 0; i < fileText.Lines.Length; i++)
            {
                string currentLine = fileText.Lines[i];
                string toBeSearched1 = "/*";
                string toBeSearched2 = "*/";
                int pFrom = currentLine.IndexOf(toBeSearched1) + toBeSearched1.Length;
                int pTo = currentLine.LastIndexOf(toBeSearched2);
                try
                {
                    string word = "/*" + currentLine.Substring(pFrom, pTo - pFrom) + "*/";
                    int start = fileText.GetFirstCharIndexFromLine(i) + currentLine.IndexOf("/*");
                    int length = word.Length;
                    if (start > -1 & length > -1)
                    {
                        fileText.SelectionStart = start;
                        fileText.SelectionLength = length;
                        if (fileText.SelectedText == word)
                            fileText.SelectionColor = Color.DarkGreen;
                    }
                }
                catch { }
                coloringText(i, "public", Color.Blue);
                coloringText(i, "private", Color.Blue);
                coloringText(i, "static", Color.Blue);
                coloringText(i, "new", Color.Blue);
                coloringText(i, "(", Color.Blue);
                coloringText(i, ")", Color.Blue);
                coloringText(i, "void", Color.Blue);
                coloringText(i, "bool", Color.Blue);
                coloringText(i, "int", Color.Blue);
                coloringText(i, "float", Color.Blue);
                coloringText(i, "double", Color.Blue);
                coloringText(i, "Double", Color.Blue);
                coloringText(i, "List", Color.Blue);
                coloringText(i, "string", Color.Blue);
                coloringText(i, "break", Color.Blue);
                coloringText(i, "false", Color.Blue);
                coloringText(i, "true", Color.Blue);
                coloringText(i, "for", Color.Blue);
                coloringText(i, "do", Color.Blue);
                coloringText(i, "while", Color.Blue);
                coloringText(i, "[", Color.Red);
                coloringText(i, "]", Color.Red);
                coloringText(i, "if", Color.Purple);
                coloringText(i, "else", Color.Purple);
                coloringText(i, "this", Color.Chocolate);
                coloringText(i, "input", Color.Brown);
                coloringText(i, "mWSNunchuckStateRawJoystickX", Color.DarkOrange);
                coloringText(i, "mWSNunchuckStateRawJoystickY", Color.DarkOrange);
                coloringText(i, "mWSNunchuckStateRawValuesX", Color.DarkOrange);
                coloringText(i, "mWSNunchuckStateRawValuesY", Color.DarkOrange);
                coloringText(i, "mWSNunchuckStateRawValuesZ", Color.DarkOrange);
                coloringText(i, "mWSNunchuckStateC", Color.DarkOrange);
                coloringText(i, "mWSNunchuckStateZ", Color.DarkOrange);
                coloringText(i, "mWSButtonStateIRX", Color.DarkOrange);
                coloringText(i, "mWSButtonStateIRY", Color.DarkOrange);
                coloringText(i, "mWSButtonStateA", Color.DarkOrange);
                coloringText(i, "mWSButtonStateB", Color.DarkOrange);
                coloringText(i, "mWSButtonStateMinus", Color.DarkOrange);
                coloringText(i, "mWSButtonStateHome", Color.DarkOrange);
                coloringText(i, "mWSButtonStatePlus", Color.DarkOrange);
                coloringText(i, "mWSButtonStateOne", Color.DarkOrange);
                coloringText(i, "mWSButtonStateTwo", Color.DarkOrange);
                coloringText(i, "mWSButtonStateUp", Color.DarkOrange);
                coloringText(i, "mWSButtonStateDown", Color.DarkOrange);
                coloringText(i, "mWSButtonStateLeft", Color.DarkOrange);
                coloringText(i, "mWSButtonStateRight", Color.DarkOrange);
                coloringText(i, "mWSRawValuesX", Color.DarkOrange);
                coloringText(i, "mWSRawValuesY", Color.DarkOrange);
                coloringText(i, "mWSRawValuesZ", Color.DarkOrange);
                coloringText(i, "EulerAnglesX", Color.DarkOrange);
                coloringText(i, "EulerAnglesY", Color.DarkOrange);
                coloringText(i, "EulerAnglesZ", Color.DarkOrange);
                coloringText(i, "DirectAnglesX", Color.DarkOrange);
                coloringText(i, "DirectAnglesY", Color.DarkOrange);
                coloringText(i, "DirectAnglesZ", Color.DarkOrange);
                coloringText(i, "camx", Color.DarkOrange);
                coloringText(i, "camy", Color.DarkOrange);
                coloringText(i, "EulerAnglesLeftX", Color.DarkOrange);
                coloringText(i, "EulerAnglesLeftY", Color.DarkOrange);
                coloringText(i, "EulerAnglesLeftZ", Color.DarkOrange);
                coloringText(i, "DirectAnglesLeftX", Color.DarkOrange);
                coloringText(i, "DirectAnglesLeftY", Color.DarkOrange);
                coloringText(i, "DirectAnglesLeftZ", Color.DarkOrange);
                coloringText(i, "EulerAnglesRightX", Color.DarkOrange);
                coloringText(i, "EulerAnglesRightY", Color.DarkOrange);
                coloringText(i, "EulerAnglesRightZ", Color.DarkOrange);
                coloringText(i, "DirectAnglesRightX", Color.DarkOrange);
                coloringText(i, "DirectAnglesRightY", Color.DarkOrange);
                coloringText(i, "DirectAnglesRightZ", Color.DarkOrange);
                coloringText(i, "LeftButtonSHOULDER_1", Color.DarkOrange);
                coloringText(i, "LeftButtonMINUS", Color.DarkOrange);
                coloringText(i, "LeftButtonCAPTURE", Color.DarkOrange);
                coloringText(i, "LeftButtonDPAD_UP", Color.DarkOrange);
                coloringText(i, "LeftButtonDPAD_LEFT", Color.DarkOrange);
                coloringText(i, "LeftButtonDPAD_DOWN", Color.DarkOrange);
                coloringText(i, "LeftButtonDPAD_RIGHT", Color.DarkOrange);
                coloringText(i, "LeftButtonSTICK", Color.DarkOrange);
                coloringText(i, "RightButtonDPAD_DOWN", Color.DarkOrange);
                coloringText(i, "LeftButtonSL", Color.DarkOrange);
                coloringText(i, "LeftButtonSR", Color.DarkOrange);
                coloringText(i, "GetStickLeftX", Color.DarkOrange);
                coloringText(i, "GetStickLeftY", Color.DarkOrange);
                coloringText(i, "RightButtonPLUS", Color.DarkOrange);
                coloringText(i, "RightButtonDPAD_RIGHT", Color.DarkOrange);
                coloringText(i, "RightButtonHOME", Color.DarkOrange);
                coloringText(i, "RightButtonSHOULDER_1", Color.DarkOrange);
                coloringText(i, "RightButtonDPAD_LEFT", Color.DarkOrange);
                coloringText(i, "RightButtonDPAD_UP", Color.DarkOrange);
                coloringText(i, "RightButtonSTICK", Color.DarkOrange);
                coloringText(i, "RightButtonSL", Color.DarkOrange);
                coloringText(i, "RightButtonSR", Color.DarkOrange);
                coloringText(i, "RightButtonSHOULDER_2", Color.DarkOrange);
                coloringText(i, "LeftButtonSHOULDER_2", Color.DarkOrange);
                coloringText(i, "GetStickRightX", Color.DarkOrange);
                coloringText(i, "GetStickRightY", Color.DarkOrange);
                coloringText(i, "GetAccelX", Color.DarkOrange);
                coloringText(i, "GetAccelY", Color.DarkOrange);
                coloringText(i, "GetAccelZ", Color.DarkOrange);
                coloringText(i, "GetAccelRightX", Color.DarkOrange);
                coloringText(i, "GetAccelRightY", Color.DarkOrange);
                coloringText(i, "GetAccelRightZ", Color.DarkOrange);
                coloringText(i, "GetAccelLeftX", Color.DarkOrange);
                coloringText(i, "GetAccelLeftY", Color.DarkOrange);
                coloringText(i, "GetAccelLeftZ", Color.DarkOrange);
                coloringText(i, "watchM", Color.DarkOrange);
                coloringText(i, "MouseHookX", Color.DarkOrange);
                coloringText(i, "MouseHookY", Color.DarkOrange);
                coloringText(i, "MouseHookWheel", Color.DarkOrange);
                coloringText(i, "MouseHookLeftButton", Color.DarkOrange);
                coloringText(i, "MouseHookRightButton", Color.DarkOrange);
                coloringText(i, "MouseHookDoubleClick", Color.DarkOrange);
                coloringText(i, "MouseHookMiddleButton", Color.DarkOrange);
                coloringText(i, "GetAsyncKeyState", Color.DarkBlue);
                coloringText(i, "System.Windows.Forms.Keys", Color.DarkBlue);
                coloringText(i, "GetCursorPos", Color.DarkBlue);
                coloringText(i, "out", Color.DarkBlue);
                coloringText(i, "_Valuechanged", Color.DarkBlue);
                coloringText(i, "valchanged", Color.DarkBlue);
                coloringText(i, "wd", Color.DarkBlue);
                coloringText(i, "wu", Color.DarkBlue);
                coloringText(i, "System.Collections.Generic.List<double>", Color.Blue);
                coloringText(i, "//keyboard control", Color.DarkGreen);
                coloringText(i, "//mouse control", Color.DarkGreen);
                coloringText(i, "foreach", Color.Blue);
                coloringText(i, "return ", Color.Blue);
                coloringText(i, "RemoveAt", Color.Blue);
                coloringText(i, "Add", Color.Blue);
            }
        }
        private void coloringText(int i, string word, Color color)
        {
            string currentLine = fileText.Lines[i];
            int index = -1;
            while ((index = currentLine.IndexOf(word, index + 1)) >= 0)
            {
                int start = fileText.GetFirstCharIndexFromLine(i) + index;
                int length = word.Length;
                if (start > -1 & length > -1)
                {
                    fileText.SelectionStart = start;
                    fileText.SelectionLength = length;
                    if (fileText.SelectedText == word)
                        fileText.SelectionColor = color;
                }
            }
        }
        private void initCode()
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
        }
        private void initConfigM(string confname)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(confname);
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
            addedcode = linesofcode;
        }
        private void checktoolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (openFilePath != null)
                try
                {
                    fileText.SaveFile("temp", RichTextBoxStreamType.PlainText);
                    checkScript("temp");
                }
                catch { }
        }
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            justSavedbefore = justSaved;
            colorWords();
            justSaved = justSavedbefore;
        }
        private void initConfigK(string confname)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(confname);
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
            addedcode = linesofcode;
        }
        private void fileText_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                fileText.ContextMenu = contextMenu;
            }
        }
        private void changeCursor(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void cutAction(object sender, EventArgs e)
        {
            fileText.Cut();
        }
        private void copyAction(object sender, EventArgs e)
        {
            if (fileText.SelectedText != "")
                Clipboard.SetText(fileText.SelectedText);
        }
        private void pasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                fileText.SelectedText = Clipboard.GetText(TextDataFormat.Text).ToString();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Exit", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
            if (filename != "")
            {
                using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter("tempsave"))
                {
                    createdfile.WriteLine(filename);
                }
            }
        }
        private void fileText_TextChanged(object sender, EventArgs e)
        {
            justSaved = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            BuildAutocompleteMenu();
            menuItem = new MenuItem("Cut");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(cutAction);
            menuItem = new MenuItem("Copy");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(copyAction);
            menuItem = new MenuItem("Paste");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(pasteAction);
            fileText.ContextMenu = contextMenu;
        }
        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(this);
            wordsearch = fileText.SelectedText;
            form3.Show();
        }
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            wordreplace = fileText.SelectedText;
            form2.Show();
        }
        public void search(bool fromstart, string word)
        {
            justSavedbefore = justSaved;
            if (fromstart)
                pos = fileText.Find(word, 0, RichTextBoxFinds.None);
            else
                pos = fileText.Find(word, fileText.SelectionStart + 1, RichTextBoxFinds.None);
            if (pos >= 0)
            {
                fileText.Select(pos, word.Length);
                fileText.SelectionBackColor = Color.Yellow;
                fileText.ScrollToCaret();
            }
            justSaved = justSavedbefore;
        }
        public string havewordsearch()
        { 
            return wordsearch; 
        }
        public void replaceAll(string searchText, string replacetext)
        {
            try
            {
                pos = fileText.Find(searchText, 0, RichTextBoxFinds.None);
                if (pos >= 0)
                {
                    fileText.Select(pos, searchText.Length);
                    fileText.ScrollToCaret();
                    if (fileText.SelectedText == searchText)
                        fileText.SelectedText = replacetext;
                }
                while (pos >= 0)
                {
                    pos = fileText.Find(searchText, fileText.SelectionStart + 1, RichTextBoxFinds.None);
                    if (pos >= 0)
                    {
                        fileText.Select(pos, searchText.Length);
                        fileText.ScrollToCaret();
                        if (fileText.SelectedText == searchText)
                            fileText.SelectedText = replacetext;
                    }
                }
            }
            catch { }
        }
        public void next(bool fromstart, string searchText)
        {
            justSavedbefore = justSaved;
            if (fromstart)
                pos = fileText.Find(searchText, 0, RichTextBoxFinds.None);
            else
                pos = fileText.Find(searchText, fileText.SelectionStart + 1, RichTextBoxFinds.None);
            if (pos >= 0)
            {
                fileText.Select(pos, searchText.Length);
                fileText.SelectionBackColor = Color.Yellow;
                fileText.ScrollToCaret();
            }
            justSaved = justSavedbefore;
        }
        public void replace(string searchText, string replacetext)
        {
            if (fileText.SelectedText == searchText)
                fileText.SelectedText = replacetext;
        }
        public void clearcolor(string word)
        {
            justSavedbefore = justSaved;
            try
            {
                string tempTxt = fileText.Text;
                fileText.Clear();
                fileText.Text = tempTxt;
                fileText.Select(pos, word.Length);
            }
            catch { }
            justSaved = justSavedbefore;
        }
        public string havewordreplace()
        {
            return wordreplace;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("tempsave"))
                {
                    filename = file.ReadLine();
                }
                if (filename != "")
                {
                    fileText.LoadFile(filename, RichTextBoxStreamType.PlainText);
                    openFilePath = filename;
                    this.Text = filename; 
                    justSaved = true;
                }
            }
            catch { }
        }
        public void checkScript(string confname)
        {
            initConfigM(confname);
            initCode();
            finalcode = code.Replace("funct_driver", addedcode);
            System.CodeDom.Compiler.CompilerParameters parameters = new System.CodeDom.Compiler.CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerResults results = provider.CompileAssemblyFromSource(parameters, finalcode);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}) : {1}", error.ErrorNumber, error.ErrorText));
                }
                MessageBox.Show("mouse control :\n\r" + sb.ToString());
            }
            initConfigK(confname);
            initCode();
            finalcode = code.Replace("funct_driver", addedcode);
            parameters = new System.CodeDom.Compiler.CompilerParameters();
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
            }
        }
        private void BuildAutocompleteMenu()
        {
            var items = new List<AutocompleteItem>();

            foreach (var item in snippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });
            foreach (var item in declarationSnippets)
                items.Add(new DeclarationSnippet(item) { ImageIndex = 0 });
            foreach (var item in methods)
                items.Add(new MethodAutocompleteItem(item) { ImageIndex = 2 });
            foreach (var item in keywords)
                items.Add(new AutocompleteItem(item));

            items.Add(new InsertSpaceSnippet());
            items.Add(new InsertSpaceSnippet(@"^(\w+)([=<>!:]+)(\w+)$"));
            items.Add(new InsertEnterSnippet());

            //set as autocomplete source
            autocompleteMenu1.SetAutocompleteItems(items);
        }
    }


    /// <summary>
    /// This item appears when any part of snippet text is typed
    /// </summary>
    class DeclarationSnippet : SnippetAutocompleteItem
    {
        public static string RegexSpecSymbolsPattern = @"[\^\$\[\]\(\)\.\\\*\+\|\?\{\}]";

        public DeclarationSnippet(string snippet)
            : base(snippet)
        {
        }

        public override CompareResult Compare(string fragmentText)
        {
            var pattern = Regex.Replace(fragmentText, RegexSpecSymbolsPattern, "\\$0");
            if (Regex.IsMatch(Text, "\\b" + pattern, RegexOptions.IgnoreCase))
                return CompareResult.Visible;
            return CompareResult.Hidden;
        }
    }

    /// <summary>
    /// Divides numbers and words: "123AND456" -> "123 AND 456"
    /// Or "i=2" -> "i = 2"
    /// </summary>
    class InsertSpaceSnippet : AutocompleteItem
    {
        string pattern;

        public InsertSpaceSnippet(string pattern)
            : base("")
        {
            this.pattern = pattern;
        }

        public InsertSpaceSnippet()
            : this(@"^(\d+)([a-zA-Z_]+)(\d*)$")
        {
        }

        public override CompareResult Compare(string fragmentText)
        {
            if (Regex.IsMatch(fragmentText, pattern))
            {
                Text = InsertSpaces(fragmentText);
                if (Text != fragmentText)
                    return CompareResult.Visible;
            }
            return CompareResult.Hidden;
        }

        public string InsertSpaces(string fragment)
        {
            var m = Regex.Match(fragment, pattern);
            if (m.Groups[1].Value == "" && m.Groups[3].Value == "")
                return fragment;
            return (m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value).Trim();
        }

        public override string ToolTipTitle
        {
            get
            {
                return Text;
            }
        }
    }

    /// <summary>
    /// Inerts line break after '}'
    /// </summary>
    class InsertEnterSnippet : AutocompleteItem
    {
        int enterPlace = 0;

        public InsertEnterSnippet()
            : base("[Line break]")
        {
        }

        public override CompareResult Compare(string fragmentText)
        {
            var tb = Parent.TargetControlWrapper;

            var text = tb.Text;
            for (int i = Parent.Fragment.Start - 1; i >= 0; i--)
            {
                if (text[i] == '\n')
                    break;
                if (text[i] == '}')
                {
                    enterPlace = i;
                    return CompareResult.Visible;
                }
            }

            return CompareResult.Hidden;
        }

        public override string GetTextForReplace()
        {
            var tb = Parent.TargetControlWrapper;

            //insert line break
            tb.SelectionStart = enterPlace + 1;
            tb.SelectedText = "\n";
            Parent.Fragment.Start += 1;
            Parent.Fragment.End += 1;
            return Parent.Fragment.Text;
        }

        public override string ToolTipTitle
        {
            get
            {
                return "Insert line break after '}'";
            }
        }
    }
}