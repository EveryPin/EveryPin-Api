---
description: 해당 엔드포인트는 실제 사용되지 않습니다.
---

# 🧪 전체 댓글 조회

## 요청

<mark style="color:blue;">`GET`</mark> `/post/comment`

전체 댓글 목록을 조회합니다.





## 요청 예시

```bash
curl -X 'GET' \
  'http://localhost:5283/api/comment' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQ4N2VlOTAwLTg4NDUtNDNlMS1hNGIxLWJkYzNiN2ViODE1YiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImlsbHVsaXplckBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi6rmA7Jew7KSRIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiTm9ybWFsVXNlciIsImV4cCI6MTcyNzk1Mzk5NSwiaXNzIjoiRXZlcnlQaW4tQXBJIiwiYXVkIjoiPGh0dHBzOi8vbG9jYWxob3N0OjUyODM-In0.Mwi3AsPZD95a62Dw_7AthXRZpVGP_xiXuVAZh3EIEi0'
```





## 응답 예시

{% tabs %}
{% tab title="200" %}
```json
[
  {
    "commentId": 1,
    "postId": 4,
    "userId": "487ee900-8845-43e1-a4b1-bdc3b7eb815b",
    "commentMessage": "test",
    "createdDate": "2024-10-03T20:10:02.6249844"
  }
]
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
