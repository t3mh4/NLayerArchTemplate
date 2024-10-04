"use strict";

window.aufw = async function (callback) { //aufw constructor
    if (document.readyState === 'complete' || (document.readyState !== 'loading' && !document.documentElement.doScroll)) {
        await callback();
    } else {
        document.addEventListener('DOMContentLoaded', await callback);
    }
    console.cInfo("DOM State is " + document.readyState);
};

aufw.init = () => {
    if (!aufw.loading)
        aufw.loading = new aufw_loading();
    if (!aufw.jstree) {
        aufw.jstree = new aufw_jstree();
    }
}