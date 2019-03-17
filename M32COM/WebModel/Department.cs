
using System;
namespace WebModel
{
	
	[Serializable]
	public partial class Department
	{
		public Department()
		{}
		#region Model
		private int _id;
		private string _name;
		private DateTime _createtime;
		private int? _createid;
		private bool _isdisabled;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CreateID
		{
			set{ _createid=value;}
			get{return _createid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsDelete
        {
            get;set;
		}
        public int SaiShiID
        {
            get; set;
        }
        public string SaiShiName
        {
            get; set;
        }
        public string Users
        {
            get; set;
        }
        public decimal Score
        {
            get;set;
        }
        public bool IsAblePinFen
        {
            get; set;
        }
        #endregion Model

    }
}

