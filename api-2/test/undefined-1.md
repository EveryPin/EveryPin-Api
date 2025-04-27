# 액세스 토큰 재발급

## 요청

<mark style="color:green;">`POST`</mark> `/api/auth/refresh`

액세스 토큰을 재발급합니다.



**Request Body**

| Name           | Type   | Description |
| -------------- | ------ | ----------- |
| `accessToken`  | string | 액세스 토큰      |
| `refreshToken` | string | 리프레쉬 토큰     |



## 응답 예시

{% tabs %}
{% tab title="200" %}
```json
{
  "accessToken": "{accessToken}",
  "refreshToken": "{refreshToken}"
}
```
{% endtab %}

{% tab title="500" %}
```json
{
  "StatusCode": 500,
  "Message": "{Error Message}"
}
```
{% endtab %}
{% endtabs %}

