using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace KPDUNPACKER
{

    class FILE
	{
		public int infosOffset { get; set; }
		public int index{get;set;}
		public long refOffset { get; set; }
		public string name{get;set;}
		public long offset{get;set;}
		public long size{get;set;}
	}



}