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
using Android.Graphics;

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
     public static int indic = 0;
     public static int VELOCITY = 0;
     public static int valnote = 25;//donne la valeur du mode choisi par defaut rien
     public static int valnumerateur = 0;
     public static int valdenominateur = 0;
     public static int nombresdemesure = 0;
     public static int booleandereprise = 0;
     public static int choixdumodenuance = 0;
     string nom = null;
     static Intent myIntent ;
     static String mesure;

     public static int finreprise = 0;
     public static int nbrdiese = 0;
     public static int nbrbemol = 0;
     

     public static string tmp = null;
     public static string tmp2 = null;
     public static string tmp3 = null;
     public static string tmp4 = null;

        EditText Numerodefinreprise;
        EditText nombrediese;
        EditText nombrebemol;





        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NuancesAlerte);



            // Create your application here

            //buttons radios
            RadioButton confirmationreprise = FindViewById<RadioButton>(Resource.Id.RepriseOui);
            RadioButton nonconfirmationreprise = FindViewById<RadioButton>(Resource.Id.RepriseNon);

            nonconfirmationreprise.Click += on_clickSwitchboolreprisenon;
            confirmationreprise.Click += on_clickSwitchboolrepriseoui;

            //pour la recuperation des valeurs de bemols dieses numero de fin de reprise
             Numerodefinreprise = FindViewById<EditText>(Resource.Id.NumeroFinReprise);
            Numerodefinreprise.Text = finreprise.ToString();
             nombrediese = FindViewById<EditText>(Resource.Id.NombreDiese);
            nombrediese.Text = nbrdiese.ToString();
             nombrebemol = FindViewById<EditText>(Resource.Id.NombreBemol);
            nombrebemol.Text = nbrbemol.ToString();


            


            Button valider = FindViewById<Button>(Resource.Id.ValiderMesure);
            Button retour = FindViewById<Button>(Resource.Id.AnnulerMesure);

            //recuperation des informations de la mesure
            mesure = Intent.GetStringExtra("id_bouton") ?? "echec de recuperation du numero de la mesure";
            numeroMesure = Convert.ToInt32(mesure);
            valeurdelanuance = Convert.ToInt32(Intent.GetStringExtra("choix_nuance"));
            valeurcouleurAlerte = Convert.ToInt32(Intent.GetStringExtra("choix_alerte"));
             booleandereprise = Convert.ToInt32(Intent.GetStringExtra("Bool_reprise"));
             finreprise = Convert.ToInt32(Intent.GetStringExtra("finreprise"));
            nbrdiese = Convert.ToInt32(Intent.GetStringExtra("nbr_diese"));
            nbrbemol = Convert.ToInt32(Intent.GetStringExtra("nbr_bemol"));

            


            // event des bouttons
            valider.Click += delegate
            {
               

                if(booleandereprise == 1 )
                {
                   
                    tmp2 = nombrediese.Text.ToString();
                    
                    if (tmp2 != null && tmp2 != string.Empty)
                    {
                        nbrdiese = Convert.ToInt32(tmp2);
                        nombrediese.Text = nbrdiese.ToString();
                    }
                    else
                    {
                        string message = "0 Diese ";
                        //Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
                    }
                    tmp3 = nombrebemol.Text.ToString();
                    if (tmp3 != null && tmp3 != string.Empty)
                    {
                        nbrbemol = Convert.ToInt32(tmp3);
                        nombrebemol.Text = tmp3.ToString();
                    }
                    else
                    {
                        string message = "0 Bemol ";
                        //Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
                    }
                    tmp = Numerodefinreprise.Text.ToString();
                    if (tmp != null && tmp != string.Empty)
                    {
                        Numerodefinreprise.Text = tmp.ToString();
                        finreprise = Convert.ToInt32(tmp);
                        int tmpo = numeroMesure - 1;
                        myIntent = new Intent(this, typeof(ParametrageMesures));
                        myIntent.PutExtra("numero_mesure", mesure);
                        myIntent.PutExtra("choix_nuance", valeurdelanuance.ToString());
                        myIntent.PutExtra("choix_alerte", valeurcouleurAlerte.ToString());
                        // myIntent.PutExtra("mode_nuance", Mesures[tmpo, ModeNuance]);
                        myIntent.PutExtra("Bool_reprise", booleandereprise.ToString());
                        myIntent.PutExtra("finreprise", finreprise.ToString());
                        myIntent.PutExtra("nbr_diese", nbrdiese.ToString());
                        myIntent.PutExtra("nbr_bemol", nbrbemol.ToString());
                        this.SetResult(Result.Ok, myIntent);
                        Finish();
                    }
                    else
                    {
                        string message = "vous n'avez pas renseigné la reprise de fin";
                        //Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
                    }
                    
                }
                else
                {
                    tmp2 = nombrediese.Text.ToString();
                    if (tmp2 != null && tmp2 != string.Empty)
                    {
                        nbrdiese = Convert.ToInt32(tmp2);
                        nombrediese.Text = tmp2.ToString();
                       
                    }
                    else
                    {
                        string message = " 0 Diese ";
                        //Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
                    }
                    tmp3 = nombrebemol.Text.ToString();
                    if (tmp3 != null && tmp3 != string.Empty)
                    {
                        nbrbemol = Convert.ToInt32(tmp3);
                        nombrebemol.Text = tmp3.ToString();
                    }
                    else
                    {
                        string message = "0 bemol ";
                        //Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
                    }


                        int tmpo = numeroMesure - 1;
                        myIntent = new Intent(this, typeof(ParametrageMesures));
                        myIntent.PutExtra("numero_mesure", mesure);
                        myIntent.PutExtra("choix_nuance", valeurdelanuance.ToString());
                        myIntent.PutExtra("choix_alerte", valeurcouleurAlerte.ToString());
                        // myIntent.PutExtra("mode_nuance", Mesures[tmpo, ModeNuance]);
                        myIntent.PutExtra("Bool_reprise", booleandereprise.ToString());
                        myIntent.PutExtra("finreprise", finreprise.ToString());
                        myIntent.PutExtra("nbr_diese", nbrdiese.ToString());
                        myIntent.PutExtra("nbr_bemol", nbrbemol.ToString());
                        this.SetResult(Result.Ok, myIntent);
                        Finish();
                }
               

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

        // resume 
        protected override void OnRestoreInstanceState(Bundle bundle)
        {
            base.OnRestoreInstanceState(bundle);

             Numerodefinreprise.Text = bundle.GetString("finreprise");
            valeurdelanuance = Convert.ToInt32(Numerodefinreprise.Text.ToString());
             nombrediese.Text =bundle.GetString("nbr_diese");
            nbrdiese = Convert.ToInt32(nombrediese.Text.ToString());
             nombrebemol.Text=bundle.GetString("nbr_bemol");
            nbrbemol = Convert.ToInt32(nombrebemol.Text.ToString());
            booleandereprise = Convert.ToInt32(bundle.GetString("Bool_reprise"));
            valeurcouleurAlerte = Convert.ToInt32(bundle.GetString("choix_alerte"));


        }

        //save all value
        protected override void OnSaveInstanceState(Bundle bundle)
        {
            base.OnSaveInstanceState(bundle);

            bundle.PutString("nom", nom); ;
            bundle.PutString("nombresdemesure", mesure);
            bundle.PutString("choix_nuance", valeurdelanuance.ToString());
            bundle.PutString("Bool_reprise", booleandereprise.ToString());
            bundle.PutString("finreprise", finreprise.ToString());
            bundle.PutString("nbr_diese", nbrdiese.ToString());
            bundle.PutString("nbr_bemol", nbrbemol.ToString());
            bundle.PutString("choix_alerte", valeurcouleurAlerte.ToString());

        }





        //pour les radio button des reprises 
        private void on_clickSwitchboolreprisenon(object sender, EventArgs e)
        {
            RadioButton monbouton = (RadioButton)sender;
            booleandereprise = 0;
            //Toast.MakeText(ApplicationContext, "non est selectionné " + booleandereprise.ToString(), ToastLength.Long).Show();
        }
        private void on_clickSwitchboolrepriseoui(object sender, EventArgs e)
        {
            RadioButton monbouton = (RadioButton)sender;
            booleandereprise = 1;
            //Toast.MakeText(ApplicationContext,"oui est selectionné " +booleandereprise.ToString(), ToastLength.Long).Show();
        }


        //event de Alerte
        private void Alerte_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var Alerteselected = (Spinner)sender;
            valeurcouleurAlerte = Convert.ToInt32(Alerteselected.SelectedItemId.ToString());
            switch (valeurcouleurAlerte)
            {
                case 1:
                    Alerteselected.SetBackgroundColor(Color.Crimson);
                    break;
                case 2:
                    Alerteselected.SetBackgroundColor(Color.DarkOliveGreen);
                    break;
                case 3:
                    Alerteselected.SetBackgroundColor(Color.DeepPink);
                    break;
                case 4:
                    Alerteselected.SetBackgroundColor(Color.LemonChiffon);
                    break;
                case 5:
                    Alerteselected.SetBackgroundColor(Color.Navy);
                    break;
                case 6:

                    Alerteselected.SetBackgroundColor(Color.Firebrick);
                    break;
                case 0:
                    Alerteselected.SetBackgroundColor(Color.Gray);
                    break;
            }
            //Toast.MakeText(ApplicationContext, valeurcouleurAlerte.ToString(), ToastLength.Long).Show();


        }
        //event de Nuance
        private void Nuance_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var nuanceselected = (Spinner)sender ;
            valeurdelanuance = Convert.ToInt32(nuanceselected.SelectedItemId.ToString());
            //Toast.MakeText(ApplicationContext, valeurdelanuance.ToString(), ToastLength.Long).Show();
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