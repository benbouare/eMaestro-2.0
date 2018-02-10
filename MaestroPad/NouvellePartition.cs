
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace MaestroPad
{
    [Activity(Label = "Nouvelle Partition")]
    public class NouvellePartition : Activity
    {
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
            string nom = nompartition.Text;
            string mesure = nombreMesures.Text;
            retour.Click += (sender, e) =>
            {
                
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
            creer.Click += (sender, e) =>
            {
                // pour recuperer le nom de la partition 


                //aller au parametrage suivant
                  if (nom == string.Empty && nom == null)
                  {
                      control();
                  }
                /*else
                    if(mesure ==string.Empty && mesure ==null ){
                                  control();
                }*/
                else
                {

                     var intent = new Intent(this, typeof(Parametragesecondaire));
                     StartActivity(intent);

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