/// <reference path="../jquery-2.0.3.min.js" />
/// <reference path="../knockout-3.0.0.js" />


$(document).ready(function () {

    var mModel = new MasterModel();
    mModel.LoadNotify();

   
    ko.applyBindings(mModel, document.getElementById("divContentMenu"));

});

MasterModel = function () {
    var self = this;

    self.Notify = ko.observable(new NotifyModel(0, 0));

    self.LoadNotify = function () {
        $.getJSON("/api/notify/", function (data) {
            if (data)
                self.Notify(new NotifyModel(data.NumberTickets, data.NumberComments));
        });
    };
};

NotifyModel = function (NumberTickets, NumberComments){
    var self = this;
    self.NumberTickets = ko.observable(NumberTickets);
    self.NumberComments = ko.observable(NumberComments);

    self.HasNotify = ko.computed(function () {
        return self.NumberTickets() > 0 || self.NumberComments() > 0;
    });

    self.NumNotify = ko.computed(function () {
        return self.NumberTickets() + self.NumberComments();
    });

}