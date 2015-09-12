using System;
using System.Collections.Generic;

namespace Database_Mat
{
	public class DataBa
	{
		public static string Bet_Class="";
		public static string Arm_Class="";
		public static string Parm_Class="";
		public static List<string> _bclassL = new List<string> {"C12/15","C16/20","C20/25","C25/30","C30/37","C35/45","C40/50","C45/55","C50/60"};
		public static List<string> _sclassL = new List<string> {"B235","B420 B","B420 C","B500 A","B500 B","B500 C"};
		public static List<string> _sPclassL = new List<string> { "Y1860S7" };

		public Bet MyBet;
		public Stom MyStom;
		public PStom MyPStom;


		public DataBa(string bc_, string ac_,string pac_)
		{
			MyBet=new Bet (bc_);
			MyStom = new Stom (ac_);
			MyPStom=new PStom(pac_);
		}

		public class Bet
		{

			private double[,] b_data=new double[2,9] 
				{{12,16,20,25,30,35,40,45,50},
				{15,20,25,30,37,45,50,55,60}};

			private string _bcl="";
			private double zf_ck=0, zf_ck_cube=0, zf_cm=0, zf_ctm=0, zf_ctk05=0, zf_ctk95=0;
			private double zEcm=0;
			private double eeps_c1=0, eeps_cu1=3.5;
			private double eeps_c2=2.0, eeps_cu2=3.5;
			private double eeps_c3=1.75, eeps_cu3=3.5, nn=2.0;

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
				//тук трябва да се прочетат данните и да се заредят променливите

			}
		}

		public class Stom
		{
			private string _scl="";
		
			private double[,] s_data=new double[3,6] {{235,420,420,500,500,500},
													  {370,460,500,550,550,575},
													  {2.5,5.0,8.0,82.5,5.0,7.5}};
			private double zf_yk=0, zf_t=0;   //табл.4 БДС 9252:2007 табл.2 БДС 4758:2008
			private double zEs=0;
			private double eeps_uk = 0;
			//private double eeps_ud = 0;

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

		}

		public class PStom
		{
			private string _spcl="";
			private double[,] sp_data=new double[2,2] {{1,1},{1,1}};
			private static double zf_pk = 1860;
			private static double zf_pk01 = zf_pk / 1.1;

			public double f_pk 	 {get {return zf_pk;}}
			public double f_pk01 {get {return zf_pk01;}	}

			public PStom(string _spclass)
			{
				_spcl=_spclass;

			}
		}
	}
}

