/// <reference path="..\breeze.debug.js" />

//var Unit = new breeze.core.Enum("Unit");
//Unit.g = Unit.addSymbol({ unitIndex: 0 });
//Unit.kg = Unit.addSymbol({ unitIndex: 1 });
//Unit.liter = Unit.addSymbol({ unitIndex: 2 });
//Unit.piece = Unit.addSymbol({ unitIndex: 3 });
//Unit.cm = Unit.addSymbol({ unitIndex: 4 });
//Unit.m = Unit.addSymbol({ unitIndex: 5});
//Unit.seal();

(function (root) {
    var breeze = root.breeze,
        ko = root.ko,
        logger = root.app.logger;

    // define Breeze namespace
    var entityModel = breeze.entityModel;

    // service name is route to the Web API controller
    var serviceName = 'api/ShoppingList';

    // manager is the service gateway and cache holder
    var manager = new entityModel.EntityManager(serviceName);

    // define the viewmodel
    var vm = {
        lists: ko.observableArray(),
        includeInactive: ko.observable(false),
        save: saveChanges,
        show: ko.observable(false)
    };

    // start fetching Lists
    getLists();

    // re-query when "includeInactive" checkbox changes
    vm.includeInactive.subscribe(getLists);

    // bind view to the viewmodel
    ko.applyBindings(vm);

    /* Private functions */

    // get Lists asynchronously
    // returning a promise to wait for     
    function getLists() {

        logger.info("querying ShoppingLists");

        var query = entityModel.EntityQuery.from("ShoppingLists")
                .expand("Items")
                .expand("Items.Unit");

        if (!vm.includeInactive()) {
            query = query.where("Active", "==", true);
        }

        return manager
            .executeQuery(query)
            .then(querySucceeded)
            .fail(queryFailed);

        // clear observable array and load the results 
        function querySucceeded(data) {
            logger.success("queried ShoppingLists");
            vm.lists.removeAll();
            var lists = data.results;
            lists.forEach(function (item) {
                vm.lists.push(ko.mapping.fromJS(item));
            });
            vm.show(true); // show the view
        }
    };

    function saveChanges() {
        return manager.saveChanges()
            .then(function () { logger.success("changes saved"); })
            .fail(saveFailed);
    }

    function queryFailed(error) {
        logger.error("Query failed: " + error.message);
    }

    function saveFailed(error) {
        logger.error("Save failed: " + error.message);
    }

}(window));