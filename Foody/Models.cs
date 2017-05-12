using System;
using SQLite;

namespace Foody
{	
	[Table("Users")]
	public class UserModel
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string username { get; set; }
		public string password { get; set; }

		public UserModel()
		{

		}

		public UserModel(string username, string password)
		{
			this.username = username;
			this.password = password;
		}
	}
}
