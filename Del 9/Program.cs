using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace BattleshipGame
{
    class Program
    {


        static void Main()
        {
            // Skapa en 10x10 spelplan
            int[,] board = new int[10, 10];
            bool gameover = false;
            int attempts = 0;
            int highscore = 1000000000;
            int hit = 3;


            // Placera skepp på slumpmässiga positioner
            Random random = new Random();
            int shipRow = 0;
            int shipCol = 0;

            for (int i = 0; i < 3; i++)
            {
                shipRow = random.Next(0, 10);
                shipCol = random.Next(0, 10);
                board[shipRow, shipCol] = 1;
            }

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
            }

            Console.WriteLine(" ");

            while (!gameover)
            {
                if (!gameover)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("\n0.Water ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("2.Hit Ship ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("3.Miss");
                    Console.ForegroundColor = ConsoleColor.White;

                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine();
                        for (int j = 0; j < 10; j++)
                        {
                            if (board[i, j] == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write("0 ");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else if (board[i, j] == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write(board[i, j] + " ");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else if (board[i, j] == 2)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(board[i, j] + " ");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else if (board[i, j] == 3)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(board[i, j] + " ");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                }


                Console.WriteLine("\nGissa en rad (0-9):");
                int guessRow = int.Parse(Console.ReadLine()!);
                if (guessRow < 0 || guessRow > 9)
                {
                    Console.WriteLine("Felaktig inmatning, försök igen.");
                    continue;
                }

                Console.WriteLine("Gissa en kolumn (0-9):");
                int guessCol = int.Parse(Console.ReadLine()!);
                if (guessCol < 0 || guessCol > 9)
                {
                    Console.WriteLine("Felaktig inmatning, försök igen.");
                    continue;
                }

                attempts++;

                if (board[guessRow, guessCol] == 1)
                {
                    Console.WriteLine("Träff!");
                    board[guessRow, guessCol] = 2;
                    hit--;
                }
                else if (board[guessRow, guessCol] == 0)
                {
                    Console.WriteLine("Miss!");
                    board[guessRow, guessCol] = 3;
                }
                else if (board[guessRow, guessCol] == 2)
                {
                    Console.WriteLine("Redan träffat här!");
                }  

                if (hit == 0)
                {
                    Console.WriteLine($"\n \nDu sänkte {hit} skepp på {attempts} försök.");

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

        static void TwoPlayer()
        {

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