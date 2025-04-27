---
description: 메인 화면 > 지도 > 핀 클릭 > 게시글 상세 화면 (댓글 영역)
---

# 댓글 조회 (페이징)

## 요청

<mark style="color:blue;">`GET`</mark> `/api/post/{postId}/comment?page={page}&size={size}`

게시글 번호에 해당하는 댓글을 페이징하여 가져옵니다.



**Path Parameters**

| Name   | Type    | Description |
| ------ | ------- | ----------- |
| postId | integer | 게시글 번호      |



**Query Parameters**

| Name | Type    | Description      |
| ---- | ------- | ---------------- |
| page | integer | 호출할 페이지 번호       |
| size | integer | 한 페이지 당 가져올 댓글 수 |





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

