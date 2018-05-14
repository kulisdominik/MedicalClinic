using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MedicalClinic;

namespace ClinicTest
{
    [TestClass]
    public class ClinicTests
    {
        [TestMethod]
        public void TestAdmin()
        {
            //arrange
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var testUser = new ApplicationUser
            {
                UserName = "example@lol.com",
                Email = "example@lol.com",
                EmailConfirmed = true
            };
            String userPassword = "pass";

            //assert
            Assert.isTrue(userManager.FindByEmailAsync("example@lol.com") != null);
        }
    }
}
