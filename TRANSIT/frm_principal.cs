using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using static DevExpress.XtraEditors.RoundedSkinPanel;
using PdfSharp.Drawing;
using DevExpress.Utils.Extensions;
using DevExpress.ClipboardSource.SpreadsheetML;

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

        public frm_principal()
        {
            InitializeComponent();
        }

        public class Serveurs
        {
            public string Ip { get; set; }
            public string Name { get; set; }
        }

        private void frm_principal_Load(object sender, EventArgs e)
        {
            btnPrint.AutoSize = true;
            drpsource.DataSource=null;
            drpdestinataire.DataSource = null;

            List<Serveurs> serveur_source = new List<Serveurs>()
            {
                //new Serveurs { Ip = "26.53.123.231", Name = "ARBIOCHEM" },
                new Serveurs { Ip = "192.168.88.162", Name = "Localhost" },
                new Serveurs { Ip = "SRV-ARB", Name = "ARBIOCHEM" },
                new Serveurs { Ip = "26.71.34.164", Name = "TAMATAVE" },
                new Serveurs { Ip = "26.16.25.130", Name = "ANALAKELY" }
            };

            List<Serveurs> serveur_dest = new List<Serveurs>()
            {
                new Serveurs { Ip = "192.168.88.162", Name = "Localhost" },
                new Serveurs { Ip = "26.53.123.231", Name = "ARBIOCHEM" },
                new Serveurs { Ip = "26.71.34.164", Name = "TAMATAVE" },
                new Serveurs { Ip = "26.16.25.130", Name = "ANALAKELY" }
            };

            drpsource.DataSource = serveur_source;
            drpsource.DisplayMember = "Name";   // affiché
            drpsource.ValueMember = "Ip";       // valeur

            drpdestinataire.DataSource = serveur_dest;
            drpdestinataire.DisplayMember = "Name";   // affiché
            drpdestinataire.ValueMember = "Ip";       // valeur

            drpdestinataire.Refresh();
            drpsource.Refresh();

            drpsource_DropDownClosed(sender, e);
            drpdestinataire_DropDownClosed(sender, e);

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

        private void drpsource_DropDownClosed(object sender, EventArgs e)
        {
            string selectedIp = drpsource.SelectedValue?.ToString();

            // Ou si vous voulez l'objet complet
            var selectedVpn = drpsource.SelectedItem as Serveurs; // Remplacez VpnObject par votre type
            if (selectedVpn != null)
            {
                string ip = selectedVpn.Ip;
                source = ip;
            }
            connectionSource = $"Server={source};Database=master;" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            cmbBase.DataSource = null;
            chargerBdd(connectionSource,cmbBase);
            
        }

        private void chargerBdd(string conns,System.Windows.Forms.ComboBox cmb)
        {
            SqlConnection con = null;

            DataTable dt = new DataTable();

            using ( con= new SqlConnection(conns))
            {
                con.Open();
                //SqlDataAdapter da = new SqlDataAdapter("SELECT name FROM sys.databases WHERE database_id > 4 and name NOT IN('BIJOU','C_Model') ORDER BY name", con);
                SqlDataAdapter da = new SqlDataAdapter("SELECT name FROM sys.databases WHERE database_id > 4 and name NOT IN('C_Model') ORDER BY name", con);
                da.Fill(dt);
            }


            cmb.DataSource = dt;
            cmb.DisplayMember = "Name";   // affiché
            cmb.ValueMember = "Name";       // valeur

            cmb.Refresh();

            if (dt.Rows.Count > 0)
            {
                cmb.Enabled = true;
            }
            else
            {
                cmb.Enabled = false;
            }

            con.Close();
        }

        private void drpdestinataire_DropDownClosed(object sender, EventArgs e)
        {
            string selectedIp = drpdestinataire.SelectedValue?.ToString();

            // Ou si vous voulez l'objet complet
            var selectedVpn = drpdestinataire.SelectedItem as Serveurs; // Remplacez VpnObject par votre type
            if (selectedVpn != null)
            {
                string ip = selectedVpn.Ip;
                destinataire = ip;
            }
            connectionDestinataire = $"Server={destinataire};Database=master;" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            cmbBase1.DataSource = null;
            chargerBdd(connectionDestinataire, cmbBase1);
        }

        private void btn_lister_Click(object sender, EventArgs e)
        {
            string baseName = cmbBase.SelectedValue?.ToString();
            connectionSource1 = $"Server={source};Database={baseName};" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            if (!txtTDD.Text.StartsWith("MS"))
            {
                MessageBox.Show("Ce n'est pas un mouvement de sortie!!!!","Message d'erreur",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                load_data();
            }
        }

        private void load_data()
        {
            dgSource.DataSource = null;
            DataTable dt = new DataTable();


            try
            {
                using (SqlConnection con = new SqlConnection(connectionSource1))
                {
                    con.Open();

                    string query = @"
                    SELECT 
                        doc.DO_Type,
                        doc.DO_Piece,
                        FORMAT(GETDATE(), 'ddMMyy') AS DO_Date,
                        doc.AR_Ref,
                        doc.DL_Design,
                        CAST(doc.DL_Qte AS INT) AS DL_Qte,
                        lot.LS_NoSerie,
                        f.DE_Intitule,
                        lot.LS_PEREMPTION,
                        tete.DO_Tiers
                    FROM F_DOCLIGNE AS doc
                    INNER JOIN F_DEPOT AS f ON f.DE_NO = doc.DE_No
                    INNER JOIN  F_DOCENTETE AS tete ON tete.DO_Piece=doc.Do_Piece
                    LEFT JOIN F_LOTSERIE AS lot 
                        ON lot.DL_NoIn = doc.DL_No 
                       AND lot.AR_Ref = doc.AR_Ref
                    WHERE doc.DO_Piece = @DoPiece
                      AND doc.DL_Qte IS NOT NULL
                      AND doc.DL_Qte <> 0";

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
                }

                dgSource.DataSource = dt;

                dgSource.Columns["DO_Type"].HeaderText = "DO_Type";
                dgSource.Columns["DO_Type"].Visible = false;
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
                        row.Cells["DEPOT_DEST"].Value = cmbdepot.SelectedValue;
                    }
                }

                if (!dgSource.Columns.Contains("LOTSERIE"))
                {
                    DataGridViewTextBoxColumn colLot = new DataGridViewTextBoxColumn();
                    colLot.Name = "LOTSERIE";
                    colLot.HeaderText = "LOTSERIE";
                    colLot.Width = 100;
                    colLot.Visible = false;   // ou true selon ton besoin

                    // Vérifier que la colonne de référence existe
                    if (dgSource.Columns.Contains("LS_PEREMPTION"))
                    {
                        int index = dgSource.Columns["LS_PEREMPTION"].Index+2;
                        dgSource.Columns.Insert(index, colLot);
                    }
 
                }

                if (dt.Rows.Count == 0)
                {
                    btn_lancer.Enabled = false;

                    MessageBox.Show("Aucune donnée trouvée pour la période sélectionnée.");
                }
                else
                {
                    btn_lancer.Enabled = true;
                    btnPrint.Enabled = true;

                    if (dgSource.Columns.Contains("DL_Qte"))
                    {
                        dgSource.Columns["DL_Qte"].HeaderText = "Quantité";
                        dgSource.Columns["DL_Qte"].DefaultCellStyle.Format = "N0"; // N0 = nombre avec 0 décimale et séparateur de milliers
                        dgSource.Columns["DL_Qte"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
        }

        private void btn_lancer_Click(object sender, EventArgs e)
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

            // Affichage
            string mdp = XtraInputBox.Show(args) as string;

            // Vérification
            while (string.IsNullOrEmpty(mdp))
            {
                XtraMessageBox.Show(
                    "Mot de passe requis",
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                mdp = XtraInputBox.Show(args) as string;
            }

            while (mdp != "P@ssw0rd2026*")
            {
                XtraMessageBox.Show(
                    "Mot de passe incorrect!!!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                mdp = XtraInputBox.Show(args) as string;
            }

            if (dgSource.Rows.Count > 0)
            {
                string recs = "";
                for (int i = 0; i < dgSource.Rows.Count-1; i++)
                {
                    if (i > 0 && i < dgSource.Rows.Count-1)
                    {
                        dgSource.Rows[i - 1].Selected = false;
                        dgSource.Rows[i].Selected = true;
                    }

                    frm_traitement frm_trait = new frm_traitement(this);
                    frm_trait.Text = "TRAITEMENT DE " + dgSource.Rows[i].Cells[1].Value.ToString();
                    frm_trait.txttype.Text = "20";
                    frm_trait.txtdesignation.Text = dgSource.Rows[i].Cells[4].Value.ToString();
                    frm_trait.txtqte1.Text = dgSource.Rows[i].Cells[5].Value.ToString();
                    frm_trait.txtreference.Text = dgSource.Rows[i].Cells[3].Value.ToString();
                    frm_trait.txtdepot.Text = dgSource.Rows[i].Cells[7].Value.ToString();
                    frm_trait.txtdepot1.Text = dgSource.Rows[i].Cells[8].Value.ToString();
                    if (i == 0)
                    {
                        frm_trait.txtligne.Text = "ME" + recuperer_last_numero(i + 1).ToString().PadLeft(5, '0');
                        recs= "ME" + recuperer_last_numero(i + 1).ToString().PadLeft(5, '0');
                    }
                    else
                    {
                        frm_trait.txtligne.Text = recs;
                    }
                        
                    string cellValues = Convert.ToString(dgSource.Rows[i].Cells[4].Value);
                    string cellValue = Convert.ToString(dgSource.Rows[i].Cells[6].Value);

                    if (!cellValues.StartsWith("MAT")){
                        if (string.IsNullOrWhiteSpace(cellValue))
                        {
                            frm_trait.txtLot.Text = "LOT" +
                                new string(txtTDD.Text.Where(char.IsDigit).ToArray());
                        }
                        else
                        {
                            frm_trait.txtLot.Text = cellValue;
                        }
                    }
                    frm_trait.ShowDialog();

                    string[] recup = lblrecup.Text.ToString().Split(';');

                    dgSource.Rows[i].Cells[0].Value = recup[0];
                    dgSource.Rows[i].Cells[1].Value = recup[1];
                    dgSource.Rows[i].Cells[5].Value = recup[2];
                    dgSource.Rows[i].Cells[6].Value = recup[3];
                    dgSource.Rows[i].Cells[11].Value = recup[4];

                    Application.DoEvents();
                }
            }

            btn_save.Enabled = true;
        }

        private string recuperer_last_numero(int cond)
        {
            string rec = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionSource2))
                {
                    con.Open();

                    string query = @"SELECT TOP 1 DO_PIECE FROM [dbo].[F_DOCENTETE] WHERE DO_PIECE LIKE 'ME%' ORDER BY DO_Piece DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            string results = new string(result.ToString().Where(char.IsDigit).ToArray());
                            int k = int.Parse(results)+cond;
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
            TextEdit textEdit = new TextEdit();
            textEdit.Properties.UseSystemPasswordChar = true; // 👈 mode password
            textEdit.Properties.PasswordChar = '●'; // optionnel
            textEdit.Width = 200;

            // Arguments de l'InputBox
            /*XtraInputBoxArgs args = new XtraInputBoxArgs();
            args.Caption = "Mot de passe";
            args.Prompt = "Entrez le mot de passe";
            args.DefaultButtonIndex = 0;
            args.Editor = textEdit;

            // Affichage
            string mdp = XtraInputBox.Show(args) as string;

            // Vérification
            while (string.IsNullOrEmpty(mdp))
            {
                XtraMessageBox.Show(
                    "Mot de passe requis",
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                mdp = XtraInputBox.Show(args) as string;
            }

            while (mdp!="P@ssw0rd2026*")
            {
                XtraMessageBox.Show(
                    "Mot de passe incorrect!!!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                mdp = XtraInputBox.Show(args) as string;
            }*/

            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Fichier texte (*.txt)|*.txt";
                saveDialog.Title = "Exporter vers TXT";
                saveDialog.FileName = "export_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveDialog.FileName, false, Encoding.GetEncoding("Windows-1252")))
                    {
                        // Écrire les données
                        foreach (DataGridViewRow row in dgSource.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                List<string> cells = new List<string>();
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    if (cell.ColumnIndex != 7)
                                    {
                                        if (cell.Value is DateTime dateValue)
                                        {
                                            // Format YYmmdd
                                            cells.Add(dateValue.ToString("yyyyMMdd"));
                                        }
                                        else
                                        {
                                            if (cell.ColumnIndex == 10)
                                            {
                                                cells.Add("1");
                                            }
                                            else
                                            {
                                                cells.Add(cell.Value?.ToString() ?? "");
                                            }
                                        }
                                    }
                                }
                                sw.WriteLine(string.Join(";", cells));
                            }
                        }
                    }

                    MessageBox.Show("Export réussi !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'export : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            connectionSource2 = $"Server={destinataire};Database={baseName1};" +
                                                    $"User ID=Dev;Password=1234;TrustServerCertificate=True;" +
                                                    $"Connection Timeout=240;";

            loaddepot();
        }

        private void loaddepot()
        {
            cmbdepot.DataSource = null;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionSource2))
                {
                    con.Open();
                    string query = "SELECT DISTINCT DE_Intitule FROM F_DEPOT ORDER BY DE_Intitule ASC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                // Ajouter une ligne vide au début (optionnel)
                DataRow emptyRow = dt.NewRow();
                emptyRow["DE_Intitule"] = "";
                dt.Rows.InsertAt(emptyRow, 0);

                // Lier au ComboBox
                cmbdepot.DataSource = dt;
                cmbdepot.DisplayMember = "DE_Intitule";  // Ce qui s'affiche
                cmbdepot.ValueMember = "DE_Intitule";    // La valeur récupérée

                // Optionnel : Sélectionner le premier élément
                if (cmbdepot.Items.Count > 0)
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
