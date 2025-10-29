﻿using EhlaklyShokran.Domain.Common;
using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Domain.Identity
{
    public sealed class RefreshToken : AuditableEntity
    {
        public string? Token { get; }
        public string? UserId { get; }
        public DateTimeOffset ExpiresOnUtc { get; }

        private RefreshToken()
        { }

        private RefreshToken(Guid id, string? token, string? userId, DateTimeOffset expiresOnUtc)
       : base(id)
        {
            Token = token;
            UserId = userId;
            ExpiresOnUtc = expiresOnUtc;
        }
        public static Result<RefreshToken> Create(Guid id, string? token, string? userId, DateTimeOffset expiresOnUtc)
        {
            if (id == Guid.Empty)
            {
                return RefreshTokenErrors.IdRequired;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return RefreshTokenErrors.TokenRequired;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RefreshTokenErrors.UserIdRequired;
            }

            if (expiresOnUtc <= DateTimeOffset.UtcNow)
            {
                return RefreshTokenErrors.ExpiryInvalid;
            }

            return new RefreshToken(id, token, userId, expiresOnUtc);
        }
    }
}
