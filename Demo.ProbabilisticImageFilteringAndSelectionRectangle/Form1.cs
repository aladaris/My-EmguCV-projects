/*
   Copyright © 2014 Fernando González López - Peñalver <aladaris@gmail.com>
   This file is part of EmguCV-Projects.

    EmguCV-Projects is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License.

    EmguCV-Projects is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with EmguCV-Projects.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.Util;
using Emgu.CV.Structure;

using Aladaris;

namespace Demo.ProbabilisticImageFilteringAndSelectionRectangle
{
    public partial class Form1 : Form
    {
        #region Atributes
        // Atributos
        private Image<Bgr, Byte> _img;
        private SelectionRectangle _selection;
        private ProbabilisticImageFiltering _imFilter;
        private List<Image<Bgr, byte>> _samples; // Lista de Images obtenidas con la multiselección
        #endregion

        // Constructor
        public Form1()
        {
            InitializeComponent();
            _selection = new SelectionRectangle(imageBox_Main);
            _imFilter = new ProbabilisticImageFiltering();
            _imFilter.ImageFiltered += OnImageFiltered;
        }
        // LOAD
        private void Form1_Load(object sender, EventArgs e)
        {
            _img = new Image<Bgr, byte>("./images/test.jpg");
            _img = _img.Resize(imageBox_Main.Width, imageBox_Main.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            imageBox_Main.Image = _img;
        }

        protected virtual void OnImageFiltered(Image<Gray, double> i_img, EventArgs e)
        {
            imageBox_output.Image = i_img;
        }

        //
        // Controls
        //

        // ImageBox_Main (MOUSEDOWN)
        private void imageBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Dibujar Rectángulo
                _selection.MouseDown(e.Location);
                Invalidate();
            }
        }
        // ImageBox_Main (MOUSEMOVE)
        private void imageBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _selection.MouseMove(e);
            }
        }
        // ImageBox_Main (PAINT)
        private void imageBox1_Paint(object sender, PaintEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Shift)
            {
                _selection.DrawRectangle(e);
            }
            else
            {
                _selection.DrawRectanglesMulti(e);
            }
        }
        // ImageBox_Main (MOUSEUP)
        private void imageBox_Main_MouseUp(object sender, MouseEventArgs e)
        {



            if (e.Button == MouseButtons.Left)
            {
                if (Control.ModifierKeys != Keys.Shift)  // Muestra de un único rectángulo
                {
                    Image<Bgr, byte> sample = _selection.GetSelectedImage<Bgr, byte>(_img);
                    if (sample != null)
                    {
                        imageBox_sample.Image = sample.Resize(imageBox_sample.Width, imageBox_sample.Height, INTER.CV_INTER_LINEAR);
                        _imFilter.SetDistributionValues<Bgr>(sample);
                        _imFilter.FilterImage<Bgr>(_img);
                        UpdateMeanandMatrixDisplayValues();
                    }
                }
                else  // Añadir rectángulo, para muestra multi rectángulo
                {
                    _selection.MouseUpMulti(e);
                }
            }
            else if (e.Button == MouseButtons.Right && Control.ModifierKeys == Keys.Shift) // Muestra de un varios rectángulos
            {
                _samples = _selection.GetSelectedImagesMulti<Bgr, byte>(_img);
                _selection.RemoveMulti();
                _imFilter.SetDistributionValues<Bgr>(_samples);
                _imFilter.FilterImage<Bgr>(_img);
                // Mostramos la media de los valores BGR como un color
                if (_imFilter.NormalDistribution != null)
                {
                    var sample = new Image<Bgr, double>(1, 1);
                    double[] mean = _imFilter.NormalDistribution.Mean;
                    sample.Data[0, 0, 0] = mean[0];
                    sample.Data[0, 0, 1] = mean[1];
                    sample.Data[0, 0, 2] = mean[2];
                    imageBox_sample.Image = sample.Resize(imageBox_sample.Width, imageBox_sample.Height, INTER.CV_INTER_LINEAR);
                }
                UpdateMeanandMatrixDisplayValues();
            }

        }

        private void UpdateMeanandMatrixDisplayValues()
        {
            label_colorAvg.Text = (int)_imFilter.NormalDistribution.Mean[0] + ", " + (int)_imFilter.NormalDistribution.Mean[1] + ", " + (int)_imFilter.NormalDistribution.Mean[2];
            label_cov_11.Text = _imFilter.NormalDistribution.Covariance[0, 0] + "";
            label_cov_12.Text = _imFilter.NormalDistribution.Covariance[0, 1] + "";
            label_cov_13.Text = _imFilter.NormalDistribution.Covariance[0, 2] + "";
            label_cov_21.Text = _imFilter.NormalDistribution.Covariance[1, 0] + "";
            label_cov_22.Text = _imFilter.NormalDistribution.Covariance[1, 1] + "";
            label_cov_23.Text = _imFilter.NormalDistribution.Covariance[1, 2] + "";
            label_cov_31.Text = _imFilter.NormalDistribution.Covariance[2, 0] + "";
            label_cov_32.Text = _imFilter.NormalDistribution.Covariance[2, 1] + "";
            label_cov_33.Text = _imFilter.NormalDistribution.Covariance[2, 2] + "";
        }
    }
}
