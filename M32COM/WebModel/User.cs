
using System;
namespace WebModel
{
	
	[Serializable]
	public partial class User
	{
		public User()
		{}
		#region Model
		private int _id;
		private int _usertype=0;
		private string _username;
		private string _userpassword;
		private string _truename;
		private int _departmentid;
		private int _roleid=0;
		private bool _isdisabled;
		private DateTime _createtime;
		private int _createid;
		private DateTime _updatetime;
		private int _updateid;
		private string _remark;
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
		public int UserType
		{
			set{ _usertype=value;}
			get{return _usertype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserPassword
		{
			set{ _userpassword=value;}
			get{return _userpassword;}
		}
        public string OldUserPassword
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string TrueName
		{
			set{ _truename=value;}
			get{return _truename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SaiShiID
        {
			set{ _departmentid=value;}
			get{return _departmentid;}
		}
        public string SaiShiName
        {
            get;set;
        }
        public int DepartmentID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DepartmentName
        {
            get; set;
        }
        public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int  CreateID
		{
			set{ _createid=value;}
			get{return _createid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int  UpdateID
		{
			set{ _updateid=value;}
			get{return _updateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public int Status { get; set; }
        public int IsDelete { get; set; }
        public string UserTypeName { get; set; }
        #endregion Model

    }
}

