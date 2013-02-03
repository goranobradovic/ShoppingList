(function () {
	init();

	function init() {
		/// <summary>
		/// stupid workaround to register ko handlers when ko is initialized
		/// </summary>
		if (window.ko) {
			registerHandlers(window.ko);
		}
		else {
			setTimeout(10, init);
		}
	}

	function registerHandlers(ko) {
		/// <summary>
		/// registers binding handlers for knockout
		/// </summary>
		/// <param name="ko">knockout</param>
		ko.bindingHandlers.showModal = {
			init: function (element, valueAccessor) {
			},
			update: function (element, valueAccessor) {
				var value = valueAccessor();
				if (value()) {
					$(element).modal('show');
					$("input", element).focus();
				}
				else {
					$(element).modal('hide');
				}
			}
		};

		ko.bindingHandlers.inplaceEdit = {

		};
	}
}());