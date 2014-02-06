/// <reference path="../jquery-2.0.3.min.js" />
/// <reference path="../knockout-3.0.0.js" />

$(document).ready(function () {

    var model = new Model();
    model.LoadUsers();

    ko.applyBindings(model, document.getElementById("divMainContent"));


});

Model = function () {
    var self = this;
    self.Users = ko.observableArray([]);
    self.currentUser = ko.observable(new User("", "", "", "", "", false, "", false));

    self.LoadUsers = function () {
        self.Users.removeAll();
        $.getJSON("/api/user?isHelpDesk=false", function (data) {
            $.each(data, function (index, value) {
                self.Users.push(new User(value.Id, value.Name, value.Surname, value.NTLogin, value.Email, value.Enabled, value.Division, value.IsAdmin, value.IsHelpDesk, value.Password));
            });

            $("#gridUtenti").kendoGrid({
                dataSource: {
                    data: self.Users()
                },
                height: 500,
                scrollable: true,
                selectable: "row",
                change: onSelectUtente,
                columns: [{
                    field: "Id()",
                    width: 166,
                    hidden: true,
                    title: "Identificativo"
                }, {
                    field: "Name()",
                    width: 155,
                    title: "Name"
                }, {
                    field: "Surname()",
                    width: 168,
                    title: "Surname"
                }, {
                    field: "Email()",
                    width: 168,
                    title: "Email"
                }, {
                    field: "Enabled()",
                    width: 91,
                    title: "Enabled"
                }, {
                    field: "Division()",
                    width: 104,
                    title: "Division"
                }, {
                    field: "NTLogin()",
                    width: 104,
                    hidden: true,
                    title: "NTLogin"
                }, {
                    field: "IsAdmin()",
                    width: 104,
                    hidden: true,
                    title: "IsAdmin"
                }, {
                    field: "IsHelpDesk()",
                    width: 104,
                    hidden: true,
                    title: "IsHelpDesk"
                }, {
                    field: "Password()",
                    width: 104,
                    hidden: true,
                    title: "Password"
                }]
            });





        });
    };

    self.resetPassword = function () {
        alert("metodo non implementato");
    }

    self.clearUser = function () {
        self.currentUser(new User("", "", "", "", "", false, "", false, false, ""));
    }

    self.addUser = function () {
        $.ajax({
            url: "/api/user/",
            type: 'post',
            data: ko.toJSON(self.currentUser()),
            contentType: 'application/json',
            success: function (result) {
                var res = result;
                self.LoadUsers();
                self.clearUser();
            }
        });
    }

    self.updateUser = function () {
        $.ajax({
            url: "/api/user?id=" + self.currentUser().Id(),
            type: 'put',
            data: ko.toJSON(self.currentUser()),
            contentType: 'application/json',
            success: function (result) {
                var res = result;
                self.LoadUsers();
            }
        });
    }

    self.deleteUser = function () {
        $.ajax({
            url: "/api/user?id=" + self.currentUser().Id(),
            type: 'delete',
            contentType: 'application/json',
            success: function (result) {
                var res = result;
                self.LoadUsers();
            }
        });
    }

    //private method
    function onSelectUtente(arg) {
        var entityGrid = $("#gridUtenti").data("kendoGrid");
        if (entityGrid) {
            var selectedItem = entityGrid.dataItem(entityGrid.select());
            if (selectedItem) {
                
                self.currentUser(new User(selectedItem.Id(),
                    selectedItem.Name(),
                    selectedItem.Surname(),
                    selectedItem.NTLogin(),
                    selectedItem.Email(),
                    selectedItem.Enabled(),
                    selectedItem.Division(),
                    selectedItem.IsAdmin(),
                    selectedItem.IsHelpDesk(),
                    selectedItem.Password()));
            }
        }
    }
};



User = function (Id, Name, Surname, NTLogin, Email, Enabled, Division, IsAdmin, IsHelpDesk, Password) {
    var self = this;

    self.Id = ko.observable(Id);
    self.Name = ko.observable(Name);
    self.Surname = ko.observable(Surname);
    self.NTLogin = ko.observable(NTLogin);
    self.Email = ko.observable(Email);
    self.Enabled = ko.observable(Enabled);
    self.Division = ko.observable(Division);
    self.IsAdmin = ko.observable(IsAdmin);
    self.IsHelpDesk = ko.observable(IsHelpDesk);
    self.Password = ko.observable(Password);

};






