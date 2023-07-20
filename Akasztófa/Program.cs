using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Akasztófa
{
    class Program
    {
        public static bool win = false;
        public static int points = 10;

        // A főmenü megjelenítése
        public static void DisplayMenu()
        {
            Console.Clear();
            DrawInfoBox(0);
            Console.WriteLine("Játék megkezdése - 1");
            Console.WriteLine("Szavak bővítése - 2");
            Console.WriteLine("Kilépés - 3");
            Console.Write("\nVálasztott menüpont száma: ");
            int temp = Convert.ToInt32(Console.ReadLine());
            switch (temp)
            {
                case 1:
                    PlayGame();
                    break;
                case 2:
                    AddWord();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        // Az infoBox kirajzolása
        public static void DrawInfoBox(int diff)
        {
            DrawLine();
            Console.WriteLine();
            Console.Write("+ Akasztófa");
            DrawBackLine(1);
            Console.WriteLine("+ by Farkas Máté Benjámin & His Imre Sándor");
            DrawBackLine(2);
            Console.WriteLine("+");
            DrawBackLine(3);
            Console.Write("+ Nehézség: ");
            // A megadott nehézség megjelenítése a nehézség száma alapján (alapértelmezett: 0, megjelenítése: - )
            switch (diff)
            {
                case 1:
                    Console.Write("Könnyű");
                    break;
                case 2:
                    Console.Write("Közepes");
                    break;
                case 3:
                    Console.Write("Nehéz");
                    break;
                default:
                    Console.Write('-');
                    break;
            }
            DrawBackLine(4);
            Console.WriteLine("+ Pontok: " + points + "/10");
            DrawBackLine(5);
            DrawLine();
            Console.SetCursorPosition(0, 8);
        }

        // Az infoBox feletti/alatti vonal kirajzolása
        public static void DrawLine()
        {
            for (int i = 0; i < 25; i++)
            {
                Console.Write("+ ");
            }
        }

        // Az infoBox jobb oldalán levő vonal kirajzolása
        public static void DrawBackLine(int l)
        {
            Console.SetCursorPosition(48, l);
            Console.Write("+");
            Console.WriteLine();
        }

        // A játék nehézségének kiválasztása, és a játék futtatása a végéig
        public static void PlayGame()
        {
            int difficulty = 0;
            List<char> letters = new List<char>();
            do
            {
                Console.Clear();
                DrawInfoBox(difficulty);
                Console.WriteLine("Könnyű - 1");
                Console.WriteLine("Közepes - 2");
                Console.WriteLine("Nehéz - 3");
                Console.WriteLine();
                Console.Write("Adja meg a kívánt nehézséget: ");
                difficulty = Convert.ToInt32(Console.ReadLine());
            } while (difficulty < 1 || difficulty > 3);
            string word = GetWord(difficulty);
            points = 10;
            win = false;
            while (points > 0 && win == false)
            {
                DisplayGame(difficulty, word, letters);
            }

            if (points <= 0)
            {
                DisplayLose(word, difficulty);
            }
        }

        // A megadott nehézség alapján egy szó véletlenszerű kiválasztása
        public static string GetWord(int diff)
        {
            string temp;
            List<string> tempList = new List<string>();
            Random random = new Random();
            FileStream fs = new FileStream("akasztofa.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            // Azon szavak hozzáadása a listához, amelyek a megadott nehézségi szinten vannak
            while (!sr.EndOfStream)
            {
                string sor = sr.ReadLine();
                if (Convert.ToInt32(sor.Split(';')[1]) == diff)
                {
                    tempList.Add(sor.Split(';')[0]);
                }
            }

            sr.Close();
            fs.Close();

            // A lista szavai közül egy darab véletlenszerű kiválasztása
            temp = tempList[random.Next(0, tempList.Count)];
            return temp;
        }
        
        // A "játekmező" megjelenítése
        public static void DisplayGame(int diff, string wrd, List<char> lttrs)
        {
            char temp;
            bool van;
            int db = 0;
            // A kijelző törlése minden megjelenítés előtt
            Console.Clear();
            DrawInfoBox(diff);
            // Betűk megjelenítése, ha a felhasználó kitalálta azokat, egyébként _ jelenik meg a még ki nem talált karakterek helyén
            for (int i = 0; i < wrd.Length; i++)
            {
                van = false;
                for (int j = 0; j < lttrs.Count; j++)
                {
                    if (wrd[i] == lttrs[j])
                    {
                        Console.Write(wrd[i] + " ");
                        van = true;
                        db++;
                    }
                }
                if (!van)
                {
                    Console.Write("_ ");
                }
            }
            Console.WriteLine();

            DrawDoll();

            // Amikor a felhasználó kitalálja az összes karaktert, a játéknak vége, egyébként új karakter kerül bekérésre
            if (db == wrd.Length)
            {
                Console.WriteLine("Ön nyert!");
                win = true;
                Console.ReadKey();
            }
            else
            {
                Console.Write("Felhasznált betűk: ");
                for (int i = 0; i < lttrs.Count; i++)
                {
                    Console.Write(lttrs[i] + " ");
                }
                Console.WriteLine();
                Console.Write("Új betű: ");
                temp = Convert.ToChar(Console.ReadLine());
                if (!lttrs.Contains(temp))
                {
                    lttrs.Add(temp);

                    int j = 0;
                    while (j < wrd.Length && wrd[j] != temp)
                    {
                        j++;
                    }
                    if (j >= wrd.Length)
                    {
                        points--;
                    }
                }
            }
        }

        // Bábu rajzolása a pontoktól függően
        public static void DrawDoll()
        {
            if (points <= 9) {Console.WriteLine("  O  ");}
            else {Console.WriteLine();}

            if (points <= 8) { Console.WriteLine("  |  "); }
            else { Console.WriteLine(); }

            if (points == 7) { Console.WriteLine("  |  "); }
            else if (points == 6) { Console.WriteLine(" /|  "); }
            else if (points <= 5) { Console.WriteLine(" /|\\ "); }
            else { Console.WriteLine(); }

            if (points <= 4) { Console.WriteLine("  |  "); }
            else { Console.WriteLine(); }

            if (points == 3) { Console.WriteLine(" /   "); }
            else if (points <= 2) { Console.WriteLine(" / \\"); }
            else { Console.WriteLine(); }

            if (points == 1) { Console.WriteLine("/    "); }
            else if (points <= 0) { Console.WriteLine("/   \\"); }
            else { Console.WriteLine(); }
        }

        // Vesztés esetén megejelenő nézet megjelenítése
        public static void DisplayLose(string wrd, int diff)
        {
            Console.Clear();
            DrawInfoBox(diff);
            // Az összes karakter megjelenítése attól függetlenül, hogy a felhasználó kitalálta-e azokat
            for (int i = 0; i < wrd.Length; i++)
            {
                Console.Write(wrd[i] + " ");
            }
            Console.WriteLine();
            DrawDoll();
            Console.WriteLine("Ön vesztett!");
            Console.ReadKey();
        }

        // Új szó hozzáadása
        public static void AddWord()
        {
            string new_word;
            Console.Clear();
            DrawInfoBox(0);
            Console.Write("Új szó: ");
            new_word = Console.ReadLine();

            FileStream fs = new FileStream("akasztofa.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            // Annak ellenőrzése, hogy a felhasználó által megadott szó szerepel-e már a listában
            bool van_mar = false;
            while (!sr.EndOfStream)
            {
                if (sr.ReadLine().Split(';')[0] == new_word)
                {
                    van_mar = true;
                }
            }

            sr.Close();
            fs.Close();

            // Ha a megadott szó még nem szerepel a listában, a szó hozzáadása a listához
            if (!van_mar)
            {
                FileStream fs2 = new FileStream("akasztofa.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs2, Encoding.UTF8);

                Console.WriteLine("\nKönnyű - 1");
                Console.WriteLine("Közepes - 2");
                Console.WriteLine("Nehéz - 3");
                Console.WriteLine();
                Console.Write("Adja meg a szó nehézségét: ");

                sw.WriteLine(new_word + ';' + Console.ReadLine());

                sw.Close();
                fs.Close();

                Console.WriteLine("\nÚj szó sikeresen hozzáadva!");
            }
            // Ha a szó szerepel már a listában, üzenet megjelenítése
            else
            {
                Console.WriteLine("\nVan már ilyen szó!");
            }
            Console.ReadKey();
        }

        static void Main()
        {
            // A játék futtatása addig, amíg a felhasználó be nem zárja azt
            while (true)
            {
                DisplayMenu();
            }
        }
    }
}
