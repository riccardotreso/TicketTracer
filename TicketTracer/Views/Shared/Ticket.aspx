<%@ Page Title="" Language="C#" MasterPageFile="~/Views/TicketTracker.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/Custom/Ticket.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Ticket</h2>

    <table>
        <tr>
            <td><span>Title</span></td>
            <td>
                <input type="text" data-bind="value: currentTicket().Title" /></td>
        </tr>
        <tr>
            <td><span>Description</span></td>
            <td>
                <textarea style="width: 300px" data-bind="value: currentTicket().Description"></textarea></td>
        </tr>
    </table>
    <% if (TicketTracer.Models.TTRepository.GetUserAdmin(HttpContext.Current.User.Identity.Name))
       { %>
    <table>
        <tr>
            <td><span>HD Assigned</span></td>
            <td>
                <select data-bind="options: AvailableUser, optionsText: function(item) {
                       return item.Name() + ' ' + item.Surname()
                   }, optionsValue: 'NTLogin', value: currentTicket().AssignedUser">
                </select></td>
        </tr>
    </table>
    <% }
       else if (TicketTracer.Models.TTRepository.GetHelpDeskUser(HttpContext.Current.User.Identity.Name))
       {%>
    <table>
        <tr>
            <td style="width:80px"><span>Closed</span></td>
            <td>
                <input type="checkbox" data-bind="checked: currentTicket().Closed" /></td>
        </tr>
        <tr data-bind="visible: currentTicket().Closed()">
            <td><span>Date Closed</span></td>
            <td>
                <input type="text" data-bind="enabled: currentTicket().Closed(), datepicker: currentTicket().DateClosed, datepickerOptions: { dateFormat: 'dd/mm/yy' }" /></td>
        </tr>
    </table>
    <%} %>
    <input type="submit" value="Insert" data-bind="click: addTicket, visible: currentTicket().Id() == ''" />
    <input type="submit" value="Update" data-bind="click: updateTicket, visible: currentTicket().Id() != ''" />
    <input type="submit" value="Pulisci" data-bind="click: clearTicket, visible: currentTicket().Id() != ''" />
    <% if (TicketTracer.Models.TTRepository.GetUserAdmin(HttpContext.Current.User.Identity.Name))
       { %>
    <input type="submit" value="Delete" data-bind="click: deleteTicket, visible: currentTicket().Id() != ''" />
    <%} %>
    <div id="gridTicket"></div>


    <div id="divComments">
        <table>
            <tr>
                <td><span>Title</span></td>
                <td>
                    <input type="text" data-bind="value: comment().Title" /></td>
            </tr>
            <tr>
                <td><span>Text</span></td>
                <td>
                    <textarea style="width: 300px" data-bind="value: comment().Text"></textarea></td>
            </tr>
        </table>
        <input type="submit" value="Insert" data-bind="click: addTicketComments" />
        <div data-bind="foreach: currentTicket().Comments">
            <div style="width: 100%; border: 1px solid silver; color: #444">
                <div style="margin: 10px">
                    <span style="font-weight: bold" data-bind="text: InsertBy"></span> ha scritto il <span data-bind="dateString: DateCreation"></span>
                    <br />
                    <span data-bind="text: Title"></span>
                    <br />
                    <span style="font-style: italic" data-bind="text: Text"></span>
                </div>
            </div>
        </div>
    </div>

</asp:Content>


