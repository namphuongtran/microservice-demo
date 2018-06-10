using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Common.Facache
{
    public static class Helpers
    {
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            string retVal = string.Empty;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType(),
                new DataContractJsonSerializerSettings()
                {
                    DateTimeFormat = new DateTimeFormat("yyyy'-'MM'-'dd'T'HH':'mm':'ss")
                });
                MemoryStream ms = new MemoryStream();
                serializer.WriteObject(ms, obj);
                retVal = Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                retVal = string.Empty;
            }

            return retVal;

            //return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));

            if (string.IsNullOrEmpty(json))
            {
                ms.Close();
                return obj;
            }

            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType(),
                new DataContractJsonSerializerSettings()
                {
                    DateTimeFormat = new DateTimeFormat("yyyy'-'MM'-'dd'T'HH':'mm':'ss")
                });
                obj = (T)serializer.ReadObject(ms);
            }
            catch (Exception)
            {
                try
                {
                    ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType(),
                        new DataContractJsonSerializerSettings()
                        {
                            DateTimeFormat = new DateTimeFormat("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff")
                        });
                    obj = (T)serializer.ReadObject(ms);
                }
                catch (Exception)
                {
                    try
                    {
                        ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType(),
                            new DataContractJsonSerializerSettings()
                            {
                                DateTimeFormat = new DateTimeFormat("yyyy'-'MM'-'dd'T'HH':'mm':'ss.ff")
                            });
                        obj = (T)serializer.ReadObject(ms);
                    }
                    catch (Exception)
                    {
                        ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType(),
                            new DataContractJsonSerializerSettings()
                            {
                                DateTimeFormat = new DateTimeFormat("yyyy'-'MM'-'dd'T'HH':'mm':'ss.f")
                            });
                        obj = (T)serializer.ReadObject(ms);
                    }
                }
            }

            ms.Close();
            return obj;

            //return JsonConvert.DeserializeObject<T>(json);
        }

        public static string EncryptPassWord(string pPassword)
        {

            SHA256 sha256 = SHA256.Create();

            byte[] data = sha256.ComputeHash(Encoding.Unicode.GetBytes(pPassword));

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public static List<KeyValuePair<string, string>> ConvertObToKeyPair(object obj)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

            PropertyInfo[] propInfos = obj.GetType().GetProperties();
            foreach (var item in propInfos)
            {
                list.Add(new KeyValuePair<string, string>(item.Name, (item.GetValue(obj) ?? "").ToString()));
            }

            return list;
        }

        public static string FormatNumber(decimal value, byte valDecimal)
        {
            return value.ToString("N" + valDecimal.ToString());
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            if (lowerCase)
                return sb.ToString().ToLower();
            return sb.ToString();
        }
    }
}
