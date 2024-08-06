using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace C969
{
    public partial class Database : Form
    {
        private static string logFilePath = "Login_History.txt";
        private IntPtr previousKeyboardLayout;
        private const int WM_INPUTLANGCHANGE = 0x51;

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private ResourceManager locRM;

        public Database()
        {
            InitializeComponent();
            previousKeyboardLayout = GetKeyboardLayout(0);
            Application.AddMessageFilter(new KeyboardLayoutMessageFilter());
            locRM = new ResourceManager("C969.Properties.Resources", typeof(Database).Assembly);
            DetectLanguage();
        }

        private void DetectLanguage()
        {
            CheckKeyboardLayout(GetKeyboardLayout(0));
        }

        bool button1WasClicked = true;

        private void CheckKeyboardLayout(IntPtr currentKeyboardLayout)
        {
            if (IsItalianKeyboardLayout(currentKeyboardLayout))
                ChangeApplicationLanguageToItalian();
            else
                ChangeApplicationLanguageToDefault();

            previousKeyboardLayout = currentKeyboardLayout;
        }

        private bool IsItalianKeyboardLayout(IntPtr keyboardLayout)
        {
            return ((uint)keyboardLayout & 0xFFFF) == 0x0410;
        }

        private void ChangeApplicationLanguageToItalian()
        {
            CultureInfo italianCulture = new CultureInfo("it");
            System.Threading.Thread.CurrentThread.CurrentUICulture = italianCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = italianCulture;
            label1.Text = "Nome Utente:";
            label2.Text = "Password:";
            UpdateUIWithLocalizedText();
        }

        private void ChangeApplicationLanguageToDefault()
        {
            CultureInfo defaultCulture = new CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = defaultCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = defaultCulture;
            UpdateUIWithLocalizedText();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_INPUTLANGCHANGE && m.WParam != previousKeyboardLayout)
            {
                CheckKeyboardLayout(m.WParam);
            }
        }

        private void UpdateUIWithLocalizedText()
        {
        }

        private class KeyboardLayoutMessageFilter : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_INPUTLANGCHANGE)
                {
                    Database form = Application.OpenForms["Database"] as Database;
                    form?.CheckKeyboardLayout(m.WParam);
                }
                return false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string userPass = textBox2.Text;

            button1WasClicked = true;

            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "it-IT")
            {
                label1.Text = "Nome Utente:";
                label2.Text = "Password:";
            }
            else
            {
                label1.Text = "Username:";
                label2.Text = "Password:";
            }

                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPass))
            {
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "it-IT")
                {
                    ChangeApplicationLanguageToItalian();
                    MessageBox.Show("Il tuo username o la tua password é incorretta.");
                }
                else
                {
                    MessageBox.Show("You haven't entered a username or password.");
                }
                return;
            }

            if ((userName == "test" && userPass == "test") && button1WasClicked)
            {
                string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

                MySqlConnection conn = null;

                try
                {
                    conn = new MySqlConnection(constr);
                    conn.Open();

                    if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "it")
                    {
                        ChangeApplicationLanguageToItalian();
                        MessageBox.Show("Sei connesso.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show("Hai completato l'autenticazione con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Connection is open");
                        MessageBox.Show("You've logged in successfully.");
                    }

                    string logMessage = $"{DateTime.Now.ToString()} - User test logged in.";
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine(logMessage);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("An error occurred while connecting to the database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn?.Close();
                }

                Main main = new Main();
                main.Show();
            }
            else
            {
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "it")
                {
                    ChangeApplicationLanguageToItalian();
                    MessageBox.Show("Il tuo username o la tua password é incorretta.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("You haven't entered a username or password.");
                }
            }
        }


        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void database_Load(object sender, EventArgs e)
        {
        }

        private void Database_Load_1(object sender, EventArgs e)
        {

        }
    }
}
