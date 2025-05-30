﻿using Contracts.Repository;
using Entites.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using Entites.Code;
using Google.Apis.PeopleService.v1.Data;
using Shared.Dtos.Common;

namespace Service.Models;

public class SingleSignOnService : ISingleSignOnService
{
    private readonly IConfiguration _configuration;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger<SingleSignOnService> _logger;
    private readonly Dictionary<string, Func<string, Task<SingleSignOnUserInfo>>> _platformStrategies;
    private string kakaoRedirectURI;
    private string kakaoClientId;
    private string googleRedirectURI;
    private string googleClientId;
    private string googleClientSecret;


    public SingleSignOnService(ILogger<SingleSignOnService> logger, IConfiguration configuration, IRepositoryManager repositoryManager) 
    {
        _logger = logger;
        _configuration = configuration;
        _repositoryManager = repositoryManager;
        _platformStrategies = new Dictionary<string, Func<string, Task<SingleSignOnUserInfo>>>
        {
            { nameof(CodePlatform.KAKAO), this.GetKakaoUserInfo },
            { nameof(CodePlatform.GOOGLE), this.GetGoogleUserInfoToIdToken }
        };

        string serverHost = _configuration["ASPNETCORE_URLS"];

        kakaoRedirectURI = $"{serverHost}/api/test/auth/kakao-web-login";
        googleRedirectURI = $"{serverHost}/api/test/auth/google-web-login";
        kakaoClientId = _configuration.GetConnectionString("kakao-rest-api-key");
        googleClientId = _configuration.GetConnectionString("google-client-id");
        googleClientSecret = _configuration.GetConnectionString("google-client-secret");
    }

    public async Task<SingleSignOnUserInfo> GetUserInfo(string platformCode, string accessToken)
    {
        if (!_platformStrategies.TryGetValue(platformCode.ToUpper(), out var GetUserInfoFromServiceProvider))
        {
            throw new Exception($"지원되지 않는 플랫폼: {platformCode}");
        }

        return await GetUserInfoFromServiceProvider(accessToken);
    }

    public async Task<string> GetKakaoAccessToken(string code)
    {
        string accessToken = "";
        //string refreshToken = "";
        string requestURL = "https://kauth.kakao.com/oauth/token";
        string authorizationCode = "authorization_code";
        string queryString = $"?grant_type={authorizationCode}" +
                             $"&client_id={kakaoClientId}" +
                             $"&redirect_uri={kakaoRedirectURI}" +
                             $"&code={code}";

        // HTTP 요청 생성
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL + queryString);
        request.Method = "POST";

        try
        {
            using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
            {
                int responseCode = (int)response.StatusCode;

                if (responseCode == 200)
                {
                    string jsonResponse;

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        jsonResponse = reader.ReadToEnd();

                        JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);
                        JsonElement root = jsonDocument.RootElement;

                        accessToken = root.GetProperty("access_token").GetString();
                        //refreshToken = root.GetProperty("refresh_token").GetString();
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }

        return accessToken;
    }

    public async Task<string> GetGoogleAccessToken(string code)
    {
        string requestURL = "https://oauth2.googleapis.com/token";
        string authorizationCode = "authorization_code";

        string postData = $"client_id={googleClientId}" +
                                $"&client_secret={googleClientSecret}" +
                                $"&code={code}" +
                                $"&grant_type={authorizationCode}" +
                                $"&redirect_uri={googleRedirectURI}";

        // HTTP 요청 생성
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);
        request.ContentType = "application/x-www-form-urlencoded";
        request.Method = "POST";
        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(postData);
        }

        try
        {
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                int responseCode = (int)response.StatusCode;

                if (responseCode == 200)
                {
                    string jsonResponse;

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        jsonResponse = reader.ReadToEnd();

                        JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);
                        JsonElement root = jsonDocument.RootElement;



                        string accessToken = root.GetProperty("access_token").GetString();
                        //int expires_in = root.GetProperty("expires_in").GetInt32();
                        //string refreshToken = root.GetProperty("refresh_token").GetString();
                        //string scope = root.GetProperty("scope").GetString();
                        //string id_token = root.GetProperty("id_token").GetString();

                        //return new GoogleTokenDto(accessToken, expires_in, refreshToken, scope, id_token);

                        return accessToken;
                    }
                }
                else
                {
                    throw new Exception("Google 토큰 요청에 실패했습니다.");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Google 토큰을 불러올 수 없습니다.");
        }
    }

    public async Task<SingleSignOnUserInfo> GetKakaoUserInfo(string kakaoAccessToken)
    {
        var result = new SingleSignOnUserInfo() { PlatformCode = CodePlatform.KAKAO };
        string postURL = "https://kapi.kakao.com/v2/user/me";

        // HTTP 요청 생성
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postURL);
        request.Method = "POST";
        request.Headers["Authorization"] = "Bearer " + kakaoAccessToken;

        using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
        {
            int responseCode = (int)response.StatusCode;

            if (responseCode == 200)
            {
                string jsonResponse;

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    try
                    {
                        jsonResponse = reader.ReadToEnd();

                        JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);
                        JsonElement root = jsonDocument.RootElement;

                        JsonElement kakao_account = root.GetProperty("kakao_account");
                        JsonElement profile = kakao_account.GetProperty("profile");

                        result.UserNickName = profile.GetProperty("nickname").GetString();
                        result.UserEmail = kakao_account.GetProperty("email").GetString();
                        
                        return result;
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("Kakao 유저 정보가 일부 누락되었습니다.");
                    }
                }
            }
            else
            {
                throw new Exception("Kakao 유저 정보를 불러올 수 없습니다.");
            }
        }
    }

    public async Task<SingleSignOnUserInfo> GetGoogleUserInfo(string googleAccessToken)
    {
        string postURL = "https://www.googleapis.com/oauth2/v1/userinfo";
        //string postURL = $"https://www.googleapis.com/drive/v2/files?access_token={googleAccessToken}";

        // HTTP 요청 생성
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postURL);
        request.Method = "GET";
        request.Headers["Authorization"] = "Bearer " + googleAccessToken;

        try
        {
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                int responseCode = (int)response.StatusCode;

                if (responseCode == 200)
                {
                    string jsonResponse;

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        try
                        {
                            jsonResponse = reader.ReadToEnd();

                            JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);
                            JsonElement root = jsonDocument.RootElement;

                            string nickname = root.GetProperty("name").GetString();
                            string email = root.GetProperty("email").GetString();

                            return new SingleSignOnUserInfo() { UserNickName = nickname, UserEmail = email };
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Google 유저 정보가 일부 누락되었습니다.");
                        }
                    }
                }
                else
                {
                    throw new Exception("Google 유저 정보를 불러올 수 없습니다.");
                }
            }
        }
        catch(Exception ex)
        {
            throw new Exception("Google 유저 정보를 불러올 수 없습니다.");
        }
    }

    public async Task<SingleSignOnUserInfo> GetGoogleUserInfoToIdToken(string googleIdToken)
    {
        var result = new SingleSignOnUserInfo() { PlatformCode = CodePlatform.GOOGLE };
        string googleClientId = _configuration.GetConnectionString("google-client-id");

        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new[] { googleClientId } // OAuth 클라이언트 ID
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(googleIdToken, settings);

        string userName = payload.Name;
        string userEmail = payload.Email;

        result.UserNickName = userName;
        result.UserEmail = userEmail;

        return result;
    }
}
