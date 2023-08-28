using BasicIC_SendEmail.Config;
using Common;
using Common.Commons;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BasicIC_SendEmail.Common
{
    public class RequestAPI
    {
        private HttpClient _client;
        private readonly string hostFabio = ConfigManager.StaticGet(Constants.CONF_HOST_FABIO_SERVICE);

        public HttpClient client
        {
            get => _client;
            set => _client = value;
        }

        public RequestAPI() { }

        private void DefaultSetting()
        {
            var handler = new MiddlewareHandler(new HttpClientHandler { UseDefaultCredentials = true });
            _client = new HttpClient(handler);

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public class MiddlewareHandler : DelegatingHandler
        {
            public MiddlewareHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
            {
            }

            protected async override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                //var requestDate = request.Headers.Date;
                // do something with the date ...

                // handle respose
                var response = await base.SendAsync(request, cancellationToken);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    string mess = string.Format("{0} {1} {2}", response.ReasonPhrase.ToString(), ((int)response.StatusCode).ToString(), response.RequestMessage.RequestUri);
                    await CommonFunc.LogErrorToKafka(mess);
                }

                return response;
            }
        }

        // rest api by manual
        public RequestAPI(string confBaseUrl, string token = "")
        {
            DefaultSetting();
            _client.BaseAddress = new Uri(ConfigManager.StaticGet(confBaseUrl));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // rest api by consult
        public RequestAPI(ResponseService<string> confBaseUrl, string token = "", string preDomainApi = "/api/")
        {
            DefaultSetting();
            _client.BaseAddress = new Uri(confBaseUrl.data + preDomainApi);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // rest api by fabio
        public RequestAPI ToFabio(string sourceFabio, string token = "", string preDomainApi = "/api/")
        {
            DefaultSetting();
            _client.BaseAddress = new Uri(hostFabio + sourceFabio + preDomainApi);
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Add("API_SECRET_KEY", ConfigManager.StaticGet("API_SECRET_KEY"));
            }
            return this;
        }
    }
}