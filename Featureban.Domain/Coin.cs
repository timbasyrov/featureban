﻿using System;
using Featureban.Domain.Common;
using Featureban.Domain.Enums;

namespace Featureban.Domain
{
    public class Coin : ValueObject<Coin>
    {
        private readonly Random _random = new Random();

        public CoinFlipResult Flip() => (CoinFlipResult)_random.Next(0, 2);

        protected override bool EqualsCore(Coin other) => Equals(other);

        protected override int GetHashCodeCore() => GetHashCode();
    }
}
