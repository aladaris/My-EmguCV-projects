Probabilistic Image Filtering
=============================
  Esta biblioteca está destinada al filtrado de color en imágenes utilizando métodos probabilísticos.
  En concreto se usa una Distribución Normal Multivariante:
  + Se hace uso de una muestra simple o un conjutno de muestras; siendo una muestra una porción de una imagen.
  + Con dicha muestra se calcula la media, de cada uno de los canales de color, y una matriz de covarianza de éstos.
  + Centrando la distribución normal (para cada una de sus variable/canales de color) en el valor de la media. Se puede evaluar la probabilidad que posee un pixel en concreto dentro de dicha distriución.
  + La biblioteca se encarga de convertir una imagen a color en una imagen en escala de grises, en la que la intensidad de cada pixel representa la probabilidad de ese pixel en la imagen original.

Dependencias:
-------------
  + Biblioteca [EmguCV](http://www.emgu.com/wiki/index.php/Main_Page).
  + Biblioteca [Accord.Statistics](http://accord-framework.net/).
    
