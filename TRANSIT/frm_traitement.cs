using DevExpress.Utils.Serializing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRANSIT
{
    public partial class frm_traitement : Form
    {
        private frm_principal _frmParent;
        string lotserie = "";
        string val = "";
        public frm_traitement(frm_principal frmParent)
        {
            InitializeComponent();
            _frmParent = frmParent;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtqte1.Text))
            {
                MessageBox.Show("La quantité ne peut pas être vide !!!!","Message d'erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            else
            {
                //Insertion dans F_LotSerie
                if (txtLot.Text == "")
                {
                    txtLot.Text = "LOT";
                }


                if (txtLot.Text != "")
                {
                    lotserie = recuperer_lotserie();

                    try
                    {
                        using (SqlConnection con = new SqlConnection(_frmParent.connectionSource2))
                        {
                            con.Open();

                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.Connection = con;

                                if (!decimal.TryParse(
                                    txtqte1.Text.Trim(),
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out decimal qte))
                                    {
                                        MessageBox.Show("Quantité invalide");
                                        return;
                                    }

                                if (_frmParent.val == "")
                                {
                                    cmd.CommandText = @"DELETE FROM F_RECUP";
                                    cmd.ExecuteNonQuery();
                                    _frmParent.val = "1";
                                }

                                int depot = 0;
                                depot=recuperer_depots(txtdepot1.Text);
                                cmd.CommandText = @"
                            INSERT INTO F_RECUP
                            (AR_Ref, LS_Peremption, LS_Lot,depot)
                            VALUES
                            (@AR_Ref, @Peremption, @LS_Lot,@depot)";

                                cmd.Parameters.AddWithValue("@AR_Ref", txtreference.Text);
                                cmd.Parameters.AddWithValue("@Peremption", Convert.ToDateTime(txtdateperemption.Text));
                                cmd.Parameters.AddWithValue("@LS_Lot", txtLot.Text);
                                cmd.Parameters.Add("@depot", SqlDbType.Int).Value = depot;

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show($"Erreur SQL : {sqlEx.Message}", "Erreur SQL",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur : {ex.Message}", "Erreur",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                _frmParent.lblrecup.Text = "";
                _frmParent.lblrecup.Text = txttype.Text + ";" + txtligne.Text + ";" + txtqte1.Text + ";"+txtLot.Text+";"+ lotserie+";"+txtRefs.Text+";"+txtdateperemption.Text;
                this.Close();
            }
        }

        private int recuperer_depots(string cond)
        {
            int rec = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(_frmParent.connectionSource2))
                {
                    con.Open();

                    string query = @"SELECT DE_No FROM [dbo].[F_DEPOT] WHERE DE_Intitule=@de_intitule";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@de_intitule",cond);

                        object result = cmd.ExecuteScalar();

                        rec = int.Parse(result.ToString());
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
        private string recuperer_lotserie()
        {
            string rec = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_frmParent.connectionSource2))
                {
                    con.Open();

                    string query = @"SELECT MAX(CAST(LotSerie AS INT)) FROM [dbo].[F_LotSerie]";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        object result = cmd.ExecuteScalar();
                        int k;

                        if (result != null && int.TryParse(result.ToString(), out k))
                        {
                            rec = (k + 1).ToString();
                        }
                        else
                        {
                            rec = "1";
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

        private int recuperer_depot(string cond)
        {
            int rec = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(_frmParent.connectionSource1))
                {
                    con.Open();

                    string query = @"SELECT DE_No FROM [dbo].[F_Depot] WHERE DE_Intitule LIKE '%"+cond+"%'";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int k = int.Parse(result.ToString());
                            rec = k;
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

        private void frmLotSerie_Load(object sender, EventArgs e)
        {
            val = "1";
            this.ControlBox = false;
        }
    }
}
