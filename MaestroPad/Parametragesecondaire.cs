using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Media.Midi;
using System.Threading;
using Java.Interop;
using System.Reflection.Emit;
using static Java.Interop.JniEnvironment;
using Java.Lang.Ref;
using Java.Util.Prefs;
using Android.Views;



namespace MaestroPad
{
    [Activity(Label = "Parametragesecondaire")]
    public class Parametragesecondaire : Activity
    {

        EnvoiViaMidi monenvoi;
        private MidiManager manager;
        public static int tempoval = 1;
        public static int indicateur = 1;
        public static int VELOCITY = 0;
        public static int valnote = 1;
        public static int valnumerateur=0;
        public static int valdenominateur = 0;
        public static int nombresdemesure = 0;
        string  nom = null;


        Thread mythread;



        [Export("ModeMesure_onclick")]
        public void ModeMesure_onclick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.Rien:
                    valnote = 25;
                    break;
                case Resource.Id.Binaire:
                    valnote = 26;
                    break;
                case Resource.Id.Ternaire:
                    valnote = 27;
                    break;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Parametragesecondaire);

            //recuperaion des données de l'étape precedente on doit les envoyer à l'activité suivante afin de les stocker dand un objet de type Mapartition
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
                    if(indicateur % 2 == 0)
                    {
                        if (indicateur == 2)
                        {
                            mythread.Start();//lancer
                            
                        }
                        else
                        {
                            mythread.Resume();  //reprendre après pause
                        }
                       
                    }
                    else
                    {
                        mythread.Suspend(); // mettre pause
                    }
                    
                }
                else
                {
                    indicateur = 1;
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
            if (valnote == 25)//rien
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
                        monenvoi.noteOn(1, valnumerateur, valnote);
                        monenvoi.noteOn(1, valnumerateur, temps);
                    }
                    else
                    {
                        Thread.Sleep(tempoval);
                        monenvoi.noteOff(2, 0, temps - 1);
                        monenvoi.noteOn(1, valnumerateur, temps);

                        // verif_tempo();
                    }
                    temps++;
                    if (temps > valnumerateur-1)
                    {
                        Thread.Sleep(tempoval);
                        monenvoi.noteOff(2, 0, temps - 1);
                        monenvoi.noteOn(1, valnumerateur, temps);
                        Thread.Sleep(tempoval);
                        monenvoi.noteOff(2, 0, temps);
                        temps = 1;

                    }
                }
            }
            if (valnote == 26)//binaire
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
                        monenvoi.noteOn(1, valnumerateur, valnote);
                        monenvoi.noteOn(1, valnumerateur, temps);
                    }
                    else
                    {
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 0, temps - 1);
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 1, temps - 1);
                        monenvoi.noteOn(1, valnumerateur, temps);

                        // verif_tempo();
                    }
                    temps++;
                    if (temps > valnumerateur - 1)
                    {
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 0, temps - 1);
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 1, temps - 1);
                        monenvoi.noteOn(1, valnumerateur, temps);
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 0, temps);
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 1, temps);
                        temps = 1;

                    }
                }
                
            }
            if(valnote == 27)//ternaire
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
                        monenvoi.noteOn(1, valnumerateur, valnote);
                        monenvoi.noteOn(1, valnumerateur, temps);
                    }
                    else
                    {
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 0, temps - 1);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 1, temps - 1);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 2, temps - 1);
                        monenvoi.noteOn(1, valnumerateur, temps);

                        // verif_tempo();
                    }
                    temps++;
                    if (temps > valnumerateur - 1)
                    {
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 0, temps - 1);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 1, temps - 1);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 2, temps - 1);
                        monenvoi.noteOn(1, valnumerateur, temps);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 0, temps);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 1, temps);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 2, temps);
                        temps = 1;

                    }
                }
            }
            


        }
       
    }
}