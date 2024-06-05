aufw(async () => {
    let modifiedProperties = [];
    let datatable = new aufw_datatable();
    await datatable.load({
        $dt: $("#tblKullanici"),
        axiosRequest: {
            controller: "User",
            action: "GetAll"
        },
        dataTable: {
            columns: [
                { data: "Id", title: "#", width: "20" },
                { data: "Username", title: "Kullanıcı Adı" },
                { data: "Name", title: "Ad" },
                { data: "Surname", title: "Soyad" },
                {
                    data: "IsActive", title: "Aktif",
                    searchable: false,
                    width: "20",
                    render: DataTable.render.check_box(),
                },
                { data: "CreatedBy", title: "Oluşturan" },
                { data: "CreatedDate", title: "Kayıt Tarihi", render: DataTable.render.moment_tr() },
                { data: "ModifiedBy", title: "Güncelleyen" },
                { data: "ModifiedDate", title: "Güncelleme Tarihi", render: DataTable.render.moment_tr() },
            ]
        }
    });
    //------------------------------------------------------
    let mdl = new aufw_modal();
    let $btnSave = $('<button type="button" class="btn btn-outline-success">Kaydet</button>');
    mdl.$footer.prepend($btnSave);
    //------------------------------------------------------
    // let crud = new aufw_crud();
    $("#btnAdd").click(async () => {
        await mdl.show({
            title: "Yeni Kayıt",
            axiosRequest: {
                controller: "User",
                action: "CorE",
                data: { Data: 0 }
            }
        });
    });

    $("#btnEdit").click(async () => {
        let id = datatable.Id;
        if (id < 1) {
            let msg = new message();
            msg.warning({ message: "Lütfen Kayıt Seçiniz..!!" });
            return;
        }
        await mdl.show({
            title: "Güncelle",
            axiosRequest: {
                controller: "User",
                action: "CorE",
                data: { Data: id }
            }
        });
        $("form").on('change paste', 'input, select, textarea', (e) => {
            let property = e.target.name;//this.name.substring(this.name.lastIndexOf('.') + 1, this.name.length);
            if (modifiedProperties.indexOf(property) === -1)
                modifiedProperties.push(property);
        });
    });

    $("#btnDelete").click(async () => {
        let id = datatable.Id;
        let msg = new message();

        if (id < 1) {
            msg.warning({ message: "Lütfen Kayıt Seçiniz..!!" });
            return;
        }
        let swal = await Swal.fire({
            title: 'Kayıt No : ' + id,
            text: 'Silme işlemini yapmak istediğinize emin misiniz?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Evet',
            cancelButtonText: 'Hayır'
        });
        if (swal.isConfirmed) {
            let axs = new axios_request();
            await axs.post({
                controller: "User",
                action: "Delete",
                data: { Data: id }
            }, (response) => {
                msg.success({ message: response.Message });
                datatable.refresh();
            });
        }
    });

    $btnSave.click(async () => {
        let axs = new axios_request();
        await axs.post({
            controller: "User",
            action: "Save",
            data: {
                Data: form.toObject(document.querySelector("form")),
                modifiedProperties: modifiedProperties
            }
        }, (response) => {
            modifiedProperties = [];
            mdl.hide();
            if (response.IsSuccess) {
                let msg = new message();
                msg.success({ message: response.Message });
                datatable.refresh();
            }
        });
    });
});
