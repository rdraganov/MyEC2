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

		// Изчислява напрежението в армировката в зависимост от отн.деформация
		public double Stress(double _eps, bool _desgnSitu)
		{
			//_eps е относителното удължение в промили
			double _k=_st.f_t/_st.f_yk;
			double f = _st.f_yk;
			double _eps0 = 0;
			int znak = (_eps >= 0) ? 1 : -1;
//			int znak = 1;
//			if (_eps<0){ znak=-1;	_eps = -_eps;	}

			if (_desgnSitu) f /= _st.gama_s;

			_eps0 = f / _st.Es*1000;
			double grade = _st.f_yk * (_k - 1) / (_st.eps_uk - _eps0);
				
			if (_eps <= _eps0)
				return znak*_eps * _st.Es / 1000;
			else
				return znak*(f + grade * (_eps - _eps0));
		}

		//Изчислява силата в армировката: сила = площ * напрежение
		public double Force (double _eps,bool _desgnSitu)
		{
			return Stress (_eps,_desgnSitu) * _area;
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
			Console.WriteLine(Mats.MBet.f_ck);
			Console.WriteLine (Mats.MStom.Es);
			Console.WriteLine (Mats.MPStom.f_pk);

		}
	}
}

