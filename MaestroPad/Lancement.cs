using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Media.Midi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MaestroPad
{
    [Activity(Label = "Lancement")]
    public class Lancement : Activity
    {
        EnvoiViaMidi monenvoi;
        private MidiManager manager;
        public static int tempoval = 1;
        public static int indicateur = 1;
        public static int VELOCITY = 0;
        public static int valnote = 25;//donne la valeur du mode choisi
        public static int valnumerateur = 0;
        public static int valdenominateur = 0;
        public static int nombresdemesure = 0;
        string nom = null;
        // string tmp = null;
        //information du tabeau de mesures
        static int[,] Mesures;
        public static int colonne = 7;
        public static int nuance = 0;
        public static int alerte = 1;
        public static int ModeNuance = 2;
        public static int BoolReprise = 3;
        public static int NumerofinReprise = 4;
        public static int NombreDieses = 5;
        public static int nombreBemols = 6;
        int mod1 = 1;
        int mod2 = 1;
        int mod3 = 1;
      //  int mod4 = 1;
        public static int position = 0;


        Thread mythread;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Lancement);
            //parametrage de midi et threads
            manager = (MidiManager)GetSystemService(MidiService);
            mythread = new Thread(new ThreadStart(envoi));

            // Create your application here

            //instanciation de la classe envoi
            monenvoi = new EnvoiViaMidi(manager, this);

            //recuperation des données 
            nom = Intent.GetStringExtra("nom") ?? "nom not available";
            string mesure = Intent.GetStringExtra("nombresdemesure") ?? "nombresdemesure not available";
            nombresdemesure = Convert.ToInt32(mesure);

            //initialisation 
            Mesures = new int[nombresdemesure,colonne];
           /* for (int i = 0; i < nombresdemesure; i++)
            {
                nuances[i] = 0; //initialisation des nuances à aucune nuance pour chaque mesure
            }
            */
            string numerateur = Intent.GetStringExtra("valeurnumerateur") ?? "numerateur not available";
            valnumerateur = Convert.ToInt32(numerateur);
            string denominateur = Intent.GetStringExtra("valeurdenominateur") ?? "denominateur not available";
            valdenominateur = Convert.ToInt32(denominateur);
            string tempo = Intent.GetStringExtra("tempoval") ?? "valeurdutempo not available";
            tempoval = Convert.ToInt32(tempo);
            string mode = Intent.GetStringExtra("valeurmode") ?? "valeurdumode not available";
            valnote = Convert.ToInt32(mode);
            for (int j = 0; j < nombresdemesure; j++)
            {
                int nummesure = j + 1;
                string tmp = Intent.GetStringExtra("mesure " + nummesure + "nuance");
                string tmp2 = Intent.GetStringExtra("mesure " + nummesure + "alerte");
                string tmp3 = Intent.GetStringExtra("mesure " + nummesure + "ModeNuance");
                string tmp4 = Intent.GetStringExtra("mesure " + nummesure + "BoolReprise");
                string tmp5 = Intent.GetStringExtra("mesure " + nummesure + "NumerofinReprise");
                string tmp6 = Intent.GetStringExtra("mesure " + nummesure + "NombreDieses");
                string tmp7 = Intent.GetStringExtra("mesure " + nummesure + "nombreBemols");
                Mesures[j, nuance] = Convert.ToInt32(tmp);
                Mesures[j, alerte] = Convert.ToInt32(tmp2);
                Mesures[j, ModeNuance] = Convert.ToInt32(tmp3);
                Mesures[j, BoolReprise] = Convert.ToInt32(tmp4);
                Mesures[j, NumerofinReprise] = Convert.ToInt32(tmp5);
                Mesures[j, NombreDieses] = Convert.ToInt32(tmp6);
                Mesures[j, nombreBemols] = Convert.ToInt32(tmp7);
            }

            //les boutons 
            Button lancer = FindViewById<Button>(Resource.Id.lanceur);
            Button pause = FindViewById<Button>(Resource.Id.Paused);
            Button stopper = FindViewById<Button>(Resource.Id.Stopped);
            Button Retour = FindViewById<Button>(Resource.Id.Backmesure);


            //event des buttons
            lancer.Click += delegate
            {
                Toast.MakeText(ApplicationContext, Mesures[2, NumerofinReprise].ToString(), ToastLength.Long).Show();
                mod1++;
                if((mod1 % 2)== 0)
                {
                        mythread.Start();
                        string message = " Lancement ";
                        Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
                    
                }
                else
                {
                    string message = " Excecution en cours ";
                    Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
                }
               
            };

            pause.Click += delegate
            {
                mod2++; 
                if(Thread.CurrentThread.IsAlive)
                {
                    pause.Text = "Continuer";
                    mythread.Suspend();
                    string message = " Pause";
                    Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
                }
                else
                {
                    if((mod2 % 2) == 0 && !Thread.CurrentThread.IsAlive)
                    {
                        string message = " Lancer une excecution ";
                        Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
                    }
                    else
                    {
                        mythread.Resume();
                    }
                
                }
                stopper.Click += delegate
                {
                    if (Thread.CurrentThread.IsAlive)
                    {
                        mythread.Abort();
                        Finish();
                    }
                };
                Retour.Click += delegate
                {
                    Intent back = new Intent(this, typeof(ParametrageMesures));
                    Finish();
                };

            };

          

        }
        public void envoi()
        {
            int dejapasse = 0;
            int intermediaire = -1;
            int temps = 1;
            //int tampon = 0;
            if (valnote == 25)//rien
            {
                while (position < nombresdemesure)
                {
                    if (Mesures[position, BoolReprise] == 1)
                    {
                        intermediaire = position;
                        //tampon = Mesures[intermediaire, NumerofinReprise] - 1;
                    }
                    if ((intermediaire >=0) && (position > (Mesures[intermediaire, NumerofinReprise] - 1)) && (dejapasse == 0))
                    {
                        position = intermediaire;
                        dejapasse = 1;
                    }
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
                        monenvoi.Keypressure(temps, Mesures[position,nombreBemols], Mesures[position, NombreDieses]);
                        monenvoi.controlChange(Mesures[position, alerte], Mesures[position, nuance], temps);//envoi de la nuance et l'alerte
                        monenvoi.noteOn(1, valnumerateur, valnote);
                        monenvoi.noteOn(1, valnumerateur, temps);
                       
                    }
                    else
                    {
                        Thread.Sleep(tempoval);
                        monenvoi.noteOff(2, 1, temps - 1);
                        monenvoi.noteOn(1, valnumerateur, temps);
                    }
                    temps++;
                    if (temps > valnumerateur - 1)
                    {
                        Thread.Sleep(tempoval);
                        monenvoi.noteOff(2, 1, temps - 1);
                        if (valnumerateur != 1)
                        {
                            monenvoi.noteOn(1, valnumerateur, temps);
                            Thread.Sleep(tempoval);
                            monenvoi.noteOff(2, 1, temps);
                        }

                        temps = 1;
                        position++;
                     //  nombresdemesure--;

                    }
                }
            }
            if (valnote == 26)//binaire
            {
                while (position < nombresdemesure)
                {
                    if (Mesures[position, BoolReprise] == 1)
                    {
                        intermediaire = position;
                        //tampon = Mesures[intermediaire, NumerofinReprise] - 1;
                    }
                    if ((intermediaire >= 0) && (position > (Mesures[intermediaire, NumerofinReprise] - 1)) && (dejapasse == 0))
                    {
                        position = intermediaire;
                        dejapasse++;
                    }
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
                        monenvoi.Keypressure(temps, Mesures[position, nombreBemols], Mesures[position, NombreDieses]);
                        monenvoi.controlChange(Mesures[position, alerte], Mesures[position, nuance], temps);//envoi de la nuance
                        monenvoi.noteOn(1, valnumerateur, valnote);
                        monenvoi.noteOn(1, valnumerateur, temps);
                        
                    }
                    else
                    {
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 1, temps - 1);
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 2, temps - 1);
                        monenvoi.noteOn(1, valnumerateur, temps);

                        // verif_tempo();
                    }
                    temps++;
                    if (temps > valnumerateur - 1)
                    {
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 1, temps - 1);
                        Thread.Sleep(tempoval / 2);
                        monenvoi.noteOff(2, 2, temps - 1);
                        if (valnumerateur != 1)
                        {
                            monenvoi.noteOn(1, valnumerateur, temps);
                            Thread.Sleep(tempoval / 2);
                            monenvoi.noteOff(2, 1, temps);
                            Thread.Sleep(tempoval / 2);
                            monenvoi.noteOff(2, 2, temps);
                        }

                        temps = 1;
                        position++;
                       //nombresdemesure--;

                    }
                }

            }
            if (valnote == 27)//ternaire
            {
                // int tmp = 2; // pour recuperer la dernière valeur du noteoff à partir du temps 
                int ind = 0;
                valnumerateur = valnumerateur / 3;

                while (position < nombresdemesure )
                {
                    if (Mesures[position, BoolReprise] == 1)
                    {
                        intermediaire = position;
                        //tampon = Mesures[intermediaire, NumerofinReprise] - 1;
                    }
                    if ((intermediaire >= 0) && (position > (Mesures[intermediaire, NumerofinReprise] - 1)) && (dejapasse == 0))
                    {
                        position = intermediaire;
                        dejapasse++;
                    }
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
                        monenvoi.Keypressure(temps, Mesures[position, nombreBemols], Mesures[position, NombreDieses]);
                        monenvoi.controlChange(Mesures[position, alerte], Mesures[position, nuance], temps);//envoi de la nuance
                        monenvoi.noteOn(1, valnumerateur, valnote);
                        monenvoi.noteOn(1, valnumerateur, temps);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 1, temps);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 2, temps);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 3, temps);
                        ind++;
                    }
                    else
                    {
                        monenvoi.noteOn(1, valnumerateur, temps);

                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 1, temps);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 2, temps);
                        Thread.Sleep(tempoval / 3);
                        monenvoi.noteOff(2, 3, temps);

                        // tmp = tmp + 2;
                        ind++;

                    }
                    if (valnumerateur != ind)
                    {
                        temps++;
                    }
                    else
                    {
                        temps = 1;
                        //   tmp = 2;
                        ind = 0;
                        position++;
                      // nombresdemesure--;
                    }


                }
            }



        }
    }
}