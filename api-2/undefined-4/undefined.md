# 프로필 조회 (본인)

## 요청

<mark style="color:blue;">`GET`</mark> `/api/chat/profile/me`

JWT에 저장된 로그인 유저 본인의 프로필 정보를 조회합니다.



**Headers**

| Name          | Value                |
| ------------- | -------------------- |
| Authorization | Bearer {accessToken} |





## 응답 예시

{% tabs %}
{% tab title="200" %}
```json
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
