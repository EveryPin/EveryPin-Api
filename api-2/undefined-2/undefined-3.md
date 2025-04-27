---
description: (로그인 유저의) 게시글 상세 화면 > 삭제
---

# 게시글 삭제

## 요청

<mark style="color:red;">`DELETE`</mark> `/api/post/{postId}`

게시글을 삭제합니다.

게시글 작성자의 액세스 토큰과 헤더의 액세스 토큰이 일치할 경우에 삭제할 수 있습니다.



**Headers**

| Name          | Value                |
| ------------- | -------------------- |
| Authorization | Bearer {accessToken} |



**Path Parameters**

| Name   | Type    | Description |
| ------ | ------- | ----------- |
| postId | integer | 게시글 번호      |





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
