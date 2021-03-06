﻿using System;

namespace NzbDrone.Core.Download.Clients.Deluge
{
    public class DelugeException : DownloadClientException
    {
        public Int32 Code { get; set; }

        public DelugeException(String message, Int32 code)
            :base (message + " (code " + code + ")")
        {
            Code = code;
        }
    }
}
