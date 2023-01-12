using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Diagnostics.Tracing;
using System.IO;
using System.Text;

namespace Project_feladat
{
    class Program
    {

        static void MegjelenitKeret(int SOR, int OSZLOP)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(4, 10);
            for (int sor = 0; sor < SOR + 2; sor++)
            {
                for (int oszlop = 0; oszlop < OSZLOP + 2; oszlop++)
                {
                    if (sor == 0 || oszlop == 0 || sor == SOR + 1 || oszlop == OSZLOP + 1)
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

        static void MegjelenitVakterkep(int sor, int oszlop, char[,] terkep, string[] beallitasok)
        {
            List<int> karakter = new List<int>() {{0}, {0}};
            karakter[0] = Convert.ToInt32(beallitasok[1].Split(',')[0]);
            karakter[1] = Convert.ToInt32(beallitasok[1].Split(',')[1]);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.DarkGray;

            foreach (string ertekPar in beallitasok[3].Remove(beallitasok[3].Count()-1).Split(','))
            {
                int[] tempPar = new int[2];
                tempPar[0] = Convert.ToInt32(ertekPar.Split(':')[0]);
                tempPar[1] = Convert.ToInt32(ertekPar.Split(':')[1]);

                if (tempPar[0] == karakter[0] && tempPar[1] == karakter[1])
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(5 + tempPar[1], 11 + tempPar[0]);
                    Console.Write(terkep[tempPar[0], tempPar[1]]);
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.SetCursorPosition(5 + tempPar[1], 11 + tempPar[0]);
                    Console.Write(terkep[tempPar[0], tempPar[1]]);
                }
            }
            
        }

        static void MegjelenitJatek(int sor, int oszlop, char[,] terkep, string[] beallitasok)
        {
            List<List<int>> bejarat = Bejarat(terkep.GetLength(0), terkep.GetLength(1), terkep);
            List<int> karakter = bejarat[1];
            if (beallitasok.Length > 3)
            {
                karakter[0] = Convert.ToInt32(beallitasok[1].Split(',')[0]);
                karakter[1] = Convert.ToInt32(beallitasok[1].Split(',')[1]);
            }

            const int TABULATOR = 5;
            const int MAGASSAG = 11;
            for (int uressorok = 0; uressorok < MAGASSAG; uressorok++)
            {
                Console.WriteLine();
            }
            Console.Write(new String(' ', TABULATOR));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            for (int sorIndex = 0; sorIndex < sor; sorIndex++)
            {
                for (int oszlopIndex = 0; oszlopIndex < oszlop; oszlopIndex++)
                {
                    if (terkep[sorIndex, oszlopIndex] == '.')
                    {
                        Console.Write(' ');
                    }

                    else
                    {
                        if (sorIndex == karakter[0] && oszlopIndex == karakter[1])
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write(terkep[sorIndex, oszlopIndex]);
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }

                        else
                        {
                            Console.Write(terkep[sorIndex, oszlopIndex]);
                        }
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(". \n");
                Console.Write(" ".PadLeft(TABULATOR));
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.BackgroundColor = ConsoleColor.Black;

        }

        static char[,] Beolvas(string[] beallitasok)
        {
            if (beallitasok.Count() < 4)
            {
                string mentes_helye = AppDomain.CurrentDomain.BaseDirectory;
                mentes_helye += "palyak";
                if (!Directory.Exists(mentes_helye))
                {
                    Directory.CreateDirectory(mentes_helye);
                }
                mentes_helye += "\\";
                var txtFiles = Directory.EnumerateFiles(mentes_helye, "*.txt");
                string[] palyak = new string[txtFiles.Count()];
                int palyaIndex = 0;
                if (txtFiles.Count() == 0)
                {
                    Console.WriteLine($"\n\n\n\n\nNincsen mentett pálya a következő útvonalon:\n{mentes_helye}");
                }

                foreach (string file in txtFiles)
                {
                    string nev = file.Split('\\')[file.Split('\\').Count() - 1];
                    nev = nev.Split('.')[0];
                    palyak[palyaIndex] = nev;
                    palyaIndex++;
                }
                int valasztottPalya = 0;
                char bekeresPalya = ' ';

                while (bekeresPalya != 'e' && bekeresPalya != '\r')
                {
                    Console.SetCursorPosition(4, 1);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("LABIRINTUS");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(1, 4);
                    Console.WriteLine("Elérhető pályák:\n");

                    for (int gombIndex = 0; gombIndex < palyak.Length; gombIndex++)
                    {
                        if (gombIndex == valasztottPalya)
                        {
                            Console.SetCursorPosition(1, 6 + gombIndex * 2);
                            Console.WriteLine($"--> {palyak[gombIndex]} <--\n");
                        }
                        else
                        {
                            Console.SetCursorPosition(4, 6 + gombIndex * 2);
                            Console.WriteLine(palyak[gombIndex]);
                        }
                    }
                    bekeresPalya = char.ToLower(Console.ReadKey(true).KeyChar);
                    if (bekeresPalya == 's' && valasztottPalya < palyak.Count() - 1)
                    {
                        valasztottPalya++;
                    }
                    else if (bekeresPalya == 'w' && valasztottPalya > 0)
                    {
                        valasztottPalya--;
                    }
                    Console.Clear();
                }

                string[] sorok = System.IO.File.ReadAllLines($"{mentes_helye + palyak[valasztottPalya] + ".txt"}");
                int SOR = sorok.Length;
                int OSZLOP = sorok[0].Length;
                char[,] terkep = new char[SOR, OSZLOP];
                for (int sorIndex = 0; sorIndex < SOR; sorIndex++)
                {
                    for (int oszlopIndex = 0; oszlopIndex < OSZLOP; oszlopIndex++)
                    {
                        terkep[sorIndex, oszlopIndex] = sorok[sorIndex][oszlopIndex];
                    }
                }
                return terkep;
            }
            else
            {
                int SOR = beallitasok[0].Split('\n').Count() - 1;
                int OSZLOP = beallitasok[0].Split('\n')[0].Count();
                char[,] terkep = new char[SOR, OSZLOP];

                for (int sorIndex = 0; sorIndex < SOR; sorIndex++)
                {
                    for (int oszlopIndex = 0; oszlopIndex < OSZLOP; oszlopIndex++)
                    {
                        terkep[sorIndex, oszlopIndex] = beallitasok[0].Split('\n')[sorIndex][oszlopIndex];
                    }
                }

                return terkep;
            }
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

        static void KarakterMozgas(char[,] terkep, Dictionary<char, List<string>> mozgasIranyok, string[] beallitasokTomb)
        {
            string[] beallitasokBool = new string[3] { beallitasokTomb[beallitasokTomb.Count() - 1].Split(',')[0], beallitasokTomb[beallitasokTomb.Count() - 1].Split(',')[1], beallitasokTomb[beallitasokTomb.Count() - 1].Split(',')[2] };
            bool[] beallitasok = new bool[] { Convert.ToBoolean(beallitasokBool[0]), Convert.ToBoolean(beallitasokBool[1]), Convert.ToBoolean(beallitasokBool[2]) };
            List<List<int>> bejarat = Bejarat(terkep.GetLength(0), terkep.GetLength(1), terkep);
            List<int> karakter = new List<int>() { bejarat[1][0], bejarat[1][1] };
            if (beallitasokTomb.Count() > 3)
            {
                karakter[0] = Convert.ToInt32(beallitasokTomb[1].Split(',')[0]);
                karakter[1] = Convert.ToInt32(beallitasokTomb[1].Split(',')[1]);
            }
            int lepesekSzama = 0;
            bool isJatek = true;
            List<List<int>> kincs = KincsesTerem(terkep);
            List<List<int>> megtalaltKincsek = new List<List<int>>();

            if (beallitasokTomb.Count() > 3 && beallitasokTomb[2] != "")
            {
                List<int> jelenlegiKincs = new List<int>() {{0}, {0}};
                foreach (string adatPar in beallitasokTomb[2].Remove(beallitasokTomb[2].Length - 1).Split(','))
                {
                    jelenlegiKincs[0] = Convert.ToInt32(adatPar.Split(':')[0]);
                    jelenlegiKincs[1] = Convert.ToInt32(adatPar.Split(':')[1]);
                    megtalaltKincsek.Add(jelenlegiKincs);
                }
            }

            char[,] bejartUt = new char[terkep.GetLength(0), terkep.GetLength(1)];

            if (Convert.ToBoolean(beallitasokBool[0]) && beallitasokTomb.Count() > 3)
            {
                foreach (string vizsgaltPar in beallitasokTomb[3].Remove(beallitasokTomb[3].Count()-1).Split(','))
                {
                    bejartUt[Convert.ToInt32(vizsgaltPar.Split(':')[0]), Convert.ToInt32(vizsgaltPar.Split(':')[1])] = '.';
                }
            }


            while (isJatek)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(2, 4);
                Console.Write($"Megtalált kincses termek száma: {megtalaltKincsek.Count}");
                LehetsegesIranyok(karakter, terkep, mozgasIranyok);

                Console.SetCursorPosition(0, 9);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("    ");
                char bekeres = char.ToLower(Console.ReadKey(true).KeyChar);

                if (beallitasok[0] == true)
                {
                    bejartUt[karakter[0], karakter[1]] = '.';
                }


                switch (bekeres)
                {
                    case 'w':
                        if (karakter[0] - 1 >= 0 && mozgasIranyok[terkep[karakter[0], karakter[1]]].Contains("fel") && mozgasIranyok[terkep[karakter[0] - 1, karakter[1]]].Contains("le"))
                        {
                            karakter[0] -= 1;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.SetCursorPosition(5 + karakter[1], 12 + karakter[0]);
                            Console.Write(terkep[karakter[0] + 1, karakter[1]]);
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(5 + karakter[1], 11 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1]]);
                            lepesekSzama++;

                        }
                        break;

                    case 's':
                        if (karakter[0] + 1 < terkep.GetLength(0) && mozgasIranyok[terkep[karakter[0], karakter[1]]].Contains("le") && mozgasIranyok[terkep[karakter[0] + 1, karakter[1]]].Contains("fel"))
                        {
                            karakter[0] += 1;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.SetCursorPosition(5 + karakter[1], 10 + karakter[0]);
                            Console.Write(terkep[karakter[0] - 1, karakter[1]]);
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(5 + karakter[1], 11 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1]]);
                            lepesekSzama++;

                        }
                        break;

                    case 'a':
                        if (karakter[1] - 1 >= 0 && mozgasIranyok[terkep[karakter[0], karakter[1]]].Contains("balra") && mozgasIranyok[terkep[karakter[0], karakter[1] - 1]].Contains("jobbra"))
                        {
                            karakter[1] -= 1;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.SetCursorPosition(6 + karakter[1], 11 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1] + 1]);
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(5 + karakter[1], 11 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1]]);
                            lepesekSzama++;
                        }

                        break;

                    case 'd':
                        if (karakter[1] + 1 < terkep.GetLength(1) && mozgasIranyok[terkep[karakter[0], karakter[1]]].Contains("jobbra") && mozgasIranyok[terkep[karakter[0], karakter[1] + 1]].Contains("balra"))
                        {
                            karakter[1] += 1;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.SetCursorPosition(4 + karakter[1], 11 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1] - 1]);
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(5 + karakter[1], 11 + karakter[0]);
                            Console.Write(terkep[karakter[0], karakter[1]]);
                            lepesekSzama++;
                        }
                        break;

                    case 'r':

                        string mentesek = "";
                        for (int sor = 0; sor < terkep.GetLength(0); sor++)
                        {
                            for (int oszlop = 0; oszlop < terkep.GetLength(1); oszlop++)
                            {
                                mentesek += terkep[sor, oszlop];
                            }
                            mentesek += "\n";
                        }

                        mentesek += $";{Convert.ToString(karakter[0])},{Convert.ToString(karakter[1])};";

                        foreach (List<int> vizsgaltKincs in megtalaltKincsek)
                        {
                            mentesek += $"{Convert.ToString(vizsgaltKincs[0])}:{Convert.ToString(vizsgaltKincs[1])},";
                        }
                        mentesek += ";";
                        if (beallitasok[0])
                        {
                            for (int sor = 0; sor < bejartUt.GetLength(0); sor++)
                            {
                                for (int oszlop = 0; oszlop < bejartUt.GetLength(1); oszlop++)
                                {
                                    if (bejartUt[sor, oszlop] == '.')
                                    {
                                        mentesek += $"{Convert.ToString(sor)}:";
                                        mentesek += $"{Convert.ToString(oszlop)},";
                                    }
                                }
                            }
                            mentesek += ";";
                        }
                        foreach (bool beallitas in beallitasok)
                        {
                            mentesek += $"{Convert.ToString(beallitas)},";
                        }
                        Mentes(mentesek);
                        break;

                    case 'e':
                        bool bejaratMezo = false;
                        foreach (List<int> jelenlegiBejarat in bejarat)
                        {
                            if (jelenlegiBejarat[0] == karakter[0] && jelenlegiBejarat[1] == karakter[1])
                            {
                                bejaratMezo = true;
                            }
                        }

                        if (bejaratMezo)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(5, 11 + terkep.GetLength(0) + 2);
                            Console.WriteLine($"Biztos elakarod hagyni a labirintust?  i/n");
                            if (megtalaltKincsek.Count != kincs.Count)
                            {
                                Console.Write("     Még nem találtad meg az összes kincset.");
                            }
                            char valasz = ' ';
                            while (valasz != 'n' && valasz != 'i')
                            {
                                valasz = Console.ReadKey(true).KeyChar;
                            }

                            if (valasz == 'n')
                            {
                                Console.SetCursorPosition(5, 11 + terkep.GetLength(0) + 2);
                                Console.WriteLine("                                                  ");
                            }
                            else if (valasz == 'i')
                            {
                                Console.SetCursorPosition(5, 11 + terkep.GetLength(0) + 2);
                                Console.WriteLine("Sikeresen kijutottál a labirintusbol!           ");
                                Console.WriteLine($"     Megtett lépések száma: {lepesekSzama}               ");
                                Console.WriteLine($"     Megtalált kincses szobák száma: {megtalaltKincsek.Count} / {kincs.Count} ");
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

        static void Mentes(string adatok)
        {
            string mentesHelye = AppDomain.CurrentDomain.BaseDirectory + "mentes";
            if (!Directory.Exists(mentesHelye))
            {
                Directory.CreateDirectory(mentesHelye);
            }

            mentesHelye += "\\mentes.SAV";
            File.Delete(mentesHelye);
            StreamWriter fajlbaIras = new StreamWriter(mentesHelye, false, Encoding.UTF8);

            fajlbaIras.Write(adatok);
            fajlbaIras.Close();
        }

        static bool MentesEllenorzese()
        {
            string mentesHelye = AppDomain.CurrentDomain.BaseDirectory + "\\mentes\\mentes.SAV";
            bool vanMentes = File.Exists(mentesHelye);
            return vanMentes;
        }

        static string MentesBetoltese()
        {

            string readText = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "mentes\\mentes.SAV");
            return readText;
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
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(2, 0);
            Console.Write("Mozgás: w, a, s, d");
            Console.SetCursorPosition(2, 1);
            Console.Write("Interakció: e");
            Console.SetCursorPosition(2, 2);
            Console.Write("Mentés: r");

        }

        static void Jatek(string beallitasok)
        {
            string[] felbontottBeallitasok = beallitasok.Split(';');
            Dictionary<char, List<string>> mozgasIranyok = new Dictionary<char, List<string>>()
            {
                { '║', new List<string> { "fel", "le" } },
                { '╠', new List<string> { "fel", "le", "jobbra" } },
                { '╣', new List<string> { "fel", "le", "balra" } },
                { '╬', new List<string> { "fel", "le", "jobbra", "balra" } },
                { '╩', new List<string> { "fel", "jobbra", "balra" } },
                { '╦', new List<string> { "le", "jobbra", "balra" } },
                { '╚', new List<string> { "fel", "jobbra" } },
                { '╝', new List<string> { "fel", "balra" } },
                { '╔', new List<string> { "le", "jobbra" } },
                { '╗', new List<string> { "le", "balra" } },
                { '═', new List<string> { "jobbra", "balra" } },
                { '█', new List<string> { "fel", "le", "jobbra", "balra" } },
                { '.', new List<string> { } }
            };
            string[] elsoKetBeallitas = new string[] {felbontottBeallitasok[felbontottBeallitasok.Count() - 1].Split(',')[0], felbontottBeallitasok[felbontottBeallitasok.Count() - 1].Split(',')[1] };
            bool rejtettTerkep = Convert.ToBoolean(elsoKetBeallitas[0]);
            bool visszaSzamlalas = Convert.ToBoolean(elsoKetBeallitas[1]);
            Thread idoMutatas = new Thread(IdoMutat);

                char[,] terkep = Beolvas(felbontottBeallitasok);

            if (!rejtettTerkep)
            {
                MegjelenitJatek(terkep.GetLength(0), terkep.GetLength(1), terkep, felbontottBeallitasok);
            }
            else
            {
                MegjelenitKeret(terkep.GetLength(0), terkep.GetLength(1));
                if (felbontottBeallitasok.Length > 3)
                {
                    MegjelenitVakterkep(terkep.GetLength(0), terkep.GetLength(1), terkep, felbontottBeallitasok);
                }
            }

            IranyitasMegjelenites();

            if (visszaSzamlalas)
            {
                idoMutatas.Start();
            }

            KarakterMozgas(terkep, mozgasIranyok, felbontottBeallitasok);
            idoMutatas.Interrupt();
        }

        static int Kezdolap() 
        {
            string[] gombok = new string[] { "Folytatás", "Új játék", "Beállítások"};
            int valasztottKep = 1;
            char bekeres = ' ';
            while (bekeres != 'e' && bekeres != '\r')
            {
                Console.SetCursorPosition(4, 1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("LABIRINTUS");
                Console.ForegroundColor = ConsoleColor.White;


                for (int gombIndex = 1; gombIndex < 3; gombIndex++)
                {
                    if (gombIndex == valasztottKep)
                    {
                        Console.SetCursorPosition(1, 4 + gombIndex*2);
                        Console.WriteLine($"--> {gombok[gombIndex]} <--\n");
                    }
                    else
                    {
                        Console.SetCursorPosition(4, 4 + gombIndex*2);
                        Console.WriteLine(gombok[gombIndex]);
                    }

                }

                if (!MentesEllenorzese())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                if (valasztottKep == 0)
                {
                    Console.SetCursorPosition(1, 4);
                    Console.WriteLine($"--> {gombok[0]} <--\n");
                }
                else
                {
                    Console.SetCursorPosition(4, 4);
                    Console.WriteLine(gombok[0]);
                }

                bekeres = char.ToLower(Console.ReadKey(true).KeyChar);

                if (bekeres == 's' && valasztottKep < 2)
                {
                    valasztottKep++;
                }
                else if (bekeres == 'w' && valasztottKep > 0)
                {
                    valasztottKep--;
                }
                Console.Clear();

            }
            return valasztottKep;
        }

        static bool[] Beallitasok(bool[] beallitasok)
        {

            string[] gombok = new string[] { "Vaktérkép", "Visszaszámláló", "nyelv", "vissza" };
            int valasztottKep = 1;
            char bekeres = ' ';
            while (bekeres != 'e' && bekeres != '\r' || valasztottKep < 3)
            {
                Console.SetCursorPosition(4, 1); 
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("LABIRINTUS");
                Console.ForegroundColor = ConsoleColor.White;


                for (int gombIndex = 0; gombIndex < 4; gombIndex++)
                {
                    if (gombIndex < 3)
                    {
                        if (gombIndex == valasztottKep)
                        {
                            Console.SetCursorPosition(4, 4 + gombIndex * 2);
                            Console.Write($"{gombok[gombIndex]}:{"".PadRight(19 - gombok[gombIndex].Count())}");
                            if (beallitasok[gombIndex] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("be");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("ki");
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" <--");
                        }
                        else
                        {
                            Console.SetCursorPosition(4, 4 + gombIndex * 2);
                            Console.Write($"{gombok[gombIndex]}:{"".PadRight(18 - gombok[gombIndex].Count())}");
                            if (beallitasok[gombIndex] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("be");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("ki");
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"\n{"Vissza", 17}");
                        if (valasztottKep == 3)
                        {
                            Console.Write(" <--");
                        }
                    } 

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("");
                }


                bekeres = char.ToLower(Console.ReadKey(true).KeyChar);

                if (bekeres == 's' && valasztottKep < 3)
                {
                    valasztottKep++;
                }

                else if (bekeres == 'w' && valasztottKep > 0)
                {
                    valasztottKep--;
                }

                else if ((bekeres == 'e' || bekeres == '\r') && valasztottKep < 3)
                {
                    if (beallitasok[valasztottKep] == false)
                    {
                        beallitasok[valasztottKep] = true;
                    }

                    else
                    {
                        beallitasok[valasztottKep] = false;
                    }
                }

                Console.Clear();

            }

            return beallitasok;
        }


        static void JatekProgram()
        {
            bool[] beallitasok = new bool[3] {false, false, false};
            int valasztottKep = 3;
            while (true)
            {
                if (valasztottKep == 0)
                {
                    Jatek(MentesBetoltese());
                }

                if (valasztottKep == 1)
                {
                    string adatok = $"{Convert.ToString(beallitasok[0])},{Convert.ToString(beallitasok[1])},{Convert.ToString(beallitasok[2])}";
                    Console.Clear();
                    Jatek(adatok);
                    Thread.Sleep(5000);
                    valasztottKep = 3;
                }

                if (valasztottKep == 2)
                {
                    beallitasok = Beallitasok(beallitasok);
                    valasztottKep = 3;
                }

                if (valasztottKep == 3)
                {
                    valasztottKep = Kezdolap();
                }
            }
        }
        static void Main(string[] args)
        {
            JatekProgram();
        }

    }
}