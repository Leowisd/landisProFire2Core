using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landis.Core;
using System.Diagnostics;
using Landis.SpatialModeling;
using System.IO;



namespace Landis.Extension.Landispro.Fire
{
    class PlugIn
        : ExtensionMain
    {
        public static readonly ExtensionType ExtType = new ExtensionType("disturbance:fire");
        public static readonly string ExtensionName = "Landis_pro Fire";


        private static ICore modelCore;

        public PlugIn()
            : base(ExtensionName, ExtType)
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
           

            Console.WriteLine("FIRE Dll loaded in...");
            Console.WriteLine();

            //Todo
            

            Console.WriteLine("End Landis_pro FIRE Parameters Loading\n");
            Console.WriteLine("========================================\n");
        }

        public override void Initialize()
        {
            Console.WriteLine("Running Landis_pro FIRE Initialization...");
            Console.WriteLine();

            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.stocking_x_value = Landis.Extension.Succession.Landispro.PlugIn.gl_param.Stocking_x_value;
            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.stocking_y_value = Landis.Extension.Succession.Landispro.PlugIn.gl_param.Stocking_y_value;
            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.stocking_z_value = Landis.Extension.Succession.Landispro.PlugIn.gl_param.Stocking_z_value;
            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.SuccessionTimeStep = Landis.Extension.Succession.Landispro.PlugIn.gl_param.SuccessionTimestep;
            // FIRE TIMESTEP
            Landis.Extension.Succession.Landispro.PlugIn.gl_sites.TimeStepFire = Landis.Extension.Succession.Landispro.PlugIn.gl_param.TimeStepFire;
            Timestep = Landis.Extension.Succession.Landispro.PlugIn.gl_sites.TimeStepFire;

            Landis.Extension.Succession.Landispro.PlugIn.numSpecies = Landis.Extension.Succession.Landispro.PlugIn.gl_spe_Attrs.NumAttrs;

            Console.WriteLine("End Landis_pro FIRE Initialization...\n");
            Console.WriteLine("========================================\n");
        }

        public override void Run()
        {
            int i = modelCore.TimeSinceStart;

            if (i % Landis.Extension.Succession.Landispro.PlugIn.gl_sites.TimeStepHarvest == 0)
            {
                //Todo
            }
            singularLandisIteration(i, Landis.Extension.Succession.Landispro.PlugIn.pPDP);

            Console.WriteLine("End Landis_pro FIRE Once");
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

                //Todo
            }
        }
    }
}