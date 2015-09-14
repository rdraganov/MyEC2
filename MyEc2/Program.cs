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
			//_crd.Add(new Point (0, 0));
			_crd.Add(new Point (16, 0));
			_crd.Add(new Point (16, 29));
			_crd.Add(new Point (7.5, 33));
			_crd.Add(new Point (7.5, 105));
			_crd.Add(new Point (37.5, 110));
			_crd.Add(new Point (37.5, 125));
			//_crd.Add(new Point (0, 130));
	
				
			Section _sec1=new Section(_crd);
			SBsec _sbs1 = new SBsec (_crd);
			_sbs1.AddArm (new armGroup (2000, 50,new Stom("B500 B")));
			_sbs1.AddArm (new armGroup (1000, 120,new Stom("B500 B")));
			Console.WriteLine ("\nArea: " + _sec1.area + 
				"\nGravity center: " + _sec1.ycg.ToString ("N2")+
				"\nMoment of inertia "+ _sec1.Imom.ToString("N0")+"\n\n");

			Console.WriteLine ("Slicing at y=35, upper part");
			Section _sec2 = _sec1.slice (35, true);
			Console.WriteLine ("\nArea: " + _sec2.area + 
				"\nGravity center: " + _sec2.ycg.ToString ("N2")+
				"\nMoment of inertia "+ _sec2.Imom.ToString("N0")+"\n\n");
			

			Console.WriteLine (_sec2.area);


		}
	}
}
