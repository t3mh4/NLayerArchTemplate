'use strict';

aufw(async () => {
    aufw.init();

    moment.locale("tr");

    let axs = new aufw_http_request();

    $("#btnLogout").click(async (e) => {
        e.preventDefault();
        await axs.post_async({
            controller: "Account",
            action: "Logout",
        }, (response) => {
            let msg = new aufw_message();
            if (response.IsSuccess) {
                msg.hidden = () => {
                    window.location.href = response.ReturnUrl;
                }
                msg.success({ message: response.Message, timeOut: 2000 });
            }
            else
                msg.error({ message: response.Message, timeOut: 2000 });
        });
    });
});