(function (root) {
    var app = root.app;

    app.models = {};

    app.models.ShoppingList = ShoppingList;
    app.models.Item = Item;

    function ShoppingList(data) {
        var self = data;
        data.select = app.vm.active;
        data.active = ko.computed(function () {
            var active = app.vm.active();
            return active && active.Id() === data.Id();
        }).extend({ throttle: 10 });
        data.remove = app.vm.remove;
        data.selectedItem = ko.observable();
        data.addItem = addItem;

        function addItem(list) {
            var newItem = app.vm.createEntity("Item");
            newItem.ShoppingList(list);
            list.Items.push(newItem);
            list.selectedItem(newItem);
        }


        return self;
    }

    function Item(data) {
        var self = data;

        data.remove = app.vm.remove;
        data.more = more;
        data.less = less;
        data.buy = buy;
        data.toggleBought = toggleBought;
        data.toggleEditName = toggleEditName;

        data.AmountFormatted = ko.computed(function () {
            return data.Amount().toFixed(2);
        });
        data.select = function (item) {
            data.ShoppingList().selectedItem(data);
            $("input[name=Amount]").focus();
        };
        data.deselect = function (form) {
            var $form = $(form) || $("form#selectedItemForm");
            if ($(form).is("form")) {
                if (!$(form).valid()) {
                    return;
                }
            }
            data.ShoppingList().selectedItem(null);
        };

        function more(item) {
            item.Amount(item.Amount() + 1);
        }

        function less(item) {
            item.Amount((item.Amount() - 1).toFixed(2));
            if (item.Amount() < 0) {
                item.Amount(0);
            }
        }

        function buy(item) {
            item.Bought(true);
        }

        function toggleBought(item) {
            item.Bought(!item.Bought());
        }

        data.editingName = ko.observable(false);

        function toggleEditName(item) {
            item.editingName(!item.editingName());
        }

        data.Name.extend({ required: "Name is required" });
        data.Amount.extend({ min: 0 });
        data.Amount.subscribe(function (value) {
            if (value < 0) {
                this.target(0);
            }
        });

        return self;
    }
}(window));