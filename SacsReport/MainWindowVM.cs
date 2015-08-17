using SGAI.xOriginNet.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SacsReport
{
    class MainWindowVM : ViewModelBase
    {
        #region Properties
        public ObservableCollection<bool> StoveToExport { get; set; }
        public DelegateCommand CmdGenerateReport { get; set; }
        public int FontSize_L { get; set; }

        private DateTime? _startTime;
        public DateTime? StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    this.NotifyPropertyChanged(p => p.StartTime);
                }
            }
        }

        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    this.NotifyPropertyChanged(p => p.EndTime);
                }
            }
        }

        private int _pbMax;
        public int PBMax
        {
            get
            {
                return _pbMax;
            }
            set
            {
                if (_pbMax != value)
                {
                    _pbMax = value;
                    this.NotifyPropertyChanged(p => p.PBMax);
                }
            }
        }

        private int _pbValue;
        public int PBValue
        {
            get
            {
                return _pbValue;
            }
            set
            {
                if (_pbValue != value)
                {
                    _pbValue = value;
                    this.NotifyPropertyChanged(p => p.PBValue);
                }
            }
        }

        private Visibility _pbVisibility;
        public Visibility PBVisibility
        {
            get
            {
                return _pbVisibility;
            }
            set
            {
                if (_pbVisibility != value)
                {
                    _pbVisibility = value;
                    this.NotifyPropertyChanged(p => p.PBVisibility);
                }
            }
        }

        #endregion

        #region Contructor

        public MainWindowVM()
        {
            this.FontSize_L = 40;
            this.PBMax = 100; this.PBValue = 0;
            this.PBVisibility = Visibility.Hidden;
            this.StartTime = DateTime.Now.AddMonths(-1);
            this.EndTime = DateTime.Now;
            this.StoveToExport = new ObservableCollection<bool>(Enumerable.Repeat<bool>(false, 7));
            this.CmdGenerateReport = new DelegateCommand(p => this.GenerateReport(), "Generate Report");
        }
        #endregion

        #region Mehtods
        void GenerateReport()
        {
            this.PBVisibility = Visibility.Visible;
            //this.PBMax = StoveToExport.Where(s => s == true).Count();
            var g = Parallel.For(1, 7, i =>
              {
                  if (StoveToExport[i])
                  {
                      using (Mongo stove = new Mongo("Stove" + i + "RunRecord"))
                      {
                          //var task = Task.Run(() => stove.GenerateReport(_startTime, _endTime));
                          //task.ContinueWith(p => PBValue++);
                          stove.GenerateReport(_startTime, _endTime);
                      }

                      PBValue++;
                  }
              });
            // MessageBox.Show("Export Over");
            while (StoveToExport.Where(s => s == false).Count() != StoveToExport.Count)
            {
                PBValue = (PBValue++) % PBMax;
            }
            MessageBox.Show("Generate is over!");
        }
        #endregion
    }
}
