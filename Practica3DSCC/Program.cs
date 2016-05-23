using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

using GHI.Glide;
using GHI.Glide.Display;
using GHI.Glide.UI;

namespace Practica3DSCC
{
    public partial class Program
    {
       
        private GHI.Glide.Display.Window controlWindow;
        private GHI.Glide.Display.Window camaraWindow;
        private Button btn_start;
        private Button btn_stop;
        private SensorProximidad sensor ; 
             
    
        void ProgramStarted()
        {
     
            sensor = new SensorProximidad(extender);
            
            Debug.Print("Program Started");


            camera.BitmapStreamed += camera_BitmapStreamed; 

            
            controlWindow = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.controlWindow));
            camaraWindow = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.camaraWindow));
            GlideTouch.Initialize();

            
            btn_start = (Button)controlWindow.GetChildByName("start");
            btn_stop = (Button)controlWindow.GetChildByName("stop");
            btn_start.TapEvent += btn_start_TapEvent;
            btn_stop.TapEvent += btn_stop_TapEvent;


            
            sensor.ObjectOn += sensor_ObjectOn;
            sensor.ObjectOff += sensor_ObjectOff;  

            
            Glide.MainWindow = controlWindow;


           

            
            
        }

        /// <summary>
        /// Evento que activa el streaming de la camara
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void camera_BitmapStreamed(Camera sender, Bitmap e)
        {
            displayT35.SimpleGraphics.DisplayImage(e, 0, 0);
 
        }

        /// <summary>
        /// Evento que obitene el objeto sensor y se dispara cuando detecta algo cerca
        /// </summary>
        void sensor_ObjectOn()
        {
            Debug.Print("Objeto entro");
            camera.StartStreaming();
        }

        /// <summary>
        /// Evento que obtiene el sensor cuando se aleja el objeto 
        /// </summary>
        void sensor_ObjectOff()
        {
            Debug.Print("Objeto salio");
            camera.StopStreaming();

            TextBlock text = (TextBlock)controlWindow.GetChildByName("status");
            text.Text = "Monitoreo ON";
            Glide.MainWindow = controlWindow;
        }

        /// <summary>
        /// Evento del boton cuando es presionado star
        /// </summary>
        /// <param name="sender"></param>
        void btn_start_TapEvent(object sender)
        {
            Debug.Print("Start");
            
            TextBlock text = (TextBlock)controlWindow.GetChildByName("status");
            text.Text = "Monitoreo ON";
            Glide.MainWindow = controlWindow;

            sensor.StartSampling();
        }

        /// <summary>
        /// Evento que lanza el boton cuando se presiona Stopp
        /// </summary>
        /// <param name="sender"></param>
        void btn_stop_TapEvent(object sender)
        {
            Debug.Print("Stop");
            
            TextBlock text = (TextBlock)controlWindow.GetChildByName("status");
            text.Text = "Monitoreo OFF";
            Glide.MainWindow = controlWindow;

            sensor.StopSampling();
        }


    }
}
