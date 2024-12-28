using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NewspaperSellerModels.Enums;

namespace NewspaperSellerModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            DayTypeDistributions = new List<DayTypeDistribution>();
            DemandDistributions = new List<DemandDistribution>();
            SimulationTable = new List<SimulationCase>();
            PerformanceMeasures = new PerformanceMeasures();
        }
        ///////////// INPUTS /////////////
        public int NumOfNewspapers { get; set; }
        public int NumOfRecords { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ScrapPrice { get; set; }
        public decimal UnitProfit { get; set; }
        public List<DayTypeDistribution> DayTypeDistributions { get; set; }
        public List<DemandDistribution> DemandDistributions { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

        public void RunSimulation()
        {
            this.PerformanceMeasures.TotalSalesProfit = 0;
            this.PerformanceMeasures.TotalLostProfit = 0;
            this.PerformanceMeasures.TotalScrapProfit = 0;
            this.PerformanceMeasures.TotalNetProfit = 0;
            this.PerformanceMeasures.TotalCost = 0;
            this.PerformanceMeasures.DaysWithMoreDemand = 0;
            this.PerformanceMeasures.DaysWithUnsoldPapers = 0;

            decimal PurchasePrice = this.PurchasePrice; // b4try bkam
            decimal unitProfit = SellingPrice - this.PurchasePrice; // ksbt kam

            SimulationTable.Clear();

            List<int> randoms = GenerateRandomValue();
            for (int dayNo = 1; dayNo <= NumOfRecords; dayNo++)
            {
                int randomDay = randoms[dayNo - 1];
                int randomDemand = randoms[dayNo - 1];
                DayType selectedDayType = GetDayType(randomDay);

                int demandNewsPapers = GetDemand(randomDay, selectedDayType); // da el demaned based on dayType we got it

                /* gehad check these calculations please */
                /*TODO*/
                // calc #newspapers to be sold, unsold, or scrapped
                int newspapersSold = Math.Min(demandNewsPapers, NumOfNewspapers); // get min
                int newspapersUnsold = NumOfNewspapers - newspapersSold;
                int newspapersScrapped = newspapersUnsold > 0 ? newspapersUnsold : 0;

                int excessPaper = demandNewsPapers > NumOfNewspapers ? (demandNewsPapers - NumOfNewspapers) : 0;

                // calc daily cost, sales profit, lost profit, and scrap profit 
                decimal dailyCost = NumOfNewspapers * PurchasePrice;
                decimal salesProfit = newspapersSold * SellingPrice;
                decimal lostProfit = excessPaper * unitProfit;
                decimal scrapProfit = newspapersScrapped * ScrapPrice;
                decimal dailyNetProfit = salesProfit - lostProfit - dailyCost + scrapProfit;

                //if(dailyNetProfit<0) 
                //    dailyNetProfit = 0;
                // here we create simCase and add it to table
                SimulationCase simulationCase = new SimulationCase
                {
                    DayNo = dayNo,
                    RandomNewsDayType = randomDay,
                    NewsDayType = selectedDayType,
                    RandomDemand = randomDemand,
                    Demand = demandNewsPapers,
                    DailyCost = dailyCost,
                    SalesProfit = salesProfit,
                    LostProfit = lostProfit,
                    ScrapProfit = scrapProfit,
                    DailyNetProfit = dailyNetProfit
                     
                };

                SimulationTable.Add(simulationCase);

                // nada update performance measures before end of each iteration cause it dependent
                /*TODO*/
                PerformanceMeasures.TotalSalesProfit += salesProfit;
                PerformanceMeasures.TotalLostProfit += lostProfit;
                PerformanceMeasures.TotalScrapProfit += scrapProfit;
                PerformanceMeasures.TotalNetProfit += dailyNetProfit;
                PerformanceMeasures.TotalCost += dailyCost;

                // nada update  DaysWithMoreDemand & DaysWithUnsoldPapers
                /*TODO*/
                if (demandNewsPapers > NumOfNewspapers)
                {
                    this.PerformanceMeasures.DaysWithMoreDemand += 1;
                }
                else if (demandNewsPapers < NumOfNewspapers)
                {
                    this.PerformanceMeasures.DaysWithUnsoldPapers += 1;
                }
                

            }

            // nada update for performance measures
            /*TODO*/
            UpdatePerformanceMeasures();
        }
        private List<int> GenerateRandomValue()
        {
            List<int> rand_list = new List<int>();
            Random rand = new Random(40);
            for (int j = 0; j < NumOfRecords; j++)
            {
                int i = rand.Next(1, 100 + 1);
                rand_list.Add(i);
            }
            return rand_list;
        }

        private DayType GetDayType(int random)
        {

            foreach (var dayType in DayTypeDistributions)
            {
                if (random >= dayType.MinRange &&  random <= dayType.MaxRange)
                {
                    return dayType.DayType;
                }
            }

            // Default to Good if nothing is selected
            return DayType.Good;
        }

        private int GetDemand(int random, DayType day)
        {
         
            foreach (var distribution in DemandDistributions)
            {
                foreach (var dist in distribution.DayTypeDistributions)
                {
                    if(dist.DayType == day)
                    {
                        if (random >= dist.MinRange && random <= dist.MaxRange){
                            return distribution.Demand;
                        }
                    }
                }
            }
            return 0;
        }

        private void UpdatePerformanceMeasures()
        {
           
        }

    }
}
