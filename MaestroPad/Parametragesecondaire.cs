using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Media.Midi;
using System.Threading;
using System.Collections.Concurrent;
using Java.Interop;
using static Java.Interop.JniEnvironment;

namespace MaestroPad
{
    [Activity(Label = "Parametragesecondaire")]
    public class Parametragesecondaire : Activity
    {

        EnvoiViaMidi monenvoi;
        private MidiManager manager;
        public static int tempoval = 1;
        public static int indicateur = 0;
        public static int VELOCITY = 0;
        public static int valnote = 1;
        public static int valnumerateur=0;
        public static int valdenominateur = 0;
        public static int nombresdemesure = 0;
        string  nom = null;


        Thread mythread;
        
        [Export("ModeMesure_onclick")]
        0 References 
        public void ModeMesure_onclick(ViewAnimator v)
        {
            switch (v.Id)
            {
                case Resource.Id.Rien:
                    valnote = 1;
                    break;
                case Resource.Id.Binaire:
                    valnote = 2;
                    break;
                case Resource.Id.Ternaire:
                    valnote = 3;
                    break;
            }
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Parametragesecondaire);

            //recuperaion des données de l'étape precedente 
            nom = Intent.GetStringExtra("nom") ?? "nom not available";
            string mesure = Intent.GetStringExtra("nombresdemesure") ?? "nombresdemesure not available";
            nombresdemesure = Convert.ToInt32(mesure);
            string numerateur = Intent.GetStringExtra("numerateur") ?? "numerateur not available";
            valnumerateur = Convert.ToInt32(numerateur);
            string denominateur = Intent.GetStringExtra("denominateur") ?? "denominateur not available";
            valdenominateur = Convert.ToInt32(denominateur);

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

            // choix du mode de la mesure 
            RadioGroup choixmode = FindViewById<RadioGroup>(Resource.Id.ModeMesure);
            RadioButton modechoisi = FindViewById<RadioButton>(choixmode.CheckedRadioButtonId);






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
                    monenvoi.noteOn(1, valnumerateur, temps);
                }
                else
                {
                    Thread.Sleep(tempoval);
                    monenvoi.noteOff(2, valnote, temps - 1);
                    monenvoi.noteOn(1, valnumerateur, temps);

                    // verif_tempo();
                }
                temps++;
                if (temps > valnumerateur)
                {
                    Thread.Sleep(tempoval);
                    monenvoi.noteOff(2, valnote, temps - 1);
                    monenvoi.noteOn(1, valnumerateur, temps);
                    Thread.Sleep(tempoval);
                    monenvoi.noteOff(2, valnote, temps);
                    temps = 1;

                }
            }


        }
    }
}