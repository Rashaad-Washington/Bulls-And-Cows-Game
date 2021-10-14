using System;
// Rashaad Washington
// CSCI 3005
// Assignment 2

namespace BullAndCow
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== Welcome to Bull and Cow ======\n");
            Console.WriteLine("== Developed by Rashaad Washington ==\n");
            Console.WriteLine("=============  Yee-Haw  =============\n");
            Console.WriteLine("=====================================\n");
            Game g = new();

            g.CreateSecret();
            g.SetupCheat();
            g.Play();
            g.ShowHighScores();
        }
    }
}
