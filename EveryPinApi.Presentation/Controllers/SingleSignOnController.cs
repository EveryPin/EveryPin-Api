﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.DataTransferObject;
using Shared.DataTransferObject.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EveryPinApi.Presentation.Controllers
{
    [Route("api/single-sign-on")]
    [ApiController]
    public class SingleSignOnController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IServiceManager _service;

        public SingleSignOnController(ILogger<SingleSignOnController> logger, IServiceManager service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("kakao-login")]
        public async Task<IActionResult> KakaoLogin(string code)
        {
            try
            {
                // URL에 포함된 code를 이용하여 액세스 토큰 발급
                string kakaoAccessToken = await _service.SingleSignOnService.GetKakaoAccessToken(code);

                // 액세스 토큰을 이용하여 카카오 서버에서 유저 정보(카톡 닉네임, 이메일) 받아오기
                SingleSignOnUserInfo userInfo = await _service.SingleSignOnService.GetKakaoUserInfo(kakaoAccessToken);

                // 만일, DB에 해당 email을 가지는 유저가 없으면 회원가입 시키고 유저 식별자와 JWT 반환
                var user = await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);

                if (user)
                {
                    var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

                    return Ok(tokenDto);
                }
                else
                {
                    RegistUserDto registerUser = new RegistUserDto()
                    {
                        Name = userInfo.UserNickName,
                        UserName = userInfo.UserNickName,
                        Email = userInfo.UserEmail,
                        Password = "0",
                        PlatformCodeId = 1,
                        Roles = new List<string>() { "NormalUser" }
                    };

                    IdentityResult registedUser = await _service.AuthenticationService.RegisterUser(registerUser);

                    if (registedUser.Succeeded && await _service.AuthenticationService.ValidateUser(userInfo.UserEmail))
                    {
                        
                        var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

                        return Ok(tokenDto);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }

            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpGet("google-login")]
        //public async Task<IActionResult> GoogleLogin(string code)
        public async Task<IActionResult> GoogleLogin(string googleAccessToken)
        {
            try
            {
                // URL에 포함된 code를 이용하여 액세스 토큰 발급
                //GoogleTokenDto googleAccessToken = await _service.SingleSignOnService.GetGoogleAccessToken(code);

                // 액세스 토큰을 이용하여 구글 서버에서 유저 정보(이메일) 받아오기
                SingleSignOnUserInfo userInfo = await _service.SingleSignOnService.GetGoogleUserInfo(googleAccessToken);

                // 만일, DB에 해당 email을 가지는 유저가 없으면 회원가입 시키고 유저 식별자와 JWT 반환
                var isUserInDb = await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);

                if (isUserInDb)
                {
                    var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

                    return Ok(tokenDto);
                }
                else
                {
                    RegistUserDto registerUser = new RegistUserDto()
                    {
                        Name = userInfo.UserNickName,
                        UserName = userInfo.UserNickName,
                        Email = userInfo.UserEmail,
                        Password = "0",
                        PlatformCodeId = 1,
                        Roles = new List<string>() { "NormalUser" }
                    };

                    IdentityResult registedUser = await _service.AuthenticationService.RegisterUser(registerUser);

                    if (registedUser.Succeeded && await _service.AuthenticationService.ValidateUser(userInfo.UserEmail))
                    {
                        var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

                        return Ok(tokenDto);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }

            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }
    }
}