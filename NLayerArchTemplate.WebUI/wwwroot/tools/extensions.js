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
    let data = Object.fromEntries(formData);
    var numberInputs = this.querySelectorAll("input[data-input-type='number']");
    for (var i = 0; i < numberInputs.length; i++) {
        let datax = data[numberInputs[i].name];
        data[numberInputs[i].name] = datax ? datax.replaceAll('.', '') : "0";
    }
    var selects = this.querySelectorAll("select[data-input-type='multiple-select']");
    for (var i = 0; i < selects.length; i++) {
        data[selects[i].name] = $(selects[i]).val().toString();
    }
    return data;
};

//HTMLFormElement.prototype.getModifiedInputName = function () {
//    $(this).on('change paste', 'input, select, textarea', (e) => {
//        let property = e.target.name;//this.name.substring(this.name.lastIndexOf('.') + 1, this.name.length);

//    });
//};