

using asp03.mylib;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapGet("/",async context =>
    {
       
        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems() ,context.Request );
        var html = HtmlHelper.HtmlDocument("xin chao",menu + HtmlHelper.HtmlTrangchu());
        await context.Response.WriteAsync(html);
    });
    endpoints.MapGet("/RequestInfo", async context =>
    {

        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);
        var info = RequestProcess.RequestInfo(context.Request);
        var html = HtmlHelper.HtmlDocument("Thong tin cua Request", menu + info.HtmlTag("div", "container"));
        await context.Response.WriteAsync(html);
    });
    endpoints.MapGet("/Json", async context =>
    {
        var p = new
        {
            TenSanPham = "Dien Thoai",
            Gia = 500000,
            NamSanXuat = 2020
        };
        context.Response.ContentType = "application/json";
        var json = JsonConvert.SerializeObject(p);

        await context.Response.WriteAsync(json);
    });
    endpoints.MapGet("/Encoding", async context =>
    {

        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);
        var html = HtmlHelper.HtmlDocument("Encoding", menu);
        await context.Response.WriteAsync(html);
    });
    

    endpoints.MapGet("/Cookies/{*action}", async context =>
    {

        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);
        var action = context.GetRouteValue("action")??"read";
        string message = "";
        if(action.ToString() == "write")
        {
            var option = new CookieOptions()
            {
                Path = "/",
                Expires = DateTime.Now.AddDays(1)
            };
            //cookie: ten - gia tri 
            context.Response.Cookies.Append("matkhau","son1821",option) ;
            message = "Cookie duoc ghi".HtmlTag("div","alert alert-danger");
        }
        else
        {
            var listcokie = context.Request.Cookies.Select((header) => $"{header.Key}: {header.Value}".HtmlTag("li"));
            message = string.Join("", listcokie).HtmlTag("ul");
           
        }

        var huongdan = "<a class= \"btn btn-danger\" href = \"/Cookies/read\">Doc Cookie</a><a class= \"btn btn-success\"  href = \"/Cookies/write\">Ghi Cookie</a>".HtmlTag("div", "container mt-4");
        var html = HtmlHelper.HtmlDocument("Cookie: "+ action, menu+huongdan+message);
        await context.Response.WriteAsync(html);
    });
    endpoints.MapGet("/Form", async context =>
    {

        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);
        var formhtml = RequestProcess.ProcessForm(context.Request);
        var html = HtmlHelper.HtmlDocument("Test submit form html", menu+ formhtml);
        await context.Response.WriteAsync(html);
    });
    endpoints.MapPost("/Form", async context =>
    {

        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);
        var formhtml = RequestProcess.ProcessForm(context.Request);
        var html = HtmlHelper.HtmlDocument("Test submit form html", menu + formhtml);
        await context.Response.WriteAsync(html);
    });



} );



/// RequestInfo    Đọc và hiện thị các thông tin về Request truy cập
///Encoding	Demo tính năng encoding dữ liệu khi xuất HTML
///Cookies	Demo - Đọc và ghi cookie
///Json	Demo trả về dữ liệu JSON
///Form
app.Run();
