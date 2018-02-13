
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
        int nbrMesure = 0;
        int valnum = 0;
        int valdenom = 0;
        string nom = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            Partition partition = new Partition();
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
                string mesure = nombreMesures.Text.ToString();

                //convertir le nombre de mesure en int
                if(mesure != string.Empty && mesure != null)
                {
                    nbrMesure = Convert.ToInt32(mesure);
                }
                if (numerateur.Text.ToString() != string.Empty && numerateur.Text.ToString() != null)
                {
                    valnum = Convert.ToInt32(numerateur.Text.ToString());
                }
                if(denominateur.Text.ToString() != string.Empty && denominateur.Text.ToString() != null)
                {
                    valdenom = Convert.ToInt32(denominateur.Text.ToString());
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
                        var intent = new Intent(this, typeof(Parametragesecondaire));
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