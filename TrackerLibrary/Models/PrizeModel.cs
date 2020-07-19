using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    /// <summary>
    /// Represnt every team position prize.
    /// </summary>
    public class PrizeModel
    {
        /// <summary>
        /// This the unique identifir for Prize
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represnt one team position.
        /// </summary>
        public int PlaceNumber { get; set; }
        /// <summary>
        /// Represnt one team position name.
        /// </summary>
        public string PlaceName { get; set; }
        /// <summary>
        /// Represnt one team position prize amount.
        /// </summary>
        public decimal PrizeAmount { get; set; }
        /// <summary>
        /// Represnt one team position prize amount percentage.
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }
        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            this.PlaceName = placeName;
            int placeNumberValue = 0;
            int.TryParse(placeNumber, out placeNumberValue);
            this.PlaceNumber = placeNumberValue;
            decimal prizeAmontValue = 0;
            decimal.TryParse(prizeAmount, out prizeAmontValue);
            this.PrizeAmount = prizeAmontValue;
            double prizePercentageValue = 0;
            double.TryParse(prizePercentage, out prizePercentageValue);
            this.PrizePercentage = prizePercentageValue;
        }
    }
}
