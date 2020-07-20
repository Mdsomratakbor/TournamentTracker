using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModel.csv";
        public PrizeModel CreatePrize(PrizeModel model)
        {
           List<PrizeModel> prizes= PrizesFile.FullFilePath().LoadFile().ConvertPrizeModels();          
            int currentId = prizes.OrderByDescending(x => x.Id).First().Id+1;
            model.Id = currentId;
            prizes.Add(model);
        }
       
    }
}
