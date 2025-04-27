---
description: 특정 유저 프로필 화면 > 핀 확인 > 메인 화면 이동 > 지도 내 해당 유저 핀 표시
---

# 유저 핀 조회

## 요청

<mark style="color:blue;">`GET`</mark> `/api/map/pin/user/{userId}`

특정 유저가 작성한 핀(게시글)을 조회합니다.



**Headers**

| Name          | Value                |
| ------------- | -------------------- |
| Authorization | Bearer {accessToken} |



&#x20;**Path Parameters**

| Name   | Type   | Description |
| ------ | ------ | ----------- |
| userId | string | 유저 아이디      |





## 응답 예시

{% tabs %}
{% tab title="200" %}
```json
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
