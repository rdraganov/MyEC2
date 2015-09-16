using System;
using System.Collections.Generic;

namespace Section_Explorer
{
	public class Point
	{
		private double[] coordinates;
		public Point(double xCoord, double yCoord)
		{
			this.coordinates = new double[2];
			coordinates[0] = xCoord; // Initializing the x coordinate
			coordinates[1] = yCoord;// Initializing the y coordinate
		}
		public double XCoord
		{	get { return coordinates[0]; }
			set { coordinates[0] = value; }	
		}
		public double YCoord
		{	get { return coordinates[1]; }
			set { coordinates[1] = value; }
		}
	}

	/// <summary>
	/// Клас за геометрична фигура с вертикална ос на симетрия
	/// </summary>
	public class Section
	{
		private int _n = 0;
		private List<Point> _vertex = new List<Point>();
		private double _area;	// площ *** area
		private double _ycg; 	// у на центъра на тежестта   *** COG y-ordinate
		private double _Smom;   // статичен момент
		private double _Imom; 	// инерционен момент second moment of inertia x-x
		private double _maxY=0;	// максимална ордината *** max ordinate y
		private double _minY=0; // минимална ордината *** min ordinate y

		public double area  { get { return _area; } }
		public double minY	{ get { return _minY; } }
		public double maxY	{ get { return _maxY; } }
		public double ycg	{ get {return _ycg;	  } }
		public double Smom  { get {return _Smom;  } }
		public double Imom  { get {return _Imom;  } }
		public List<Point> vertex {get { return _vertex; }}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="_vtx">Координати на сечението отдясно на оста на симетрия</param>
		public Section(List<Point> _vtx)  //конструктор
		{
			_n = _vtx.Count;
			//_vertex = new Point[_n];
			_maxY = _vtx [0].YCoord;
			_minY = _maxY;
			//Ако първата точка не е на оста на симетрия, добавяме такава.
			if (Math.Abs(_vtx [0].XCoord)>1e-6) _vertex.Add(new Point (0, _vtx [0].YCoord));
			_vertex.AddRange (_vtx);
			for (int i = 0; i < _n; i++) 
			{
				_maxY = (_vtx [i].YCoord > _maxY) ? _vtx [i].YCoord : _maxY;
				_minY = (_vtx [i].YCoord < _minY) ? _vtx [i].YCoord : _minY;
			}
			// Ако последната точка не е на оста на симетрия, добавяме такава
			if (Math.Abs(_vtx [_vtx.Count-1].XCoord)>1e-6) _vertex.Add(new Point (0, _vtx [_vtx.Count-1].YCoord));
			_n = _vertex.Count;
			_area = calcArea();		// изчисляване на площта
			_Smom = calcSmom();
			_ycg = _Smom / _area; // център на тежестта
			_Imom = calcImom ()-_area*_ycg*_ycg;  //инерционен момент за главната ос х
		}

		public Section(double a, double b)
		{
			_n = 4;
			_minY = 0;
			_maxY = b;
			_vertex.Add (new Point ( 0, 0 ));
			_vertex.Add (new Point ( 0.5 * a, 0 ));
			_vertex.Add (new Point ( 0.5 * a, b ));
			_vertex.Add (new Point ( 0, b ));
			_area = a * b;
			_Smom = _area * b / 2;
			_ycg = b / 2;
			_Imom = a * b * b * b / 12;

		}

		private enum geoType{areaT=0,smomT,imomT};  //типове геометрични характеристики

		private double FGeom(Point p1, Point p2, geoType typ)	//формули за геометрични характеристики
		{
			switch (typ) 
			{
			case geoType.areaT:	{ return p1.XCoord*p2.YCoord-p2.XCoord*p1.YCoord;	}
			case geoType.smomT:	{ return (p1.XCoord * p2.YCoord - p2.XCoord * p1.YCoord) * (p1.YCoord + p2.YCoord)/3;}
			case geoType.imomT:	{ return (p1.XCoord * p2.YCoord - p2.XCoord * p1.YCoord) * (Math.Pow(p1.YCoord + p2.YCoord,2) - p1.YCoord * p2.YCoord)/6;	}
			default:{return 0;}
			}
				
		}

		private double calcGeom(geoType typ) //цикъл за обхождане
		{
			double _S = FGeom(_vertex [_n - 1], _vertex [0] , typ);
			for (int i = 0; i < _n-1; i++) 	_S+=FGeom(_vertex [i], _vertex [i+1],typ);
			return _S;
		}

		private double calcArea()	{ return Math.Abs(calcGeom(geoType.areaT));	} //площ
		private double calcSmom()	{ return calcGeom(geoType.smomT);				} //статичен момент
		private double calcImom ()	{ return calcGeom(geoType.imomT);				} //инерционен момент

		public Section slice(double _h, bool up) //отрязване на ниво _h, взема се горната част up=true или долната up=false
		{
			if ((_h <= _minY && up)||(_h >= _maxY && !up)) return new Section (_vertex);
			else 
			{
				List<Point> _nVertx = new List<Point> ();
				//Обработвам първия елемент
				if ((_vertex[0].YCoord>=_h && up)||(_vertex[0].YCoord<=_h && !up)) _nVertx.Add(_vertex[0]);
				else _nVertx.Add(new Point(_vertex[0].XCoord,_h));
				for (int i=1; i<_vertex.Count-1;i++)
				{
					if ((_vertex[i].YCoord>=_h && up)||(_vertex[i].YCoord<=_h && !up)) _nVertx.Add(_vertex[i]);
					if ((_h-_vertex[i].YCoord)*(_h-_vertex[i+1].YCoord)<0)
						{
							double _nX=_vertex[i].XCoord+
								(_vertex[i+1].XCoord-_vertex[i].XCoord)/
								(_vertex[i+1].YCoord-_vertex[i].YCoord)*
								(_h-_vertex[i].YCoord);
							_nVertx.Add(new Point(_nX,_h));
						}
							
				}
				int _nm = _vertex.Count - 1;
				//Обработвам последния елемент
				if ((_vertex[_nm].YCoord>=_h && up)||(_vertex[_nm].YCoord<=_h && !up)) _nVertx.Add(_vertex[_nm]);
				else _nVertx.Add(new Point(_vertex[_nm].XCoord,_h));

				return new Section (_nVertx);
			}
		}
		//изчисляване на деформация по нулева линия и скъсяване горен ръб
		public double eps_xc(double _yi,double _x, double _epsc)
		{
			return _epsc / _x * (_x - _maxY + _yi);
		}
		//изчисляване на деформация по нулева линия и удължаване долен ръб
		public double eps_xs(double _yi,double _x, double _epss)
		{
			return _epss /((_maxY-_minY)-_x) * (_maxY - _x - _yi);
		}

		public static List<Point> deltaSec = new List<Point> ();
		public List<Point> ddSec {get{return deltaSec;}}

		/// <summary> Конструктор на клас AnalyseSec </summary>
		/// <param name="_sec">Section</param>
		/// <param name="_n">брой части</param>
		public void AnalyseSec(int _n)
		{
			double _dy = (_maxY - _minY) / _n;
			Section _tmpSec;

			//test variables
			double _tmpA = 0, _tmpS = 0, _tmpI = 0;


			for (int i = 0; i < _n; i++) 
			{
				_tmpSec = slice (_maxY - ( i + 1 ) * _dy, true).slice(_maxY - i * _dy, false);
				deltaSec.Add (new Point(_tmpSec.area, _tmpSec.ycg));
				//test rows
				_tmpA += _tmpSec.area;
				_tmpS += _tmpSec.area * _tmpSec.ycg;
				_tmpI += _tmpSec.Imom+_tmpSec.area * _tmpSec.ycg * _tmpSec.ycg;

			}
			//test rows
			//Console.WriteLine ("Area analysed = " + _tmpA.ToString("N2")+"   Area real = "+_area.ToString("N2"));
			//Console.WriteLine ("Static moment analysed = " + _tmpS.ToString("N2")+"   Static moment real = "+_Smom.ToString("N2"));
			//_tmpI -= _tmpA * Math.Pow (_tmpS / _tmpA, 2);	
			//Console.WriteLine ("Moment II analysed = " + _tmpI.ToString("N2")+"   Moment II real = "+_Imom.ToString("N2"));
			//			foreach  (Point p_ in deltaSec)
			//			{
			//				Console.WriteLine("delta Area = "+p_.XCoord.ToString("N2")+"  ord y = "+p_.YCoord.ToString("N2"));
			//			}
		}


	}


}

