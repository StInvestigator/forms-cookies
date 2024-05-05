var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles();

app.MapGet("/", async context =>
{
    if (!context.Request.Cookies.ContainsKey("login"))
    {
        context.Response.Redirect("/login");
    }
    else
    {
        context.Response.ContentType = "text/html";
        await context.Response
        .WriteAsync($"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Hello!</title>\r\n    <link rel=\"stylesheet\" href=\"css\\\\style.css\">\r\n</head>\r\n<body>\r\n    <a href=\"/logout\">Logout</a>\r\n    <h1>Hello {context.Request.Cookies["login"]}!</h1>\r\n</body>\r\n</html>");
    }
});

app.MapGet("/login", async context =>
{
    string? message = context.Request.Cookies["message"];
    context.Response.Cookies.Delete("message");

    context.Response.ContentType = "text/html";

    await context.Response
    .WriteAsync("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Form</title>\r\n    <link rel=\"stylesheet\" href=\"css\\style.css\">\r\n</head>\r\n<body>\r\n    <form method=\"post\">\r\n        <h3>Registration</h3>\r\n        <div class=\"center\"><label>Login <input name=\"login\" /></label></div>\r\n        <div class=\"center\"><label>Password <input name=\"password\" /></label></div>\r\n" +
    $"<p>{message}</p>" +
    "<button type=\"submit\">Submit</button>\r\n    </form>\r\n</body>\r\n</html>");

});

app.MapPost("/login", async context =>
{

    string? login = context.Request.Form["login"];
    string? password = context.Request.Form["password"];

    if(login == "" || password == "")
    {
        if(login == "") { context.Response.Cookies.Append("message","Login can`t be empty!"); }
        if(password == "") { context.Response.Cookies.Append("message", "Password can`t be empty!"); }
        context.Response.Redirect("/login");
    }
    else
    {
        context.Response.Cookies.Append("login", login);
        context.Response.Cookies.Append("password", password);
        context.Response.Redirect("/");
    }
});

app.MapGet("/logout", async context =>
{
    context.Response.Cookies.Delete("login");
    context.Response.Cookies.Delete("password");

    context.Response.Redirect("/login");
});

app.Run();
