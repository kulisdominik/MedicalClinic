using MedicalClinic.Models;
using System;
using System.Linq;


namespace MedicalClinic.Data
{
	public static class DbInitializer 
	{
        public static void Initialize(UsersContext context)
        {
            context.Database.EnsureCreated();

            if(context.Users.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User("D", "K", "Admin"),
                new User("D", "K", "Guest"),
                new User("D", "K", "User")
            };

            foreach (var user in users)
            {
                context.Users.Add(user);
            }
        }

    }
}