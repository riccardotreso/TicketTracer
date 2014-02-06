/// <reference path="../jquery-2.0.3.min.js" />
/// <reference path="../knockout-3.0.0.js" />


$(document).ready(function () {

    $("#divComments").dialog({
        modal: true,
        autoOpen: false,
        height: 500,
        width: 500,
        show: { effect: "blind", duration: 800 },
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });


    var VModel = new Model();
    VModel.LoadTicket();
    VModel.LoadUser();

    ko.applyBindings(VModel, document.getElementById("divMainContent"));
    ko.applyBindings(VModel, document.getElementById("divComments"));

});

Model = function () {
    var self = this;
    self.Tickets = ko.observableArray([]);
    self.currentTicket = ko.observable(new Ticket("", "", "", new Date(), "", "", new Date(), false, "", []));
    self.comment = ko.observable(new Comment(new Date(), "", "", ""));
    
    self.AvailableUser = ko.observableArray([]);
    self.LoadUser = function () {
        self.AvailableUser.removeAll();
        self.AvailableUser.push(new User("", "", "", "-1", "", "", "", ""));
        $.getJSON("/api/user?isHelpDesk=true", function (data) {
            $.each(data, function (index, value) {
                self.AvailableUser.push(new User(value.Id, value.Name, value.Surname, value.NTLogin, value.Email, value.Enabled, value.Division, value.IsAdmin));
            });
            
        });
    }


    self.LoadTicket = function () {
        self.Tickets.removeAll();
        $.getJSON("/api/ticket", function (data) {
            $.each(data, function (index, value) {
                self.Tickets.push(new Ticket(value.Id, value.Title, value.Description, value.DateCreation, value.SubmittedBy, value.SubmittedEmail, value.DateClosed, value.Closed, value.AssignedUser, value.Comments));
            });

            $("#gridTicket").kendoGrid({
                dataSource: {
                    data: self.Tickets()
                },
                height: 500,
                scrollable: true,
                selectable: "row",
                change: onSelectTicket,
                columns: [{
                    field: "Id()",
                    width: 166,
                    hidden: true,
                    title: "Identificativo"
                }, {
                    field: "Title()",
                    width: 155,
                    title: "Title"
                }, {
                    field: "Description()",
                    width: 168,
                    title: "Description"
                }, {
                    field: "DateCreation()",
                    width: 168,
                    title: "DateCreation",
                    template: "#= kendo.toString(kendo.parseDate(DateCreation(), 'yyyy-MM-dd'), 'dd/MM/yyyy') #"
                }, {
                    field: "SubmittedBy()",
                    width: 91,
                    title: "SubmittedBy"
                }, {
                    field: "Closed()",
                    width: 104,
                    title: "Closed"
                }, {
                    field: "Comments()",
                    width: 104,
                    hidden: true,
                    title: "Comments"
                }, {
                    field: "AssignedUser()",
                    width: 104,
                    hidden: true,
                    title: "AssignedUser"
                }, {
                    field: "DateClosed()",
                    width: 104,
                    hidden: true,
                    title: "DateClosed"
                }, {
                    field: "Closed()",
                    width: 104,
                    hidden: true,
                    title: "Closed"
                }]
            });





        });
    };


    function LoadComments(objTicket) {
        objTicket.Comments.removeAll();
        $.getJSON("/api/comment?idTicket=" + objTicket.Id(), function (data) {
            $.each(data, function (index, value) {
                objTicket.Comments.push(new Comment(value.DateCreation, value.Text, value.Title, value.InsertBy));
            });
        });

    }


    self.clearTicket = function () {
        self.currentTicket(new Ticket("", "", "", new Date(), "", "", new Date(), false, "", []));
    }

    self.addTicketComments = function () {

        $.ajax({
            url: "/api/comment?idTicket=" + self.currentTicket().Id(),
            type: 'post',
            data: ko.toJSON(self.comment()),
            contentType: 'application/json',
            success: function (result) {
                self.currentTicket().Comments.push(new Comment(result.DateCreation, result.Text, result.Title, result.InsertBy));
                self.comment(new Comment(new Date(), "", "", ""));
            }
        });
    }


    self.updateTicket = function () {
        $.ajax({
            url: "/api/ticket?id=" + self.currentTicket().Id(),
            type: 'put',
            data: ko.toJSON(self.currentTicket()),
            contentType: 'application/json',
            success: function (result) {
                var res = result;
                self.LoadTicket();
            }
        });
    }

    self.readTicket = function () {
        $.ajax({
            url: "/api/notify?id=" + self.currentTicket().Id(),
            type: 'put',
            contentType: 'application/json'
        });
    }

    self.deleteTicket = function () {
        $.ajax({
            url: "/api/ticket?id=" + self.currentTicket().Id(),
            type: 'delete',
            contentType: 'application/json',
            success: function (result) {
                var res = result;
                self.LoadTicket();
            }
        });
    }

    self.addTicket = function () {
        $.ajax({
            url: "/api/ticket/",
            type: 'post',
            data: ko.toJSON(self.currentTicket()),
            contentType: 'application/json',
            success: function (result) {
                var res = result;
                self.LoadTicket();
                self.clearTicket();
            }
        });
    }

    //private method
    function onSelectTicket(arg) {
        var entityGrid = $("#gridTicket").data("kendoGrid");
        if (entityGrid) {
            var selectedItem = entityGrid.dataItem(entityGrid.select());
            if (selectedItem) {

                self.currentTicket(new Ticket(selectedItem.Id(),
                    selectedItem.Title(),
                    selectedItem.Description(),
                    selectedItem.DateCreation(),
                    selectedItem.SubmittedBy(),
                    selectedItem.SubmittedEmail(),
                    selectedItem.DateClosed(),
                    selectedItem.Closed(),
                    selectedItem.AssignedUser(),
                    []));


                self.readTicket();
                LoadComments(self.currentTicket());

                $("#divComments").dialog("open");

            }
        }
    }

}



Ticket = function (id, title, description, creationDate, submittedBy, submittedEmail, dateClosed, closed, assignedUser, comments) {
    var self = this;

    self.Id = ko.observable(id);
    self.Title = ko.observable(title);
    self.Description = ko.observable(description);

    self.DateCreation = ko.observable(new Date(creationDate));
    self.SubmittedBy = ko.observable(submittedBy);
    self.SubmittedEmail = ko.observable(submittedEmail);

    self.DateClosed = ko.observable(new Date(dateClosed));
    self.Closed = ko.observable(closed);
    self.AssignedUser = ko.observable(assignedUser === "" ? "-1" : assignedUser);

    self.Comments = ko.observableArray(comments);

}


Comment = function (dateCreation, text, title, InsertBy) {
    var self = this;

    self.DateCreation = ko.observable(new Date(dateCreation));
    self.Text = ko.observable(text);
    self.Title = ko.observable(title);
    self.InsertBy = ko.observable(InsertBy);
}


User = function (Id, Name, Surname, NTLogin, Email, Enabled, Division, IsAdmin) {
    var self = this;

    self.Id = ko.observable(Id);
    self.Name = ko.observable(Name);
    self.Surname = ko.observable(Surname);
    self.NTLogin = ko.observable(NTLogin);
    self.Email = ko.observable(Email);
    self.Enabled = ko.observable(Enabled);
    self.Division = ko.observable(Division);
    self.IsAdmin = ko.observable(IsAdmin);

};




