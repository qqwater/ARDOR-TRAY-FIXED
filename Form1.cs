namespace ARDOR_TRAY
{
    public partial class Form1 : Form
    {
        private System.Diagnostics.Process _ardorGamingProcess;
        private bool isNotificationsEnabled = false;
        private string ardorExecutablePath;

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            Location = new Point(1250, 268);
            TopMost = true;

            // Load the saved path from Settings
            ardorExecutablePath = Properties.Settings.Default.ArdorGamingExePath;
            if (!string.IsNullOrEmpty(ardorExecutablePath))
            {
                TryOpenArdorGamingAgile();
            }
            else
            {
                // Use the default path if no saved path is found
                ardorExecutablePath = "C:\\Program Files (x86)\\ARDOR GAMING\\Agile Wired\\1\\OemDrv.exe";
                TryOpenArdorGamingAgile();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Text = "ARDOR GAMING Agile";
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
            TopMost = true;
            TryOpenArdorGamingAgile();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon1.Visible = true;
                if (_ardorGamingProcess != null && !_ardorGamingProcess.HasExited)
                {
                    _ardorGamingProcess.CloseMainWindow();
                    _ardorGamingProcess.WaitForExit();
                }
                if (isNotificationsEnabled)
                {
                    notifyIcon1.BalloonTipTitle = "ARDOR GAMING Agile";
                    notifyIcon1.BalloonTipText = "Приложение свернуто";
                    notifyIcon1.ShowBalloonTip(3000);
                }
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TryOpenArdorGamingAgile()
        {
            try
            {
                _ardorGamingProcess = System.Diagnostics.Process.Start(ardorExecutablePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия файла, укажите путь к исполняемому файлу(Please indicate the path to the program): {ex.Message}");
            }
        }

        private void включитьУведомленияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isNotificationsEnabled = !isNotificationsEnabled;
            включитьУведомленияToolStripMenuItem.Checked = isNotificationsEnabled;
        }

        private void указатьПутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.RootFolder = Environment.SpecialFolder.ProgramFiles;
            folderDialog.Description = "Выберите папку с ARDOR GAMING Agile";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                ardorExecutablePath = Path.Combine(folderDialog.SelectedPath, "OemDrv.exe");
                Properties.Settings.Default.ArdorGamingExePath = ardorExecutablePath;
                Properties.Settings.Default.Save();
                TryOpenArdorGamingAgile();
            }
        }
    }
}
