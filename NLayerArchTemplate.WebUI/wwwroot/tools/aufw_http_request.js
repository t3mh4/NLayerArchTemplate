'use strict';
class aufw_http_request {

    #errorTypes = {
        Default: 0,
        Validation: 1,
        HttpRequest: 2,
        Authorization: 3
    };

    #config = {
        headers: {
            "X-HttpRequest-Token": "",
            "X-Requested-With":"",
            "Accept": "application/json",
        }
    };

    get = (options, callback) => {
        let getOptions = this.#getGetOptions(options);
        axios.get(getOptions.url, getOptions.params, this.#config)
            .then(response => { callback(response.data); })
            .catch(err => {
                this.#handleError(err);
            });
    }

    get_async = async (options, callback) => {
        let getOptions = this.#getGetOptions(options);
        let response = await axios.get(getOptions.url, getOptions.params, this.#config)
            .catch(err => {
                this.#handleError(err);
            });
        if (response)
            callback(response.data);
    }

    post = (options, callback) => {
        let postOptions = this.#getPostOptions(options);
        axios.post(postOptions.url, postOptions.data, this.#config)
            .then(response => { callback(response.data); })
            .catch(err => {
                this.#handleError(err);
            });
    }

    post_async = async (options, callback) => {
        let postOptions = this.#getPostOptions(options);
        let response = await axios.post(postOptions.url, postOptions.data, this.#config)
            .catch(err => {
                this.#handleError(err);
            });
        if (response)
            callback(response.data);
    }

    #getPostOptions(options) {
        let defOptions = {
            url: "",
            data: undefined,
        };
        defOptions.url = '/' + options.controller + "/" + options.action;
        defOptions.data = options.data;
        this.#config.headers["X-HttpRequest-Token"] = $("input[name='RequestVerificationToken']").val();
        this.#config.headers["X-Requested-With"] = "POST";
        return defOptions;
    }

    #getGetOptions(options) {
        let defOptions = {
            url: "",
        };
        this.#config.headers["X-Requested-With"] = "GET";
        defOptions.url = '/' + options.controller + "/" + options.action;
        defOptions.params =  options.params ;
        return defOptions;
    }

    #handleError(err) {
        if (err.response) {
            if (err.response.data.Data)//ErrorController.Handle'dan gelen hataları gösteriyoruz
                this.#showError(err.response.data);
            else {//Manuel hataları gösteriyoruz
                let msg = new aufw_message();
                msg.error({ message: err.response.data.Message });
            }
            // The client was given an error response (5xx, 4xx)
        } else if (err.request) {
            console.cError(err.request);
            // The client never received a response, and the request was never left
        } else {
            console.cError(err);
            let msg = new aufw_message();
            msg.error({ message: "Beklenmedik bir hata ile karşılaşıldı..!!-3" });
            // Anything else
        }
    }

    #showError(reponseData) {
        if (reponseData.Data.ErrorType == this.#errorTypes.Validation) {
            this.#showValidationErrors(JSON.parse(reponseData.Data.Message));
        }
        else if (reponseData.Data.ErrorType == this.#errorTypes.HttpRequest) {
            this.#showHttpRequestError(reponseData);
        }
        else if (reponseData.Data.ErrorType == this.#errorTypes.Authorization) {
            this.#showAuthorizationError(reponseData);
        }
        else {
            let msg = new aufw_message();
            msg.error({ message: reponseData.Data.Message });
        }
    }

    #showValidationErrors(errors) {
        let text = "";
        errors.forEach(function (error, index) {
            text = text.concat("-&nbsp;&nbsp;" , error.ErrorMessage , "<br/>")
        });
        let msg = new aufw_message();
        msg.error({ message: text });
    }

    #showHttpRequestError(reponseData) {
        let msg = new aufw_message();
        msg.error({ message: reponseData.Data.Message + " (HttpRequest)" });
    }

    #showAuthorizationError(reponseData) {
        let msg = new aufw_message();
        msg.hidden = () => {
            window.location.href = reponseData.ReturnUrl;
        }
        msg.error({ message: reponseData.Message });
    }
}
