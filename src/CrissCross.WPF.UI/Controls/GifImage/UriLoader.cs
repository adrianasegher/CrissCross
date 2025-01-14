﻿// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Packaging;
using System.Net.Http;
using System.Security.Cryptography;
using CrissCross.WPF.UI.Controls.Extensions;

namespace CrissCross.WPF.UI.Controls;

internal static class UriLoader
{
    public static string DownloadCacheLocation { get; set; } = Path.GetTempPath();

    public static Task<Stream> GetStreamFromUriAsync(Uri uri, IProgress<int>? progress)
    {
        if (uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https"))
        {
            return GetNetworkStreamAsync(uri, progress)!;
        }

        return GetStreamFromUriCoreAsync(uri);
    }

    private static async Task<Stream?> GetNetworkStreamAsync(Uri uri, IProgress<int>? progress)
    {
        var cacheFileName = GetCacheFileName(uri);
        var cacheStream = await OpenTempFileStreamAsync(cacheFileName);
        if (cacheStream == null)
        {
            await DownloadToCacheFileAsync(uri, cacheFileName, progress);
            cacheStream = await OpenTempFileStreamAsync(cacheFileName);
        }

        progress?.Report(100);
        return cacheStream;
    }

    private static async Task DownloadToCacheFileAsync(Uri uri, string fileName, IProgress<int>? progress)
    {
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var length = response.Content.Headers.ContentLength ?? 0;
            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = await CreateTempFileStreamAsync(fileName);
            IProgress<long> absoluteProgress = default!;
            if (progress != null)
            {
                absoluteProgress =
                    new Progress<long>(bytesCopied =>
                    {
                        if (length > 0)
                        {
                            progress.Report((int)(100 * bytesCopied / length));
                        }
                        else
                        {
                            progress.Report(-1);
                        }
                    });
            }

            await responseStream.CopyToAsync(fileStream, absoluteProgress);
        }
        catch
        {
            DeleteTempFile(fileName);
            throw;
        }
    }

    private static Task<Stream> GetStreamFromUriCoreAsync(Uri uri)
    {
        if (uri.Scheme == PackUriHelper.UriSchemePack)
        {
            var sri = uri.Authority == "siteoforigin:,,,"
                ? Application.GetRemoteStream(uri)
                : Application.GetResourceStream(uri);

            if (sri != null)
            {
                return Task.FromResult(sri.Stream);
            }

            throw new FileNotFoundException("Cannot find file with the specified URI");
        }

        if (uri.Scheme == Uri.UriSchemeFile)
        {
            return Task.FromResult<Stream>(File.OpenRead(uri.LocalPath));
        }

        throw new NotSupportedException("Only pack:, file:, http: and https: URIs are supported");
    }

    private static Task<Stream?> OpenTempFileStreamAsync(string fileName)
    {
        if (!Directory.Exists(DownloadCacheLocation))
        {
            Directory.CreateDirectory(DownloadCacheLocation);
        }

        var path = Path.Combine(DownloadCacheLocation, fileName);
        Stream? stream = default;
        try
        {
            stream = File.OpenRead(path);
        }
        catch (FileNotFoundException)
        {
        }

        return Task.FromResult(stream);
    }

    private static Task<Stream> CreateTempFileStreamAsync(string fileName)
    {
        var path = Path.Combine(DownloadCacheLocation, fileName);
        Stream stream = File.OpenWrite(path);
        stream.SetLength(0);
        return Task.FromResult(stream);
    }

    private static void DeleteTempFile(string fileName)
    {
        var path = Path.Combine(DownloadCacheLocation, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private static string GetCacheFileName(Uri uri)
    {
        using var sha1 = SHA1.Create();
        var bytes = Encoding.UTF8.GetBytes(uri.AbsoluteUri);
        var hash = sha1.ComputeHash(bytes);
        return ToHex(hash);
    }

    private static string ToHex(byte[] bytes) => bytes.Aggregate(
            new StringBuilder(),
            (sb, b) => sb.Append(b.ToString("X2")),
            sb => sb.ToString());
}
