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
    class Partition
    {
        string nompartition;
        string[] typenotemesure= { "ronde", "blanche", "noir","croche","noirpointé","blanchepointé","crochepointé" };
        int nombremesure;
        int tempo;

        void ParametrageInitial(string partition,int mesure)
        {
            nompartition = partition;
            nombremesure = mesure;
        }
        void Parametragesecondaire(int letempo)
        {

        }
    }
}