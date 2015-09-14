using System;
using System.Collections.Generic;
using Section_Explorer;
using Database_Mat;

namespace Sect_ExpEC2
{
	public class armGroup
	{
		public double _area = 0;
		public double _ydis = 0;
		private Stom _st;

		public armGroup (double _a, double _y, Stom _s)
		{
			_area = _a;
			_ydis = _y;
			_st=_s;
		}

		//Изчислява силата в армировката: сила = площ * напрежение
		public double Force (double _eps,bool _desgnSitu)
		{
			return _st.Stress (_eps,_desgnSitu) * _area;
		}
	}

	public class SBsec
	{
		private Section B_sec;
		public DataBa Mats ;
		public List<armGroup> armList=new List<armGroup>();

		public SBsec (List<Point> _vtx)
		{
			B_sec= new Section(_vtx);
			Mats = new DataBa ("C20/25", "B500 B", "Y1860S7");
			test ();
		}

		public void AddArm(armGroup _arm)
		{
			armList.Add (_arm);
		}

		public void test()
		{
			Console.WriteLine("f_ck="+Mats.MBet.f_ck);
			Console.WriteLine ("Es="+Mats.MStom.Es);
			Console.WriteLine ("f_pk="+Mats.MPStom.f_pk);

		}
	}
}

