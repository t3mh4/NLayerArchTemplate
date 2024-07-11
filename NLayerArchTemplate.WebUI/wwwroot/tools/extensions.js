"use strict"

//get value from nested objects
Object.deepGet = (obj, path, separator = '.') => {
    var properties = Array.isArray(path) ? path : path.split(separator);
    return properties.reduce((prev, curr) => prev?.[curr], obj);
}

Number.prototype.isFloat = function(){
    return typeof this == 'number' && !isNaN(this) && !Number.isInteger(this)
}

String.prototype.isEmpty = function () {
    return typeof this !== "string" || this === "";
}

Array.prototype.isEmpty = function () {
    return !Array.isArray(this) || !this || this.length === 0;
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

HTMLFormElement.prototype.toObject = function () {
    let formData = new FormData(this);
    return Object.fromEntries(formData);
};

//HTMLFormElement.prototype.getModifiedInputName = function () {
//    $(this).on('change paste', 'input, select, textarea', (e) => {
//        let property = e.target.name;//this.name.substring(this.name.lastIndexOf('.') + 1, this.name.length);
        
//    });
//};