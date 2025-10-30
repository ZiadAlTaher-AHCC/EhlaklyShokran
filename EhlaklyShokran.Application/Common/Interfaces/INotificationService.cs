﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailAsync(string to, CancellationToken cancellationToken = default);

        Task SendSmsAsync(string phoneNumber, CancellationToken cancellationToken = default);
    }
}
