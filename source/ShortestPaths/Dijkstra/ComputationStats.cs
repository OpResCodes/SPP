using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPaths.Dijkstra
{
    public struct ComputationStats
    {

        /// <summary>
        /// Returns the calculation time of the algorithm in milliseconds
        /// </summary>
        public long ComputationTimeInMilSec { get; set; }

        /// <summary>
        /// returns the writing time in milliseconds (currently not implemented)
        /// </summary>
        public long WritingTimeInMilSec { get; set; }

        /// <summary>
        /// Number of Nodes
        /// </summary>
        public int NumberOfNodes { get; set; }

        /// <summary>
        /// Number of Arcs
        /// </summary>
        public int NumberOfArcs { get; set; }
        
        /// <summary>
        /// Summarizes status of calculation
        /// </summary>
        public CalculationStatus Status { get; set; }

        /// <summary>
        /// Returns a detailed Status
        /// </summary>
        public string StatusDetail { get; set; }

        internal void Reset()
        {
            ComputationTimeInMilSec = 0;
            WritingTimeInMilSec = 0;
            NumberOfNodes = 0;
            NumberOfArcs = 0;
            Status = CalculationStatus.NotCalculated;
            StatusDetail = string.Empty;
        }

        public override string ToString()
        {
            return OutputPrinter.PrintStats(this);
        }

    }
}
