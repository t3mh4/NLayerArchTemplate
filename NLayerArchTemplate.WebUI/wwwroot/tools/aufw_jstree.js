'use strict';

class aufw_jstree {

    #$div = undefined;

    #getDefOptions() {
        return {
            $div: undefined,
            data: undefined,
            selectedValues: undefined,
            $search: undefined,
            jstree: {
                plugins: ["checkbox", "search", "sort", "wholerow"],
                core: {
                    data: undefined,
                    themes: {
                        variant: "large",
                        icons: false
                    },
                },
                search: {
                    show_only_matches: true,
                    show_only_matches_children: true
                },
                checkbox: {
                    keep_selected_style: false
                },
                'sort': function (a, b) {
                    return this.get_text(a).localeCompare(this.get_text(b), 'tr', { sensitivity: 'base' });
                }
            }
        };
    }

    load = (opitons) => {
        let defOptions = this.#getDefOptions();
        $.extend(true, defOptions, opitons);
        let x = this.#convertData(defOptions.data);
        defOptions.jstree.core.data = this.#convertData(defOptions.data);
        this.#$div = defOptions.$div;
        this.#$div.jstree(defOptions.jstree).on('loaded.jstree', () => {
            this.#$div.jstree(true).select_node(defOptions.selectedValues);
        });

        if (defOptions.$search) {
            var to = false;
            defOptions.$search.keyup(() => {
                if (to) { clearTimeout(to); }
                to = setTimeout(() => {
                    var v = defOptions.$search.val();
                    this.#$div.jstree(true).search(v.toLocaleUpperCase('tr-TR'))
                }, 250);
            });
        }
    }

    activate_node = (func) => {
        this.#$div.bind('activate_node.jstree', func);
    }

    #convertData = (data) => {
        return data.map(function (group) {

            return {
                text: group.GroupName,
                children: group.Items.map(function (item) {
                    return {
                        id: item.Id,
                        text: item.Name,
                    };
                })
            };
        });
    }
}