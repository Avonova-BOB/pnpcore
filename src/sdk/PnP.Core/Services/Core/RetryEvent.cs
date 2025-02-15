﻿using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PnP.Core.Services
{
    /// <summary>
    /// Event containing information about the happened retry
    /// </summary>
    public sealed class RetryEvent
    {
        /// <summary>
        /// Retry event constructor
        /// </summary>
        /// <param name="requestMessage">Request that's retried</param>
        /// <param name="httpStatusCode">Http status code</param>
        /// <param name="waitTime">Wait before the next try in seconds</param>
        /// <param name="exception">Socket exception that triggered the retry</param>
        internal RetryEvent(HttpRequestMessage requestMessage, int httpStatusCode, int waitTime, Exception exception)
        {
            Request = requestMessage.RequestUri;
            HttpStatusCode = httpStatusCode;
            WaitTime = waitTime;
            Exception = exception;

            ProcessRequestProperties(requestMessage);
        }

        /// <summary>
        /// Request that was retried
        /// </summary>
        public Uri Request { get; private set; }

        /// <summary>
        /// Http status code for the retried request
        /// </summary>
        public int HttpStatusCode { get; private set; }

        /// <summary>
        /// Wait before the next try in seconds
        /// </summary>
        public int WaitTime { get; private set; }

        /// <summary>
        /// SocketException that triggered the retry
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// PnPContext properties
        /// </summary>
        public IDictionary<string, object> PnpContextProperties { get; internal set; } = new Dictionary<string, object>();


        private void ProcessRequestProperties(HttpRequestMessage requestMessage)
        {
#if NET5_0_OR_GREATER
            if (requestMessage.Options != null)
            {
                foreach(var property in requestMessage.Options)
                {
                    PnpContextProperties[property.Key] = property.Value;
                }
            }
#else
            if (requestMessage.Properties != null && requestMessage.Properties.Count > 0)
            {
                foreach (var property in requestMessage.Properties)
                {
                    PnpContextProperties[property.Key] = property.Value;
                }
            }
#endif
        }
    }
}
