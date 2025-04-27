---
description: 메인 화면 > 지도 > 핀 클릭 > 게시글 상세 화면 (댓글 영역)
---

# 댓글 수정

## 요청

<mark style="color:orange;">`PUT`</mark> `/api/post/{postId}/comment/{commentId}`

댓글을 수정합니다.

댓글 작성자와 헤더의 액세스 토큰의 유저 정보가 일치할 경우 수정할 수 있습니다.



**Headers**

| Name          | Value                |
| ------------- | -------------------- |
| Authorization | Bearer {accessToken} |



**Path Parameters**

| Name      | Type    | Description |
| --------- | ------- | ----------- |
| postId    | integer | 게시글 번호      |
| commentId | integer | 댓글 번호       |



**Request Body**

| Name           | Type   | Description |
| -------------- | ------ | ----------- |
| commentMessage | string | 댓글 내용       |





## 응답 예시

{% tabs %}
{% tab title="201" %}
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
