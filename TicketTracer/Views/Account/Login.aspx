﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/TicketTracker.Master" Inherits="System.Web.Mvc.ViewPage<TicketTracer.Models.LoginModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Login</h2>
    <section id="loginForm">
        <h2>Use a local account to log in.</h2>
        <% using (Html.BeginForm(new { ReturnUrl = ViewBag.ReturnUrl }))
           { %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Log in Form</legend>
            <ol>
                <li>
                    <%: Html.LabelFor(m => m.UserName) %>
                    <%: Html.TextBoxFor(m => m.UserName) %>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </li>
                <li>
                    <%: Html.LabelFor(m => m.Password) %>
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </li>
                <li>
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe, new { @class = "checkbox" }) %>
                </li>
            </ol>
            <input type="submit" value="Log in" />
        </fieldset>
        <p>
            <%: Html.ActionLink("Registrati","Register") %>
        </p>
        <% } %>
    </section>
</asp:Content>


