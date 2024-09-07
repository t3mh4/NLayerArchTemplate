'use strict';
class aufw_modal {

    #$mdl = undefined;
    #$spinner = undefined;
    constructor(settings) {
        let defSettings = {
            id: "",
            width: "600px",
            footer: {
                html: "",
                btnCancel: "İptal"
            }
        };
        $.extend(true, defSettings, settings);
        let modalHtml = '<div class="modal fade" id="coreModal-' + defSettings.id + '" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="true"> \
                        <div class="modal-dialog" style="max-width:'+ defSettings.width + '">\
                            <div class="modal-content">\
                                <div class="modal-header">\
                                    <h5 class="modal-title">-</h5>\
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">\
                                        <span aria-hidden="true">&times;</span>\
                                    </button>\
                                </div>\
                                <div class="modal-body">\
                                    <input type="hidden" id="modal-data"/>\
                                    <div style="overflow-y:hidden;height:calc(100vh - 15rem);">\
                                        <div class="px-2" style="height:100%;" id="modal-inner-body">\
                                        </div>\
                                        <div class="loader-container">\
                                            <div class="loader"></div>\
                                        </div>\
                                    </div>\
                                </div>\
                                <div class="modal-footer">\
                                    '+ defSettings.footer.html + '\
                                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal">'+ defSettings.footer.btnCancel + '</button>\
                                </div>\
                            </div>\
                        </div>\
                    </div>'
        this.#$mdl = $(modalHtml);
        document.body.appendChild(this.#$mdl[0]);
        this.#$spinner = this.#$mdl.find(".loader-container");
        this.#$mdl.on('hide.bs.modal', this.#hidden);
    }

    show = async (options, callback) => {
        let defOptions = {
            title: "",
            html: "",
            axiosRequest: {
                controller: undefined,
                action: undefined,
                data: undefined
            }
        };
        this.#showSpinner();
        $.extend(true, defOptions, options);
        this.#$mdl.find(".modal-title").text(defOptions.title);
        let html = "";
        this.#$mdl.modal('show');
        setTimeout(async () => {
            try {
                if (defOptions.html) {
                    html = defOptions.html;
                }
                else {
                    let axs = new axios_request();
                    await axs.post_async(options.axiosRequest, (response) => {
                        html = response;
                    });
                }
                this.#hideSpinner();
                if (!html) return;
                this.$body.html(html);
                if (callback && typeof (callback) == "function") {
                    await callback();
                }
            }
            catch (error) {
                this.#hideSpinner();
                console.cError(error);
            }
        }, 500);
    }

    hide = async () => {
        this.#$mdl.modal('hide');
    }

    #hidden = async () => {
        this.#clear();
    }

    get $footer() {
        return this.#$mdl.find(".modal-footer");
    }

    get $body() {
        return this.#$mdl.find(".px-2");
    }

    set data(val) {
        let data = document.getElementById("modal-data");
        data.value = val;
    }

    #showSpinner() {
        this.#$spinner.show();
    }

    #hideSpinner() {
        this.#$spinner.hide();
    }

    #clear() {
        this.$body.empty();
    }
}
