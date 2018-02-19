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

namespace MaestroPad

   
{
    [Activity(Label = "ParametrageNuancesALertes")]
    public class ParametrageNuancesALertes : Activity
    {
      public static  long valeurdelanuance = -1 ;
     public  static  long valeurcouleurAlerte = 0 ;
     public static   long valeurdelaReprise = 0 ;
     public static   long numeroMesure = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NuancesAlerte);

            // Create your application here

            Button valider = FindViewById<Button>(Resource.Id.ValiderMesure);
            Button retour = FindViewById<Button>(Resource.Id.AnnulerMesure);

            //recuperation du numero de la mesure
            // final Intent intent = getIntent();

            String mesure = Intent.GetStringExtra("id_bouton") ?? "echec de recuperation du numero de la mesure";
            numeroMesure = Convert.ToInt64(mesure);

            // event des bouttons
            valider.Click += delegate
            {
             
                Intent myIntent = new Intent(this, typeof(ParametrageMesures));
                myIntent.PutExtra("numero_mesure", mesure);
                myIntent.PutExtra("choix_nuance", valeurdelanuance.ToString());
                SetResult(Result.Ok, myIntent);
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

            //Liste deroulante des reprises
            var Reprises = FindViewById<Spinner>(Resource.Id.ListeReprises);
            Reprises.Prompt = "Selectionner la reprises";
            Reprises.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Reprises_ItemSelected);
            var adapter3 = ArrayAdapter.CreateFromResource(this, Resource.Array.ListeReprise, Android.Resource.Layout.SimpleSpinnerItem);
            adapter3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            Reprises.Adapter = adapter3;
        }
        //event reprises
        private void Reprises_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var repriseselected = sender as Spinner;
            valeurdelaReprise = repriseselected.GetItemIdAtPosition(e.Position);
           // Toast.MakeText(ApplicationContext, valeurdelanuance.ToString(), ToastLength.Long).Show();
        }

        //event de Alerte
        private void Alerte_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var Alerteselected = sender as Spinner;
         valeurcouleurAlerte = Alerteselected.GetItemIdAtPosition(e.Position);
           // Toast.MakeText(ApplicationContext, valeurdelanuance.ToString(), ToastLength.Long).Show();

        }
        //event de Nuance
        private void Nuance_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var nuanceselected = sender as Spinner;
           valeurdelanuance=(int) nuanceselected.GetItemIdAtPosition(e.Position);
           // Toast.MakeText(ApplicationContext, valeurdelanuance.ToString(), ToastLength.Long).Show();
        }
    }
}