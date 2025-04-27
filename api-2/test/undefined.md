---
description: 로그인 화면
---

# 로그인/회원가입

## 요청

<mark style="color:green;">`POST`</mark> `/api/auth/login`

플랫폼을 통한 로그인을 진행합니다. (Google, Kakao)

신규 회원의 경우 인증 후, 자동으로 회원가입 후 메인 페이지로 연결됩니다.\
기존 회원의 경우 인증 후, 바로 메인 페이지로 연결됩니다.

FCM 토큰을 DB 내 유저 테이블에 저장합니다. 서버의 API에서 알림을 전송할 때, 저장된 FCM 토큰을 통해 전송합니다.



**Request Body**

| Name         | Type   | Description      |
| ------------ | ------ | ---------------- |
| platformCode | string | 로그인 SSO 플랫폼 Code |
| accessToken  | string | 액세스 토큰           |
| fcmToken     | string | FCM 토큰           |



**참고💡- platformCode**

| Name     | Type   | Description |
| -------- | ------ | ----------- |
| `google` | string | Google 로그인  |
| `kakao`  | string | Kakao 로그인   |





## 응답 예시

{% tabs %}
{% tab title="200" %}
```json
{
  "accessToken": "string",
  "refreshToken": "string"
}
```
{% endtab %}

{% tab title="401" %}
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.2",
  "title": "Unauthorized",
  "status": 401,
  "traceId": "00-000000000000000000000-000000000000000-00"
}
```
{% endtab %}
{% endtabs %}
