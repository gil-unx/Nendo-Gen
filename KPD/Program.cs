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
            //KPDUNPACK kpd = new KPDUNPACK("C:\\Users\\Administrator\\Desktop\\NENDOROID\\DATAPACK.KPD");
            KPDREPACK kpd = new KPDREPACK("C:\\Users\\Administrator\\Desktop\\NENDOROID\\DATAPACK.KPD.toc");
            //KPDREPACK kpd = new KPDREPACK("C:\\Users\\Administrator\\Desktop\\NENDOROID\\DATAPACK_UNPACK\\pkdata\\dance.kpd.toc");

        }

    }
    
}
