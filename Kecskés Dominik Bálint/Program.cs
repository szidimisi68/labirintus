using System;
using System.Collections;
using System.Text;

namespace Labirintus_Palyaszerk
{
    static class GlobalVariables
    {
        public static Dictionary<string, char> characters = new Dictionary<string, char>() {
                {"Room", '█' },
                {"Fill", '.' },
                {"All Intersect", '╬' },
                {"Horizontal Line", '═' },
                {"T Intersect", '╦' },
                {"Reverse T Intersect", '╩' },
                {"Vertical Line", '║' },
                {"Right T Intersect", '╣' },
                {"Left T Intersect", '╠' },
                {"Right Upper Corner", '╗' },
                {"Right Lower Corner", '╝' },
                {"Left Lower Corner", '╚' },
                {"Left Upper Corner", '╔' }
            };
    }
    class Program
    {
        static void PrintMatrix(char[,] text, int startHere)
        {
            Console.SetCursorPosition(0, startHere);
            for (int i = 0; i < text.GetLength(0); i++)
            {
                for (int j = 0; j < text.GetLength(1); j++)
                {
                    Console.Write(text[i, j]);
                }
                Console.WriteLine();
            }
        }

        static char[,] Palyakeszites()
        {
            Console.Write("Méret (m*n):");
            string valasz = Console.ReadLine();
            string[] a = valasz.Split('*');
            int[] ertekek = new int[2];
            ertekek[0] = int.Parse(a[0]);
            ertekek[1] = int.Parse(a[1]);
            Console.Clear();
            char[,] palya = new char[ertekek[1], ertekek[0]];
            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    palya[i, j] = GlobalVariables.characters["Fill"];
                }
            }
            return palya;
        }

        static void Palyaszerkesztes(char[,] palya, bool mentett)
        {
            Console.Clear();
            Console.WriteLine($"Méret: {palya.GetLength(1)}*{palya.GetLength(0)} ");
            int[] karakterPozicio = new int[2] { 0, 0 };
            int elemTipus = 0;
            Dictionary<int, char> elemek = new Dictionary<int, char>();
            foreach (KeyValuePair<string, char> karakter in GlobalVariables.characters)
            {
                elemek.Add(elemTipus, karakter.Value);
                if (elemek[elemTipus] == GlobalVariables.characters["Fill"])
                {
                    elemek.Remove(elemTipus);
                    elemTipus--;
                }
                elemTipus++;

            }
            elemTipus = 0;
            bool lehetHasznalniOpciokat = true;
            Console.WriteLine("Új elem: U");
            Console.WriteLine("Elem típus választás: R");
            Console.WriteLine("Elem eltávolítás: E");
            Console.WriteLine($"Elem típus: {elemek[elemTipus]}");
            Console.WriteLine("Mozgás: W, A, S, D");
            Console.WriteLine("Mentés: M");
            Console.WriteLine("Kilépés: K");
            if (mentett)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Pálya Mentve]");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Pálya Nincs Elmentve]");
                Console.ResetColor();
            }
            PrintMatrix(palya, 12);
            Console.SetCursorPosition(0, 12);
            while (lehetHasznalniOpciokat)
            {
                string bevitel = Console.ReadKey(true).Key.ToString();
                bevitel = bevitel.ToLower();
                if (bevitel == "u")
                {
                    palya[karakterPozicio[0], karakterPozicio[1]] = elemek[elemTipus];
                    Console.Write(palya[karakterPozicio[0], karakterPozicio[1]]);
                    Console.SetCursorPosition(karakterPozicio[1], karakterPozicio[0] + 12);
                    if (mentett)
                    {
                        Console.SetCursorPosition(0, 8);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("[Pálya Nincs Elmentve]");
                        Console.ResetColor();
                        mentett = false;
                    }
                }
                else if (bevitel == "w" || bevitel == "a" || bevitel == "s" || bevitel == "d")
                {
                    if (bevitel == "w" && karakterPozicio[0] > 0)
                    {
                        karakterPozicio[0]--;
                    }
                    else if (bevitel == "s" && karakterPozicio[0] < palya.GetLength(0) - 1)
                    {
                        karakterPozicio[0]++;
                    }
                    else if (bevitel == "a" && karakterPozicio[1] > 0)
                    {
                        karakterPozicio[1]--;
                    }
                    else if (bevitel == "d" && karakterPozicio[1] < palya.GetLength(1) - 1)
                    {
                        karakterPozicio[1]++;
                    }
                    Console.SetCursorPosition(karakterPozicio[1], karakterPozicio[0] + 12);
                }
                else if (bevitel == "e")
                {
                    palya[karakterPozicio[0], karakterPozicio[1]] = GlobalVariables.characters["Fill"];
                    Console.Write(palya[karakterPozicio[0], karakterPozicio[1]]);
                    Console.SetCursorPosition(karakterPozicio[1], karakterPozicio[0] + 12);
                    if (mentett)
                    {
                        Console.SetCursorPosition(0, 8);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("[Pálya Nincs Elmentve]");
                        Console.ResetColor();
                        mentett = false;
                    }
                }
                else if (bevitel == "r")
                {
                    Console.SetCursorPosition(0, 4);
                    if (elemTipus < elemek.Count - 1)
                    {
                        elemTipus++;
                    }
                    else
                    {
                        elemTipus = 0;
                    }
                    Console.Write($"Elem típus: {elemek[elemTipus]}");
                    Console.SetCursorPosition(karakterPozicio[1], karakterPozicio[0] + 12);
                }
                else if (bevitel == "m")
                {
                    if (!mentett)
                    {
                        lehetHasznalniOpciokat = false;
                        bool[] visszaTer = PalyaMentes(palya);
                        if (!visszaTer[0])
                        {
                            Palyaszerkesztes(palya, visszaTer[1]);
                        }
                    }
                }
                else if (bevitel == "k")
                {
                    if (!mentett)
                    {
                        lehetHasznalniOpciokat = false;
                        Console.Clear();
                        int valasztott = 0;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Biztosan kilépsz mentés nélkül?");
                        Console.ResetColor();
                        Console.WriteLine("");
                        string[] opc = { "Igen", "Nem", "Mentés és kilépés" };
                        for (int i = 0; i < opc.Length; i++)
                        {
                            Console.WriteLine(opc[i]);
                        }
                        bool ad = true;
                        while (ad)
                        {

                            ConsoleKey bevitel2 = Console.ReadKey(true).Key;

                            if (bevitel2.ToString().ToLower() == "w" || bevitel2.ToString().ToLower() == "a")
                            {
                                if (valasztott > 0)
                                {
                                    Console.SetCursorPosition(0, valasztott+2);
                                    Console.Write(opc[valasztott]);
                                    valasztott--;
                                }
                            }
                            else if (bevitel2.ToString().ToLower() == "s" || bevitel2.ToString().ToLower() == "d")
                            {
                                if (valasztott < opc.Length - 1)
                                {
                                    Console.SetCursorPosition(0, valasztott+2);
                                    Console.Write(opc[valasztott]);
                                    valasztott++;
                                }
                            }
                            else if (bevitel2 == ConsoleKey.Enter)
                            {
                                Console.Clear();
                                if (valasztott == 0)
                                {
                                    Menu();
                                }
                                else if (valasztott == 1)
                                {
                                    Palyaszerkesztes(palya, false);
                                }
                                else if (valasztott == 2)
                                {
                                    PalyaMentes(palya);
                                    Menu();
                                }
                                ad = false;
                            }
                            Console.SetCursorPosition(0, valasztott+2);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(opc[valasztott]);
                            Console.ResetColor();
                        }
                    } else
                    {
                        lehetHasznalniOpciokat = false;
                        Menu();
                    }
                }
            }
        }

        static bool[] PalyaMentes(char[,] palya, bool kilep = false)
        {
            bool[] toReturn = new bool[2] {kilep, false};
            string mentesHelye = AppDomain.CurrentDomain.BaseDirectory + "mentes";
            if (!Directory.Exists(mentesHelye))
            {
                Directory.CreateDirectory(mentesHelye);
            }
            Random rand = new Random();
            mentesHelye += $"\\{Directory.GetFiles(mentesHelye).Length+1}.txt";
            StreamWriter fajlbaIras = new StreamWriter(mentesHelye, false, Encoding.UTF8);
            string formazottSzoveg = "";
            for (int i = 0; i < palya.GetLength(0); i++)
            {
                for (int j = 0; j < palya.GetLength(1); j++)
                {
                    formazottSzoveg += palya[i, j];
                }
                formazottSzoveg += "\n";
            }
            fajlbaIras.Write(formazottSzoveg);
            fajlbaIras.Close();
            toReturn[1] = true;
            return toReturn;
        }

        static char[,] Palyabetoltes()
        {
            string mentesHelye = AppDomain.CurrentDomain.BaseDirectory + "mentes";
            char[,] palya = new char[0,0];
            string valasztottFajl = "";
            if (Directory.Exists(mentesHelye))
            {
                int valasztott = 0;

                string[] fajlok = Directory.GetFiles(mentesHelye);
                for (int i = 0; i < fajlok.Length; i++)
                {
                    Console.WriteLine(Path.GetFileName(fajlok[i]));
                }
                while (true)
                {
                    ConsoleKey bevitel = Console.ReadKey(true).Key;
                    
                    if (bevitel.ToString().ToLower() == "w" || bevitel.ToString().ToLower() == "a")
                    {
                        if (valasztott > 0)
                        {
                            Console.SetCursorPosition(0, valasztott);
                            Console.Write(Path.GetFileName(fajlok[valasztott]));
                            valasztott--;
                        }
                    } 
                    else if (bevitel.ToString().ToLower() == "s" || bevitel.ToString().ToLower() == "d")
                    {
                        if (valasztott < fajlok.Length-1)
                        {
                            Console.SetCursorPosition(0, valasztott);
                            Console.Write(Path.GetFileName(fajlok[valasztott]));
                            valasztott++;
                        }
                    }
                    else if (bevitel == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        valasztottFajl = fajlok[valasztott];
                        break;
                    }
                    Console.SetCursorPosition(0, valasztott);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(Path.GetFileName(fajlok[valasztott]));
                    Console.ResetColor();
                }
            }
            
            
            string[] formazott = System.IO.File.ReadAllText(valasztottFajl).Split(new char[] { '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            int[] meret = { 0, 0 };
            for (int i = 0; i < formazott.Length; i++)
            {
                for (int j = 0; j < formazott[i].Length; j++)
                {
                    meret[1]++;
                }
                meret[0]++;
            }
            meret[1] /= meret[0];
            palya = new char[meret[0], meret[1]];
            for (int i = 0; i < formazott.Length; i++)
            {
                for (int j = 0; j < formazott[i].Length; j++)
                {
                    palya[i, j] = formazott[i][j];
                }
            }
            return palya;
        }

        static void Menu()
        {
            Console.Clear();
            Console.WriteLine("Labirintus pályaszerkesztő");
            Console.WriteLine("Új pálya: F");
            Console.WriteLine("Pálya betöltése: G");
            string a = Console.ReadKey(true).Key.ToString();
            if (a.ToLower() == "f")
            {
                Console.Clear();
                Palyaszerkesztes(Palyakeszites(), false);
            }
            else if (a.ToLower() == "g")
            {
                Console.Clear();
                Palyaszerkesztes(Palyabetoltes(), true);
            }
            else
            {
                Menu();
            }
            

        }

        static void Main()
        {
            Menu();
        }
    }
}
