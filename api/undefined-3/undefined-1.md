---
description: 게시글 상세 보기
---

# 🟠 댓글 조회

## 요청

<mark style="color:blue;">`GET`</mark> `/post/{postId}/comment`

게시글 번호에 해당하는 상세 내용을 조회합니다.

게시글 상세 화면에서 댓글은 별도의 API로 불러옵니다.



**쿼리 파라미터**

| Name     | Type    | Description |
| -------- | ------- | ----------- |
| `postID` | integer | 게시글 번호      |





## 요청 예시

```bash
curl -X 'GET' \
  'https://everypin-api.azurewebsites.net/api/post/{postId}/comment' \
  -H 'accept: text/plain'
```





## 응답 예시

{% tabs %}
{% tab title="200" %}
```json
[
  {
    "commentId": 0,
    "postId": 0,
    "userName": "김연중",
    "commentMessage": "댓글 입력",
    "createdDate": "2024-08-26T12:40:52.797Z"
  },
  ...
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
