﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileDBSerializing
{
    public static class VersionDetector
    {
        public static FileDBDocumentVersion GetCompressionVersion(Stream fs)
        {
            if (CheckVersion(fs, FileDBDocumentVersion.Version2))
                return FileDBDocumentVersion.Version2;
            else if (CheckVersion(fs, FileDBDocumentVersion.Version3))
                return FileDBDocumentVersion.Version3;

            //how can we determine Version 1 from Unknown here? :(
            else
                return FileDBDocumentVersion.Version1;
        }

        private static bool CheckVersion(Stream stream, FileDBDocumentVersion version)
        {
            long Position = stream.Position;
            var expectedMagics = Versioning.GetMagicBytes(version);
            int magicByteSize = expectedMagics.Length;

            stream.Position = stream.Length - magicByteSize;
            byte[] magics = new byte[magicByteSize];
            stream.Read(magics, 0, magicByteSize);
            stream.Position = Position;
            return magics.SequenceEqual<byte>(expectedMagics);
        }
    }
}
