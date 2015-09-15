using System;
using System.Collections.Generic;

namespace Section_Explorer
{
	public class Section_Integra
	{
		private double _betForce=0;
		public double betForce{get{return _betForce;}}
		private double _betY = 0;
		public double betY{get{return _betY;}}

		public Section_Integra (List<Point> L1, List<double> _sig)
		{
			double _betS = 0;
			for (int i=0; i<L1.Count; i++)
			{
				_betForce += L1[i].XCoord * _sig[i];
				_betS += L1 [i].XCoord * L1 [i].YCoord * _sig [i];
			}
			_betY = _betS / _betForce;
		}
	}


}

