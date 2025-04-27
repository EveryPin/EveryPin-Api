---
description: 메인 화면 > 지도 > 핀 클릭 > 게시글 상세 화면
---

# 🟢 게시글 상세 조회

## 요청

<mark style="color:blue;">`GET`</mark> `/api/post/{postId}`

게시글 번호에 해당하는 상세 내용을 조회합니다.

게시글 상세 화면에서 댓글은 별도의 API로 불러옵니다.



**쿼리 파라미터**

| Name     | Type    | Description |
| -------- | ------- | ----------- |
| `postId` | integer | 게시글 번호      |





## 요청 예시

```bash
curl -X 'GET' \
  'https://everypin-api.azurewebsites.net/api/post/{postID}' \
  -H 'accept: text/plain'
```





## 응답 예시

{% tabs %}
{% tab title="200" %}
```json
{
  "postId": 5,
  "profileName": "김연중",
  "postContent": "테스트",
  "x": 120,
  "y": 120,
  "postPhotos": [
    {
      "postPhotoId": 6,
      "photoUrl": "https://everypinimg.blob.core.windows.net/everypin-image/PostPhoto_6"
    }
    ...
  ],
  "likeCount": 263
  "updateDate": null,
  "createdDate": "2024-08-06T17:36:15.7341504"
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
