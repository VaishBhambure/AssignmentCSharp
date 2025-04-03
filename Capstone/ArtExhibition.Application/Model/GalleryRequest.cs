using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Application.Model
{
    public class GalleryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    
        public List<int> ArtworkIds { get; set; }
    }

}
