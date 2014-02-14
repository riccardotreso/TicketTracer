<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if (Request.IsAuthenticated)
   { %>



<ul class="menu">
    <li>
        <div class="widget center icon arrow"></div>
        <span class="label welcome"><%: HttpContext.Current.User.Identity.Name %></span>
        <ul>
            <li>
                <div class="widget center icon setting"></div>
                <span class="label">Settings</span>
            </li>
            <li>
                <% using (Html.BeginForm("LogOff", "Account", FormMethod.Post, new { id = "logoutForm" }))
                   { %>
                <%: Html.AntiForgeryToken() %>
                <a href="javascript:document.getElementById('logoutForm').submit()"><span class="label">Log Off</span></a>
                <% } %>
            </li>
        </ul>
    </li>
    
</ul>
<% }
   else
   { %>
<ul class="menu">

    <li>
        <a href='<%: Url.Action("Register", "Account") %>'>
            <div class="widget center icon star"></div>
            <span class="label">Register</span>
        </a>
    </li>

    <li>
        <a href='<%: Url.Action("Login", "Account") %>'>
            <div class="widget center icon user"></div>
            <span class="label">Login</span>
        </a>
    </li>


</ul>
<% } %>
