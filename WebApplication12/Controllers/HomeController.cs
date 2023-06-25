using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication12.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public void Message()
        {
            
            Response.ContentType = "text/event-stream";
            //Response.Connection = "keep-alive";
            Response.AddHeader("Connection", "keep-alive");
            

            DateTime startDate = DateTime.Now;
            try
            {

            while (startDate.AddMinutes(1) > DateTime.Now)
            //while(true)
                {
                Response.Write(string.Format("data: {0}\n\n", DateTime.Now.ToString()));
                Response.Flush();

                System.Threading.Thread.Sleep(500);
            }
            }
            catch (Exception e)
            {

            }

            Response.Close();
        }
    }
}
