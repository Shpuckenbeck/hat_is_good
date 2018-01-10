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
using System.IO;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] shuffle(string[] listick)
        {
            Random random = new Random();
            int leng = listick.Length - 1;
            for (int i = leng; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = listick[j];
                listick[j] = listick[i];
                listick[i] = temp;
            }
            return listick;
        }

        string[] _filePaths;
        DispatcherTimer answertimer = null;
        string currentimg;
        string pl1, pl2;
        int round, findex;
        List<string> play = new List<string>();
        bool STOP_THIS_SHIT = false;
        public ObservableCollection<int> ochki;

        private void timerStart()
        {
            answertimer = new DispatcherTimer();  // если надо, то в скобках указываем приоритет, например DispatcherPriority.Render
            answertimer.Tick += new EventHandler(timerTick);
            answertimer.Interval = new TimeSpan(0, 0, 1);
            answertimer.Start();
            timer.Content = 27;
        }
        

        private void timerTick(object sender, EventArgs e)
        {
            timer.Content = Convert.ToInt32(timer.Content)-1;
            if (Convert.ToInt32(timer.Content) <= 0)
            {
                stopper();
                Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("!white_shit.jpg");
                Ihochki.Content = 0;
            }
        }

        private void stopper()
        {
            answertimer.Stop();
            ochki[findex] += Convert.ToInt32(Ihochki.Content);
            ochki[(findex + round) % play.Count] += Convert.ToInt32(Ihochki.Content);
            Schet.ItemsSource = ochki;
            if (findex == play.Count - 1)
            {
                findex = -1;
                round += 1;
            }
            findex += 1;
            pl1 = play[findex];
            pl2 = play[(findex + round) % play.Count];
            Para.Content = pl1 + " - " + pl2;
            if (round == play.Count )
            {
                Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Game_over.jpg");
                STOP_THIS_SHIT = true;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void shind_Loaded(object sender, RoutedEventArgs e)
        {
            _filePaths = Directory.GetFiles("Shlyapa");
            FileStream f = new FileStream("players.txt", FileMode.Open);
            StreamReader reader = new StreamReader(f, Encoding.UTF8);
            while (reader.EndOfStream == false)
            {
                play.Add(reader.ReadLine());
            }
            reader.Close();
            ochki = new ObservableCollection<int>();
            for(int i=0; i<play.Count; i++)
            {
                ochki.Add(0);
            }
            word_count.Content = _filePaths.Length;
            players.ItemsSource = play;
            findex = 0;
            pl1 = play[findex];
            round = 1;
            pl2 = play[(findex + round) % play.Count];
            Para.Content = pl1 + " - " + pl2;
            Schet.ItemsSource = ochki;
        }

        private void shind_KeyDown(object sender, KeyEventArgs e)
        {
            string[] Sh_path = shuffle(_filePaths);
            if (!(STOP_THIS_SHIT) && (Convert.ToInt32(word_count.Content) > 1))
            {
                if (e.Key == Key.Up)
                {
                    currentimg = Sh_path[Sh_path.Length - 1];
                    Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(currentimg);
                    timerStart();
                    Ihochki.Content = 0;
                }
                if (e.Key == Key.Down)
                {
                    _filePaths = _filePaths.Where(a => a != currentimg).ToArray();
                    Sh_path = shuffle(_filePaths);
                    word_count.Content = Convert.ToInt32(word_count.Content) - 1;
                    if (Convert.ToInt32(timer.Content) >= 7)

                    {
                        timer.Content = Convert.ToInt32(timer.Content) - 7;
                        currentimg = Sh_path[Sh_path.Length - 1];
                        Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(currentimg);
                    }
                    else
                    {
                        stopper();
                        Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("!white_shit.jpg");
                        Ihochki.Content = 0;
                    }
                }
                if (e.Key == Key.Right)
                {
                    _filePaths = _filePaths.Where(a => a != currentimg).ToArray();
                    Sh_path = shuffle(_filePaths);
                    word_count.Content = Convert.ToInt32(word_count.Content) - 1;
                    Ihochki.Content = Convert.ToInt32(Ihochki.Content) + 1;
                    currentimg = Sh_path[Sh_path.Length - 1];
                    Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(currentimg);
                }
            }
            else
            {
                if(!(STOP_THIS_SHIT) && Convert.ToInt32(word_count.Content)==1)
                {
                    if (e.Key == Key.Up)
                    {
                        timerStart();
                        Ihochki.Content = 0;
                        currentimg = Sh_path[Sh_path.Length - 1];
                        Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(currentimg);
                    }
                    if (e.Key == Key.Down)
                    {
                        _filePaths = _filePaths.Where(a => a != currentimg).ToArray();
                        Sh_path = shuffle(_filePaths);
                        word_count.Content = Convert.ToInt32(word_count.Content) - 1;
                        Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("!white_shit.jpg");
                        stopper();
                        if (Convert.ToInt32(timer.Content) >= 7)

                        {
                            timer.Content = Convert.ToInt32(timer.Content) - 7;
                            Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("!white_shit.jpg");
                            stopper();
                        }
                        else
                        {
                            stopper();
                            Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("!white_shit.jpg");
                            Ihochki.Content = 0;
                        }
                    }
                    if (e.Key == Key.Right)
                    {
                        _filePaths = _filePaths.Where(a => a != currentimg).ToArray();
                        Sh_path = shuffle(_filePaths);
                        word_count.Content = Convert.ToInt32(word_count.Content) - 1;
                        Ihochki.Content = Convert.ToInt32(Ihochki.Content) + 1;
                        Imga.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("!white_shit.jpg");
                        stopper();
                    }
                }
            }
        }
        //private void ses_Click(object sender, RoutedEventArgs e)
        //{
        //    Para.Content = "Content";
        //}
    }
}
