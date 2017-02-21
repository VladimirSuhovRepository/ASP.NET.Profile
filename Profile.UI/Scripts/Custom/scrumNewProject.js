$(document).ready(function(){
  $("#scrumProjectInfo").validate();

  mask(".textField", (/[~@#$%^\]\[&*=|/}{]+/ig));

  $('.textFieldRequired').each(function () {
      $(this).rules('add', {
          required: true,
          messages: {
              required: "Это обязательное поле",
          }
      });
  });

  $('#saveButton').click(function () {

	  if($("#scrumProjectInfo").valid())
	  {
		console.log("validation ok");
        $('#readyConfirm').removeClass('hide');
        $('#readyConfirm').modal('show'); 
	  };
		 
   });

  $(".close_modal").click(function () {
      $('#readyConfirm').addClass('hide');
      $('#readyConfirm').modal('hide');
  })

});