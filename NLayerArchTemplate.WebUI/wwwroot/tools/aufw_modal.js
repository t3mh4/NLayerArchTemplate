'use strict';
class aufw_modal {

    #$mdl = undefined;
    #$spinner = undefined;
    constructor() {
        let modalHtml = '<div class="modal fade" id="coreModal" tabindex="-1" role="dialog" aria-labelledby="coreModal" data-backdrop="static" data-keyboard="true"> \
                        <div class="modal-dialog" style="max-width:600px">\
                            <div class="modal-content">\
                                <div class="modal-header">\
                                    <h5 class="modal-title">-</h5>\
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">\
                                        <span aria-hidden="true">&times;</span>\
                                    </button>\
                                </div>\
                                <div class="modal-body" style="position:relative">\
                                    <input type="hidden" id="modal-data"/>\
                                    <div style="overflow-y:hidden;height:calc(100vh - 15rem);">\
                                        <div class="px-2" style="overflow-y:auto; height:100%;">\
                                        </div>\
                                        <div class="loader-container">\
                                            <div class="loader"></div>\
                                        </div>\
                                    </div>\
                                </div>\
                                <div class="modal-footer">\
                                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal">İptal</button>\
                                </div>\
                            </div>\
                        </div>\
                    </div>'
        this.#$mdl = $(modalHtml);
        document.body.appendChild(this.#$mdl[0]);
        this.#$spinner = this.#$mdl.find(".loader-container");
    }

    show = async (options) => {
        let defOptions = {
            title: "",
            html: "",
            axiosRequest: {
                controller: undefined,
                action: undefined,
                data: undefined
            }
        };
        this.#clear();
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
                this.#$mdl.find(".px-2").html(html);
            }
            catch (error) {
                this.#hideSpinner();
                console.error(error);
            }
        }, 750);
    }

    hide = async () => {
        this.#$mdl.modal('hide');
    }

    get $footer() {
        return this.#$mdl.find(".modal-footer");
    }

    get $body() {
        return this.#$mdl.find(".modal-body");
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
        this.#$mdl.find(".px-2").empty();
    }
}
