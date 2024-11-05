// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using CrissCross.WPF.UI.Controls.Extensions;

namespace CrissCross.WPF.UI.Controls.Decoding;

// label 0xFF
internal class GifApplicationExtension : GifExtension
{
    internal const int ExtensionLabel = 0xFF;

    private GifApplicationExtension()
    {
    }

    public int BlockSize { get; private set; }

    public string? ApplicationIdentifier { get; private set; }

    public byte[]? AuthenticationCode { get; private set; }

    public byte[]? Data { get; private set; }

    internal override GifBlockKind Kind => GifBlockKind.SpecialPurpose;

    internal static async Task<GifApplicationExtension> ReadAsync(Stream stream)
    {
        var ext = new GifApplicationExtension();
        await ext.ReadInternalAsync(stream).ConfigureAwait(false);
        return ext;
    }

    private async Task ReadInternalAsync(Stream stream)
    {
        // Note: at this point, the label (0xFF) has already been read
        var bytes = new byte[12];
        await stream.ReadAllAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        BlockSize = bytes[0]; // should always be 11
        if (BlockSize != 11)
        {
            throw GifHelpers.InvalidBlockSizeException("Application Extension", 11, BlockSize);
        }

        ApplicationIdentifier = GifHelpers.GetString(bytes, 1, 8);
        var authCode = new byte[3];
        Array.Copy(bytes, 9, authCode, 0, 3);
        AuthenticationCode = authCode;
        Data = await GifHelpers.ReadDataBlocksAsync(stream).ConfigureAwait(false);
    }
}
