using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace HeeckerenTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource tokenSource;
        Task process = null;
        int filter = 250;

        public object NavigationService { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Reset();
        }

        public void Reset()
        {
            Result.Items.Clear();
            Statistic.Items.Clear();
            Progress.Value = 0;
            FilterLabel.Content = filter;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            StartStop.Click -= Start_Click;
            Reset();
            StartStop.Content = "STOP";
            tokenSource = new CancellationTokenSource();
            process = Task.Run(() => Process(tokenSource.Token));
            StartStop.Click += Stop_Click;
        }

        private async void Stop_Click(object sender, RoutedEventArgs e)
        {
            StartStop.Click -= Stop_Click;
            StartStop.Content = "WAIT";
            //Reset();
            tokenSource.Cancel();
            try { await process; }
            catch (OperationCanceledException _e) { }
            finally { tokenSource.Dispose(); }
            StartStop.Content = "START";
            StartStop.Click += Start_Click;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            filter = (int)e.NewValue * 50;
            FilterLabel.Content = filter;
        }

        delegate void UpdateProgressBarDelegate(DependencyProperty dp, object value);
        delegate int UpdateListDelegate(object value);

        class ExButton : Button
        {
            public string name { get; set; }
        };

        public async Task Process(CancellationToken ct)
        {
            UpdateProgressBarDelegate updProgress = new UpdateProgressBarDelegate(Progress.SetValue);
            UpdateListDelegate udStatistic = new UpdateListDelegate(Statistic.Items.Add);
            UpdateListDelegate udResult = new UpdateListDelegate(Result.Items.Add);

            ct.ThrowIfCancellationRequested();

            //int value=0;
            try
            {
                Parser parser = new Parser();

                //lloking for all files
                DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory + "\\logs\\");
                FileInfo[] files = d.GetFiles("*.txt");
                Dispatcher.Invoke(udStatistic, new object[] { "Found " + files.Count() + " files." });
                if (files.Count() == 0) throw new Exception();


                //creating the kill list
                List<string> file = new List<string>();
                foreach (FileInfo fi in files)
                {
                    file.AddRange(parser.Load(fi, (s) => Dispatcher.Invoke(udStatistic, new object[] { s }), ct));
                }
                if (file.Count() == 0) throw new Exception();

                //parse the kill list
                List<SingleKill> kill = parser.Parse(file,
                    (s) => Dispatcher.Invoke(udStatistic, new object[] { s }),
                    (p) => Dispatcher.Invoke(updProgress, new object[] { ProgressBar.ValueProperty, p }),
                    ct);

                if (kill==null || kill.Count() == 0) { throw new Exception(); }

                //create a players list 
                Dictionary dict = new Dictionary(kill);
                Dispatcher.Invoke(udStatistic, new object[] { "Found " + dict.Count + " names." });

                //calculate ratios
                RatioTable rt = new RatioTable(dict.Count);
                double pbar = 25.0;
                double step = 74.0 / (double)kill.Count;
                foreach (SingleKill sk in kill)
                {
                    int k = dict.FindName(sk.killer);
                    int v = dict.FindName(sk.victim);
                    rt.Add(k, v);
                    Dispatcher.Invoke(updProgress, new object[] { ProgressBar.ValueProperty, pbar += step });
                }

                SortedArray sa = new SortedArray(dict.Count);
                for (int n = 0; n < dict.Count; n++)
                {
                    pbar += step;
                     if (rt.GetKills(n) >= filter) sa.Add((n, rt.GetLevel(n),rt.GetRelations(n)));
                }

                foreach ((int, int, int) v in sa.Array())
                {
                    Dispatcher.Invoke(udResult, new object[] { ((Math.Abs((double)v.Item2)/(double)v.Item3) * 100).ToString("F2") + "% (" + v.Item2 +" of " + v.Item3 + ") "+ dict.Name(v.Item1) });
                }

                //https://battlelog.battlefield.com/bf3/user/

                Dispatcher.Invoke(udStatistic, new object[] { "Done. Selected " + sa.Count + (sa.Count>1?" names.":" name.") });
                Dispatcher.Invoke(updProgress, new object[] { ProgressBar.ValueProperty, 100.0 });
            }
            catch (Exception e) { }
            finally
            {
                Dispatcher.Invoke(() =>
                {
                    StartStop.Click -= Stop_Click;
                    StartStop.Content = "START";
                    StartStop.Click += Start_Click;
                });
            }
            await Task.CompletedTask;
        }

        private void Result_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string item=e.AddedItems[0] as string;
            string[] chunks=item.Split(' ');
            if (chunks.Length>4) System.Diagnostics.Process.Start(@"https://battlelog.battlefield.com/bf3/user/"+chunks[4]);
        }
    }
}
