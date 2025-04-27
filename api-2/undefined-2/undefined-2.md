---
description: (로그인 유저의) 게시글 상세 화면 > 수정 > 수정 완료
---

# 게시글 수정

## 요청

<mark style="color:orange;">`PUT`</mark> `/api/post/{postId}`

게시글을 수정합니다.

게시글작성자의 액세스 토큰과 헤더의 액세스 토큰이 일치할 경우에 수정할 수 있습니다.



**Headers**

| Name          | Value                |
| ------------- | -------------------- |
| Content-Type  | multipart/form-data  |
| Authorization | Bearer {accessToken} |



**Path Parameters**

| Name   | Type    | Description |
| ------ | ------- | ----------- |
| postId | integer | 게시글 번호      |



**Request Body (Form Data)**

| Name        | Type   | Description |
| ----------- | ------ | ----------- |
| postContent | string | 게시글 내용      |
| x           | number | 좌표 X        |
| y           | number | 좌표 Y        |
| photoFiles  | array  | 게시글 사진 리스트  |





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
