/// Carlos Whited
/// August 28th, 2014
/// 
/// This is a simple game I made to try to learn the ropes in C# and try to 
/// implement some game design patterns that I've read about.  Ultimately, 
/// I would want this to run on the command console and generate a maze that 
/// players have to solve under a given time limit.  All the code that generates
/// and solves the maze was created for an assignment in a computer science class
/// that I took in college.  

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningCSharp
{
    public class Game
    {
        static void gameLoop()
        {
            int xPos = 0;
            int yPos = 0;

            System.Console.WriteLine("You are now at (" + xPos + ", " + yPos + ")");
            showCurrentPos(xPos + 11, yPos);
            System.ConsoleKeyInfo userInput = System.Console.ReadKey(true);

            while (userInput.KeyChar != 'x')
            {
                userInput = System.Console.ReadKey(true);
                if (userInput.KeyChar == 'w' && yPos < 5)
                    yPos++; 
                else if (userInput.KeyChar == 's' && yPos > -5)
                    yPos--;
                else if (userInput.KeyChar == 'd' && xPos < 10)
                    xPos++;
                else if (userInput.KeyChar == 'a' && xPos > -10)
                    xPos--;
                
                /*if ((userInput.Key & ConsoleKey.UpArrow) == 0 && yPos < 5)
                    yPos++;
                else if ((userInput.Key & ConsoleKey.DownArrow) == 0 && yPos > -5)
                    yPos--;
                else if ((userInput.Key & ConsoleKey.RightArrow) == 0 && xPos < 10)
                    xPos++;
                else if ((userInput.Key & ConsoleKey.LeftArrow) == 0 && xPos > -10)
                    xPos--;
                */
                System.Console.Clear();
                System.Console.WriteLine("You are now at (" + xPos + ", " + yPos + ")");

                showCurrentPos(xPos+11, yPos);
            }
        }

        static void showCurrentPos(int xPos, int yPos)
        {
            StringBuilder normalLine = new StringBuilder("|                     |");
            StringBuilder changedLine = new StringBuilder("|                     |");
            string upperBorder = "-----------------------";
            System.Console.WriteLine(upperBorder);
            for (int i = 5; i > -6; i--)
            {
                if (i == yPos)
                {
                    changedLine[xPos] = '0';
                    System.Console.WriteLine(changedLine);
                }
                else
                    System.Console.WriteLine(normalLine);
            }
            System.Console.WriteLine(upperBorder);
        }

        static void makeMaze(Maze gameMaze)
        {
            gameMaze.makeGraph(11, 24);
            disjointSet ds = new disjointSet(11*24);
            gameMaze.mazeCreate(ds);
            gameMaze.output();
        }
        static int Main(string[] args)
        {
            //Maze gameMaze = new Maze();
            //makeMaze(gameMaze);
            gameLoop();
            return 0;
        }
    }

    public class cell
    {
        public cell()
        {
            for (int i = 0; i < 4; i++)
            {
                neighbor[i] = -1;
                wall[i] = false;
            }
        }

        public int[] neighbor = new int[4];
        public bool[] wall = new bool[4];
    }

    public class Maze
    {
        public Maze() { }

        public void makeGraph(int rows, int cols) 
        {
            cell e = new cell();
            for (int i = 0; i < rows*cols; i++)
                grid.Add(e);

            for (int i = 0; i < grid.Count; i++) 
            {
                if(i % cols != 0) 			
			        grid[i].neighbor[0] = i - 1;
		        else						
		        	grid[i].neighbor[0] = -1;

		        // Case for if there isn't a cell on top
 		        if((i - cols)  > 0) 		
 			        grid[i].neighbor[1] = i - cols;
 	        	else						
 		        	grid[i].neighbor[1] = -1;

 		        // Case for if there isn't a cell to the right
 		        if((i + 1) % cols != 0)		
 		        	grid[i].neighbor[2] = i + 1;
 	        	else						
 	        		grid[i].neighbor[2] = -1;

 	        	// Case for if there isn't a cell below it
 	        	if((i + cols) < rows*cols)	
 		        	grid[i].neighbor[3] = i + cols;
 		        else						
 		        	grid[i].neighbor[3] = -1;
	        }
        }
       
        public void mazeCreate(disjointSet ds)
        {
            int randCell = new int(), randNeighbor = new int(), adjNeighbor = new int();
            randCell = 0; 
            randNeighbor = 0; 
            adjNeighbor = 0;
            Random rnd = new Random();

            while (ds.size() != 1) 
            {
                randCell = rnd.Next()%grid.Count;
		        while (true) 
                {
			        randNeighbor = rnd.Next()%4;
			        if (grid[randCell].neighbor[randNeighbor] >= 0)
			        	break;
		        }
		
		        adjNeighbor = grid[randCell].neighbor[randNeighbor];

	        	// Merges adjacent cells if they aren't already in the same set
	        	if (ds.find(randCell) != ds.find(adjNeighbor)) 
                {
		        	ds.merge(randCell, adjNeighbor);
	        		breakWall(randCell, randNeighbor);
	        	}
	        }
        }

        public void mazeCreate(List<int> cellIndex, List<int> wallPosition)
        {
            for (int i = 0; i < cellIndex.Count; i++) 
            {
	        	for (int j = 0; j < 4; j++) 
                {
		        	if (grid[cellIndex[i]].neighbor[j] == wallPosition[i]) { 
			        	makeWall(cellIndex[i], j);
			        }
		        }
	        }
        }

        public void output()
        {
            for (int i = 0; i < grid.Count; i++) 
            {
		        for (int j = 0; j < 4; j++) 
                {
			        if (grid[i].wall[j] && grid[i].neighbor[j] > i)
                        System.Console.WriteLine(i + " " + grid[i].neighbor[j]);
			        	//cout << left << i << " " << grid[i].neighbor[j] << endl;
		        }
	        }
        }

        public void solve(int Nrows, int Ncols)
        {
            vcolor.Capacity = grid.Count;
	        vcolor.Capacity = grid.Count;

            for (int i = 0; i < vcolor.Count; i++)
                vcolor[i] = cellColor.WHITE;

	        // If depth first search returns a path, then the path is outputted to the console
	        if (dfs(0, grid.Count-1)) 
            { 
	        	//cout << "PATH" << " " << Nrows << " " << Ncols << endl;
                System.Console.WriteLine("PATH " + Nrows + " " + Ncols);

	        	for (int i = 0; i < vorder.Count; i++) {
	        		//cout << vorder[i] << endl;
                    System.Console.WriteLine(vorder[i]);
	        	}
	        }
        }

        private void breakWall(int randCell, int randNeighbor)
        {
            int adjNeighbor = 0;
	
	        adjNeighbor = grid[randCell].neighbor[randNeighbor];
	
        	// 4 cases that are dependent on location of the random neighbor
        	if (randNeighbor == 0) {
        		grid[randCell].wall[randNeighbor] = false;
        		grid[adjNeighbor].wall[2] = false;
        		return;
        	}
        	else if (randNeighbor == 1) {
        		grid[randCell].wall[randNeighbor] = false;
        		grid[adjNeighbor].wall[3] = false;
        		return;
        	}
        	else if (randNeighbor == 2) {
        		grid[randCell].wall[randNeighbor] = false;
        		grid[adjNeighbor].wall[0] = false;
        		return;
        	}
        	else if (randNeighbor == 3) {
        		grid[randCell].wall[randNeighbor] = false;
        		grid[adjNeighbor].wall[1] = false;
        		return;
        	}
        }	

        private void makeWall(int cell, int wall)
        {
            int adjNeighbor = 0;
	
        	adjNeighbor = grid[cell].neighbor[wall];
	
        	// 4 cases that are dependent on location of the random neighbor
        	if (wall == 0) {
        		grid[cell].wall[wall] = true;
        		grid[adjNeighbor].wall[2] = true;
        		return;
        	}
	        else if (wall == 1) {
	        	grid[cell].wall[wall] = true;
	        	grid[adjNeighbor].wall[3] = true;
	        	return;
	        }
	        else if (wall == 2) {
	        	grid[cell].wall[wall] = true;
	        	grid[adjNeighbor].wall[0] = true;
        		return;
        	}
        	else if (wall == 3) {
        		grid[cell].wall[wall] = true;
        		grid[adjNeighbor].wall[1] = true;
        		return;
        	}
        }

        private bool dfs(int i, int sink) 
        {
            // Sets vertex to gray and adds it to the path
            vcolor[i] = cellColor.GRAY;
            vorder.Add(i);

            // Breaks out of recursion if you're at the sink
            if (i == sink)
                return true;

            // Loop to check all neighbors for next vertex to visit
            for (int k = 0; k < 4; k++)
            {
                int j = grid[i].neighbor[k];

                // Recursive call that continues the search
                if (!grid[i].wall[k] && j > 0 && vcolor[j] == cellColor.WHITE && dfs(j, sink))
                {
                    return true;
                }
            }

            // Deletes entries if dead ends are reached, sets the vertices to black, and returns false
            vorder.RemoveAt(vorder.Count);
            vcolor[i] = cellColor.BLACK;
            return false;
        }

        private List<cell> grid = new List<cell>();
        private enum cellColor { WHITE, BLACK, GRAY };
        private List<cellColor> vcolor = new List<cellColor>();
        private List<int> vorder = new List<int>();
    }

    public class disjointSet
    {
        private class element
        {
            public element()
            {
                rank = new int(); rank = 0;
                parent = new int(); parent = 0;
            }
            public int rank;
            public int parent;
        }

        public disjointSet(int N = 0)
        {
            if (N > 0)
            {
                element e = new element();
                for (int i = 0; i < N; i++) 
                    S.Add(e);
            }
            Nsets = N;   
        }

        public int size() { return Nsets; }

        public int addNew()
        {
            element e = new element();
            S.Add(e);
            Nsets++;
            return S.Count - 1;
        }

        public int merge(int i, int j) 
        {
            i = find(i);
            j = find(j);

            if (i != j) 
            {
                element Si = new element();
                Si = S[i];
                element Sj = new element();
                Sj = S[j];

                if (Si.rank > Sj.rank)
                    Sj.parent = i;
                else if (Si.rank < Sj.rank)
                    Si.parent = j;
                else 
                { 
                    Sj.parent = i;
                    Si.rank++;
                }

                Nsets--;
            }

            return find(i);
        }
        
        public int find(int i)
        {
            if (S[i].parent == -1)
                return i;

            S[i].parent = find(S[i].parent);
            return S[i].parent;
        }

        private int Nsets;
        private List<element> S = new List<element>();
    }
}


