---
description: 메인 화면 > 메뉴 > 글쓰기
---

# 게시글 작성

## 요청

<mark style="color:green;">`POST`</mark> `/api/post`

게시글을 작성합니다.

헤더의 액세스 토큰에 해당하는 유저의 계정으로 작성됩니다.



**Headers**

| Name          | Value                |
| ------------- | -------------------- |
| Content-Type  | multipart/form-data  |
| Authorization | Bearer {accessToken} |



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
{
  "postId": 5,
  "postContent": "테스트",
  "x": 120,
  "y": 120,
  "updateDate": null,
  "createdDate": "2024-08-06T17:36:15.7341504+00:00"
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
