using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC_Tema1
{
     public class TokenRingFrame
    {

            public bool IsToken { get; set; }
            public int SourceId { get; set; }
            public int DestinationId { get; set; }
            public string Data { get; set; }
            public string Crc { get; set; } 
  
        public bool DestinationFound { get; set; }
        public bool DataCopied { get; set; }
    }

}
