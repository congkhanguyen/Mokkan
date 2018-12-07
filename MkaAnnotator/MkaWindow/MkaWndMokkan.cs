using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MokkAnnotator.MkaDrawTools;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaWndMokkan : DockContent
    {
        private MkaFrmAnnotator _owner; // parent form
        private String _preVal;         // store previous remain id        

        /// <summary>
        /// List of mokkans
        /// </summary>
        public MokkanList MokkanList { get; set; }       

        /// <summary>
        /// Constructor
        /// </summary>        
        public MkaWndMokkan(MkaFrmAnnotator owner)
        {
            InitializeComponent();
            _owner = owner;
        }

        /// <summary>
        /// Set data to view in grid
        /// </summary>
        public void ChangeGridData(MokkanList MokkanList)
        {
            this.MokkanList = MokkanList;
            this.MokkanList.Sort();
            dgrMokkan.Rows.Clear();
            foreach (DrawObject o in MokkanList.MokkanObjectList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgrMokkan);
                row.Cells[0].Value = o.MokkanInfo.RBangou;
                row.Cells[1].Value = o.MokkanInfo.KariShakubun;
                row.Cells[2].Value = o.MokkanInfo.GaihouShoshuuJyouhou;
                row.Cells[3].Value = o.MokkanInfo.ShasinBangouJyouhou;
                row.Cells[4].Value = o.MokkanInfo.Bikou;
                dgrMokkan.Rows.Add(row);                
            }           

            // change select row
            ChangeSelect(MokkanList);
        }

        /// <summary>
        /// Change select row
        /// </summary>
        public void ChangeSelect(MokkanList MokkanList)
        {
            this.MokkanList = MokkanList;            
            List<String> selectedRid = new List<String>();

            // get selected rid
            foreach (DrawObject o in MokkanList.MokkanObjectList)
            {
                if (o.Selected)
                    selectedRid.Add(o.MokkanInfo.RBangou.ToString());
            }

            // select row
            dgrMokkan.ClearSelection();            
            foreach (DataGridViewRow row in dgrMokkan.Rows)
            {
                if (selectedRid.Contains(row.Cells[0].Value.ToString()))
                    row.Selected = true;            
            }            
        }

        /// <summary>
        /// Clear data
        /// </summary>
        public void Clear()
        {
            MokkanList = null;
            dgrMokkan.Rows.Clear();
        }

        /// <summary>
        /// Change width of column when window size changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MkaWndMokkan_SizeChanged(object sender, EventArgs e)
        {
            dgcRBangou.Width = (int)(0.1 * dgrMokkan.Width);
            dgcKariShyakubun.Width = (int)(0.2 * dgrMokkan.Width);
            dgcGaihouShoshuuJyouhou.Width = (int)(0.25 * dgrMokkan.Width);
            dgcShashinBangouJyouhou.Width = (int)(0.25 * dgrMokkan.Width);
            dgcBikou.Width = dgrMokkan.Width - dgcRBangou.Width - dgcKariShyakubun.Width - dgcGaihouShoshuuJyouhou.Width - dgcShashinBangouJyouhou.Width;
            if(dgrMokkan.Rows.Count == 0)
                dgcBikou.Width -= 5;
            else
                dgcBikou.Width -= 20;
        }

        /// <summary>
        /// Row selected change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgrMokkan_SelectionChanged(object sender, EventArgs e)
        {
            if (dgrMokkan.Focused)
            {
                MokkanList.UnselectAll();
                foreach (DataGridViewRow row in dgrMokkan.SelectedRows)
                {
                    String rid = row.Cells[0].Value.ToString();
                    if (MokkanList[rid] != null)
                        MokkanList[rid].Selected = true;
                }

                if (dgrMokkan.SelectedRows.Count > 0)
                    _owner.MokkanSelectChange();
            }
        }

        /// <summary>
        /// With remain id number only input
        /// </summary>        
        private void dgrMokkan_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox txtEdit = e.Control as TextBox;            

            if (dgrMokkan.CurrentCell.ColumnIndex == 0)                            
                txtEdit.KeyPress += txtEdit_KeyPress;            
        }

        /// <summary>
        /// Number only input
        /// </summary>        
        private void txtEdit_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (dgrMokkan.CurrentCell.ColumnIndex == 0)
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        /// <summary>
        /// Update value        
        /// </summary> 
        private void dgrMokkan_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            String curId = dgrMokkan[0, e.RowIndex].Value.ToString();
            String value = dgrMokkan[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();
            
            switch (e.ColumnIndex)
            {
                case 0:
                    MokkanList[_preVal].MokkanInfo.RBangou = Int32.Parse(curId);
                    break;
                case 1:
                    MokkanList[curId].MokkanInfo.KariShakubun = value;
                    break;
                case 2:
                    MokkanList[curId].MokkanInfo.GaihouShoshuuJyouhou = value;
                    break;
                case 3:
                    MokkanList[curId].MokkanInfo.ShasinBangouJyouhou = value;
                    break;
                case 4:
                    MokkanList[curId].MokkanInfo.Bikou = value;
                    break;
            }

            // raise data change event
            if (dgrMokkan.CurrentCell.Selected && value != _preVal)
                _owner.MokkanDataChange();
        } 

        /// <summary>
        /// Check input remain id
        /// </summary>
        /// <returns></returns>
        private bool CheckId()
        {
            String curId = dgrMokkan[0, dgrMokkan.CurrentCell.RowIndex].EditedFormattedValue.ToString();
            int rid;

            String error = "";

            // check valid input
            if (!Int32.TryParse(curId, out rid))
            {
                error = String.Format(MkaMessage.ErrNumberRequest, dgrMokkan.Columns[0].HeaderText);
                MkaMessage.ShowError(error);
                return false;
            }

            // check duplication
            if (MokkanList.CheckDuplexRID(rid))
            {
                error = String.Format(MkaMessage.ErrDublication, dgrMokkan.Columns[0].HeaderText);
                MkaMessage.ShowError(error);
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Check valid remain id
        /// </summary>        
        private void dgrMokkan_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            _preVal = dgrMokkan.CurrentCell.Value.ToString();

            if (e.ColumnIndex == 0)
            {
                String curId = e.FormattedValue.ToString();                

                if (_preVal == null || curId == _preVal)
                    return;

                if (!CheckId())
                    e.Cancel = true;
            }
        }
    }   
}
