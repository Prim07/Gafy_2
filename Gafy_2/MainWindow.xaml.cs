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
                    OutPut = "Tak.";
                else
                    OutPut = "Nie.";
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
                adjacencyMatrix.Display(MyCanvas, TabOfInt, true);
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
            else if ((Int32.Parse(Vertexes.Text) * Int32.Parse(Vertexes.Text) - Int32.Parse(Vertexes.Text)) / 2 >= Int32.Parse(Edges.Text))
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

                adjacencyMatrix.DrawGraph(num_of_v, MyCanvas);



                //sprawdzanie NSS
                
                int v = num_of_v;
                
                Stack<int> stos = new Stack<int>();
                int cn = 0;
                int[] c = new int[v];
                for(int i=0; i<v; i++)
                {
                    c[i] = 0;
                }
                for (int i = 0; i < v; i++)
                {
                    if (c[i] > 0)
                    {
                        continue;
                    }
                    cn++;
                    stos.Push(i);
                    c[i] = cn;
                    while (stos.Count > 0)
                    {
                        int vv = stos.Pop();
                        List<int> neighbours = new List<int>();
                        for(int j=0; j<v; j++)
                        {
                            if( adjacencyMatrix.AdjacencyArray[vv, j] == 1)
                            {
                                neighbours.Add(j);
                            }
                        }
                        for (int j = 0; j < neighbours.Count; j++)
                        {
                            if (c[neighbours[j]] > 0)
                            {
                                continue;
                            }
                            stos.Push(neighbours[j]);
                            c[neighbours[j]] = cn;
                        }




                    }


                    if (c.Count(x => x == cn) == v)
                        break;
                }

                int max = 0, maxval = 0;
                for( int i=1; i<=cn; i++)
                {
                    if (c.Count(x => x == i) > max)
                    {
                        max = c.Count(x => x == i);
                        maxval = i;
                    }
                }

                string ciag = "";
                int ile = 0;
                for (int i = 0; i < v; i++)
                {
                    if (c[i] == maxval)
                    {
                        ciag += (i+1).ToString() + ", ";
                        ile++;
                    }
                }
                string nowyciag = " ";
                for(int i=0; i<ciag.Length-5; i++)
                {
                    nowyciag += ciag[i];
                }

                string wypisz = "NSS zawiera: " + nowyciag + ".";
                Output.Text = wypisz;

            }
            else
            {
                Vertexes.Background = Brushes.OrangeRed;
                Edges.Background = Brushes.OrangeRed;
            }
        }

        
      


        //budujemy graf eulerowski i znajdujemy cykl
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            //budujemy
            if (Veuler.Text != "0")
            {
                Veuler.Background = Brushes.White;
                var num_of_v = Int32.Parse(Veuler.Text);
                AdjacencyMatrix adjacencyMatrix = new AdjacencyMatrix(num_of_v);
                adjacencyMatrix.Display(MyCanvas, num_of_v);

                //cykl
                Stack stos = new Stack();
                Random r = new Random();int v = r.Next(0, num_of_v);
                int[,] byc = new int[num_of_v, num_of_v];
                for (int i = 0; i < num_of_v; i++)
                {
                    for (int j = 0; j < num_of_v; j++)
                    {
                        byc[i, j] = 0;
                    }
                }
                Euler(v, stos, num_of_v, adjacencyMatrix.AdjacencyArray, byc);
                
                //wypisz na ekran
                object[] tab = stos.ToArray();
                string wypisz = "";
                for(int i=0; i<tab.Length; i++)
                {
                    wypisz = wypisz + tab[i].ToString() + ", ";
                }
                wypisz = wypisz + tab[0].ToString() + ".";
                Output1.Text = wypisz;
            }
            else
            {
                Veuler.Background = Brushes.OrangeRed;
            }

            
        }
        
        //znajdowanie cyklu eulera
        public void Euler(int v, Stack stos, int num_of_v, int[,] AdjacencyArray, int[,] byc)
        {
           
            int dev = 0;
            for (int i = 0; i < num_of_v; i++)
            {
                if (AdjacencyArray[v, i] == 1)
                {
                    dev++;
                }
            }
            if( dev!= 0)
            {
                int u = -1;
                for (int i = 0; i < num_of_v; i++)
                {
                    if (AdjacencyArray[v, i] == 1)
                    {
                        u = i;
                    }
                }
                AdjacencyArray[v, u] = 0;
                AdjacencyArray[u, v] = 0;
                if (byc[v, u] == 0)
                {
                    byc[v, u] = 1;
                    byc[u, v] = 1;
                    Euler(u, stos, num_of_v, AdjacencyArray, byc);
                    stos.Push(u + 1);
                 
                }

            }
        }


        private void Button_Click4(object sender, RoutedEventArgs e)
        {
           var v = Int32.Parse(Ver.Text);
            var k = Int32.Parse(kkk.Text);
            int[] TabOfInt = new int[v];
            for (int i = 0; i < v; i++)
            {
                TabOfInt[i] = (int)k;
            }


            //Ta zmienna mówi nam czy ciąg jest grafuiczny czy nie jest
            bool TrueOrFalse = false;
            //znacznik konca petli
            bool EndLoop = false;
            //idneks ostatniego elementu tablicy
            int max = TabOfInt.Length - 1;

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
            if (TrueOrFalse == true)
            {
                kkk.Background = Brushes.White;
                Ver.Background = Brushes.White;

                for (int i = 0; i < v; i++)
                {
                    TabOfInt[i] = (int)k;
                }

                AdjacencyMatrix adjacencyMatrix = new AdjacencyMatrix(TabOfInt.Length);
                adjacencyMatrix.Display(MyCanvas, TabOfInt, true);


            }
              

            else
            {
                kkk.Background = Brushes.OrangeRed;
                Ver.Background = Brushes.OrangeRed;
            }
           

     
        }
            



    }
}
