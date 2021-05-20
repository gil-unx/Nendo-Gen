using System.IO;
using GIL.FUNCTION;
namespace KPDREPACKER
{
    class KPDREPACK
    {

        public byte[] magic;
        public int version;
        public long kpdSize;
        public long paddingSize;
        public long foldersOffset;
        public long foldersSize;
        public long datasOffset;
        public long datasSize;
        public FOLDERREPACK folder;


        public KPDREPACK(string tocName)
        {
            string kpdName = Path.GetDirectoryName(tocName) + "\\" + Path.GetFileNameWithoutExtension(tocName);
            string outFolder = Path.GetDirectoryName(tocName) + "\\" + Path.GetFileNameWithoutExtension(kpdName)+ "_UNPACK";
            FileStream fileStream = new FileStream(tocName, FileMode.Open, FileAccess.Read);
            FileStream outStream = new FileStream(kpdName, FileMode.Create, FileAccess.Write);
            BinaryReader toc = new BinaryReader(fileStream);
            BW writer = new BW(outStream);
            magic = toc.ReadBytes(4);
            version = toc.ReadInt32();
            kpdSize = toc.ReadInt64();
            paddingSize = toc.ReadInt64();
            foldersOffset = toc.ReadInt64();
            foldersSize = toc.ReadInt64();
            datasOffset = toc.ReadInt64();
            datasSize = toc.ReadInt64();
            toc.BaseStream.Seek(foldersOffset, SeekOrigin.Begin);// padd
            folder = new FOLDERREPACK(new MemoryStream(toc.ReadBytes((int)foldersSize)), outFolder);//dataoffset
            byte[] newFolders = folder.Repack(0,paddingSize);
            byte[] newDatas = folder.datasBuff.ToArray();
            writer.Write(magic);
            writer.Write(version);
            writer.Write(newDatas.Length+ datasOffset);//new
            writer.Write(paddingSize);
            writer.Write(foldersOffset);
            writer.Write(foldersSize);
            writer.Write(datasOffset);
            writer.Write(folder.datasBuff.Length);//new
            writer.WritePadding((int)foldersOffset, 0);
            writer.Write(newFolders);
            writer.WritePadding((int)paddingSize, 0);
            writer.Write(newDatas);
            writer.Flush();
            writer.Close();
            System.Console.WriteLine(kpdName);











        }

    }
}