using System;

namespace SacsReport
{
    class Result
    {
        public int StoveNum { get; set; }
        public DateTime BurnStartTime { get; set; }
        public int BurningDuration { get; set; }
        public int BlastDuration { get; set; }
        public int DomeTemp1Duration { get; set; }
        public int DomeTemp2Duration { get; set; }
        public int DomeTempMin { get; set; }
        public int DomeTempMax { get; set; }
        public int DomeTempAverage { get; set; }
        public int DomeTempWhenStop { get; set; }
        public int StackTempWHenIgnition { get; set; }
        public int StackTempWHenStop { get; set; }
        public int StackTempAverage { get; set; }
        public int StackTempDiff { get; set; }
        public double AirFlowAmount { get; set; }
        public double AirFlowMin { get; set; }
        public double AirFlowMax { get; set; }
        public double AirFlowAverage { get; set; }
        public double GasFlowAmount { get; set; }
        public double GasFlowMin { get; set; }
        public double GasFlowMax { get; set; }
        public double GasFlowAverage { get; set; }
        public double AirFuelRatioMax { get; set; }
        public double AirFuelRatioMin { get; set; }
        public double AirFuelRatioAverage { get; set; }
        public double CaloricMin { get; set; }
        public double CaloricMax { get; set; }
        public double CaloricAverage { get; set; }
        public int HBMainTempAverage { get; set; }
    }
}
