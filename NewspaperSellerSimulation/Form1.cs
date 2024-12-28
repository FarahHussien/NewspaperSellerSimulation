using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;

namespace NewspaperSellerSimulation
{
    public partial class Form1 : Form
    {
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }
        public Form1(SimulationSystem system)
        {
            InitializeComponent();
            this.SimulationTable = system.SimulationTable;
            this.PerformanceMeasures = system.PerformanceMeasures;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Table_Form is loading...");
            DataTable table = new DataTable();

            table.Columns.Add("Day", typeof(int));
            table.Columns.Add("RandomTypeOfNewsday", typeof(int));
            table.Columns.Add("TypeOfNewsday", typeof(string));
            table.Columns.Add("RandomDemand", typeof(int));
            table.Columns.Add("Demand", typeof(int));
            table.Columns.Add("DailyCost", typeof(int));
            table.Columns.Add("SalesProfit", typeof(int));
            table.Columns.Add("LostProfit", typeof(int));
            table.Columns.Add("ScrapProfit", typeof(int));
            table.Columns.Add("DailyProfit", typeof(int));
            
            foreach (var simCase in SimulationTable)
            {
                table.Rows.Add(
                    simCase.DayNo,
                    simCase.RandomNewsDayType,
                    simCase.NewsDayType,
                    simCase.RandomDemand,
                    simCase.Demand,
                    simCase.DailyCost,
                    simCase.SalesProfit,
                    simCase.LostProfit,
                    simCase.ScrapProfit,
                    simCase.DailyNetProfit
                );
            }
            dataGridView1.DefaultCellStyle.NullValue = "0";
            dataGridView1.DataSource = table;
            label1.Text = PerformanceMeasures.TotalSalesProfit.ToString();
            label2.Text = PerformanceMeasures.TotalCost.ToString();
            label3.Text = PerformanceMeasures.TotalLostProfit.ToString();
            label4.Text = PerformanceMeasures.TotalScrapProfit.ToString();
            label5.Text = PerformanceMeasures.TotalNetProfit.ToString();
            label6.Text = PerformanceMeasures.DaysWithMoreDemand.ToString();
            label7.Text = PerformanceMeasures.DaysWithUnsoldPapers.ToString();
        }
    }
}
