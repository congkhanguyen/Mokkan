using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using System.Security.Permissions;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using MokkAnnotator.MkaWindow;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

using MokkAnnotator.MkaDocToolkit.Xml;
using MokkAnnotator.MkaToolsData;

namespace MokkAnnotator.MkaDrawTools
{
    using MokkanObjectList = List<DrawObject>;
    using MokkanInfoList = List<MkaMokkanInfo>;
    using MokkAnnotator.MkaCommon;

    /// <summary>
    /// List of graphic objects
    /// </summary>
    [Xmlable("mokkans", IsUnique = false)]
    public class MokkanList : IXmlable,ICloneable
    {
        private MokkanObjectList _mokkanObjList;
        public MokkanObjectList MokkanObjectList
        {
            get { return this._mokkanObjList; }
            set { this._mokkanObjList = value; }
        }

        private const string entryCount = "Count";
        private const string entryType = "Type";        

        public MokkanList()
        {
            _mokkanObjList = new MokkanObjectList();
        }

        public int KaishiRBangou { get; set; }

        /// <summary>
        /// Clone this instance
        /// </summary>
        /// <returns></returns>
        public MokkanList Clone()
        {
            MokkanList ret = (MokkanList)this.MemberwiseClone();
            MokkanObjectList list = new MokkanObjectList();
            foreach (DrawObject o in MokkanObjectList)
                list.Add(o.Clone());
            ret.MokkanObjectList = list;
            return ret;
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public void Draw(Graphics g)
        {
            int n = _mokkanObjList.Count;
            DrawObject o;

            // Enumerate list in reverse order to get first
            // object on the top of Z-order.
            for (int i = n - 1; i >= 0; i--)
            {
                o = _mokkanObjList[i];

                o.Draw(g);

                if (o.Selected == true)
                {
                    o.DrawTracker(g);
                }
            }
        }

        /// <summary>
        /// Draw mokkan items to print
        /// </summary>
        /// <param name="g"></param>
        public void DrawToPrint(Graphics g, float ratio, Point startPoint)
        {        
            foreach(DrawObject o in _mokkanObjList)
                o.DrawToPrint(g, ratio, startPoint);            
        }

        /// <summary>
        /// Clear all objects in the list
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool Clear()
        {
            bool result = (_mokkanObjList.Count > 0);
            _mokkanObjList.Clear();
            return result;
        }

        /// <summary>
        /// Count and this [nIndex] allow to read all graphics objects
        /// from mokkanList in the loop.
        /// </summary>
        public int Count
        {
            get
            {
                return _mokkanObjList.Count;
            }
        }

        /// <summary>
        /// Get mokkan by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DrawObject this[int index]
        {
            get
            {
                if (index < 0 || index >= _mokkanObjList.Count)
                    return null;

                return _mokkanObjList[index];
            }
        }

        /// <summary>
        /// Get mokkan by rid
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DrawObject this[string rid]
        {
            get
            {
                foreach (DrawObject o in _mokkanObjList)
                    if (o.MokkanInfo.RBangou.ToString() == rid)
                        return o;

                return null;
            }
        }

        /// <summary>
        /// Selected mokkans
        /// </summary>
        public MokkanObjectList SelectedCloneObjects
        {
            get
            {
                MokkanObjectList objects = new MokkanObjectList();
                foreach (DrawObject o in Selection)
                {
                    objects.Add(o.Clone());
                }

                return objects;
            }
        }

        /// <summary>
        /// Selected mokkans
        /// </summary>
        public MokkanObjectList SelectedObjects
        {
            get
            {
                MokkanObjectList objects = new MokkanObjectList();
                foreach (DrawObject o in Selection)
                {
                    objects.Add(o);
                }

                return objects;
            }
        }

        /// <summary>
        /// Selected mokkans' properties
        /// </summary>
        public MkaMokkanInfo[] SelectedObjectsProperties
        {
            get
            {
                MokkanInfoList objects = new MokkanInfoList();
                foreach (DrawObject o in Selection)
                {
                    objects.Add(o.MokkanInfo);
                }

                return objects.ToArray();
            }
        }


        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public int SelectionCount
        {
            get
            {
                int n = 0;

                foreach (DrawObject o in Selection)
                {
                    n++;
                }

                return n;
            }
        }

        /// <summary>
        /// Get boundary of drawed objects
        /// </summary>        
        public Rectangle GetBoundaryRectangle()
        {
            Rectangle rec = new Rectangle();
            rec.X = rec.Y = 0;
            rec.Width = rec.Height = 999999;

            foreach (DrawObject o in _mokkanObjList)
            {
                if (o.Top < rec.Top)
                    rec.Y = o.Top;
                if (o.Bottom > rec.Bottom)
                    rec.Height = o.Bottom - rec.Y;
                if (o.Left < rec.Left)
                    rec.X = o.Left;
                if (o.Right < rec.Right)
                    rec.Width = o.Right - rec.X;
            }

            return rec;
        }

        /// <summary>
        /// Returns INumerable object which may be used for enumeration
        /// of selected objects.
        /// 
        /// Note: returning IEnumerable<DrawObject> breaks CLS-compliance
        /// (assembly CLSCompliant = true is removed from AssemblyInfo.cs).
        /// To make this program CLS-compliant, replace 
        /// IEnumerable<DrawObject> with IEnumerable. This requires
        /// casting to object at runtime.
        /// </summary>
        /// <value></value>
        public IEnumerable<DrawObject> Selection
        {
            get
            {
                foreach (DrawObject o in _mokkanObjList)
                {
                    if (o.Selected)
                    {
                        yield return o;
                    }
                }
            }
        }

        /// <summary>
        /// Add item 
        /// </summary>
        /// <param name="obj"></param>
        public void Add(DrawObject obj)
        {
            // insert to the top of z-order            
            _mokkanObjList.Insert(0, obj);
        }

        /// <summary>
        /// Insert object to specified place.
        /// Used for Undo.
        /// </summary>
        public void Insert(int index, DrawObject obj)
        {
            if (index >= 0 && index < _mokkanObjList.Count)
            {
                _mokkanObjList.Insert(index, obj);
            }
        }

        /// <summary>
        /// Replace object in specified place.
        /// Used for Undo.
        /// </summary>
        public void Replace(int index, DrawObject obj)
        {
            if (index >= 0 && index < _mokkanObjList.Count)
            {
                _mokkanObjList.RemoveAt(index);
                _mokkanObjList.Insert(index, obj);
            }
        }

        /// <summary>
        /// Remove object by index.
        /// Used for Undo.
        /// </summary>
        public void RemoveAt(int index)
        {            
            _mokkanObjList.RemoveAt(index);
            MkaMokkanInfo.LastRBangou = GetMaxRBangou() + 1;
        }

        /// <summary>
        /// Delete last added object from the list
        /// (used for Undo operation).
        /// </summary>
        public void DeleteLastAddedObjects(int addedCount)
        {
            if (_mokkanObjList.Count > 0 && addedCount <= _mokkanObjList.Count)
            {
                _mokkanObjList.RemoveRange(_mokkanObjList.Count - addedCount, addedCount);                
            }
            MkaMokkanInfo.LastRBangou = GetMaxRBangou() + 1;
        }

        public void SelectInRectangle(Rectangle rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in _mokkanObjList)
            {
                if (o.IntersectsWith(rectangle))
                    o.Selected = true;
            }

        }

        public void UnselectAll()
        {
            foreach (DrawObject o in _mokkanObjList)
            {
                o.Selected = false;
            }
        }

        public void SelectAll()
        {
            foreach (DrawObject o in _mokkanObjList)
            {
                o.Selected = true;
            }
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool DeleteSelection()
        {
            bool result = false;

            int n = _mokkanObjList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_mokkanObjList[i]).Selected)
                {
                    _mokkanObjList.RemoveAt(i);
                    result = true;
                }
            }

            MkaMokkanInfo.LastRBangou = GetMaxRBangou() + 1;

            return result;
        }


        /// <summary>
        /// Move selected items to front (beginning of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToFront()
        {
            int n;
            int i;
            MokkanObjectList tempList;

            tempList = new MokkanObjectList();
            n = _mokkanObjList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((_mokkanObjList[i]).Selected)
                {
                    tempList.Add(_mokkanObjList[i]);
                    _mokkanObjList.RemoveAt(i);
                }
            }

            // Read temporary list in direct order and insert every item
            // to the beginning of the source list
            n = tempList.Count;

            for (i = 0; i < n; i++)
            {
                _mokkanObjList.Insert(0, tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Move selected items to back (end of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToBack()
        {
            int n;
            int i;
            MokkanObjectList tempList;

            tempList = new MokkanObjectList();
            n = _mokkanObjList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((_mokkanObjList[i]).Selected)
                {
                    tempList.Add(_mokkanObjList[i]);
                    _mokkanObjList.RemoveAt(i);
                }
            }

            // Read temporary list in reverse order and add every item
            // to the end of the source list
            n = tempList.Count;

            for (i = n - 1; i >= 0; i--)
            {
                _mokkanObjList.Add(tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Check duplication of remain id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckDuplexRID(int id)
        {
            if (_mokkanObjList == null || _mokkanObjList.Count == 0)
                return false;

            foreach (DrawObject o in _mokkanObjList)
            {
                if (o.MokkanInfo.RBangou == id)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check double remain id (in case of property value changed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckDoubleRID(int id)
        {
            int count = 0;

            if (_mokkanObjList == null || _mokkanObjList.Count == 0)
                return false;

            foreach (DrawObject o in _mokkanObjList)
            {
                if (o.MokkanInfo.RBangou == id)
                    count++;
            }

            if (count > 1) return true;

            return false;
        }

        /// <summary>
        /// Get max remain id
        /// </summary>
        /// <returns></returns>
        public int GetMaxRBangou()
        {
            int max = 0;
            foreach (DrawObject o in _mokkanObjList)
            {
                if (o.MokkanInfo.RBangou > max)
                    max = o.MokkanInfo.RBangou;
            }
            return Math.Max(max, KaishiRBangou - 1);
        }

        /// <summary>
        /// Get next usable id
        /// </summary>
        /// <returns>next usable r bangou</returns>
        public int GetNextRBangou(int id, int skip)
        {
            foreach (DrawObject o in _mokkanObjList.Skip(skip))
            {
                if (o.MokkanInfo.RBangou == id)
                    return GetNextRBangou(id + 1, skip);
            }
            return id;
        }

        /// <summary>
        /// Rearrange remain id
        /// </summary>
        /// <returns>true if order is changed, else is false</returns>
        public void ReArrangeId(int kaishiRBangou)
        {
            int i, j, id;
            int top1, left1, top2, left2, tempTop, tempLeft;
            int pos;
            int n = _mokkanObjList.Count;
            int half = (int)(n / 2 + 0.5);
            int[] upper = new int[half];

            Calculate();

            // not changed
            if(n < 1) return;
            if(n == 1)
            {
                _mokkanObjList[0].MokkanInfo.RBangou = kaishiRBangou;
                _mokkanObjList[0].Properties.RShowPosition = ShowPosition.Top;
                return;
            }


            // arrange
            int[,] top = new int[n,2];
            int[,] left = new int[n,2];            
            for (i = 0; i < n; i++)
            {
                id = _mokkanObjList[i].MokkanInfo.RBangou;
                top[i, 0] = id;
                top[i, 1] = _mokkanObjList[i].Top;
                left[i, 0] = id;
                left[i, 1] = _mokkanObjList[i].Left;                
            }

            // rearrange order
            for (j = n - 1; j >= 0; j--)
            {
                for (i = 1; i <= j; i++)
                {
                    top1 = top[i - 1, 1];
                    top2 = top[i, 1];
                    if (top1 > top2)
                    {
                        id = top[i,0];
                        top[i,0] = top[i-1,0];
                        top[i - 1,0] = id;
                        tempTop = top[i, 1];
                        top[i, 1] = top[i - 1, 1];
                        top[i - 1, 1] = tempTop;
                    }

                    left1 = left[i - 1, 1];
                    left2 = left[i, 1];
                    if (left1 > left2)
                    {
                        id = left[i, 0];
                        left[i, 0] = left[i - 1, 0];
                        left[i - 1, 0] = id;
                        tempLeft = left[i, 1];
                        left[i, 1] = left[i - 1, 1];
                        left[i - 1, 1] = tempLeft;
                    }
                }
            }

            // list of upper mokkan
            for (i = 0; i < half; i++)
                upper[i] = top[i, 0];
            
            // rearrange remain id
            for (i = 0; i < n; i++)
            {
                id = _mokkanObjList[i].MokkanInfo.RBangou;
                pos = 0;
                if (upper.Contains(id))
                {                    
                    for (j = 0; j < n; j++)
                    {
                        if (upper.Contains(left[j, 0])) pos++;
                        if (left[j, 0] == id)
                            break;
                    }

                    _mokkanObjList[i].MokkanInfo.RBangou = kaishiRBangou + pos - 1;
                    _mokkanObjList[i].Properties.RShowPosition = ShowPosition.Top;
                }
                else
                {                    
                    for (j = 0; j < n; j++)
                    {
                        if (!upper.Contains(left[j, 0])) pos++;
                        if (left[j, 0] == id)
                            break;
                    }

                    _mokkanObjList[i].MokkanInfo.RBangou = kaishiRBangou + half + pos - 1;
                    _mokkanObjList[i].Properties.RShowPosition = ShowPosition.Bottom;
                }             
            }
        }

        /// <summary>
        /// Sort mokkan list by id
        /// </summary>
        public void Sort()
        {
            _mokkanObjList.Sort(delegate(DrawObject o1, DrawObject o2)
              {
                  return (o1.MokkanInfo.RBangou.CompareTo(o2.MokkanInfo.RBangou));
              });
        }

        /// <summary>
        /// Calculate object's parameters
        /// </summary>
        public void Calculate()
        {
            foreach (DrawObject o in _mokkanObjList)
                o.Calculate();
        }

        #region Show Property

        /// <summary>
        /// Get properties from selected objects and fill GraphicsProperties instance
        /// </summary>
        /// <returns></returns>
        private void GetProperties(ref MkaMokkanInfo info, ref GraphicsProperties properties)
        {
            bool bFirst = true;

            string firstKariShakubun = "";
            string firstGaihouShoshuuJyouhou = "";
            string firstShashinBangouJyouhou = "";
            string firstBikou = "";
            string firstBorderColor ="";
            float firstPenWidth = 1;
            string firstFillColor = "";
            int firstAreaAlpha = 0;
            ShowPosition firstPos = ShowPosition.Top;

            bool allKariShakubunAreEqual = true;
            bool allGaihouShoshuuJyouhouAreEqual = true;
            bool allShashinBangouJyouhouAreEqual = true;
            bool allBikouAreEqual = true;
            bool allBorderColorAreEqual = true;
            bool allPenWidthAreEqual = true;
            bool allFillColorAreEqual = true;
            bool allAreaAlphaAreEqual = true;
            bool allShowPosAreEqual = true;

            foreach (DrawObject o in Selection)
            {
                if (bFirst)
                {
                    firstKariShakubun = o.MokkanInfo.KariShakubun;
                    firstGaihouShoshuuJyouhou = o.MokkanInfo.GaihouShoshuuJyouhou;
                    firstShashinBangouJyouhou = o.MokkanInfo.ShasinBangouJyouhou;
                    firstBikou = o.MokkanInfo.Bikou;

                    firstBorderColor = o.Properties.BorderColorHtml;
                    firstPenWidth = o.Properties.PenWidth;
                    firstFillColor = o.Properties.FillColorHtml;
                    firstAreaAlpha = o.Properties.FillColorAlpha;
                    firstPos = o.Properties.RShowPosition;
                    bFirst = false;
                }
                else
                {
                    if (o.MokkanInfo.KariShakubun != firstKariShakubun)
                        allKariShakubunAreEqual = false;

                    if (o.MokkanInfo.GaihouShoshuuJyouhou != firstGaihouShoshuuJyouhou)
                        allGaihouShoshuuJyouhouAreEqual = false;

                    if (o.MokkanInfo.ShasinBangouJyouhou != firstShashinBangouJyouhou)
                        allShashinBangouJyouhouAreEqual = false;

                    if (o.MokkanInfo.Bikou != firstBikou)
                        allBikouAreEqual = false;

                    if (o.Properties.BorderColorHtml != firstBorderColor)
                        allBorderColorAreEqual = false;

                    if (o.Properties.PenWidth != firstPenWidth)
                        allPenWidthAreEqual = false;

                    if (o.Properties.FillColorHtml != firstFillColor)
                        allFillColorAreEqual = false;

                    if (o.Properties.FillColorAlpha != firstAreaAlpha)
                        allAreaAlphaAreEqual = false;

                    if (o.Properties.RShowPosition != firstPos)
                        allShowPosAreEqual = false;
                }
            }

            if (SelectionCount == 1)
                info.RBangou = SelectedObjects[0].MokkanInfo.RBangou;

            if (allKariShakubunAreEqual)
                info.KariShakubun = firstKariShakubun;

            if (allGaihouShoshuuJyouhouAreEqual)
                info.GaihouShoshuuJyouhou = firstGaihouShoshuuJyouhou;

            if (allShashinBangouJyouhouAreEqual)
                info.ShasinBangouJyouhou = firstShashinBangouJyouhou;

            if (allBikouAreEqual)
                info.Bikou = firstBikou;

            if (allBorderColorAreEqual)
                properties.BorderColor = ColorTranslator.FromHtml(firstBorderColor);
            
            if (allPenWidthAreEqual)
                properties.PenWidth = firstPenWidth;

            if (allFillColorAreEqual)
                properties.FillColor = ColorTranslator.FromHtml(firstFillColor);

            if (allAreaAlphaAreEqual)
               properties.FillColorAlpha = firstAreaAlpha;

            if (allShowPosAreEqual)
                properties.RShowPosition = firstPos;
        }

        /// <summary>
        /// Apply properties for all selected objects.
        /// Returns TRue if at least one property is changed.
        /// </summary>
        private bool ApplyProperties(MkaMokkanInfo info, GraphicsProperties properties)
        {
            bool changed = false;

            if (SelectionCount == 1)
            {
                if (SelectedObjects[0].MokkanInfo.RBangou != info.RBangou)
                {
                    SelectedObjects[0].MokkanInfo.RBangou = info.RBangou;
                    changed = true;
                }   
            }

            foreach (DrawObject o in Selection)
            {
                if (o.MokkanInfo.KariShakubun != info.KariShakubun)
                {
                    o.MokkanInfo.KariShakubun = info.KariShakubun;
                    changed = true;
                }

                if (o.MokkanInfo.GaihouShoshuuJyouhou != info.GaihouShoshuuJyouhou)
                {
                    o.MokkanInfo.GaihouShoshuuJyouhou = info.GaihouShoshuuJyouhou;
                    changed = true;
                }

                if (o.MokkanInfo.ShasinBangouJyouhou != info.ShasinBangouJyouhou)
                {
                    o.MokkanInfo.ShasinBangouJyouhou = info.ShasinBangouJyouhou;
                    changed = true;
                }

                if (o.MokkanInfo.Bikou != info.Bikou)
                {
                    o.MokkanInfo.Bikou = info.Bikou;
                    changed = true;
                }

                // border color
                if (o.Properties.BorderColor != properties.BorderColor)
                {
                    o.Properties.BorderColor = properties.BorderColor;
                    changed = true;
                }

                // border pen width
                if (o.Properties.PenWidth != properties.PenWidth)
                {
                    o.Properties.PenWidth = properties.PenWidth;
                    changed = true;
                }


                // fill area color
                if (o.Properties.FillColor != properties.FillColor)
                {
                    o.Properties.FillColor = properties.FillColor;
                    changed = true;
                }

                // fill area transparent
                if (o.Properties.FillColorAlpha != properties.FillColorAlpha)
                {
                    o.Properties.FillColorAlpha = properties.FillColorAlpha;
                    changed = true;
                }

                // remain id show position
                if (o.Properties.RShowPosition != properties.RShowPosition)
                {
                    o.Properties.RShowPosition = properties.RShowPosition;
                    changed = true;
                }
            }

            return changed;
        }

        /// <summary>
        /// Show Properties dialog. Return true if list is changed
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool ShowPropertiesDialog(MkaDocument parent)
        {
            if (SelectionCount < 1)
                return false;

            MkaMokkanInfo info = new MkaMokkanInfo();
            GraphicsProperties properties = new GraphicsProperties();
            bool multi = (SelectionCount > 1);

            // get properties from selected object
            GetProperties(ref info, ref properties);

            MkaPropertiesMokkan dlg = new MkaPropertiesMokkan(this, info, properties, multi);
            CommandChange c = new CommandChange(this);

            if (dlg.ShowDialog(parent) != DialogResult.OK)
                return false;

            if (ApplyProperties(dlg.MokkanProperty, dlg.Properties))
            {
                c.NewState(this);
                parent.AddCommandToHistory(c);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Update font size to display remain id
        /// </summary>
        /// <param name="size"></param>
        public void UpdateRFontSize(float size)
        {
            foreach (DrawObject o in _mokkanObjList)            
                o.Properties.RFontSize = size;
        }

        #endregion

        #region IXmlable Members

        void IXmlable.FromXml(XmlElement xmlEle, IXmlContext cnt)
        {
            this._mokkanObjList.Clear();
            DrawObject.Ratio = 1.0f;
            DrawObject.Origin = new Point(0, 0);
            foreach (XmlElement objEle in xmlEle)
            {
                DrawObject obj = cnt.FromXml(objEle) as DrawObject;
                System.Diagnostics.Debug.Assert(null != obj);
                if (null != obj)
                {
                    obj.Calculate();
                    _mokkanObjList.Add(obj);
                }
            }
        }

        void IXmlable.ToXml(XmlElement xmlEle, IXmlContext cnt)
        {
            foreach (DrawObject obj in _mokkanObjList)
            {
                XmlElement objEle = cnt.ToXml(obj);
                if (null != objEle)
                    xmlEle.AppendChild(objEle);
            }
        }

        #endregion
    }
}
