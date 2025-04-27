# EveryPin API

EveryPin는 지도기반의 SNS로,  사용자가 지도 위에 위치 정보를 입력하여 게시글을 작성하고, 다양한 사용자들과 소통할 수 있는 **지도 기반 SNS**입니다.\
사용자는 게시글 작성 시 위치를 선택하거나 현재 위치를 기반으로 글을 남길 수 있으며, 작성된 게시글은 피드(Feed) 화면에서 **지도 위 핀(Pin)** 형태로 표시됩니다.\
사용자는 지도에서 핀을 눌러 다른 사람들의 게시글을 조회하거나, 지도를 기반으로 게시글을 탐색할 수 있습니다.\


**주요 기능**

* SSO 기반회원가입 및 로그인 (JWT 기반 인증)
* 사용자 프로필 관리
* 위치 기반 게시글 작성 및 주변 게시글 조회
* 게시글 좋아요, 댓글 작성 기능
* FCM을 통한 실시간 알림
* Azure 기반의 클라우드 서비스 사용
* GitHub Actions을 통한 CI/CD 연동



**기술 스택**

* **Backend**: .NET 8, ASP.NET Core Web API (C#)
* **Database**: Azure SQL Database (SQL Server), EF Core
* **Cloud/Storage**: Azure App Service, Azure Blob Storage, Firebase Cloud Messaging
* **DevOps**: GitHub Actions
