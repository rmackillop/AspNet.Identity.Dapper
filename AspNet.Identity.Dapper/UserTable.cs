using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace AspNet.Identity.Dapper
{
  /// <summary>
  /// Class that represents the Users table in the Database
  /// </summary>
  public class UserTable<TUser>
      where TUser : IdentityMember
  {
    private DbManager db;

    /// <summary>
    /// Constructor that takes a DbManager instance 
    /// </summary>
    /// <param name="database"></param>
    public UserTable(DbManager database)
    {
      db = database;
    }

    /// <summary>
    /// Returns the Member's name given a Member id
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    public string GetUserName(int memberId)
    {
      return db.Connection.ExecuteScalar<string>("SELECT Name FROM Member WHERE Id = @Id", new { Id = memberId });
    }

    /// <summary>
    /// Returns a Member ID given a Member name
    /// </summary>
    /// <param name="userName">The Member's name</param>
    /// <returns></returns>
    public int GetmemberId(string userName)
    {
      return db.Connection.ExecuteScalar<int>("SELECT Id FROM Member WHERE UserName = @UserName", new { UserName = userName });
    }

    /// <summary>
    /// Returns an TUser given the Member's id
    /// </summary>
    /// <param name="memberId">The Member's id</param>
    /// <returns></returns>
    public TUser GetUserById(int memberId)
    {
      return db.Connection.Query<TUser>("SELECT * FROM Member WHERE Id = @Id", new { Id = memberId })
          .FirstOrDefault();
    }

    /// <summary>
    /// Returns a list of TUser instances given a Member name
    /// </summary>
    /// <param name="userName">Member's name</param>
    /// <returns></returns>
    public List<TUser> GetUserByName(string userName)
    {
      return db.Connection.Query<TUser>("SELECT * FROM Member WHERE UserName = @UserName", new { UserName = userName })
          .ToList();
    }

    public List<TUser> GetUserByEmail(string email)
    {
      return null;
    }

    /// <summary>
    /// Return the Member's password hash
    /// </summary>
    /// <param name="memberId">The Member's id</param>
    /// <returns></returns>
    public string GetPasswordHash(int memberId)
    {
      return db.Connection.ExecuteScalar<string>("SELECT PasswordHash FROM Member WHERE Id = @Id", new { Id = memberId });
    }

    /// <summary>
    /// Sets the Member's password hash
    /// </summary>
    /// <param name="memberId"></param>
    /// <param name="passwordHash"></param>
    /// <returns></returns>
    public void SetPasswordHash(int memberId, string passwordHash)
    {
      db.Connection.Execute(@"
                    UPDATE
                        Member
                    SET
                        PasswordHash = @pwdHash
                    WHERE
                        Id = @Id", new { pwdHash = passwordHash, Id = memberId });
    }

    /// <summary>
    /// Returns the Member's security stamp
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    public string GetSecurityStamp(int memberId)
    {
      return db.Connection.ExecuteScalar<string>("SELECT SecurityStamp FROM Member WHERE Id = @Id", new { MemberId = memberId });
    }

    /// <summary>
    /// Inserts a new Member in the Users table
    /// </summary>
    /// <param name="Member"></param>
    /// <returns></returns>
    public void Insert(TUser member)
    {
      DynamicParameters p = new DynamicParameters();
      string sql = "INSERT INTO Member (";
      string sqlVal = "VALUES (";
      int propCount = 0;

      foreach (PropertyInfo propertyInfo in member.GetType().GetProperties())
      {
        if (propertyInfo.Name != "Id")
        {
          sql += (propCount == 0 ? "" : ",") + propertyInfo.Name;
          sqlVal += (propCount == 0 ? "@" : ",@") + propertyInfo.Name;
          p.Add(propertyInfo.Name, propertyInfo.GetValue(member, null));
          propCount++;
        }
      }
      sql = sql + ") " + sqlVal + "); SELECT Cast(SCOPE_IDENTITY() as int)";

      member.Id = db.Connection.ExecuteScalar<int>(sql, p);
    }

    /// <summary>
    /// Deletes a Member from the Users table
    /// </summary>
    /// <param name="memberId">The Member's id</param>
    /// <returns></returns>
    private void Delete(int memberId)
    {
      db.Connection.Execute(@"DELETE FROM Member WHERE Id = @Id", new { Id = memberId });
    }

    /// <summary>
    /// Deletes a Member from the Users table
    /// </summary>
    /// <param name="Member"></param>
    /// <returns></returns>
    public void Delete(TUser Member)
    {
      Delete(Member.Id);
    }

    /// <summary>
    /// Updates a Member in the Users table
    /// </summary>
    /// <param name="Member"></param>
    /// <returns></returns>
    public void Update(TUser member)
    {
      DynamicParameters p = new DynamicParameters();
      string sql = "UPDATE Member SET ";
      int propCount = 0;

      foreach (PropertyInfo propertyInfo in member.GetType().GetProperties())
      {
        if (propertyInfo.Name != "Id")
        {
          sql += (propCount == 0 ? "" : ",") + propertyInfo.Name + "=@" + propertyInfo.Name;
          propCount++;
        }

        p.Add(propertyInfo.Name, propertyInfo.GetValue(member, null));

      }
      sql += " WHERE Id=@Id";

      db.Connection.Execute(sql, p);

    }
  }
}
