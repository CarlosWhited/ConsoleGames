/// Carlos Whited
/// August 29th, 2014
/// 
/// This is a version of pong that can be played from the command line.  It borrows heavily from the source code
/// of MazeGame.cs except this gets user input directly from keys pressed instead of from the input stream
/// 
/// *** Still need to fix a lot of this code

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningCSharp
{
    class PongClone
    {
        static void gameLoop()
        {
            int playerOnePos = 0;
            int playerTwoPos = 0;

            System.Console.WriteLine("Player one is at " + playerOnePos);
            System.Console.WriteLine("Player two is at " + playerTwoPos);
            showCurrentPos(playerOnePos, playerTwoPos);
            System.ConsoleKeyInfo userInput = System.Console.ReadKey(true);

            while (/* the 'x' key isn't pressed */)
            {
                userInput = System.Console.ReadKey(true);

                if (Keys.S == 0)
                {
                    playerTwoPos++;
                }
                else if (Keys.S == 0)
                {
                    playerTwoPos--;
                }

                if (Keys.I == 0)
                {
                    playerOnePos++;
                }
                else if (Keys.K == 0)
                {
                    playerOnePos--;
                }
                
                System.Console.Clear();
                System.Console.WriteLine("Player one is at " + playerOnePos);
                System.Console.WriteLine("Player two is at " + playerTwoPos);

                showCurrentPos(playerOnePos, playerTwoPos);
            }
        }

        static void showCurrentPos(int playerOnePos, int playerTwoPos)
        {
            StringBuilder normalLine = new StringBuilder("|          |          |");
            StringBuilder changedLine = new StringBuilder("|          |          |");
           
            string upperBorder = "-----------------------";
            System.Console.WriteLine(upperBorder);
            
            
            for (int i = 5; i > -6; i--)
            {
                changedLine.Clear(); 
                changedLine.Insert(0, "|          |          |");
                
                if (i == playerTwoPos || i == playerTwoPos + 1 || i == playerTwoPos - 1)
                {
                    changedLine[2] = '0';
                }
                
                if (i == playerOnePos || i == playerOnePos + 1 || i == playerOnePos - 1)
                {
                    changedLine[20] = '0';
                }
                
                System.Console.WriteLine(changedLine);
            }
            System.Console.WriteLine(upperBorder);
        }

        static int Main(string[] args)
        {
            gameLoop();
            return 0;
        }
    }
}
