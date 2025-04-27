---
description: 메인 화면 > 메뉴 > 글쓰기
---

# 🟢 게시글 작성

## 요청

<mark style="color:green;">`POST`</mark> `/api/post`

게시글을 작성합니다.



**헤더**

| Name          | Value                 |
| ------------- | --------------------- |
| Content-Type  | `multipart/form-data` |
| Authorization | Bearer {accessToken}  |



**본문**

| Name        | Type   | Description |
| ----------- | ------ | ----------- |
| PostContent | string | 게시글 내용      |
| X           | number | 좌표 X        |
| Y           | number | 좌표 Y        |
| PhotoFiles  | array  | 게시 사진 배열    |



## 요청 예시

```bash
curl -X 'POST' \
  'https://everypin-api.azurewebsites.net/api/post' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer {accessToken}' \
  -H 'Content-Type: multipart/form-data' \
  -F 'PostContent=테스트' \
  -F 'X=120' \
  -F 'Y=120' \
  -F 'PhotoFiles=@everypin_logo.png;type=image/png'
```





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
