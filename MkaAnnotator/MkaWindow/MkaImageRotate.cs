using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaImageRotate : Form
    {
        /// <summary>
        /// Rotate angle
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Fill color
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// Rotate type
        /// </summary>
        public RotateType RotateType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaImageRotate()
        {
            InitializeComponent();

            // initialize
            Init();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {
            Angle = 0;
            mkaAngleSelector.Angle = 0;
            mkaColorButton.CenterColor = Color.White;
            cmbInterpolation.SelectedIndex = 0;
        }

        /// <summary>
        /// Change circular angle selector
        /// </summary>
        private void mkaAngleSelector_AngleChanged()
        {
            Angle = mkaAngleSelector.Angle;
            txtAngle.Text = Angle.ToString();
        }

        /// <summary>
        /// Change angle text box
        /// </summary>        
        private void txtAngle_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (!int.TryParse(txtAngle.Text, out val)) return;

            while (val < 0)
                val += 360;

            while (val > 360)
                val -= 360;

            Angle = val;
            mkaAngleSelector.Angle = val;
        }

        /// <summary>
        /// Input digit, - only in text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != 45 &&
                e.KeyChar != 8)
                e.Handled = true;
        }

        /// <summary>
        /// Change fill color
        /// </summary>        
        private void mkaColorButton_Click(object sender, EventArgs e)
        {
            MkaColorButton btn = (MkaColorButton)sender;
            Point p = new Point(btn.Left, btn.Top + btn.Height);
            p = PointToScreen(p);

            MkaColorDialog clDlg = new MkaColorDialog(p.X, p.Y);

            clDlg.ShowDialog();

            if (clDlg.DialogResult == DialogResult.OK)
                btn.CenterColor = clDlg.Color;
        }

        /// <summary>
        /// Reset form
        /// </summary>        
        private void btnReset_Click(object sender, EventArgs e)
        {
            // initialize
            Init();
        }

        /// <summary>
        /// Submit form
        /// </summary>        
        private void btnOK_Click(object sender, EventArgs e)
        {
            FillColor = mkaColorButton.CenterColor;
            RotateType = (RotateType)cmbInterpolation.SelectedIndex;
        }
    }
}
