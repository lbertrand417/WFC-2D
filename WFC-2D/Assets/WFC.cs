using System;
using System.Collections.Generic;

namespace myApp
{
    class Program
    { 
                public static int w=4;
                public static int h=4;
                public static int numberTiles =4;

    private static bool[,] grid;
        
        public static int[,] rules = {
                //0:up 1:right 2: down 3: left

                    {1,2,0},
                    {2,1,2},
                    {1,0,1},
                    {0,1,3},
                    {1,0,0},
                    {0,1,2},
                    {1,2,1},
                    {2,1,3},
                    {2,0,1},
                    {0,2,3},
                    {0,1,1},
                    {1,0,3},
                    {0,0,1},
                    {0,0,3},
                    {2,1,1},
                    {1,2,3},
                    {2,2,1},
                    {2,2,3},
                    {0,1,0},
                    {1,0,2},
                    {1,2,2},
                    {2,1,0}

            };


            

            static void DisplayGrid(){
                //affiche la grille -> ne pas appeler dans unity a priori
            for(int i = 0; i<h; i++){
                Console.Write("ligne "+ i.ToString()+ " :  ");
                for(int j=0; j<w; j++){
                    int index = i*w + j;
                    for(int z=0; z<numberTiles; z++){
                    if(z!=0) Console.Write(",");
                    Console.Write(grid[index,z]);
                    }
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
            }
            static bool sparse(int x, int voisin, int d){
                bool changement = false;
                bool[] result = new bool[numberTiles];
                for(int i =0;i<rules.GetLength(0);i++){
                    for(int a=0;a<numberTiles;a++){
                        if (grid[x,a]){
                            //IMPORTANT result[rules[i,1]]= check(x,voisin,d);
                            if(rules[i,0]==a && rules[i,2]==d){
                                result[rules[i,1]] = true;

                            }
                            }
                        }
                }
                for (int j =0; j<numberTiles; j++){
                    if(!result[j] && grid[voisin,j]){
                        changement = true;
                        grid[voisin,j]=false;
                        Console.Write("pop("+voisin+","+j+")");
                    }
                }
                return changement;

            }

            static void WFC(int x){
                //nouvelle itération de WFC
                Console.WriteLine("examine "+ x); 
                // DisplayGrid() qui peut être mis en commentaire
                DisplayGrid();
                //conditions sur les bords 
                //IMPORTANT -> changer les directions
                if (x>w-1) {
                    if(sparse(x,x-w,0)){
                    WFC(x-w);
                    }
                }
                if(x%w<h-1){
                    if(sparse(x,x+1,1)){
                    WFC(x+1);
                    }
                }
                if (x<(h-1)*w){
                    if(sparse(x,x+w,2)){
                    WFC(x+w);
                    }
                }
                if(x%w>0){
                    if(sparse(x,x-1,3)){
                    WFC(x-1);
                    }
                }
                return;

                }
            

        

        static void Main(string[] args)
        {

        grid = new bool[w * h, numberTiles]; 
        for (int i = 0; i < w * h; i++){
            for (int j =  0; j < numberTiles; j++){
                grid[i, j] = true;
            }
        }


 
while(true){
            List<int> entropy = new List<int>();
            int mins= int.MaxValue;
            bool flagcontinuer = false;
            for(int i=0; i<w*h;i++){
                int s =0;
                for(int j=0; j<numberTiles; j++){
                    if(grid[i,j]){
                        s+=1;
                    }
                }
                if(s==0){
                    Console.WriteLine("aie une des cases n'a plus aucune possibilité");
                    // IMPORTANT il faudrait recommencer -> rappeler Run()
                    break;
                }
                if(s>1){
                    if (s==mins){ // i est aussi d'entropie minimale -> on l'ajoute
                        entropy.Add(i);
                    }
                    if(s<mins){ // i a une entropy strictement plus petite -> on remplace entropy
                        entropy.Clear();
                        entropy.Add(i);
                        mins = s;
                        flagcontinuer = true;
                    }
                }
            }
            if(!flagcontinuer){ break;} //si la case x ne change rien sur la case voisin, on ne continue pas
            Random aleatoire = new Random();
            // b case random avec la plus petite entropie
            int b = aleatoire.Next(entropy.Count);
            int ind= entropy[b];
            // tuile a choisi random parmis les 'true' de la case b
            int a = aleatoire.Next(mins);
            for(int i =0; i<numberTiles; i++){
                if(grid[ind,i]){
                    if(a !=0){
                        grid[ind,i]=false;
                }
                a--;
                }
            }
            // Console.WriteLine("nouveau WFC sur "+ ind);
            WFC(ind);
            }
    }



}
}




        


