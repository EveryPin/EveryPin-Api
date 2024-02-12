﻿using Service.Contracts;
using Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace EveryPinApi.Presentation.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IServiceManager _service;

        public CommentController(ILogger<CommentController> logger, IServiceManager service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet(Name = "GetComment")]
        [Authorize(Roles ="NormalUser")]
        public IActionResult GetAllComment()
        {
            var comments = _service.CommentService.GetAllComment(trackChanges: false);
            return Ok(comments);
        }

        [HttpGet("{postId:int}")]
        public IActionResult GetCommentToPostId(int postId)
        {
            var comments = _service.CommentService.GetCommentToPostId(postId, trackChanges: false);

            return Ok(comments);
        }
    }
}
