using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NzbDrone.Common.Cache;

namespace NzbDrone.Common.UniqueIdentifier
{
    public interface IUniqueIdentifierGenerator
    {
        int Get(string key);
    }

    public class UniqueIdentifierGenerator : IUniqueIdentifierGenerator
    {
        private static int _lastIdentifier;

        private ICached<int> _cache;

        public UniqueIdentifierGenerator(ICacheManager cacheManager)
        {
            _cache = cacheManager.GetCache<int>(typeof(UniqueIdentifierGenerator));
        }

        public int Get(string key)
        {
            return _cache.Get(key, GetNextIdentifier, TimeSpan.FromHours(24));
        }

        private int GetNextIdentifier()
        {
            var nextId = Interlocked.Increment(ref _lastIdentifier);

            if (nextId <= 0)
            {
                // Wrap around to 1.
                var wrapId = nextId;
                while (wrapId <= 0 && wrapId != Interlocked.CompareExchange(ref _lastIdentifier, (int)(wrapId + 0x80000001), wrapId))
                {
                    wrapId = Thread.VolatileRead(ref _lastIdentifier);
                }

                nextId = (int)(nextId + 0x80000001);
            }

            return nextId;
        }
    }
}
