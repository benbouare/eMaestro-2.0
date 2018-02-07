using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media.Midi;
using System;
using System.Threading;

namespace TestEnvoi
{
    [Activity(Label = "Envoi couleur", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private MidiManager manager;
        private PortEnvoi port;
        private byte[] buffer = new byte[3];
        private Button bleu;
        private Button rouge;
        private Button vert;
        private TextView texte;
        private static int DEFAULT_VELOCITY = 64;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            bleu = (Button) FindViewById<Button>(Resource.Id.buttonBleu);
            vert = (Button) FindViewById<Button>(Resource.Id.buttonVert);
            rouge = (Button) FindViewById<Button>(Resource.Id.buttonRouge);
            

            setupMidi();

        }
        private void setupMidi()
        {
            manager = (MidiManager) GetSystemService(MidiService);
            if (manager == null)
            {
                Toast.MakeText(this, "MidiManager is null!", ToastLength.Long).Show();
                return;
            }
            port = new PortEnvoi(manager, this);

            //Bouton bleu
            bleu.Click += (sender, e) =>
            {
                noteOn(1, 50, DEFAULT_VELOCITY);
            };

            //Bouton Vert
            vert.Click += (sender, e) =>
            {
                noteOff(1, 50, DEFAULT_VELOCITY);
            };

            //Bouton Rouge
            rouge.Click += (sender, e) =>
            {
                noteOn(1, 50, DEFAULT_VELOCITY);
                Thread.Sleep(1000);
                noteOff(1, 50, DEFAULT_VELOCITY);
            };
        }

        private void noteOn(int channel, int note, int velocity)
        {
            midiCommand(0x90 + channel, note, velocity);
        }

        private void noteOff(int channel, int note, int velocity)
        {
            midiCommand(0x80 + channel, note, velocity);
        
        }

        private void midiCommand(int status, int data1, int data2)
        {
            buffer[0] = (byte) status;
            buffer[1] = (byte) data1;
            buffer[2] = (byte) data2;
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
            }
            catch(Exception e)
            {
                Toast.MakeText(this, "Sending Error : "+e, ToastLength.Long).Show();
            }
        }
    }
}

