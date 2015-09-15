using System;
using System.Collections.Generic;
using Section_Explorer;
using Database_Mat;

namespace Sect_ExpEC2
{
	/// <summary>
	/// Група армировки, разглеждани като една
	/// </summary>
	public class armGroup
	{
		public double _area = 0;
		public double _ydis = 0;
		private Stom _st;

		/// <summary>
		/// Конструктор <see cref="Sect_ExpEC2.armGroup"/> class.
		/// </summary>
		/// <param name="_a">Площ на сечението</param>
		/// <param name="_y">Ордината на центъра на тежестта</param>
		/// <param name="_s">Тип на армировката</param>
		public armGroup (double _a, double _y, Stom _s)
		{
			_area = _a;
			_ydis = _y;
			_st=_s;
		}

		/// <summary>
		/// Изчислява силата в армировката в зависимост от деформацията
		/// </summary>
		/// <param name="_eps">Деформация в промили</param>
		/// <param name="_desgnSitu">Ако е <c>true</c> - изчислителна ситуация.</param>
		public double Force (double _eps,bool _desgnSitu)
		{
			return _st.Stress (_eps,_desgnSitu) * _area;
		}
	}

	/// <summary>
	/// Клас за стоманобетоново сечение
	/// </summary>
	public class SBsec
	{
		public static Section B_sec;
		public static DataBa Mats ;
		public List<armGroup> armList=new List<armGroup>();

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="_vtx">координати на бетоновото сечение</param>
		public SBsec (List<Point> _vtx)
		{
			B_sec= new Section(_vtx);
		}

		/// <summary>
		/// Задаване на материали
		/// </summary>
		/// <param name="_sbet">Бетон</param>
		/// <param name="_sstom">Обикновена армировка</param>
		/// <param name="_spstom">Напрегната армировка</param>
		public void SetMats(string _sbet,string _sstom,string _spstom)
		{
			Mats = new DataBa (_sbet,_sstom,_spstom);
		}
		/// <summary>
		/// Добавя армировка
		/// </summary>
		/// <param name="_arm">Добавя група армировки</param>
		public void AddArm(armGroup _arm)
		{
			armList.Add (_arm);
		}


			
		public void AnalyseStress(double _x, double _ec, List<Point> _lst,
						out double bF, out double bM)
		{
			double y=0;
			double _sig=0;
			double bForce = 0;
			double bMoment = 0;
			for (int i=0; i<_lst.Count; i++)
			{
				y=_lst[i].YCoord; //y координата на дискретната площ
				_sig=Mats.MBet.Stress(B_sec.eps_xc(y,_x,_ec),true,2);
				bForce += _sig * _lst [i].XCoord/10;
				bMoment -=_sig*_lst[i].XCoord * (y - B_sec.ycg)/1000;
			}
			bF = bForce;
			bM = bMoment;		
		}
	

	}
}

