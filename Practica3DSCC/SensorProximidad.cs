using System;
using Microsoft.SPOT;
using GTM = Gadgeteer.Modules;
using GT = Gadgeteer;

namespace Practica3DSCC
{
    // Referencia tipo "delegate" para función callback ObjectOn
    public delegate void ObjectOnEventHandler();

    // Referencia tipo "delegate" para función callback ObjectOff
    public delegate void ObjectOffEventHandler();


    /*
     * Clase SensorProximidad, encapsula el funcionanmiento del sensor de proximidad infrarrojo.
     * Esta clase gestiona los dos componentes del sensor: el LED infrarrojo y el foto-transistor.
     * Además, dispara dos eventos: ObjectOn y ObjectOff cuando el sensor detecta la presencia o
     * ausencia de un objeto.
     */
    class SensorProximidad
    {
        
        public event ObjectOffEventHandler ObjectOff;

        public event ObjectOnEventHandler ObjectOn;

        private GT.Timer timer;
        private bool tip = false;

        /*private enum ESTADO
        {
            APAGADO,
            MONITOREANDO,
            CAPTURAR
        }

        ESTADO Estado;*/

        private GT.SocketInterfaces.AnalogInput entrada = null;
        private GT.SocketInterfaces.DigitalOutput salida = null;
        
        public SensorProximidad(GTM.GHIElectronics.Extender extender)
        {

            entrada = extender.CreateAnalogInput(GT.Socket.Pin.Three);
            salida = extender.CreateDigitalOutput(GT.Socket.Pin.Five, false);
            timer = new GT.Timer(1000);
            timer.Tick += timer_Tick;
            //Estado = ESTADO.APAGADO;  
        }

        void timer_Tick(GT.Timer timer)
        {
            Double voltaje = entrada.ReadVoltage();
            Debug.Print("Voltaje: " + entrada.ReadVoltage());

            if (voltaje < 1 )
            {
                //Estado = ESTADO.CAPTURAR;
                if (!tip)
                {
                    ObjectOn();
                    tip = true;
                }
            }
            else
            {
                //Estado = ESTADO.MONITOREANDO;
                ObjectOff();
                tip = false;
            }
        }

        public void StartSampling()
        {
         
            Debug.Print("Voltaje: " + entrada.ReadVoltage());
            salida.Write(true);
            Debug.Print("Encendido");
            timer.Start();

            //Estado = ESTADO.MONITOREANDO;
        }

        public void StopSampling()
        {
            
            salida.Write(false);
            Debug.Print("Apagado");
            timer.Stop();

            //Estado = ESTADO.APAGADO;
        }
    }
}
