'use strict';

var tr = {
    "emptyTable": "Tabloda herhangi bir veri mevcut değil",
    "info": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
    "infoEmpty": "Kayıt yok",
    "infoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
    "infoThousands": ".",
    "lengthMenu": "Sayfada _MENU_ kayıt göster",
    "loadingRecords": "Yükleniyor...",
    "processing": "İşleniyor...",
    "search": "",
    "zeroRecords": "Eşleşen kayıt bulunamadı",
    "paginate": {
        "first": "İlk",
        "last": "Son",
        "next": "Sonraki",
        "previous": "Önceki"
    },
    "aria": {
        "sortAscending": ": artan sütun sıralamasını aktifleştir",
        "sortDescending": ": azalan sütun sıralamasını aktifleştir"
    },
    "select": {
        "rows": {
            "_": "",
            "1": "",
            "0": ""
        }
    },
    "autoFill": {
        "cancel": "İptal",
        "fill": "Bütün hücreleri <i>%d<i> ile doldur<\/i><\/i>",
        "fillHorizontal": "Hücreleri yatay olarak doldur",
        "fillVertical": "Hücreleri dikey olarak doldur",
        "info": ""
    },
    "buttons": {
        "collection": "Koleksiyon <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
        "colvis": "Sütun görünürlüğü",
        "colvisRestore": "Görünürlüğü eski haline getir",
        "copy": "Koyala",
        "copyKeys": "Tablodaki sisteminize kopyalamak için CTRL veya u2318 + C tuşlarına basınız.",
        "copySuccess": {
            "1": "1 satır panoya kopyalandı",
            "_": "%ds satır panoya kopyalandı"
        },
        "copyTitle": "Panoya kopyala",
        "csv": "CSV",
        "excel": "Excel",
        "pageLength": {
            "-1": "Bütün satırları göster",
            "1": "-",
            "_": "%d satır göster"
        },
        "pdf": "PDF",
        "print": "Yazdır"
    },
    "decimal": "-",
    "searchBuilder": {
        "add": "Koşul Ekle",
        "button": {
            "0": "Arama Oluşturucu",
            "_": "Arama Oluşturucu (%d)"
        },
        "clearAll": "Hepsini Kaldır",
        "condition": "Koşul",
        "conditions": {
            "date": {
                "after": "Sonra",
                "before": "Önce",
                "between": "Arasında",
                "empty": "Boş",
                "equals": "Eşittir",
                "not": "Değildir",
                "notBetween": "Dışında",
                "notEmpty": "Dolu"
            },
            "moment": {
                "after": "Sonra",
                "before": "Önce",
                "between": "Arasında",
                "empty": "Boş",
                "equals": "Eşittir",
                "not": "Değildir",
                "notBetween": "Dışında",
                "notEmpty": "Dolu"
            },
            "number": {
                "between": "Arasında",
                "empty": "Boş",
                "equals": "Eşittir",
                "gt": "Büyüktür",
                "gte": "Büyük eşittir",
                "lt": "Küçüktür",
                "lte": "Küçük eşittir",
                "not": "Değildir",
                "notBetween": "Dışında",
                "notEmpty": "Dolu"
            },
            "string": {
                "contains": "İçerir",
                "empty": "Boş",
                "endsWith": "İle biter",
                "equals": "Eşittir",
                "not": "Değildir",
                "notEmpty": "Dolu",
                "startsWith": "İle başlar"
            }
        },
        "data": "Veri",
        "deleteTitle": "Filtreleme kuralını silin",
        "leftTitle": "Kriteri dışarı çıkart",
        "logicAnd": "ve",
        "logicOr": "veya",
        "rightTitle": "Kriteri içeri al",
        "title": {
            "0": "Arama Oluşturucu",
            "_": "Arama Oluşturucu (%d)"
        },
        "value": "Değer"
    },
    "searchPanes": {
        "clearMessage": "Hepsini Temizle",
        "collapse": {
            "0": "Arama Bölmesi",
            "_": "Arama Bölmesi (%d)"
        },
        "count": "{total}",
        "countFiltered": "{shown}\/{total}",
        "emptyPanes": "Arama Bölmesi yok",
        "loadMessage": "Arama Bölmeleri yükleniyor ...",
        "title": "Etkin filtreler - %d"
    },
    "searchPlaceholder": "Ara",
    "thousands": "."
}

class aufw_datatable {

    #dt = undefined;

    #startTime = undefined;

    #defOptions = {
        $dt: undefined,
        Pk: "Id",
        detailText: "Detaylar",
        axiosRequest: {
            controller: undefined,
            action: undefined,
            data: undefined
        },
        dataTable: {
            language: tr,
            select: {
                style: 'single'
            },
            deferRender: true,
            //scrollX: true,
            responsive: {
                //details: {
                //    display: $.fn.DataTable.Responsive.display.modal({
                //        header: function (e) {
                //            return "Detaylar";
                //        }
                //    }),
                //    renderer: DataTable.Responsive.renderer.listHidden()
                //}
                details: {
                    display: $.fn.DataTable.Responsive.display.modal({
                        header: function (e) {
                            return "Detaylar";
                        }
                    }),
                    renderer: function(api, rowIdx, columns) {
                        var rows = "<ul>";
                        columns.forEach((col) => {
                            if (!col.hidden) return;
                            rows += '<li>' +
                                '<span style="font-weight:bold">' + col.title + ' : </span>' +
                                '<span>' + col.data + '</span>' +
                                '</li>';
                        });
                        rows += "<ul/>";
                        return rows;
                    }
                }
            },
            initComplete: (settings, json) => {
                let endTime = new Date().getTime();
                let diff = endTime - this.#startTime;
                console.cInfo('Table initialization completed in : ' + diff + ' ms');
            },
        }
    };

    load = async (options) => {

        this.#startTime = new Date().getTime();

        $.extend(true, this.#defOptions, options);

        let axs = new axios_request();
        await axs.post_async(this.#defOptions.axiosRequest, response => {
            this.#defOptions.dataTable.data = response.Data;
            this.#dt = this.#defOptions.$dt.DataTable(this.#defOptions.dataTable).columns.adjust();
        });
    }

    refresh = async () => {
        let axs = new axios_request();

        await axs.post_async(this.#defOptions.axiosRequest, response => {
            this.#dt.clear().rows.add(response.Data).draw();
        });
    }
    get id() {
        let data = this.#dt.row({ selected: true }).data();
        if (data === undefined) return 0;
        return data[this.#defOptions.Pk];
    }
}

