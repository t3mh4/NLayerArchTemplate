'use strict';

aufw(() => {
    $("#Username").focus();

    let sb = new aufw_button_spinner("btnLogin");

    sb.click_event = async () => {
        let data = document.querySelector("form").toObject();
        let req = new aufw_http_request();
        await req.post_async({
            controller: "Account",
            action: "Login",
            data: { Data: data } 
        }, (response) => {
            let msg = new aufw_message();
            if (response.IsSuccess) {
                msg.hidden = () => {
                    window.location.href = "/Home";
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