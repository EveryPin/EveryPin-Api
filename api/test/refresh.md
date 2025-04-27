# 🟢 액세스 토큰 Refresh

## 요청

<mark style="color:green;">`POST`</mark> `/api/token/refresh`

만료된 액세스 토큰을 재발급합니다.



**헤더**

| Name         | Value              |
| ------------ | ------------------ |
| Content-Type | `application/json` |



**본문**

| Name           | Type   | Description |
| -------------- | ------ | ----------- |
| `accessToken`  | string | 액세스 토큰      |
| `refreshToken` | string | 리프레쉬 토큰     |





## 요청 예시

```bash
curl -X 'POST' \
  'https://everypin-api.azurewebsites.net/api/token/refresh' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "accessToken": "{accessToken}",
  "refreshToken": "{refreshToken}"
}'
```





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

