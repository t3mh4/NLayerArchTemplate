
class aufw_spinner {
    #spinner;

    constructor() {
        this.#spinner = document.createElement("div");
        this.#spinner.className = "loader-container";
        this.#spinner.innerHTML = `<div class="loader"></div>`;
    }

    show = () => {
        this.#spinner.style.display = 'flex';
    }

    hide = () => {
        this.#spinner.style.display = 'none';
    }

    get html() {
        return this.#spinner;
    }
}