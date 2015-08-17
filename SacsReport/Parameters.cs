using MongoDB.Bson;

namespace SacsReport
{
    class Parameters
    {
        public ObjectId Id;
        public int StackTempStopBurnPAR { get; set; }
        public int DomeTempSaveUpperLimitPAR { get; set; }
    }

    class stoveConfig
    {
        public ObjectId Id;
        public string Key;
        public object Value;
        public int StoveNum;
    }
}
