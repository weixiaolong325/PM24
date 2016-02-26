using PM24.Models;
using PM24.ViewModes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace PM24.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            //1.判断是否是在站内跳转过来的
            string url = Request.QueryString["reurl"];
            if (url != null)
                ViewBag.reurl = url;

            //2.判断cookie中是否存有用户登录信息
            HttpCookie cookie = Request.Cookies.Get("UserId");
            if (cookie != null)
            {
                Session["UserId"] = cookie.Value;

                if (url != null)
                {
                    //返回原来页面
                    return Redirect(url);
                }
                else
                    // return RedirectToAction("Index", "Home");
                    return Content("登录成功");
            }
            else
                return View();
     
        }
        //登录
        [HttpPost]
        public ActionResult Login(UserLoginView user)
        {
            if (ModelState.IsValid)
            {
                //1.密码MD5转换
                user.Pwd = MD5.ToMD5(user.Pwd);
                //2.判断用户名密码是否正确
                string sql = "select COUNT(1) from PM24_Users where UserId=@UserId COLLATE Chinese_PRC_CS_AI_WS and Pwd=@Pwd";
                SqlParameter[] param ={new SqlParameter("@UserId",SqlDbType.VarChar,20){Value=user.UserId},
                                         new SqlParameter("@Pwd",SqlDbType.VarChar,64){Value=user.Pwd}};
                if (Convert.ToInt32(SQLHelper.ExecuteScalar(sql, CommandType.Text, param))<=0)
                {
                    ModelState.AddModelError("error_login", "用户名或密码错误");
                    return View();
                }
                //3.用户名密码正确则保持用户登录状态
                Session["UserId"] = user.UserId;
                //4.如果用户选择一周免登录,则把用户保存在cookie中
                if (Request.Form["remberpwd"] != null)
                {
                    HttpCookie cookie = new HttpCookie("UserId");
                    cookie.Expires = DateTime.Now.AddDays(7);
                    cookie.Value = user.UserId;
                    Response.Cookies.Add(cookie);
                }
                //5.如果是站内跳转来的则回到原来链接
                string reurl = Request.Form["reurl"];
                if (!string.IsNullOrEmpty(reurl))
                {
                    return Redirect(reurl);
                }
                else

                    return RedirectToAction("Index", "Home");
               
            }
            return View();
            
        }
        //退出登录
        public ActionResult ExitLogin()
        {
            Session.Remove("UserId");
            HttpCookie cookie = Request.Cookies.Get("UserId");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Logup()
        {
            return View();
        }
        //注册
        [HttpPost]
        public ActionResult Logup(UserInfoView userInfoView)
        {
            //model通过验证
            if (ModelState.IsValid)
            {
                try
                {
                    UserInfo userInfo = new UserInfo();
                    userInfo = userInfoView.userInfo;
                    //1.判断验证码是否正确
                    if (Session["validateCode"] == null)
                    {
                        ModelState.AddModelError("err_validateCode", "验证码不正确");
                        return View();
                    }
                    else if (userInfoView.validateCode != Session["validateCode"].ToString())
                    {
                        ModelState.AddModelError("err_validateCode", "验证码不正确");
                        return View();
                    }
                    //2.判断两次密码是否一致
                    if (userInfoView.userInfo.Pwd != userInfoView.Pwd2)
                    {
                        ModelState.AddModelError("err_pwdNoEqual", "密码不一致");
                        return View();
                    }
                    //3.判断用户是否已经存在
                    string sql_UserExist = "select count(1) from PM24_Users where UserId=@UserId";
                    SqlParameter[] param_UserExist = { new SqlParameter("@UserID", SqlDbType.VarChar, 20) { Value = userInfo.UserId } };
                    if(Convert.ToInt32(SQLHelper.ExecuteScalar(sql_UserExist, CommandType.Text, param_UserExist))>0)
                    {
                        ModelState.AddModelError("err_UserIsExit", "用户名已被用");
                        return View();
                    }

                    //4.把数据插入数据库
                    //获取用户IP
                    string IP = Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress;
                    userInfo.CreateIp = IP;
                    if (userInfo.Name == null) userInfo.Name = userInfo.UserId;
                    if (userInfo.Email == null) userInfo.Email = "";
                    userInfo.Status = 1;
                    //密码进行md5加盐加密
                    string pwd = MD5.ToMD5(userInfo.Pwd);

                    //sql语句
                    string sql = "insert into PM24_Users(UserId,Pwd,Name,Email,CreateIp,Status) values (@UserId,@Pwd,@Name,@Email,@CreateIp,@Status)";
                    SqlParameter[] param ={new SqlParameter("@UserId",SqlDbType.VarChar,20){Value=userInfo.UserId},
                                       new SqlParameter("@Pwd",SqlDbType.VarChar,64){Value=pwd},
                                       new SqlParameter("@Name",SqlDbType.NVarChar,10){Value=userInfo.Name},
                                       new SqlParameter("@Email",SqlDbType.VarChar,50){Value=userInfo.Email},
                                       new SqlParameter("@CreateIP",SqlDbType.VarChar,50){Value=userInfo.CreateIp},
                                       new SqlParameter("@Status",SqlDbType.Int){Value=userInfo.Status}};

                    if (SQLHelper.ExecuteNonQuery(sql, CommandType.Text, param) > 0)
                    {
                        return RedirectToAction("SignInSuccess");
                    }
                    else {
                        return View();
                    }
                }
                catch
                {
                    return View();
                }
            }
            else 
               return View();
        }
        //显示验证码
        public  ActionResult CreateValidateCode()
        {
            //生成验证码(随机码)
            string validateCode = new Random().Next(1000, 10000).ToString();
            //把验证码保存到session
            Session["validateCode"] = validateCode;
            //根据验证码生成图片
          byte[] bytes= CreateValidateGraphic(validateCode);
            //显示验证码
          return File(bytes, @"image/jpeg");
        }
        public ActionResult SignInSuccess()
        {
            return View();
        }
        /// <summary>
        /// 根据验证码生成图片
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            //创建一个图像，宽度跟据字体长度*12
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            //获取Image的画布
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                //设置字体样式
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                //创建一个画刷
                LinearGradientBrush brush = new LinearGradientBrush
                (new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                //在图片中加文字
                g.DrawString(validateCode, font, brush, 3, 2);

                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                Response.Clear();
                Response.ContentType = "image/jpeg";
                return  stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

    }
}
