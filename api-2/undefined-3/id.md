# 프로필 조회 페이지 (ID)

## 요청

<mark style="color:blue;">`GET`</mark> `/api/profile/{userId}`

유저 아이디에 해당하는 프로필 페이지 정보를 조회합니다.

피드 정보를 제외한 프로필 페이지 전체의 정보를 반환합니다.

피드 정보는 [undefined-2.md](undefined-2.md "mention")에서 가져옵니다.



**Path Parameters**

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

