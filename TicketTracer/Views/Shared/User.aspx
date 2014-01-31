<%@ Page Title="" Language="C#" MasterPageFile="~/Views/TicketTracker.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/Custom/User.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Gestione Utenti e Help desk</h2>
    <table>
        <tr>
            <td><span>Name</span></td>
            <td>
                <input type="text" data-bind="value: currentUser().Name" /></td>
        </tr>
        <tr>
            <td><span>Surname</span></td>
            <td>
                <input type="text" data-bind="value: currentUser().Surname" /></td>
        </tr>
        <tr>
            <td><span>UserName</span></td>
            <td>
                <input type="text" data-bind="value: currentUser().NTLogin" /></td>
        </tr>
        <tr>
            <td><span>Password</span></td>
            <td>
                <input type="text" data-bind="visible: currentUser().Id() == '', value: currentUser().Password" />
                <input type="submit" value="reset" data-bind="click: resetPassword, visible: currentUser().Id() != ''" />
            </td>
        </tr>
        <tr>
            <td><span>Email</span></td>
            <td>
                <input type="text" data-bind="value: currentUser().Email" /></td>
        </tr>
        <tr>
            <td><span>Enabled</span></td>
            <td>
                <input type="checkbox" data-bind="checked: currentUser().Enabled" /></td>
        </tr>
        <tr>
            <td><span>Division</span></td>
            <td>
                <input type="text" data-bind="value: currentUser().Division" /></td>
        </tr>
        <tr>
            <td><span>Admin</span></td>
            <td>
                <input type="checkbox" data-bind="checked: currentUser().IsAdmin" /></td>
        </tr>
        <tr>
            <td><span>Help Desk</span></td>
            <td>
                <input type="checkbox" data-bind="checked: currentUser().IsHelpDesk" /></td>
        </tr>
    </table>
    <input type="submit" value="Insert" data-bind="click: addUser, visible: currentUser().Id() == ''" />
    <input type="submit" value="Update" data-bind="click: updateUser, visible: currentUser().Id() != ''" />
    <input type="submit" value="Delete" data-bind="click: deleteUser, visible: currentUser().Id() != '' && !currentUser().IsAdmin()" />
    <input type="submit" value="Pulisci" data-bind="click: clearUser, visible: currentUser().Id() != ''" />
    <div id="gridUtenti"></div>




</asp:Content>


