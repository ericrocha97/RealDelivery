﻿$(function () {
	$("input[type='password'][data-eye]").each(function (i) {
		let $this = $(this);

		$this.wrap($("<div/>", {
			style: 'position:relative'
		}));
		$this.css({
			paddingRight: 60
		});
		$this.after($("<div/>", {
			html: 'Exibir',
			class: 'btn btn-danger btn-sm',
			id: 'passeye-toggle-' + i,
			style: 'position:absolute;right:10px;top:50%;transform:translate(0,-50%);padding: 2px 7px;font-size:12px;cursor:pointer;'
		}));
		$this.after($("<input/>", {
			type: 'hidden',
			id: 'passeye-' + i
		}));
		$this.on("keyup paste", function () {
			$("#passeye-" + i).val($(this).val());
		});
		$("#passeye-toggle-" + i).on("click", function () {
			if ($this.hasClass("Exibir")) {
				$this.attr('type', 'password');
				$this.removeClass("Exibir");
				$(this).removeClass("btn-outline-danger");
			} else {
				$this.attr('type', 'text');
				$this.val($("#passeye-" + i).val());
				$this.addClass("Exibir");
				$(this).addClass("btn-outline-danger");
			}
		});
	});
});