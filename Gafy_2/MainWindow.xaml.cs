using System;
using System.Collections.Generic;
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
            if (Dagrees.Text != "0")
            {
                Dagrees.Background = Brushes.White;
                string FromTheBox = Dagrees.Text;
                string[] SeparetedFromTheBox = FromTheBox.Split(',');
                int[] TabOfInt = Array.ConvertAll(SeparetedFromTheBox, i => int.Parse(i));
                Array.Sort(TabOfInt);
                AdjacencyMatrix adjacencyMatrix = new AdjacencyMatrix(TabOfInt.Length);
                adjacencyMatrix.Display(MyCanvas, TabOfInt);
            }
            else
            {
                Dagrees.Background = Brushes.OrangeRed;
            }
        }
    }
}
