using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Android.Media.Midi;
using System.Threading;
using System.Reflection.Emit;
using static Java.Interop.JniEnvironment;
using Java.Lang.Ref;
using Java.Util.Prefs;


namespace MaestroPad

   
{
    [Activity(Label = "ParametrageNuancesALertes")]
    public class ParametrageNuancesALertes : Activity
    {
     public static  int valeurdelanuance = -1 ;
     public  static  int valeurcouleurAlerte = 0 ;
     public static   int valeurdelaReprise = 0 ;
     public static   int numeroMesure = 0;
     public static int tempoval = 1;
     public static int indicateur = 1;
     public static int VELOCITY = 0;
     public static int valnote = 25;//donne la valeur du mode choisi par defaut rien
     public static int valnumerateur = 0;
     public static int valdenominateur = 0;
     public static int nombresdemesure = 0;
     string nom = null;
     static Intent myIntent ;
     static String mesure;
        //information du tabeau de mesures
     


        //pour les buttons radios
        [Export("Radiogroup_onclick")]

       /* public void Radiogroup_onclick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.aucunModeNuanace:
                    Mesures[numeroMesure-1, ModeNuance] = 0;
                    break;
                case Resource.Id.AscendantModeNuance:
                    Mesures[numeroMesure-1, ModeNuance] = 1;
                    break;
                case Resource.Id.DescendantModeNuance:
                    Mesures[numeroMesure-1, ModeNuance] = 2;
                    break;
                case Resource.Id.RepriseNon:
                    Mesures[numeroMesure-1, BoolReprise] = 0;
                    break;
                case Resource.Id.RepriseOui:
                    Mesures[numeroMesure-1, BoolReprise] = 1;
                    break;
                    
            }
        }
        */

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NuancesAlerte);



            // Create your application here
            
            //recuperation des valeurs de bemols dieses numero de fin de reprise
            EditText Numerodefinreprise = FindViewById<EditText>(Resource.Id.NumeroFinReprise);
            EditText nombrediese = FindViewById<EditText>(Resource.Id.NombreDiese);
            EditText nombrebemol = FindViewById<EditText>(Resource.Id.NombreBemol);

           
            

            Button valider = FindViewById<Button>(Resource.Id.ValiderMesure);
            Button retour = FindViewById<Button>(Resource.Id.AnnulerMesure);

            //recuperation du numero de la mesure
            mesure = Intent.GetStringExtra("id_bouton") ?? "echec de recuperation du numero de la mesure";
            numeroMesure = Convert.ToInt32(mesure);

            // event des bouttons
            valider.Click += delegate
            {
                int finreprise = 0;
                int nbrdiese = 0;
                int nbrbemol = 0;

                string tmp = Numerodefinreprise.Text;
                string tmp2 = nombrediese.Text;
                string tmp3 = nombrebemol.Text;

                if(tmp != null && tmp != string.Empty)
                {
                     finreprise = Convert.ToInt32(tmp.ToString());
                }
                else
                {
                    control();
                }
                if (tmp2 != null && tmp2 != string.Empty)
                {
                    nbrdiese = Convert.ToInt32(tmp2.ToString());
                }
                else
                {
                    control();
                }
                if (tmp3 != null && tmp3 != string.Empty)
                {
                    nbrbemol = Convert.ToInt32(tmp3.ToString());
                }
                else
                {
                    control();
                }


                

                int tmpo = numeroMesure - 1;
                myIntent = new Intent(this, typeof(ParametrageMesures));
                myIntent.PutExtra("numero_mesure", mesure);
                myIntent.PutExtra("choix_nuance", valeurdelanuance.ToString());
                myIntent.PutExtra("choix_alerte", valeurcouleurAlerte.ToString());
               // myIntent.PutExtra("mode_nuance", Mesures[tmpo, ModeNuance]);
               // myIntent.PutExtra("Bool_reprise", Mesures[tmpo, BoolReprise]);
                myIntent.PutExtra("finreprise", finreprise.ToString());
                myIntent.PutExtra("nbr_diese", nbrdiese.ToString());
                myIntent.PutExtra("nbr_bemol", nbrbemol.ToString());
                this.SetResult(Result.Ok, myIntent);
                Finish();

            };

            retour.Click += delegate
             {
                 Finish();
             };
            //Liste deroulante pour les nuances
            var LaNuanace = FindViewById<Spinner>(Resource.Id.ListeNuance);
            LaNuanace.Prompt = "Selectionner la nuance";
            LaNuanace.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Nuance_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.ListeNuances, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            LaNuanace.Adapter = adapter;

            //liste deroulante pour les Alertes
            var Alerte = FindViewById<Spinner>(Resource.Id.ChoixAlerte);
            Alerte.Prompt = "Selectionner la alertes";
            Alerte.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Alerte_ItemSelected);
            var adapter2 = ArrayAdapter.CreateFromResource(this, Resource.Array.ListeCouleur, Android.Resource.Layout.SimpleSpinnerItem);
            adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            Alerte.Adapter = adapter2;

        }

        //event de Alerte
        private void Alerte_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var Alerteselected = (Spinner)sender; 
            valeurcouleurAlerte = Convert.ToInt32(Alerteselected.SelectedItemId.ToString());
            Toast.MakeText(ApplicationContext, valeurcouleurAlerte.ToString(), ToastLength.Long).Show();
  

        }
        //event de Nuance
        private void Nuance_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var nuanceselected = (Spinner)sender ;
            valeurdelanuance = Convert.ToInt32(nuanceselected.SelectedItemId.ToString());
            Toast.MakeText(ApplicationContext, valeurdelanuance.ToString(), ToastLength.Long).Show();
           // Toast.MakeText(ApplicationContext, numeroMesure.ToString(), ToastLength.Long).Show();
            

        }
        private void control()
        {

            string message = string.Empty;


           
                message = "veuillez renseinger le ou les champs avant de continuer";

            Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();

        }
    }
}