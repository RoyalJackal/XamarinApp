﻿using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Pet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Breed { get; set; }

        public List<Feed> Feeds { get; set; }
    }
}