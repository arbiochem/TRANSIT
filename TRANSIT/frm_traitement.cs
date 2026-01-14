using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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

        public frm_traitement(frm_principal frmParent)
        {
            InitializeComponent();
            _frmParent = frmParent;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtqte1.Text) || string.IsNullOrWhiteSpace(txtLot.Text))
            {
                MessageBox.Show("La quantité ou le nom du lot ne peut pas être vide !!!!","Message d'erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            else
            {
                //Insertion dans F_LotSerie

                lotserie = recuperer_lotserie();

                try
                {
                    using (SqlConnection con = new SqlConnection(_frmParent.connectionSource2))
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = con;

                            /*cmd.CommandText = @"
                                DISABLE TRIGGER TG_CBUPD_F_LOTSERIE ON F_LotSerie;
                                DISABLE TRIGGER TG_UPD_F_LOTSERIE ON F_LotSerie;";

                            cmd.ExecuteNonQuery();

                            if (txtdateperemption.Text == "")
                            {
                                txtdateperemption.Text = "2000 -01-01";
                            }

                            cmd.CommandText = @"
                            UPDATE F_LotSerie
                            SET LotSerie = @LotSerie
                            WHERE
                            Ar_Ref=@AR_Ref AND DL_NoOut=@DL_No";

                            cmd.Parameters.AddWithValue("@AR_Ref", txtreference.Text);
                            cmd.Parameters.AddWithValue("@DL_No", lbldlnoout.Text);
                            cmd.Parameters.AddWithValue("@LotSerie", lotserie);

                            cmd.ExecuteNonQuery();

                            cmd.CommandText = @"
                                ENABLE TRIGGER TG_CBUPD_F_LOTSERIE ON F_LotSerie;
                                ENABLE TRIGGER TG_UPD_F_LOTSERIE ON F_LotSerie;";*/

                            if (!string.IsNullOrWhiteSpace(txtLot.Text))
                            {
                                cmd.CommandText = @"
                                DISABLE TRIGGER TG_INS_F_LOTSERIE ON F_LotSerie;
                                ALTER TABLE F_LotSerie NOCHECK CONSTRAINT FKA_F_LOTSERIE_AR_Ref;";
                                cmd.ExecuteNonQuery();

                                if (txtdateperemption.Text == "")
                                {
                                    txtdateperemption.Text = "2000-01-01";
                                }

                                cmd.CommandText = @"
                                INSERT INTO F_LotSerie
                                (AR_Ref, LS_NoSerie, LS_Qte, LS_QteRestant, LS_Peremption, DE_No, LotSerie,DL_NoOut)
                                VALUES
                                (@AR_Ref, @LS_NoSerie, @Qte, @Qte, @Peremption, @DE_No, @LotSerie, @DL_NoOut)";

                                cmd.Parameters.AddWithValue("@AR_Ref", txtreference.Text);
                                cmd.Parameters.AddWithValue("@LS_NoSerie", txtLot.Text);
                                cmd.Parameters.AddWithValue("@Qte", Convert.ToDecimal(txtqte1.Text));
                                cmd.Parameters.AddWithValue("@Peremption", Convert.ToDateTime(txtdateperemption.Text));
                                cmd.Parameters.AddWithValue("@DE_No", recuperer_depot(txtdepot1.Text));
                                cmd.Parameters.AddWithValue("@LotSerie", lotserie);
                                cmd.Parameters.AddWithValue("@DL_NoOut", lbldlnoout.Text);

                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"
                                ENABLE TRIGGER TG_INS_F_LOTSERIE ON F_LotSerie;
                                ALTER TABLE F_LotSerie CHECK CONSTRAINT FKA_F_LOTSERIE_AR_Ref;";
                                cmd.Parameters.Clear();
                                cmd.Parameters.Clear();
                            }
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

                _frmParent.lblrecup.Text = "";
                _frmParent.lblrecup.Text = txttype.Text + ";" + txtligne.Text + ";" + txtqte1.Text + ";"+txtLot.Text+";"+ lotserie;
                this.Close();
            }
        }
        private string recuperer_lotserie()
        {
            string rec = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_frmParent.connectionSource2))
                {
                    con.Open();

                    string query = @"SELECT TOP 1 LotSerie FROM [dbo].[F_LotSerie] ORDER BY LotSerie DESC";

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
            this.ControlBox = false;
        }
    }
}
