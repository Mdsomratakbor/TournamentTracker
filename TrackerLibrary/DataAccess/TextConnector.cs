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

        public PersonModel CreatePerson(PersonModel model)
        {
            return model;
        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
           List<PrizeModel> prizes= PrizesFile.FullFilePath().LoadFile().ConvertPrizeModels();
            int currentId = 1;
            if (prizes.Count > 0)
            {
                 currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
          
            model.Id = currentId;
            prizes.Add(model);
            prizes.SaveToPrizeFile(PrizesFile);
            return model;
        }
       
    }
}
