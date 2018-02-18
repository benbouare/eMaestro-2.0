using Android.App;
using Android.OS;
using Android.Widget;
using Android.Systems;
using Android.Content;
using Android.Views;
using System;
using Android.Graphics;

namespace BouttonEmaestroS
{
  [Activity(Label = "Sasi dees différentes mesures", MainLauncher = true)]
  public class MainActivity : Activity
  {
    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      createLayoutDynamically(100);
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
          myButton.Text = " "+ myButton.Id;
        }
        else
        {
          myButton.Text = "Envoyer" ;
          myButton.SetBackgroundColor(Color.Green);
          myButton.SetTextColor(Color.Black);


        }
      
        layout.AddView(myButton);

    }
            this.SetContentView(sv);

    }
  }

  }



