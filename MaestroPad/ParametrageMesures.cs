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
        public static int ResultCode = 999;
        public static int tempoval = 1;
        public static int indicateur = 1;
        public static int VELOCITY = 0;
        public static int valnote = 1;//pour determiner le mode 
        public static int valnumerateur = 0;
        public static int valdenominateur = 0;
        public static int nombresdemesure ;
        string nom = null;
        static string num = null;
        static string nua = null;
        static string aler = null;
        static string modnu = null;
        static string BoolRep = null;
        static string numfinrep = null;
        static string nbrdies = null;
        static string nbrbem = null;

        // ArrayList tabdeboutons = new ArrayList();
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

        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //reception du nombre de mesures 
            string mesure = Intent.GetStringExtra("nombresdemesure") ?? "nombresdemesure not available";
            nombresdemesure = Convert.ToInt32(mesure);
            Mesures = new int[nombresdemesure,colonne];
            for(int i=0; i < nombresdemesure; i++)
            {
                Mesures[i,nuance] = 0; //initialisation des nuances à aucune nuance pour chaque mesure
                Mesures[i, alerte] = 0; //initialiser des alertes à aucune alerte pour chaque mesure
                Mesures[i, ModeNuance] = 0; //definit si la nuance doit etre ascendante descendante ou auncune 
                Mesures[i, BoolReprise] = 0; //definir s'il y a reprise ou non 
                Mesures[i, NumerofinReprise] = 0;// numer de la mesure indiquand la fin de la reprise
                Mesures[i, NombreDieses] = 0; //nombre de diese dans la mesure
                Mesures[i, nombreBemols] = 0; //nombre de bemols dans la mesure
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
           if(requestCode == 0)
            {
                //Toast.MakeText(ApplicationContext, "Nous sommes dans le onactivityresult", ToastLength.Long).Show();
                if (resultCode == Result.Ok)
                {

                    modnu = data.GetStringExtra("mode_nuance");
                    BoolRep = data.GetStringExtra("Bool_reprise");
                    numfinrep = data.GetStringExtra("finreprise");
                    nbrdies = data.GetStringExtra("nbr_diese");
                    nbrbem = data.GetStringExtra("nbr_bemol");
                    num = data.GetStringExtra("numero_mesure");
                    nua = data.GetStringExtra("choix_nuance");
                    aler = data.GetStringExtra("choix_alerte");
                    int inter = (Convert.ToInt32(num) - 1);//numero de la mesure -1 car le tableau commence à 0
                    Mesures[inter, nuance] = Convert.ToInt32(nua);//pour chaque mesure (button) on sauvegarde ici la nuance selectionnée 
                    Mesures[inter, alerte] = Convert.ToInt32(aler);//pour chaque mesure (button) on sauvegarde ici l'alerte  selectionnée
                    Mesures[inter, ModeNuance] = Convert.ToInt32(modnu);
                    Mesures[inter, BoolReprise] = Convert.ToInt32(BoolRep);
                    Mesures[inter, NumerofinReprise] = Convert.ToInt32(numfinrep);
                    Mesures[inter, NombreDieses] = Convert.ToInt32(nbrdies);
                    Mesures[inter, nombreBemols] = Convert.ToInt32(nbrbem);

                    //Toast.MakeText(ApplicationContext, nua, ToastLength.Long).Show();
                    //Toast.MakeText(ApplicationContext, Mesures[(Convert.ToInt32(num) - 1),nuance].ToString(), ToastLength.Long).Show();
                    Toast.MakeText(ApplicationContext,"La valeur de bool de la mesure "+ num + " " + BoolRep, ToastLength.Long).Show();
                    Toast.MakeText(ApplicationContext, "La mesure de fin " + numfinrep.ToString(), ToastLength.Long).Show();
                }
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
                        formulaire(myButton.Id);
                    }
                    else
                    {
                        if(myButton.Id == tmp - 1)
                        {
                            Intent myintent = new Intent(this, typeof(Lancement));
                            myintent.PutExtra("nom",nom);
                            myintent.PutExtra("nombresdemesure", nombresdemesure.ToString());
                            myintent.PutExtra("valeurnumerateur",valnumerateur.ToString());
                            myintent.PutExtra("valeurdenominateur",valdenominateur.ToString());
                            myintent.PutExtra("valeurmode",valnote.ToString());
                            myintent.PutExtra("tempoval",tempoval.ToString());
                            for(int j = 1; j <=tmp-2; j++)
                            {
                                myintent.PutExtra("mesure " + j + "nuance", Mesures[(j - 1),nuance].ToString());
                                myintent.PutExtra("mesure " + j + "alerte", Mesures[(j - 1), alerte].ToString());
                                myintent.PutExtra("mesure " + j + "ModeNuance", Mesures[(j - 1), ModeNuance].ToString());
                                myintent.PutExtra("mesure " + j + "BoolReprise", Mesures[(j - 1), BoolReprise].ToString());
                                myintent.PutExtra("mesure " + j + "NumerofinReprise", Mesures[(j - 1), NumerofinReprise].ToString());
                                myintent.PutExtra("mesure " + j + "NombreDieses", Mesures[(j - 1), NombreDieses].ToString());
                                myintent.PutExtra("mesure " + j + "nombreBemols", Mesures[(j - 1), nombreBemols].ToString());
                            }
                            StartActivity(myintent);
                            //Toast.MakeText(ApplicationContext, Mesures[0,nuance].ToString(), ToastLength.Long).Show();
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
            //Toast.MakeText(ApplicationContext, numeroMesure.ToString(), ToastLength.Long).Show();
            StartActivityForResult(intent,0);
        }
    }
}