ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var $el = $(element);

        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datepickerOptions || {};
        $el.datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($el.datepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $el.datepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            $el = $(element),
            current = $el.datepicker("getDate");

        if (value - current !== 0) {
            $el.datepicker("setDate", new Date(value));
        }
    }
};


ko.bindingHandlers.dateString = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        var pattern = allBindings.datePattern || 'dd/mm/yyyy';
        $(element).text(valueUnwrapped.toFormattedString(pattern));
    }
}


Date.prototype.toFormattedString = function (f) {
    var nm = this.getMonthName();
    var nd = this.getDayName();
    f = f.replace(/yyyy/g, this.getFullYear());
    f = f.replace(/yy/g, String(this.getFullYear()).substr(2, 2));
    f = f.replace(/MMM/g, nm.substr(0, 3).toUpperCase());
    f = f.replace(/Mmm/g, nm.substr(0, 3));
    f = f.replace(/MM\*/g, nm.toUpperCase());
    f = f.replace(/Mm\*/g, nm);
    f = f.replace(/mm/g, String(this.getMonth() + 1).padLeft('0', 2));
    f = f.replace(/DDD/g, nd.substr(0, 3).toUpperCase());
    f = f.replace(/Ddd/g, nd.substr(0, 3));
    f = f.replace(/DD\*/g, nd.toUpperCase());
    f = f.replace(/Dd\*/g, nd);
    f = f.replace(/dd/g, String(this.getDate()).padLeft('0', 2));
    f = f.replace(/d\*/g, this.getDate());
    return f;
};

Date.prototype.getMonthName = function () {
    return this.toLocaleString().replace(/[^a-z]/gi, '');
};

//n.b. this is sooo not i18n safe :)
Date.prototype.getDayName = function () {
    switch (this.getDay()) {
        case 0: return 'Sunday';
        case 1: return 'Monday';
        case 2: return 'Tuesday';
        case 3: return 'Wednesday';
        case 4: return 'Thursday';
        case 5: return 'Friday';
        case 6: return 'Saturday';
    }
};

String.prototype.padLeft = function (value, size) {
    var x = this;
    while (x.length < size) { x = value + x; }
    return x;
};