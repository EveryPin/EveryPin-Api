using System;

namespace Shared.Dtos.Follow.Requests;

public class CreateFollowRequest
{
    public string FollowerId { get; set; } = string.Empty;  // 팔로우 요청을 하는 사용자 ID
    public string FollowingId { get; set; } = string.Empty;  // 팔로우 대상 사용자 ID

    // 주의: 여기서는 모든 필수 속성이 초기화되지 않는 부분 데이터만 생성합니다.
    // 이 메서드로 생성된 객체는 직접 DB에 저장할 수 없으며,
    // Repository 계층에서 Follower, Following 엔티티 참조를 포함한 완전한 객체로 변환되어야 합니다.
    public FollowData ToFollowData()
    {
        return new FollowData
        {
            FollowerId = FollowerId,
            FollowingId = FollowingId,
            CreatedDate = DateTime.Now
        };
    }
}

// Follow 엔티티의 데이터만 담는 DTO
public class FollowData
{
    public string FollowerId { get; set; } = string.Empty;
    public string FollowingId { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}