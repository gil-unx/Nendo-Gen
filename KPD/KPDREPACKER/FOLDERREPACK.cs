using System;
using System.IO;
using System.Collections.Generic;
using GIL.FUNCTION;

namespace KPDREPACKER
{
    class FOLDERREPACK
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
		private BinaryReader reader;
		public MemoryStream datasBuff = new MemoryStream();
		public BW datasWriter;
		public  FOLDERREPACK(MemoryStream buff, string parent)
		{
			folderName = parent;
			reader = new BinaryReader(buff);
			location = reader.ReadInt32();
			infosOffset = reader.ReadInt32();
			infosSize = reader.ReadInt64();
			folderOffset = reader.ReadInt64();
			nool = reader.ReadInt64();
			datasOffset = reader.ReadInt64();
			datasSize = reader.ReadInt64();
			count = reader.ReadInt16();
			unk = reader.ReadBytes((int)infosOffset - 0x32);
			infos = new INFOS(new MemoryStream(reader.ReadBytes((int)infosSize)), count);
			
		}

		public byte[] Repack(long dataOffset,long padding)
        {
			MemoryStream buff = new MemoryStream();
			BW folderWriter = new BW(buff);
			datasWriter = new BW(datasBuff);
			List<byte[]> newFolders = new List<byte[]>();
			long chunkDataOffset = dataOffset;
			for (int i = 0; i < count; i++)
			{
				switch (infos.locations[i])
				{
					case 0:
						chunkDataOffset = datasWriter.BaseStream.Position;
						FOLDERREPACK folder = new FOLDERREPACK(new MemoryStream(reader.ReadBytes((int)infos.sizes[i])), folderName + "/" + infos.names[i]);
						newFolders.Add(folder.Repack(chunkDataOffset+dataOffset,padding));
						datasWriter.Write(folder.datasBuff.ToArray());
						chunkDataOffset += folder.datasBuff.Length;
						break;
					case 1:
						if(Path.GetExtension(infos.names[i]) == ".kpd")
                        {
							KPDREPACK kpdLite = new KPDREPACK(folderName + "/" + infos.names[i]+".toc");
                        }
						FileStream fileStream = new FileStream(folderName + "/" + infos.names[i], FileMode.Open, FileAccess.Read);
                        Console.WriteLine(folderName + "/" + infos.names[i]);
						infos.sizes[i] = fileStream.Length;
						infos.offsets[i] = datasWriter.BaseStream.Position;
						byte[] fileData = new byte[fileStream.Length];
						fileStream.Read(fileData, 0, (int)fileData.Length);
						datasWriter.Write(fileData);
						datasWriter.WritePadding((int)padding, 0);//padding
						break;
					default:
						Console.WriteLine("exception");
						Console.ReadKey();
						break;
				}
			}
			folderWriter.WritePadding(0x800,0);
			folderWriter.Write(location);
			folderWriter.Write(infosOffset);
			folderWriter.Write(infosSize);
			folderWriter.Write(folderOffset);
			folderWriter.Write(nool);
			folderWriter.Write(dataOffset);//new
			folderWriter.Write(datasWriter.BaseStream.Position);//new
			folderWriter.Write(count);
			folderWriter.Write(unk);
			folderWriter.Write(infos.GETNEWINFOS(padding));
			foreach (byte[] newFolder in newFolders)
            {
				folderWriter.Write(newFolder);
            }
			return buff.ToArray();
		}
	}
}
