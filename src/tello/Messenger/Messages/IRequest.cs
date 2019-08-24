﻿using System;

namespace Messenger
{
    public interface IRequest: IEnvelope
    {
        /// <summary>
        /// some requests take longer than others, so this makes it possible to set per-request timeout
        /// </summary>
        TimeSpan Timeout { get; }

        /// <summary>
        /// Gets if the request has a response.
        /// </summary>
        bool NoWait { get; }
    }

    public interface IRequest<T> : IRequest, IEnvelope<T>
    {
    }
}
