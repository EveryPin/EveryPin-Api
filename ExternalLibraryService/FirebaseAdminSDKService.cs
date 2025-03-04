using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLibraryService;
public class FirebaseAdminSDKService
{
    // Firebase Admin SDK를 사용하여 FCM 메시지를 보내는 서비스
    // 참고: https://firebase.google.com/docs/cloud-messaging/send-message?hl=ko

    public FirebaseAdminSDKService(GoogleCredential credential)
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential
            });
        }
    }

    public async Task<string> SendMessageToUser(string userFcmToken, string title, string message)
    {
        var messages = new Message()
        {
            Token = userFcmToken,
            Notification = new Notification()
            {
                Title = title,
                Body = message
            },
        };

        var response = await FirebaseMessaging.DefaultInstance.SendAsync(messages);

        return response;
    }

    public async Task<IReadOnlyList<SendResponse>> SendMessageToManyUser(List<string> userFcmTokens, string title, string message)
    {
        var messages = new MulticastMessage()
        {
            Tokens = userFcmTokens,
            Notification = new Notification()
            {
                Title = title,
                Body = message
            },
        };

        var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(messages);

        if (response.FailureCount > 0)
        {
            var failedTokens = new List<string>();
            for (var i = 0; i < response.Responses.Count; i++)
            {
                if (!response.Responses[i].IsSuccess)
                {
                    // The order of responses corresponds to the order of the registration tokens.
                    failedTokens.Add(userFcmTokens[i]);
                }
            }

            string error = $"[FCM-SendMessageToManyUser] List of tokens that caused failures: {failedTokens}";
        }

        //bool isSuccess = userFcmTokens.Count - response.SuccessCount == 0 ;

        return response.Responses;
    }

    public async Task<IReadOnlyList<SendResponse>> SendManyMessage(List<Message> messages)
    {
        var response = await FirebaseMessaging.DefaultInstance.SendEachAsync(messages);

        return response.Responses;
    }
}
