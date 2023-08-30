using BasicIC_SendEmail.Services.Implement;
using Common;
using Common.ApiHelper;
using Common.Commons;
using Common.Params.Base;
using Repository.CustomModel;
using Repository.EF;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BasicIC_SendEmail.RestAPI
{
    public class DefaultCommonSettingAPI : BaseService
    {
        //private readonly RestfulApi restfulApi;
        public DefaultCommonSettingAPI()
        {

        }
        public DefaultCommonSettingAPI(string token = "")
        {
            //restfulApi = new RestfulApi().ToFabio(Constants.SOURCE_FABIO_SETTING, token);
            //restfulApi = new RestfulApi().ToFabio("main", token);
        }

        public async Task<string> GetByKey(string key)
        {
            RestfulApi restfulApi = new RestfulApi().ToFabio(Constants.SOURCE_FABIO_SETTING, "");
            PagingParam pagingParam = new PagingParam();
            var _response = await restfulApi.client.PostAsJsonAsync(Constants.PATH_PRE_API_SETTING, pagingParam);
            var result = _response.Content.ReadAsAsync<ResponseService<ListResult<M02_DefaultCommonSetting>>>();
            var value = result.Result.data.items.FirstOrDefault(q => q.key.Equals(key))?.value;
            if (string.IsNullOrEmpty(value))
                _logger.LogError("Default common setting error: key missing " + key);
            return value;
        }
    }
}