using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using PriceEngine;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace InstrumentMonitor.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);
        private RelayCommand addCommand;
        private RelayCommand removeCommand;
        private RelayCommand startCommand;
        private RelayCommand stopCommand;

        private string inSymbol;
        private string outSymbol;

      #region Commands

        public ICommand StartCommand
        {
            get
            {
                if (startCommand == null)
                {
                    startCommand = new RelayCommand(param => this.StartCommandExecute(), param => true);
                }
                return startCommand;
            }
        }

        public ICommand StopCommand
        {
            get
            {
                if (stopCommand == null)
                {
                    stopCommand = new RelayCommand(param => this.StopCommandExecute(), param => true);
                }
                return stopCommand;
            }
        }

        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(param => this.AddCommandExecute(), param => true);
                }
                return addCommand;
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                {
                    removeCommand = new RelayCommand(param => this.RemoveCommandExecute(), param => true);
                }
                return removeCommand;
            }
        }

        private void StartCommandExecute()
        {
            try
            {
                if (Instruments == null)
                {
                    this.Instruments = Engine.GetSubcribedInstruments();
                }
                Engine.Start();
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Trouble starting Engine, error: " + ex.Message);
            }
        }

        private void StopCommandExecute()
        {
            try
            {
                Engine.Stop();
                timer.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Trouble stopping Engine, error: " + ex.Message);
            }
        }


        private void AddCommandExecute()
        {
            try
            {
                if (!String.IsNullOrEmpty(InSymbol))
                {
                    this.Engine.Subscribe(InSymbol);
                    MessageBox.Show("Instrument Added Successfully");
                    InSymbol = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Trouble addinig instrument, error: " + ex.Message);
            }
        }

        private void RemoveCommandExecute()
        {
            try
            {
                if (!String.IsNullOrEmpty(OutSymbol))
                {
                    this.Engine.UnSubscribe(OutSymbol);
                    MessageBox.Show("Instrument Successfully Unsubscribed");
                    OutSymbol = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Trouble removing instrument, error: " + ex.Message);
            }
        }

    # endregion


        private SimEngine engine = null;
        public SimEngine Engine
        {
            get
            {
                if (engine == null) { engine = new SimEngine(); }
                return engine;

            }
        }

        public string InSymbol
        {
            get
            {
                return inSymbol;
            }
            set
            {
                inSymbol = value;
                OnPropertyChanged("InSymbol");
            }
        }

        public string OutSymbol
        {
            get
            {
                return outSymbol;
            }
            set
            {
                outSymbol = value;
                OnPropertyChanged("OutSymbol");
            }
        }

    
        ObservableCollection<Instrument> instruments;
        public ObservableCollection<Instrument> Instruments 
        {
            get
            {
                return instruments;
            }
            set
            {
                instruments = value;
                OnPropertyChanged("Instruments");
            }
        }

        public MainWindowViewModel()
        {
            timer.Tick += new EventHandler(GetSubscribedInstruments);
            timer.Interval = TimeSpan.FromSeconds(10);
        }

        private void GetSubscribedInstruments(object sender, EventArgs e)
        {
            this.Instruments = Engine.GetSubcribedInstruments();
        }
    }
}
