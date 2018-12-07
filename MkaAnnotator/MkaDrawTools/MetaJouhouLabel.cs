using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MokkAnnotator.MkaDrawTools
{
    public partial class MetaJouhouLabel : Label
    {
        private bool down;
        private Point LastLocation;
        public Point Origin;

        //public PointF PrevLocation;
        public float WidthOwner { get; set; }
        public float HeightOwner { get; set; }
        public float CurentRatio { get; set; }

        public MetaJouhouLabel(string name)
        {
            down = false;
            CurentRatio = 0;
            WidthOwner = 0;
            HeightOwner = 0;
            this.BackColor = Color.White;
            this.Text = this.Name = name;
            LastLocation = new Point();
            Origin = new Point();
            //PrevLocation = new PointF();
            this.DoubleBuffered = true;
            this.Margin = new Padding(0, 0, 0, 0);
            this.Padding = new Padding(0,0,0,0);
        }

        protected override void OnMouseDown(MouseEventArgs e) 
        {
            if (e.Button == MouseButtons.Left)
            {
                LastLocation = e.Location;
                //BringToFront();
                down = true;
                this.BorderStyle = BorderStyle.FixedSingle;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (down)
            {
                int dx = e.X - LastLocation.X;
                int dy = e.Y - LastLocation.Y;

                //Check if out of border
                int moveX = Left + dx, moveY = Top + dy;
                if (moveX < Origin.X)
                    moveX = Origin.X;
                if (moveY < Origin.Y)
                    moveY = Origin.Y;
                if (moveX + this.Width > WidthOwner * CurentRatio + Origin.X)
                    moveX = (int)(WidthOwner * CurentRatio - this.Width + Origin.X);
                if (moveY + this.Height > HeightOwner * CurentRatio + Origin.Y)
                    moveY = (int)(HeightOwner * CurentRatio - this.Height + Origin.Y);

                this.Location = new Point(moveX, moveY);
                //PrevLocation = 
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            down = false;
            this.BorderStyle = BorderStyle.None;
            base.OnMouseUp(e);
        }
    }
}
