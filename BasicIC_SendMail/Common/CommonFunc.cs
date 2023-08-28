using BasicIC_SendEmail.App_Start;
using BasicIC_SendEmail.KafkaComponents;
using BasicIC_SendEmail.Models.Main;
using BasicIC_SendEmail.RestAPI;
using Common.Interfaces;
using Ninject;
using Repository.EF;
using SocialConnection.KafkaComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicIC_SendEmail.Common
{
    public class CommonFunc
    {
        private static ILogger _logger = CreateInstanceDJ<ILogger>();
        private static List<M02_DefaultCommonSetting> listEmailSetting;
        private static readonly DefaultCommonSettingAPI defaultCommonSettingApi = new DefaultCommonSettingAPI();
        public static T CreateInstanceDJ<T>()
        {
            return NinjectWebCommon.kernel.Get<T>();
        }
        public static async Task LogErrorToKafka(string topic, Exception ex)
        {
            //send log kafka
            LogSystemErrorModel logMess = new LogSystemErrorModel(ex, topic);
            ProducerWrapper<LogSystemErrorModel> _producer = new ProducerWrapper<LogSystemErrorModel>();
            await _producer.CreateMess(Topic.LOG_ERROR_SYSTEM, logMess);
        }
        public static async Task LogErrorToKafka(string mess)
        {
            //send log kafka
            LogSystemErrorModel logMess = new LogSystemErrorModel(mess);
            ProducerWrapper<LogSystemErrorModel> _producer = new ProducerWrapper<LogSystemErrorModel>();
            await _producer.CreateMess(Topic.LOG_ERROR_SYSTEM, logMess);
        }
        public static string GetValueDefaultCommonSettingFromMemory(string key)
        {
            if (listEmailSetting == null)
                Task.WaitAll(GetAllDefaultCommonSettingToMemory());

            var value = listEmailSetting.FirstOrDefault(q => q.key.Equals(key))?.value;
            if (string.IsNullOrEmpty(value))
                _logger.LogError("Default common setting error: key missing " + key);
            return value;
        }
        public static async Task GetAllDefaultCommonSettingToMemory()
        {
            var responseEmailSetting = await defaultCommonSettingApi.GetAll();
            if (!responseEmailSetting.status)
                _logger.LogError(responseEmailSetting.message);
            else
                listEmailSetting = responseEmailSetting.data.items;
        }
    }
}