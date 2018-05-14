casper.test.begin('testing the title', 1, function(test) {
  casper.start('http://localhost:57492/', function() {
  });

  casper.then(function() {
    test.assertTitle('Start - MedicalClinic', 'the website title is as expected');
  });

  casper.run(function() {
    test.done();
  });
});