---
description: 메인 화면 > 지도 > 핀 클릭 > 게시글 상세 화면
---

# 게시글 조회

## 요청

<mark style="color:blue;">`GET`</mark> `/api/post/{postId}`

게시글 번호에 해당하는 상세 내용을 조회합니다.

게시글 상세 화면에서 댓글은 [undefined-6.md](undefined-6.md "mention") API로 불러옵니다.



**Path Parameters**

| Name   | Type    | Description |
| ------ | ------- | ----------- |
| postId | integer | 게시글 번호      |



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
