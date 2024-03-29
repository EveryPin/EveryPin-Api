﻿using Contracts.Repository.Models;
using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class LikeRepository : RepositoryBase<Like>, ILikeRepository
    {
        public LikeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Like> GetAllLike(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.LikeId)
            .ToList();

        public IEnumerable<Like> GetLikeToPostId(int postId, bool trackChange) =>
            FindByCondition(like => like.PostId.Equals(postId), trackChange)
            .ToList();

        public int GetLikeCountToPostId(int postId, bool trackChange) =>
            FindByCondition(like => like.PostId.Equals(postId), trackChange)
            .ToList().Count;

        public void CreateLike(Like like) =>
            Create(like);
    }
}
