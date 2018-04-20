[TestMethod]
public void AddDoctorTest(){
	service.AddDoctor("doctorTest1","John Snow","90010112345","506999000","john.snow@gmail.com","Snow's address","dermatologia");
	User doctorTest = service.FindUserById("doctorTest1");
	Assert.IsNotNull(doctorTest);
	Assert.AreEqual(doctorTest.name,"John Snow");
	Assert.AreEqual(doctorTest.pesel,"90010112345");
	Assert.AreEqual(doctorTest.phoneNumber,"506999000");
	Assert.AreEqual(doctorTest.email,"john.snow@gmail.com");
	Assert.AreEqual(doctorTest.address,"Snow's address");
	Assert.AreEqual(doctorTest.branch,"dermatologia");
}

[TestMethod]
public void AddReceptionistTest(){
	service.AddReceptionist("RecTest1","Larry Osborn","91070435441","786841266","MonicaHMadrid@rhyta.com","Fiolkowa 5");
	User RecTest = service.FindUserById("RecTest1");
	Assert.IsNotNull(RecTest);
	Assert.AreEqual(RecTest.name,"Larry Osborn");
	Assert.AreEqual(RecTest.pesel,"91070435441");
	Assert.AreEqual(RecTest.phoneNumber,"786841266");
	Assert.AreEqual(RecTest.email,"MonicaHMadrid@rhyta.com");
	Assert.AreEqual(RecTest.address,"Fiolkowa 5");
}

[TestMethod]
public void AddPatientTest(){
	service.AddPatient("patTest1","Justyna Pawlak","44041846988","697850826","tynaPawlak@jourrapide.com","Krakowska 15 Warszawa");
	User patTest = service.FindUserById("patTest1");
	Assert.IsNotNull(patTest);
	Assert.AreEqual(patTest.name,"Justyna Pawlak");
	Assert.AreEqual(patTest.pesel,"44041846988");
	Assert.AreEqual(patTest.phoneNumber,"697850826");
	Assert.AreEqual(patTest.email,"JustynaPawlak@jourrapide.com");
	Assert.AreEqual(patTest.address,"Krakowska 15 Warszawa");
}

[TestMethod]
public void AddDoctorHoursTest(){
	User doctorTest = service.FindUserById("doctorTest1");
	Assert.IsNotNull(doctorTest);
	service.AddDoctorHours(doctorTest,"hoursTest1",Days.Tuesday | Days.Thursday,CreateTime(8,0,0),CreateTime(16,0,0));
	Assert.IsTrue(doctorTest.doesWork(Days.Tuesday,CreateTime(10,0,0)));
}

[TestMethod]
public void AddVisitTest(){
	User doctorTest = service.FindUserById("doctorTest1");
	User patTest = service.FindUserById("patTest1");
	Assert.IsNotNull(patTest);
	Assert.IsNotNull(doctorTest);
	service.AddVisit(doctorTest,patTest,new DateTime(2019,01,15),CreateTime(10,0,0),new TimeSpan(0,15,0));
	Assert.IsTrue(doctorTest.IsAssigned(new DateTime(2019,01,15),CreateTime(10,0,0)));
}