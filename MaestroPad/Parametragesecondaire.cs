using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Media.Midi;
using System.Threading;
using Android.Views;
using static Java.Interop.JniEnvironment;

namespace MaestroPad
{
    [Activity(Label = "Parametragesecondaire")]
    public class Parametragesecondaire : Activity
    {

        EnvoiViaMidi monenvoi;
        private MidiManager manager;
        public static int tempoval = 1;
        int indicateur = 0;
        public static int VELOCITY = 0;
        public static int valnote = 1;
        Thread mythread;
        //  int compteurTemps = 0;


        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Parametragesecondaire);

            //initialisation du manager
            manager = (MidiManager)GetSystemService(MidiService);
            mythread = new Thread(new ThreadStart(envoi));

            // Create your application here
            Button back = FindViewById<Button>(Resource.Id.back);
            Button envoyer = FindViewById<Button>(Resource.Id.send);
            EditText tempo = FindViewById<EditText>(Resource.Id.tempovaleur);

            // choix de la note des BPM
            RadioGroup choixnotes = FindViewById<RadioGroup>(Resource.Id.notedutempo);
            RadioButton choixnote = FindViewById<RadioButton>(choixnotes.CheckedRadioButtonId);
            


            monenvoi = new EnvoiViaMidi(manager, this);

            back.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(NouvellePartition));
                StartActivity(intent);
            };
            // MainActivity nvll = new MainActivity();
            envoyer.Click += (sender, e) =>
            {
                string tmp = tempo.Text;
                if ((tempoval = Convert.ToInt32(tmp.ToString())) != 0)
                {
                    string note = choixnote.Text.ToString();
                    if (note == "ronde")
                    {
                        valnote = 1;
                    }
                    if (note == "blanche")
                    {
                        valnote = 2;
                    }
                    if (note == "noire")
                    {
                        valnote = 4;
                    }
                    if (note == "croche")
                    {
                        valnote = 8;
                    }
                    if (note == "blanchepointé")
                    {
                        valnote = 3;
                    }
                    if (note == "noirepointé")
                    {
                        valnote = 6;
                    }
                    if (note == "crochepointé")
                    {
                        valnote = 12;
                    }

                    indicateur++;
                    tempoval = (60000 / tempoval);
                    mythread.Start();
                }
                else
                {
                    indicateur = 0;
                    verif_tempo();
                }






            };
        }
        private void verif_tempo()
        {
            //
            string message = string.Empty;

            //
            if (indicateur != 0)
                message = "recuperation et conversion correcte du tempo" + tempoval + " BPM " + VELOCITY;
            else
                message = "echec de la recuperation du tempo";

            //
            Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();

        }
        private void control()
        {

            string message = string.Empty;


            if (tempoval != 0)
                message = "Etape suivante";
            else
                message = "veuillez renseinger le ou les champs avant de continuer";

            Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();

        }
        public void envoi()
        {

            int temps = 1;
            while (Thread.CurrentThread.IsAlive)
            {
                //envoi  noteON et noteOFF
                if (temps == 1)
                {
                    monenvoi.noteOn(1, 120, temps);
                }
                else
                {
                    Thread.Sleep(tempoval);
                    monenvoi.noteOff(2, 120, temps - 1);
                    monenvoi.noteOn(1, 120, temps);

                    // verif_tempo();
                }
                temps++;
                if (temps > 3)
                {
                    Thread.Sleep(tempoval);
                    monenvoi.noteOff(2, 120, temps - 1);
                    monenvoi.noteOn(1, 120, temps);
                    Thread.Sleep(tempoval);
                    monenvoi.noteOff(2, 120, temps);
                    temps = 1;

                }
            }


        }
        [Java.Interop.Export("RadioButton_OnClick")]
        0References
        public void RadioButton_OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.radioButton1:

            }
        }
    }
}