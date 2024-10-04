/*! © SpryMedia Ltd - datatables.net/license */

(function (factory) {
	if (typeof define === 'function' && define.amd) {
		// AMD
		define(['jquery', 'datatables.net'], function ($) {
			return factory($, window, document);
		});
	}
	else if (typeof exports === 'object') {
		// CommonJS
		var jq = require('jquery');
		var cjsRequires = function (root, $) {
			if (!$.fn.dataTable) {
				require('datatables.net')(root, $);
			}
		};

		if (typeof window === 'undefined') {
			module.exports = function (root, $) {
				if (!root) {
					// CommonJS environments without a window global must pass a
					// root. This will give an error otherwise
					root = window;
				}

				if (!$) {
					$ = jq(root);
				}

				cjsRequires(root, $);
				return factory($, root, root.document);
			};
		}
		else {
			cjsRequires(window, jq);
			module.exports = factory(jq, window, window.document);
		}
	}
	else {
		// Browser
		factory(jQuery, window, document);
	}
}(function ($, window, document, undefined) {
	'use strict';
	var DataTable = $.fn.dataTable;

	DataTable.render.moment = function (from, to, locale) {
		// Argument shifting
		if (arguments.length === 1) {
			to = from;
			from = 'YYYY-MM-DD';
		}
		return function (d, type, row) {
			if (!d) {
				return type === 'sort' || type === 'type' ? 0 : d;
			}
			var m = window.moment(d, from, locale, true);
			// Order and type get a number value from Moment, everything else
			// sees the rendered value
			return m.format(type === 'sort' || type === 'type' ? 'x' : to);
		};
	};

	DataTable.render.moment_tr = function (from = 'DD.MM.YYYY HH:mm:ss', to = 'DD.MM.YYYY HH:mm:ss') {

		return function (d, type, row) {
			if (!d) {
				return type === 'sort' || type === 'type' ? 0 : d;
			}
			var m = window.moment(d, from);
			// Order and type get a number value from Moment, everything else
			// sees the rendered value
			return m.format(type === 'sort' || type === 'type' ? 'x' : to);
		};
	};

	DataTable.render.check_box = function (columnName) {

		return function (d, type, row) {
			let isTrue = false;
			if (d && (d === true || d === 'true' || d === 'True')) {
				isTrue = true;
			}
			columnName = columnName || "dt_checkbox";
			return '<div class="icheck-primary d-inline" style="pointer-events:none;"><input type="checkbox" id="' + columnName + '" name="' + columnName + '" ' + (isTrue ? 'checked' : '') + '><label for="' + columnName + '"></label></div>';
			//return '<input class="checkbox-readonly" name="' + columnName + '" type="checkbox" ' + (isTrue ? 'checked' : '') + '>';
		};
	};

}));