using MediatR;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Commands.AddBoxscoreByRoundNo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZiggyCreatures.Caching.Fusion;

namespace OpenData.Basketball.AbaLeague.Application.Behaviourses
{
    public class CacheInvalidationBehavior<TRequest, TResponse> (IFusionCache _fusionCache) : 
                    IPipelineBehavior<TRequest, TResponse> where 
                    TRequest : ICacheInvalidatorMediatrQuery<AddBoxscoreByRoundNoCommand>
    {


        public async Task<TResponse> Handle(TRequest request, 
                                            RequestHandlerDelegate<TResponse> next, 
                                            CancellationToken cancellationToken)
        {
            // run through the request handler pipeline for this request
            var result = await next();

            await _fusionCache.RemoveAsync(request.CacheKey, token: cancellationToken);

            return result;
        }
    }
}
