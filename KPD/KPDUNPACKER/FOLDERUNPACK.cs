using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace KPDUNPACKER
{

    class FOLDERUNPACK
	{
		public string folderName;
		public int location;
		public int infosOffset;
		public long infosSize;
		public long folderOffset;
		public long nool;
		public long datasOffset;
		public long datasSize;
		public short count;
		public byte[] unk;
		public INFOS infos;
		
		
		public List<RECORDUNPACK> records = new List<RECORDUNPACK>();
		
		public FOLDERUNPACK(MemoryStream buff, string parent, long padding)
		{
			
			folderName = parent;
			BinaryReader reader = new BinaryReader(buff);
			location = reader.ReadInt32();
			infosOffset = reader.ReadInt32();
			infosSize = reader.ReadInt64();
			folderOffset = reader.ReadInt64();
			nool = reader.ReadInt64();
			datasOffset = reader.ReadInt64();
			datasSize = reader.ReadInt64();
			count = reader.ReadInt16();
			unk = reader.ReadBytes((int)infosOffset - 0x32);
			infos = new INFOS(new MemoryStream(reader.ReadBytes((int)infosSize)),count);
			for(int i = 0 ;i < count;i++)
			{
				RECORDUNPACK record = new RECORDUNPACK();
				switch(infos.locations[i])
				{
					case 0:
						FOLDERUNPACK folder = new FOLDERUNPACK(new MemoryStream(reader.ReadBytes((int)infos.sizes[i])),folderName+"/"+infos.names[i],padding);
						record.folder = folder;
						foreach(var chunk in record.folder.records)
						{
							records.Add(chunk);
						}
						records.Add(record);


						break;
					case 1:
						FILE file = new FILE();
						file.infosOffset = infosOffset;
						file.index = i;
						file.refOffset = datasOffset;
						file.name = folderName + "/"+infos.names[i];
						file.offset = infos.offsets[i];
						file.size = infos.sizes[i];
						record.file = file;
						records.Add(record);
						break;
					default:
					Console.WriteLine("exception");
					Console.ReadLine();
						break;
					
					
					
					
				}
			}
			
		}

		

	}
}