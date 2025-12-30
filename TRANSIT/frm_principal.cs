using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static DevExpress.XtraEditors.RoundedSkinPanel;
using DevExpress.XtraEditors;

namespace TRANSIT
{
    public partial class frm_principal : DevExpress.XtraEditors.XtraForm
    {

        private string source = "";
        private string destinataire = "";
        string connectionSource = "";
        string connectionDestinataire = "";
        string connectionSource1 = "";

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
            drpsource.DataSource=null;
            drpdestinataire.DataSource = null;

            List<Serveurs> vpnList = new List<Serveurs>()
            {
                //new Serveurs { Ip = "26.53.123.231", Name = "ARBIOCHEM" },
                new Serveurs { Ip = "SRV-ARB", Name = "ARBIOCHEM" },
                new Serveurs { Ip = "26.71.34.164", Name = "TAMATAVE" },
                new Serveurs { Ip = "26.16.25.130", Name = "ANALAKELY" }
            };

            List<Serveurs> vpnLists = new List<Serveurs>()
            {
                new Serveurs { Ip = "26.53.123.231", Name = "ARBIOCHEM" },
                new Serveurs { Ip = "26.71.34.164", Name = "TAMATAVE" },
                new Serveurs { Ip = "26.16.25.130", Name = "ANALAKELY" }
            };

            drpsource.DataSource = vpnList;
            drpsource.DisplayMember = "Name";   // affiché
            drpsource.ValueMember = "Ip";       // valeur

            drpdestinataire.DataSource = vpnLists;
            drpdestinataire.DisplayMember = "Name";   // affiché
            drpdestinataire.ValueMember = "Ip";       // valeur

            drpdestinataire.Refresh();
            drpsource.Refresh();

            //drpsource_DropDownClosed(sender, e);
            //drpdestinataire_DropDownClosed(sender, e);
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
                SqlDataAdapter da = new SqlDataAdapter("SELECT name FROM sys.databases WHERE database_id > 4 and name NOT IN('BIJOU','C_Model') ORDER BY name", con);
                da.Fill(dt);
            }


            cmb.DataSource = dt;
            cmb.DisplayMember = "Name";   // affiché
            cmb.ValueMember = "Name";       // valeur

            cmb.Refresh();

            if (dt.Rows.Count > 0)
            {
                cmb.Enabled = true;
                dtDate1.Enabled = true;
                dtDate2.Enabled = true;
            }
            else
            {
                cmb.Enabled = false;
                dtDate1.Enabled = false;
                dtDate2.Enabled = false;
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
            load_data();
        }

        private void load_data()
        {
            dgSource.DataSource = null;
            DataTable dt = new DataTable();

            DateTime dt1 = dtDate1.Value;
            DateTime dt2 = dtDate2.Value;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionSource1))
                {
                    con.Open();

                    /*string query = @"SELECT doc.DO_Type,doc.DO_Piece, doc.Do_Date, doc.AR_Ref, FORMAT(doc.DL_Qte, 'N0') AS DL_Qte, doc.DL_Design, f.DE_Intitule ,lot.LS_PEREMPTION
                        FROM F_DOCLIGNE AS doc 
                        INNER JOIN F_DEPOT AS f ON f.DE_NO = doc.DE_No 
                        LEFT JOIN F_LOTSERIE AS lot ON lot.DL_NoIn = doc.DL_No AND lot.AR_Ref = doc.AR_Ref
                        WHERE doc.DO_Type = 21 
                          AND doc.DO_Piece LIKE 'MS%' 
                          AND doc.DO_DATE BETWEEN @DateDebut AND @DateFin 
                          AND doc.DL_Qte IS NOT NULL AND doc.DL_Qte <> 0
                        ORDER BY doc.Do_Piece ASC";*/

                    string query = @"SELECT doc.DO_Type,doc.DO_Piece, doc.Do_Date, doc.AR_Ref, FORMAT(doc.DL_Qte, 'N0') AS DL_Qte, doc.DL_Design, f.DE_Intitule
                        FROM F_DOCLIGNE AS doc 
                        INNER JOIN F_DEPOT AS f ON f.DE_NO = doc.DE_No 
                        WHERE doc.DO_Type = 21 
                          AND doc.DO_Piece LIKE 'MS%' 
                          AND doc.DO_DATE BETWEEN @DateDebut AND @DateFin 
                          AND doc.DL_Qte IS NOT NULL AND doc.DL_Qte <> 0
                        ORDER BY doc.Do_Piece ASC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Ajout des paramètres
                        cmd.Parameters.AddWithValue("@DateDebut", dt1.Date);
                        cmd.Parameters.AddWithValue("@DateFin", dt2.Date);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                dgSource.DataSource = dt;

                dgSource.Columns["DO_Type"].HeaderText = "DO_Type";
                dgSource.Columns["DO_Piece"].HeaderText = "N° Pièce";
                dgSource.Columns["Do_Date"].HeaderText = "Date";
                dgSource.Columns["AR_Ref"].HeaderText = "Référence";
                dgSource.Columns["DL_Qte"].HeaderText = "Quantité";
                dgSource.Columns["DL_Design"].HeaderText = "Désignation";
                dgSource.Columns["DE_Intitule"].HeaderText = "Dépôt";
                //dgSource.Columns["LS_PEREMPTION"].HeaderText = "Date de péremption";

                DataGridViewTextBoxColumn colLot = new DataGridViewTextBoxColumn();
                colLot.Name = "LOT";
                colLot.HeaderText = "LOT";
                colLot.Width = 100; // Ajustez la largeur selon vos besoins

                // Insérer la colonne après "Quantité" (à l'index de DL_Design)
                int indexDesignation = dgSource.Columns["DL_Design"].Index;
                dgSource.Columns.Insert(indexDesignation, colLot);

                if (dt.Rows.Count == 0)
                {
                    btn_lancer.Enabled = false;
                    btn_save.Enabled = false;
                    ckTous.Enabled = false;

                    MessageBox.Show("Aucune donnée trouvée pour la période sélectionnée.");
                }
                else
                {
                    ckTous.Enabled = true;
                    btn_lancer.Enabled = true;
                    btn_save.Enabled = true;

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
            int total = dgSource.Rows.Count-1;
            progressBarControl1.Properties.Minimum = 0;
            progressBarControl1.Properties.Maximum = total; // nombre total de lignes
            progressBarControl1.Position = 0; // départ

            if (dgSource.Rows.Count > 0)
            {
                progressBarControl1.Enabled = true;
                if (ckTous.Checked)
                {
                    string lot = XtraInputBox.Show(
                        "Saisir le lot",
                        "Lot",
                        ""
                    );

                    if (string.IsNullOrWhiteSpace(lot))
                    {
                        XtraMessageBox.Show(
                            "Le lot est obligatoire",
                            "Attention",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        lot = XtraInputBox.Show(
                        "Saisir le lot",
                        "Lot",
                        ""
                        ); 
                    }

                    for (int i = 0; i < dgSource.Rows.Count-1; i++)
                    {
                        if (i > 0 && i < dgSource.Rows.Count-1)
                        {
                            dgSource.Rows[i - 1].Selected = false;
                            dgSource.Rows[i].Selected = true;
                        }

                        dgSource.Rows[i].Cells[0].Value = 20;
                        dgSource.Rows[i].Cells[1].Value = "ME" + recuperer_last_numero(i + 1);
                        dgSource.Rows[i].Cells[5].Value = lot;

                        progressBarControl1.Position = i + 1;
                        Application.DoEvents();
                    }

                    progressBarControl1.Enabled = false;
                    progressBarControl1.Position = 0;

                    dgSource.Refresh();
                }
                else
                {
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
                        frm_trait.txtdesignation.Text = dgSource.Rows[i].Cells[6].Value.ToString();
                        frm_trait.txtqte1.Text = dgSource.Rows[i].Cells[4].Value.ToString();
                        frm_trait.txtreference.Text = dgSource.Rows[i].Cells[3].Value.ToString();
                        frm_trait.txtdepot.Text = dgSource.Rows[i].Cells[7].Value.ToString();
                        frm_trait.txtligne.Text = "ME" + recuperer_last_numero(i + 1);
                        frm_trait.txtLigne1.Text = i.ToString();
                        frm_trait.ShowDialog();

                        string[] recup = lblrecup.Text.ToString().Split(';');

                        dgSource.Rows[i].Cells[0].Value = recup[0];
                        dgSource.Rows[i].Cells[1].Value = recup[1];
                        dgSource.Rows[i].Cells[4].Value = recup[2];
                        dgSource.Rows[i].Cells[5].Value = recup[3];

                        progressBarControl1.Position = i + 1;
                        Application.DoEvents();
                    }

                    progressBarControl1.Enabled = false;
                    progressBarControl1.Position = 0;
                }
            }
        }

        private string recuperer_last_numero(int cond)
        {
            string rec = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionSource1))
                {
                    con.Open();

                    string query = @"SELECT TOP 1 DO_PIECE FROM [dbo].[F_DOCENTETE] WHERE DO_PIECE LIKE 'ME%' ORDER BY DO_Piece DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int k = int.Parse(result.ToString().Replace("ME", "").Trim())+cond;
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
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Fichier texte (*.txt)|*.txt";
                saveDialog.Title = "Exporter vers TXT";
                saveDialog.FileName = "export_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveDialog.FileName, false, Encoding.UTF8))
                    {
                        // Écrire les données
                        foreach (DataGridViewRow row in dgSource.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                List<string> cells = new List<string>();
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    if (cell.OwningColumn.Visible)
                                    {
                                        if (cell.Value is DateTime dateValue)
                                        {
                                            // Format YYmmdd
                                            cells.Add(dateValue.ToString("yyyyMMdd"));
                                        }
                                        else
                                        {
                                            cells.Add(cell.Value?.ToString() ?? "");
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
    }
}
