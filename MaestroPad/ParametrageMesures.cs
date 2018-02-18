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
        ArrayList tabdeboutons = new ArrayList();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //reception du nombre de mesures 
            string mesure = Intent.GetStringExtra("nombresdemesure") ?? "nombresdemesure not available";
            nombresdemesure = Convert.ToInt32(mesure);

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
        private void createLayoutDynamically(int n)
        {
            int tmp = n + 1;
            ScrollView sv = new ScrollView(this);
            LinearLayout layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;
            sv.AddView(layout);
            for (int i = 1; i <= tmp; i++)
            {
                Button myButton = new Button(this);
                myButton.Id = i;

                if (i < tmp)
                {
                    myButton.Text = " " + myButton.Id;
                }
                else
                {
                    myButton.Text = "Enregistrer";
                    myButton.SetBackgroundColor(Color.Green);
                    myButton.SetTextColor(Color.Black);


                }

                layout.AddView(myButton);

            }
            this.SetContentView(sv);

        }
    }
}