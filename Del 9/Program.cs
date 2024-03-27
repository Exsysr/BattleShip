using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace BattleshipGame
{
    class Program
    {
        static int[,] board = new int[0, 0];
        static int size;
        static bool gameover = false;
        static int attempts = 0;
        static int highscore = 1000000000;
        static int hit;
        static bool p1W = false;
        static int[,] p1board = new int[0, 0];
        static bool p2W = false;
        static int[,] p2board = new int[0, 0];
        static int p1Attempts = 0;
        static int p2Attempts = 0;
        static int p1Hit;
        static int p2Hit;
        static int p1Highscore = 1000000000;
        static int p2Highscore = 1000000000;
        static int p1Ships;
        static int p2Ships;

        static void Main()
        {
            Console.Clear();
            Console.WriteLine("Välkommen till Sänka Skepp!");
            Console.WriteLine("Välj ett alternativ:");
            Console.WriteLine("1. Spela solo");
            Console.WriteLine("2. Två spelare");
            Console.WriteLine("3. Avsluta");

            string choice = Console.ReadLine()!;
            Console.Clear();

            if (Command(choice, "Spela solo") || choice == "1")
            {
                PlaySolo();
            }
            else if (Command(choice, "Två spelare") || choice == "2")
            {
                TwoPlayer();
            }
            else if (Command(choice, "Avsluta") || choice == "3")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Felaktig inmatning, försök igen.");
                Main();
            }
        }

        static void PlaySolo()
        {
            CreateSingleBoard();

            while (!gameover)
            {
                ShowSingleBoard();

                Console.WriteLine($"\nGissa en rad (1-{size}):");
                int guessRow = int.Parse(Console.ReadLine()!) - 1;
                if (guessRow < 0 || guessRow > 9)
                {
                    Console.WriteLine("Felaktig inmatning, försök igen.");
                    continue;
                }

                Console.WriteLine($"Gissa en kolumn (1-{size}):");
                int guessCol = int.Parse(Console.ReadLine()!) - 1;
                if (guessCol < 0 || guessCol > 9)
                {
                    Console.WriteLine("Felaktig inmatning, försök igen.");
                    continue;
                }

                attempts++;

                if (board[guessRow, guessCol] == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Träff!");
                    board[guessRow, guessCol] = 2;
                    hit--;
                }
                else if (board[guessRow, guessCol] == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Miss!");
                    board[guessRow, guessCol] = 3;
                }
                else if (board[guessRow, guessCol] == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Redan träffat här!");
                }
                else if (board[guessRow, guessCol] == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Redan skjutit här!");
                }

                if (hit == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Du sänkte {hit} skepp på {attempts} försök.");

                    highscore = Math.Min(highscore, attempts);

                    Console.WriteLine($"Din highscore är {highscore} försök");
                    Console.WriteLine("Vill du spela igen? (ja/nej)");
                    string playAgain = Console.ReadLine()!;
                    if (Command(playAgain, "Ja"))
                    {
                        Main();
                    }
                    else
                    {
                        Console.ReadLine();
                    }

                    gameover = true;
                }
            }
        }

        static void CreateSingleBoard() // skapar ett spelplan
        {
            Console.WriteLine("Hur stor vill du att spelplanet ska vara?");
            size = int.Parse(Console.ReadLine()!);
            Console.Clear();
            if (size > 0 && size < 31)
            {
                board = new int[size, size];
            }
            else if (size > 30)
            {
                Console.WriteLine("För stor spelplan, försök igen.");
                CreateSingleBoard();
            }
            else
            {
                board = new int[10, 10];
            }
            
            AddShips();
        }

        static void AddShips()
        {
            Console.WriteLine("Hur många skepp vill du ha?");
            hit = int.Parse(Console.ReadLine()!);
            Console.Clear();

            if (hit > size * size)
            {
                Console.WriteLine("För många skepp, försök igen.");
                AddShips();
            }

            Random random = new Random();
            int shipRow = 0;
            int shipCol = 0;

            for (int i = 0; i < hit; i++)
            {
                shipRow = random.Next(0, size);
                shipCol = random.Next(0, size);

                if (board[shipRow, shipCol] == 1)
                {
                    i--;
                }
                else
                {
                    board[shipRow, shipCol] = 1;
                }

                board[shipRow, shipCol] = 1;
            }
        }

        static void ShowSingleBoard()
        {
            if (!gameover)
            {
                //AdminView();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n0.Vatten ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("2.Slaget skepp ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("3.Miss");
                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < size; i++)
                {
                    Console.WriteLine();
                    for (int j = 0; j < size; j++)
                    {
                        if (board[i, j] == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("O ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (board[i, j] == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("O ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (board[i, j] == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("X ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (board[i, j] == 3)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("# ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
            }
        }

        static void TwoPlayer()
        {
            CreateTwoBoards();

            while (p1W || p2W)
            {
                Console.WriteLine("Spelare 1:");

                ShowP2Board();

                Console.WriteLine($"\nGissa en rad (1-{size}):");
                int guessRow1 = int.Parse(Console.ReadLine()!) - 1;
                if (guessRow1 < 0 || guessRow1 > 9)
                {
                    Console.WriteLine("Felaktig inmatning, försök igen.");
                    continue;
                }

                Console.WriteLine($"Gissa en kolumn (1-{size}):");
                int guessCol1 = int.Parse(Console.ReadLine()!) - 1;
                if (guessCol1 < 0 || guessCol1 > 9)
                {
                    Console.WriteLine("Felaktig inmatning, försök igen.");
                    continue;
                }

                p1Attempts++;

                if (p2board[guessRow1, guessCol1] == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Träff!");
                    p2board[guessRow1, guessCol1] = 2;
                    p1Hit--;
                }
                else if (p2board[guessRow1, guessCol1] == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Miss!");
                    p2board[guessRow1, guessCol1] = 3;
                }
                else if (p2board[guessRow1, guessCol1] == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Redan träffat här!");
                }
                else if (p2board[guessRow1, guessCol1] == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Redan skjutit här!");
                }

                Console.WriteLine("Spelare 2:");

                ShowP1Board();

                Console.WriteLine($"\nGissa en rad (1-{size}):");
                int guessRow2 = int.Parse(Console.ReadLine()!) - 1;
                if (guessRow2 < 0 || guessRow2 > 9)
                {
                    Console.WriteLine("Felaktig inmatning, försök igen.");
                    continue;
                }

                Console.WriteLine($"Gissa en kolumn (1-{size}):");
                int guessCol2 = int.Parse(Console.ReadLine()!) - 1;
                if (guessCol2 < 0 || guessCol2 > 9)
                {
                    Console.WriteLine("Felaktig inmatning, försök igen.");
                    continue;
                }
                
                p2Attempts++;

                if (p1board[guessRow2, guessCol2] == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Träff!");
                    p1board[guessRow2, guessCol2] = 2;
                    p2Hit--;
                }
                else if (p1board[guessRow2, guessCol2] == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Miss!");
                    p1board[guessRow2, guessCol2] = 3;
                }
                else if (p1board[guessRow2, guessCol2] == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Redan träffat här!");
                }
                else if (p1board[guessRow2, guessCol2] == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Redan skjutit här!");
                }

                if (p1Hit == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Spelare 1 sänkte {p1Hit} skepp på {p1Attempts} försök.");

                    p1Highscore = Math.Min(p1Highscore, p1Attempts);

                    Console.WriteLine($"Din highscore är {p1Highscore} försök");
                    Console.WriteLine("Vill du spela igen? (ja/nej)");
                    string playAgain = Console.ReadLine()!;
                    if (Command(playAgain, "Ja"))
                    {
                        Main();
                    }
                    else
                    {
                        Console.ReadLine();
                    }

                    p1W = true;
                }
                if (p2Hit == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Spelare 2 sänkte {p2Hit} skepp på {p2Attempts} försök.");

                    p2Highscore = Math.Min(p2Highscore, p2Attempts);

                    Console.WriteLine($"Din highscore är {p2Highscore} försök");
                    Console.WriteLine("Vill du spela igen? (ja/nej)");
                    string playAgain = Console.ReadLine()!;
                    if (Command(playAgain, "Ja"))
                    {
                        Main();
                    }
                    else
                    {
                        Console.ReadLine();
                    }

                    p2W = true;
                }                
            }
        }

        static void CreateTwoBoards()
        {
            Console.WriteLine("Hur stor vill du att spelplanet ska vara?");
            size = int.Parse(Console.ReadLine()!);
            Console.Clear();

            if (size > 0 && size < 31)
            {
                p1board = new int[size, size];
                p2board = new int[size, size];
            }
            else if (size > 30)
            {
                Console.WriteLine("För stor spelplan, försök igen.");
                CreateTwoBoards();
            }
            else
            {
                p1board = new int[10, 10];
                p2board = new int[10, 10];
            }

            AddTwoShips();
        }

        static void AddTwoShips()
        {
            Console.WriteLine("Hur många skepp vill ni ha?");
            hit = int.Parse(Console.ReadLine()!);

            p1Hit = hit;
            p2Hit = hit;
            p1Ships = hit;
            p2Ships = hit;

            if (hit > size * size)
            {
                Console.WriteLine("För många skepp, försök igen.");
                AddTwoShips();
            }

            Console.WriteLine("Placera skeppen slupmässigt? (ja/nej)");
            string random = Console.ReadLine()!;
            Console.Clear();

            if (Command(random, "JA"))
            {
                RandomShips();
            }
            else
            {
                P1Ships();
                P2Ships();
            }
        }

        static void RandomShips()
        {
            Random random = new Random();
            int shipRow = 0;
            int shipCol = 0;

            for (int i = 0; i < hit; i++)
            {
                shipRow = random.Next(0, size);
                shipCol = random.Next(0, size);
                p1board[shipRow, shipCol] = 1;
            }

            for (int i = 0; i < hit; i++)
            {
                shipRow = random.Next(0, size);
                shipCol = random.Next(0, size);
                p2board[shipRow, shipCol] = 1;
            }
        }
        static void P1Ships()
        {
            for (int i = 0; i < p1Ships; i++)
            {
                Console.WriteLine("Spelare 1, var vill du placera ditt skepp?");

                Console.WriteLine("Välj en rad (1-10):");
                int shipRow = int.Parse(Console.ReadLine()!) - 1;
                if (shipRow > size)
                {
                    Console.WriteLine("Spelplanet är för liten!");
                    P1Ships();
                }

                Console.WriteLine("Välj en kolumn (1-10):");
                int shipCol = int.Parse(Console.ReadLine()!) - 1;
                if (shipCol > size)
                {
                    Console.WriteLine("Spelplanet är för liten!");
                    P1Ships();
                }

                p1board[shipRow, shipCol] = 1;
                p1Ships--;
                Console.Clear();
            }
        }
        static void P2Ships()
        {
            for (int i = 0;i < p2Ships; i++)
            {
                Console.WriteLine("Spelare 2, var vill du placera ditt skepp?");

                Console.WriteLine("Välj en rad (1-10):");
                int shipRow = int.Parse(Console.ReadLine()!) - 1;
                if (shipRow > size)
                {
                    Console.WriteLine("Spelplanet är för liten!");
                    P2Ships();
                }

                Console.WriteLine("Välj en kolumn (1-10):");
                int shipCol = int.Parse(Console.ReadLine()!) - 1;
                if (shipCol > size)
                {
                    Console.WriteLine("Spelplanet är för liten!");
                    P2Ships();
                }

                p2board[shipRow, shipCol] = 1;
                p2Ships--;
                Console.Clear();
            }
        }

        static void ShowP1Board()
        {
            if (!gameover)
            {
                //AdminP1View();

                Console.WriteLine("");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n0.Vatten ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("2.Slaget skepp ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("3.Miss");
                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < size; i++)
                {
                    Console.WriteLine();
                    for (int j = 0; j < size; j++)
                    {
                        if (p1board[i, j] == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("O ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (p1board[i, j] == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("O ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (p1board[i, j] == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("X ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (p1board[i, j] == 3)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("# ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
            }
        }
        static void ShowP2Board()
        {
            if (!gameover)
            {
                //AdminP2View();

                Console.WriteLine("");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n0.Vatten ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("2.Slaget skepp ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("3.Miss");
                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < size; i++)
                {
                    Console.WriteLine();
                    for (int j = 0; j < size; j++)
                    {
                        if (p2board[i, j] == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("O ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (p2board[i, j] == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("O ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (p2board[i, j] == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("X ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (p2board[i, j] == 3)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("# ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
            }
        }

        static void AdminView()
        {
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < size; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
            }
            Console.WriteLine("");
        }
        static void AdminP1View()
        {
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < size; j++)
                {
                    Console.Write(p1board[i, j] + " ");
                }
            }
            Console.WriteLine("");
        }
        static void AdminP2View()
        {
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < size; j++)
                {
                    Console.Write(p2board[i, j] + " ");
                }
            }
            Console.WriteLine("");
        }

        static bool Command(string s, string c)
        {
            bool valid = false;
            if (c[0..s.Length].ToLower() == s.ToLower())
            {
                valid = true;
            }
            else if (s is null)
            {
                valid = false;
            }
            return valid;
        }
    }
}