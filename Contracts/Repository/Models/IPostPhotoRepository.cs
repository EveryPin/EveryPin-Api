﻿using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repository.Models
{
    public interface IPostPhotoRepository
    {
        IEnumerable<PostPhoto> GetAllPostPhoto(bool trackChanges);
        IEnumerable<PostPhoto> GetPostPhotoToPostId(int postId, bool trackChange);
        void CreatePostPhoto(PostPhoto postphoto);
    }
}
