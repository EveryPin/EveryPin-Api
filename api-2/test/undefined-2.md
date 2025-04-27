---
description: 개인 프로필 화면 > 설정 > 로그아웃
---

# 로그아웃

## 요청

<mark style="color:green;">`POST`</mark> `/api/auth/logout`

로그아웃 처리를 통해 헤더의 액세스 토큰을 폐기합니다.

DB 내 유저 테이블에 저장된 FCM 토큰을 삭제합니다.



**Headers**

| Name          | Value                |
| ------------- | -------------------- |
| Authorization | Bearer {accessToken} |



## 응답 예시

{% tabs %}
{% tab title="200" %}
```json
```
{% endtab %}
{% endtabs %}
