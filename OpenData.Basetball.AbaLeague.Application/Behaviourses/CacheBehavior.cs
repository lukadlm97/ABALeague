using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.Configurations;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreDraftByRound;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using ZiggyCreatures.Caching.Fusion;

namespace OpenData.Basketball.AbaLeague.Application.Behaviourses
{
    public class CachingBehavior<TRequest, TResponse>(IFusionCache _fusionCache, IOptions<CacheSettings> _settings, ILogger<TResponse> _logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableMediatrQuery
    {
        private readonly CacheSettings _cacheSettings = _settings.Value;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            {
                TResponse response;
                if (request.BypassCache || request.CacheKey == null) return await next();

                response = await _fusionCache.GetOrDefaultAsync<TResponse>(request.CacheKey, token: cancellationToken);

                if (response == null)
                {
                    response = await next();

                    await _fusionCache.SetAsync(request.CacheKey, response, new FusionCacheEntryOptions
                    {
                        Duration = _cacheSettings.SlidingExpiration,
                    }, token: cancellationToken);
                }
                return response;
            }
        }
    }
}
