// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.HttpOverrides;
using AspNetIPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;
using IPAddress = System.Net.IPAddress;
using IPNetwork = System.Net.IPNetwork;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Options for <see cref="ForwardedHeadersMiddleware"/>
/// </summary>
public class ForwardedHeadersOptions
{
    /// <summary>
    /// Gets or sets the header used to retrieve the originating client IP. Defaults to the value specified by
    /// <see cref="ForwardedHeadersDefaults.XForwardedForHeaderName"/>.
    /// </summary>
    public string ForwardedForHeaderName { get; set; } = ForwardedHeadersDefaults.XForwardedForHeaderName;

    /// <summary>
    /// Gets or sets the header used to retrieve the original value of the Host header field.
    /// Defaults to the value specified by <see cref="ForwardedHeadersDefaults.XForwardedHostHeaderName"/>
    /// </summary>
    public string ForwardedHostHeaderName { get; set; } = ForwardedHeadersDefaults.XForwardedHostHeaderName;

    /// <summary>
    /// Gets or sets the header used to retrieve the value for the originating scheme (HTTP/HTTPS).
    /// Defaults to the value specified by <see cref="ForwardedHeadersDefaults.XForwardedProtoHeaderName"/>
    /// </summary>
    public string ForwardedProtoHeaderName { get; set; } = ForwardedHeadersDefaults.XForwardedProtoHeaderName;

    /// <summary>
    /// Gets or sets the header used to retrieve the value for the path base.
    /// Defaults to the value specified by <see cref="ForwardedHeadersDefaults.XForwardedPrefixHeaderName"/>
    /// </summary>
    public string ForwardedPrefixHeaderName { get; set; } = ForwardedHeadersDefaults.XForwardedPrefixHeaderName;

    /// <summary>
    /// Gets or sets the header used to store the original value of client IP before applying forwarded headers.
    /// Defaults to the value specified by <see cref="ForwardedHeadersDefaults.XOriginalForHeaderName"/>
    /// </summary>
    /// <seealso cref="ForwardedHeadersDefaults"/>
    public string OriginalForHeaderName { get; set; } = ForwardedHeadersDefaults.XOriginalForHeaderName;

    /// <summary>
    /// Gets or sets the header used to store the original value of the Host header field before applying forwarded headers.
    /// Defaults to the value specified by <see cref="ForwardedHeadersDefaults.XOriginalHostHeaderName"/>
    /// </summary>
    /// <seealso cref="ForwardedHeadersDefaults"/>
    public string OriginalHostHeaderName { get; set; } = ForwardedHeadersDefaults.XOriginalHostHeaderName;

    /// <summary>
    /// Gets or sets the header used to store the original scheme (HTTP/HTTPS) before applying forwarded headers.
    /// Defaults to the value specified by <see cref="ForwardedHeadersDefaults.XOriginalProtoHeaderName"/>
    /// </summary>
    /// <seealso cref="ForwardedHeadersDefaults"/>
    public string OriginalProtoHeaderName { get; set; } = ForwardedHeadersDefaults.XOriginalProtoHeaderName;

    /// <summary>
    /// Gets or sets the header used to store the original path base before applying forwarded headers.
    /// Defaults to the value specified by <see cref="ForwardedHeadersDefaults.XOriginalPrefixHeaderName"/>
    /// </summary>
    /// <seealso cref="ForwardedHeadersDefaults"/>
    public string OriginalPrefixHeaderName { get; set; } = ForwardedHeadersDefaults.XOriginalPrefixHeaderName;

    /// <summary>
    /// Identifies which forwarders should be processed.
    /// </summary>
    public ForwardedHeaders ForwardedHeaders { get; set; }

    /// <summary>
    /// Limits the number of entries in the headers that will be processed. The default value is 1.
    /// Set to null to disable the limit, but this should only be done if
    /// KnownProxies or KnownNetworks are configured.
    /// </summary>
    public int? ForwardLimit { get; set; } = 1;

    /// <summary>
    /// Addresses of known proxies to accept forwarded headers from.
    /// </summary>
    public IList<IPAddress> KnownProxies { get; } = new List<IPAddress>() { IPAddress.IPv6Loopback };

    /// <summary>
    /// Address ranges of known proxies to accept forwarded headers from.
    /// Obsolete, please use <see cref="KnownIPNetworks"/> instead
    /// </summary>
    [Obsolete("Please use KnownIPNetworks instead. For more information, visit https://aka.ms/aspnet/deprecate/005.", DiagnosticId = "ASPDEPR005")]
    public IList<AspNetIPNetwork> KnownNetworks { get; } = new List<AspNetIPNetwork>() { new(IPAddress.Loopback, 8) };

    /// <summary>
    /// Address ranges of known proxies to accept forwarded headers from.
    /// </summary>
    public IList<IPNetwork> KnownIPNetworks { get; } = new List<IPNetwork>() { new(IPAddress.Loopback, 8) };

    /// <summary>
    /// The allowed values from x-forwarded-host. If the list is empty then all hosts are allowed.
    /// Failing to restrict this these values may allow an attacker to spoof links generated by your service.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item><description>Port numbers must be excluded.</description></item>
    /// <item><description>A top level wildcard "*" allows all non-empty hosts.</description></item>
    /// <item><description>Subdomain wildcards are permitted. E.g. "*.example.com" matches subdomains like foo.example.com,
    ///    but not the parent domain example.com.</description></item>
    /// <item><description>Unicode host names are allowed but will be converted to punycode for matching.</description></item>
    /// <item><description>IPv6 addresses must include their bounding brackets and be in their normalized form.</description></item>
    /// </list>
    /// </remarks>
    public IList<string> AllowedHosts { get; set; } = new List<string>();

    /// <summary>
    /// Require the number of header values to be in sync between the different headers being processed.
    /// The default is 'false'.
    /// </summary>
    public bool RequireHeaderSymmetry { get; set; }
}
