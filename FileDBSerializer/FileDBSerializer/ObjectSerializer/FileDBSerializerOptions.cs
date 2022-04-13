﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDBSerializing.ObjectSerializer
{
    public class FileDBSerializerOptions
    {
        public FileDBDocumentVersion Version { get; set; }
        public String NoneTag { get; } = "None";
        public Encoding DefaultEncoding { get; set; } = Encoding.UTF8;


        public FileDBSerializerOptions()
        {
            Version = FileDBDocumentVersion.Version1;
        }
    }
}