using BasicIC_SendEmail.Common;
using BasicIC_SendEmail.Services.Implement;
using Common;
using Common.Commons;
using Common.Params.Base;
using Repository.CustomModel;
using Repository.EF;
using System.Net.Http;
using System.Threading.Tasks;

namespace BasicIC_SendEmail.RestAPI
{
    public class DefaultCommonSettingAPI : BaseService
    {
        private readonly RequestAPI _requestAPI;
        private HttpResponseMessage _response;
        private const string GET_ALL_ENDPOINT = "default-common-setting/get-all";

        public DefaultCommonSettingAPI(string token = "")
        {
            _requestAPI = new RequestAPI().ToFabio(Constants.SOURCE_FABIO_SETTING, token);
        }

        public async Task<ResponseService<ListResult<M02_DefaultCommonSetting>>> GetAll()
        {
            PagingParam pagingParam = new PagingParam();
            var result = new ResponseService<ListResult<M02_DefaultCommonSetting>>();
            _response = await _requestAPI.client.PostAsJsonAsync(GET_ALL_ENDPOINT, pagingParam);
            if (_response.IsSuccessStatusCode)
            {
                result = await _response.Content.ReadAsAsync<ResponseService<ListResult<M02_DefaultCommonSetting>>>();
            }
            else
            {
                var error = await _response.Content.ReadAsAsync<ResponseService<object>>();
                result.status = false;
            }
            return result;
        }
    }
}