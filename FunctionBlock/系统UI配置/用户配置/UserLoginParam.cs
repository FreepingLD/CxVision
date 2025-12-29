using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    public class UserLoginParam
    {      
        private enUserName _user;
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserLoginParam(enUserName UserName, string password)
        {
            this.User = UserName;
            this.Password = password;
            this.IsLogin = false;
        }
        public UserLoginParam()
        {
            this.User = enUserName.操作员;
            this.Password = "";
            this.IsLogin = false;
        }

        public enUserName CurUser { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public enUserName User
        {
            set
            {
                this._user = value;
                CurUser = value;
                //if (UserChange != null)
                //    UserChange.Invoke(this, new EventArgs());
            }
            get { return this._user; }
        }
        public string Password
        {
            set;
            get;
        }
        public int State
        {
            set;
            get;
        }
        public bool IsLogin
        {
            set;
            get;

        }

    }
    public enum enUserName
    {
        操作员,
        工程师,
        开发人员,
        管理员,
    }


}
