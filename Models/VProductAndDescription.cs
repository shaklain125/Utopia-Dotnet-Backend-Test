﻿using System;
using System.Collections.Generic;

namespace UtopiaBackendChallenge.Models
{
    public partial class VProductAndDescription
    {
        public int ProductId { get; set; }
        public string Culture { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
