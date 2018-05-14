casper.test.begin('testing admin login', 1, function(test) {
  casper.start('http://localhost:57492/Account/Login', function() {
  	casper.waitForSelector('form', function(){});
  	this.fill('form',{'Email':'test@admin.pl','Password':'P@ssw0rd'},true);
  	this.clickLabel('Zaloguj');
  });

  casper.then(function() {
    test.assertTitle('Start - MedicalClinic', 'logged in correctly');
  });

  casper.run(function() {
    test.done();
  });
});