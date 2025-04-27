---
description: 해당 엔드포인트는 실제 사용되지 않습니다.
---

# 🧪 전체 게시글 조회

## 요청

<mark style="color:blue;">`GET`</mark> `/api/post`

전체 게시글을 조회합니다.





## 요청 예시

```bash
curl -X 'GET' \
  'https://everypin-api.azurewebsites.net/api/post' \
  -H 'accept: text/plain'
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

