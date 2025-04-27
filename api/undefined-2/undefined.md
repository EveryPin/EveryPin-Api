---
description: 게시글 상세 화면 > 좋아요 토글
---

# ❌ 좋아요 추가

## 요청

<mark style="color:green;">`POST`</mark> `/api/post/{postId}/like`

메인 화면 내 유저의 지도에 보일 핀(게시글)을 조회합니다.



**쿼리 파라미터**

| Name     | Type   | Description    |
| -------- | ------ | -------------- |
| `postId` | number | 좋아요 추가할 게시글 번호 |



## 요청 예시

```bash
curl -X 'p o  'https://everypin-api.azurewebsites.net/api/post/{x}/{y}/{range}' \
  -H 'accept: */*'
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
