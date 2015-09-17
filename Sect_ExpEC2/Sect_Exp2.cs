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
		public Stom _st;

		public armGroup (double _a, double _y, Stom _s)
		{
			_area = _a;
			_ydis = _y;
			_st=_s;
		}


		public double Force (double _eps,bool _desgnSitu, Stom.ssx _typD )
		{
			return _st.Stress (_eps,_desgnSitu,_typD) * _area;
		}
	}


	public class SBsec:Section
	{
		//public static Section B_sec;
		public static Bet _bet ;
		public List<armGroup> armList=new List<armGroup>();

		public Bet abet
		{ get { return _bet; } }

		public SBsec (List<Point> _vtx, string _betS) : base(_vtx)
		{
			//B_sec= new Section(_vtx);
			_bet = new Bet (_betS);
		}
			

		public void AddArm(armGroup _arm)
		{
			armList.Add (_arm);
		}

		public void RemoveArm ()
		{
			armList.Clear();
		}

			
		public void AnalyseStress(double _ec, double _es, List<Point> _lst, Bet.bsx _typD,
						out double bF, out double bM)
		{
			double y=0;
			double _sig=0;
			double bForce = 0;
			double bMoment = 0;

			for (int i=0; i<_lst.Count; i++)
			{
				y=_lst[i].YCoord; //y координата на дискретната площ
				_sig=_bet.Stress(eps_cs(y,_ec,_es),true,_typD);
				bForce += _sig * _lst [i].XCoord/10;
				bMoment -=_sig*_lst[i].XCoord * (y - ycg)/1000;
			}
			//Console.WriteLine ("Force="+bForce.ToString("N2"));
			bF = bForce;
			bM = bMoment;		
		}

		public void AnalyseStressArm(double _ec, double _es, Stom.ssx _typD,
						out double aF, out double aM)
		{
			double _sig = 0;
			double aForce = 0, aMoment = 0;
			foreach (armGroup ar1 in armList) 
			{
				_sig = ar1._st.Stress (eps_cs (ar1._ydis, _ec, _es), true,_typD);
				aForce += _sig * ar1._area / 10;
				aMoment -= _sig * ar1._area * (ar1._ydis - ycg) / 1000;
			}
			aF = aForce;
			aM = aMoment;
		}
	

	}
}

