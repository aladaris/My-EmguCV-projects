using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Aladaris
{
    public class SelectionRectangle
    {
        #region Atributes
        private Point _startPoint;
        private Rectangle _rect = new Rectangle();
        private static Color _brushColor = Color.FromArgb(128, 72, 145, 220);
        private Brush _selBrush = new SolidBrush(_brushColor);
        private ImageBox _control;
        #region MultiSelection
        private List<Rectangle> _rectangles = new List<Rectangle>();
        private static Color _brushColorMulti = Color.FromArgb(128, 220, 72, 105);
        private Brush _selBrushMulti = new SolidBrush(_brushColorMulti);
        #endregion
        #endregion

        #region Properties
        public ImageBox Control
        {
            get
            {
                return _control;
            }
            set
            {
                _control = value;
            }
        }
        #endregion

        #region Public Methods
        public SelectionRectangle(ImageBox i_imBox)
        {
            _control = i_imBox;
        }
        public void MouseDown(Point i_loc)
        {
            _startPoint = i_loc;
        }
        public void MouseMove(MouseEventArgs e)
        {
            Point tmpEndPoint = e.Location;
            _rect.Location = new Point(Math.Min(_startPoint.X, tmpEndPoint.X), Math.Min(_startPoint.Y, tmpEndPoint.Y));
            _rect.Size = new Size(Math.Abs(_startPoint.X - tmpEndPoint.X), Math.Abs(_startPoint.Y - tmpEndPoint.Y));
            _control.Invalidate();
        }
        public void DrawRectangle(PaintEventArgs e)
        {
            if (_control.Image != null)
                if (_rect != null && _rect.Width > 0 && _rect.Height > 0)
                    e.Graphics.FillRectangle(_selBrush, _rect);
        }
        public Image<C, D> GetSelectedImage<C, D>(Image<C, D> i_img)
            where C : struct, IColor
            where D : new()
        {
            try
            {
                return i_img.Copy(_rect);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("[ERROR] Area seleccionada insuficiente.", "ERROR de selección");
                return null;
            }
        }
        #region MultiSelection
        public void DrawRectanglesMulti(PaintEventArgs e)
        {
            if (_control.Image != null)
            {
                if (_rect != null)
                    e.Graphics.FillRectangle(_selBrush, _rect);

                foreach (Rectangle r in _rectangles)
                    e.Graphics.FillRectangle(_selBrushMulti, r);
            }
        }

        public void MouseUpMulti(MouseEventArgs e)
        {
            _rectangles.Add(_rect);
            _rect = new Rectangle();
        }

        public List<Image<C, D>> GetSelectedImagesMulti<C, D>(Image<C, D> i_img)
            where C : struct, IColor
            where D : new()
        {
            if (_rectangles.Capacity <= 0)
                return new List<Image<C, D>>();

            var result = new List<Image<C, D>>();
            Image<C, D> sample = null;
            foreach (Rectangle r in _rectangles)
            {
                try
                {
                    sample = i_img.Copy(r);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("[ERROR] Una de las áreas seleccionadas es insuficiente.", "ERROR de selección");
                }
                if (sample != null)
                    result.Add(sample);
            }
            return result;
        }
        public void RemoveMulti()
        {
            _rectangles = new List<Rectangle>();
        }
        #endregion
        #endregion
    }
}