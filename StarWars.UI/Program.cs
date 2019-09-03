using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StarWars.Domain.Entities;
using StarWars.Service;

namespace StarWars.UI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.WriteLine($"\n\nSW Star Ships - Ressupply Calculation for Travel between Planets\n\n");

                Console.Write("Input the distance in mega lights (MGLT): ");

                var input = Console.ReadLine();

                if (InputIsValid(input, out long distanceInMGLT))
                {
                    Console.WriteLine("\nLoading... please wait");

                    try
                    {
                        var starShips = new StarWarsService().GetAllStarshipsWithNumberOfRessupplies(distanceInMGLT);

                        // Clear loading message on console
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 2);

                        DisplayStarShips(starShips);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                    Console.WriteLine("\nInput invalid.");

                Console.WriteLine($"\n\nPress 'Esc' to escape or any other key to try again");

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        public static bool InputIsValid(string input, out long distanceInMGLT)
        {
            distanceInMGLT = 0;

            // A number (long) is an input valid
            return Regex.IsMatch(input, @"^\d+$") && long.TryParse(input, out distanceInMGLT) && distanceInMGLT > 0;
        }

        private static void DisplayStarShips(List<Starship> starships)
        {
            if (starships != null && starships.Count > 0)
            {
                var titleStarShip = "Star Ship";
                var titleResupplies = "Resupplies";

                // Format columns sizes
                var maxLenghtName = starships.Max(x => x.Name.Length);
                if (titleStarShip.Length > maxLenghtName)
                    maxLenghtName = titleStarShip.Length;

                var maxLenghtResupplies = starships.Max(x => x.ResupplyFrequency.Length);
                if (titleResupplies.Length > maxLenghtResupplies)
                    maxLenghtResupplies = titleResupplies.Length;

                // Print columns header 
                Console.WriteLine($"\n{titleStarShip.PadRight(maxLenghtName)}   { titleResupplies.PadLeft(maxLenghtResupplies) }");
                Console.WriteLine($"{new String('-', maxLenghtName)}   { new String('-', maxLenghtResupplies) }");

                // Print Star Ships
                foreach (var starship in starships.OrderBy(spc => spc.Name))
                    Console.WriteLine($"{starship.Name.PadRight(maxLenghtName)}   { starship.ResupplyFrequency.PadLeft(maxLenghtResupplies) }");
            }
        }
    }
}
