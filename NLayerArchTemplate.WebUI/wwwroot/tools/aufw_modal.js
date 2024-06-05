'use strict';

class aufw_modal {

    #modalHtml = '<div class="modal fade" id="coreModal" tabindex="-1" role="dialog" aria-labelledby="coreModal" data-backdrop="static" data-keyboard="true" > \
                        <div class="modal-dialog" role="document" >\
                            <div class="modal-content">\
                                <div class="modal-header">\
                                    <h5 class="modal-title">-</h5>\
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">\
                                        <span aria-hidden="true">&times;</span>\
                                    </button>\
                                </div>\
                                <div class="modal-body">\
                                </div>\
                                <div class="modal-footer">\
                                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal">İptal</button>\
                                </div>\
                            </div>\
                        </div>\
                    </div>';

    #$mdl = undefined;

    constructor() {
        this.#$mdl = $(this.#modalHtml);
        document.body.appendChild(this.#$mdl[0]);
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

        $.extend(true, defOptions, options);

        this.#$mdl.find(".modal-title").text(options.title);
        let html = "";
        if (defOptions.html) {
            html = defOptions.html;
        }
        else {
            let axs = new axios_request();
            await axs.post_async(options.axiosRequest, (response) => {
                html = response;
            });
        }
        if (!html) return;
        this.#$mdl.find(".modal-body").html(html);
        this.#$mdl.modal('show');
    }

    hide = async () => {
        this.#$mdl.modal('hide');
    }

    get $footer() {
        return this.#$mdl.find(".modal-footer");
    }
}
