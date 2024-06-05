'use strict';

aufw(() => {
    let queryString = window.location.search;
    let parameters = new URLSearchParams(queryString);
    let returnUrl = parameters.get("ReturnUrl");
    $("#Username").focus();

    let sb = new aufw_spinner_button("btnLogin");

    sb.click_event = async () => {
        let data = form.toObject(document.querySelector("form"));
        let axs = new axios_request();
        await axs.post_async({
            controller: "Account",
            action: "Login",
            data: { Data: data } 
        }, (response) => {
            let msg = new message();
            if (response.IsSuccess) {
                msg.hidden = () => {
                    window.location.href = returnUrl ?? "";
                }
                msg.success({ message: response.Message , timeOut: 2000 });
            }
            else
                msg.error({ message: response.Message, timeOut: 2000 });
        });
    };

    $(document).on("keypress", "form", function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            sb.click();
        }
    });
});