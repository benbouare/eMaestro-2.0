using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


using Android.Media.Midi;
using System.Threading;

namespace MaestroPad
{
    class EnvoiViaMidi
    {
        private MidiManager manager;
        private Activity activity;
        private PortEnvoi port;
        private byte[] buffer = new byte[3];

        public EnvoiViaMidi(MidiManager managerx, Activity act)
        {
                manager = managerx;
                activity = act;
                port = new PortEnvoi(manager, activity);
         
        }

       

        public void noteOn(int channel, int note, int velocity)
        {
            midiCommand(0x90 + channel, note, velocity);
        }

        public void noteOff(int channel, int note, int velocity)
        {
            midiCommand(0x80 + channel, note, velocity);

        }
        public void controlChange(int channel,int controllernumber,int controllervalue)
        {
            midiCommand(0xB0 + channel, controllernumber, controllervalue);
        }

        public void Keypressure(int channel, int Key, int Pressure)
        {
            midiCommand(0xA0 + channel, Key, Pressure);
        }

        private void midiCommand(int status, int data1, int data2)
        {
            buffer[0] = (byte)status;
            buffer[1] = (byte)data1;
            buffer[2] = (byte)data2;
            midiSend(buffer, 3);
        }

        private void midiSend(byte[] buffer, int count)
        {
            try
            {
                MidiReceiver receiver = port.getReceiver();
                if (receiver != null)
                {
                    receiver.Send(buffer, 0, count);
                }
                else
                {
                    Toast.MakeText(activity, "Receiver Null", ToastLength.Long).Show();
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(activity, "Sending Error : " + e, ToastLength.Long).Show();
            }
        }

    }
}