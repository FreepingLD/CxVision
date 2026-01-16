using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    // 权限实体
    [Serializable]
    public class Permission
    {
        public int Id { get; set; }
        public enPermissionCode Code { get; set; } // 如"编辑"、"下载"、"用户管理"
        public string Name { get; set; } // 权限名称

        public Permission()
        {
            this.Id = 0;
            this.Code = enPermissionCode.操作;
            this.Name = enPermissionCode.操作.ToString();
        }
        public Permission(enPermissionCode permissionCode)
        {
            this.Id = 0;
            this.Code = permissionCode;
            this.Name = permissionCode.ToString();
        }
    }

    // 角色实体
    //[Serializable]
    //public class Role
    //{
    //    public int Id { get; set; }
    //    public enUserName Name { get; set; } // 操作员/工程师/管理员
    //    public List<Permission> Permissions { get; set; }
    //    public Role() 
    //    {
    //        this.Id = 0;
    //        this.Name = enUserName.操作员;
    //        this.Permissions = new List<Permission>();  
    //    }

    //    public Role(enUserName name)
    //    {
    //        this.Id = 0;
    //        this.Name = name;
    //        this.Permissions = new List<Permission>();
    //    }
    //}

    // 用户实体
    [Serializable]
    public class User
    {
        public int Id { get; set; }
        public enUserName UserName { get; set; }
        public string Password { get; set; } // 建议存MD5加密串，不要明文
        //public Role UserRole { get; set; }
        public List<Permission> Permissions { get; set; }
        public bool IsActive { get; set; } // 是否启用

        public User()
        {
            this.Id = 0;
            this.UserName = enUserName.操作员;
            this.Password = "123";
            //this.UserRole = new Role();
            this.Permissions = new List<Permission>();
            this.IsActive = true;
        }
        public User(enUserName name,string Password)
        {
            this.Id = 0;
            this.UserName = name;
            this.Password = Password;
            //this.UserRole = new Role(name);
            this.Permissions = new List<Permission>();
            this.IsActive = true;
        }

    }

    [Flags]
    public enum enPermissionCode
    {
        操作 = 1,
        编辑 = 1 << 1,
        管理 = 1 << 2,
        开发 = 1 << 3,
    }


}
