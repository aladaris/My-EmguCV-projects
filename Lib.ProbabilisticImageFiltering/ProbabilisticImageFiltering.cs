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

using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Accord.Statistics.Distributions.Multivariate;
using Accord.Statistics.Distributions.Fitting;

namespace Aladaris
{
    /// <summary>
    /// Delegate para eventos que generan/devuelven objetos Image
    /// </summary>
    /// <typeparam name="C">Espacio de Color</typeparam>
    /// <typeparam name="D">Profundidad</typeparam>
    /// <param name="i_img">Objeto Image devuelto</param>
    /// <param name="e">Argumentos del evento</param>
    public delegate void GeneratedImage<C, D>(Image<C, D> i_img, EventArgs e)
        where C : struct, IColor
        where D : new();

    public class ProbabilisticImageFiltering
    {
        #region Atributes
        private MultivariateNormalDistribution _dist;
        private bool _realTime;
        private bool _filtering = false;
        private int _reductionFactor = 2;
        #endregion

        #region Properties
        /// <summary>
        /// Accesor al objeto de la distribución continua
        /// </summary>
        public MultivariateNormalDistribution NormalDistribution
        {
            get
            {
                return _dist;
            }
        }
        /// <summary>
        /// Obtiene/Establece si se realizará un filtrado de imagenes en tiempo real; es decir,
        /// se filtrará una imagen cada vez que se maneje el evento de nuevi frame.
        /// Una vez establecido a false no se podrá volver a establecer como true.
        /// </summary>
        public bool RealTimeFiltering
        {
            get
            {
                return _realTime;
            }
            set
            {
                if (_realTime == false)
                    _realTime = false;
                else
                    _realTime = value;
            }
        }
        /// <summary>
        /// Cantidad entera por la que se dividirá la imagen original para su procesado.
        /// </summary>
        public int ReductionFactor
        {
            get
            {
                return _reductionFactor;
            }
            set
            {
                if (value > 0)
                    _reductionFactor = value;
            }
        }
        #endregion

        #region Events Stuff
        /// <summary>
        /// Evento generado cada vez que se termina de filtrar una imagen.
        /// </summary>
        public event GeneratedImage<Gray, double> ImageFiltered;
        protected virtual void OnImageFiltered(Image<Gray, double> i_img, EventArgs e)
        {
            if (ImageFiltered != null)
                ImageFiltered(i_img, e);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Constructor por defecto. No permitirá el procesado en tiempo real
        /// pues no se asignará ningún dispositivo de captura a escuchar.
        /// </summary>
        public ProbabilisticImageFiltering()
        {
            _realTime = false;
        }

        /// <summary>
        /// Constructor destinado a la caputra en tiempo real.
        /// </summary>
        /// <param name="i_cap">Dispositivo de captura que genera eventos 'ImageGrabbed'</param>
        /// <param name="i_realTime">¿Realizar la conversión en tiempo real?</param>
        public ProbabilisticImageFiltering(Capture i_cap, bool i_realTime = true)
        {
            _realTime = i_realTime;
            if (_realTime)
                i_cap.ImageGrabbed += OnImageGrabbed;
        }

        /// <summary>
        /// Recibe una imagen (o lista de imágenes) del tipo Image<C, byte>,
        /// construye la matriz (bidimensional) con los datos de cada canal de
        /// color que usa para instanciar el objeto de la distribución normal
        /// multivariada.
        /// </summary>
        /// <typeparam name="C">Color Space</typeparam>
        /// <param name="i_img">Imagen o Lista de imágenes. Image<C, byte> or List<Image<C, byte>></param>
        public void SetDistributionValues<C>(Object i_img)
            where C : struct, IColor
        {
            // TODO: Crear mis propias excepciones para estos menesteres
            double[][] observs;
            if (i_img is Image<C, byte>)
                observs = BuildObservations<C>((Image<C, byte>)i_img);
            else if (i_img is List<Image<C, byte>>)
                observs = BuildObservations<C>((List<Image<C, byte>>)i_img);
            else
                throw new ArgumentException("Imgage<C, byte> and List<Image<C, byte>> are the only suitable types for this Argument", "i_img");

            _dist = new MultivariateNormalDistribution(3);
            NormalOptions options = new NormalOptions() { Regularization = double.Epsilon };
            try
            {
                _dist.Fit(observs);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error adaptando la matriz de datos a la distribución. Variedad de color insuficiente", e);
            }
        }

        /// <summary>
        /// Realiza, en segundo plano, el filtrado de una imagen dada.
        /// Cuando se genera dicha imagen filtrada se generará un evento
        /// del tipo 'ImageFiltered'.
        /// </summary>
        /// <typeparam name="C">Color Space</typeparam>
        /// <param name="i_img">Input image</param>
        public void FilterImage<C>(Image<C, byte> i_img)
            where C : struct, IColor
        {
            if (_dist != null && !_filtering)
            {
                i_img = i_img.Resize(i_img.Width / _reductionFactor, i_img.Height / _reductionFactor, INTER.CV_INTER_LINEAR);
                Image<Gray, double> result = new Image<Gray, double>(i_img.Size);
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = false;
                bw.DoWork += new DoWorkEventHandler(
                    delegate(object o, DoWorkEventArgs args)
                    {
                        _filtering = true;
                        BackgroundWorker b = o as BackgroundWorker;
                        result = GenerateProbabilisticImage<C>(i_img);
                    });
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                    delegate(object o, RunWorkerCompletedEventArgs args)
                    {
                        result = result.Resize(result.Width * _reductionFactor, result.Height * _reductionFactor, INTER.CV_INTER_LINEAR);
                        OnImageFiltered(result, args);
                        _filtering = false;
                    });

                bw.RunWorkerAsync();
            }

        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Construye una imagen en escala de grises a partir de una imagen de entrada.
        /// Cada pixel de la imagen de entrada es representado en escala de grises en
        /// función del valor devuelto por la función de densidad de la distribución
        /// establecida.
        /// </summary>
        /// <typeparam name="C">Color Space</typeparam>
        /// <param name="i_img">Input image</param>
        /// <returns>GrayScale filtered image.</returns>
        private Image<Gray, double> GenerateProbabilisticImage<C>(Image<C, byte> i_img)
            where C : struct, IColor
        {
            if (_dist != null && i_img.Data != null)
            {
                // Calcular y mostrar la imagen de propabilidades
                Image<Gray, double> probImg = new Image<Gray, double>(i_img.Size);
                byte[, ,] imgData = i_img.Data;
                double[, ,] probIdata = probImg.Data;
                const long FACTOR = 100000000000000;  // TODO: Estudiame

                for (int i = 0; i < i_img.Height; i++)
                    for (int j = 0; j < i_img.Width; j++)
                        probIdata[i, j, 0] = EvaluatePixel(new double[] { imgData[i, j, 0], imgData[i, j, 1], imgData[i, j, 2] }) * FACTOR;
                //probImg = probImg.SmoothBlur(10, 10);
                return probImg;
            }
            else
                return null;
        }

        /// <summary>
        /// Comprueba la probabilidad de un pixel dentro de la distribución Normal
        /// multivariada.
        /// </summary>
        /// <param name="i_pixel">Pixel a evaluar. Es un array de longitud 3; un valor por canal de color.</param>
        /// <returns>Probabilidad del pixel evaluado.</returns>
        private double EvaluatePixel(double[] i_pixel)
        {
            if (_dist != null)
                return _dist.ProbabilityDensityFunction(i_pixel);
            else
                return 0d;
        }

        /*
         * Ejemplo de construcción de observaciones:
         * 
         *     1  4  7          3  5  1           0  4  3
         * B = 2  5  8      G = 4  7  9       R = 1  5  4
         *     3  6  9          2  0  0           7  9  2
         *     
         *            (B)(G)(R)
         *             1  3  0
         *             2  4  1
         *             3  2  7
         *             4  5  4
         * RESULTADO = 5  7  5
         *             6  0  9
         *             7  1  3
         *             8  9  4
         *             9  0  2
         */
        /// <summary>
        /// Función encargada de convertir un objeto Image en una matriz del tipo double[][]
        /// preparada para ser usada con un objeto 'MultivariateNormalDistribution'.
        /// </summary>
        /// <typeparam name="C">Color Space</typeparam>
        /// <param name="i_img">Input Image</param>
        /// <returns>Crea una matriz en la que cada unade sus tres columnas contiene todos los datos de cada uno de los tres canales de color de la imagen.</returns>
        private double[][] BuildObservations<C>(Image<C, byte> i_img)
            where C : struct, IColor
        {
            Image<Gray, byte>[] BGR = i_img.Split();  // Obtenemos las 3 matrices bidimensionales (una por canal de color)(todas son de las mismas dimensiones)
            // Calculamos la matriz que en cada columna contiene el contenido de cada matriz bidimensional (de cada canal) puesta en forma de columna (convertida en un vector vertical).
            int nElems = BGR[0].Cols * BGR[0].Rows;
            double[][] observs = new double[nElems][];
            for (int i = 0; i < nElems; i++)
                observs[i] = new double[3];
            // Cada columna de esta matriz contendrá todos los datos de cada una de las matrices de cada canal
            for (int j = 0; j < BGR[0].Cols; j++)
            {
                for (int i = 0; i < BGR[0].Rows; i++)
                {
                    observs[i + (j * BGR[0].Rows)][0] = BGR[0].Data[i, j, 0];
                    observs[i + (j * BGR[0].Rows)][1] = BGR[1].Data[i, j, 0];
                    observs[i + (j * BGR[0].Rows)][2] = BGR[2].Data[i, j, 0];
                }
            }
            return observs;
        }

        /// <summary>
        /// Función encargada de convertir una List de objeto Image en una matriz del tipo double[][]
        /// preparada para ser usada con un objeto 'MultivariateNormalDistribution'.
        /// </summary>
        /// <typeparam name="C">Color Space</typeparam>
        /// <param name="i_img">Input Image</param>
        /// <returns>Crea una matriz en la que cada unade sus tres columnas contiene todos los datos de cada uno de los tres canales de color de la imagen.</returns>
        private double[][] BuildObservations<C>(List<Image<C, byte>> i_imgs)
            where C : struct, IColor
        {
            int nElems = 0;
            // Construcción de la matriz de resultados
            foreach (Image<C, byte> img in i_imgs)  // TODO: Estudiar alguna manera de evitar los dos LOOPS
                nElems += img.Cols * img.Rows;
            double[][] observs = new double[nElems][];
            for (int i = 0; i < nElems; i++)
                observs[i] = new double[3];

            // Llenado de la matriz de resultados
            int imgIndex = 0;
            foreach (Image<C, byte> img in i_imgs)
            {
                Image<Gray, byte>[] BGR = img.Split();  // Obtenemos las 3 matrices bidimensionales (una por canal de color)(todas son de las mismas dimensiones)
                // Cada columna de esta matriz contendrá todos los datos de cada una de las matrices de cada canal
                for (int j = 0; j < BGR[0].Cols; j++)
                {
                    for (int i = 0; i < BGR[0].Rows; i++)
                    {
                        observs[(i + (j * BGR[0].Rows)) + imgIndex][0] = BGR[0].Data[i, j, 0];
                        observs[(i + (j * BGR[0].Rows)) + imgIndex][1] = BGR[1].Data[i, j, 0];
                        observs[(i + (j * BGR[0].Rows)) + imgIndex][2] = BGR[2].Data[i, j, 0];
                    }
                }
                imgIndex = BGR[0].Rows * BGR[0].Cols;
            }


            return observs;
        }

        /// <summary>
        /// Manejador para el evento generado cada vez que el objeto Capture
        /// obtiene un frame del Stream de video.
        /// Lo que hace en este caso es llamar automáticamente al método de filtrado por color (si está activado el modo realtime)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnImageGrabbed(object sender, EventArgs e)
        {
            if (RealTimeFiltering)
            {
                Image<Bgr, byte> img = ((Capture)sender).RetrieveBgrFrame();
                FilterImage<Bgr>(img);
            }
        }
        #endregion
    }
}
