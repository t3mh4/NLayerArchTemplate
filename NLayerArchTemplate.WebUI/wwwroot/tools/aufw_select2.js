'use strict';

class aufw_select2 {

    #tr = {
        errorLoading: function () {
            return 'Sonuç yüklenemedi';
        },
        inputTooLong: function (args) {
            var overChars = args.input.length - args.maximum;

            var message = overChars + ' karakter daha girmelisiniz';

            return message;
        },
        inputTooShort: function (args) {
            var remainingChars = args.minimum - args.input.length;

            var message = 'En az ' + remainingChars + ' karakter daha girmelisiniz';

            return message;
        },
        loadingMore: function () {
            return 'Daha fazla…';
        },
        maximumSelected: function (args) {
            var message = 'Sadece ' + args.maximum + ' seçim yapabilirsiniz';

            return message;
        },
        noResults: function () {
            return 'Sonuç bulunamadı';
        },
        searching: function () {
            return 'Aranıyor…';
        },
        removeAllItems: function () {
            return 'Tüm öğeleri kaldır';
        }
    };
    #$select2;
    #defOptions = {
        id: "",
        selectedValue: null,
        dataSource: undefined,
        axios: undefined,
        select2: {
            dropdownParent: $('#coreModal'),
            placeholder: "Seçiniz..",
            theme: "bootstrap4",
            allowClear: true,
            language: this.#tr,
            escapeMarkup: function (markup) {
                return markup;
            }
        }
    };
    #isBinded = false;
    
    load = (options) => {
        this.#clearOptions();
        $.extend(true, this.#defOptions, options);
        this.#$select2 = $("#" + this.#defOptions.id);
        this.#$select2.select2(this.#defOptions.select2).val(this.#defOptions.selectedValue).trigger("change");
        this.#$select2.on("select2:select", this.#selected);
        this.#$select2.on("select2:unselecting", this.#unselected);
        this.#$select2.options = this.#defOptions;
        if (this.#defOptions.dataSource) {
            this.#dataSource = this.#defOptions.dataSource;
        }
        else if (this.#defOptions.axios) {
            this.#setDataSourceFromAxios(this.#defOptions.axios);
        }
    }

    #selected = (e) => {
        let selectedValue = e.params.data.id;
        if (selectedValue == null) { alert(""); }
        if (this.#isBinded) {
            let childSelect2Options = this.#$select2.child.options;
            if (childSelect2Options.dataSource) {
                this.#dataSource = childSelect2Options.dataSource;
            }
            else if (childSelect2Options.axios) {
                childSelect2Options.axios.data = { Data: selectedValue };
                this.#setDataSourceFromAxios(childSelect2Options.axios);
            }
        }
        this.selected_event(e);
    }

    selected_event = (e) => undefined;

    #unselected = (e) => {
        if (this.#isBinded) {
            this.#$select2.child.empty();
        }
    }

    set #dataSource(data) {
        let select = this.#isBinded ? this.#$select2.child : this.#$select2;
        select.empty();
        for (var i = 0; i < data.length; i++) {
            select.append('<option value="' + data[i].Id + '">' + data[i].Name + '</option>');
        }
        select.val(this.#defOptions.selectedValue).trigger("change");
    }

    #setDataSourceFromAxios(axios) {
        let axs = new axios_request();
        axs.post(axios, (response) => {
            if (response.IsSuccess) {
                this.#dataSource = response.Data;
            }
            else {
                let msg = new message();
                msg.error({ message: response.Message });
            }
        });
    }

    #clearOptions() {
        let defOptions = {
            id: "",
            selectedValue: null,
            dataSource: undefined,
            axios: undefined,
            select2: {
                dropdownParent: $('#coreModal'),
                placeholder: "Seçiniz..",
                theme: "bootstrap4",
                allowClear: true,
                language: this.#tr,
                escapeMarkup: function (markup) {
                    return markup;
                }
            }
        };
        this.#defOptions = defOptions;
        this.#isBinded = false;
    }

    bind = (options) => {
        this.#isBinded = true;
        this.#$select2.child = options.child;
        this.#$select2.child.options.axios = options.axios;
        this.#$select2.child.options.dataSource = options.dataSource;
    }

    get select () {
        return this.#$select2;
    }

    set value(value) {
        this.#$select2.val(value).trigger("change");
    }
    get value() {
        return this.#$select2.val(value);
    }
}