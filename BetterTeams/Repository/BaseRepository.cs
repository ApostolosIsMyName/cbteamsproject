using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Repository
{
    public class BaseRepository : IDisposable
    {
        protected IDbConnection _con;
        protected bool _ownsConnection;
        public BaseRepository()
        {
            _ownsConnection = true;
            string connectionString = @"";
            _con = new SqlConnection(connectionString);
        }

        public BaseRepository(IDbConnection connection)
        {
            _ownsConnection = false;
            _con = connection;
        }

        protected string GetInsertQuery(IEnumerable<string> keys)
        {
            string joinedKeys = String.Join(",", keys);
            return $"({joinedKeys.Replace("@", String.Empty)}) VALUES ({joinedKeys})";
        }

        protected string GetUpdateQuery(IEnumerable<string> keys)
        {
            return String.Join(",", keys.Select(x => x.Replace("@", String.Empty) + "=" + x)); // Key = @Key
        }

        public void Dispose()
        {
            if (_ownsConnection) // If the connection belongs to another repo do not dispose it
                _con.Dispose();
        }
        public IDbConnection Connection
        {
            get
            {
                return _con;
            }
        }
		public string EncryptPassword(string Password)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(Password);
			byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
			return Convert.ToBase64String(inArray);
		}
	}
}
