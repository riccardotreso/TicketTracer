<%@ Page Title="" Language="C#" MasterPageFile="~/Views/TicketTracker.Master" Inherits="System.Web.Mvc.ViewPage<TicketTracer.Models.RegisterModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1>Register.</h1>
        <h2>Create a new account.</h2>
    </hgroup>

    <%using (Html.BeginForm())
      { %>
    <%: Html.AntiForgeryToken() %>
    <%: Html.ValidationSummary() %>

    <fieldset>
        <legend>Registration Form</legend>
        <ol>
            <li>
                <%: Html.LabelFor(m => m.Name) %>
                <%: Html.TextBoxFor(m => m.Name) %>
            </li>
            <li>
                <%: Html.LabelFor(m => m.Surname) %>
                <%: Html.TextBoxFor(m => m.Surname) %>
            </li>
            <li>
                <%: Html.LabelFor(m => m.Division) %>
                <%: Html.TextBoxFor(m => m.Division) %>
            </li>
            <li>
                <%: Html.LabelFor(m => m.Email) %>
                <%: Html.TextBoxFor(m => m.Email) %>
            </li>
            <li>
                <%: Html.LabelFor(m => m.UserName) %>
                <%: Html.TextBoxFor(m => m.UserName) %>
            </li>
            <li>
                <%: Html.LabelFor(m => m.Password) %>
                <%: Html.PasswordFor(m => m.Password) %>
            </li>
            <li>
                <%: Html.LabelFor(m => m.ConfirmPassword) %>
                <%: Html.PasswordFor(m => m.ConfirmPassword) %>
            </li>
        </ol>
        <input type="submit" value="Register" />
    </fieldset>

    <%} %>
</asp:Content>
