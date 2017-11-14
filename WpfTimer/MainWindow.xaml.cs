using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using System.Windows.Controls;

namespace WpfTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer = new Timer(15000); 
        //25 min = 1500000  task time
        //5 min = 300000   short brake
        //20 min = 1200000   long brake
        private int counter = 0;
        private ElapsedEventHandler lastHandler;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Change(ElapsedEventHandler handler, int newInterval)
        {
            timer.Enabled = false;
            if (lastHandler != null)
            {
                timer.Elapsed -= lastHandler;
            }
            if (handler != null)
            {
                lastHandler = handler;
                timer.Elapsed += lastHandler;
            }
            if (newInterval > 0)
            {
                timer.Interval = newInterval;
                timer.Enabled = true;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            output.Text = "Start working on your tasks.";
            timer.AutoReset = false;

            Change(new ElapsedEventHandler(OnTimedEvent), 2500);
            timer.Start();
            counter = 0;
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                output.Text = "Time is out!";
            });
            if(counter == 3)
            {
                Change(new ElapsedEventHandler(OnTimedBreak), 2000);
                counter = 0;
                this.Dispatcher.Invoke(() =>
                {
                    output.Text = "Time for a long break. Have a walk. :)";
                });
            }
            else
            {
                counter++;
                Change(new ElapsedEventHandler(OnTimedBreak), 1500);
                this.Dispatcher.Invoke(() =>
                {
                    output.Text = "Take a 5 minute break.";
                });
            }
        }

        private void OnTimedBreak(object source, ElapsedEventArgs e)
        {
            Change(new ElapsedEventHandler(OnTimedEvent), 2500); //task time
            this.Dispatcher.Invoke(() =>
            {
                output.Text = "Time is out! Back to work.";
            });
        }
        
    }
}
