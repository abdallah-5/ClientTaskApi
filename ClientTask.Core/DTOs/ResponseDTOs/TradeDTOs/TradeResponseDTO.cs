using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTask.Core.DTOs.ResponseDTOs.PolygonDTOs
{
    public class TradeResponseDTO
    {
        public double V { get; set; } // Volume
        public double VW { get; set; } // Volume Weighted Average Price
        public double O { get; set; } // Open
        public double C { get; set; } // Close
        public double H { get; set; } // High
        public double L { get; set; } // Low
        public long T { get; set; } // Timestamp (Epoch milliseconds)
        public long N { get; set; } // Number of items in the aggregate
    }
}
