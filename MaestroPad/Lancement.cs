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
        public static int valnote = 1;//donne la valeur du mode choisi
        public static int valnumerateur = 0;
        public static int valdenominateur = 0;
        public static int nombresdemesure = 0;
        string nom = null;
       // string tmp = null;
        static int[] nuances;
        int mod1 = 1;
        int mod2 = 1;
        int mod3 = 1;
      //  int mod4 = 1;
        int position = 0;


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
            nuances = new int[nombresdemesure];
           /* for (int i = 0; i < nombresdemesure; i++)
            {
                nuances[i] = 0; //initialisation des nuances à aucune nuance pour chaque mesure
            }*/
            
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
                string tmp = Intent.GetStringExtra("mesure " + j+1);
                nuances[j] = Convert.ToInt32(tmp);
            }

            //les boutons 
            Button lancer = FindViewById<Button>(Resource.Id.lanceur);
            Button pause = FindViewById<Button>(Resource.Id.Stopped);
            Button stopper = FindViewById<Button>(Resource.Id.Stopped);
            Button Retour = FindViewById<Button>(Resource.Id.Backmesure);


            //event des buttons
            lancer.Click += delegate
            {
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

            int temps = 1;
            if (valnote == 25)//rien
            {
                while (nombresdemesure!=0)
                {
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
                        monenvoi.noteOn(1, valnumerateur, valnote);
                        monenvoi.noteOn(1, valnumerateur, temps);
                        monenvoi.controlChange(1, nuances[position], temps);//envoi de la nuance
                    }
                    else
                    {
                        Thread.Sleep(tempoval);
                        monenvoi.noteOff(2, 1, temps - 1);
                        monenvoi.noteOn(1, valnumerateur, temps);
                        

                        // verif_tempo();
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
                        nombresdemesure--;

                    }
                }
            }
            if (valnote == 26)//binaire
            {
                while (nombresdemesure!=0)
                {
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
                        monenvoi.noteOn(1, valnumerateur, valnote);
                        monenvoi.noteOn(1, valnumerateur, temps);
                        monenvoi.controlChange(1, nuances[position], temps);//envoi de la nuance
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
                        nombresdemesure--;

                    }
                }

            }
            if (valnote == 27)//ternaire
            {
                // int tmp = 2; // pour recuperer la dernière valeur du noteoff à partir du temps 
                int ind = 0;
                valnumerateur = valnumerateur / 3;

                while (nombresdemesure!=0)
                {
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
                        monenvoi.noteOn(1, valnumerateur, valnote);
                        monenvoi.noteOn(1, valnumerateur, temps);
                        monenvoi.controlChange(1, nuances[position], temps);//envoi de la nuance
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
                        nombresdemesure--;
                    }


                }
            }



        }
    }
}