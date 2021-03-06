﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Services
{
    public interface IPointsService
    {
        Task AddPointsForMatchAsync(Points points);
        Task EditPointsForMatchAsync(ICollection<Points> points);
        Task DeletePointsForMatchAsync(int matchId);
    }
}
