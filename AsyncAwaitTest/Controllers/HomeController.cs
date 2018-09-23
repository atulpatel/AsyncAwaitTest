using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AsyncAwaitTest.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> IndexAsync()
        {
            Debug.WriteLine("========================== IndexAsync Start time: {0}", DateTime.Now);

            HttpClient client = new HttpClient();

            Task<string> getstringTask = client.GetStringAsync("http://msdn.microsoft.com");

            var taskasync = DoAsyncWork();

            DoIndependentWork();

            string urlcontents = await getstringTask;

            int length = await taskasync;
            Debug.WriteLine("XXX========================== IndexAsync END time: {0}", DateTime.Now);
            return View("Index",new HtmlString(urlcontents));
        }

        public ActionResult Index()
        {
            Debug.WriteLine("========================== Index Start time: {0}", DateTime.Now);
            HttpClient client = new HttpClient();

            //Task<string> getstringTask = ;
            var getstringTask = Task.Run(async () => await client.GetStringAsync("http://msdn.microsoft.com"));
            
            var taskasync = Task.Run(async () => await DoAsyncWork());

            DoIndependentWork();

            string urlcontents = getstringTask.Result;

            int length = taskasync.Result;
            Debug.WriteLine("XXX========================== Index End time: {0}", DateTime.Now);

            return View(new HtmlString(urlcontents));
        }

        public ActionResult IndexSync()
        {
            Debug.WriteLine("========================== IndexSync Start time: {0}", DateTime.Now);
            HttpClient client = new HttpClient();

            //Task<string> getstringTask = ;
            var getstringTask = client.GetStringAsync("http://msdn.microsoft.com");
            string urlcontents = getstringTask.Result;

            var taskasync = DoSyncWork();

            DoIndependentWork();


            Debug.WriteLine("XXX========================== IndexSync End time: {0}", DateTime.Now);

            return View("Index",new HtmlString(urlcontents));
        }


        private void DoIndependentWork()
        {
            Thread.Sleep(2000);
        }

        private async Task<int> DoAsyncWork()
        {
            Debug.WriteLine("start DoAsyncWork:{0} ", DateTime.Now);
            HttpClient client = new HttpClient();
            Thread.Sleep(2000);
            Task<string> getstringTask = client.GetStringAsync("http://msdn.microsoft.com");
            var length = (await getstringTask).Length;
            Debug.WriteLine("End DoAsyncWork: {0}", DateTime.Now);

            return length;
        }

        private int DoSyncWork()
        {
            Debug.WriteLine("start DoAsyncWork:{0} ", DateTime.Now);
            HttpClient client = new HttpClient();
            Thread.Sleep(2000);
            Task<string> getstringTask = client.GetStringAsync("http://msdn.microsoft.com");
            var length = getstringTask.Result.Length;
            Debug.WriteLine("End DoAsyncWork: {0}", DateTime.Now);

            return length;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}