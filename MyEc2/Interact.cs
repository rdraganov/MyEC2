﻿using System;
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

		public Interact (SBsec sbs1)
		{
			_sbs1 = sbs1;
			eps = _sbs1.armList [1]._st.eps_uk * 0.9;
			epc = eps;
			h = _sbs1.maxY - _sbs1.minY;

			FLoop (-eps / pts,0);
			epc = 0;
			FLoop (_sbs1.abet.eps_cu3 / pts,0);
			FLoop (0,-eps / pts);
			FLoop (-(epc - _sbs1.abet.eps_c2) / pts,(eps +_sbs1.abet.eps_c2) / pts);
			FLoop (-epc / pts, -(eps-_sbs1.abet.eps_cu3) / pts);
			FLoop (_sbs1.armList [1]._st.eps_uk * 0.9 / pts,0);
			FLoop (0,-eps/pts);
			FLoop (0,_sbs1.armList [1]._st.eps_uk * 0.9 / pts);
			if (savea ()) Console.WriteLine("Запис - готово");
		}

		public void FLoop(double d1, double d2)
		{
			for (int i = 0; i < pts; i++, epc+=d1,eps+=d2) {
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

				//Console.WriteLine(x.ToString("N2").PadRight(15)+(M1+M2).ToString("N2").PadRight(15)+
				//				 (F1+F2).ToString("N2").PadRight(12));
			}
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

