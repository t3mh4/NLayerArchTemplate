'use strict';

aufw(async () => {

    moment.locale("tr");

    let req = new aufw_http_request();

    $("#btnLogout").click(async (e) => {
        e.preventDefault();
        await req.post_async({
            controller: "Account",
            action: "Logout",
        }, (response) => {
            window.location.href = response.ReturnUrl;
        });
    });
});