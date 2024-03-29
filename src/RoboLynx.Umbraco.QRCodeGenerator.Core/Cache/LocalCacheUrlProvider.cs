﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class LocalCacheUrlProvider : IQRCodeCacheUrlProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Uri _baseUrl;
        private readonly bool _absoluteModeIsDefault;

        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="baseUrl">Relative or absolute URL to location where resources are stored</param>
        /// <param name="absoluteModeIsDefault">If <c>true</c> then urlMode set to UrlMode.Auto or UrlMode. Default give a absolute URL as result</param>
        /// <exception cref="ArgumentNullException"></exception>
        public LocalCacheUrlProvider(IHttpContextAccessor httpContextAccessor, Uri baseUrl, bool absoluteModeIsDefault)
        {
            if (baseUrl is null)
            {
                throw new ArgumentNullException(nameof(baseUrl), $"'{nameof(baseUrl)}' cannot be null or empty.");
            }
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = baseUrl;
            _absoluteModeIsDefault = absoluteModeIsDefault;
        }

        public LocalCacheUrlProvider(IHttpContextAccessor httpContextAccessor, Uri baseUrl): this(httpContextAccessor, baseUrl, baseUrl.IsAbsoluteUri)
        {
            
        }

        public LocalCacheUrlProvider(IHttpContextAccessor httpContextAccessor, string baseUrl) : this(httpContextAccessor, new Uri(baseUrl, UriKind.RelativeOrAbsolute))
        {

        }

        public virtual string Url(string path, UrlMode urlMode)
        {
            Uri url = new(_baseUrl.ToString().EnsureEndsWith("/") + path.TrimStart("/"), UriKind.RelativeOrAbsolute);

            if (!url.IsAbsoluteUri)
            {
                if (_httpContextAccessor.HttpContext == null)
                {
                    throw new NullReferenceException("HttpContext not found.");
                }
                var requestUrl = new Uri(_httpContextAccessor.HttpContext.Request.GetEncodedUrl());

                if (requestUrl is not null && requestUrl.IsAbsoluteUri)
                {
                    url = new Uri(requestUrl, url);
                }
                else
                {
                    throw new InvalidOperationException("Could not build valid absolute URL.");
                }
            }

            if (urlMode == UrlMode.Relative || (!_absoluteModeIsDefault && (urlMode == UrlMode.Auto || urlMode == UrlMode.Default)))
            {
                return url.PathAndQuery;
            }

            return url.AbsoluteUri;
        }
    }
}