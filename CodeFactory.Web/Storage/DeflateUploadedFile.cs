using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace CodeFactory.Web.Storage
{
    public class DeflateUploadedFile : UploadedFile
    {
        public DeflateUploadedFile()
            : this(Guid.NewGuid())
        {
        }

        public DeflateUploadedFile(Guid id)
            : base(id)
        {
        }

        public override void MarkOld()
        {
            // Requires decompression
            if (base.InputStream != Stream.Null && base.InputStream.Length != base.ContentLength)
            {
                // source for the compressed stream
                Stream source = base.InputStream;

                source.Position = 0;

                // Undeflate wrapper for the compressed stream
                Stream compressedStream = new DeflateStream(source, CompressionMode.Decompress);

                try
                {
                    // Destination for uncompressed stream
                    Stream destination = new MemoryStream();

                    BinaryReader reader = new BinaryReader(compressedStream);
                    BinaryWriter decompressor = new BinaryWriter(destination);

                    try
                    {
                        // This is weird because we have to know the length from the source.
                        decompressor.Write(reader.ReadBytes((int)this.ContentLength));
                        decompressor.Flush();

                        destination.Position = 0;

                        Debug.WriteLine(string.Format("Decompressing: Compressed Length {0}, Original Length {1}",
                            source.Length, destination.Length));
                    }
                    finally
                    {
                        reader.Close();
                    }

                    // Replaces the compressed stream with the uncompressed stream.
                    base.InputStream = destination;
                }
                finally
                {
                    source.Close();
                }
            }

            base.MarkOld();
        }

        protected override void DataInsert()
        {
            // source for the uncompressed stream
            Stream source = base.InputStream;

            if (source != Stream.Null)
            {
                source.Position = 0;

                BinaryReader reader = new BinaryReader(source);

                // Destination for compressed stream and source for the decompressed stream
                Stream destination = new MemoryStream();

                try
                {
                    // Deflate wrapper stream for memory stream
                    Stream compressedStream = new DeflateStream(destination, CompressionMode.Compress);

                    BinaryWriter compressor = new BinaryWriter(compressedStream);

                    compressor.Write(reader.ReadBytes((int)source.Length));
                    compressor.Flush();

                    destination.Position = 0;

                    Debug.WriteLine(string.Format("Compressing: Original Length {0}, Compressed Length {1}",
                            source.Length, destination.Length));

                    // Replace the uncompressed stream with the compressed stream.
                    base.InputStream = destination;
                    // Insert the compressed stream.
                    base.DataInsert();
                    // Restore to the uncompressed stream.
                    base.InputStream = source;
                }
                finally
                {
                    destination.Close();
                    destination.Dispose();
                }
            }
        }
    }
}
