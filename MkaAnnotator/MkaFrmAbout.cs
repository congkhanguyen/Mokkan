using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator
{
    partial class mkaFrmSelect : Form
    {
        public mkaFrmSelect()
        {
            InitializeComponent();
        }

        private void MkaFrmAbout_Load(object sender, EventArgs e)
        {
            this.Text = MkaDefine.AssemblyProduct + "�ɂ���";

            lblText.Text = "�v���O����: " + MkaDefine.AssemblyProduct;
            lblText.Text += "\n\n�o�[�W����: " + MkaDefine.AssemblyVersion;
            lblText.Text += "\n\n" + MkaDefine.AssemblyCopyright;

        }
    }
}