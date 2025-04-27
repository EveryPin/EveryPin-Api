---
description: 메인 화면 > 지도 > 핀 클릭 > 게시글 상세 화면 (댓글 영역)
---

# 댓글 작성

## 요청

<mark style="color:green;">`POST`</mark> `/api/post/{postId}/comment`

댓글을 작성합니다.

헤더의 액세스 토큰에 해당하는 유저의 계정으로 작성됩니다.



**Headers**

| Name          | Value                |
| ------------- | -------------------- |
| Authorization | Bearer {accessToken} |



**Path Parameters**

| Name   | Type    | Description |
| ------ | ------- | ----------- |
| postId | integer | 게시글 번호      |



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
