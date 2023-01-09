using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Diagnostics.Tracing;

namespace Project_feladat
{
    class Program
    {

        static void MegjelenitKeret(int SOR, int OSZLOP)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(4, 9);
            for (int sor = 0; sor < SOR+2; sor++)
            {
                for (int oszlop = 0; oszlop < OSZLOP+2; oszlop++)
                {
                    if (sor == 0 || oszlop == 0 || sor == SOR+1 || oszlop == OSZLOP+1)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("    ");
                Console.BackgroundColor = ConsoleColor.DarkRed;
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static void MegjelenitJatek(int sor, int oszlop, char[,] map, List<int> karakter)
        {
            const int TABULATOR = 5;
            const int MAGASSAG = 10;
            for (int uressorok = 0; uressorok < MAGASSAG; uressorok++)
            {
                Console.WriteLine();
            }
            Console.Write(new String(' ', TABULATOR));
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Gray;
            for (int sorIndex = 0; sorIndex < sor; sorIndex++)  // todo  playert irányítani
            {
                for (int oszlopIndex = 0; oszlopIndex < oszlop; oszlopIndex++)
                {
                    if (map[sorIndex, oszlopIndex] == '.')
                    {
                        Console.Write(' ');
                    }

                    else
                    {
                        if (sorIndex == karakter[0] && oszlopIndex == karakter[1])
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write(map[sorIndex, oszlopIndex]);
                            Console.BackgroundColor = ConsoleColor.Gray;
                        }
                        else
                        {
                            Console.Write(map[sorIndex, oszlopIndex]);
                        }
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(". \n");
                Console.Write(" ".PadLeft(TABULATOR));
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.BackgroundColor = ConsoleColor.Black;

        }

        static char[,] Beolvas()
        {

            string mentes_helye = AppDomain.CurrentDomain.BaseDirectory;
            mentes_helye += @"palyak\";

            var txtFiles = Directory.EnumerateFiles(mentes_helye, "*.txt");
            Console.WriteLine("Pályák nevei:\n");
            foreach (string file in txtFiles)
            {
                string nev = file.Split(@"\")[file.Split(@"\").Count()-1];
                nev = nev.Split(".txt")[0];
                Console.WriteLine($" - {nev}");
            }
            Console.Write("\nKérem a választott pálya nevét: ");
            string palya = Console.ReadLine();

            string[] sorok = System.IO.File.ReadAllLines(@$"{mentes_helye+palya+".txt"}");
            int SOR = sorok.Length;
            int OSZLOP = sorok[0].Length;
            char[,] map = new char[SOR, OSZLOP];
            for (int sorIndex = 0; sorIndex < SOR; sorIndex++)
            {
                for (int oszlopIndex = 0; oszlopIndex < OSZLOP; oszlopIndex++)
                {
                    map[sorIndex, oszlopIndex] = sorok[sorIndex][oszlopIndex];
                }
            }
            return map;
        }

        static List<List<int>> Bejarat(int SOR, int OSZLOP, char[,] terkep)
        {
            List<char> sorBejaratFent = new List<char> { '║', '╠', '╣', '╬', '╩', '╚', '╝' };
            List<char> sorBejaratLent = new List<char> { '║', '╠', '╣', '╬', '╔', '╗', '╦' };
            List<char> oszlopBejaratBalra = new List<char> { '═', '╣', '╬', '╝', '╩', '╗', '╦' };
            List<char> oszlopBejaratJobbra = new List<char> { '═', '╬', '╩', '╦', '╔', '╚', '╠' };
            List<List<int>> bejarat = new List<List<int>>();
            for (int sorIndex = 0; sorIndex < SOR; sorIndex++)
            {
                for (int oszlopIndex = 0; oszlopIndex < OSZLOP; oszlopIndex++)
                {
                    if (sorIndex == 0 && sorBejaratFent.Contains(terkep[sorIndex, oszlopIndex]) || oszlopIndex == 0 && oszlopBejaratBalra.Contains(terkep[sorIndex, oszlopIndex]) || sorIndex == SOR - 1 && sorBejaratLent.Contains(terkep[sorIndex, oszlopIndex]) || oszlopIndex == OSZLOP - 1 && oszlopBejaratJobbra.Contains(terkep[sorIndex, oszlopIndex]))
                    {
                        bejarat.Add(new List<int> { sorIndex, oszlopIndex });
                    }
                }
            }
            return bejarat;
        }

        static List<List<int>> KincsesTerem(char[,] terkep)
        {
            int SOR = terkep.GetLength(0);
            int OSZLOP = terkep.GetLength(1);
            List<List<int>> kincs = new List<List<int>>();

            for (int sorIndex = 0; sorIndex < SOR; sorIndex++)
            {
                for (int oszlopIndex = 0; oszlopIndex < OSZLOP; oszlopIndex++)
                {
                    if (terkep[sorIndex, oszlopIndex] == '█')
                    {
                        kincs.Add(new List<int> { sorIndex, oszlopIndex });
                    }
                }
            }
            return kincs;
        }

        static void KarakterMozgas(char[,] terkep, Dictionary<char, List<string>> mozgasIranyok, List<List<int>> bejarat)
        {
            List<int> karakter = bejarat[1];
            int lepesekSzama = 0;
            bool isJatek = true;
            List<List<int>> kincs = KincsesTerem(terkep);
            List<List<int>> megtalaltKincsek = new List<List<int>>();
            while (isJatek)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(2, 4);
                Console.Write($"Megtalált kincses termek száma: {megtalaltKincsek.Count()}");
                LehetsegesIranyok(karakter, terkep, mozgasIranyok);

                Console.SetCursorPosition(0, 9);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("    ");
                char bekeres = Console.ReadKey(true).KeyChar;

                switch (bekeres)
                {
                    case 'w':
                        if (karakter[0] - 1 >= 0 && mozgasIranyok[terkep[karakter[0], karakter[1]]].Contains("fel") && mozgasIranyok[terkep[karakter[0] - 1, karakter[1]]].Contains("le"))
                        {
                            karakter[0] -= 1;
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.SetCursorPosition(5 + karakter[1], 11 + karakter[0]);
                            Console.Write(terkep[karakter[0] + 1, karakter[1]]);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(5 + karakter[1], 10 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1]]);
                            lepesekSzama++;

                        }
                        break;

                    case 's':
                        if (karakter[0] + 1 < terkep.GetLength(0) && mozgasIranyok[terkep[karakter[0], karakter[1]]].Contains("le") && mozgasIranyok[terkep[karakter[0] + 1, karakter[1]]].Contains("fel"))
                        {
                            karakter[0] += 1;
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.SetCursorPosition(5 + karakter[1], 9 + karakter[0]);
                            Console.Write(terkep[karakter[0] - 1, karakter[1]]);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(5 + karakter[1], 10 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1]]);
                            lepesekSzama++;

                        }
                        break;

                    case 'a':
                        if (karakter[1] - 1 >= 0 && mozgasIranyok[terkep[karakter[0], karakter[1]]].Contains("balra") && mozgasIranyok[terkep[karakter[0], karakter[1] - 1]].Contains("jobbra"))
                        {
                            karakter[1] -= 1;
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.SetCursorPosition(6 + karakter[1], 10 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1] + 1]);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(5 + karakter[1], 10 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1]]);
                            lepesekSzama++;
                        }

                        break;

                    case 'd':
                        if (karakter[1] + 1 < terkep.GetLength(1) && mozgasIranyok[terkep[karakter[0], karakter[1]]].Contains("jobbra") && mozgasIranyok[terkep[karakter[0], karakter[1] + 1]].Contains("balra"))
                        {
                            karakter[1] += 1;
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.SetCursorPosition(4 + karakter[1], 10 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1] - 1]);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(5 + karakter[1], 10 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1]]);
                            lepesekSzama++;
                        }
                        break;

                    case 'r':
                        string mentes_helye = AppDomain.CurrentDomain.BaseDirectory;
                        mentes_helye += @"mentes";
                        if (!Directory.Exists(mentes_helye))
                        {
                            Directory.CreateDirectory(mentes_helye);
                        }
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(5, 20);
                        Console.WriteLine(mentes_helye);

                        break;

                    case 'e':
                        if (bejarat.Contains(karakter))
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(5, 10 + terkep.GetLength(0) + 2);
                            Console.Write($"Biztos elakarod hagyni a labirintust?  i/n");
                            char valasz = ' ';
                            while (valasz != 'n' && valasz != 'i')
                            {
                                valasz = Console.ReadKey(true).KeyChar;
                            }

                            if (valasz == 'n')
                            {
                                Console.SetCursorPosition(5, 10 + terkep.GetLength(0) + 2);
                                Console.WriteLine("                                                  ");
                            }
                            else if (valasz == 'i')
                            {
                                Console.SetCursorPosition(5, 10 + terkep.GetLength(0) + 2);
                                Console.WriteLine("Sikeresen kijutottál a labirintusbol!           ");
                                Console.WriteLine($"     Megtett lépések száma: {lepesekSzama}");
                                Console.WriteLine($"     Megtalált kincses szobák száma: {megtalaltKincsek.Count()} / {kincs.Count()} ");
                                isJatek = false;
                            }
                        }
                        break;

                    default:
                        break;
                }
                foreach (List<int> vizsgaltKincs in kincs)
                {
                    if (vizsgaltKincs[0] == karakter[0] && vizsgaltKincs[1] == karakter[1])
                    {
                        bool benneVane = false;
                        foreach (List<int> megtalaltKincs in megtalaltKincsek)
                        {
                            if (megtalaltKincs[0] == vizsgaltKincs[0] && megtalaltKincs[1] == vizsgaltKincs[1])
                            {
                                benneVane = true;
                            }
                        }
                        if (!benneVane)
                        {
                            megtalaltKincsek.Add(new List<int> { karakter[0], karakter[1] });
                        }
                    }
                }
            }
        }

        public static async Task mentes(string[] adatok)
        {
            await File.WriteAllLinesAsync("WriteLines.txt", adatok);
        }
        static void IdoMutat()
        {
            
            float ido = 100;
            while (ido > 0)
            {
                Thread.Sleep(100);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(2, 8);
                Console.Write("Idő a teljes beomlásig: ");
                ido -= 0.1f;
                Console.SetCursorPosition(27, 8);
                Console.Write("   ");
                Console.SetCursorPosition(26, 8);
                Console.Write($"{ido:0.0}");
                Console.SetCursorPosition(30, 8);
                Console.Write("mp                                          ");
                Console.SetCursorPosition(0, 9);
            }

            
        }

        static void LehetsegesIranyok(List<int> karakter, char[,] terkep, Dictionary<char, List<string>> mozgasIranyok)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(2, 6);
            if (terkep[karakter[0], karakter[1]] != '█')
            {
            Console.Write($"Lehetséges irányok: {string.Join(", ", mozgasIranyok[terkep[karakter[0], karakter[1]]])}                                ");
            }
            else
            {
                Console.Write("          kincses terem                   ");
            }
        }

        static void IranyitasMegjelenites()
        {

            Console.SetCursorPosition(2, 0);
            Console.Write("Mozgás: w, a, s, d");
            Console.SetCursorPosition(2, 1);
            Console.Write("Interakció: e");
            Console.SetCursorPosition(2, 2);
            Console.Write("Mentés: r");

        }

        static void Jatek()
        {
            Dictionary<char, List<string>> mozgasIranyok = new Dictionary<char, List<string>>();
            mozgasIranyok.Add('║', new List<string> { "fel", "le" });
            mozgasIranyok.Add('╠', new List<string> { "fel", "le", "jobbra" });
            mozgasIranyok.Add('╣', new List<string> { "fel", "le", "balra" });
            mozgasIranyok.Add('╬', new List<string> { "fel", "le", "jobbra", "balra" });
            mozgasIranyok.Add('╩', new List<string> { "fel", "jobbra", "balra" });
            mozgasIranyok.Add('╦', new List<string> { "le", "jobbra", "balra" });
            mozgasIranyok.Add('╚', new List<string> { "fel", "jobbra" });
            mozgasIranyok.Add('╝', new List<string> { "fel", "balra" });
            mozgasIranyok.Add('╔', new List<string> { "le", "jobbra" });
            mozgasIranyok.Add('╗', new List<string> { "le", "balra" });
            mozgasIranyok.Add('═', new List<string> { "jobbra", "balra" });
            mozgasIranyok.Add('█', new List<string> { "fel", "le", "jobbra", "balra" });
            mozgasIranyok.Add('.', new List<string> {});

            bool rejtettTerkep = true;
            bool visszaSzamlalas = true;
            Thread idoMutatas = new Thread(IdoMutat);
            char[,] terkep = Beolvas();
            int SOR = terkep.GetLength(0);
            int OSZLOP = terkep.GetLength(1);
            List<List<int>> bejarat = Bejarat(SOR, OSZLOP, terkep);
            List<int> karakter = bejarat[1];
            Console.Clear();
            if (!rejtettTerkep)
            {
            MegjelenitJatek(SOR, OSZLOP, terkep, karakter);
            }
            else
            {
                MegjelenitKeret(SOR, OSZLOP);
            }

            IranyitasMegjelenites();
  
            if (visszaSzamlalas)
            {
                idoMutatas.Start();
            }

            KarakterMozgas(terkep, mozgasIranyok, bejarat);
            idoMutatas.Interrupt();
        }

        //static string[] Kezdolap() 
        //{ 
        //
        ///}
        static void Main(string[] args)
        {

            Jatek();
        }

    }
}