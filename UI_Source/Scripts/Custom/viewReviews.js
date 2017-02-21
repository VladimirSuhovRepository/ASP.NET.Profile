$(document).ready(function(){

	// skills tabs highlighting with click

	$(document).on("click", ".skillTab,.roleTabs li:not(.disabled) a", function(){

		$(this).addClass("active").siblings().removeClass("active")
	})

	// team skills tabs navigation

	function skillHash(skillName){

		$(".disabled a").removeAttr("href");

		if(skillName){

			$(".skillsTabs li:nth-child(" + +skillName.replace(/\D+/g,"") + ")").addClass("active").siblings().removeClass("active");

			$(".skillReviewList").fadeOut(0)
			$("#" + skillName).fadeIn(200);	
		}				
	};

	// navigation events

	window.onhashchange = hashView;
	hashView();

	// main navigation function

	function hashView(){

		var hash = location.hash.substring(1).split("&");

		if(hash[1]){

			if($("#" + hash[1]).length) skillHash(hash[1]);

			else{
				getTab(hash);
			}
		}
		else if(hash[0]){

			getTab(hash);
		}
		else{
			return
		}		
	}

	// request for main tabs

	function getTab(hashArr){

		$("[href*='" + hashArr[0] + "']").closest("li").addClass("active").siblings().removeClass("active");

		var id = $(".prf-questionary-up").attr("traineeId");

		console.log("/review/get" + hashArr[0] + "/" + id);

		$.ajax({

			type:"GET",
			// url: "/review/get" + hashArr[0] + "/" + id,
			url: hashArr[0] + ".html",

			success: function(data){

				$(".reviewWrapper").empty().append(data);

				skillHash(hashArr[1]);
			}
		})
	}
})