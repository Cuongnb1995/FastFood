using Microsoft.AspNetCore.Mvc.Filters;

namespace FastFood.Areas.Admin.Atrributes
{
    public class CheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //set gia tri session (de qua buoc dang nhap)
            context.HttpContext.Session.SetString("email", "nva@gmail.com");
            //lay gia tri cua session
            string _email = context.HttpContext.Session.GetString("email");
            //neu ko ton tai thi chuyen den login
            if (string.IsNullOrEmpty(_email))
                context.HttpContext.Response.Redirect("/Admin/Account/Login");
            base.OnActionExecuting(context);
        }
    }
}
