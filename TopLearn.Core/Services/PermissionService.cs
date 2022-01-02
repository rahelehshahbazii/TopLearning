using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private TopLearnContext _context;
        public PermissionService(TopLearnContext context)
        {
            _context = context;
        }
        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach(int roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId=roleId,
                    UserId=userId,
                });
            }
            _context.SaveChanges();
        }

        public void EditRolesUser(int UserId, List<int> RolesId)
        {
          //Delete All ROles User 
            _context.UserRoles.Where(r=>r.UserId == UserId).ToList().ForEach(r=>_context.UserRoles.Remove(r));
            //Add New Roles
            AddRolesToUser(RolesId, UserId);

        }
        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            UpdateRole(role);
        }

        public List<Permission> GetAllPermission()
        {
            return _context.permission.ToList();
        }

        public void AddermissionsToRole(int RoleId, List<int> permission)
        {
            foreach( var p in permission)
            {
                _context.RolePermission.Add(new RolePermission()
                {
                    permissionId=p,
                    RoleId=RoleId
                });
            }
            _context.SaveChanges();
        }

        public List<int> PermissionsRole(int roleId)
        {
            return _context.RolePermission
                .Where(r => r.RoleId == roleId)
                .Select(r=>r.permissionId).ToList();
        }

        public void UpdatePermissionsRole(int roleId, List<int> permissions)
        {
            _context.RolePermission.Where(p => p.RoleId == roleId)
                .ToList().ForEach(p=> _context.RolePermission.Remove(p));

            AddermissionsToRole(roleId,permissions);

        }
        public bool CheckPermisssion(int permssionId, string userName)
        {
            int userId = _context.Users.Single( u=>u.UserName == userName).UserId;
            
            List<int> UserRoles = _context.UserRoles
                .Where(r=>r.UserId == userId).Select(r=>r.RoleId).ToList();

            if (!UserRoles.Any())
                return false;
            List<int> RolePermission = _context.RolePermission
                .Where(p=>p.permissionId == permssionId)
                .Select(p=>p.RoleId).ToList();

            return RolePermission.Any(p => UserRoles.Contains(p));
        }
    }
}
