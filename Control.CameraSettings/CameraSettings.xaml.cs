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
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DirectShowLib;

namespace Aladaris
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class CameraSettingsControl : UserControl
    {
        #region Atributes
        private bool _initialized = false;
        private bool _calculatingParamRange = false;
        private Color _labelColorNormal = Color.FromRgb(0, 105, 205);
        private Color _labelColorDisabled = Color.FromRgb(150, 150, 160);
        private Color _rectangleIndicatorOK = Color.FromRgb(15, 255, 50);
        private Color _rectangleIndicatorWorking = Color.FromRgb(255, 195, 25);
        CalibrationParametersMaxValues _calib;
        List<Tuple<Slider, Label>> _slidersAndLabels = new List<Tuple<Slider, Label>>();
        #region DirecShow Mode
        #endregion
        #endregion
        #region Properties
        public bool IsCalculatingParamRange
        {
            get
            {
                return _calculatingParamRange;
            }
        }
        #endregion
        #region Data Structures
        private enum Activation { Activate, Deactivate };
        /*
        * Struct utilizado para manejar/encapsular la información sobre los valores 
        * de los parámetros de la cámara.
        */
        [Serializable]
        struct CalibrationParametersMaxValues
        {
            public int brightnessMax;     // 0
            public int contrastMax;       // 1
            public int gainMax;           // 2
            public int gammaMax;          // 3
            public int hueMax;            // 4
            public int saturationMax;     // 5
            public int sharpnessMax;      // 7
            public int whiteBalanceMax;   // 8
            public int brightnessMin;     // 9
            public int contrastMin;       // 10
            public int gainMin;           // 11
            public int gammaMin;          // 12
            public int hueMin;            // 13
            public int saturationMin;     // 14
            public int sharpnessMin;      // 15
            public int whiteBalanceMin;   // 16
            public Tuple<int, int> this[int i]
            {
                get
                {
                    switch (i)
                    {
                        case 0: return new Tuple<int, int>(brightnessMin, brightnessMax);
                        case 1: return new Tuple<int, int>(contrastMin, contrastMax);
                        case 2: return new Tuple<int, int>(gainMin, gainMax);
                        case 3: return new Tuple<int, int>(gammaMin, gammaMax);
                        case 4: return new Tuple<int, int>(hueMin, hueMax);
                        case 5: return new Tuple<int, int>(saturationMin, saturationMax);
                        case 6: return new Tuple<int, int>(sharpnessMin, sharpnessMax);
                        case 7: return new Tuple<int, int>(whiteBalanceMin, whiteBalanceMax);
                        default: return new Tuple<int, int>(-1, -1);
                    }
                }
            }
        };
        #endregion

        public CameraSettingsControl()
        {
            InitializeComponent();
            PopulateSlidersAndLabels();
        }

        private void PopulateSlidersAndLabels()
        {
            _slidersAndLabels.Add(new Tuple<Slider, Label>(Slider_brightness, Label_brightness));
            _slidersAndLabels.Add(new Tuple<Slider, Label>(Slider_contrast, Label_contrast));
            _slidersAndLabels.Add(new Tuple<Slider, Label>(Slider_gamma, Label_gamma));
            _slidersAndLabels.Add(new Tuple<Slider, Label>(Slider_gain, Label_gain));
            _slidersAndLabels.Add(new Tuple<Slider, Label>(Slider_hue, Label_hue));
            _slidersAndLabels.Add(new Tuple<Slider, Label>(Slider_saturation, Label_saturation));
            _slidersAndLabels.Add(new Tuple<Slider, Label>(Slider_sharpness, Label_sharpness));
            _slidersAndLabels.Add(new Tuple<Slider, Label>(Slider_wb, Label_wb));
        }

        /// <summary>
        /// Inicializa el control. Intenta cargar desde el fichero el rango de valores
        /// de los sliders; si falla realiza el cálculo de éstos mediante peticiones
        /// al dispositivo (esta operación se realiza en segundo plano y requiere
        /// algo de tiempo)
        /// </summary>
        /// <param name="i_cap">Dispositivo de captura deseado</param>
        public void InitControl()
        {

            _initialized = true;
            _calib = new CalibrationParametersMaxValues();

            Rectangle_indicator.Fill = new SolidColorBrush(_rectangleIndicatorWorking);
            // La obtención del rango de los parámetros se realiza en segundo plano
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = false;
            bw.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    _calculatingParamRange = true;
                    BackgroundWorker b = o as BackgroundWorker;
                    GetParametersRange();
                });
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate(object o, RunWorkerCompletedEventArgs args)
                {
                    _calculatingParamRange = false;
                    SetSlidersRange();
                    DrawSlidersValues();
                    Rectangle_indicator.Fill = new SolidColorBrush(_rectangleIndicatorOK);
                });
            bw.RunWorkerAsync();

        }

        #region DirecShow Mode
        private void GetParametersRange()
        {
            DsDevice[] capDevices;
            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            DsDevice dev = capDevices[0];  // TODO: Almacenar en la clase el ID del dispositivo (no usar este 0)
            object o;
            Guid IID_IBaseFilter = new Guid("56a86895-0ad4-11ce-b03a-0020af0ba770");
            dev.Mon.BindToObject(null, null, ref IID_IBaseFilter, out o);
            IAMVideoProcAmp vProps = (IAMVideoProcAmp)o;
            VideoProcAmpFlags pFlags;
            int dumb;
            vProps.GetRange(VideoProcAmpProperty.Brightness, out _calib.brightnessMin, out _calib.brightnessMax, out dumb, out dumb, out pFlags);
            vProps.GetRange(VideoProcAmpProperty.Contrast, out _calib.contrastMin, out _calib.contrastMax, out dumb, out dumb, out pFlags);
            vProps.GetRange(VideoProcAmpProperty.Gain, out _calib.gainMin, out _calib.gainMax, out dumb, out dumb, out pFlags);
            vProps.GetRange(VideoProcAmpProperty.Gamma, out _calib.gammaMin, out _calib.gammaMax, out dumb, out dumb, out pFlags);
            vProps.GetRange(VideoProcAmpProperty.Hue, out _calib.hueMin, out _calib.hueMax, out dumb, out dumb, out pFlags);
            vProps.GetRange(VideoProcAmpProperty.Saturation, out _calib.saturationMin, out _calib.saturationMax, out dumb, out dumb, out pFlags);
            vProps.GetRange(VideoProcAmpProperty.Sharpness, out _calib.sharpnessMin, out _calib.sharpnessMax, out dumb, out dumb, out pFlags);
            vProps.GetRange(VideoProcAmpProperty.WhiteBalance, out _calib.whiteBalanceMin, out _calib.whiteBalanceMax, out dumb, out dumb, out pFlags);
        }

        private void DrawSlidersValues()
        {
            DsDevice[] capDevices;
            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            DsDevice dev = capDevices[0];  // TODO: Almacenar en la clase el ID del dispositivo (no usar este 0)
            object o;
            Guid IID_IBaseFilter = new Guid("56a86895-0ad4-11ce-b03a-0020af0ba770");
            dev.Mon.BindToObject(null, null, ref IID_IBaseFilter, out o);
            IAMVideoProcAmp vProps = (IAMVideoProcAmp)o;
            VideoProcAmpFlags pFlags;
            int value;
            vProps.Get(VideoProcAmpProperty.Brightness, out value, out pFlags);
            Slider_brightness.Value = value;
            vProps.Get(VideoProcAmpProperty.Contrast, out value, out pFlags);
            Slider_contrast.Value = value;
            vProps.Get(VideoProcAmpProperty.Gain, out value, out pFlags);
            Slider_gain.Value = value;
            vProps.Get(VideoProcAmpProperty.Gamma, out value, out pFlags);
            Slider_gamma.Value = value;
            vProps.Get(VideoProcAmpProperty.Hue, out value, out pFlags);
            Slider_hue.Value = value;
            vProps.Get(VideoProcAmpProperty.Saturation, out value, out pFlags);
            Slider_saturation.Value = value;
            vProps.Get(VideoProcAmpProperty.Sharpness, out value, out pFlags);
            Slider_sharpness.Value = value;
            vProps.Get(VideoProcAmpProperty.WhiteBalance, out value, out pFlags);
            Slider_wb.Value = value;
        }

        private void SetParameterValue(VideoProcAmpProperty i_prop, int value)
        {
            DsDevice[] capDevices;
            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            DsDevice dev = capDevices[0];  // TODO: Almacenar en la clase el ID del dispositivo (no usar este 0)
            object o;
            Guid IID_IBaseFilter = new Guid("56a86895-0ad4-11ce-b03a-0020af0ba770");
            dev.Mon.BindToObject(null, null, ref IID_IBaseFilter, out o);
            IAMVideoProcAmp vProps = (IAMVideoProcAmp)o;
            VideoProcAmpFlags pFlags = new VideoProcAmpFlags();
            vProps.Set(i_prop, value, pFlags);
        }
        #endregion

        #region GUI stuff
        /// <summary>
        /// Establece el valor MAXIMUN a los Sliders basandose en los datos obtenidos, bien del
        /// fichero de configuración de calibrado o del cálculo de éstos mediante consultas al dispositivo
        /// </summary>
        private void SetSlidersRange()
        {
            int proIndex = 0;
            foreach (Tuple<Slider, Label> sl in _slidersAndLabels)
            {
                SetSliderRangeValue(sl.Item1, sl.Item2, _calib[proIndex]);
                proIndex++;
            }
        }

        /// <summary>
        /// Establece el rango y comprueba que es consistente y de no ser así, desactiva el Slider
        /// </summary>
        /// <param name="i_slider">Slider a comprobar/establecer</param>
        /// <param name="i_label">Label de control del valor asociada</param>
        /// <param name="i_value">Valor a establecer</param>
        private void SetSliderRangeValue(Slider i_slider, Label i_label, Tuple<int, int> i_rangeValues)
        {
            i_slider.Minimum = i_rangeValues.Item1;
            i_slider.Maximum = i_rangeValues.Item2;
            if (i_slider.Minimum >= i_slider.Maximum)
                ToggleSlider(i_slider, i_label, Activation.Deactivate);
            else
                ToggleSlider(i_slider, i_label, Activation.Activate);
        }

        private void ToggleSlider(Slider i_slider, Label i_label, Activation i_act)
        {
            switch (i_act)
            {
                case Activation.Activate:
                    i_slider.IsEnabled = true;
                    i_label.Foreground = new SolidColorBrush(_labelColorNormal);
                    break;
                case Activation.Deactivate:
                    i_slider.IsEnabled = false;
                    i_label.Foreground = new SolidColorBrush(_labelColorDisabled);
                    break;
            }
        }

        private void ToggleAllSlidersActive(Activation i_toggle)
        {
            int proIndex = 0;
            foreach (Tuple<Slider, Label> sl in _slidersAndLabels)
            {
                ToggleSlider(sl.Item1, sl.Item2, i_toggle);
                proIndex++;
            }
        }
        #endregion

        #region Sliders Event Handlers
        private void Slider_brightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Label_brightness.Content = "[ " + e.NewValue.ToString() + " ]";
            if (_initialized)
                SetParameterValue(VideoProcAmpProperty.Brightness, (int)e.NewValue);
        }

        private void Slider_contrast_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Label_contrast.Content = "[ " + e.NewValue.ToString() + " ]";
            if (_initialized)
                SetParameterValue(VideoProcAmpProperty.Contrast, (int)e.NewValue);
        }

        private void Slider_saturation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Label_saturation.Content = "[ " + e.NewValue.ToString() + " ]";
            if (_initialized)
                SetParameterValue(VideoProcAmpProperty.Saturation, (int)e.NewValue);
        }

        private void Slider_hue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Label_hue.Content = "[ " + e.NewValue.ToString() + " ]";
            if (_initialized)
                SetParameterValue(VideoProcAmpProperty.Hue, (int)e.NewValue);
        }

        private void Slider_gain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Label_gain.Content = "[ " + e.NewValue.ToString() + " ]";
            if (_initialized)
                SetParameterValue(VideoProcAmpProperty.Gain, (int)e.NewValue);
        }

        private void Slider_exposure_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Label_gamma.Content = "[ " + e.NewValue.ToString() + " ]";
            if (_initialized)
                SetParameterValue(VideoProcAmpProperty.Gamma, (int)e.NewValue);
        }

        private void Slider_wbred_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Label_wb.Content = "[ " + e.NewValue.ToString() + " ]";
            if (_initialized)
                SetParameterValue(VideoProcAmpProperty.WhiteBalance, (int)e.NewValue);
        }


        private void Slider_sharpness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Label_sharpness.Content = "[ " + e.NewValue.ToString() + " ]";
            if (_initialized)
                SetParameterValue(VideoProcAmpProperty.Sharpness, (int)e.NewValue);
        }
        #endregion
    }
}
