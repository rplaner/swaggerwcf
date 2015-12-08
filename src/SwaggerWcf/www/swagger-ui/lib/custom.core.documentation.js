$(document).ready(function(){

	var $container = $(".resources_container"),
		$resNav = $("#resources_nav");

	function remover_acentos(palavra){
		com_acento = 'áàãâäéèêëíìîïóòõôöúùûüçÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÖÔÚÙÛÜÇ ';
		sem_acento = 'aaaaaeeeeiiiiooooouuuucAAAAAEEEEIIIIOOOOOUUUUC-';
		nova='';
		for(i=0;i<palavra.length;i++) {
			if (com_acento.search(palavra.substr(i,1))>=0) {
				nova+=sem_acento.substr(com_acento.search(palavra.substr(i,1)),1);
			}
			else {
				nova+=palavra.substr(i,1);
			}
		}
		return nova;
	}

	$container.each(function(i,e){
		var $this = $(e),
			dataMainTopicLabel = $this.data("main-topic-label");

		$resNav.append('<div data-resource="resource_pet" label="'+dataMainTopicLabel+'" class="'+remover_acentos(dataMainTopicLabel)+' active main-content"></div>');

		$this.find("h2").each(function(j,e){
			var $this = $(e),
				comAcento = $this.text(),
				semAcento = remover_acentos(comAcento),
				$main = $resNav.find("."+remover_acentos(dataMainTopicLabel));
			$this.attr("id",semAcento);
			$main.append("<div data-endpoint='"+semAcento+"' class='item select-item'>"+comAcento+"</div>");
			if(i===0 && j===0) $main.find(".item").attr("data-selected","");
		});
	});


	$(document).on("click",".select-item",function(e){
		e.preventDefault();
		var $this = $(this),
			endpoint = $this.data("endpoint"),
			$title = $("#"+endpoint);

		$resNav.find("div[data-selected]").removeAttr("data-selected");
		$this.attr("data-selected","");

		$('html, body').animate({
			scrollTop: $title.parent().offset().top-85
		}, 300);

	});
});