$(document).ready(function(){

    function EmailFormat(value) {
        return /^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$/i.test(value);
    }
    $.validator.addMethod('email_format', EmailFormat, 'Неправильный формат. Введите e-mail');	

	$('#saveButton').click(function () {
		$('#emailForPassRec').validate({
			rules: {   
				email: {
					required:true, 
					email_format: true 
				}
			},
			messages: {
				email: {
				  	required: "Введите e-mail",
			  	}
			},
			submitHandler: function(emailForPassRec) {
			CheckEmail();
			}
		});

	});



	function CheckEmail() {
		console.log("CheckEmail ok");
		var email = $("#InputEmail").val();
		console.log(email);
		var data = new Object();
		data.email = email; 
		//закомментить с 41 до 53 включительно при проверке с БЭ
		var response = false;
		if (response) {
		    $('#okConfirm').modal('show');
			$(".ok_btn_modal").click(function () {
			    $('#okConfirm').modal('hide');
			    window.location.href = "http://www.google.com" // Homepage 
			});	
		} else {
		    $('#errorConfirm').modal('show');
			$(".ok_btn_modal").click(function () {
			    $('#errorConfirm').modal('hide');
			});	
		};


	    $.ajax({
	        type: 'POST',
	        url: '/Account/ForgotPassword',  //ссылку вставить
	        data: data, // Изменил
	        //contentType: 'application/json; charset=utf-8',
	        success: function (data) {
	        	console.log(data);
	        	var emailInDB = data;
	        	console.log(emailInDB);
	        	console.log(emailInDB.IsEmailExisting);
	        	if (emailInDB.IsEmailExisting) {
	        		console.log("emailExist true");
					    $('#okConfirm').modal('show');
						$(".ok_btn_modal").click(function () {
						    $('#okConfirm').modal('hide');
						    window.location.href = "http://www.it-academy.by/" // Homepage 
						});	
	        	} else {
	        		console.log("email false");
				    $('#errorConfirm').modal('show');
					$(".ok_btn_modal").click(function () {
					    $('#errorConfirm').modal('hide');
					});	
	        	}
	            // PLACE FOR CALLBACK
	            console.log("ok Ajax");

	        },
	        error: function (data, error, mess) {
	            console.log("error: " + data + " " + error + " " + mess);
	        }
	    });	
	};

});

