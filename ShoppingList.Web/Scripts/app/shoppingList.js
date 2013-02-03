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
        logger = root.app.logger,
        models = root.app.models;


    // define Breeze namespace
    var entityModel = breeze.entityModel;

    // service name is route to the Web API controller
    var serviceName = 'api/ShoppingList';

    // manager is the service gateway and cache holder
    var manager = new entityModel.EntityManager(serviceName);

    manager.metadataStore.registerEntityTypeCtor("ShoppingList", function () { }, models.ShoppingList);
    manager.metadataStore.registerEntityTypeCtor("Item", function () { }, models.Item);

    // define the viewmodel
    var vm = {
        lists: ko.observableArray(),
        includeInactive: ko.observable(false),
        active: ko.observable(),
        editingActive: ko.observable(false),
        showBought: ko.observable(false),
        editMode: ko.observable(window.innerWidth >= 800 && window.innerHeight >= 600),
        remove: removeItem,
        hasChanges: ko.observable(false),
        save: saveChanges,
        revert: revertChanges,
        show: ko.observable(false),
        createEntity: createEntity,
        initialized: ko.observable(false),
        createList: createList,
        editActive: editActive,
        stopEditingActive: stopEditingActive
    };
    vm.shoppingMode = ko.computed(function () {
        return !vm.editMode();
    });
    vm.toggleShowBought = function () {
        vm.showBought(!vm.showBought());
    };
    root.app.vm = vm;

    manager.hasChanges.subscribe(observeChanges);
    //vm.active.subscribe(observeChanges);
    function observeChanges() {
        //vm.hasChanges(vm.active() && vm.active().entityAspect.entityState.isAddedModifiedOrDeleted());
        vm.hasChanges(manager.hasChanges());
    }

    //setInterval(function() { vm.hasChanges(manager.hasChanges()); }, 100);

    // start fetching Lists
    loadData();

    // re-query when "includeInactive" checkbox changes
    vm.includeInactive.subscribe(loadData);

    // bind view to the viewmodel
    ko.applyBindings(vm);

    /* Private functions */

    // get Lists asynchronously
    // returning a promise to wait for     
    function loadData() {

        logger.info("querying ShoppingLists");

        var query = entityModel.EntityQuery.from("ShoppingLists")
            .expand("Items");
        //.expand("Items.Unit");

        if (!vm.includeInactive()) {
            query = query.where("Active", "==", true);
        }

        return manager
            .executeQuery(query)
            .then(querySucceeded)
            .fail(queryFailed);

        // clear observable array and load the results 
        function querySucceeded(data) {
            vm.lists.removeAll();
            vm.lists(data.results);
            //lists.forEach(function (item) {
            //    vm.lists.push(item);
            //});
            if (vm.lists().length) {
                vm.show(true); // show the view
                vm.active(vm.lists()[0]);
                logger.success("Loaded {count} ShoppingLists".assign({ count: data.results.length }));
            } else {
                logger.success("You have no ShoppingLists. You can create one by clicking on 'NEW'");
            }
            vm.initialized(true);
        }
    };

    function removeItem(item) {
        item.entityAspect.setDeleted();
    }

    function createEntity(name, addToManager) {
        var entity = manager.metadataStore.getEntityType(name).createEntity();
        if (addToManager !== false) {
            manager.addEntity(entity);
        }
        return entity;
    }

    function createList(vm) {
        var newList = createEntity("ShoppingList");
        newList.Active(true);
        vm.lists.push(newList);
        vm.active(newList);
        vm.editingActive(true);
    }

    function saveChanges() {
        return manager.saveChanges()
            .then(function () { logger.success("changes saved"); })
            .fail(saveFailed);
    }

    function revertChanges() {
        if (manager.hasChanges()) {
            manager.rejectChanges();
        }
    }

    function editActive() {
        vm.editingActive(true);
    }
    
    function stopEditingActive() {
        vm.editingActive(false);
    }

    function queryFailed(error) {
        logger.error("Query failed: " + error.message);
    }

    function saveFailed(error) {
        logger.error("Save failed: " + error.message);
    }

}(window));