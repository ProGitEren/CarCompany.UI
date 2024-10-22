﻿using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Handlers
{
    public class RetryHandler : DelegatingHandler
    {

        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy =
            Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .RetryAsync(3);

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        
        {
            var policyResult = await _retryPolicy.ExecuteAndCaptureAsync(
                () => base.SendAsync(request, cancellationToken)
                );
            if (policyResult.Outcome == OutcomeType.Failure)
            {
                throw new HttpRequestException("Something went wrong", policyResult.FinalException);
            }

            return policyResult.Result;

        
        }
            
        
    }
}