using System;
using System.Collections.Generic;


namespace MedicalClinic.Models
{
	public class User 
	{
        public User(string _FirstName, string _LastName, string _AccountType)
        {
            FirstName = _FirstName;
            LastName = _LastName;
            AccountType = _AccountType;
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountType { get; set; }
	}
}