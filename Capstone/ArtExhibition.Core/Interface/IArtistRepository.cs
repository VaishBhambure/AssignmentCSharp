using ArtExhibition.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Domain.Interface
{
    public interface IArtistRepository
    {
        Task AddArtistAsync(Artist artist);
    }
}
