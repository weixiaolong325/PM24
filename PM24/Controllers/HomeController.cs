using PM24.ViewModes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace PM24.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //1.查看session是否已经有用户登录
            HttpCookie cookie=Request.Cookies.Get("UserId");
            IndexView indexViewModel = new IndexView();
            if (Session["UserId"] != null)
            {
                indexViewModel.LoginState = "yes";
                indexViewModel.UserId = Session["UserId"].ToString();
            }
              //2.看cookie是否保存有用户登录信息
            else if (cookie != null)
            {
                indexViewModel.LoginState = "yes";
                Session["UserId"] = cookie.Value;
                indexViewModel.UserId = Session["UserId"].ToString();
            }
            else
            {
                indexViewModel.LoginState = "no";
            }
            //3.查询用户昵称
            if (indexViewModel.UserId != null)
            {
                string sql = "select Name from PM24_Users where UserId=@UserId";
                SqlParameter[] param = { new SqlParameter("@UserId", SqlDbType.VarChar, 20) { Value = indexViewModel.UserId } };
                indexViewModel.Name = SQLHelper.ExecuteScalar(sql, CommandType.Text, param);
            }
            return View(indexViewModel);
        }

    }
}
