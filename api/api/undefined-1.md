# 🟢 로그인 테스트

## 요청

<mark style="color:green;">`POST`</mark> `/api/test/login`

Swagger에서 `email`을 사용하여 로그인을 테스트할 수 있습니다.



**헤더**

| Name   | Value        |
| ------ | ------------ |
| accept | `text/plain` |



**쿼리 파라미터**

| Name        | Type   | Description |
| ----------- | ------ | ----------- |
| userId      | string | 삭제 예정       |
| email       | string | Email       |
| phoneNumber | string | 삭제 예정       |





## 요청 예시

```bash
curl -X 'POST' \
  'http://localhost:5283/api/test/login' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "userId": "string",
  "email": "illulizer@gmail.com",
  "phoneNumber": "string"
}'
```





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
{% endtabs %}
