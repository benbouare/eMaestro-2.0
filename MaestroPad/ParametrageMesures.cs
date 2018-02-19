using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media.Midi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MaestroPad
{
    [Activity(Label = "ParametrageMesures")]
    public class ParametrageMesures : Activity
    {
       
        public static int tempoval = 1;
        public static int indicateur = 1;
        public static int VELOCITY = 0;
        public static int valnote = 1;//pour determiner le mode 
        public static int valnumerateur = 0;
        public static int valdenominateur = 0;
        public static int nombresdemesure = 0;
        string nom = null;
        string num = null;
        string nua = null;
       // ArrayList tabdeboutons = new ArrayList();
        static long[] nuances;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //reception du nombre de mesures 
            string mesure = Intent.GetStringExtra("nombresdemesure") ?? "nombresdemesure not available";
            nombresdemesure = Convert.ToInt32(mesure);
            nuances = new long[nombresdemesure];
            for(int i=0; i < nombresdemesure; i++)
            {
                nuances[i] = 0; //initialisation des nuances à aucune nuance pour chaque mesure
            }
            

            base.OnCreate(savedInstanceState);
            createLayoutDynamically(nombresdemesure);//crée les buttons

            //recevoir les informations des parametrages precedents 
            nom = Intent.GetStringExtra("nom") ?? "nom not available";
            string numerateur = Intent.GetStringExtra("numerateur") ?? "numerateur not available";
            valnumerateur = Convert.ToInt32(numerateur);
            string denominateur = Intent.GetStringExtra("denominateur") ?? "denominateur not available";
            valdenominateur = Convert.ToInt32(denominateur);
            string tempo = Intent.GetStringExtra("valeurdutempo") ?? "valeurdutempo not available";
            tempoval = Convert.ToInt32(tempo);
            string mode = Intent.GetStringExtra("valeurdumode") ?? "valeurdumode not available";
            valnote = Convert.ToInt32(mode);

            
          
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
               num  = data.GetStringExtra("numero_mesure");
               nua = data.GetStringExtra("choix_nuance");
                nuances[Convert.ToInt32(num)-1] = Convert.ToInt64(nua);//pour chaque mesure (button) on sauvegarde ici la nuance selectionner 
                Toast.MakeText(ApplicationContext, nuances[Convert.ToInt32(num) - 1].ToString(), ToastLength.Long).Show();

            }
        }
        private void createLayoutDynamically(int n)
        {
            int tmp = n + 2;
            ScrollView sv = new ScrollView(this);
            LinearLayout layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;
            sv.AddView(layout);
            for (int i = 1; i <= tmp; i++)
            {
                Button myButton = new Button(this);
                myButton.Id = i;
                myButton.Click += delegate 
                {
                    if(myButton.Id < tmp - 1)
                    {
                        formulaire(i);
                    }
                    else
                    {
                        if(myButton.Id == tmp - 1)
                        {
                            Intent myintent = new Intent(this, typeof(Lancement));
                            myintent.PutExtra("nom",nom);
                            myintent.PutExtra("valeurnumerateur",valnumerateur.ToString());
                            myintent.PutExtra("valeurdenominateur",valdenominateur.ToString());
                            myintent.PutExtra("valeurmode",valnote.ToString());
                            myintent.PutExtra("tempoval",tempoval.ToString());
                            for(int j = 1; j <=tmp; j++)
                            {
                                myintent.PutExtra("mesure " + j, nuances[j - 1].ToString());
                            }
                            StartActivity(myintent);
                        }
                        else
                        {
                            Finish();
                        }
                    }
                    
                };

            if (i < tmp-1)
                {
                    myButton.Text = " Mesure " + myButton.Id;
                }
                else
                {
                    if (i == tmp - 1)
                    {
                        myButton.Text = "Enregistrer";
                        myButton.SetBackgroundColor(Color.Green);
                        myButton.SetTextColor(Color.Black);
                    }
                    else
                    {
                        myButton.Text = " Retour ";
                       // myButton.SetBackgroundColor(Color.Blue);
                       // myButton.SetTextColor(Color.Blue);
                    }
                    


                }

                layout.AddView(myButton);

            }
            this.SetContentView(sv);
          

        }
        private void formulaire(int numeroMesure)
        {
            Intent intent = new Intent(this, typeof(ParametrageNuancesALertes));
            intent.PutExtra("id_bouton", numeroMesure.ToString());
            StartActivityForResult(intent,0);
        }
    }
}