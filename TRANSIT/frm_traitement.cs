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
                _frmParent.lblrecup.Text = "";
                _frmParent.lblrecup.Text = txttype.Text + ";" + txtligne.Text + ";" + txtqte1.Text + ";" + txtLot.Text;
                this.Close();
            }
        }

        private void frmLotSerie_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }
    }
}
