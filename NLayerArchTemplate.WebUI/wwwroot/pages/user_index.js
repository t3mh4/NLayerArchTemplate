aufw(async () => {
    let modifiedProperties = [];
    let datatable = new aufw_datatable();
    let controller = "User";
    let getAllAction = "GetAll";
    let coreAction = "CorE";
    let deleteAction = "Delete";
    let saveAction = "Save";
    let msg = new aufw_message();
    await datatable.load({
        $dt: $("#tblKullanici"),
        axiosRequest: {
            controller: controller,
            action: getAllAction
        },
        dataTable: {
            columns: [
                { data: "Id", title: "#", width: "20" },
                { data: "Username", title: "Kullanıcı Adı" },
                { data: "Name", title: "Ad" },
                { data: "Surname", title: "Soyad" },
                { data: "Email", title: "Email" },
                {
                    data: "IsActive", title: "Aktif",
                    searchable: false,
                    width: "20",
                    render: DataTable.render.check_box(),
                }
            ]
        }
    });
    //------------------------------------------------------
    let mdl = new aufw_modal({
        id: "crud",
        footer: {
            html: '<button type="button" id="btnSave" name="btnSave" class="btn btn-outline-success">Kaydet</button>',
        }
    });
    //------------------------------------------------------
    $("#btnAdd").click(() => {
        mdl.show({
            numOfInputs: 6,
            title: "Yeni Kayıt",
            axiosRequest: {
                controller: controller,
                action: coreAction,
                data: { Data: 0 }
            }
        });
    });

    $("#btnEdit").click(() => {
        let id = datatable.id;
        if (id < 1) {
            msg.warning({ message: "Lütfen Kayıt Seçiniz..!!" });
            return;
        }
        mdl.show({
            numOfInputs: 6,
            title: "Güncelle",
            axiosRequest: {
                controller: controller,
                action: coreAction,
                data: { Data: id }
            }
        }, async () => {
            mdl.$body[0].querySelector("form").handleInputsEventTo(modifiedProperties);
        });
    });

    $("#btnDelete").click(async () => {
        let id = datatable.id;
        if (id < 1) {
            msg.warning({ message: "Lütfen Kayıt Seçiniz..!!" });
            return;
        }
        let swal = await Swal.fire({
            title: 'Kayıt No : ' + id,
            text: 'Seçili kaydı silmek istediğinize emin misiniz?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Evet',
            cancelButtonText: 'Hayır'
        });
        if (swal.isConfirmed) {
            let req = new aufw_http_request();
            await req.post_async({
                controller: controller,
                action: deleteAction,
                data: { Data: id }
            }, (response) => {
                if (response.IsSuccess) {
                    msg.success({ message: response.Message });
                }
                else {
                    msg.error({ message: response.Message });
                }
                datatable.refresh();
            });
        }
    });

    $("#btnSave").click(async () => {
        let data = mdl.$body[0].querySelector("form").toObject();
        let req = new aufw_http_request();
        await req.post_async({
            controller: controller,
            action: saveAction,
            data: {
                data: data,
                modifiedProperties: modifiedProperties,
            }
        }, (response) => {
            modifiedProperties = [];
            mdl.hide();
            msg.success({ message: response.Message });
            datatable.refresh();
        });
    });
});
