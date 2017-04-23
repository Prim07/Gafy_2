using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gafy_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private AdjacencyMatrix adjacencyMatrix;



        public MainWindow()
        {
            InitializeComponent();
        }

        //Tutaj przekazujemy ciąg znaków i sprawdzamy czy jest on graficzny
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Sequence.Text != "0")
            {
                Sequence.Background = Brushes.White;
                string FromTheBox = Sequence.Text;
                Sequence.Text = "";
                string[] SeparetedFromTheBox = FromTheBox.Split(',');
                int[] TabOfInt= Array.ConvertAll(SeparetedFromTheBox, i => int.Parse(i));
                Array.Sort(TabOfInt);
                //W tym miejscu mamy juz posortowana tablice intów
                //Ta zmienna mówi nam czy ciąg jest grafuiczny czy nie jest
                bool TrueOrFalse = false;
                //znacznik konca petli
                bool EndLoop = false;
                //idneks ostatniego elementu tablicy
                int max = TabOfInt.Length-1;
                
                //tu się dzieja rzeczy
                while (EndLoop == false)
                {
                    //bierzemy ostani element i sprawdzamy czy nie jest on za duzy jak np. w ciagu 1,2,8 
                    //bo wtedy nie mozemy go odjac i z miejsca taki ciag nie jest graficzny
                    Array.Sort(TabOfInt);
                    int value = TabOfInt[max];
                    if (value > max)
                    {
                        TrueOrFalse = false;
                        EndLoop = true;
                    }
                    //jesli sie zgadza wszytko to po kolei odejmujemy i ustawiamy value na 0
                    else
                    {
                        for (int i = max - 1; i >= max - value; i--)
                        {
                            TabOfInt[i]--;
                        }
                        TabOfInt[max] = 0;
                        //sortowanie
                        Array.Sort(TabOfInt);
                        //jesli ostatnia, czyli najwieksza liczba, to 0, to znaczy ze jest koniec
                        //tutaj zaznaczamy EndLoop na true (konczymy petle)
                        //zakladamy tez ze mamy same 0 - wtedy ciag jest graficzny - czyli TrueOrFalse = true
                        //potem w petli for sprawdzamy czy nie mamy jakiego (-1) - ciag nie jest graficzny
                        if (TabOfInt[max] == 0)
                        {
                            EndLoop = true;
                            TrueOrFalse = true;
                            for (int i = 0; i <= max; i++)
                            {
                                if (TabOfInt[i] < 0) TrueOrFalse = false;
                            }
                        }
                    }
                    
                }

                //wyswietlamy odpowiedni komunikat w miejscu, gdzie byl ciag
                string OutPut;
                if (TrueOrFalse == true)
                    OutPut = "Tak";
                else
                    OutPut = "Nie";
                Sequence.Text = OutPut;
            }
            else
            {
                Sequence.Background = Brushes.OrangeRed;
            }
        }




        //Tutaj rysujemy graf losowy o zadanych stopniach wierzcholkow
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            //prawie to samo co wyżej
            if (Dagrees.Text != "0")
            {
                Dagrees.Background = Brushes.White;
                string FromTheBox = Dagrees.Text;
                string[] SeparetedFromTheBox = FromTheBox.Split(',');
                int[] TabOfInt = Array.ConvertAll(SeparetedFromTheBox, i => int.Parse(i));
                Array.Sort(TabOfInt);
                //towrzymy sobie macierz i rysujemy graf
                AdjacencyMatrix adjacencyMatrix = new AdjacencyMatrix(TabOfInt.Length);
                adjacencyMatrix.Display(MyCanvas, TabOfInt);
            }
            else
            {
                Dagrees.Background = Brushes.OrangeRed;
            }
        }





        //Rysujemy graf i szukamy w nim największej wspólnej składowej
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            //ta czesc jest skopiowana z zestawu 1
            if (Vertexes.Text == "")
            {
                Vertexes.Background = Brushes.OrangeRed;
            }
            else if (Edges.Text == "")
            {
                Vertexes.Background = Brushes.White;
                Edges.Background = Brushes.OrangeRed;
            }
            else if ((Int32.Parse(Edges.Text) * Int32.Parse(Edges.Text) - Int32.Parse(Edges.Text)) / 2 <= Int32.Parse(Vertexes.Text))
            {
                Vertexes.Background = Brushes.White;
                Edges.Background = Brushes.White;

                var num_of_v = Int32.Parse(Vertexes.Text);
                var num_of_e = Int32.Parse(Edges.Text);

                AdjacencyMatrix adjacencyMatrix = new AdjacencyMatrix(num_of_v);

                for (int i = 0; i < num_of_v; i++)
                {
                    for (int j = i + 1; j < num_of_v; j++)
                    {
                        adjacencyMatrix.AdjacencyArray[i, j] = 0;
                        adjacencyMatrix.AdjacencyArray[j, i] = 0;
                    }
                }

                int counter = 0;

                Random r = new Random();

                while (counter < num_of_e)
                {
                    int vertexToBeAdded_row = r.Next(0, num_of_v);
                    int vertexToBeAdded_col = r.Next(0, num_of_v);
                    if (adjacencyMatrix.AdjacencyArray[vertexToBeAdded_row, vertexToBeAdded_col] == 0 && vertexToBeAdded_row != vertexToBeAdded_col)
                    {
                        adjacencyMatrix.AdjacencyArray[vertexToBeAdded_row, vertexToBeAdded_col] = 1;
                        adjacencyMatrix.AdjacencyArray[vertexToBeAdded_col, vertexToBeAdded_row] = 1;
                        counter++;
                    }

                }

                //sprawdzanie od Tomka NWS
                //////////////////////////////

                int v = num_of_v;

                const int INF = 207;
                int n, m, a, b, ile_skladowych, max_sklad, ta_sklad, poprz_sklad;

                List <int> [] G = new List<int>[201];
                for(int i=0; i< 201; i++)
                {
                    G[i] = new List<int>();
                }
            
                Queue<int> Q = new Queue<int>();
                int[] D = new int[201];


                for (int i = 1; i <= v; i++)
                    D[i] = INF;

                for (int i = 0; i < v; i++)
                {
                    for (int j = i + 1; j < v; j++)
                    {
                        if (adjacencyMatrix.AdjacencyArray[i, j] == 1)
                        {
                            G[i+1].Add(j+1);
                            G[j+1].Add(i+1);
                        }
                    }
                }
                max_sklad = ta_sklad = poprz_sklad = ile_skladowych = 0;
                bfs(1, D, G, Q);

                int ile_w_sklad = HowManyInOne(ta_sklad, poprz_sklad, v, D);
                if (ile_w_sklad > max_sklad)
                    max_sklad = ile_w_sklad;
                poprz_sklad = ile_w_sklad;
                ta_sklad = poprz_sklad;
                ile_skladowych++;

                for (int h = 1; h <= v; h++)
                {
                    if (D[h] == INF)
                    {
                        bfs(h, D, G, Q);
                        ile_w_sklad = HowManyInOne(ta_sklad, poprz_sklad, v, D);
                        if (ile_w_sklad > max_sklad)
                            ile_w_sklad = ta_sklad;
                        ile_skladowych++;
                    }
                }
                Output.Text = max_sklad.ToString();
                ile_skladowych = 0;

                adjacencyMatrix.DrawGraph(num_of_v, MyCanvas);
            }
            else
            {
                Vertexes.Background = Brushes.OrangeRed;
                Edges.Background = Brushes.OrangeRed;
            }
        }



        //do NWS
        public int HowManyInOne(int ta, int poprz, int n, int[] D)
        {
            const int INF = 207;
            for (int l = 1; l <= n; l++)
            {
                if (D[l] != INF) //&& D[l]!=0
                {
                    ta++;
                }
            }
            return ta - poprz;
        }

        
        
        //do NWS
        void bfs(int p, int[] D, List<int>[] G, Queue<int> Q)
        {
            const int INF = 207;
            D[p] = 0;
            Q.Enqueue(p);
            while (Q.Count != 0)
            {
                int x = Q.Peek();
                Q.Dequeue();
                for (int i = 0; i < G[x].Count; i++)
                {
                    int y = G[x][i];
                    if (D[y] == INF)
                    {
                        D[y] = D[x] + 1;
                        Q.Enqueue(y);
                    }
                }
            }
        }




    }
}
