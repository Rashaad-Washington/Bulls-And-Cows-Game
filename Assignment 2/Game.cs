using System;
using System.IO;
using System.Linq;
// Rashaad Washington
// CSCI 3005
// Assignment 2

namespace BullAndCow
{
    class Secret //represents the secret code the user has to guess
    {
        private int[] _target;
        private int bullCount = 0;
        private int cowCount = 0;

        public string Target
        {
            get
            {
                string result = "";
                for (int i = 0; i < _target.Length; i++)
                {
                    result += $"{_target[i]}";
                }
                return result;
            }
            set { }
        }

        public int Length
        {
            get { return _target.Length; }
            set { }
        }

        public int Bulls
        {
            get { return bullCount; }
            set { }
        }

        public int Cows
        {
            get { return cowCount; }
            set { }
        }

        public Secret(int numDigits)
        {
            Random rand = new(); //creates a random number bw 1-9 that does not repeat itself
            _target = new int[numDigits];
            int[] values = { 0, 1, 2, 3, 4, 5, 6, 7, 8,9 };
            values = values.OrderBy(i => rand.Next()).ToArray();
            bullCount = 0;
            cowCount = 0;
            for (int i = 0; i < _target.Length; i++)
            {
                _target[i] = values[i];
            }
        }

        public void CheckGuess(int[] guess)
        {
            bullCount = 0;
            cowCount = 0;
            for (int i = 0; i < _target.Length; i++)
            {
                if(_target[i] == guess[i])
                {
                    bullCount++;
                } else {
                    int[] intersect = _target.Intersect(guess).ToArray();
                    cowCount = intersect.Length;
                }
            }
        }
    }
//-------------------------------------------------------------------------------------------
    class HighScoreList //responsible for reading and writing the high score list
    {
        private int[] _scores = new int[8];

    public string HighScoreTable
        {
            get
            {
                string tableHead = "Digits : Best Score";
                string print = "";
                for (int i = 0; i < _scores.Length; i++) {
                        print += $"\n     {i+3} : {_scores[i]}";
                }
                return tableHead + print;
            }
            set { }
        }

        public HighScoreList()
        {
            Load(); //Does nothing except Load()
        }

        private void Load()
        {
            string path = @"C:\data\highscore.txt";
            if (!File.Exists(path))
                {
                    using StreamWriter writer = new(path);
                    writer.Close();
                }
            using StreamReader reader = new(path);
            int i = 0;
            try
            {
                foreach (string line in File.ReadLines(path))
                {
                    _scores[i] = Convert.ToInt32(line);
                    i++;
                }
            } catch (Exception e)
            {
                Console.WriteLine("Failed to open file: " + e.Message);
                for (int a = 0; a < _scores.Length; a++)
                {
                    _scores[a] = 1000000;
                }
            }
        }

        private void Save()
        {
            string path = @"C:\data\highscore.txt";
            using StreamWriter writer = new(path);
            try
            {
                for (int i = 0; i < _scores.Length; i++)
                {
                    writer.WriteLine(_scores[i].ToString());
                }
            } catch (Exception e)
            {
                Console.WriteLine("Failed to save file: " + e.Message);
            }
            writer.Close();
            
        }

        public void UpdateHighScoreForDigit(int digits, int numGuesses)
        {
            if (_scores[digits] == 0)
            {
                _scores[digits - 3] = numGuesses;
                Save();
            } else if (numGuesses < _scores[digits - 3])
            {
                _scores[digits - 3] = numGuesses;
                Save();
            }
        }
    }
//-------------------------------------------------------------------------------------------
    class Game //Manages the game and contains the logic
    {
        private Secret _secret;
        private HighScoreList _highScoreList;
        private bool _doCheat;

        public Game()
        {
            _secret = null;
            _doCheat = false;
            _highScoreList = new HighScoreList(); // initializes the object
        }

        public void CreateSecret()
        {
            int input = 0;
            while (input < 3 || input > 10)
            {
                try
                {
                    Console.Write("How many digits for this game (3 - 10)? "); //Will only accept an int bw 3-10
                    input = int.Parse(Console.ReadLine());
                    if (input < 3 || input > 10)
                    {
                        Console.WriteLine("WARNING: num digits is out of range. Try again.");
                    }
                    else
                    {
                        _secret = new Secret(input);
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("WARNING Not a number");
                }
            }
        }

        public void SetupCheat()
        {
            Console.Write("Do you want to cheat (Y/N)? "); //prompts user if they want to cheat
            string input = Console.ReadLine();
            Console.Write("\n");
            if (input.ToLower() == "y")
            {
                _doCheat = true; //if yes then cheat mode will be activated
            }
            else if (input.ToLower() == "n")
            {
                _doCheat = false; //if no then cheat mode will not be activated
            }
        }

        public void Play()
        {
            int count = 0;
            if (_secret == null)
            {
                throw new NullReferenceException("Field has not been initialized");
            }
            else
            {
                while (_secret.Bulls < _secret.Length)
                {
                    if (_doCheat == true)
                    {
                        Console.WriteLine($"Secret= {_secret.Target}    CHEATER!!!");
                    }
                    int i = 0;
                    int[] arr = new int[_secret.Length];
                    Console.Write("Enter guess: \n");
                    while (i < _secret.Length) 
                    {
                        Console.Write($"Position {i + 1}: ");
                        arr[i] = Int32.Parse(Console.ReadLine());
                        i++;
                    }
                    _secret.CheckGuess(arr);
                    Console.WriteLine($"Bulls: {_secret.Bulls}, Cows: {_secret.Cows}\n");
                    count++;
                }
                Console.WriteLine($"It took you ({count}) guess(es)");
                _highScoreList.UpdateHighScoreForDigit(_secret.Length, count); //updates the high score
            }
        }

        public void ShowHighScores()
        {
            Console.WriteLine("\n***************\n" +
                                "  High Scores  \n" +
                                "***************\n");
            Console.WriteLine(_highScoreList.HighScoreTable);
            Console.WriteLine("\nThanks for playing Rashaad's Bulls and Cows!\n");
        }
    }
}
