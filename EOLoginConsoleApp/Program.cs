using EO.Login_Controller;
using EO.ViewModels.ControllerModels;
using EO.ViewModels.DataModels;
using LoginServiceLayer.Interface;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Topshelf;

namespace EOLoginConsoleApp
{
    public class LoginConsoleApp
    {
        static void Main(string[] args)
        {
            //string baseAddress = "http://localhost:9000/";
            //string baseAddress = "http://192.168.1.3:9000/";

            StartOptions options = new StartOptions();
            options.Urls.Add("http://localhost:9000");
            options.Urls.Add("http://127.0.0.1:9000");
            //options.Urls.Add(GetNetworkConfig());
            options.Urls.Add("http://192.168.0.129:9000");    //Me

            //options.Urls.Add("http://192.168.1.134:9000/");   //Thom

            options.Urls.Add("http://*:9000");

            options.Urls.Add("http://eo.hopto.org:9000");  //No-Ip Vince's account

            //options.Urls.Add("http://192.168.1.1:9000");
            //options.Urls.Add("http://192.168.1.2:9000");
            //options.Urls.Add("http://192.168.1.3:9000");


            // Start OWIN host 
            using (WebApp.Start<Startup>(options))
            //using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                //var response = client.GetAsync(baseAddress + "api/login").Result;

                Console.WriteLine("The Elegant Orchids Login Service is now running");

                LoginController controller = new LoginController();

                while (Console.ReadLine() == String.Empty)
                {

                }
            }
        }

        static private string GetNetworkConfig()
        {
            string LAN_Address = "http://127.0.0.1:9000";

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection nics = mc.GetInstances();

            List<string> enabledIPs = new List<string>();

            foreach (ManagementObject nic in nics)
            {
                if (Convert.ToBoolean(nic["ipEnabled"]) == true)
                {
                    string IpAddress = (nic["IPAddress"] as String[])[0];

                    enabledIPs.Add(IpAddress);

                    string IPSubnet = (nic["IPSubnet"] as String[])[0];

                    string DefaultGateWay = (nic["DefaultIPGateway"] as String[])[0];
                }
            }

            if (enabledIPs.Count > 1)
            {
                LAN_Address = enabledIPs[1];

                LAN_Address = "http://" + LAN_Address + ":9000";
            }

            return LAN_Address;
        }
    }

    public class HttpMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.RequestUri.AbsoluteUri.Contains("swagger"))
            {
                if (!ValidateKey(request))
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(resp);
                    return tsc.Task;
                }
            }
            Task<HttpResponseMessage> response = base.SendAsync(request, cancellationToken);

            //ApiResponse r = response.Result.Content.ReadAsAsync<ApiResponse>().Result;
            //if(r.Success)

            if (response.Result.IsSuccessStatusCode)
            {
                IEnumerable<string> values;
                request.Headers.TryGetValues("EO-Header", out values);
                if (values != null && values.ToList().Count == 1)
                {
                    response.Result.Headers.Add("EO-Header", values.First());
                }
            }
           
            return response;
        }

        private bool ValidateKey(HttpRequestMessage message)
        {
            bool success = false;

            var query = message.RequestUri.ParseQueryString();
            IEnumerable<string> values;
            message.Headers.TryGetValues("EO-Header", out values);
            if(values != null && values.ToList().Count == 1 )
            {
                LoginManager manager = new LoginManager();
                LoginDTO login = new LoginDTO();
                string[] userNamePwd = values.First().Split(':');
                if (userNamePwd.Length == 2)
                {
                    login.UserName = userNamePwd[0].Trim();
                    login.Password = userNamePwd[1].Trim();
                    login = manager.GetUser(login);
                    if(login.UserId > 0)
                    {
                        success = true;
                    }
                }
           }

            return success;
        }
    }

    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MessageHandlers.Add(new HttpMessageHandler());

            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings();

            SwaggerConfig.Register(config);

            SwaggerConfig.Register();

            appBuilder.UseWebApi(config);
        }
    }
}
