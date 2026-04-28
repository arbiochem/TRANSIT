using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using MimeKit;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.XtraEditors.RoundedSkinPanel;

namespace TRANSIT
{
    public partial class frm_principal : DevExpress.XtraEditors.XtraForm
    {

        private string source = "";
        private string destinataire = "";
        string connectionSource = "";
        string connectionDestinataire = "";
        public string connectionSource1 = "";
        public string connectionSource2 = "";
        public string val = "";
        private bool is_email = false;

        private async Task EnvoyerMailAvecMailKit(string filePath, string destinataire)
        {
            try
            {
                // Paramètres spécifiques pour moov.mg
                string smtpServer = "smtpauth.moov.mg";
                int smtpPort = 465; // ou 587 pour TLS
                string username = "rija.razanakoto@arbiochem.mg";
                string password = "LYp@paBIO2400"; // Vérifiez ce mot de passe

                // Créer le message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Mail auto Intégration ME dans SAGE", username));
                message.To.Add(new MailboxAddress("Destinataire", destinataire));
                message.Subject = $"Mail auto Intégration ME dans SAGE";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Intégration ME dans SAGE";
                bodyBuilder.Attachments.Add(filePath);
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Options de debug pour voir ce qui se passe
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Essayer différents ports et méthodes de sécurité
                    MailKit.Security.SecureSocketOptions sslOption = MailKit.Security.SecureSocketOptions.SslOnConnect;

                    await client.ConnectAsync(smtpServer, smtpPort, sslOption);

                    // Essayer différentes méthodes d'authentification
                    try
                    {
                        await client.AuthenticateAsync(username, password);
                        //MessageBox.Show("Authentification réussie !", "Succès",
                                       //MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (System.Security.Authentication.AuthenticationException)
                    {
                        MessageBox.Show("Échec de l'authentification. Vérifiez vos identifiants.",
                                       "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    is_email = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            string dossier = AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(dossier);

            string fichier = Path.Combine(
                dossier,
                "export_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
            );

            // 🔹 Création du fichier TXT
            using (StreamWriter sw = new StreamWriter(fichier, false, Encoding.GetEncoding("Windows-1252")))
            {
                foreach (DataGridViewRow row in dgSource.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        List<string> cells = new List<string>();

                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.ColumnIndex != 7 &&
                                cell.ColumnIndex != 9 &&
                                cell.ColumnIndex != 12)
                            {
                                if (cell.Value is DateTime dateValue)
                                {
                                    cells.Add(dateValue.ToString("yyyyMMdd"));
                                }
                                else
                                {
                                    if (cell.ColumnIndex == 10)
                                        cells.Add("1");
                                    else
                                        cells.Add(cell.Value?.ToString() ?? "");
                                }
                            }
                        }

                        sw.WriteLine(string.Join(";", cells) + ";");
                    }
                }
            }
            
        }

        public frm_principal()
        {
            InitializeComponent();
        }

        public class Serveurs
        {
            public string Ip { get; set; }
            public string Name { get; set; }
        }

        string prot_guid;
        string cbCreation;
        private void frm_principal_Load(object sender, EventArgs e)
        {
            dgSource.DataError += (s, ev) => { ev.Cancel = true; };
            using (frm_login f = new frm_login())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }

            this.Enabled = true;
            load_source();
            load_destinataire();
            btnPrint.AutoSize = true;

            txtTDD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }

        private void cmbBase_DropDownClosed(object sender, EventArgs e)
        {
            
            if (cmbBase.Items.ToString() != "")
            {
                btn_lister.Enabled = true;
            }
            else
            {
                btn_lister.Enabled = false;
            }
           
        }

        private void load_source()
        {
            connectionSource = $"Server={frm_login.leserveur};Database=master;" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            cmbBase.DataSource = null;

            List<string> bases = new List<string>
            {
                "ACTIVO",
                "ACTIVOFEED_ANALAKELY",
                "ACTIVOFEED_ANTANIMORA",
                "ACTIVOFEED_DIEGO_AG",
                "ACTIVOFEED_IMERINTSIATOSIKA",
                "ACTIVOFEED_MAHINTSY",
                "ACTIVOFEED_MAJUNGA",
                "ARBIOCHEM",
                "TRANSIT",
                "TSARAKOHO"

            };

            chargerBdd(connectionSource, cmbBase, bases);
        }

        private void chargerBdd(string conns, System.Windows.Forms.ComboBox cmb, List<string> bases)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conns))
            {
                con.Open();

                var parameters = bases
                    .Select((b, i) => $"@db{i}")
                    .ToArray();

                 string sql = $@"
                SELECT name
                FROM sys.databases
                WHERE database_id > 4
                  AND name NOT IN ('BIJOU','C_Model')
                  AND name IN ({string.Join(",", parameters)})
                ORDER BY name";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    for (int i = 0; i < bases.Count; i++)
                    {
                        cmd.Parameters.AddWithValue(parameters[i], bases[i]);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            cmb.DataSource = dt;
            cmb.DisplayMember = "name";
            cmb.ValueMember = "name";
            cmb.Enabled = dt.Rows.Count > 0;
        }

        private void load_destinataire()
        {
            connectionDestinataire = $"Server={frm_login.leserveur};Database=master;" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            cmbBase1.DataSource = null;

            List<string> bases = new List<string>
            {
                "ACTIVO",
                "ACTIVOFEED_ANALAKELY",
                "ACTIVOFEED_ANTANIMORA",
                "ACTIVOFEED_DIEGO_AG",
                "ACTIVOFEED_IMERINTSIATOSIKA",
                "ACTIVOFEED_MAHINTSY",
                "ACTIVOFEED_MAJUNGA",
                "ARBIOCHEM",
                "TRANSIT",
                "TSARAKOHO"
            };

            chargerBdd(connectionDestinataire, cmbBase1, bases);
        }

        private void btn_lister_Click(object sender, EventArgs e)
        {
            string baseName = cmbBase.SelectedValue?.ToString();
            connectionSource1 = $"Server={frm_login.leserveur};Database={baseName};" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            if (!txtTDD.Text.StartsWith("MS") && !txtTDD.Text.StartsWith("METS"))
            {
                MessageBox.Show("Ce n'est pas un mouvement de sortie!!!!","Message d'erreur",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                load_data();
            }
        }

        bool tester_tdd_flotserie(String cond)
        {
            Boolean b_test = false;

            using (SqlConnection con = new SqlConnection(connectionSource1))
            {
                con.Open();
                string checkQuery = @"
                SELECT COUNT(*) 
                FROM F_LOTSERIE l
                INNER JOIN F_DOCLIGNE doc ON l.DL_NoOut = doc.DL_No AND l.AR_Ref = doc.AR_Ref
                WHERE doc.DO_Piece = @DoPiece";

                using (SqlCommand cmdCheck = new SqlCommand(checkQuery, con))
                {
                    cmdCheck.Parameters.Add("@DoPiece", SqlDbType.VarChar).Value = txtTDD.Text.Trim();
                    int count = (int)cmdCheck.ExecuteScalar();
                    if (count > 0)
                    {
                        b_test = true;
                    }
                }
            }

            return b_test;
        }

        private void load_data()
        {
            dgSource.DataSource = null;
            dgSource.Columns.Clear();
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionSource1))
                {
                    con.Open();

                    string query = @"
                    SELECT DISTINCT
                        doc.DO_Type,
                        doc.DO_Piece,
                        FORMAT(GETDATE(), 'ddMMyy') AS DO_Date,
                        doc.AR_Ref,
                        doc.DL_Design,
                        CAST(doc.DL_Qte AS DECIMAL(24,6)) AS DL_Qte,
                        lot.LS_NoSerie,
                        f.DE_Intitule,
                        lot.LS_PEREMPTION,
                        tete.DO_Tiers,
                        doc.DL_No
                    FROM F_DOCLIGNE AS doc
                    INNER JOIN F_DEPOT AS f ON f.DE_NO = doc.DE_No
                    INNER JOIN  F_DOCENTETE AS tete ON tete.DO_Piece=doc.Do_Piece
                    LEFT JOIN F_LOTSERIE AS lot 
                       ON lot.DL_NoOut = doc.DL_No 
                       AND lot.AR_Ref = doc.AR_Ref
                    WHERE doc.DO_Piece = @DoPiece
                      AND doc.DL_Qte IS NOT NULL
                      AND doc.DL_Qte <> 0 
                      AND (doc.DL_DESIGN <> '' and doc.DL_DESIGN IS NOT NULL)
                    ";
                      

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // 🔐 PARAMÈTRE SÉCURISÉ
                        cmd.Parameters.Add("@DoPiece", SqlDbType.VarChar).Value = txtTDD.Text.Trim();

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt.Clear();
                            da.Fill(dt);
                        }
                    }

                    dgSource.DataSource = dt;

                    dgSource.Columns["DO_Type"].HeaderText = "DO_Type";
                    dgSource.Columns["DO_Type"].Visible = false;
                    dgSource.Columns["DL_No"].Visible = false;
                    dgSource.Columns["DO_Piece"].HeaderText = "N° Pièce";
                    dgSource.Columns["Do_Date"].HeaderText = "Date";
                    dgSource.Columns["AR_Ref"].HeaderText = "Référence";
                    dgSource.Columns["DL_Design"].HeaderText = "Désignation";
                    dgSource.Columns["DL_Qte"].HeaderText = "Quantité";
                    dgSource.Columns["LS_NoSerie"].HeaderText = "Lot";
                    dgSource.Columns["DE_Intitule"].HeaderText = "Dépôt source";
                    dgSource.Columns["LS_PEREMPTION"].HeaderText = "Date de péremption";
                    dgSource.Columns["Do_TIERS"].HeaderText = "TIERS";
                    dgSource.Columns["Do_TIERS"].Visible = false;

                    DataGridViewTextBoxColumn coldest = new DataGridViewTextBoxColumn();
                    coldest.Name = "DEPOT_DEST";
                    coldest.HeaderText = "Dépôt Dest";
                    coldest.Width = 100;
                    coldest.ReadOnly = false;

                    // Vérifier que la colonne de référence existe
                    if (dgSource.Columns.Contains("DE_Intitule"))
                    {
                        int index = dgSource.Columns["DE_Intitule"].Index + 1;
                        dgSource.Columns.Insert(index, coldest);
                    }

                    // Remplir toutes les lignes existantes
                    foreach (DataGridViewRow row in dgSource.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            row.Cells["DEPOT_DEST"].Value = cmbdepot.Text;
                        }
                    }

                    DataGridViewTextBoxColumn colREFERENCES = new DataGridViewTextBoxColumn();
                    colREFERENCES.Name = "REFERENCES";
                    colREFERENCES.HeaderText = "REFERENCES";
                    colREFERENCES.Width = 100;
                    colREFERENCES.Visible = true;
                    dgSource.Columns.Insert(12, colREFERENCES);

                    if (dt.Rows.Count == 0)
                    {
                        btn_lancer.Enabled = false;

                        MessageBox.Show("Aucune donnée trouvée");
                    }
                    else
                    {
                        btn_lancer.Enabled = true;
                        btnPrint.Enabled = true;

                        if (dgSource.Columns.Contains("DL_Qte"))
                        {
                            dgSource.Columns["DL_Qte"].HeaderText = "Quantité";
                            //dgSource.Columns["DL_Qte"].DefaultCellStyle.Format = "N0"; // N0 = nombre avec 0 décimale et séparateur de milliers
                            dgSource.Columns["DL_Qte"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                //MessageBox.Show($"Erreur SQL : {sqlEx.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgSource.Columns["DL_Qte"].ValueType = typeof(string);
        }

        bool tester_lancement(string cond)
        {
            string baseName = cmbBase.SelectedValue?.ToString();
            connectionSource1 = $"Server={frm_login.leserveur};Database={cmbBase1.Text};" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            Boolean b_test = false;
            using (SqlConnection con = new SqlConnection(connectionSource1))
            {
                con.Open();
                string checkQuery = @"
                SELECT COUNT(*) 
                FROM F_DOCENTETE
                WHERE DO_Ref = @DO_Ref";

                using (SqlCommand cmdCheck = new SqlCommand(checkQuery, con))
                {
                    cmdCheck.Parameters.Add("@DO_Ref", SqlDbType.VarChar).Value = txtTDD.Text.Trim();
                    int count = (int)cmdCheck.ExecuteScalar();
                    if (count > 0)
                    {
                        b_test = true;
                    }
                }
            }
            return b_test;
        }

        private void btn_lancer_Click(object sender, EventArgs e)
        {
            cmbdepot.Enabled = false;

            if (!tester_lancement(txtTDD.Text))
            {
                TextEdit textEdit = new TextEdit();
                textEdit.Properties.UseSystemPasswordChar = true; // 👈 mode password
                textEdit.Properties.PasswordChar = '●'; // optionnel
                textEdit.Width = 200;

                // Arguments de l'InputBox
                XtraInputBoxArgs args = new XtraInputBoxArgs();
                args.Caption = "Mot de passe";
                args.Prompt = "Entrez le mot de passe";
                args.DefaultButtonIndex = 0;
                args.Editor = textEdit;

                // Affichage"Lot

                dgSource.Columns[5].ValueType = typeof(decimal);

                if (dgSource.Rows.Count > 0)
                {
                    string recs = "";
                    for (int i = 0; i < dgSource.Rows.Count - 1; i++)
                    {
                        if (i > 0 && i < dgSource.Rows.Count - 1)
                        {
                            dgSource.Rows[i - 1].Selected = false;
                            dgSource.Rows[i].Selected = true;
                        }

                        frm_traitement frm_trait = new frm_traitement(this);
                        frm_trait.Text = "TRAITEMENT DE " + dgSource.Rows[i].Cells[1].Value.ToString();
                        frm_trait.txttype.Text = "20";
                        frm_trait.txtdesignation.Text = dgSource.Rows[i].Cells[4].Value.ToString();
                        //frm_trait.txtqte1.Text = dgSource.Rows[i].Cells[5].Value.ToString();
                        string valBrute = dgSource.Rows[i].Cells[5].Value?.ToString()
                          .Replace(",", ".").Trim();

                                if (decimal.TryParse(valBrute, NumberStyles.Any,
                                                     CultureInfo.InvariantCulture, out decimal val))
                                {
                                    frm_trait.txtqte1.Text = val.ToString("G29", CultureInfo.CurrentCulture); // "98,75"
                                }
                                else
                                {
                                    frm_trait.txtqte1.Text = dgSource.Rows[i].Cells[5].Value?.ToString();
                                }
                        frm_trait.txtreference.Text = dgSource.Rows[i].Cells[3].Value.ToString();
                        frm_trait.txtRefs.Text = txtTDD.Text;
                        frm_trait.txtdepot.Text = cmbBase.Text.ToString();
                        frm_trait.txtdepot1.Text = cmbdepot.Text.ToString();

                        string cellValuelot = dgSource.Rows[i].Cells[6].Value?.ToString();

                        frm_trait.txtLot.Text = !string.IsNullOrWhiteSpace(cellValuelot) ? cellValuelot : "LOT";

                        string cellValueperemption = dgSource.Rows[i].Cells[9].Value?.ToString();

                        frm_trait.txtdateperemption.Text = !string.IsNullOrWhiteSpace(cellValueperemption)
                        ? cellValueperemption
                        : $"{DateTime.Now.Year}-12-31";

                        frm_trait.cbUserCreation.Text = cbCreation;
                        if (i == 0)
                        {
                            frm_trait.txtligne.Text = "ME" + recuperer_last_numero().ToString().PadLeft(5, '0');
                            recs = "ME" + recuperer_last_numero().ToString().PadLeft(5, '0');
                        }
                        else
                        {
                            frm_trait.txtligne.Text = recs;
                        }

                        string cellValues = Convert.ToString(dgSource.Rows[i].Cells[4].Value);
                        string cellValue = Convert.ToString(dgSource.Rows[i].Cells[6].Value);
                        frm_trait.ShowDialog();

                        string[] recup = lblrecup.Text.ToString().Split(';');

                        dgSource.Rows[i].Cells[0].Value = recup[0];
                        dgSource.Rows[i].Cells[1].Value = recup[1];
                        dgSource.Rows[i].Cells[6].Value = recup[3];

                        decimal.TryParse(recup[2].Replace(",", ".").Trim(),
                         NumberStyles.Any,
                         CultureInfo.InvariantCulture,
                         out decimal qtes);
                                dgSource.Rows[i].Cells[5].Value = qtes;

                        dgSource.Rows[i].Cells[12].Value = recup[5];
                        dgSource.Rows[i].Cells[9].Value = recup[6];

                        Application.DoEvents();
                    }
                }

                btn_save.Enabled = true;
            }
            else
            {
                MessageBox.Show("Ce TDD est déjà traité!!!!", "Message d'erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string recuperer_last_numero()
        {
            string rec = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionSource2))
                {
                    con.Open();

                    string query = @"SELECT TOP 1 DC_PIECE FROM [dbo].[F_DOCCURRENTPIECE] WHERE DC_PIECE LIKE 'ME%' ORDER BY DC_PIECE DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            string results = new string(result.ToString().Where(char.IsDigit).ToArray());
                            int k = int.Parse(results);
                            rec = k.ToString();
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Erreur SQL : {sqlEx.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return rec;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string dossier = AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(dossier);

            string fichier = Path.Combine(
                dossier,
                "export_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
            );

            // 🔹 Création du fichier TXT
            using (StreamWriter sw = new StreamWriter(fichier, false, Encoding.GetEncoding("Windows-1252")))
            {
                foreach (DataGridViewRow row in dgSource.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        List<string> cells = new List<string>();
                        string rect = "";

                        // Premier foreach : récupérer rect
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.ColumnIndex == 3)
                            {
                                rect = cell.Value?.ToString() ?? "";
                            }
                        }

                        // Deuxième foreach : construire cells
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            // Ignorer les colonnes 7, 9, 11
                            if (cell.ColumnIndex == 7 || cell.ColumnIndex == 9 || cell.ColumnIndex == 11)
                                continue;

                            // Ignorer colonne 6 si rect commence par "MAT"
                            if (rect.StartsWith("MAT") && cell.ColumnIndex == 6)
                                continue;

                            if (cell.Value is DateTime dateValue)
                            {
                                cells.Add(dateValue.ToString("yyyyMMdd"));
                            }
                            else if (cell.ColumnIndex == 8)
                            {
                                cells.Add(cell.Value?.ToString() ?? "");
                                cells.Add("1"); // ✅ toujours ajouter "1" après colonne 8
                            }
                            else
                            {
                                cells.Add(cell.Value?.ToString() ?? "");
                            }
                        }

                        sw.WriteLine(string.Join(";", cells) + ";");
                    }
                }
            }

            if (is_email)
            {
                MessageBox.Show("E-mail déjà envoyé pour ce document!!!!",
                                       "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // 🔹 Envoi du mail avec la pièce jointe
                EnvoyerMailAvecMailKit(fichier, "todisoa.rakotoarijaona@arbiochem.mg");
                EnvoyerMailAvecMailKit(fichier, "rija.razanakoto@arbiochem.mg");
                EnvoyerMailAvecMailKit(fichier, "mounisse.ali@arbiochem.mg");
                //sEnvoyerMailAvecMailKit(fichier, "glpi@arbiochem.mg");


                MessageBox.Show("Email envoyé avec succès !", "Succès",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);

                maj_numero();
            }
        }

        private void maj_numero()
        {
            string baseName = cmbBase.SelectedValue?.ToString();
            connectionSource1 = $"Server={frm_login.leserveur};Database={cmbBase1.Text};" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            string rect = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionSource1))
                {
                    con.Open();

                    string queryUpdate = @"UPDATE [dbo].[F_DOCCURRENTPIECE]
                          SET DC_PIECE = 'ME' + CAST(CAST(SUBSTRING(DC_PIECE, 3, LEN(DC_PIECE) - 2) AS INT) + 1 AS VARCHAR)
                          WHERE DC_PIECE LIKE 'ME%'";

                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con))
                    {
                        int rowsAffected = cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Erreur SQL : {sqlEx.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTDD_EditValueChanged(object sender, EventArgs e)
        {
            dgSource.Columns.Clear();
            dgSource.DataSource = null;
            btnPrint.Enabled = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dgSource.DataSource == null)
            {
                XtraMessageBox.Show("Aucune donnée à imprimer");
                return;
            }

            PrintDocument printDoc = new PrintDocument();
            PrintPreviewDialog preview = new PrintPreviewDialog();

            printDoc.DefaultPageSettings.Landscape = true;
            printDoc.DefaultPageSettings.Margins = new Margins(30, 30, 40, 40);

            int rowHeight = 28;

            printDoc.PrintPage += (s, ev) =>
            {
                Graphics g = ev.Graphics;
                Rectangle bounds = ev.MarginBounds;

                int xStart = bounds.Left;
                int y = bounds.Top;
                int x;

                Font headerFont = new Font("Arial", 10, FontStyle.Bold);
                Font cellFont = new Font("Arial", 9);

                StringFormat centerFormat = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.NoWrap
                };

                // Largeur totale des colonnes visibles
                int totalWidth = dgSource.Columns
                    .Cast<DataGridViewColumn>()
                    .Where(c => c.Visible)
                    .Sum(c => c.Width);

                float scale = (float)bounds.Width / totalWidth;

                // ============ HEADERS ============
                x = xStart;
                foreach (DataGridViewColumn col in dgSource.Columns)
                {
                    if (!col.Visible) continue;

                    int w = (int)(col.Width * scale);
                    Rectangle rect = new Rectangle(x, y, w, rowHeight);

                    g.FillRectangle(Brushes.LightGray, rect);
                    g.DrawRectangle(Pens.Black, rect);
                    g.DrawString(col.HeaderText, headerFont, Brushes.Black, rect, centerFormat);

                    x += w;
                }

                y += rowHeight;

                // ============ ROWS ============
                foreach (DataGridViewRow row in dgSource.Rows)
                {
                    if (row.IsNewRow) continue;

                    x = xStart;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (!cell.OwningColumn.Visible) continue;

                        int w = (int)(cell.OwningColumn.Width * scale);
                        Rectangle rect = new Rectangle(x, y, w, rowHeight);

                        g.DrawRectangle(Pens.Black, rect);
                        g.DrawString(
                            cell.Value?.ToString() ?? "",
                            cellFont,
                            Brushes.Black,
                            rect,
                            centerFormat
                        );

                        x += w;
                    }

                    y += rowHeight;

                    // Pagination verticale
                    if (y + rowHeight > bounds.Bottom)
                    {
                        ev.HasMorePages = true;
                        return;
                    }
                }

                ev.HasMorePages = false;
            };

            preview.Document = printDoc;
            preview.WindowState = FormWindowState.Maximized;
            preview.ShowDialog();
        }

        private void cmbBase1_DropDownClosed(object sender, EventArgs e)
        {
            cmbdepot.Enabled = true;

            string baseName1 = cmbBase1.SelectedValue?.ToString();
            connectionSource2 = $"Server={frm_login.leserveur};Database={baseName1};" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            loaddepot();
        }

        private void loaddepot()
        {
            cmbdepot.DataSource = null;
            DataTable dt = new DataTable();


            string connectionString = $"Server={frm_login.leserveur};Database=ARBIOCHEM;" +
                                                     $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                     $"Connection Timeout=240;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"
                SELECT 
                    REPLACE(PROT_Guid, '{', '') as PROT_Guid,
                    cbCreationUser 
                FROM F_PROTECTIONCIAL 
                WHERE PROT_EMail = @usermail", conn))
                {
                    cmd.Parameters.Add("@usermail", SqlDbType.NVarChar, 256).Value = frm_login.Username;

                    conn.Open(); // ✅ OBLIGATOIRE - Décommentez cette ligne !

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            prot_guid = reader["PROT_Guid"]?.ToString() ?? string.Empty;
                            cbCreation = reader["cbCreationUser"]?.ToString() ?? string.Empty;
                        }
                    }
                }
            }

            cmbdepot.Items.Clear();
            cmbdepot.Items.Add("");

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "select DISTINCT d.DE_Intitule from F_DEPOT as d INNER JOIN F_DEPOT_DEDIE as fd on d.DE_NO=fd.DE_No WHERE fd.PROT_Guid=@prodguid AND fd.AUTHORIZED=1 ORDER BY d.DE_Intitule ASC";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@prodguid", SqlDbType.NVarChar, 256).Value = prot_guid;

                        // Exécuter la requête et lire les résultats
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            cmbdepot.Items.Clear(); // Vider d'abord
                            cmbdepot.Items.Add(""); // Ajouter une ligne vide

                            while (reader.Read())
                            {
                                cmbdepot.Items.Add(reader["DE_Intitule"].ToString());
                            }
                        }
                    }
                }

                cmbdepot.SelectedIndex = 0;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Erreur SQL : {sqlEx.Message}\n\nDétails: {sqlEx.Number}",
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbdepot_DropDownClosed(object sender, EventArgs e)
        {
            if(cmbdepot.Text != "")
            {
                txtTDD.Enabled = true;
            }
        }
    }
}
