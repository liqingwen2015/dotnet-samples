using System.ComponentModel;
using System.Reflection;

namespace MicroService.CorrelationId.Core
{
    
    public static class EnumHelper
    {
        public static string GetEnumDescription(Enum obj)
        {
            FieldInfo fi = obj.GetType().GetField(obj.ToString());
            DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (arrDesc == null)
            { return string.Empty; }
            else
            {
                return arrDesc[0].Description;
            }
        }
        /// <summary>  
        /// 枚举转字典集合  
        /// </summary>  
        /// <typeparam name="T">枚举类名称</typeparam>  
        /// <param name="keyDefault">默认key值</param>  
        /// <param name="valueDefault">默认value值</param>  
        /// <returns>返回生成的字典集合</returns> 
        public static Dictionary<int, string> EnumListDic<T>(int keyDefault, string valueDefault = "")
        {
            Dictionary<int, string> dicEnum = new Dictionary<int, string>();
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                return dicEnum;
            }
            if (!string.IsNullOrEmpty(valueDefault))
            {
                dicEnum.Add(keyDefault, valueDefault);
            }
            string[] fieldstrs = Enum.GetNames(enumType);
            foreach (var item in fieldstrs)
            {
                string description = string.Empty;
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (arr != null && arr.Length > 0)
                {
                    description = ((DescriptionAttribute)arr[0]).Description;
                }
                else
                {
                    description = item;
                }
                dicEnum.Add((int)Enum.Parse(enumType, item), description);
            }
            return dicEnum;
        }
        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="status">枚举的值</param>
        /// <returns></returns>
        public static string GetEnumName<T>(this int status)
        {
            try
            {
                return Enum.GetName(typeof(T), status);
            }
            catch (Exception ex)
            {
                return status.ToString();
            }
        }
        /// <summary>
        /// 获取枚举名称集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetNamesArr<T>()
        {
            return Enum.GetNames(typeof(T));
        }
    }
}