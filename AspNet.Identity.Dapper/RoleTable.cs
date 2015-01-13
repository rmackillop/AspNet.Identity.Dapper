using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Linq;

namespace AspNet.Identity.Dapper
{
  /// <summary>
  /// Class that represents the Role table in the Database
  /// </summary>
  public class RoleTable
  {
    private DbManager db;
    /// <summary>
    /// Constructor that takes a DbManager instance 
    /// </summary>
    /// <param name="database"></param>
    public RoleTable(DbManager database)
    {
      db = database;
    }

    /// <summary>
    /// Deltes a role from the Roles table
    /// </summary>
    /// <param name="roleId">The role Id</param>
    /// <returns></returns>
    public void Delete(int roleId)
    {
      db.Connection.Execute(@"DELETE FROM Role WHERE Id = @Id", new { Id = roleId });
    }

    /// <summary>
    /// Inserts a new Role in the Roles table
    /// </summary>
    /// <param name="roleName">The role's name</param>
    /// <returns></returns>
    public void Insert(IdentityRole role)
    {
      db.Connection.Execute(@"INSERT INTO Role (Name) VALUES (@name)", new { name = role.Name });
    }

    /// <summary>
    /// Returns a role name given the roleId
    /// </summary>
    /// <param name="roleId">The role Id</param>
    /// <returns>Role name</returns>
    public string GetRoleName(int roleId)
    {
      return db.Connection.ExecuteScalar<string>("SELECT Name FROM Role WHERE Id = @Id", new { Id = roleId });
    }

    /// <summary>
    /// Returns the role Id given a role name
    /// </summary>
    /// <param name="roleName">Role's name</param>
    /// <returns>Role's Id</returns>
    public int GetRoleId(string roleName)
    {
      return db.Connection.ExecuteScalar<int>("SELECT Id FROM Role WHERE Name = @Name", new { Name = roleName });
    }

    /// <summary>
    /// Gets the IdentityRole given the role Id
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public IdentityRole GetRoleById(int roleId)
    {
      var roleName = GetRoleName(roleId);
      IdentityRole role = null;

      if (roleName != null)
      {
        role = new IdentityRole(roleName, roleId);
      }

      return role;
    }

    /// <summary>
    /// Gets the IdentityRole given the role name
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public IdentityRole GetRoleByName(string roleName)
    {
      var roleId = GetRoleId(roleName);
      IdentityRole role = null;

      if (roleId > 0)
      {
        role = new IdentityRole(roleName, roleId);
      }

      return role;
    }

    public void Update(IdentityRole role)
    {
      db.Connection.Execute(@"UPDATE Role SET Name = @Name WHERE Id = @Id", new { Name = role.Name, Id = role.Id });
    }
  }
}
