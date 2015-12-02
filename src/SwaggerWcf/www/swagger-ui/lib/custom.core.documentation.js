$(document).ready(function(){

	var $container = $("#resources_container"),
		$resNav = $("#resources_nav"),
		$main = $resNav.find(".main-content");

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

	$container.find("h2").each(function(i,e){
		var $this = $(e),
			comAcento = $this.text(),
			semAcento = remover_acentos(comAcento);
		$this.attr("id",semAcento);
		$main.append("<div data-endpoint='"+semAcento+"' class='item select-item'>"+comAcento+"</div>");
		if(i===0) $main.find(".item").attr("data-selected","");
	});

	$(document).on("click",".select-item",function(e){
		e.preventDefault();
		var $this = $(this),
			endpoint = $this.data("endpoint"),
			$title = $("#"+endpoint);

		$main.find("div[data-selected]").removeAttr("data-selected");
		$this.attr("data-selected","");

		$('html, body').animate({
			scrollTop: $title.parent().offset().top
		}, 300);

	});

});