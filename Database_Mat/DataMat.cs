using System;
using System.Collections.Generic;

namespace Database_Mat
{
	public class DataBa
	{
		//Глобални променливи
		private static Bet MyBet;
		private static Stom MyStom;
		private static PStom MyPStom;

		//Конструктор
		public DataBa(string bc_, string ac_,string pac_)
		{
			MyBet = new Bet (bc_);
			MyStom = new Stom (ac_);
			MyPStom = new PStom(pac_);
		}

		public Bet MBet {get {return MyBet;}}
		public Stom MStom { get { return MyStom; } }
		public PStom MPStom { get { return MyPStom; } }
	}

	public class Bet
	{

	   public static List<string> _bclassL = new List<string> {"C12/15","C16/20","C20/25","C25/30","C30/37","C35/45","C40/50","C45/55","C50/60"};
		private double[,] b_data=new double[2,9] 
			{{12,16,20,25,30,35,40,45,50},
			{15,20,25,30,37,45,50,55,60}};
		
		public enum bsx{bsx1=1,bsx2=2,bsx3=3,bsx4=4}; //възможни диаграми напрежение-деформация

		private string _bcl="";
		private double zf_ck=0, zf_ck_cube=0, zf_cm=0, zf_ctm=0, zf_ctk05=0, zf_ctk95=0;
		private double zEcm=0;
		private double eeps_c1=0, eeps_cu1=-3.5;
		private double eeps_c2=-2.0, eeps_cu2=-3.5;
		private double eeps_c3=-1.75, eeps_cu3=-3.5, nn=2.0;

		public double gama_c=1.5;
		public double f_ck 		{get {return zf_ck;}}
		public double f_ck_cube {get {return zf_ck_cube;}}
		public double f_cm 		{get {return zf_cm;}}
		public double f_ctm 	{get {return zf_ctm;}}
		public double f_ctk05 	{get {return zf_ctk05;} }
		public double f_ctk95 	{get {return zf_ctk95;} }

		public double Ecm { get { return zEcm; } set { zEcm = value; } }

		public double eps_c1 	{get {return eeps_c1;} }
		public double eps_cu1 	{get {return eeps_cu1;}}
		public double eps_c2 	{get {return eeps_c2;} }
		public double eps_cu2 	{get {return eeps_cu2;}}
		public double eps_c3 	{get {return eeps_c3;} }
		public double eps_cu3 	{get {return eeps_cu3;}}
		public double n_ 		{get {return nn;}   }

		public Bet (string _bclass)
		{
			_bcl = _bclass;
			int nm =_bclassL.IndexOf (_bcl);
			zf_ck = b_data [0, nm];
			zf_ck_cube = b_data [1, nm];
			zf_cm = f_ck + 8;
			zf_ctm = 0.3 * Math.Pow (f_ck, 2.0 / 3.0);
			zf_ctk05 = 0.7 * f_ctm;
			zf_ctk05 = 1.3 * f_ctm;
			zEcm = 22 * Math.Pow (f_cm / 10, 0.3);
			eeps_c1 = 0.7 * Math.Pow (f_cm, 0.31);
		}

		//Изчисляване на напрежението в бетона при зададено отн.деформация
		public double Stress (double _eps, bool _designSitu, Bet.bsx _typDia)
		{
			double tempF = 0;
			if (_eps > 0) return 0;
			switch (_typDia) {
			//Параболично-линейна диаграма
			case bsx.bsx1:
				{
					//if _eps>
					double eta = _eps / eeps_c1;
					double k = 1.05 * zEcm * eeps_c1 / zf_cm;
					tempF = (k * eta - eta * eta) / (1 + (k - 2) * eta);
					break;
				}

			case bsx.bsx2:
				{
					if (_eps < eeps_c2)	tempF = -zf_ck;
					else tempF = -zf_ck*(1-Math.Pow(1-_eps/eeps_c2,nn));
					break;
				}
			//Билинейна диаграма
			case bsx.bsx3:
				{
					if (_eps < eeps_c3)	tempF = - zf_ck;
					else tempF = -zf_ck*(_eps/eeps_c3);
					break;
				}
			//Правоъгълна диаграма, като се взема 0,8 от нея.
			case bsx.bsx4:
				{
					if (_eps < 0.2 * eeps_cu3)
						tempF = -zf_ck;
					else
						tempF = 0;
					break;
				}
			default: return 0;
			}
			//Изчислителна ситуация
			if (_designSitu) tempF/=gama_c;
			return tempF;
		}
	}

	//Клас за характеристики на обикновената армировка
	public class Stom
	{
		private string _scl="";

		public enum ssx{ssx1=1,ssx2=2}; //възможни диаграми напрежение-деформация

		public static List<string> _sclassL = new List<string> {"B235","B420 B","B420 C","B500 A","B500 B","B500 C"};
		private double[,] s_data=new double[3,6] {{235,420,420,500,500,500},
												  {370,460,500,550,550,575},
												  {25,50,80,25,50,75}};
		private double zf_yk=0, zf_t=0;   //табл.4 БДС 9252:2007 табл.2 БДС 4758:2008
		private double zEs=200000;
		private double eeps_uk = 0;
		//private double eeps_ud = 0;

		public double gama_s=1.15;

		public double f_yk 	{get {return zf_yk;}  }
		public double f_t	{get {return zf_t;}		}
		public double Es 	{get {return zEs;} 		}
		public double eps_uk{get {return eeps_uk;}	} 
		public string st_class { get { return _scl; } }

		public Stom(string _sclass)
		{
			_scl = _sclass;
			int nm = _sclassL.IndexOf (_scl);
			zf_yk = s_data [0, nm];
			zf_t = s_data [1, nm];
			eeps_uk = s_data [2, nm];
		}

		//Изчисляване на напрежението в амрировката при зададено отн.деформация
		public double Stress(double _eps, bool _desgnSitu, ssx _typDia)
		{
			//_eps е относителното удължение в промили
			if (!_desgnSitu && _eps>eeps_uk || _desgnSitu && _eps>0.9*eeps_uk) return 0;
			double _k=zf_t/zf_yk;
			double f = zf_yk;
			double _eps0 = 0;
			int znak = (_eps >= 0) ? 1 : -1;
			_eps = Math.Abs (_eps);
			if (_desgnSitu) f /= gama_s;

			_eps0 = f / zEs*1000;
			double grade = 0;
			if (_typDia == ssx.ssx2) grade = zf_yk * (_k - 1) / (eeps_uk - _eps0);
			if (_eps <= _eps0)
				return znak*_eps * zEs / 1000;
			else
				return znak*(f + grade * (_eps - _eps0));
		}

	}

	//Клас за характеристики на напрегнатата армировка
	public class PStom
	{
		public static List<string> _sPclassL = new List<string> { "Y1860S7" };
		private string _spcl = "";
		// private double[,] sp_data = new double[2, 2] { { 1, 1 }, { 1, 1 } };
		private static double zf_pk = 1860;
		private static double zf_pk01 = zf_pk / 1.1;
		private static double _Esp=190000;

		public double gama_sp=1.15;
		public double f_pk 	 { get { return zf_pk; } }
		public double f_pk01 { get { return zf_pk01; } }
		public double Esp 	 { get { return _Esp; } }

		public PStom(string _spclass)
		{
			_spcl=_spclass;

		}
	}

}

