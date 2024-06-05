"use strict"

//get value from nested objects
Object.deepGet = (obj, path, separator = '.') => {
    var properties = Array.isArray(path) ? path : path.split(separator);
    return properties.reduce((prev, curr) => prev?.[curr], obj);
}

Number.prototype.isFloat = (n) => {
    return typeof n == 'number' && !isNaN(n) && !Number.isInteger(n)
}

String.prototype.isEmpty = (str) => {
    return typeof str !== "string" || str === "";
}

Array.prototype.isEmpty = (arr) => {
    return !Array.isArray(arr) || !arr || arr.length === 0;
}


console.cError = function (message) {
    const error = new Error();
    const callerLine = error.stack.split('\n')[2].trim();
    console.log(`%c${message} (${callerLine})`,'background-color: white; color: red; font-style: italic;');
};

console.cInfo = function (message) {
    const error = new Error();
    const callerLine = error.stack.split('\n')[2].trim();
    console.log(`%c${message} (${callerLine})`, 'background-color: white; color: green; font-style: italic;');
};

//HTMLFormElement.prototype.getModifiedInputName = function () {
//    $(this).on('change paste', 'input, select, textarea', (e) => {
//        let property = e.target.name;//this.name.substring(this.name.lastIndexOf('.') + 1, this.name.length);
        
//    });
//};