//tests are adapted to Medical Clinic app of 2018-06-10 22:05

PHANTOMJS_EXECUTABLE="phantomjs/bin/phantomjs.exe"
var nickname;
var password;

casper.test.begin("MedicalClinic", 69, function(test){
	casper.start("http://localhost:57492/", function(){
		casper.viewport(1100,800);
		casper.waitForSelector("footer",function(){
			casper.echo("main page");
			test.assertTitle("Start - MedicalClinic", "main page title");
			test.assertSelectorHasText("a", "Rejestracja");
			test.assertSelectorHasText("a", "Logowanie");
			test.assertSelectorHasText("a", "Informacje kontaktowe");
			test.assertElementCount("h2", 3, "h2 headers");
			test.assertElementCount("h3", 1, "h3 headers");
			test.assertElementCount("p", 7, "paragraphs in description");
			test.assertElementCount("h5", 2, "h5 header");
			test.assertElementCount("li", 14, "list elements");
			test.assertExists(".row", "row exists");
			test.assertExists(".menu", "menu exists");
			test.assertExists(".services", "services exists");
			test.assertExists("header", "header exists");
			test.assertExists("footer", "footer exists");
			test.assertExists("#invite", "invite exists");
			test.assertHttpStatus(200, "http status ok");
			this.clickLabel("Informacje kontaktowe","a");
			});

		casper.waitForSelector("footer",function(){
			casper.echo("informacje kontaktowe page");
			test.assertTitle("Informacje kontaktowe - MedicalClinic", "informacje kontaktowe page title");
			test.assertSelectorHasText("a", "Rejestracja");
			test.assertSelectorHasText("a", "Logowanie");
			test.assertSelectorHasText("a", "Informacje kontaktowe");
			test.assertExists("#address", "address");
			test.assertExists("#mail", "mail");
			test.assertExists("#phone", "phone");
			test.assertExists("#staff", "staff");
			test.assertResourceExists("contact_img_1.svg", "img address");
			test.assertResourceExists("contact_img_2.svg", "img mail");
			test.assertResourceExists("contact_img_3.svg", "img phone");
			test.assertResourceExists("contact_img_4.svg", "img staff");
			this.clickLabel("Rejestracja");
		});

		casper.waitForSelector("footer", function(){
			casper.echo("rejestracja page");
			test.assertTitle("Rejestracja - MedicalClinic");
			test.assertSelectorHasText("a", "Rejestracja");
			test.assertSelectorHasText("a", "Logowanie");
			test.assertSelectorHasText("a", "Informacje kontaktowe");
			test.assertExists("#Email", "mail");
			test.assertExists("#Password", "password");
			test.assertExists("#ConfirmPassword", "confirm password");
			test.assertElementCount(".form-group", 5, "form elements");
			test.assertElementCount("h2", 1, "h2 header");
			nickname = randomNickname();
			this.fill("form",{"Email":nickname},true);
			test.assertField("Email", nickname);
			test.assertExists("#Email-error");
			test.assertEval(function(){
				return __utils__.findOne("#Email-error").textContent == "Podano nieprawidłowy adres e-mail.";
			});
			this.fill("form", {"Email":nickname+"@"+nickname+".com", "Password":"abc"},true);
			test.assertExists("#Password-error");
			test.assertEval(function(){
				return __utils__.findOne("#Password-error").textContent == "Hasło musi mieć co najmniej 6 znaków i być krótsze niż 20 znaków.";
			});
			test.assertExists("#ConfirmPassword-error");
			test.assertEval(function(){
				return __utils__.findOne("#ConfirmPassword-error").textContent == "Podane hasła się nie zgadzają.";
			});
			password = nickname+"Q1!"
			nickname = nickname+"@"+nickname+".com";
			casper.echo("user: "+nickname+" "+password);
			this.fill("form", {"Email":nickname, "Password":password, "ConfirmPassword":password, "FirstName":nickname, "LastName":nickname},true);
		});

		casper.waitForSelector("footer", function(){
			casper.echo("profil page");
			test.assertTitle("Start - MedicalClinic");
			test.assertSelectorHasText("a", "Profil");
			test.assertSelectorHasText("button", "Wyloguj");
			test.assertElementCount("h2", 3, "headers");
			this.clickLabel("Profil");
		});
		
		casper.waitForSelector("footer", function(){
			casper.echo("save phone number");
			test.assertSelectorHasText("#Username", nickname);
			test.assertSelectorHasText("#Email", nickname);
			test.assertExists("form");
			test.assertExists("#PhoneNumber");
			casper.evaluate(function(){
				document.querySelector("#PhoneNumber").value = "12345";
			});
			test.assertElementCount(".send_button",1);
			this.click(".send_button");
		});

		casper.waitForSelector("footer",function(){
			this.clickLabel("Zmiana hasła");
		});

		casper.waitForSelector("footer", function(){
			casper.echo("change password");
			casper.evaluate(function(){
				document.querySelector("#OldPassword").value = password;
				document.querySelector("#NewPassword").value = password+"x";
				document.querySelector("#ConfirmPassword").value = password+"x";
			});
			password += "x";
			this.click(".send_button");
		});

		casper.waitForSelector("footer", function(){
			this.clickLabel("Edycja profilu");
		})

		casper.waitForSelector("footer", function(){
			casper.echo("check phone number");
			test.assertSelectorHasText("a", "Profil");
			test.assertSelectorHasText("button", "Wyloguj");
			test.assertSelectorHasText("#PhoneNumber", "12345");
			this.clickLabel("Wyloguj");
		});
		
		casper.waitForSelector("footer", function(){
			casper.echo("main page back");
			test.assertSelectorHasText("a", "Rejestracja");
			test.assertSelectorHasText("a", "Logowanie");
			test.assertSelectorHasText("a", "Informacje kontaktowe");
			this.clickLabel("Logowanie");
		});

		casper.waitForSelector("footer", function(){
			casper.echo("log in as admin");
			test.assertTitle("Logowanie - MedicalClinic");
			test.assertElementCount(".form-group", 5, "form elements");
			test.assertExists("#Email", "email");
			test.assertExists("#Password", "password");
			test.assertExists("#Email", "email");
			this.fill("form",{"Email":"test@admin.pl","Password":"P@ssw0rd"},true);
		})

		casper.waitForSelector("footer", function(){
			casper.echo("click panel");
			test.assertSelectorHasText("a", "Profil");
			test.assertSelectorHasText("a", "Panel administratora");
			test.assertSelectorHasText("button", "Wyloguj");
			this.clickLabel("Panel administratora");
		});

		casper.waitForSelector("footer", function(){
			casper.echo("chceck if there is list of users");
			test.assertExists(".table");
			test.assertSelectorHasText("td", nickname);
		});
	});

	casper.run(function(){
		test.done();
	});
});

function randomNickname(){
	var nickname = "";
	var consonants = "wrtpsdfghjklzxcvbnm";
	var vowels = "aeiouy";

	nickname += consonants.charAt(Math.floor(Math.random() * consonants.length));
	nickname += vowels.charAt(Math.floor(Math.random() * vowels.length));
	nickname += consonants.charAt(Math.floor(Math.random() * consonants.length));
	nickname += vowels.charAt(Math.floor(Math.random() * vowels.length));
	nickname += consonants.charAt(Math.floor(Math.random() * consonants.length));

	return nickname;
}