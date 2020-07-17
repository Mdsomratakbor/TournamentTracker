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
    }
}
