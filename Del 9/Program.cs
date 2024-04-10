using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace BattleshipGame
{
    class Program
    {
        // Variabler
        static int[,] board = new int[0, 0];
        static int[,] p1board = new int[0, 0];
        static int[,] p2board = new int[0, 0];

        static bool gameover = false;
        static bool p1W = false;
        static bool p2W = false;

        static int size;
        static int hit;
        static int attempts = 0;
        static int p1Attempts = 0;
        static int p2Attempts = 0;
        static int highscore = 1000000000;
        static int p1Highscore = 1000000000;
        static int p2Highscore = 1000000000;
        static int p1Hit;
        static int p2Hit;
        static int p1Ships;
        static int p2Ships;
        static int guessRow;
        static int guessCol;
        static int shipRow;
        static int shipCol;

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
        }  // Funktion för att välja speltyp

        static void PlaySolo()
        {
            CreateSingleBoard(); // Skapar spelplanen

            while (!gameover) // Kör spelet
            {
                ShowSingleBoard(); // Visar spelplanen

                Console.WriteLine($"\nGissa en rad (1-{size}):");
                try
                {
                    guessRow = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.WriteLine("Ingen rad vald");
                    continue;
                } // Gissar rad


                Console.WriteLine($"Gissa en kolumn (1-{size}):");
                try
                {
                    guessCol = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.WriteLine("Ingen kolumn vald");
                    continue;
                } // Gissar kolumn


                attempts++;

                if (board[guessRow, guessCol] == 1) // Om träff ändras värdet på skeppet till 2 för att visa att det är träff
                {
                    Console.Clear();
                    Console.WriteLine("Träff!");
                    board[guessRow, guessCol] = 2;
                    hit--; // Minskar antalet skepp kvar att träffa
                }
                else if (board[guessRow, guessCol] == 0) // Om miss ändras värdet på vattnet till 3 för att visa att det är miss
                {
                    Console.Clear();
                    Console.WriteLine("Miss!");
                    board[guessRow, guessCol] = 3;
                }
                else if (board[guessRow, guessCol] == 2) // Om redan träffat här
                {
                    Console.Clear();
                    Console.WriteLine("Redan träffat här!");
                }
                else if (board[guessRow, guessCol] == 3) // Om redan skjutit här
                {
                    Console.Clear();
                    Console.WriteLine("Redan skjutit här!");
                }

                if (hit == 0) // Om alla skepp är sänkta är spelet över
                {
                    int skepp = hit;
                    Console.Clear();
                    Console.WriteLine($"Du sänkte {skepp} skepp på {attempts} försök.");

                    highscore = Math.Min(highscore, attempts); // Jämför highscore med antalet försök och sparar det minsta
                    Console.WriteLine($"Din highscore är {highscore} försök");
                    
                    Console.WriteLine("Vill du spela igen? (ja/nej)");
                    string playAgain = Console.ReadLine()!; // Frågar om spelaren vill spela igen
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
        } // Spelaren splear själv

        static void CreateSingleBoard()
        {
            Console.WriteLine("Hur stor vill du att spelplanet ska vara?");
            try
            {
            size = int.Parse(Console.ReadLine()!);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Ingen storlek vald!");
                CreateSingleBoard();
            } // Frågar om storlek på spelplanen
            Console.Clear();

            if (size > 0 && size < 31)
            {
                board = new int[size, size];
            } // Spelplanen kan vara mellan 1-30
            else if (size > 30)
            {
                Console.WriteLine("För stor spelplan, försök igen.");
                CreateSingleBoard();
            } // Om spelplanen är för stor
            else
            {
                board = new int[10, 10];
            } // Om ingen storlek valts
            
            AddShips(); // Lägger till skepp
        } // skapar spelplanen för solo

        static void AddShips()
        {
            Console.WriteLine("Hur många skepp vill du ha?");
            try
            {
            hit = int.Parse(Console.ReadLine()!);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Ingen antal skepp valda!");
                AddShips();
            } // Frågar om antal skepp som splaren vill ha
            Console.Clear();

            if (hit > size * size)
            {
                Console.WriteLine("För många skepp, försök igen.");
                AddShips();
            } // ser till att det inte är för många skepp

            Random random = new Random();
            shipRow = 0;
            shipCol = 0;

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
            } // Lägger till skepp slumpmässigt
        } // Lägger till skepp

        static void ShowSingleBoard()
        {
            if (!gameover) // Kör spelet
            {
                //AdminView();

                // Skiriver ut vad varje tecken betyder med färg
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\nO.Vatten ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("X.Slaget skepp ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("#.Miss");
                Console.ForegroundColor = ConsoleColor.White;

                // Skriver ut spelplanen
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
        } // Visar spelplanen till spelaren

        static void TwoPlayer()
        {
            CreateTwoBoards(); // Skapar spelplanen för två spelare

            while (!p1W || !p2W)
            {
                Console.WriteLine("Spelare 1:");

                ShowP2Board(); // Visar spelplanen för spelare 1

                Console.WriteLine($"\nGissa en rad (1-{size}):");

                try
                {
                    guessRow = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Ingen rad vald!");
                    continue;
                } // spelare 1 gissar rad


                Console.WriteLine($"Gissa en kolumn (1-{size}):");
                try
                {
                    guessCol = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Ingen kolumn vald!.");
                    continue;
                } // spelare 1 gissar kolumn


                p1Attempts++;

                if (p2board[guessRow, guessCol] == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Träff!");
                    p2board[guessRow, guessCol] = 2;
                    p1Hit--;
                } // Om träff ändras värdet på skeppet till 2 för att visa att det är träff
                else if (p2board[guessRow, guessCol] == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Miss!");
                    p2board[guessRow, guessCol] = 3;
                } // Om miss ändras värdet på vattnet till 3 för att visa att det är miss
                else if (p2board[guessRow, guessCol] == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Redan träffat här!");
                } // Om redan träffat här
                else if (p2board[guessRow, guessCol] == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Redan skjutit här!");
                } // Om redan skjutit här
                
                if (p1Hit == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Spelare 1 sänkte {p1Ships} skepp på {p1Attempts} försök.");

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
                } // Om spelare 1 vinner

                Console.WriteLine("Spelare 2:");

                ShowP1Board(); // Visar spelplanen för spelare 2

                Console.WriteLine($"\nGissa en rad (1-{size}):");
                try
                {
                     guessRow = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Ingen rad vald!");
                    continue;
                } // spelare 2 gissar rad



                Console.WriteLine($"Gissa en kolumn (1-{size}):");
                try
                {
                     guessCol = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Ingen kolumn vald!");
                    continue;
                } // spelare 2 gissar kolumn
                

                p2Attempts++;

                if (p1board[guessRow, guessCol] == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Träff!");
                    p1board[guessRow, guessCol] = 2;
                    p2Hit--;
                } // Om träff ändras värdet på skeppet till 2 för att visa att det är träff
                else if (p1board[guessRow, guessCol] == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Miss!");
                    p1board[guessRow, guessCol] = 3;
                } // Om miss ändras värdet på vattnet till 3 för att visa att det är miss
                else if (p1board[guessRow, guessCol] == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Redan träffat här!");
                } // Om redan träffat här
                else if (p1board[guessRow, guessCol] == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Redan skjutit här!");
                } // Om redan skjutit här

                if (p2Hit == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Spelare 2 sänkte {p2Ships} skepp på {p2Attempts} försök.");

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
                } // Om spelare 2 vinner
                }                
        } // Två spelare spelar mot varandra

        static void CreateTwoBoards()
        {
            // Väljer storlek på spelplanen
            Console.WriteLine("Hur stor vill du att spelplanet ska vara?");
            try
            {
            size = int.Parse(Console.ReadLine()!);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Ingen storlek vald!");
                CreateTwoBoards();
            }
            
            Console.Clear();

            // skapar spelplanen
            if (size > 0 && size < 31)
            {
                p1board = new int[size, size];
                p2board = new int[size, size];
            }
            else if (size > 30)
            {
                Console.Clear();
                Console.WriteLine("För stor spelplan, försök igen.");
                CreateTwoBoards();
            }
            else
            {
                p1board = new int[10, 10];
                p2board = new int[10, 10];
            }

            AddTwoShips(); // lägger till skepp
        } // Skapar ett spelplan för två spelare

        static void AddTwoShips()
        {
            // väljer antal skepp
            Console.WriteLine("Hur många skepp vill ni ha?");
            try
            {
            hit = int.Parse(Console.ReadLine()!);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Ingen antal skepp valda!");
                AddTwoShips();
            }


            p1Hit = hit;
            p2Hit = hit;
            p1Ships = hit;
            p2Ships = hit;

            if (hit > size * size)
            {
                Console.Clear();
                Console.WriteLine("För många skepp, försök igen.");
                AddTwoShips();
            }
            // väjer om skeppen ska placeras slumpmässigt eller inte
            Console.WriteLine("Placera skeppen slupmässigt? (ja/nej)");
            string random = Console.ReadLine()!;
            Console.Clear();

            // Placerar skeppen
            if (Command(random, "JA"))
            {
                RandomShips();
            }
            else
            {
                P1Ships();
                P2Ships();
            }
        } // Lägger till skepp för två spelare

        static void RandomShips()
        {
            Random random = new Random();
            shipRow = 0;
            shipCol = 0;

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
        } // Lägger till skepp slumpmässigt för 2 spelare
        static void P1Ships()
        {
            int a = p1Ships;
            while (a > 0)
            {
                Console.WriteLine("Spelare 1, placera ditt skepp:");
                Console.WriteLine($"Välj en rad (1-{size}):");
                try
                {
                shipRow = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Ingen rad vald!");
                    continue;
                }

                Console.WriteLine($"Välj en kolumn (1-{size}):");
                try
                {
                shipCol = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Ingen kolumn vald!");
                    continue;
                }

                if (p1board[shipRow, shipCol] == 1)
                {
                    Console.WriteLine("Redan ett skepp här, försök igen.");
                    continue;
                }
                else
                {
                p1board[shipRow, shipCol] = 1;
                    a--;
                }
                Console.Clear();
            }
        } // Spelare 1 lägger till skepp
        static void P2Ships()
        {
            int a = p2Ships;
            while (a > 0)
            {
                Console.WriteLine("Spelare 2, placera ditt skepp:");
                Console.WriteLine($"Välj en rad (1-{size}):");
                try
                {
                shipRow = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Ingen rad vald!");
                    continue;
                }

                Console.WriteLine($"Välj en kolumn (1-{size}):");
                try
                {
                shipCol = int.Parse(Console.ReadLine()!) - 1;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Ingen kolumn vald!");
                    continue;
                }

                if (p2board[shipRow, shipCol] == 1)
                {
                    Console.WriteLine("Redan ett skepp här, försök igen.");
                    continue;
                }
                else
                {
                p2board[shipRow, shipCol] = 1;
                    a--;
                }
                Console.Clear();
            }
        } // Spelare 2 lägger till skepp

        static void ShowP1Board()
        {
            if (!gameover)
            {
                //AdminP1View();

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
        } // Visar spelplan 1
        static void ShowP2Board()
        {
            if (!gameover)
            {
                //AdminP2View();

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
        }  // Visar spelplan 2

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
        } // Visar spelplanen så man kan se skeppen
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
        }   // Visar spelplanen så man kan se spelare 1´s skepp
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
        }      // Visar spelplanen så man kan se spelare 2´s skepp

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
        } // Kollar om inmatningen är rätt
    }
}