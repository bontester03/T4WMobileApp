using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using T4sV1.Model.Auth;
using T4sV1.Services.Security;

namespace T4sV1.Services.Http
{

    public sealed class BearerAuthHandler : DelegatingHandler
    {
        private readonly ITokenStore _store;
        private readonly IAuthService _auth;
        private readonly SemaphoreSlim _gate = new(1, 1);

        public BearerAuthHandler(ITokenStore store, IAuthService auth)
        {
            _store = store;
            _auth = auth;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await AttachHeadersAsync(request);

            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                return response;

            // try refresh once
            var refreshed = await TryRefreshAsync(cancellationToken);
            if (!refreshed)
                return response;

            response.Dispose();
            var retry = request.Clone(); // extension below
            await AttachHeadersAsync(retry);
            return await base.SendAsync(retry, cancellationToken);
        }

        private async Task AttachHeadersAsync(HttpRequestMessage req)
        {
            var at = await _store.GetAccessTokenAsync();
            if (!string.IsNullOrWhiteSpace(at))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", at);

            var deviceId = await _store.GetOrCreateDeviceIdAsync();
            if (!req.Headers.Contains("X-Device-Id"))
                req.Headers.Add("X-Device-Id", deviceId);
        }

        private async Task<bool> TryRefreshAsync(CancellationToken ct)
        {
            await _gate.WaitAsync(ct);
            try { return await _auth.RefreshAsync(ct); }
            finally { _gate.Release(); }
        }
    }

    // Small helper to clone requests
    file static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage Clone(this HttpRequestMessage req)
        {
            var clone = new HttpRequestMessage(req.Method, req.RequestUri)
            {
                Content = req.Content,
                Version = req.Version
            };
            foreach (var h in req.Headers)
                clone.Headers.TryAddWithoutValidation(h.Key, h.Value);
            foreach (var p in req.Properties)
                clone.Properties.Add(p);
            return clone;
        }
    }
}
