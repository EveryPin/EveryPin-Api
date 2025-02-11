using Entites.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class RepositoryContext : IdentityDbContext<User>
{
    public RepositoryContext(DbContextOptions options): base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 더미 데이터 생성
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new CodeOAuthPlatformConfiguration());


        // Post 삭제 시, 자식(Comment, Like, PostPhoto) 자동 삭제되지 않도록 추가
        modelBuilder.Entity<Comment>()
                    .HasOne(c => c.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(c => c.PostSeq)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Like>()
                    .HasOne(c => c.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(c => c.PostSeq)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PostPhoto>()
                    .HasOne(c => c.Post)
                    .WithMany(p => p.PostPhotos)
                    .HasForeignKey(c => c.PostSeq)
                    .OnDelete(DeleteBehavior.NoAction);

        // Follows 엔티티 구성 예시
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.FollowingList)  // 사용자의 팔로우 목록(팔로워로서의 관계)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Cascade); // 원래대로 Cascade 적용 (필요에 따라 변경)

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Following)
            .WithMany(u => u.FollowerList)  // 사용자의 팔로워 목록(팔로잉으로서의 관계)
            .HasForeignKey(f => f.FollowingId)
            .OnDelete(DeleteBehavior.NoAction); // Cascade 대신 NoAction으로 변경하여 순환 문제 회피
    }

    //public DbSet<CodeOAuthPlatform>? CodeOAuthPlatform { get; set; }
    public DbSet<Post>? Posts { get; set; }
    public DbSet<PostPhoto>? PostPhotos { get; set; }
    public DbSet<Like>? Likes { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<Follow>? Follows { get; set; }
    public DbSet<Profile>? Profiles { get; set; }
}