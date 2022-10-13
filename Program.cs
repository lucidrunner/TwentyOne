namespace TwentyOne;

internal class Program
{
    static void Main(string[] args)
    {
        //Personal preference to put it into its own class since I run it as a static anyway
        Casino.PlayTwentyOne();
    }

    public class Casino
    {
        public static void PlayTwentyOne()
        {
            //Welcome message
            Console.WriteLine("Välkommen till 21:an!");

            //Define our starting variables
            int playerChoice = 0;
            string latestWinner = "";

            
            while (playerChoice != 4)
            {
                //Play menu inside our while so we don't have to run it twice
                PrintPlayMenu();
                //I really like this convention for checking / converting console input to numbers
                //?? nullchecks the input (which happens if we just hit enter) and assigns a default value to it
                int.TryParse(Console.ReadLine() ?? "0", out playerChoice);
                switch (playerChoice)
                {
                    case 1:
                        //Returning something other than "" means we have a new latest winner
                        string winner = RunTwentyOne();
                        if (winner.Length > 0)
                            latestWinner = winner;
                        break;
                    case 2:
                        //Conditional operator - ?, if bool evaluates to true do first, otherwise do what's after :
                        Console.WriteLine(latestWinner.Length > 0 ? $"Senaste vinnaren är {latestWinner}!" : "Det finns ingen tidigare vinnare.");
                        break;
                    case 3:
                        PrintRules();
                        break;
                    //Do nothing and exit the loop
                    case 4: break;
                    default:
                        Console.WriteLine("Felaktig input.");
                        break;
                }
            }

            Console.WriteLine("Avslutar programmet...");
        }

        private static string RunTwentyOne()
        {
            const int startingCards = 2;

            //Create our randomizer and set up our starting values
            Random rand = new();
            int playerPoints = 0, computerPoints = 0;
            string cardChoice = "j";

            Console.WriteLine();
            Console.WriteLine("Nu kommer två kort dras per spelare.");
            //1 more line than just doing 4 += so let's pretend this futureproofs the program for if we ever want to start with 8 cards
            for (int i = 0; i < startingCards; i++)
            {
                playerPoints += rand.Next(1, 11);
                computerPoints += rand.Next(1, 11);
            }


            while (cardChoice != "n" && playerPoints <= 21)
            {
                //Local method since we'll have to do this for the computer later too
                PrintPoints();
                do
                {
                    Console.WriteLine("Vill du ha ett kort till? (j/n)");
                    //Default to invalid value if it's null
                    cardChoice = Console.ReadLine() ?? "x";
                    //Doing a switch here because it's in the pseduocode, however an if(j) else if(!n) would be cleaner since we ignore "n" anyway
                    switch (cardChoice.ToLower())
                    {
                        case "j":
                            int cardValue = rand.Next(1, 11);
                            Console.WriteLine($"Ditt nya kort är värt {cardValue} poäng.");
                            playerPoints += cardValue;
                            break;
                        case "n": break;
                        default:
                            Console.WriteLine("Felaktigt input.");
                            break;
                    }
                    //ToLower is a good way to avoid double checks for N/n
                } while (cardChoice.ToLower() != "n" && cardChoice.ToLower() != "j");
            }

            //Check if player has already lost
            if (playerPoints > 21 || playerPoints < computerPoints)
            {
                PrintPoints();
                Console.WriteLine("Du förlorade!");
                return "";
            }

            while (computerPoints < 21 && computerPoints < playerPoints)
            {
                int cardValue = rand.Next(1, 11);
                Console.WriteLine($"Datorn drog ett kort värt {cardValue} poäng.");
                computerPoints += cardValue;
                PrintPoints();
                //Easier to visualize the computer with a gap between rounds
                Thread.Sleep(800);
            }

            //Since we previously checked if the player has already finished below the computer / above 21 we now only need to check
            //if the computer has gone over 21 or if the player has finished above the computer (since tie is a loss too)
            if (computerPoints > 21 || playerPoints > computerPoints)
            {
                Console.WriteLine("Du vann! Skriv in ditt namn.");
                return Console.ReadLine() ?? "Okänd Spelare";
            }
            
            //Return nothing if the player lost
            return "";

            void PrintPoints()
            {
                Console.WriteLine($"Din poäng: {playerPoints}");
                Console.WriteLine($"Datorns poäng: {computerPoints}");
                Console.WriteLine();
            }
        }

        public static void PrintPlayMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Välj ett alternativ");
            Console.WriteLine("1. Spela 21:an");
            Console.WriteLine("2. Visa senaste vinnaren");
            Console.WriteLine("3. Spelets regler");
            Console.WriteLine("4. Avsluta programmet");
        }

        private static void PrintRules()
        {
            Console.WriteLine();
            Console.WriteLine("Få högre än datorn men inte högre än 21, yadda yadda, två kort första rundan men ett efter det, om du går över 21 eller lika med datorn förlorar du förresten.");
        }
    }
}