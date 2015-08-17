using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SacsReport
{
    class Mongo : IDisposable
    {
        public IMongoClient Client = null;
        public IMongoDatabase Database = null;
        public IMongoCollection<SourceData> Collection = null;

        #region ctor
        public Mongo() : this("localhost", "RunRecord", "Stove1RunRecord")
        {

        }

        public Mongo(string db, string coll) : this("localhost", db, coll) { }

        public Mongo(string coll) : this("localhost", "RunRecord", coll) { }
        public Mongo(string client, string db, string collection)
        {
            this.Client = new MongoClient();
            this.Database = this.Client.GetDatabase(db);
            this.Collection = this.Database.GetCollection<SourceData>(collection);
        }
        #endregion

        public Task<long> GetRecordCount(DateTime? startTime = null, DateTime? endTime = null)
        {
            startTime = startTime ?? DateTime.MinValue;
            endTime = endTime ?? DateTime.MaxValue;
            var builder = Builders<SourceData>.Filter;
            var filter = builder.Gte("Timestamp", startTime) & builder.Lte("Timestamp", endTime).ToBsonDocument();
            var count = this.Collection.CountAsync(filter);
            return count;
        }

        public async void GenerateReport(DateTime? startTime = null, DateTime? endTime = null)
        {
            var result = new List<Result>();
            var burnStart = false;
            var burnStop = false;
            var blastStart = false;
            var blastStop = false;

            try
            {
                startTime = startTime ?? DateTime.MinValue;
                endTime = endTime ?? DateTime.MaxValue;
                var builder = Builders<SourceData>.Filter;
                var filter = builder.Gte("Timestamp", startTime) & builder.Lte("Timestamp", endTime);
                using (StreamWriter record = new StreamWriter(Collection.CollectionNamespace.CollectionName + ".csv"))
                {
                    record.WriteLine("Timestamp,StoveNum,GasFlowSetPoint,GasManiFlowL1_A,GasAdjustValveRealL1_A,GasAdjustValveAutoL1_A,GasAdjustValveSetL1RTN,AirFlowSetPoint,AirManiFlowL1_A,AirAdjustValveRealL1_A,AirAdjustValveAutoL1_A,AirAdjustValveSetL1RTN,DomeTempL1_A,StackTempL1_A,FlueGasO2ConL1_A,HBMainTempL1_A,MixBlastValveRealL1_A,GasMainCaloricL1_A,CBValveDiffPresL1_A,GasManiPresL1_A,AutoBurnControlL1_A");
                    using (var cursor = await Collection.FindAsync(filter))
                    {
                        while (await cursor.MoveNextAsync())
                        {
                            var batch = cursor.Current;
                            //batch.ToList<SourceData>().ForEach(record.WriteLine);
                            foreach (var item in batch)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var message = string.Empty;
                if (e.InnerException != null)
                {
                    var ee = e.InnerException;
                    while (ee.InnerException != null)
                    {
                        ee = ee.InnerException;
                    }
                    message = ee.Message;
                }
                else
                {
                    message = e.Message;
                }
                using (StreamWriter log = new StreamWriter("Exception_" + Collection.CollectionNamespace.CollectionName + ".txt", true))
                {
                    log.WriteLine(string.Format("{0:yyyyMMddHHmmss.fff}:\t{1}", DateTime.Now, message));
                }
            }
        }

        public Task<Parameters> SelectStovePars(int stoveNum)
        {
            try
            {
                return Task.Run<Parameters>(async () =>
                {
                    var rtn = new Parameters();
                    var filter = Builders<stoveConfig>.Filter;
                    var query = filter.Eq("StoveNum", stoveNum) & (filter.Eq("Key", "StackTempStopBurnPAR") | filter.Eq("Key", "DomeTempSaveUpperLimitPAR"));
                    var coll = Database.GetCollection<stoveConfig>("StovePAR");
                    var StackTempStopBurnPAR = coll.Find<stoveConfig>(query & filter.Eq("Key", "StackTempStopBurnPAR")).SingleOrDefaultAsync();
                    var DomeTempSaveUpperLimitPAR = coll.Find<stoveConfig>(query & filter.Eq("Key", "DomeTempSaveUpperLimitPAR")).SingleOrDefaultAsync();
                    await Task.WhenAll(StackTempStopBurnPAR, DomeTempSaveUpperLimitPAR);
                    rtn.StackTempStopBurnPAR = (int)StackTempStopBurnPAR.Result.Value;
                    rtn.DomeTempSaveUpperLimitPAR = (int)DomeTempSaveUpperLimitPAR.Result.Value;

                    return rtn;
                    //rtn.StackTempStopBurnPAR = Convert.ToInt32(rst["StackTempStopBurnPAR"]);
                    //rtn.DomeTempSaveUpperLimitPAR = Convert.ToInt32(rst["DomeTempSaveUpperLimitPAR"]);
                });

            }
            catch (MongoConnectionException e)
            {
                using (StreamWriter log = new StreamWriter("Exception_" + Collection.CollectionNamespace.CollectionName + ".txt", true))
                {
                    log.WriteLine(string.Format("{0:yyyyMMddHHmmss.fff}:\t{1}", DateTime.Now, e.Message));
                }
                return null;
            }
            catch (MongoCommandException e)
            {
                //_logger.WriteException("GeneralFunction", MethodBase.GetCurrentMethod().Name, e);
                return null;
            }
            catch (Exception e)
            {
                var message = string.Empty;
                if (e.InnerException != null)
                {
                    var ee = e.InnerException;
                    while (ee.InnerException != null)
                    {
                        ee = ee.InnerException;
                    }
                    message = ee.Message;
                }
                else
                {
                    message = e.Message;
                }
                using (StreamWriter log = new StreamWriter("Exception_" + Collection.CollectionNamespace.CollectionName + ".txt", true))
                {
                    log.WriteLine(string.Format("{0:yyyyMMddHHmmss.fff}:\t{1}", DateTime.Now, message));
                }
            }
            return null;
        }

        public void Dispose()
        {
            Collection = null;
            Database = null;
            Client = null;
        }
    }
}
