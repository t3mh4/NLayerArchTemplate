"use strict";

window.aufw = async function (callback) { //aufw constructor
	if (document.readyState === 'complete' || (document.readyState !== 'loading' && !document.documentElement.doScroll)) {
		// The DOM is already ready, so execute the callback immediately.
		await callback();
	} else {
		// Add an event listener to execute the callback when the DOM is ready.
		document.addEventListener('DOMContentLoaded', await callback);
	}
	console.cInfo("DOM State : " + document.readyState);
} || {};