using System;
using System.Collections.Generic;
using Section_Explorer;
using Database_Mat;
using Sect_ExpEC2;

namespace MyEc2
{
	public class Interact
	{
		//private double delt1 = 0, delt2=0;
		private double eps = 0, epc = 0, x=0, pts=16;
		private enum nx{ix=0,iec,ies,iM,iN};
		private string[] dp1;
		private List<string[]> l1 =new List<string[]>();
		SBsec _sbs1;
		private double F1 = 0, M1 = 0, F2 = 0, M2 = 0;
		private double h;
		double maxM=0, minM=0;

		public Interact (SBsec sbs1)
		{
			_sbs1 = sbs1;
			double epsud=_sbs1.armList [1]._st.eps_uk * 0.9;
			double epssd=_sbs1.armList [1]._st.f_yk/sbs1.armList [1]._st.Es/1.15*1000;
			double epsmin = _sbs1.abet.eps_cu3;
			double epsc2 = _sbs1.abet.eps_c2;
			Console.WriteLine ("epsud = " + epsud + "   epssd = " + epssd);
			eps = epsud;
			epc = epsud;
			h = _sbs1.maxY - _sbs1.minY;

			FLoop (-epsud / pts,0);   						//долу опън на мах, горе опън от мах до нула
			FLoop (epsmin / pts,0); 						//долу опън на мах, горе натиск от нула до мах
			pts=64;
			FLoop (0,- (epsud) / pts);

			//FLoop (0,- (epssd) / pts); 				// долу опън от мах до нула, горе ръб натиск на мах
			pts=16;
			FLoop (-(epc - epsc2) / pts,(eps +epsc2) / pts);// долу натиск от нула до 2, горе от мах до 2
			FLoop (-epc / pts, -(eps-epsmin) / pts); 		// горе натиск от 2 до нула, долу натиск от 2 до мах
			pts=64;
			FLoop (epsud / pts,0); 						// горе се опъва от нула до мах, долу натиск на мах
			pts=16;
			FLoop (0,-epsmin/pts); 							// горе опън на мах, долу натиск до нула
			FLoop (0,epsud / pts);							// горе опън на мах, долу расте от нула до опън на мах
			if (savea ()) Console.WriteLine("Запис - готово");
			Console.WriteLine("maxM = "+maxM+"  minM = "+minM);
		}

		public void FLoop(double d1, double d2)
		{
			for (int i = 0; i < pts+1; i++, epc+=d1,eps+=d2) {
				_sbs1.AnalyseStress (epc, eps, _sbs1.ddSec,Bet.bsx.bsx2,out F1,out M1);
				_sbs1.AnalyseStressArm (epc, eps, Stom.ssx.ssx1, out F2,out M2);
				if (eps == epc)
					x = 1E+6;
				else if (epc < eps)
					x = h * ( -epc / (eps - epc));
				else x = h * (-eps / (epc - eps));
				dp1 = new string[5];
				dp1 [(int)nx.ix] = x.ToString("F2"); 
				dp1 [(int)nx.iec] = epc.ToString("F2");
				dp1 [(int)nx.ies] = eps.ToString("F2");
				dp1 [(int)nx.iM] = (M1+M2).ToString("F2");
				dp1 [(int)nx.iN] =(-0.1*(F1+F2)).ToString("F2");
				l1.Add (dp1);

				if (maxM < M1 + M2)
					maxM = M1 + M2;
				if (minM > M1 + M2)
					minM = M1 + M2;


			}
			epc -= d1;eps -= d2;
			string[] dp0=new string[]{"","","","0","0"};
			l1.Add (dp0);
			//Console.WriteLine ("epc = " + epc.ToString ("N3") + "   eps = " + eps.ToString ("N3"));

		}

		public bool savea()
		{
			string fileN=System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.txt");
			string txt="", txtall="";

			for ( int i = 0 ; i < l1.Count; i++ )
			{
				//txt=string.Join(",",l1[i]);
				txt=l1[i][3]+","+l1[i][4];
				txtall+=txt+"\r\n";
				txt = "";
			}
			try{
				System.IO.File.WriteAllText(fileN,txtall);
				System.Diagnostics.Process.Start(fileN);
				return true;
			}catch{
				return false;
			}
		}
	}
}

