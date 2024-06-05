'use strict';

class aufw_spinner_button {

    #jQbutton = undefined;

    #text = "";

    constructor(buttonId) {
        this.#jQbutton = $("#" + buttonId);
        this.#text = this.#jQbutton.text();
        this.#jQbutton.on("click", this.click);
    }

    #show = () => this.#jQbutton.html(`${this.#text}<i class="fas fa-sync fa-spin" style="margin-left: 10px;"></i>`);

    #hide = () => this.#jQbutton.text(this.#text);

    click = (e) => {
        this.#jQbutton.prop('disabled', true);
        this.#show();
        try {
            this.click_event();
        } catch (e) {

        }
        setTimeout(() => {
            this.#hide();
            this.#jQbutton.prop('disabled', false); }, 1000);
        return false;
    }

    click_event = () => undefined;
}