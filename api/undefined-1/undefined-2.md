---
description: 특정 유저 프로필 화면 > 핀 확인 > 메인 화면 이동 > 지도 내 해당 유저 핀 표시
---

# ❌ 유저 게시글 조회

## 요청

<mark style="color:blue;">`GET`</mark> `/api/users/{userId}/post`

특정 유저가 작성한 핀(게시글)을 조회합니다.



**헤더**

| Name          | Value                 |
| ------------- | --------------------- |
| Content-Type  | `multipart/form-data` |
| Authorization | Bearer {accessToken}  |



**쿼리 파라미터**

| Name     | Type | Description |
| -------- | ---- | ----------- |
| `postId` | int  | 게시글 번호      |





## 요청 예시

```bash
curl -X 'GET' \
  'https://everypin-api.azurewebsites.net/api/post/4' \
  -H 'accept: text/plain'
```





## 응답 예시

{% tabs %}
{% tab title="200" %}
```json
[
  {
    "postId": 4,
    "postContent": "테스트22",
    "x": 123,
    "y": 123,
    "postPhotos": [
      {
        "postPhotoId": 3,
        "photoUrl": "https://everypinimg.blob.core.windows.net/everypin-image/PostPhoto_3"
      },
      // ...
    ],
    "updateDate": null,
    "createdDate": "2024-07-31T02:16:10.0831085"
  },
  // ...
]
```
{% endtab %}

{% tab title="401" %}
Code: 401

Error: Unauthorized
{% endtab %}

{% tab title="404" %}
```json
{
  "StatusCode": 404,
  "Message": "Post ID [{postId}]에 해당하는 값이 데이터베이스에 존재하지 않습니다."
}
```
{% endtab %}
{% endtabs %}
