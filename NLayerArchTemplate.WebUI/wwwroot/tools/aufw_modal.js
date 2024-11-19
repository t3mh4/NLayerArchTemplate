'use strict';
class aufw_modal {

    #$mdl = undefined;
    #spinner = undefined;
    #modalId = undefined;
    constructor(settings) {
        let defSettings = {
            id: "",
            width: "85vh",
            isHeightStatic: true,
            isBackdropStatic: true,
            isKeyboard: true,
            footer: {
                html: "",
                btnCancel: "İptal"
            }
        };
        $.extend(true, defSettings, settings);
        let backdrop = defSettings.isBackdropStatic ? "data-backdrop=\"static\"" : "";
        let keyboard = defSettings.isKeyboard ? "data-keyboard=\"true\"" : "";
        this.#modalId = defSettings.id;
        let modalHtml = `<div class="modal fade" id="coreModal-${defSettings.id}" tabindex="-1" role="dialog" ${backdrop} ${keyboard}> \
                        <div class="modal-dialog" style="max-width:${defSettings.width}">\
                            <div class="modal-content">\
                                <div class="modal-header">\
                                    <h5 class="modal-title">-</h5>\
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">\
                                        <span aria-hidden="true">&times;</span>\
                                    </button>\
                                </div>\
                                <div class="modal-body">\
                                    <input type="hidden" id="modal-data-${defSettings.id}"/>\
                                    <div id="modal-outer-body-${defSettings.id}" style="height:calc(100vh - 15rem);">\
                                        <div class="px-2" style="height:100%;overflow-y:hidden" id="modal-inner-body-${defSettings.id}">\
                                        </div>\
                                    </div>\
                                </div>\
                                <div class="modal-footer">\
                                    ${defSettings.footer.html}\
                                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal">${defSettings.footer.btnCancel}</button>\
                                </div>\
                            </div>\
                        </div>\
                    </div>`;
        this.#$mdl = $(modalHtml);
        this.#spinner = new aufw_spinner();
        this.#$mdl.find(".modal-body").append(this.#spinner.html);
        document.body.appendChild(this.#$mdl[0]);
        this.#$mdl.on('hide.bs.modal', this.#hidden);
    }

    show = (options, callback) => {
        let defOptions = {
            title: "",
            html: "",
            isFlexible: true,
            axiosRequest: {
                controller: undefined,
                action: undefined,
                data: undefined
            }
        };
        $.extend(true, defOptions, options);
        let modal_body = this.#$mdl.find(`#modal-outer-body-${this.#modalId}`);
        if (defOptions.isFlexible) {
            modal_body.css("height", "100px");
        }
        this.#showSpinner();
        this.#$mdl.find(".modal-title").text(defOptions.title);
        let html = "";
        this.#$mdl.modal('show');
        setTimeout(async () => {
            try {
                if (defOptions.html) {
                    html = defOptions.html;
                }
                else {
                    let axs = new aufw_http_request();
                    await axs.post_async(options.axiosRequest, (response) => {
                        html = response;
                    });
                }
                if (!html) {
                    this.#hideSpinner();
                    return;
                }
                this.$body.html(html);
                if (callback && typeof (callback) == "function") {
                    await callback();
                }
            }
            catch (error) {
                console.cError(error);
            }
            this.#hideSpinner();
            if (defOptions.isFlexible) {
                modal_body.css("height", "");
            }
        }, 500);
    }

    hide = () => {
        this.#$mdl.modal('hide');
    }

    #hidden = () => {
        this.#clear();
    }

    get $footer() {
        return this.#$mdl.find(".modal-footer");
    }

    get $body() {
        return this.#$mdl.find(`#modal-inner-body-${this.#modalId}`);
    }

    set data(val) {
        this.#$mdl.find(`#modal-data-${this.#modalId}`).val(val);
    }

    #showSpinner = () => {
        this.#spinner.show();
        //this.#$mdl.find(".loader-container").show();
    }

    #hideSpinner = () => {
        this.#spinner.hide();
        //this.#$mdl.find(".loader-container").hide();
    }

    #clear = () => {
        this.$body.empty();
    }
}
