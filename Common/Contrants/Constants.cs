using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Common
{
    public class Constants
    {
        public static Dictionary<string, Type> CREATED_DYNAMIC_TYPE = new Dictionary<string, Type>();
        public enum TYPE_DATA_CAMPARE { STRING, DATE, DATE_TIME, INT, FLOAT, BOOL };
        public static readonly string CONF_CROSS_ORIGIN = "CROSS_ORIGIN";
        public static readonly string KEY_SESSION_TENANT_ID = "KEY_SESSION_TENANT_ID";
        public static readonly int SERVICE_CODE = 3;
        public static readonly string KEY_SESSION_USER_ID = "KEY_SESSION_USER_ID";
        public static readonly string KEY_SESSION_EMAIL = "KEY_SESSION_EMAIL";
        public static readonly string LOG_USER_CREATE = "Log User Create";
        public static readonly string CONF_STATE_SOURCE = "STATE_SOURCE";
        public static readonly string STATE_SOURCE_DEV = "dev";
        public static readonly string KEY_SESSION_IS_SECRET_KEY = "KEY_SESSION_IS_SECRET_KEY";
        public static readonly string ERROR_MAPPING_MODEL = "Error mapping models";
        public static readonly string RECORD_NOT_FOUND = "Record not found";
        public static readonly string CONF_MAX_ERROR_MESS = "MAX_ERROR_MESS";
        public static readonly string CONF_MAX_DEGREE_PARALLELISM = "MAX_DEGREE_PARALLELISM";
        public static readonly string CONF_KAFKA_BOOSTRAP_SERVER = "KAFKA_BOOSTRAP_SERVER";
        public static readonly string CONF_KAFKA_GROUP_ID = "KAFKA_GROUP_ID";

        public static ModuleBuilder MODULE_BUILDER = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Dynamic Assembly"), AssemblyBuilderAccess.Run).DefineDynamicModule("MainModule");
    }
}
