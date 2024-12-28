using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;

namespace NewspaperSellerSimulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string file = "";
            
            SimulationSystem system = new SimulationSystem();
            ReadFile readFile = new ReadFile();
            system = readFile.ReadSimulationFile(file);
            system.RunSimulation();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(system));

            string testingResult = TestingManager.Test(system, Constants.FileNames.TestCase3);
            MessageBox.Show(testingResult); 
        }

    }
}
