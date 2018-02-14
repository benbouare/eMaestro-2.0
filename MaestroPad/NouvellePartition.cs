
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace MaestroPad
{
    [Activity(Label = "Nouvelle Partition")]
    public class NouvellePartition : Activity
    {
       public static int nbrMesure = 0;
        public static int valnum = 0;
       public static int valdenom = 0;
        public static string nom = null;
        static Mapartition partition=  new Mapartition();



        protected override void OnCreate(Bundle savedInstanceState)
        {

           
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Parametrage);
            Button creer = FindViewById<Button>(Resource.Id.createpartition);
            Button retour = FindViewById<Button>(Resource.Id.Home);
            EditText nompartition = FindViewById<EditText>(Resource.Id.namePartition);
            EditText nombreMesures = FindViewById<EditText>(Resource.Id.nbrmesures);
            EditText numerateur = FindViewById<EditText>(Resource.Id.valnumerateur);
            EditText denominateur = FindViewById<EditText>(Resource.Id.valdenominateur);

            retour.Click += (sender, e) =>
            {
                
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
            creer.Click += (sender, e) =>
            {
                int toutestok = 0;
                // pour recuperer le nom de la partition 
                nom = nompartition.Text.ToString();
                partition.setNompartition(nom);

                string mesure = nombreMesures.Text.ToString();


                //convertir le nombre de mesure en int
                if(mesure != string.Empty && mesure != null)
                {
                    nbrMesure = Convert.ToInt32(mesure);
                    partition.setNombredemesure(nbrMesure);
                }
                if (numerateur.Text.ToString() != string.Empty && numerateur.Text.ToString() != null)
                {
                    valnum = Convert.ToInt32(numerateur.Text.ToString());
                    partition.SetValeurNumerateur(valnum);
                }
                if(denominateur.Text.ToString() != string.Empty && denominateur.Text.ToString() != null)
                {
                    valdenom = Convert.ToInt32(denominateur.Text.ToString());
                    partition.setValeurDenominateur(valdenom);
                }
                
                
                //aller au parametrage suivant
                if (nom == string.Empty || nom == null)
                  {
                      control();
                }
                else
                {
                    toutestok++;
                }
                if (nbrMesure == 0 || mesure ==string.Empty )
                {
                    control();
                }else

                {
                    toutestok++;
                }
                if(valnum ==0 || numerateur.Text.ToString() == string.Empty)
                {
                    control();
                }
                else
                {
                    toutestok++;
                }
                if(valdenom == 0 || denominateur.Text.ToString()==string.Empty)
                {
                    control();
                }
                else
                {
                    toutestok++;
                }

                if(toutestok==4)
                {
                    if (valnum > 24 || valdenom > 24)
                    {
                        Toast.MakeText(ApplicationContext, "Veuillez renseigner un nombre inferieur ou egal 24", ToastLength.Long).Show();
                    }
                    else
                    {
                       
                        Intent intent = new Intent(this, typeof(Parametragesecondaire));
                        intent.PutExtra("nom", nompartition.Text.ToString());
                        intent.PutExtra("nombresdemesure", nombreMesures.Text.ToString());
                        intent.PutExtra("numerateur", numerateur.Text.ToString());
                        intent.PutExtra("denominateur", denominateur.Text.ToString());
                        //intent.PutExtra("partition", partition);
                       StartActivity(intent);
                   }


                 }



           };

       }
       private void control()
       {
           //
           string message = string.Empty;

           //
          // if (tempoval != 0)
            //   message = "Etape suivante";
          // else
               message = "veuillez renseinger le ou les champs avant de continuer";

           //
           Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();

       }
   }
}
 