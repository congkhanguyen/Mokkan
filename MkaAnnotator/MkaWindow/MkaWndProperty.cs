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
using System.Reflection;
using MokkAnnotator.MkaToolsData;

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaWndProperty : DockContent
    {
        private MkaFrmAnnotator _owner;        // parent form

        /// <summary>
        /// Selected object type
        /// </summary>
        public SelectedObjectType SelectedType { get; set; }

        /// <summary>
        /// Activate bat to edit property
        /// </summary>        
        public void SelectBat(MkaBatManager bat)
        {
            this.propertyGrid.SelectedObject = bat.BatInfo;
            SelectedType = SelectedObjectType.Bat;
        }

        /// <summary>
        /// Activate document to edit property
        /// </summary> 
        public void SelectDoc(MkaDocument doc)
        {
            this.DocumentArea = doc;
            this.MokkanList = doc.MokkanList;
            if (MokkanList.SelectionCount > 0)  // mokkan property
            {
                this.propertyGrid.SelectedObjects = MokkanList.SelectedObjectsProperties;
                SelectedType = SelectedObjectType.Mokkan;
            }
            else    // glass property
            {
                this.propertyGrid.SelectedObject = doc.GlassInfo;
                SelectedType = SelectedObjectType.Glass;
            }
        }

        /// <summary>
        /// List of mokkans
        /// </summary>
        public MokkanList MokkanList { get; set; }

        /// <summary>
        /// Document area
        /// </summary>
        public MkaDocument DocumentArea { get; set; }

        // control invalid input
        private bool _error;
        private String _sError;

        private CommandChange command;
        private int[] Rids;

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaWndProperty(MkaFrmAnnotator owner)
        {
            InitializeComponent();
            _owner = owner;
        }

        /// <summary>
        /// Clear property grid
        /// </summary>
        public void Clear()
        {
            if (propertyGrid.SelectedObject != null)                
                propertyGrid.SelectedObject = null;
            SelectedType = SelectedObjectType.None;
        }

        /// <summary>
        /// Check id is duplicated or not
        /// </summary>        
        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {            
            GridItem changedItem = e.ChangedItem;
            _sError = "";
            _error = false;

            if ((changedItem.PropertyDescriptor.Name == MkaDefine.BatOoChiku) ||
                (changedItem.PropertyDescriptor.Name == MkaDefine.BatChuushouChiku) ||
                (changedItem.PropertyDescriptor.Name == MkaDefine.BatIkoumei) ||
                (changedItem.PropertyDescriptor.Name == MkaDefine.BatDosoumei) ||
                (changedItem.PropertyDescriptor.Name == MkaDefine.BatDate) ||
                (changedItem.PropertyDescriptor.Name == MkaDefine.BatBangou) ||            
                (changedItem.PropertyDescriptor.Name == MkaDefine.GlassItaBangou))
            {
                String value = changedItem.Value.ToString().Trim();
                if(value == String.Empty)
                {
                    _sError = String.Format(MkaMessage.ErrInputRequest, changedItem.PropertyDescriptor.DisplayName);
                    _error = true;
                    changedItem.PropertyDescriptor.SetValue(propertyGrid.SelectedObject, e.OldValue);
                }
            }
            else if ((changedItem.PropertyDescriptor.Name == MkaDefine.BatChousaJisuu) ||
                (changedItem.PropertyDescriptor.Name == MkaDefine.BatGrid) ||
                (changedItem.PropertyDescriptor.Name == MkaDefine.GlassKaishiRBangou))
            {
                int value = Convert.ToInt32(changedItem.Value);
                if (value <= 0)
                {
                    _sError = String.Format(MkaMessage.ErrNumberRequest, changedItem.PropertyDescriptor.DisplayName);
                    _error = true;
                    changedItem.PropertyDescriptor.SetValue(propertyGrid.SelectedObject, e.OldValue);                    
                }
                else
                {
                    if (changedItem.PropertyDescriptor.Name == MkaDefine.GlassKaishiRBangou)
                        DocumentArea.Rearrange(value - (int)e.OldValue);
                }                
            }
            else if (changedItem.PropertyDescriptor.Name == MkaDefine.MokkanRBangou)
            {
                int rid = Convert.ToInt32(changedItem.Value);
                if (rid < 0)
                {
                    _sError = String.Format(MkaMessage.ErrNumberRequest, changedItem.PropertyDescriptor.DisplayName);
                    _error = true;                    
                    changedItem.PropertyDescriptor.SetValue(propertyGrid.SelectedObject, e.OldValue);
                }
                else if (MokkanList.CheckDoubleRID(rid))
                {
                    _sError = String.Format(MkaMessage.ErrDublication, changedItem.Label);
                    _error = true;
                    if (propertyGrid.SelectedObjects.Length == 1)
                        changedItem.PropertyDescriptor.SetValue(propertyGrid.SelectedObject, e.OldValue);
                    else
                    {
                        int i;

                        for (i = 0; i < propertyGrid.SelectedObjects.Length; i++)
                        {
                            MkaMokkanInfo mk = (MkaMokkanInfo)propertyGrid.SelectedObjects[i];
                            mk.RBangou = Rids[i];
                        }
                    }
                }
            }
            else if (changedItem.PropertyDescriptor.Name == MkaDefine.GraphicPenWidth)
            {
                float penWid = (float)Convert.ToDouble(changedItem.Value);
                if (penWid < 0.25 || penWid > 6)
                {
                    _sError = MkaMessage.ErrInvalidPenWidth;
                    _error = true;
                    changedItem.PropertyDescriptor.SetValue(propertyGrid.SelectedObject, e.OldValue);
                }
            }
            else if (changedItem.PropertyDescriptor.Name == MkaDefine.GraphicColorAlpha)
            {
                int alpha = Convert.ToInt32(changedItem.Value);
                if (alpha < 0 || alpha > 255)
                {
                    _sError = MkaMessage.ErrInvalidAlpha;
                    _error = true;
                    changedItem.PropertyDescriptor.SetValue(propertyGrid.SelectedObject, e.OldValue);
                }
            }

            if (_error)
            {
                MkaMessage.ShowError(_sError);
                this.Focus();
                changedItem.Select();
                return;
            }

            if (SelectedType == SelectedObjectType.Mokkan)    
            {
                command.NewState(MokkanList);
                DocumentArea.AddCommandToHistory(command);                
            }
            DocumentArea.SetDirty();
            
            // raise data change event 
            _owner.PropertyDataChange();
        }

        /// <summary>
        /// Store previous status
        /// </summary>        
        private void propertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (SelectedType == SelectedObjectType.Mokkan)
                command = new CommandChange(MokkanList);
            int i;

            if (propertyGrid.SelectedObjects.Length > 1)
            {
                Rids = new int[propertyGrid.SelectedObjects.Length];
                for (i = 0; i < propertyGrid.SelectedObjects.Length; i++ )
                {
                    MkaMokkanInfo mk = (MkaMokkanInfo)propertyGrid.SelectedObjects[i];
                    Rids[i] = mk.RBangou;
                }
            }            
        }        
    }
}
