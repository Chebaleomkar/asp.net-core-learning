
using GameStoreApi.Entities;

namespace GameStoreApi.Mapping;

public static class GenreMapping{
    
    public static Genre ToEntity(this CreateGenreDto dto)
    {
        return new Genre
        {
            Name = dto.Name
        };
    }

}