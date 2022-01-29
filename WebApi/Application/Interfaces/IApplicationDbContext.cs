﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Game> Games { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Platform> Platforms { get; set; }
        DbSet<Download> Downloads { get; set; }
        Task<int> SaveChangesAsync();
    }
}
