using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SacsReport
{
    class SourceData
    {
        public ObjectId Id;
        public int StoveNum;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timestamp;
        public double GasManiFlowL1_A;
        public int GasAdjustValveRealL1_A;
        public double AirManiFlowL1_A;
        public int AirAdjustValveRealL1_A;
        public int DomeTempL1_A;
        public int StackTempL1_A;
        public double FlueGasO2ConL1_A;
        public int HBMainTempL1_A;
        public int MixBlastValveRealL1_A;
        public double GasMainCaloricL1_A;
        public int GasFlowSetPoint;
        public int AirFlowSetPoint;
        public double CBValveDiffPresL1_A;
        public double GasManiPresL1_A;
        public int GasAdjustValveAutoL1_A;
        public int AirAdjustValveAutoL1_A;
        public int GasAdjustValveSetL1RTN;
        public int AirAdjustValveSetL1RTN;
        public bool AutoBurnControlL1_A;
        public override string ToString()
        {
            return Timestamp.ToLocalTime() + "," + StoveNum + "," + GasFlowSetPoint + "," + GasManiFlowL1_A + "," + GasAdjustValveRealL1_A + "," + GasAdjustValveAutoL1_A + "," + GasAdjustValveSetL1RTN + "," + AirFlowSetPoint + "," + AirManiFlowL1_A + "," + AirAdjustValveRealL1_A + "," + AirAdjustValveAutoL1_A + "," + AirAdjustValveSetL1RTN + "," + DomeTempL1_A + "," + StackTempL1_A + "," + FlueGasO2ConL1_A + "," + HBMainTempL1_A + "," + MixBlastValveRealL1_A + "," + GasMainCaloricL1_A + "," + CBValveDiffPresL1_A + "," + GasManiPresL1_A + "," + AutoBurnControlL1_A;
        }

    }
}
