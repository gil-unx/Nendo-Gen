using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using KPDREPACKER;
using KPDUNPACKER;
namespace NENDOROID
{
    class Program
    {
        static void Main(string[] args)
        {
            string mode = "";
            string input = "";
            Console.WriteLine("KPD unpacker by Gil_Unx");
            Console.WriteLine("------------------------------------");
            if (args.Length < 2) { Console.WriteLine("ERROR: Argument tidak spesifik\n\nExtract: KPDEX -x <*.KPD>\n\nRepack kpd: KPDEX -c <*.KPD.toc>"); return; }
            mode = args[0];
            input = args[1];
            if (mode == "-x")
            {
                KPDUNPACK kpd = new KPDUNPACK(input);
            }
            if (mode == "-c")
            {
                KPDREPACK kpd = new KPDREPACK(input);
            }

        }

    }
    
}
