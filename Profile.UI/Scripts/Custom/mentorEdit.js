$(document).ready(function(){

	// correct mentor name width with symbol length

	$("#mentorName").width(($("#mentorName").val().length) * (parseInt($("#mentorName").css("fontSize")) / 1.85));

	// hide empty datas
	
	$(".infoInput").each(function(ind,item){

		if(!$(this).val()){

			$(this).closest("label").hide(0);
		}
	})

	// ajax POST

	function sendData(callb){

		var pathname = $("#saveChanges").attr("data-src");

		if(!pathname) pathname = $("#truePhoto").attr("src");

		// var data = {};

		var file = document.getElementById("loadPhoto").files[0];
		var data = new FormData();

		data.append("Id", $("#metorId").val());
		data.append("FullName", $("#mentorName").val());
		data.append("IsAvatarDeleting", Boolean($("#removePhoto").val()));
		data.append("Skype", $("#skype").val());
		data.append("LinkedIn", $("#linkedin").val());
		data.append("Phone", $("#phone").val()); 
		data.append("Email", $("#email").val());
		data.append("NewAvatar", file);

	    $.ajax({

	        url: '/mentor/edit',
	        type: 'POST',
	        data: data,
	        cache: false,
	        processData: false, 
	        contentType: false,

	        success: function( data, textStatus, jqXHR ){
	 
	            if( typeof data.error === 'undefined' ){

	            	if(typeof callb == "function"){

	            		callb(data)
	            	}	                
	            }

	            else{
	                console.log('ОШИБКИ ОТВЕТА сервера: ' + data.error );
	            }
	        },

	        error: function( jqXHR, textStatus, errorThrown ){

	            console.log('ОШИБКИ AJAX запроса: ' + textStatus );
	        }
	    });
	}

	// validation rules and masks

	$("#email").on("keyup", function(e){

		if(e.keyCode == 32){

			e.preventDefault();
		}
	})

	function validate(){

		function validHighlight(input,message){

			$(input).on("keyup", function(){

				if($(this).val()){

					$(this).css("outline","1px solid black").next("b").remove();

					$("#saveChanges").attr("data-target","#myModal");
				}

			}).css("outline","1px solid red").after("<b class='error'>" + message + "</b>");


			$(".error").fadeIn();

			back = false;
		}

		var back = true;

		if($("#phone").val().indexOf("_") + 1){

			validHighlight("#phone","Укажите корректный номер телефон");
		}

		if($("#email").val() && !($("#email").val().indexOf("@") + 1)){

			validHighlight("#email","E-mail должен содержать @");
		}

		if(!($("#mentorName").val())){

			validHighlight("#mentorName","Это поле обязательно для заполнения");
		}

		return back;
	}

	$('#phone').inputmask("8-099-999-99-99");
	$("#email").inputmask("Regex");
	$("#skype").inputmask("Regex");
	$("#linkedin").inputmask("Regex");
	$("#mentorName").inputmask("Regex");

	// deleting photo

	$("#removePhotoButton").on("click", function(e){

		e.preventDefault();

		$(".modal-body p").text("Фото будет удалено. Удалить ?");

		$("#saveChangesModal").off().on("click",function(e){

			$("#saveChanges").removeClass("disabled").attr("data-target","#myModal");

			$("#removePhoto").val("true");

			$("#truePhoto").fadeOut(0).attr("src","/images/avatar-big.gif").fadeIn(300);

			$('#myModal').modal('hide');

			$("#loadPhoto").val("").next("span").text("Загрузить фото");
		});	
	});		

	// fileAPI function for changing and validating user photo

	$("#loadPhoto").on("change", function(e){

		var imgs = e.target.files;

		// if(imgs.length == 1){

		if(imgs.length){

			if(imgs[0].size < (500 * 1024)){

				var type = imgs[0].type.split("/")[1];

				if(type == "jpeg" || type == "gif" || type == "bmp"|| type == "png"){

					var img = new FileReader();

					$(img).on("load",(function(){

						return function(ev){

							// $("#testImage").attr("src",ev.target.result);

							// var height = document.getElementById("testImage").naturalHeight;
							// var width = document.getElementById("testImage").naturalWidth;

							// if(height < 501 && width < 201){

								$("#truePhoto").fadeOut(0);

								$(e.target).next("span").text(e.target.files[0].name).siblings("b").hide();

								$("#saveChanges").attr("data-src",$("#truePhoto").attr("src")).removeClass("disabled").attr("data-target","#myModal");

								$("#truePhoto").attr("src",ev.target.result).fadeIn(300);
							// }
							// else{

							// 	$(e.target).siblings("b").text("Изображение должно быть не больше 200x500px").fadeIn(300);
							// }
						}

					})(imgs[0]))

					img.readAsDataURL(imgs[0]);
				}
				else{

					$(e.target).siblings("b").text("Неверный формат файла. Выберите .jpeg, .png, .gif или .bmp").fadeIn(300);
				}
			}
			else{

				$(e.target).siblings("b").text("Вес файла не может превышать 0.5Mb").fadeIn(300);
			}
		}
		// }
		// else{

		// 	$(e.target).siblings("b").text("Вы можете загрузить только одно фото").fadeIn(300);
		// }
	});

	// change information click

	$("#change").on("click", function(){

		$(".infoInput").closest("label").show();

		$(this).fadeOut(0);

		$(".infoInput").removeAttr("disabled").css({

			"outline":"1px solid #000",
			"border-bottom":"1px solid transparent"
		})[0].focus();

		$(".photoBut").fadeIn(300);

		$(".correct").animate({"opacity":"1"});

		$(this).siblings("button").fadeIn(300);

		$("a[href]").each(function(ind,item){

			$(this).attr("data-href",$(this).attr("href")).attr("href","#test");
		});

		$("a[href]").attr("data-toggle","modal").attr("data-target","#myModal").on("click", function(e){

			var self = this;

			e.preventDefault();

			$(".modal-body p").text("Данные не будут сохранены. Выйти ?");

			$("#saveChangesModal").click(function(){

				location.pathname = $(self).attr("data-href");
			});
		})
	});

	$(".disabled").attr("data-target","").on("click", function(e){

		e.preventDefault();

	});

	$(".infoInput").each(function (ind, item){

		var oldVal = $(this).val();

		$(this).on("keyup", function(){

			var newVal = $(this).val();

			if(newVal != oldVal){

				$("#saveChanges").removeClass("disabled").attr("data-target","#myModal");
			}
		})
		// $("#cancelFromEdit").attr("data-toggle","modal").attr("data-target","#myModal");
	});

	// cancel from editting to contacts page

	$("#cancelFromEdit").on("click", function(e){

		// if(changes){

			e.preventDefault();

			$(".modal-body p").text("Данные не будут сохранены. Выйти ?");

			$("#saveChangesModal").off();

			$("#saveChangesModal").on("click",function(e){

				location.reload();
			});
		// }
	});

	// save changes button

	$("#saveChanges").click(function(e){

		var self = this;

		// e.preventDefault();

		var valid = validate();

		if(valid){

			if($(this).attr("data-src")){

				$(".modal-body p").text("Текущее фото будет удалено. Продолжить ?");

				$("#saveChangesModal").off().on("click",function(e){

					// callback after request

					function okRequest(){

						$(".modal-body p").text("Ваши данные были изменены");

						$("#notSaveChanges").hide(0);

						$("#saveChangesModal").text("OK").off().on("click", function(){

							location.reload();
						});	
					}

					sendData(okRequest);
				});

				$("#notSaveChanges").on("click", function(){

					$("#truePhoto").attr("src",$(self).attr("data-src"));

					$("#loadPhoto + span").text("Загрузить фото");

					$(self).removeAttr("data-src");
				})
			}
			else{

				$(".modal-body p").text("Данные будут изменены. Сохранить ?");

				$("#saveChangesModal").off().on("click",function(e){

					function okRequest(){

						$(".modal-body p").text("Ваши данные были изменены");

						$("#notSaveChanges").hide(0);

						$("#saveChangesModal").text("OK").off().on("click", function(){

							location.reload();
						});		
					}				

					sendData(okRequest);		
				});
			}	
		}	
		else{
			$(this).attr("data-target","");
		}	
	});

	// validate text inputs

	$(window).on("keydown", function(e){

		if(e.keyCode == 13){

			e.preventDefault();
		}
	})


});