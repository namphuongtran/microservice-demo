using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Framework.Common.Facache;
using Framework.Entities.API;

namespace Framework.Common.Http
{
    public class ConnectAPI
    {
        public static async Task<ResponseData> GetToken(RequestInfor requestInfor)
        {
            ResponseData responseData = new ResponseData();

            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(requestInfor.UrlBase) })
                {
                    string creds = String.Format("{0}:{1}", requestInfor.HeaderValue.SecretUser, requestInfor.HeaderValue.SecretPass);
                    byte[] bytes = Encoding.ASCII.GetBytes(creds);
                    var header = new AuthenticationHeaderValue(requestInfor.HeaderValue.AuthorizationType, Convert.ToBase64String(bytes));

                    client.DefaultRequestHeaders.Authorization = header;

                    var request = await client.PostAsync("Token", new FormUrlEncodedContent(requestInfor.FormValue));

                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        var jToken = Helpers.Deserialize<AuthenticationToken>(request.Content.ReadAsStringAsync().Result);

                        responseData = new ResponseData()
                        {
                            Code = (int)HttpStatusCode.OK,
                            Data = jToken.access_token,
                            Message = "Get access_token successful!"
                        };
                    }
                    else
                    {
                        var errorData = Helpers.Deserialize<ErrorData>(request.Content.ReadAsStringAsync().Result);

                        responseData = new ResponseData()
                        {
                            Code = (int)request.StatusCode,
                            Message = errorData.error_description//errorData.error + " - " + 
                        };
                    }
                }
            }
            catch (Exception e)
            {
                responseData = new ResponseData()
                {
                    Code = 0,
                    Message = "Hệ thống đang bảo trì hoặc gặp sự cố kỹ thuật, xin vui lòng quay lại sau! => " + e.Message
                };
                // Write log file
            }

            return responseData;
        }

        public static async Task<ResponseData> ConnectRestAPI(RequestInfor requestInfor, MethodType type)
        {
            ResponseData responseData = new ResponseData();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (requestInfor.HeaderValue != null)
                    {
                        client.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue(requestInfor.HeaderValue.AuthorizationType, requestInfor.HeaderValue.AuthorizationValue);
                    }

                    var request = new HttpResponseMessage();
                    switch (type)
                    {
                        case MethodType.GET:
                            request = await client.GetAsync(requestInfor.UrlBase);
                            break;
                        case MethodType.POST:
                            request = await client.PostAsync(requestInfor.UrlBase, new FormUrlEncodedContent(requestInfor.FormValue));
                            break;
                        case MethodType.PUT:
                            request = await client.PutAsync(requestInfor.UrlBase, new FormUrlEncodedContent(requestInfor.FormValue));
                            break;
                        case MethodType.DELETE:
                            request = await client.DeleteAsync(requestInfor.UrlBase);
                            break;
                        default:
                            break;
                    }

                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string resultData = request.Content.ReadAsStringAsync().Result;

                        responseData = new ResponseData()
                        {
                            Code = (int)HttpStatusCode.OK,
                            Data = resultData,
                            Message = "Request successful!"
                        };
                    }
                    else if (request.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        responseData = new ResponseData()
                        {
                            Code = (int)HttpStatusCode.Unauthorized,
                            Message = "Bạn không có quyền thực hiện yêu cầu này!"
                        };
                    }
                    else
                    {
                        var errorData = Helpers.Deserialize<ErrorData>(request.Content.ReadAsStringAsync().Result);

                        responseData = new ResponseData()
                        {
                            Code = (int)request.StatusCode,
                            Message = errorData.error + " - " + errorData.error_description
                        };
                    }
                }
            } catch(Exception e)
            {
                responseData = new ResponseData()
                {
                    Code = 0,
                    Message = "Hệ thống đang bảo trì hoặc gặp sự cố kỹ thuật, xin vui lòng quay lại sau! => " + e.Message
                };
                // Write log file
            }

            return responseData;
        }
    }

    public class AuthenticationToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }
    }

    public class ErrorData
    {
        public string error { get; set; }

        public string error_description { get; set; }
    }

    public enum MethodType
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3
    }
}
