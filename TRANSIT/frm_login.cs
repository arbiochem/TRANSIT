using DevExpress.Data.Utils;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TRANSIT;

namespace TRANSIT
{
    public partial class frm_login : Form
    {
        public static string Username { get; private set; }
        public string IDName { get; private set; }
        public string UserRole { get; private set; }

        public bool success;
        public static string leserveur { get; set; }
        public string labase { get; set; }

        public string pwd
        {
            get; set;
        }

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Le mot de passe ne peut pas être vide.");

            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertir le mot de passe en tableau de bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Calculer le hash
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convertir le hash en chaîne de caractères hexadécimale
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2")); // Convertir chaque byte en une valeur hexadécimale
                }

                return hashString.ToString();
            }
        }

        public string Server { get; set; }
        public frm_login()
        {
            InitializeComponent();

            string filePath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "serveurs_connex.txt");
            string[] lines = File.ReadAllLines(filePath);

            // Ajouter chaque ligne au ComboBox
            foreach (string line in lines)
            {
                cboServers.Properties.Items.Add(line);
            }
            //cboServers.SelectedIndex = 0;
            cboServers.EditValueChanged += cboServers_EditValueChanged;

            this.ControlBox = false;
        }

        public string serverName;

        private void cboServers_EditValueChanged_1(object sender, EventArgs e)
        {
            serverName = cboServers.EditValue?.ToString();
            cboServers.SelectedIndexChanged += comboBox_SelectedIndexChanged;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupérer la valeur réelle (realName) du ComboBoxEdit
            string realName = cboServers.EditValue?.ToString();

        }

        private void cboServers_EditValueChanged(object sender, EventArgs e)
        {
            BtnOK.Enabled = !string.IsNullOrEmpty(cboServers.EditValue?.ToString());
        }

        public static string mailuser = "";
        private bool testerAutorisation()
        {
            try
            {
                if (tokenEmail.EditValue == null || tokenEmail.EditValue.ToString() == "")
                    return false;

                string userName = tokenEmail.EditValue.ToString();
                string password = textPwd.Text;

                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Veuillez entrer un nom d'utilisateur et un mot de passe.",
                                    "Erreur",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return false;
                }

                if (!VerifyLogin(userName, password))
                {
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect, ou vérifiez le service réseau.",
                                    "Erreur",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return false; // 🔴 BLOQUE ICI
                }

                using (SqlConnection connection = new SqlConnection(
                       $"Data Source={serverName};Initial Catalog=arbapp;User ID=Dev;Password=1234;TrustServerCertificate=True"))
                {
                    connection.Open();

                    string query = "SELECT id_user, UserGroup, IDName, IsPasswordChanged FROM T_UserRole WHERE UserName = @UserName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Utilisateur introuvable.",
                                                "Erreur",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                                return false;
                            }

                            Guid id_user = Guid.Parse(reader["id_user"].ToString());
                            string userRole = reader["UserGroup"].ToString();
                            string idname = reader["IDName"].ToString();
                            bool isPasswordChanged = (bool)reader["IsPasswordChanged"];

                            Username = userName;
                            UserRole = userRole;
                            IDName = idname;

                            if (!isPasswordChanged)
                            {
                                MessageBox.Show("Vous devez changer votre mot de passe pour la première connexion.");
                                ShowChangePasswordForm(userName);
                                return false; // 🔴 BLOQUE TANT QUE PAS CHANGÉ
                            }

                            ApposerLastDate(serverName, userName);

                            return true; // ✅ LOGIN OK
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}",
                                "Erreur",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }


        private void ShowChangePasswordForm(string userName)
        {
            // Créer un formulaire pour changer le mot de passe
            FrmChangePassword changePasswordForm = new FrmChangePassword(userName);
            changePasswordForm.ShowDialog();
        }


        public static void ApposerLastDate(string serverName, string Username)
        {
            string connectionString = $"Data Source={serverName};Initial Catalog=arbapp;User ID=Dev;Password=1234;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string queryUpdateLastDate = "UPDATE T_UserRole SET LastDateLogon = @LastDateLogon WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(queryUpdateLastDate, connection))
                {
                    command.Parameters.AddWithValue("@LastDateLogon", DateTime.Now); // Hash du nouveau mot de passe
                    command.Parameters.AddWithValue("@Username", Username);

                    command.ExecuteNonQuery();

                }
            }
        }

        private bool VerifyLogin(string username, string inputPassword)
        {
            using (SqlConnection connection = new SqlConnection(
                       $"Data Source={serverName.Replace(";AMBOHIMANGAKELY", "")};Initial Catalog=arbapp;User ID=Dev;Password=1234;TrustServerCertificate=True"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT Password FROM T_UserRole WHERE Username = @UserName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", username);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string storedHashedPassword = result.ToString();

                            string hashedInputPassword = HashPassword(inputPassword);

                            return hashedInputPassword == storedHashedPassword;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MethodBase m = MethodBase.GetCurrentMethod();
                    MessageBox.Show($"Une erreur est survenue :{m}  : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            leserveur = cboServers.EditValue?.ToString();
            Username = tokenEmail.Text;
            pwd = textPwd.Text;

            if (testerAutorisation()) // ✅ seulement si TRUE
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public static void TokenEdit_ValidateToken(object sender, TokenEditValidateTokenEventArgs e)
        {
            if (IsValidEmail(e.Value.ToString()))
            {
                e.IsValid = true;
                e.Description = e.Value.ToString();
            }
            else
            {
                e.IsValid = false;
            }
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void frm_login_Load(object sender, EventArgs e)
        {
            this.AcceptButton = BtnOK;
            LoadEmailsFromDatabase(tokenEmail);
        }

        public static void LoadEmailsFromDatabase(TokenEdit tokenEdit)
        {
            string serverName = File.ReadLines("servermail.txt").FirstOrDefault();

            if (serverName != null)
            {
                Console.WriteLine("Première ligne : " + serverName);
            }
            else
            {
                Console.WriteLine("Le fichier est vide.");
            }

            string connectionString = $"Data Source={serverName};Initial Catalog=arbApp;User ID=Dev;Password=1234;TrustServerCertificate=True";
            string query = "SELECT Username FROM T_UserRole WHERE Username IS NOT NULL";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            tokenEdit.Properties.Tokens.Clear();
                            while (reader.Read())
                            {
                                string email = reader["Username"].ToString();
                                tokenEdit.Properties.Tokens.Add(new TokenEditToken(email, email));
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des emails : {ex.Message} " + Environment.NewLine + "Veuillez réessayer plus tard !");
                //Environment.Exit(0);
            }
        }

        private void BtnOK_Click_1(object sender, EventArgs e)
        {
            leserveur = cboServers.EditValue?.ToString();
            pwd = textPwd.Text;
            if (testerAutorisation()) // ✅ seulement si TRUE
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void tokenEmail_EditValueChanged(object sender, EventArgs e)
        {
            //_labelControl2.Text = tokenEmail.EditValue?.ToString();
        }
    }
}
