﻿using System;

namespace Mihaylov.Common.Host.Abstract.Authorization
{
    [Flags]
    public enum ClaimType : int
    {
        Username = 1,

        Email = 2,
        
        FullName = 4,
        
        FirstName = 8,
        
        LastName = 16,
    }
}