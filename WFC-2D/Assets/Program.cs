using System;

namespace myApp
{
    class Program
    { 
        
        public static bool[,] grid = new bool[9,3] { 
                    {true, true,true}, 
                    {true, true,true}, 
                    {true, true,true}, 
                    {true, true,true}, 
                    {true, true,true}, 
                    {true, true,true}, 
                    {true, true,true}, 
                    {true, true,true}, 
                    {true, true,true}
                };
        public static int[,] rules = {
                //0:up 1:right
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


            
                public static int w=3;
                public static int h=3;

            static void DisplayGrid(){
            // Console.WriteLine(grid.Length);
            for(int i = 0; i<h; i++){
                Console.Write("ligne "+ i.ToString()+ " :  ");
                for(int j=0; j<w; j++){
                    int index = i*w + j;
                    for(int z=0; z<3; z++){
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
                bool[] result = {false,false,false};
                for(int i =0;i<rules.GetLength(0);i++){
                    for(int a=0;a<3;a++){
                        if (grid[x,a]){
                            // result[rules[i,1]]= check(x,voisin,d);
                            if(rules[i,0]==a && rules[i,2]==d){
                                result[rules[i,1]] = true;

                            }
                            }
                        }
                }
                for (int j =0; j<3; j++){
                    if(!result[j] && grid[voisin,j]){
                        changement = true;
                        grid[voisin,j]=false;
                        Console.Write("pop("+voisin+","+j+")");
                    }
                }
                

                return changement;

            }

            static void WFC(int x){
                Console.WriteLine("examine "+ x);
                DisplayGrid();
                // examine(x);

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


            


            // static bool examine(int x){
            //     if (x>w-1) {
            //         check(x,x-w,0);
            //     }
            //     if(x%w<h-1){
            //         check(x,x+1,1);
            //     }
            //     if (x<(h-1)*w+1){
            //         check(x,x+w,2);
            //     }
            //     if(x%w>0){
            //         check(x,x-1,3);
            //     }
            // }

            // static void check(int x,int z, int d){
            //     bool[] result = {false,false,false};
            //     for(int i =0;i<rules.GetLength(0);i++){
            //         for(int a=0;a<3;a++){
            //             if 
            //             for(int b=0;b<3;b++){
            //                 // if(rules[i]==)
            //             }
            //         }
            //     }
            // }

        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");


            // Console.Write(grid);
            // for (int i = 0; i < visited.Length; i++) { Console.Write(visited[i]); }
            // Console.WriteLine(visited[1]);
            // DisplayGrid();
                // Console.WriteLine(rules.GetLength(0));
                // Console.WriteLine(rules[1,]);
           
            // DisplayGrid();

            //on choisit 0
            grid[0,1]= false;
            grid[0,2]= false;
            WFC(0);

            DisplayGrid();

while(true){
            // int[] entropy = new int[9] {
            //     0,0,0,0,0,0,0,0,0
            // };
            int mins= int.MaxValue;
            int index=0;
            bool flagcontinuer = false;
            for(int i=0; i<9;i++){
                int s =0;
                for(int j=0; j<3; j++){
                    if(grid[i,j]){
                        s+=1;
                    }
                }
                if(s>1){
                    // entropy[i]=s;
                    if(s<mins){
                        mins = s;
                        index = i;
                        flagcontinuer = true;
                    }
                }
            }
            if(!flagcontinuer){ break;}
            Console.WriteLine("nouveau WFC sur "+index);
            Random aleatoire = new Random();
            int a = aleatoire.Next(mins);
            for(int i =0; i<3; i++){
                if(grid[index,i]){
                    if(a !=0){
                        grid[index,i]=false;
                }
                a--;
                }
            }
            WFC(index);
}



            }
}
    }




        


