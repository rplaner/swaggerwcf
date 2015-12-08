$(document).ready(function(){
	setTimeout(function(){
		$(".modal").on("shown.bs.modal",function(e,modal){
			console.log("shown.bs.modal",$(this));

			var $modal = $(this),
				$response = $modal.find(".response"),
				$blocks = $response.find(".block");
			$blocks.each(function(i,e){
				var $ele = $(e);
				if($ele.hasClass("response_headers")){
					$ele.addClass("closed");
					var $h5 = $ele.prev();
					$h5.addClass("title closed");
				}
			});

			$(".title").click(function(){
				$(this).toggleClass("closed");
				$(this).next().toggleClass("closed");
			});
			$(".block").click(function(){
				$(this).toggleClass("closed");
				$(this).prev().toggleClass("closed");
			})
		});
		$(".param-property[data-label='body']").attr("data-label","Body");
	},1000);

});