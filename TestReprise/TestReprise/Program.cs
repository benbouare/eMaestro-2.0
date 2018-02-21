using System;
using System.Threading;

namespace TestReprise
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] mesure = new int[21]; //un tableau de mesure
            int[] reprise = new int[21]; //un tableau de reprise(oui/non)
            int[] finRep = new int[21]; //un tableau de valeur(fin reprise)
            int[] dejaPasse = new int[21]; // test si on est déja passé dans la reprise
            int nombre; //nombre de mesures
            int choix; //mettre des reprises ou non

            Console.WriteLine("Nombre de mesures : ");
            nombre = Convert.ToInt16(Console.ReadLine());

            for (int i = 1; i <= nombre; i++) {
                mesure[i] = i;
                reprise[i] = 0;
                finRep[i] = 0;
            }

            Console.WriteLine("Une reprise ? : (0/1) : ");
            choix = Convert.ToInt16(Console.ReadLine());

            while (choix == 1)
            {
                Console.WriteLine("Mesure de début : ");
                int debut = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("Mesure de fin : ");
                int fin = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("Nombre de passages : ");
                int pass = Convert.ToInt16(Console.ReadLine());

                reprise[debut] = 1;
                dejaPasse[debut] = pass;
                finRep[debut] = fin;

                Console.WriteLine("Une autre reprise ? : (0/1) : ");
                choix = Convert.ToInt16(Console.ReadLine());
            }

            int j = 1;
            int k = 0;
            while (j <= nombre)
            {
                if (reprise[j] == 1)
                {
                     k = mesure[j];
                }
                if ((j > finRep[k]) && (dejaPasse[k] > 0))
                {
                    j = mesure[k];
                    dejaPasse[k]--;
                }
                Console.WriteLine("Mesure " + mesure[j]);
                j++;
                Thread.Sleep(1000);
            }

            /*int mesureDebutRep = 0;
            int mesureFinRep = 0;
            int nbRep = 0;

            Console.WriteLine("Voulez-vous une reprise ? : (0/1) : ");
            int choix = Convert.ToInt16(Console.ReadLine());
            if (choix == 1)
            {
                Console.WriteLine("Mesure de début : ");
                mesureDebutRep = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("Mesure de Fin : ");
                mesureFinRep = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("Combien de fois on effectue la reprise ? : ");
                nbRep = Convert.ToInt16(Console.ReadLine());
            }
            int i = 1;
            int dejaPasse = 0;
            while (i <= 20)
            {
                if (mesureFinRep !=0 && i > mesureFinRep && dejaPasse < nbRep)
                {
                    i = mesureDebutRep;
                    dejaPasse++;
                }
                Console.WriteLine("Mesure " + i);
                i++;
                Thread.Sleep(1000);
            }*/
            Console.Read();
        }
    }
}
