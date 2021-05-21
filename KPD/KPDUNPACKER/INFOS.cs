using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using GIL.FUNCTION;
using System.Text;

namespace KPDUNPACKER
{

    class INFOS
	{
		public List<long> locations = new List<long>();
		public List<long> offsets = new List<long>();
		public List<long> sizes  = new List<long>();
		public List<short>  unk = new List<short>();
		public List<string>  names  = new List<string>();
		private int count;
		
		public INFOS(MemoryStream memoryStream,int c)
		{
			count = c;
			BR reader = new BR(memoryStream);
			for(int i=0;i<count;i++)
			{
				locations.Add(reader.ReadInt64());
				offsets.Add(reader.ReadInt64());
				sizes.Add(reader.ReadInt64());
				unk.Add(reader.ReadInt16());
				string name = reader.GetUtf8();
				names.Add(name);
				reader.ReadPadding(0x50);
				
				
			
			}
			
			
		}

		public byte[] GETNEWINFOS(long padding)
		{
			MemoryStream memoryStream = new MemoryStream();
			BW writer = new BW(memoryStream);
			for (int i = 0; i < count; i++)
			{
				writer.Write(locations[i]);
				writer.Write(offsets[i]);
				writer.Write(sizes[i]);
				writer.Write(unk[i]);
				writer.Write(Encoding.UTF8.GetBytes(names[i]));
				writer.WritePadding(0x50,0);



			}
			writer.WritePadding((int)padding, 0);
			return memoryStream.ToArray();
		}
	}
	
}