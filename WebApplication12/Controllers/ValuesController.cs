using Microsoft.Ajax.Utilities;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApplication12.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        [ActionName("ll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("{id}")]
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("post")]
        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        //// PUT api/values/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}

        [HttpGet]
        [Route("SampleGet")]
        public int SampleGet(string val)
        {
            return 0;
        }

        [HttpGet]
        [Route("Message")]
        public void Message()
        {
            //var h = HttpContext;

            var ctx = HttpContext.Current;
            var k = new HttpResponseMessage();
            
            ctx.Response.Headers.Add("Content-Type", "text/event-stream");
            //Response.Connection = "keep-alive";
            ctx.Response.Headers.Add("Connection", "keep-alive");

            //ctx.Response.Headers.Add("Cache-Control", "no-cache");
            //ctx.Response.Headers.Add("Content-Type", "text/event-stream");
            //await ctx.Response.FlushAsync();


            DateTime startDate = DateTime.Now;
            try
            {

                while (startDate.AddMinutes(1) > DateTime.Now)
                {
                    ctx.Response.Write(string.Format("data: {0}\n\n", DateTime.Now.ToString()));
                    ctx.Response.Flush();

                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {

            }

            ctx.Response.Close();
        }

        [HttpPost]
        public int PostSSEFromValues(string lll)
        {

            SseListenerController.MessageCallback(lll);
            return lll.Length;

        }
    }
}
