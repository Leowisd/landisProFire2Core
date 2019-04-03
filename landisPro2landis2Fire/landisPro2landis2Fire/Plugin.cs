using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landis.Core;
using Landis.Library.FireManagement;
using System.Diagnostics;
using Landis.SpatialModeling;
using System.IO;



namespace Landis.Extension.Landispro.Fire
{
    class Plugin
        : FireExtensionMain
    {
        public static readonly string ExtensionName = "Landis_pro Fire";


        private static ICore modelCore;

        public PlugIn()
            : base(ExtensionName)
        {
        }

        public static ICore ModelCore
        {
            get
            {
                return modelCore;
            }
            private set
            {
                modelCore = value;
            }
        }

        public override void LoadParameters(string dataFile, ICore mCore)
        {
            modelCore = mCore;
            Console.WriteLine("The datafile is: " + dataFile);
            Console.WriteLine();
           

            Console.WriteLine("Harvest Dll loaded in...");
            Console.WriteLine();

            GlobalFunctions.HarvestPass(Landis.Extension.Succession.Landispro.PlugIn.gl_sites, Landis.Extension.Succession.Landispro.PlugIn.gl_spe_Attrs);
            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.Harvest70outputdim();

            Console.WriteLine("End Landis_pro Harvest Parameters Loading\n");
            Console.WriteLine("========================================\n");
        }

        public override void Initialize()
        {
            Console.WriteLine("Running Landis_pro Harvest Initialization...");
            Console.WriteLine();

            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.stocking_x_value = Landis.Extension.Succession.Landispro.PlugIn.gl_param.Stocking_x_value;
            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.stocking_y_value = Landis.Extension.Succession.Landispro.PlugIn.gl_param.Stocking_y_value;
            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.stocking_z_value = Landis.Extension.Succession.Landispro.PlugIn.gl_param.Stocking_z_value;
            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.SuccessionTimeStep = Landis.Extension.Succession.Landispro.PlugIn.gl_param.SuccessionTimestep;
            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.TimeStepHarvest = Landis.Extension.Succession.Landispro.PlugIn.gl_param.TimeStepHarvest;
            Timestep = Landis.Extension.Succession.Landispro.PlugIn.gl_sites.TimeStepHarvest;

            Landis.Extension.Succession.Landispro.PlugIn.numSpecies = Landis.Extension.Succession.Landispro.PlugIn.gl_spe_Attrs.NumAttrs;

            Landis.Extension.Succession.Landispro.PlugIn.freq[5] = 1;

            Console.WriteLine("End Landis_pro Harvest Initialization...\n");
            Console.WriteLine("========================================\n");
        }

        public override void Run()
        {
            int i = modelCore.TimeSinceStart;

            if (i % Landis.Extension.Succession.Landispro.PlugIn.gl_sites.TimeStepHarvest == 0)
            {
                GlobalFunctions.HarvestPassCurrentDecade(i);
                for (int r = 1; r <= Landis.Extension.Succession.Landispro.PlugIn.snr; r++)
                {
                    for (int c = 1; c <= Landis.Extension.Succession.Landispro.PlugIn.snc; c++)
                    {
                        GlobalFunctions.setUpdateFlags(r, c);
                    }
                }
            }
            singularLandisIteration(i, Landis.Extension.Succession.Landispro.PlugIn.pPDP);

            Console.WriteLine("End Landis_pro Harvest Once");
            Console.WriteLine();
        }

        public void singularLandisIteration(int itr, Landis.Extension.Succession.Landispro.pdp ppdp)
        {
            DateTime ltime, ltimeTemp;
            TimeSpan ltimeDiff;

            string fptimeBU = Landis.Extension.Succession.Landispro.PlugIn.fpforTimeBU_name;
            using (StreamWriter fpforTimeBU = File.AppendText(Landis.Extension.Succession.Landispro.PlugIn.fpforTimeBU_name))
            {
                fpforTimeBU.WriteLine("\nProcessing succession at Year: {0}:", itr);

                if (itr % Landis.Extension.Succession.Landispro.PlugIn.gl_sites.TimeStepHarvest == 0)
                {
                    Console.WriteLine("\nProcessing harvest events.\n");
                    ltime = DateTime.Now;
                    Harvest.GlobalFunctions.HarvestprocessEvents(itr / Landis.Extension.Succession.Landispro.PlugIn.gl_sites.SuccessionTimeStep);  //Global Function
                    putHarvestOutput(itr / Landis.Extension.Succession.Landispro.PlugIn.gl_sites.TimeStepHarvest, Landis.Extension.Succession.Landispro.PlugIn.wAdfGeoTransform); //output img files
                    ltimeTemp = DateTime.Now;
                    ltimeDiff = ltimeTemp - ltime;
                    fpforTimeBU.WriteLine("Processing harvest: " + ltimeDiff + " seconds");
                }
            }
        }
    }
}