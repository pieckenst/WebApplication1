using System;

namespace BRU.WEBFORMS.ASPNET.APP.Models
{
    /// <summary>
    /// Represents an application user account (app_user table).
    /// Separate from Employee - a user is an authentication entity,
    /// while an employee is a business entity.
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // Navigation properties
        public string EmployeeName { get; set; }
        public string EmployeeJobTitle { get; set; }

        /// <summary>
        /// Gets a user-friendly status description.
        /// </summary>
        public string StatusDescription
        {
            get
            {
                if (!IsActive)
                    return "Неактивен";
                return "Активен";
            }
        }
    }

    /// <summary>
    /// Represents a role (app_role table).
    /// </summary>
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        // For counting
        public int UserCount { get; set; }
        public int PermissionCount { get; set; }
    }

    /// <summary>
    /// Represents a permission (permission table).
    /// </summary>
    public class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Gets a display-friendly permission name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(PermissionName))
                    return string.Empty;

                // Convert VIEW_BUS to "View Bus"
                string[] words = PermissionName.Replace("_", " ").ToLower().Split(' ');
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (string word in words)
                {
                    if (word.Length > 0)
                    {
                        sb.Append(char.ToUpper(word[0]));
                        if (word.Length > 1)
                            sb.Append(word.Substring(1));
                        sb.Append(" ");
                    }
                }
                return sb.ToString().Trim();
            }
        }
    }

    /// <summary>
    /// Represents a user-role assignment (user_role table).
    /// </summary>
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime AssignedAt { get; set; }
        public string AssignedBy { get; set; }

        // Navigation properties
        public string Login { get; set; }
        public string RoleName { get; set; }
        public string AssignedByLogin { get; set; }
    }

    /// <summary>
    /// Represents a role-permission assignment (role_permission table).
    /// </summary>
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime GrantedAt { get; set; }

        // Navigation properties
        public string RoleName { get; set; }
        public string PermissionName { get; set; }
    }

    /// <summary>
    /// Represents combined user information with roles and permissions.
    /// </summary>
    public class UserWithRolesAndPermissions
    {
        public User User { get; set; }
        public System.Collections.Generic.List<string> Roles { get; set; }
        public System.Collections.Generic.List<string> Permissions { get; set; }

        public UserWithRolesAndPermissions()
        {
            Roles = new System.Collections.Generic.List<string>();
            Permissions = new System.Collections.Generic.List<string>();
        }
    }
}
