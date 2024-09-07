'use strict';

aufw(async () => {

    moment.locale("tr");

    let axs = new axios_request();

    $("#btnLogout").click(async (e) => {
        e.preventDefault();
        await axs.post_async({
            controller: "Account",
            action: "Logout",
        }, (response) => {
            window.location.href = response.ReturnUrl;
        });
    });
});