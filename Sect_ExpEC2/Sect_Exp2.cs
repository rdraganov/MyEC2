using System;
using System.Collections.Generic;
using Section_Explorer;

namespace Sect_ExpEC2
{
	public class SBsec
	{
		private Section B_sec;

		public SBsec (List<Point> _vtx)
		{
			B_sec= new Section(_vtx);
		}
	}
}

