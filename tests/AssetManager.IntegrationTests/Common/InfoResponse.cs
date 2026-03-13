using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.IntegrationTests.Common
{
    public class InfoResponse
    {
        public string Student { get; set; } = default!;
        public string Environment { get; set; } = default!;
        public string ServerTimeUtc { get; set; } = default!;
    }
}
