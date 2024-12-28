using NewspaperSellerModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace NewspaperSellerSimulation
{
    internal class ReadFile
    {
        public SimulationSystem ReadSimulationFile(string filePath)
        {
            SimulationSystem system = new SimulationSystem();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                int count = 5;
                // read single line simulation parameters
                while ((line = reader.ReadLine()) != null && count != 0)
                {
                    line = line.Trim();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string key = line;

                    line = reader.ReadLine();
                    if (line == null || string.IsNullOrWhiteSpace(line) || !IsNumeric(line))
                        continue; 

                    string value = line.Trim();

                    switch (key)
                    {
                        case "NumOfNewspapers":
                            system.NumOfNewspapers = int.Parse(value);
                            count--;
                            break;
                        case "NumOfRecords":
                            system.NumOfRecords = int.Parse(value);
                            count--;
                            break;
                        case "PurchasePrice":
                            system.PurchasePrice = decimal.Parse(value);
                            count--;
                            break;
                        case "ScrapPrice":
                            system.ScrapPrice = decimal.Parse(value);
                            count--;
                            break;
                        case "SellingPrice":
                            system.SellingPrice = decimal.Parse(value);
                            count--;
                            break;
                    }
                }


                // read DayTypeDistributions
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == "DayTypeDistributions")
                        break;
                }

                line = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] dayTypeValues = line.Split(',');

                    decimal cumulativeProbability = 0;
                    int minRange = 1;
                    int dayTypeIndex = 0;

                    foreach (string value in dayTypeValues)
                    {
                        decimal probability = decimal.Parse(value.Trim());

                        cumulativeProbability += probability;
                        int maxRange = (int)(cumulativeProbability * 100);

                        system.DayTypeDistributions.Add(new DayTypeDistribution
                        {
                            DayType = (Enums.DayType)dayTypeIndex,
                            Probability = probability,
                            CummProbability = cumulativeProbability,
                            MinRange = minRange,
                            MaxRange = maxRange
                        });

                        minRange = maxRange + 1;
                        dayTypeIndex++;
                    }
                }

                // read DemandDistributions
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == "DemandDistributions")
                        break;
                }

                decimal cumulativeGoodProbability = 0;
                decimal cumulativeFairProbability = 0;
                decimal cumulativePoorProbability = 0;

                int minRangeGood = 1;
                int minRangeFair = 1;
                int minRangePoor = 1;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] values = line.Split(',');

                    // 1st = demand
                    int demand = int.Parse(values[0]);

                    DemandDistribution demandDistribution = new DemandDistribution
                    {
                        Demand = demand
                    };

                    for (int i = 1; i < values.Length; i++)
                    {
                        decimal probability = decimal.Parse(values[i].Trim());

                        if (i == 1) // good prob
                        {
                            cumulativeGoodProbability += probability;
                            int maxRangeGood = (int)(cumulativeGoodProbability * 100);
                            demandDistribution.DayTypeDistributions.Add(new DayTypeDistribution
                            {
                                DayType = Enums.DayType.Good,
                                Probability = probability,
                                CummProbability = cumulativeGoodProbability,
                                MinRange = minRangeGood,
                                MaxRange = maxRangeGood
                            });

                            minRangeGood = maxRangeGood + 1;
                        }
                        else if (i == 2) // fair prob
                        {
                            cumulativeFairProbability += probability;
                            int maxRangeFair = (int)(cumulativeFairProbability * 100);
                            demandDistribution.DayTypeDistributions.Add(new DayTypeDistribution
                            {
                                DayType = Enums.DayType.Fair,
                                Probability = probability,
                                CummProbability = cumulativeFairProbability,
                                MinRange = minRangeFair,
                                MaxRange = maxRangeFair
                            });

                            minRangeFair = maxRangeFair + 1;
                        }
                        else if (i == 3) // poor prob
                        {
                            cumulativePoorProbability += probability;
                            int maxRangePoor = (int)(cumulativePoorProbability * 100);
                            demandDistribution.DayTypeDistributions.Add(new DayTypeDistribution
                            {
                                DayType = Enums.DayType.Poor,
                                Probability = probability,
                                CummProbability = cumulativePoorProbability,
                                MinRange = minRangePoor,
                                MaxRange = maxRangePoor
                            });

                            minRangePoor = maxRangePoor + 1;
                        }
                    }

                    system.DemandDistributions.Add(demandDistribution);
                }


            }

            return system;
        }

        // check if a string is a valid number not contain str
        private bool IsNumeric(string str)
        {
            return decimal.TryParse(str, out _);
        }
    }
}
