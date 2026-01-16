using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Converters;

namespace FunctionBlock
{
    // 全局权限管理类，登录后缓存当前用户和权限
    public static class UserManager
    {
        private static string ParaPath = @"VisionParam\ConfigParam";
        private static User _currentUser;

        public static event EventHandler UserChange;
        /// <summary>
        /// 更改用户需要触发一次事件
        /// </summary>
        public static User CurrentUser
        {
            get => _currentUser;
            private
            set
            {
                _currentUser = value;
                if (UserChange != null)
                {
                    UserChange?.Invoke(_currentUser, new EventArgs());
                }
            }
        }

        // 登录验证
        public static bool Login(enUserName userName, string password)
        {
            var user = UserList.FirstOrDefault(u => u.UserName == userName
                                                  && u.Password == password // 实际用MD5校验
                                                  && u.IsActive);
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            else
            {
                if (userName == enUserName.操作员) return true;
            }
            return false;
        }

        // 核心：权限校验（工控场景高频调用），业务逻辑代码只对权限作判断，这样可实现对用户实现动态的权限赋值
        public static bool HasPermission(enPermissionCode permCode)
        {
            //return CurrentUser != null
            //       && CurrentUser.UserRole != null
            //       && CurrentUser.UserRole.Permissions.Any(p => p.Code == permCode);
            //return CurrentUser != null && CurrentUser.Permissions.Any(p => p.Code == permCode);
            return CurrentUser != null && CurrentUser.Permissions.Any(p => (p.Code & permCode) == p.Code);
        }

        // 退出登录
        public static void Logout() => CurrentUser = null;

        /// <summary>
        /// 用户列表
        /// </summary>
        public static BindingList<User> UserList { get; set; }

        public static bool HasUser(enUserName userName)
        {
            foreach (var item in UserList)
            {
                if (item.UserName == userName) return true;
            }
            return false;
        }
        public static User GetUser(enUserName userName)
        {
            foreach (var item in UserList)
            {
                if (item.UserName == userName) return item;
            }
            return null;
        }
        public static void RemoveUser(enUserName userName)
        {
            User user = null;
            foreach (var item in UserList)
            {
                if (item.UserName == userName) user = item;
            }
            if (user != null)
                UserList.Remove(user);
        }




        public static bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<User>>.Save(UserList, ParaPath + "\\" + "UserManager.xml"); // 以类名作为文件名
            return IsOk;
        }

        public static void Read()
        {
            if (File.Exists(ParaPath + "\\" + "UserManager.xml"))
                UserList = XML<BindingList<User>>.Read(ParaPath + "\\" + "UserManager.xml");
            else
            {
                UserList = new BindingList<User>();
            }
            if (UserList == null)
            {
                UserList = new BindingList<User>();
            }
            if (UserList.Count == 0)
            {
                User user = new User(enUserName.操作员, "");
                user.Permissions.Add(new Permission(enPermissionCode.操作));
                UserList.Add(user);
                ////
                user = new User(enUserName.工程师, "123");
                user.Permissions.Add(new Permission(enPermissionCode.操作));
                user.Permissions.Add(new Permission(enPermissionCode.编辑));
                UserList.Add(user);
                /////////////////
                user = new User(enUserName.管理员, "manager");
                user.Permissions.Add(new Permission(enPermissionCode.操作));
                user.Permissions.Add(new Permission(enPermissionCode.编辑));
                user.Permissions.Add(new Permission(enPermissionCode.管理));
                UserList.Add(user);
                //////////////////
                user = new User(enUserName.开发人员, "admin");
                user.Permissions.Add(new Permission(enPermissionCode.操作));
                user.Permissions.Add(new Permission(enPermissionCode.编辑));
                user.Permissions.Add(new Permission(enPermissionCode.管理));
                user.Permissions.Add(new Permission(enPermissionCode.开发));
                UserList.Add(user);
            }

        }




    }


}
