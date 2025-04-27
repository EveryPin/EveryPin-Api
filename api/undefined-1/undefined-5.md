---
description: 게시글 상세 화면(본인) > 게시글 더보기 메뉴 > 게시글 수정
---

# ❌ 게시글 수정

## 요청

<mark style="color:orange;">`PUT`</mark> `/api/post/{postId}`

게시글을 수정합니다.&#x20;



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
curl -X 'PUT' \
  'https://everypin-api.azurewebsites.net/api/post/{postId}' \
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
```
{% endtab %}

{% tab title="401" %}
```json
```
{% endtab %}
{% endtabs %}
