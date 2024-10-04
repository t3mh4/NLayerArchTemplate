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

Error.prototype.callerLine = function (stack) {
    let urlPattern = /https?:\/\/[^\s)]+/g;
    let urls = stack.split('\n')
        .map(line => line.match(urlPattern))  // Her satırda URL arar
        .filter(match => match !== null)      // Sadece eşleşmeleri al
        .flat();
    return urls[1] == undefined ? "-" : urls[1];
}

console.cError = function (message) {
    let error = new Error();
    console.log(`%cError : ${message}\nLine : (${error.callerLine(error.stack) })`,'background-color: white; color: red; font-style: italic;');
};

console.cInfo = function (message) {
    let error = new Error
    console.log(`%c\nMessage : ${message}\nLine : (${error.callerLine(error.stack)})`, 'background-color: white; color: green; font-style: italic;');
};

HTMLFormElement.prototype.toObject = function () {
    let formData = new FormData(this);
    let data = Object.fromEntries(formData);
    for (let numberInput of this.querySelectorAll("input[data-input-type='number']")) {
        let datax = data[numberInput.name];
        data[numberInput.name] = datax ? datax.replaceAll('.', '') : "0";
    }

    for (let select of this.querySelectorAll("select[data-input-type='multiple-select']")) {
        //Spread syntax allows an iterable (...) to be expanded
        let textList = [...select.selectedOptions].map(function (option) {
            return option.text;
        });
        data[select.name] = $(select).val().toString();
        let textName = select.getAttribute("data-text-property")
        data[textName] = textList.join();
    }

    for (let treeview of this.querySelectorAll("div[data-input-type='treeview']")) {
        let nodes = $(treeview).jstree(true).get_selected(true).filter(f => !f.id.startsWith('j'));
        let ids = nodes.map(function (node) {
            return node.id;
        });
        let textList = nodes.map(function (node) {
            return node.text;
        });
        data.MahalleIds = ids.join();
        let textName = treeview.getAttribute("data-text-property")
        data[textName] = textList.join();
    }
    return data;
};

HTMLFormElement.prototype.handleInputsEventTo = function (modifiedProperties) {
    $(this).on('change paste', 'input, select, textarea', (e) => {
        let property = e.target.name;
        if (modifiedProperties.indexOf(property) === -1)
            modifiedProperties.push(property);
    });
};