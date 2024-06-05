//'use strict';

//class aufw_crud {

//    #type = "";
//    #modifiedProperties = undefined;

//    constructor() {
//    }

//    add = async (options) => {
//        let defOptions = this.#getAddDefaultOptions(options);
//        await this.#mdl.show({
//            title: defOptions.title,
//            axiosRequest: defOptions.partial
//        });
//        this.#type = "add";
//    }

//    update = async (options) => {
//        //this.#modifiedProperties = [];
//        let defOptions = this.#getUpdateDefaultOptions(options);
//        if (defOptions.pkValue != undefined && defOptions.pkValue < 1) {
//            let msg = new message();
//            msg.warning({ message: "Lütfen Kayıt Seçiniz..!!" });
//            return;
//        }

//        await this.#mdl.show({
//            title: defOptions.title,
//            axiosRequest: defOptions.partial,
//        });
//        this.#collectmodifiedProperties();
//        this.#type = "update";
//    }

//    #getAddDefaultOptions(options) {
//        let defOptions = {
//            $btn: undefined,
//            title: "Yeni Kayıt",
//            partial: {
//                controller: undefined,
//                action: undefined,
//                data: { Data: 0 }
//            }
//        }
//        $.extend(true, defOptions, options);

//        return defOptions;
//    }

//    #getUpdateDefaultOptions(options) {

//        let defOptions = {
//            title: "Güncelle",
//            pkValue: undefined,
//            partial: {
//                controller: undefined,
//                action: undefined,
//                data: undefined
//            }
//        }
//        $.extend(true, defOptions, options);

//        return defOptions;
//    }

//    save = async (options) => {
//        let defoptions = {
//            controller: undefined,
//            action: undefined,
//            data: undefined
//        };
//        $.extend(true, defoptions, options);
//        let axs = new axios_request();
//        if (!defoptions.data)
//            defoptions.data = form.toObject(document.querySelector("form"));

//        await axs.post({
//            controller: defoptions.controller,
//            action: defoptions.action,
//            data: {
//                Data: defoptions.data, modifiedProperties: this.#modifiedProperties
//            }
//        }, (response) => {
//            this.#modifiedProperties = undefined;
//            this.#mdl.hide();
//            if (response.IsSuccess) {
//                let msg = new message();
//                msg.success({ message: "İşlem başarılı..!!" });
//            }
//        });
//        //let $btnSave = $('<button type="button" class="btn btn-outline-success" id="mdlDefaultBtn">Kaydet</button>');
//        //this.#mdl.$footer.prepend($btnSave);
//        //$btnSave.on("click", this.#save_click);
//        //$btnSave.click(async () => {
//        //    let axs = new axios_request();
//        //    if (!defoptions.data)
//        //        defoptions.data = form.toObject(document.querySelector("form"));

//        //    await axs.post({
//        //        controller: defoptions.controller,
//        //        action: defoptions.action,
//        //        data: {
//        //            Data: defoptions.data, modifiedProperties: this.#modifiedProperties
//        //        }
//        //    }, (response) => {
//        //        this.#modifiedProperties = undefined;
//        //        this.#mdl.hide();
//        //    });
//        //})
//    }

//    #collectmodifiedProperties() {
//        if (this.#modifiedProperties === undefined) {
//            this.#modifiedProperties = [];
//            $("form").on('change paste', 'input, select, textarea', (e) => {
//                let property = e.target.name;//this.name.substring(this.name.lastIndexOf('.') + 1, this.name.length);
//                if (this.#modifiedProperties.indexOf(property) === -1)
//                    this.#modifiedProperties.push(property);
//            });
//        }
//    }
//    /*
//    add = async (options) => {
//        let defOptions = {
//            $btn: undefined,
//            title: "Yeni Kayıt",
//            partial: {
//                controller: undefined,
//                action: undefined,
//                data: { Data: 0 }
//            }
//        }
//        $.extend(true, defOptions, options);
//        //let mdl = new aufw_modal();
//        this.#mdl.show({
//            title: defOptions.title,
//            axiosRequest: defOptions.partial
//        });
//        this.#type = "add";
//    }

//    update = async (options) => {

//        if (options.partial.data < 1) {
//            let msg = new message();
//            msg.warning({ message: "Lütfen Kayıt Seçiniz..!!" });
//            return;
//        }

//        let defOptions = {
//            $btn: undefined,
//            title: "Güncelle",
//            partial: {
//                controller: undefined,
//                action: undefined,
//                data: undefined
//            }
//        }
//        $.extend(true, defOptions, options);
//        //let mdl = new aufw_modal();
//        this.#mdl.show({
//            title: defOptions.title,
//            axiosRequest: defOptions.partial
//        });
//        this.#type = "update";
//    }

//    save = async (options) => {
//        let defoptions = {
//            controller: undefined,
//            action: undefined,
//            data: undefined
//        };

//        $.extend(true, defoptions, options);

//        this.#mdl.save = async (options) => {
//            let axs = new axios_request();
//            if (!defoptions.data)
//                defoptions.data = form.toObject(document.querySelector("form"));
//            if (this.#type === "update") {

//            }


//            await axs.post({
//                controller: defoptions.controller,
//                action: defoptions.action,
//                data: {//burada kaldım.modifid properties'i yapacağım
//                    Data: defoptions.data, modifiedProperties: ["aa", "bb"]
//                }
//            }, (response) => {
//                this.#mdl.hide();
//            });
//        }
//    }

//    get #$form() {
//        return this.#mdl.$mdl.find("form");
//    }

//    get #btnSave() {
//        return this.#$mdl.$mdl.find("#mdlBtnSave");
//    }

//    #collectmodifiedProperties() {
//        let $form = this.#mdl.form;
//        this.#$form.on('change paste', 'input, select, textarea', function () {
//            let id = $form.find("input[name*='Id']").val();
//            if (id === "0")
//                return;
//            let property = this.name;//this.name.substring(this.name.lastIndexOf('.') + 1, this.name.length);
//            if (modifiedProperties.indexOf(property) === -1)
//                modifiedProperties.push(property);
//        });
//    }
//    */
//}