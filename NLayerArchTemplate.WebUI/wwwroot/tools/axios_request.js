'use strict';

class axios_request {

    #errorTypes = {
        Default: 0,
        Validation: 1,
        Session: 2,
        Authorization: 3,
        NotFound: 4,
    };

    #config = {
        headers: {
            "X-HttpRequest-Type": "Axios",
            "X-HttpRequest-Token": "",
            "X-Requested-With":"",
            "Accept": "application/json",
        }
    };

    //this classic method is deprecated by me
    //post = async (options) => {
    //    let postOptions = this.#getPostOptions(options);

    //    try {
    //        let response = await axios.post(postOptions.url, postOptions.data, this.#config)
    //        this.after_response(response.data);
    //    } catch (err) {
    //        if (err.response) {
    //            this.#showError(err.response.data.Data);
    //            // The client was given an error response (5xx, 4xx)
    //        } else if (err.request) {
    //            console.log(err.request);
    //            // The client never received a response, and the request was never left
    //        } else {
    //            console.log("Beklendik bir hata ile karşılaşıldı..!!");
    //            // Anything else
    //        }
    //    }
    //}

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

    //after_response = (response) => undefined;

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
            //params: undefined,
        };
        this.#config.headers["X-Requested-With"] = "GET";
        defOptions.url = '/' + options.controller + "/" + options.action;// + '?' + new URLSearchParams(options.params).toString();;
        defOptions.params =  options.params ;
        return defOptions;
    }

    #handleError(err) {
        //err.response.data = HttpResponse -> Data,Message,IsSuccess,ReturnUrl
        if (err.response) {
            if (err.response.data.Data)//Galobal Error'den gelen hataları gösteriyoruz
                this.#showError(err.response.data);
            else {//Manuel hataları gösteriyoruz
                let msg = new message();
                msg.error({ message: err.response.data.Message });
            }
            // The client was given an error response (5xx, 4xx)
        } else if (err.request) {
            console.cError(err.request);
            // The client never received a response, and the request was never left
        } else {
            console.cError(err);
            let msg = new message();
            msg.error({ message: "Beklendik bir hata ile karşılaşıldı..!!" });
            // Anything else
        }
    }

    #showError(reponseData) {
        if (reponseData.Data.ErrorType == this.#errorTypes.Validation) {
            this.#showValidationErrors(JSON.parse(reponseData.Data.Message));
        }
        else if (reponseData.Data.ErrorType == this.#errorTypes.Session) {
            this.#showSessionError(reponseData);
        }
        else {
            let msg = new message();
            msg.error({ message: reponseData.Data.Message });
            console.cError("Message : " + reponseData.Data.Message);
            console.cError("StackTrace : " + reponseData.Data.StackTrace);
        }
    }

    #showValidationErrors(errors) {
        let text = "";
        errors.forEach(function (error, index) {
            text += error.ErrorMessage + "<br/>"
        });
        let msg = new message();
        msg.error({ message: text });
    }

    #showSessionError(reponseData) {
        let msg = new message();
        msg.hidden = () => {
            window.location.href = reponseData.ReturnUrl;
        }
        msg.error({ message: reponseData.Message });
    }
}

class form {
    static toObject(form) {
        let formData = new FormData(form);
        return Object.fromEntries(formData);
    }
}
