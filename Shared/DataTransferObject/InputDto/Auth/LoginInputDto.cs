﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.InputDto.Auth;
public record class LoginInputDto(
    string platformCode,
    string accessToken,
    string fcmToken
);
