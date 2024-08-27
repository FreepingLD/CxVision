using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class UserLoginParamManager
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static UserLoginParamManager _Instance;
        public static UserLoginParamManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new UserLoginParamManager();
                    }
                }
                return _Instance;
            }
        }

        public event EventHandler UserChange;

        private enUserName currentUser;
        public enUserName CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                if (UserChange != null)
                {
                    UserLoginParam loginParam = GetUser(currentUser);
                    UserChange.Invoke(loginParam, new EventArgs());
                }  
            }
        }
        private BindingList<UserLoginParam> _loginParam = new BindingList<UserLoginParam>();
        public BindingList<UserLoginParam> LoginParam { get => _loginParam; set => _loginParam = value; }


        public UserLoginParam GetUser(enUserName userName)
        {
            foreach (var item in _loginParam)
            {
                if (item.User == userName) return item;
            }
            return null;
        }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<UserLoginParam>>.Save(_loginParam, ParaPath + "\\" + "UserLoginParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "UserLoginParam.xml"))
                this._loginParam = XML<BindingList<UserLoginParam>>.Read(ParaPath + "\\" + "UserLoginParam.xml");
            else
                this._loginParam = new BindingList<UserLoginParam>();
            if (this._loginParam == null)
                this._loginParam = new BindingList<UserLoginParam>();
            if (this._loginParam != null && this._loginParam.Count > 0)
                this.CurrentUser = this._loginParam[0].CurUser;
        }




    }
}
