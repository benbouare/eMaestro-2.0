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
    class  Mapartition 
    {
         string nompartion = null;
         int nombredemesure = 0;
         int valeurnumerateur = 0;
         int valeurdenominateur = 0;
         int valeurdutempo = 0;

        public  void setNompartition(string nom)
        {
            nompartion = nom;
        }
        public string getNompartion()
        {
            return nompartion;
        }

        public  void setNombredemesure(int mesure)
        {
            nombredemesure = mesure;
        }
        public int getNombredemesure()
        {
            return nombredemesure;
        }

        public  void SetValeurNumerateur(int numerateur)
        {
            valeurnumerateur = numerateur;
        }
        public int GetValeurNumerateur()
        {
            return valeurnumerateur;
        }
        public void setValeurDenominateur(int denominateur)
        {
            valeurdenominateur = denominateur;
        }
        public int GetvaleurDenominateur()
        {
            return valeurdenominateur;
        }
        public  void SetTempo(int tempo)
        {
            valeurdutempo = tempo;
        }
        public int GEtTempo()
        {
            return valeurdutempo;
        }
    }


}