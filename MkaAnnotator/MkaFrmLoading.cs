using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator
{
    public partial class MkaFrmLoading : Form
    {
        public MkaFrmLoading()
        {
            InitializeComponent();
                        
            lblNara.Parent = picBackground;
            lblNara.Text = MkaDefine.AssemblyCompanyJp;
            lblNara.BackColor = Color.Transparent;
                        
            lblName.Parent = picBackground;
            lblName.Text = MkaDefine.AssemblyTitleJp;
            lblName.BackColor = Color.Transparent;

            lblVersion.Parent = picBackground;
            lblVersion.Text = "Version " + MkaDefine.AssemblyVersion;
            lblVersion.BackColor = Color.Transparent;
  
            lblCopyright.Text = MkaDefine.AssemblyCopyright;
            lblCopyright.Parent = picBackground;
            lblCopyright.BackColor = Color.Transparent;
        }
    }
}
