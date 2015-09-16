using System; 
using System.Collections.Generic;
using Section_Explorer;
using Database_Mat;
using Sect_ExpEC2;

namespace MyEc2
{
	class MainClass
	{
		//Дефиниране на библиотеки


		//public Section_Explorer sExp=new Section_Explorer();

		public static void Main (string[] args)
		{
			
			List<Point> _crd = new List<Point>();
//			//_crd.Add(new Point (0, 0));
//			_crd.Add(new Point (16, 0));
//			_crd.Add(new Point (16, 29));
//			_crd.Add(new Point (7.5, 33));
//			_crd.Add(new Point (7.5, 105));
//			_crd.Add(new Point (37.5, 110));
//			_crd.Add(new Point (37.5, 125));
//			//_crd.Add(new Point (0, 125));
//			Section _sec1=new Section(_crd);
			Section _sec1=new Section(25,40);

			SBsec _sbs1 = new SBsec (_sec1.vertex,"C20/25");
			_sec1.AnalyseSec(256);

			_sbs1.AddArm (new armGroup (10, 5,new Stom("B500 B")));
			_sbs1.AddArm (new armGroup (10, 35,new Stom("B500 B")));
			//Тестове
			double F1 = 0, M1 = 0;
			double F2 = 0, M2 = 0;
			_sbs1.AnalyseStress (10, -3.5, _sec1.ddSec,Bet.bsx.bsx2,out F1,out M1);
			Console.WriteLine ("F bet = "+F1.ToString("N2")+"\n\n");
			Console.WriteLine ("M bet = "+M1.ToString("N2")+"\n\n");

			_sbs1.AnalyseStressArm (10, -3.5,Stom.ssx.ssx1, out F2,out M2);
			Console.WriteLine ("F arm = "+F2.ToString("N2")+"\n\n");
			Console.WriteLine ("M arm = "+M2.ToString("N2")+"\n\n");

			Console.WriteLine ("F tot = "+(F1+F2).ToString("N2")+"\n\n");
			Console.WriteLine ("M tot = "+(M1+M2).ToString("N2")+"\n\n");

			//Тестване
//			Console.WriteLine ("Stress sc = "+_sbs1.Mats.MBet.Stress(-1,false,2).ToString("N2")+"\n\n");
//
//			Console.WriteLine ("deform1 = "+_sbs1.B_sec.eps_xc(0,20,-3.5)+"\n\n");
//			Console.WriteLine ("deform2 = "+_sbs1.B_sec.eps_xs(85,20,18.375)+"\n\n");
//
//
//			Console.WriteLine ("Slicing at y = 35, upper part");
//			Section _sec2 = _sec1.slice (35, true);
//			Console.WriteLine ("\nArea: " + _sec2.area + 
//				"\nGravity center: " + _sec2.ycg.ToString ("N2")+
//				"\nMoment of inertia "+ _sec2.Imom.ToString("N0")+"\n\n");
//			
//
//			Console.WriteLine (_sec2.area);


		}
	}
}
