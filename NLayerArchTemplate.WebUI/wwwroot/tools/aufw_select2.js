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
    #defOptions = undefined;
    #isBinded = false;
    #$parent = undefined
    constructor(options) {
        if (options) {
            this.#$parent = options.$parent;
        }
    }

    load = (options) => {
        this.#setOptions(options);
        this.#$select2 = $("#" + this.#defOptions.id);
        this.#defOptions.select2.dropdownParent = this.#$parent;
        this.#$select2.on("select2:select", this.#selected);
        this.#$select2.on("select2:unselecting", this.#unselected);
        this.#$select2.options = this.#defOptions;
        if (this.#defOptions.dataSource) {
            this.#setDataSource(this.#defOptions.dataSource);
        }
        else if (this.#defOptions.axios) {
            this.#setDataSourceFromAxios(this.#defOptions.axios);
        }
        this.#$select2.select2(this.#defOptions.select2).val(this.#defOptions.selectedValue).trigger("change");
    }

    #selected = (e) => {
        if (this.#isBinded) {
            let childSelect2Options = this.#$select2.child.options;
            if (childSelect2Options.dataSource) {
                this.#setDataSource(childSelect2Options.dataSource);
            }
            else if (childSelect2Options.axios) {
                let selectedValue = e.params.data.id;
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

    #setDataSource(data) {
        let select = this.#isBinded ? this.#$select2.child : this.#$select2;
        select.empty();
        if (this.#defOptions.isGroupedItems) {
            for (let i = 0; i < data.length; i++) {
                let group = data[i];
                let optgroup = $('<optgroup>').attr('label', group.GroupName).attr('data-group-id', group.GroupId);
                this.#createItems(optgroup, group.Items)
                select.append(optgroup);
            }
        }
        else {
            this.#createItems(select, data);
        }
        select.val(this.#defOptions.selectedValue).trigger("change");
    }

    #setDataSourceFromAxios(axios) {
        let axs = new aufw_http_request();
        axs.post(axios, (response) => {
            if (response.IsSuccess) {
                this.#setDataSource(response.Data);
            }
            else {
                let msg = new aufw_message();
                msg.error({ message: response.Message });
            }
        });
    }

    #setOptions(options) {
        this.#defOptions = {
            id: "",
            selectedValue: null,
            dataSource: undefined,
            axios: undefined,
            isGroupedItems: false,
            select2: {
                placeholder: "Seçiniz..",
                theme: "bootstrap4",
                allowClear: true,
                language: this.#tr,
                escapeMarkup: function (markup) {
                    return markup;
                }
            }
        };
        this.#isBinded = false;
        $.extend(true, this.#defOptions, options);
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
        return this.#$select2.val();
    }

    #createItems(parent, items) {
        for (let i = 0; i < items.length; i++) {
            let option = $('<option>').val(items[i].Id).text(items[i].Name);
            parent.append(option);
        }
    }
}