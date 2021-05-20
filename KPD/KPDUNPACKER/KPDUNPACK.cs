using System;
using System.IO;
using System.Collections.Generic;
using GIL.FUNCTION;
namespace KPDUNPACKER
{




    class KPDUNPACK 
    {

    	private byte[] magic;
    	private int version;
    	private long kpdSize;
    	private long paddingSize;
    	private long foldersOffset;
    	private long foldersSize;
    	
    	private long datasOffset;
    	private long datasSize;
    	public FOLDERUNPACK folder;
        public BR datas;
    	
    	
    	public KPDUNPACK(string kpdName)
    	{
            string outFolder = Path.GetDirectoryName(kpdName) + "\\"+Path.GetFileNameWithoutExtension(kpdName)+"_UNPACK";
    		FileStream fileStream = new FileStream(kpdName,FileMode.Open,FileAccess.Read);
    		BR reader = new BR(fileStream);
    		magic = reader.ReadBytes(4);
    		version = reader.ReadInt32();
    		kpdSize = reader.ReadInt64();
    		paddingSize = reader.ReadInt64();
    		foldersOffset = reader.ReadInt64();
    		foldersSize = reader.ReadInt64();
    		datasOffset = reader.ReadInt64();
    		datasSize = reader.ReadInt64();
    		reader.BaseStream.Seek(foldersOffset,SeekOrigin.Begin);
    		folder = new FOLDERUNPACK(new MemoryStream(reader.ReadBytes((int)foldersSize)),outFolder,paddingSize);
            reader.BaseStream.Seek(datasOffset, SeekOrigin.Begin);
            datas = new BR(new MemoryStream(reader.ReadBytes((int)datasSize)));
            Unpack();
            CreateToc(reader, kpdName);
            fileStream.Close();

    	}
        public void Unpack()
        {
            foreach (var node in folder.records)
            {
                if (node.folder != null)
                {
                    //null directory
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(node.file.name));
                    Console.WriteLine("Extract : {0}", node.file.name);
                    FileStream outputFile = new FileStream(node.file.name, FileMode.Create, FileAccess.Write);
                    outputFile.Write(datas.GetBytes(node.file.refOffset + node.file.offset, (int)node.file.size), 0, (int)node.file.size);
                    outputFile.Flush();
                    outputFile.Close();
                    if(Path.GetExtension(node.file.name) == ".kpd")
                    {
                        KPDUNPACK kpdLite = new KPDUNPACK(node.file.name);

                    }
                }
            }
        }
        public void CreateToc(BR reader,string tocName)
        {

            FileStream fileStream = new FileStream(tocName+".toc", FileMode.Create, FileAccess.Write);
            fileStream.Write(reader.GetBytes(0, (int)datasOffset), 0, (int)datasOffset);
            fileStream.Flush();
            fileStream.Close();


        }

           
    }
}