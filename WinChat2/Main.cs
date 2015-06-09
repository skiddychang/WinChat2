using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

namespace WinChat2
{
    public partial class frmMain : Form
    {
        TcpClient Sck = new TcpClient();
        NetworkStream ServerStream = default(NetworkStream);
        Thread ctThread = null;

        String Data = String.Empty;

        EndPoint epLocal, epRemote;
        Dictionary<String, Object> Settings;
        const String SettingsFilename = "Settings.txt";

        const Int32 BufferSize = 65536;
        const Int32 Port = 1337;

        Boolean connected = false;
        Boolean connecting = false;

        String MyNick = String.Empty;
        String DefaultAddress = String.Empty;
        String CurrentConnection = String.Empty;

        public frmMain()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            KeyPreview = true;
            SetControls();
            LoadSettings();
            Connected(false);
            TryToConnect(DefaultAddress);

        }

        public void LoadSettings()
        {
            Settings = FileHelper.GetDictionary(SettingsFilename);
            DefaultAddress = TryGetSetting("defaultaddress");
            String FontFamily = TryGetSetting("fontfamily", lblContent.Font.FontFamily.ToString());
            float FontSize = TryGetSetting("fontsize", lblContent.Font.Size);
            try
            {
                lblContent.Font = new Font(new FontFamily(FontFamily), FontSize);
            }
            catch { }
            MyNick = TryGetSetting("nick", "Me");
        }

        void SetControls()
        {
            int margin = 9;
            int clientWidth = this.ClientRectangle.Width;
            int clientHeight = this.ClientRectangle.Height;

            tbInput.Location = new Point(margin, clientHeight - tbInput.Height - margin * 3);
            tbInput.Width = clientWidth - (margin * 2);
            tbInput.Height = 40;

            lblContent.Location = new Point(margin, margin * 4);
            lblContent.Width = clientWidth - (margin * 2);
            lblContent.Height = clientHeight - margin - tbInput.Height - margin * 5;

            lblConnected.Location = new Point(clientWidth - lblConnected.Width - margin, margin);
            tbInput.Focus();
        }

        void TryToConnect(String Address)
        {
            try
            {
                IPAddress[] AddressesList = Dns.GetHostAddresses(Address);

                Sck = new TcpClient();
                Sck.Connect(AddressesList, Port);
                ServerStream = Sck.GetStream();

                byte[] OutBuffer = Encoding.UTF8.GetBytes(MyNick + "$");
                ServerStream.Write(OutBuffer, 0, OutBuffer.Length);
                ServerStream.Flush();

                ctThread = new Thread(GetMessage);
                ctThread.Start();

                Connected(true);
                CurrentConnection = Address;
                connecting = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void Disconnect()
        {
            ctThread.Abort();
            Sck.Close();
            //MessageBox.Show("Disconnected");
            CurrentConnection = String.Empty;
            Connected(false);
        }

        void Reconnect()
        {
            Disconnect();
            TryToConnect(CurrentConnection);
        }

        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    HandleInput();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        void ProcessCommand(String Command)
        {
            if (Command.StartsWith("connect") || Command.StartsWith("/connect") && !connected)
            {
                try
                {
                    switch (Command.Split(' ')[1].ToString())
                    {
                        case "default":
                        case "def":
                            TryToConnect(DefaultAddress);
                            break;
                        default:
                            TryToConnect(Command.Split(' ')[1].ToString());
                            break;

                    }
                }
                catch { }
                return;
            }
            // kostki
            if (Char.IsDigit(Command[0]) && Char.IsDigit(Command[Command.Length - 1]))
            {
                int count = int.Parse(Command.Split('d')[0]);
                int range = int.Parse(Command.Split('d')[1]);
                for (int i = 0; i <= count; i++)
                {
                    Random r = new Random(Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value));
                    SendMessage(r.Next(1, range).ToString());
                }
            }

            switch (Command)
            {
                case "exit":
                    Application.Exit();
                    break;
                case "cls":
                case "clear":
                    lblContent.Clear();
                    break;
                case "time /t":
                    lblContent.AppendText(DateTime.Now.TimeOfDay.ToString());
                    lblContent.AppendText(Environment.NewLine);
                    break;
                case "disconnect":
                    if (connected)
                        Disconnect();
                    break;
                case "nociekawe":
                    String ULTIMATESTRINGOFLIFEANDDEATH = "WYPOWIEDZENIE";
                    for (int i = 0; i < ULTIMATESTRINGOFLIFEANDDEATH.Length; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            lblContent.AppendText(" ");
                        }
                        lblContent.AppendText(ULTIMATESTRINGOFLIFEANDDEATH[i].ToString());
                        lblContent.AppendText(Environment.NewLine);
                    }
                    break;
                case "reconnect":
                    if (connected)
                        Reconnect();
                    break;
            }
            lblContent.ScrollToCaret();
        }

        void HandleInput()
        {
            String Input = tbInput.Text;
            if (String.IsNullOrEmpty(Input))
                return;

            if (Input.StartsWith("/"))
            {
                ProcessCommand(Input.Substring(1));
                tbInput.Clear();
                return;
            }

            if (connected)
            {
                SendMessage();
            }
            tbInput.Clear();
        }

        void SendMessage()
        {
            Byte[] OutputStream = Encoding.UTF8.GetBytes(tbInput.Text + "$");
            try
            {
                ServerStream.Write(OutputStream, 0, OutputStream.Length);
                ServerStream.Flush();
            }
            catch
            {
                Disconnect();
            }
        }

        void SendMessage(String Msg)
        {
            Byte[] OutputStream = Encoding.UTF8.GetBytes(Msg + "$");
            try
            {
                ServerStream.Write(OutputStream, 0, OutputStream.Length);
                ServerStream.Flush();
            }
            catch
            {
                Disconnect();
            }
        }


        String GetTime()
        {
            return DateTime.Now.Hour + ":" + ((Convert.ToInt32(DateTime.Now.Minute) < 10) ? "0" : "") + DateTime.Now.Minute.ToString();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Application.Exit();
                    break;

            }
        }

        private void GetMessage()
        {
            for (; ; )
            {
                ServerStream = Sck.GetStream();
                int buffSize = 0;
                byte[] InputStream = new byte[BufferSize];
                buffSize = Sck.ReceiveBufferSize;
                try
                {
                    ServerStream.Read(InputStream, 0, buffSize);
                }
                catch
                {
                    Disconnect();
                }
                String ReturnData = Encoding.UTF8.GetString(InputStream);
                Data = String.Empty + ReturnData;

                if (connecting)
                {
                    if (!Data.Contains("Joined"))
                        Disconnect();
                    connecting = false;
                }

                Msg();
                //Bold();
            }
        }

        private void Msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(Msg));
            else
            {
                Data = Data.Replace("\0", String.Empty);
                lblContent.AppendText(Data + Environment.NewLine);
                lblContent.ScrollToCaret();
            }
        }

        void Bold(String PWord)
        {
            try
            {
                Int32 S_Start = lblContent.SelectionStart, StartIndex = 0, Index;
                String Word = PWord;
                while ((Index = lblContent.Text.IndexOf(Word, StartIndex)) != -1)
                {
                    lblContent.Select(Index, Word.Length);
                    lblContent.SelectionFont = new Font(lblContent.Font, FontStyle.Bold);
                    //lblContent.SelectionColor = Color.FromArgb(TryGetSetting("fontcolor", "black"));
                    StartIndex = Index + Word.Length;
                }

                lblContent.SelectionStart = S_Start;
                lblContent.SelectionLength = 0;
                lblContent.SelectionFont = new Font(lblContent.Font, FontStyle.Regular);
                //lblContent.SelectionColor = Color.Black;
            }
            catch { }
        }

        void Connected(Boolean Con)
        {
            if (Con)
            {
                lblConnected.ForeColor = Color.Green;
                lblConnected.Text = "Connected";
                //tbInput.Enabled = true;
                tbInput.Focus();
                connectToolStripMenuItem1.Enabled = false;
                disconnectToolStripMenuItem.Enabled = true;
                connected = true;
            }
            else
            {
                lblConnected.ForeColor = Color.Red;
                lblConnected.Text = "Not Connected";
                //tbInput.Enabled = false;
                connectToolStripMenuItem1.Enabled = true;
                disconnectToolStripMenuItem.Enabled = false;
                tbInput.Clear();
                connected = false;
            }
        }

        private void hehsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            String Input = String.Empty;
            if (ShowInputDialog(ref Input, TryGetSetting("defaultaddress")) == System.Windows.Forms.DialogResult.OK)
                TryToConnect(Input);
        }

        String TryGetSetting(String Key)
        {
            try { return Settings[Key].ToString(); }
            catch { return String.Empty; }
        }

        String TryGetSetting(String Key, String Default)
        {
            try { return Settings[Key].ToString(); }
            catch { return Default; }
        }

        Int32 TryGetSetting(String Key, Int32 Default)
        {
            try { return (Int32)Settings[Key]; }
            catch { return Default; }
        }
        float TryGetSetting(String Key, float Default)
        {
            try { return float.Parse(Settings[Key].ToString()); }
            catch { return Default; }
        }

        DialogResult ShowInputDialog(ref String Input, String Default)
        {
            Size FormSize = new Size(300, 70);

            Form InputBox = new Form();
            InputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            InputBox.Text = "Connect";
            InputBox.Size = FormSize;

            TextBox tbInput = new TextBox();
            tbInput.Size = new System.Drawing.Size(200, 23);
            tbInput.Location = new System.Drawing.Point(5, 5);
            tbInput.Text = Default;
            InputBox.Controls.Add(tbInput);

            Button btnConfirm = new Button();
            btnConfirm.DialogResult = DialogResult.OK;
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new System.Drawing.Size(75, 23);
            btnConfirm.Text = "Connect";
            btnConfirm.Location = new System.Drawing.Point(tbInput.Location.X + tbInput.Width + 5, 5);
            InputBox.Controls.Add(btnConfirm);

            InputBox.AcceptButton = btnConfirm;

            DialogResult Result = InputBox.ShowDialog();
            Input = tbInput.Text;
            return Result;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Settings = new Settings();
            Settings.FormClosed += Settings_FormClosed;
            Settings.Show();
        }

        void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            String OldNick = MyNick;
            LoadSettings();
            //lblContent.Text = lblContent.Text.Replace(OldNick, MyNick);
            //Bold(MyNick);
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // just for sure
            if (connected)
                Disconnect();
        }

        private void frmMain_ResizeEnd(object sender, EventArgs e)
        {
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (connected)
                Disconnect();
        }

        private void frmMain_ClientSizeChanged(object sender, EventArgs e)
        {
            lblContent.Width = this.ClientRectangle.Width - 15;
            lblContent.Height = this.ClientRectangle.Height - 85;
            tbInput.Width = this.ClientRectangle.Width - 15;
            tbInput.Location = new Point(8, this.ClientRectangle.Height - 45);
            lblConnected.Location = new Point(this.ClientRectangle.Width - lblConnected.Width - 15, 5);
        }
    }
}
