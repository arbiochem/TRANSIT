using DevExpress.Utils.Serializing;
using DevExpress.Xpo.DB.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using static DevExpress.XtraEditors.Mask.MaskSettings;

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

        private Boolean existe_document(string cond)
        {
            bool b = false;
            try
            {
                using (SqlConnection con = new SqlConnection(_frmParent.connectionSource2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = @"SELECT COUNT(*) FROM F_DOCENTETE WHERE DO_PIECE=@do_piece";
                        cmd.Parameters.AddWithValue("@do_piece", cond);
                        object result = cmd.ExecuteScalar();
                        if (result != null && Convert.ToInt32(result) > 0)
                        {
                            b = true;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return b;
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
                int DL_NoIn = 0;
                if (txtLot.Text != "")
                {
                    //lotserie = recuperer_lotserie();

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

                                if (!string.IsNullOrWhiteSpace(txtLot.Text))
                                {

                                    //En-tête
                                    cmd.CommandText = @"
                                DISABLE TRIGGER ALL ON F_DOCENTETE;
                                ";
                                    cmd.ExecuteNonQuery();

                                    cmd.CommandText = @"
                                INSERT INTO F_DOCENTETE
                                (DO_PIECE, DO_DOMAINE, DO_TYPE,DO_DATE,DO_REF, DO_TIERS, DE_No, DO_DOCTYPE,DO_HEURE,cbCreationUser,CO_No,DO_Period,DO_Devise,DO_Cours,LI_No,DO_Expedit,DO_NbFacture,DO_BLFact,DO_TxEscompte,DO_Reliquat,DO_Imprim,DO_Souche,DO_DateLivr,DO_Condition,DO_Tarif,DO_Colisage,DO_TypeColis,DO_Transaction,DO_Langue,DO_Ecart,DO_Regime,N_CatCompta,DO_Ventile,AB_No,DO_DebutAbo,DO_FinAbo,DO_DebutPeriod,DO_FinPeriod,DO_Statut,CA_No,CO_NoCaissier,DO_Transfere,DO_Cloture,DO_Attente,DO_Provenance,MR_No,DO_TypeFrais,DO_ValFrais,DO_TypeLigneFrais,DO_TypeFranco,DO_ValFranco,DO_TypeLigneFranco,DO_Taxe1,DO_TypeTaxe1,DO_TypeTaux1,DO_Taxe2,DO_TypeTaux2,DO_TypeTaxe2,DO_Taxe3,DO_TypeTaxe3,DO_TypeTaux3,DO_MajCpta,DO_FactureElec,DO_TypeTransac,DO_DateLivrRealisee,DO_DateExpedition,DO_EStatut,DO_DemandeRegul,ET_No,DO_Valide,DO_Coffre,DO_TotalHT,DO_StatutBAP,DO_Escompte,DO_TypeCalcul,DO_TotalHTNet,DO_TotalTTC,DO_NetAPayer,DO_MontantRegle,DO_PaiementLigne,DO_MotifDevis,cbProt,cbReplication,cbFlag,cbHashVersion)
                                VALUES
                                (@DO_PIECE, @DO_DOMAINE,@DO_TYPE, @DO_DATE, @DO_REF, @DO_TIERS, 0, @DO_DOCTYPE, @DO_HEURE,@user,0,0,0,0,0,0,0,0,0,0,0,0,'1753-01-01 00:00:00.000',0,0,1,1,0,0,0,0,0,0,0,'1753-01-01 00:00:00.000','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000','1753-01-01 00:00:00.000',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'1753-01-01 00:00:00.000','1753-01-01 00:00:00.000',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1)";

                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@DO_PIECE", txtligne.Text);
                                    cmd.Parameters.AddWithValue("@DO_DOMAINE",2);
                                    cmd.Parameters.Add("@DO_TYPE", 20);
                                    cmd.Parameters.AddWithValue("@DO_DATE", Convert.ToDateTime(DateTime.Now).Date);
                                    cmd.Parameters.AddWithValue("@DO_REF", txtRefs.Text);
                                    cmd.Parameters.AddWithValue("@DO_TIERS", recuperer_depot(txtdepot1.Text));
                                    cmd.Parameters.AddWithValue("@DO_DOCTYPE", 20);
                                    cmd.Parameters.AddWithValue("@DO_HEURE", DateTime.Now.ToString("HHmmssfff"));
                                    cmd.Parameters.AddWithValue("@user", cbUserCreation.Text);

                                    if (!existe_document(txtligne.Text))
                                    {
                                        cmd.ExecuteNonQuery();
                                    }

                                    cmd.CommandText = @"
                                        ENABLE TRIGGER ALL ON F_DOCENTETE;
                                    ";
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();

                                    //Ligne
                                    cmd.CommandText = @"
                                    DISABLE TRIGGER ALL ON F_DOCLIGNE;
                                    ALTER TABLE F_DOCLIGNE NOCHECK CONSTRAINT ALL;
                                    ";
                                    cmd.ExecuteNonQuery();


                                    cmd.CommandText = @"
                                    DECLARE @newDLNo INT = (SELECT ISNULL(MAX(DL_No), 0) + 1 FROM F_DOCLIGNE);
                                    DECLARE @newDLLigne INT = (
                                        SELECT ISNULL(MAX(DL_LIGNE), 0) + 1000 
                                        FROM F_DOCLIGNE 
                                        WHERE DO_PIECE = @do_piece
                                    );

                                    SET @newDLLigne = ISNULL(@newDLLigne, 1000);


                                    INSERT INTO F_DOCLIGNE
                                    (DO_PIECE, DO_DOMAINE, DO_TYPE, DO_DATE, AR_REF, CT_Num, DE_No, cbDE_No, cbCreationUser, DL_DateBL, DL_Design, DL_Qte, EU_Qte, PF_Num,DL_No,DL_LIGNE,DL_DateBC,DL_TNomencl,DL_TRemPied,DL_TRemExep,DL_QteBC,DL_QteBL,DL_PoidsNet,DL_PoidsBrut,DL_Remise01REM_Valeur,DL_Remise01REM_Type,DL_Remise02REM_Valeur,DL_Remise02REM_Type,DL_Remise03REM_Valeur,DL_Remise03REM_Type,DL_PrixUnitaire,DL_PUBC,DL_Taxe1,DL_TypeTaux1,DL_TypeTaxe1,DL_Taxe2,DL_TypeTaux2,DL_TypeTaxe2,CO_No,AG_No1,AG_No2,DL_PrixRU,DL_CMUP,DL_MvtStock,DT_No,DL_TTC,EU_Enumere,DL_TypePL,DL_PUDevise,DL_PUTTC,DO_DateLivr,DL_Taxe3,DL_TypeTaux3,DL_TypeTaxe3,DL_Frais,DL_Valorise,DL_NonLivre,DL_MontantHT,DL_MontantTTC,DL_FactPoids,DL_Escompte,DL_DatePL,DL_QtePL,DL_NoLink,DL_QteRessource,DL_DateAvancement,DL_PieceOFProd,DL_DateDE,DL_QteDE,DL_NoSousTotal,CA_No,DO_Doctype,cbProt,DL_NoRef)
                                    VALUES
                                    (@DO_PIECE, @DO_DOMAINE, @DO_TYPE, @DO_DATE, @AR_REF, @CT_Num, @DE_No, @DE_No, @user, @DO_DATE, @DL_Design, @DL_Qte, @DL_Qte, ' ',@newDLNo,@newDLLigne,'1753-01-01 00:00:00.000',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,@eu_enumere,0,0,0,'1753-01-01 00:00:00.000',0,0,0,0,1,0,0,0,0,0,'1753-01-01 00:00:00.000',0,0,0,'1753-01-01 00:00:00.000',0,'1753-01-01 00:00:00.000',0,0,0,20,0,1);
                                                                        SELECT @newDLNo";

                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@DO_PIECE", txtligne.Text);
                                    cmd.Parameters.AddWithValue("@DO_DOMAINE", 2);
                                    cmd.Parameters.Add("@DO_TYPE", 20);
                                    cmd.Parameters.AddWithValue("@DO_DATE", Convert.ToDateTime(DateTime.Now).Date);
                                    cmd.Parameters.AddWithValue("@AR_REF", txtreference.Text);
                                    cmd.Parameters.AddWithValue("@DL_Design", txtdesignation.Text);
                                    cmd.Parameters.AddWithValue("@CT_Num", 1);
                                    cmd.Parameters.AddWithValue("@DE_No", recuperer_depot(txtdepot1.Text));
                                    cmd.Parameters.AddWithValue("@user", cbUserCreation.Text);
                                    cmd.Parameters.AddWithValue("@eu_enumere", recuperer_unite(txtreference.Text));
                                    cmd.Parameters.AddWithValue("@DL_Qte", txtqte1.Text.Replace(",","."));

                                    DL_NoIn = Convert.ToInt32(cmd.ExecuteScalar());

                                    cmd.CommandText = @"
                                    ENABLE TRIGGER ALL ON F_DOCLIGNE;
                                    ALTER TABLE F_DOCLIGNE CHECK CONSTRAINT ALL;
                                    ";
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();

                                    cmd.CommandText = @"
                                    ALTER TABLE F_LotSerie NOCHECK CONSTRAINT ALL;";
                                    cmd.ExecuteNonQuery();

                                    cmd.CommandText = @"
                                    INSERT INTO F_LotSerie
                                    (AR_Ref, LS_NoSerie, LS_Qte, LS_QteRestant, LS_Peremption, DE_No,LS_Fabrication,cbCreationUser,DL_NoIn,LS_LotEpuise,LS_MvtStock,DL_NoOut,LS_QteRes)
                                    VALUES
                                    (@AR_Ref, @LS_NoSerie, @Qte, @Qte, @Peremption, @DE_No,@lsfabrication, @user,@dlnoin,0,1,0,0)";
                                   
                                    cmd.Parameters.AddWithValue("@AR_Ref", txtreference.Text);
                                    cmd.Parameters.AddWithValue("@LS_NoSerie", txtLot.Text);
                                    cmd.Parameters.Add("@Qte", qte);
                                    cmd.Parameters.AddWithValue("@Peremption", Convert.ToDateTime(txtdateperemption.Text).Date);
                                    cmd.Parameters.AddWithValue("@DE_No", recuperer_depot(txtdepot1.Text));
                                    cmd.Parameters.AddWithValue("@lsfabrication", Convert.ToDateTime(DateTime.Now).Date);
                                    cmd.Parameters.AddWithValue("@user", cbUserCreation.Text);
                                    //cmd.Parameters.AddWithValue("@lscomplement", txtRefs.Text);
                                    cmd.Parameters.AddWithValue("@dlnoin", DL_NoIn);

                                    cmd.ExecuteNonQuery();


                                    cmd.CommandText = @"
                                    ALTER TABLE F_LotSerie CHECK CONSTRAINT ALL; ";
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();

                                    //ARTSTOCK
                                    cmd.CommandText = @"
                                DISABLE TRIGGER ALL ON F_ARTSTOCK;
                                ALTER TABLE F_ARTSTOCK NOCHECK CONSTRAINT ALL; ";
                                    cmd.ExecuteNonQuery();

                                    string reference = txtreference.Text;
                                    decimal qtes = decimal.Parse(txtqte1.Text.ToString().Replace(",","."), CultureInfo.InvariantCulture);

                                    try
                                    {

                                         cmd.CommandText = @"
                                         SELECT TOP 1 AS_QteSto
                                         FROM F_ARTSTOCK
                                         WHERE AR_Ref = @AR_Ref AND DE_No = @DE_No";

                                        decimal? existingQte = null;

                                        cmd.Parameters.AddWithValue("@AR_Ref", reference);
                                        cmd.Parameters.AddWithValue("@DE_No", recuperer_depot(txtdepot1.Text));

                                        var result = cmd.ExecuteScalar();

                                        if (result != null && result != DBNull.Value)
                                        {
                                            existingQte = Convert.ToDecimal(result);
                                        }

                                        if (existingQte == null)
                                        {
                                            cmd.CommandText = @"
                                                 INSERT INTO F_ARTSTOCK (
                                                     AR_Ref,
                                                     DE_No,
                                                     DP_NoPrincipal,
                                                     AS_QteSto,
                                                     AS_QteRes,
                                                     AS_QteCom,
                                                     AS_QtePrepa,
                                                     AS_MontSto,
                                                     AS_QteMini,
                                                     AS_QteMaxi
                                                 )
                                                 VALUES (
                                                     @AR_Ref,
                                                     @DE_No,
                                                     @DP_NoPrincipal,
                                                     @Qte,
                                                     0, 0, 0, 0, 0, 0
                                                 )";

                                            cmd.Parameters.Clear();
                                            cmd.Parameters.AddWithValue("@AR_Ref", reference);
                                            cmd.Parameters.AddWithValue("@DE_No", recuperer_depot(txtdepot1.Text));
                                            cmd.Parameters.AddWithValue("@DP_NoPrincipal", 1);
                                            cmd.Parameters.Add("@Qte", SqlDbType.Decimal).Value = qtes;
                                            cmd.Parameters["@Qte"].Precision = 24;
                                            cmd.Parameters["@Qte"].Scale = 6;

                                            cmd.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            cmd.CommandText = @"
                                                 UPDATE F_ARTSTOCK
                                                 SET AS_QteSto = AS_QteSto + @Qte
                                                 WHERE AR_Ref = @AR_Ref AND DE_No = @DE_No";

                                            cmd.Parameters.Clear();
                                            cmd.Parameters.AddWithValue("@AR_Ref", reference);
                                            cmd.Parameters.AddWithValue("@DE_No", recuperer_depot(txtdepot1.Text));
                                            cmd.Parameters.Add("@Qte", SqlDbType.Decimal).Value = qtes;
                                            cmd.Parameters["@Qte"].Precision = 24;
                                            cmd.Parameters["@Qte"].Scale = 6;

                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "Erreur");
                                    }

                                    cmd.CommandText = @"
                                ENABLE TRIGGER ALL ON F_ARTSTOCK;
                                ALTER TABLE F_ARTSTOCK CHECK CONSTRAINT ALL; ";
                                    cmd.ExecuteNonQuery();
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
                }

                _frmParent.lblrecup.Text = "";
                _frmParent.lblrecup.Text = txttype.Text + ";" + txtligne.Text + ";" + txtqte1.Text + ";"+txtLot.Text+";"+ lotserie+";"+txtRefs.Text+";"+txtdateperemption.Text;
                this.Close();
            }
        }

        private string recuperer_unite(string cond)
        {
            string rec = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_frmParent.connectionSource2))
                {
                    con.Open();


                    string query = @"SELECT U_Intitule 
                     FROM [dbo].[P_UNITE] 
                     WHERE cbIndice = (
                         SELECT AR_UnitePoids 
                         FROM [dbo].[F_Article] 
                         WHERE AR_Ref = @AR_Ref
                     )";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@AR_Ref", SqlDbType.VarChar).Value = cond;

                        object result = cmd.ExecuteScalar();
                        rec = result?.ToString() ?? "";
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

                    string query = @"SELECT DE_No FROM [dbo].[F_Depot] WHERE DE_Intitule = '"+cond.Trim()+"'";

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

        private void txtqte1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                // Autoriser : chiffres, virgule, backspace
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
                {
                    e.Handled = true; // bloque la touche
                }

                // Empêcher plusieurs virgules
                if (e.KeyChar == ',' && ((TextBox)sender).Text.Contains(","))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '.' && ((TextBox)sender).Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex) { }
        }
    }
}
