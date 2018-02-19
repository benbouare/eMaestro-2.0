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
        string tmp = null;
        static long[] nuances;
        int mod = 1;


        Thread mythread;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //recuperation des données à retoucher pour gerer les types
            nom = Intent.GetStringExtra("nom") ?? "nom not available";
            string mesure = Intent.GetStringExtra("nombresdemesure") ?? "nombresdemesure not available";
            nombresdemesure = Convert.ToInt32(mesure);
            string numerateur = Intent.GetStringExtra("numerateur") ?? "numerateur not available";
            valnumerateur = Convert.ToInt32(numerateur);
            string denominateur = Intent.GetStringExtra("denominateur") ?? "denominateur not available";
            valdenominateur = Convert.ToInt32(denominateur);
            for (int j = 1; j <= nombresdemesure; j++)
            {
                string tmp = Intent.GetStringExtra("mesure " + j);
                nuances[j - 1] = Convert.ToInt64(tmp);
            }

            //les boutons 
            Button lancer = FindViewById<Button>(Resource.Id.lanceur);
            Button pause = FindViewById<Button>(Resource.Id.Stopped);
            Button stopper = FindViewById<Button>(Resource.Id.Stopped);
            Button Retour = FindViewById<Button>(Resource.Id.Backmesure);


            //event des buttons
            lancer.Click += delegate
            {
                mythread.Start();
            };

            pause.Click += delegate
            {
                lancer.Text = "Continuer";
                mythread.Suspend();
            };

            //parametrage de midi et threads
            manager = (MidiManager)GetSystemService(MidiService);
            mythread = new Thread(new ThreadStart(envoi));

            // Create your application here

            //instanciation de la classe envoi
            monenvoi = new EnvoiViaMidi(manager, this);

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

                    }
                }

            }
            if (valnote == 27)//ternaire
            {
                // int tmp = 2; // pour recuperer la dernière valeur du noteoff à partir du temps 
                int ind = 0;
                valnumerateur = valnumerateur / 3;

                while (Thread.CurrentThread.IsAlive)
                {
                    //envoi  noteON et noteOFF
                    if (temps == 1)
                    {
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
                    }


                }
            }



        }
    }
}