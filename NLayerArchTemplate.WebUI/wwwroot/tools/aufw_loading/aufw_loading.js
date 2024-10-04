class aufw_loading {
    #loading;
    #text = "İşleminiz devam ediyor, lütfen bekleyiniz...";
    constructor() {
        this.#loading = document.createElement('div');
        this.#loading.id = 'aufw_loading';
        this.#loading.className ="aufw_loading"
        this.#loading.innerHTML = `<div class="spinner-border text-light" role="status"></div>
                                   <p></p>`;
        document.body.appendChild(this.#loading);
    }

    show = (text) => {
        if (text) {
            this.#loading.querySelector("p").innerText = text;
        }
        else {
            this.#loading.querySelector("p").innerText = this.#text;
        }
        this.#loading.style.display = 'flex';
        //document.body.appendChild(this.#loading);
    }

    hide = () => {
        this.#loading.style.display = 'none';
        //this.#loading.remove();
        //this.#loading = undefined;
    }
}