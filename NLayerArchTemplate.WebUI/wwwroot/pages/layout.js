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

    //datatable detail modal ile ilgili
    $(document).on("show.bs.modal", ".dtr-bs-modal", function () {
        let $footer = '<div class="modal-footer"><button type="button" class="btn btn-outline-secondary" data-dismiss="modal">Kapat</button></div>';
        if ($(this).find(".modal-footer").length === 0)
            $(this).find(".modal-content").append($footer);
        $(".dtr-title").append(" : ").css("font-weight", "bold");
    });

    $(document).on("hidden.bs.modal", ".dtr-bs-modal", function () {
        $(this).detach();
    });
});