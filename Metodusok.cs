using System;
using System.Collections.Generic;

namespace ConsoleApp23
{
    class Program
    {
        /// <summary>
        /// Megadja, hogy hány termet tartamaz a térkép
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Termek száma</returns>
        static int TeremSzamolo(char[,] map)
        {
            int szam = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '█' )
                    {
                        szam++;
                    }
                }
            }

            return szam;
        }
        /// <summary>
        /// A kapott térkép széleit végignézve megállapítja, hogy hány kijárat van.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Az alkalmas kijáratok száma</returns>
        static int GetSuitableEntrance(char[,] map)
        {
            List<char> sorBejaratFent = new List<char> { '║', '╠', '╣', '╬', '╩', '╚', '╝' };
            List<char> sorBejaratLent = new List<char> { '║', '╠', '╣', '╬', '╔', '╗', '╦' };
            List<char> oszlopBejaratBalra = new List<char> { '═', '╣', '╬', '╝', '╩', '╗', '╦' };
            List<char> oszlopBejaratJobbra = new List<char> { '═', '╬', '╩', '╦', '╔', '╚', '╠' };
            int kijaratok = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == 0 && sorBejaratFent.Contains(map[i, j])){
                        kijaratok++;
                    }
                    else if (j == 0 && oszlopBejaratBalra.Contains(map[i, j]))
                    {
                        kijaratok++;
                    }
                    else if (i == map.GetLength(0) - 1 && sorBejaratLent.Contains(map[i, j]))
                    {
                         kijaratok++;
                    }
                    else if (j == map.GetLength(1) - 1 && oszlopBejaratJobbra.Contains(map[i, j]))
                    {
                        kijaratok++;
                    }
                }
            }
            return kijaratok;
        }
        /// <summary>
        /// Megnézi, hogy van-e a térképen meg nem engedett karakter?
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>true - A térkép tartalmaz szabálytalan karaktert, false - nincs benne ilyen</returns>
        static bool IsInvalidElement(char[,] map)
        {
            List<char> jok = new List<char>() { '╬', '═', '╦', '╩', '║', '╣', '╠', '╗', '╝', '╚', '╔', '.', '█' };
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (!jok.Contains(map[i, j]))
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        /// <summary>
        /// Visszaadja azoknak a járatkaraktereknek a pozícióját, amelyekhez egyetlen szomszéd pozícióból sem lehet eljutni.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>A pozíciók "sor_index:oszlop_index" formátumban szerepelnek a lista elemeiként
        static List<string> GetUnavailableElements(char[,] map)
        {
            List<string> unavailables = new List<string>();
            // ?
            // pld: string poz = "4:12"; 
            return unavailables;
        }
        /// <summary>
        /// Labiritust generál a kapott pozíciókat tartalmazó lista alapján. A lista elemei egymáshoz kapcsolódó járatok pozíciói.
        /// </summary>
        /// <param name="positionsList">"sor_index:oszlop_index" formátumban az egymáshoz kapcsolódó járatok pozícióit tartalmazó lista </param>
        /// <returns>A létrehozott labirintus térképe</returns>
        static char[,] GenerateLabyrinth(List<string> positionsList)
        {
            return null;
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}