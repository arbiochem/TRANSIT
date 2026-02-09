using DevExpress.XtraEditors;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRANSIT
{
    public partial class FrmChangePassword : DevExpress.XtraEditors.XtraForm
    {
        private string userName;
        public FrmChangePassword(string userName)
        {
            InitializeComponent();
            this.userName = userName;
        }

        public  string leserveur = frm_login.leserveur;

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

        private void btnChangePassword_Click_1(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirm.Text;
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas. Veuillez réessayer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Vérifier si le nouveau mot de passe est "1234"
            if (newPassword == "1234")
            {
                MessageBox.Show("Le mot de passe ne peut pas être '1234'. Veuillez choisir un mot de passe plus sécurisé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Veuillez entrer un nouveau mot de passe.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsPasswordComplexEnough(newPassword))
            {
                MessageBox.Show("Le mot de passe doit contenir au moins 8 caractères.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Vérifier que le mot de passe n'est pas vide
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Veuillez entrer un nouveau mot de passe.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Hashage du nouveau mot de passe
            string hashedNewPassword = HashPassword(newPassword);

            using (SqlConnection connection = new SqlConnection(
                       $"Data Source={leserveur};Initial Catalog=arbapp;User ID=Dev;Password=1234;TrustServerCertificate=True"))
            {
                try
                {
                    connection.Open();

                    // Mettre à jour le mot de passe et changer le statut
                    string updateQuery = "UPDATE T_UserRole SET Password = @Password, IsPasswordChanged = 1 WHERE UserName = @UserName";
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Password", hashedNewPassword);
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Mot de passe changé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Fermer le formulaire de changement de mot de passe
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool IsPasswordComplexEnough(string password)
        {
            // Vérifier la longueur minimale
            if (password.Length < 8)
            {
                return false;
            }

            //// Vérifier la présence d'une majuscule
            //if (!password.Any(char.IsUpper))
            //{
            //    return false;
            //}

            //// Vérifier la présence d'une minuscule
            //if (!password.Any(char.IsLower))
            //{
            //    return false;
            //}

            //// Vérifier la présence d'un chiffre
            //if (!password.Any(char.IsDigit))
            //{
            //    return false;
            //}

            //// Vérifier la présence d'un caractère spécial
            //if (!password.Any(c => !char.IsLetterOrDigit(c)))
            //{
            //    return false;
            //}

            return true;
        }
    }
}