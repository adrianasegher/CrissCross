// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace CrissCross.WPF.UI.Controls.Decoding;

internal sealed class GifImageData
{
    private GifImageData()
    {
    }

    public byte LzwMinimumCodeSize { get; set; }

    public long CompressedDataStartOffset { get; set; }

    internal static async Task<GifImageData> ReadAsync(Stream stream)
    {
        var imgData = new GifImageData();
        await imgData.ReadInternalAsync(stream).ConfigureAwait(false);
        return imgData;
    }

    private async Task ReadInternalAsync(Stream stream)
    {
        LzwMinimumCodeSize = (byte)stream.ReadByte();
        CompressedDataStartOffset = stream.Position;
        await GifHelpers.ConsumeDataBlocksAsync(stream).ConfigureAwait(false);
    }
}
