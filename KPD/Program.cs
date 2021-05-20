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
            //KPDUNPACK kpd = new KPDUNPACK("DATAPACK.KPD");
            KPDREPACK kpd = new KPDREPACK("DATAPACK.KPD.toc");
            
        }

    }
    
}
