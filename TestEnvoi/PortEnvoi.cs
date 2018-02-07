using System;
using Android.Media.Midi;
using Android.Widget;

namespace TestEnvoi
{
    class PortEnvoi : MidiManager.DeviceCallback, MidiManager.IOnDeviceOpenedListener
    {
        private MidiInputPort inputPort;
        private MidiDeviceInfo portInfo;
        private MidiManager manager;
        private MainActivity activity;
        private TextView texte;
        private int index;


        public PortEnvoi(MidiManager m, MainActivity act)
        {
            manager = m;
            activity = act;
            texte = (TextView)activity.FindViewById<TextView>(Resource.Id.textViewDevice);
            manager.RegisterDeviceCallback(this, new Android.OS.Handler());

            MidiDeviceInfo[] infos = manager.GetDevices();
            foreach (MidiDeviceInfo info in infos)
            {
                onDeviceAdded(info);
            }
            manager.OpenDevice(portInfo, this, null);
        }

        public void OnDeviceOpened(MidiDevice device)
        {
             if(device != null)
             {
                 inputPort = device.OpenInputPort(index);
             }
             else
             {
                Toast.MakeText(activity, "Can't open MIDI Device !", ToastLength.Long).Show();
            }
        }

        public MidiReceiver getReceiver() { return inputPort; }

        public void onDeviceAdded(MidiDeviceInfo info)
        {
            int nombrePort = info.InputPortCount;
            for(int i = 0; i < nombrePort; i++)
            {
                InfosPorts infosPort = new InfosPorts(info, i);
                portInfo = infosPort.getDeviceInfo();
                index = infosPort.getPortIndex();
            }
        }
    }
}