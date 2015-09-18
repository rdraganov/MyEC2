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
			Section _sec1=new Section(25,50);

			SBsec _sbs1 = new SBsec (_sec1.vertex,"C25/30");
			_sec1.AnalyseSec(256);

			_sbs1.AddArm (new armGroup (0, 5,new Stom("B500 B")));
			_sbs1.AddArm (new armGroup (20, 45,new Stom("B500 B")));

			//Тестове

			Interact in1=new Interact (_sbs1);
		
		}
	}
}
