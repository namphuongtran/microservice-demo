using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using IdentityServer.Entity;
using IdentityServer.Library;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IOptions<Audience> _settings;
        private readonly IAccountService _sv;
        //private ITokenRepository _tokenRepository;

        public TokenController(IOptions<Audience> settings, IAccountService sv)//, ITokenRepository tokenRepository)
        {
            _settings = settings;
            _sv = sv;
            //_tokenRepository = tokenRepository;
        }

        [HttpGet("auth")]
        //[Authorize]
        public async Task<IActionResult> AuthAsync([FromQuery]Parameters parameters)
        {
            if (parameters == null)
            {
                return Json(new ResponseData
                {
                    Code = "901",
                    Message = "Null parameters",
                    Data = null
                });
            }

            if (parameters.grant_type == "password")
            {
                return Json(await DoPasswordAsync(parameters));
            }
            else if (parameters.grant_type == "refresh_token")
            {
                return Json(await DoPasswordAsync(parameters));// await DoRefreshTokenAsync(parameters));
            }
            else
            {
                return Json(new ResponseData
                {
                    Code = "904",
                    Message = "Bad request",
                    Data = null
                });
            }
        }

        //scenario 1 ： get the access-token by username and password
        private async Task<ResponseData> DoPasswordAsync(Parameters parameters)
        {
            //validate the client_id/client_secret/username/password          
            bool isValidUser = await _sv.IsValidUser(parameters.username, parameters.password);

            ////var user = UserInfo.GetAllUsers().SingleOrDefault(x => x.ClientId == parameters.client_id
            ////                        && x.ClientSecret == parameters.client_secret
            ////                        && x.UserName == parameters.username
            ////                        && x.Password == parameters.password);

            if (!isValidUser)
            {
                return new ResponseData
                {
                    Code = "902",
                    Message = "Invalid user infomation",
                    Data = null
                };
            }

            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

            var token = new RefreshToken
            {
                ClientId = parameters.client_id,
                Token = refresh_token,
                Id = Guid.NewGuid().ToString(),
                IsStop = 0,
                UserName = parameters.username
            };

            //store the refresh_token 
            ////if (await _tokenRepository.AddTokenAsync(token))
            ////{
            ////    return new ResponseData
            ////    {
            ////        Code = "999",
            ////        Message = "Ok",
            ////        Data = GetJwt(parameters.client_id, user.UserName, refresh_token, _settings.Value.ExpireMinutes)
            ////    };
            ////}
            ////else
            ////{
            ////    return new ResponseData
            ////    {
            ////        Code = "909",
            ////        Message = "Cannot add token to database",
            ////        Data = null
            ////    };
            ////}

            return new ResponseData
            {
                Code = "999",
                Message = "Ok",
                Data = GetJwt(parameters.client_id, parameters.username, refresh_token, _settings.Value.ExpireMinutes)
            };
        }

        //scenario 2 ： get the access_token by refresh_token
        //private async Task<ResponseData> DoRefreshTokenAsync(Parameters parameters)
        //{
        //    var token = await _tokenRepository.GetTokenAsync(parameters.refresh_token, parameters.client_id);

        //    if (token == null)
        //    {
        //        return new ResponseData
        //        {
        //            Code = "905",
        //            Message = "can not refresh token",
        //            Data = null
        //        };
        //    }

        //    if (token.IsStop == 1)
        //    {
        //        return new ResponseData
        //        {
        //            Code = "906",
        //            Message = "refresh token has expired",
        //            Data = null
        //        };
        //    }

        //    var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

        //    token.IsStop = 1;

        //    //expire the old refresh_token and add a new refresh_token
        //    var updateFlag = await _tokenRepository.ExpireTokenAsync(token);

        //    var addFlag = await _tokenRepository.AddTokenAsync(new RefreshToken
        //    {
        //        ClientId = parameters.client_id,
        //        Token = refresh_token,
        //        Id = Guid.NewGuid().ToString(),
        //        IsStop = 0,
        //        UserName = token.UserName
        //    });

        //    if (updateFlag && addFlag)
        //    {
        //        return new ResponseData
        //        {
        //            Code = "999",
        //            Message = "Ok",
        //            Data = GetJwt(parameters.client_id, token.UserName, refresh_token, _settings.Value.ExpireMinutes)
        //        };
        //    }
        //    else
        //    {
        //        return new ResponseData
        //        {
        //            Code = "910",
        //            Message = "Cannot expire token or create a new token",
        //            Data = null
        //        };
        //    }
        //}

        private string GetJwt(string clientId, string userName, string refreshToken, int expireMinutes)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, clientId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                new Claim("Name", userName)
            };

            var symmetricKeyAsBase64 = _settings.Value.Secret;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
                issuer: _settings.Value.Iss,
                audience: _settings.Value.Aud,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(expireMinutes)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(expireMinutes).TotalSeconds,
                refresh_token = refreshToken,
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}