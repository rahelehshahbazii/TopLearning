using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Roles
        List<Role> GetRoles();
        int AddRole(Role role);
        Role GetRoleById(int roleId);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        void AddRolesToUser(List<int> roleIds, int userId);
        void EditRolesUser(int UserId, List<int> RolesId);
        #endregion

        #region Permission
        List<Permission> GetAllPermission();
        void AddermissionsToRole(int RoleId,List<int>permission);
        List<int> PermissionsRole(int roleId);
        void UpdatePermissionsRole(int roleId,List<int> permissions);
        bool CheckPermisssion(int permssionId, string userName);
        #endregion
    }
}
