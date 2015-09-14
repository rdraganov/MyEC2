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

	public class Section
	{
		private int _n = 0;
		private List<Point> _vertex = new List<Point>();
		private double _area;	// площ *** area
		private double _ycg; 	// у на центъра на тежестта   *** COG y-ordinate
		private double _Imom; 	// инерционен момент second moment of inertia x-x
		private double _maxY=0;	// максимална ордината *** max ordinate y
		private double _minY=0; // минимална ордината *** min ordinate y

		public double area  { get { return _area; } }
		public double minY	{ get { return _minY; } }
		public double maxY	{ get { return _maxY; } }
		public double ycg	{ get {return _ycg;	  } }
		public double Imom  { get {return _Imom;  } }


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
			_ycg = calcSmom() / _area; // център на тежестта
			_Imom = calcImom ()-_area*_ycg*_ycg;  //инерционен момент за главната ос х
		}

		private enum geoType{areaT=0,smomT,imomT};  //типове геометрични характеристики

		private double FGeom(Point p1, Point p2, int typ)	//формули за геометрични характеристики
		{
			switch ((int)typ) 
			{
			case 0:	{ return p1.XCoord*p2.YCoord-p2.XCoord*p1.YCoord;	}
			case 1:	{ return (p1.XCoord * p2.YCoord - p2.XCoord * p1.YCoord) * (p1.YCoord + p2.YCoord)/3;}
			case 2:	{ return (p1.XCoord * p2.YCoord - p2.XCoord * p1.YCoord) * (Math.Pow(p1.YCoord + p2.YCoord,2) - p1.YCoord * p2.YCoord)/6;	}
			default:{return 0;}
			}
				
		}

		private double calcGeom(int typ) //цикъл за обхождане
		{
			double _S = FGeom(_vertex [_n - 1], _vertex [0] , typ);
			for (int i = 0; i < _n-1; i++) 	_S+=FGeom(_vertex [i], _vertex [i+1],typ);
			return _S;
		}

		private double calcArea()	{ return Math.Abs(calcGeom((int)geoType.areaT));	} //площ
		private double calcSmom()	{ return calcGeom((int)geoType.smomT);				} //статичен момент
		private double calcImom ()	{ return calcGeom((int)geoType.imomT);				} //инерционен момент

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
	}
}

