$(document).ready(function(){

	$("#password1").inputmask("Regex");
	$("#password2").inputmask("Regex");

	$("input").keypress( function(e) {
		var chr = String.fromCharCode(e.which);
		if ("{}[]<>~`\\|/*^%$#@".indexOf(chr) >= 0){
	      return false;
	      } else {
	        return true;
	      }
		});
//добавить запрет на ввод кириллицы

	jQuery.validator.addMethod(
		'containsOneDigitOneLatter', 
		function (value) { 
        	return /^(?=.*\d)(?=.*[a-zA-Z])/i.test(value); 
		}, 
		'Пароль должен содержать как минимум одну букву и одну цифру');

	$('#saveButton').click(function () {
		console.log("ok");
		$('#newPassword').validate({
			rules: {   
				password1: {
					required:true, 
					rangelength: [6,14],
					containsOneDigitOneLatter: true
				},
				password2: {
					required:true, 
					rangelength: [6,14], 
					containsOneDigitOneLatter: true  
				}			
			},
			messages: {
				password1: {
					required:"Введите пароль", 
					rangelength: "Длинна пароля - 6-14 символов"
				},
				password2: {
					required:"Введите пароль", 
					rangelength: "Длинна пароля - 6-14 символов" 
				}
			},
			submitHandler: function(emailForPassRec) {
				console.log("ok submitHandler");
				var password1 = $("#password1").val();
				var password2 = $("#password2").val();
				if (password1===password2) {
					MatchOld();
				} else {
				    $('#passwordMatch').removeClass('hide');
				    $('#passwordMatch').modal('show');
					$(".ok_btn_modal").click(function () {
					    $('#passwordMatch').addClass('hide');
					    $('#passwordMatch').modal('hide');
					});
					$("#password1").val("");
					$("#password2").val("");	
				}
			}
		});

	});

	function MatchOld() {
		console.log("MatchOld ok");
		var pass0 = $("#password0").val();
		var pass1 = $("#password1").val();
		var pass2 = $("#password2").val();
		var data = new Object();
		data.token = encodeURIComponent(pass0); // Изменил
		data.pass1 = pass1;
		data.pass2 = pass2; 		  	
		console.log(data);
		var json = JSON.stringify(data);  
		console.log(json);
	    $.ajax({
	        type: 'POST',
	        contentType: 'application/json; charset=utf-8', // Добавил
	        url: '/Account/ResetPassword',  //ссылку изменить
	        data: json, // изменил
	        success: function (data) {
	        	// PLACE FOR CALLBACK
	        	console.log(data);
	        	var passMatch = data;
	        	console.log(passMatch);
	        	console.log(passMatch.Succeded); // Изменил        	
	            console.log("ok Ajax MatchOld");
	            if (!passMatch.Succeeded) {  //false - совпадение со старым, показать попап
				    $('#matchOldPassword').removeClass('hide');
				    $('#matchOldPassword').modal('show');
					$(".ok_btn_modal").click(function () {
					    $('#matchOldPassword').addClass('hide');
					    $('#matchOldPassword').modal('hide');
					});
					$("#password1").val("");
					$("#password2").val("");	            	
	            } else { //true - пароль не совпал со старым - показываем "Пароль успешно изменен"
				    $('#okConfirm').modal('show');
					$(".ok_btn_modal").click(function () {
					    $('#okConfirm').modal('hide');
	        			window.location.href = "/" // Homepage 
					});	
	            }
	        },
	        error: function (data, error, mess) {
	            console.log("error: " + data + " " + error + " " + mess);
	        }
	    });	

	}

	// function UpdatePasword() {
	// 	console.log("UpdatePasword ok");
	// 	var password = $("#password1").val();
	// 	console.log(password);
	// 	var json = JSON.stringify(password);  
	// 	console.log(json);
	//     $.ajax({
	//         type: 'POST',
	//         url: '/Mentor/Review',  //ссылку изменить
	//         data: json,
	//         contentType: 'application/json; charset=utf-8',
	//         success: function (data) { 
	//         	// PLACE FOR CALLBACK
	// 			    //показываем "Пароль успешно изменен"
	// 			    $('#okConfirm').modal('show');
	// 				$(".ok_btn_modal").click(function () {
	// 				    $('#okConfirm').modal('hide');
	//         			window.location.href = "http://www.it-academy.by/" // Homepage 
	// 				});	
	//         	console.log("ok UpdatePasword");

	//         },
	//         error: function (data, error, mess) {
	//             console.log("error: " + data + " " + error + " " + mess);
	//         }
	//     });	
	// };


});