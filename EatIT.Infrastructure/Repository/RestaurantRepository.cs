using AutoMapper;
using EatIT.Core.DTOs;
using EatIT.Core.Entities;
using EatIT.Core.Interface;
using EatIT.Core.Sharing;
using EatIT.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIT.Infrastructure.Repository
{
    public class RestaurantRepository : GenericRepository<Restaurants>, IRestaurantRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;

        public RestaurantRepository(ApplicationDBContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateRestaurantDTO dto)
        {
            if (dto.rimage == null) return false;

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.rimage.FileName)}";
            var relativePath = Path.Combine("images", "restaurants", fileName);

            var fileInfo = _fileProvider.GetFileInfo(relativePath);
            var physicalPath = fileInfo.PhysicalPath;
            var dir = Path.GetDirectoryName(physicalPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var fs = new FileStream(physicalPath, FileMode.Create))
            {
                await dto.rimage.CopyToAsync(fs);
            }

            var restaurant = _mapper.Map<Restaurants>(dto);
            restaurant.RestaurantImg = "/" + relativePath.Replace("\\", "/");
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Restaurants>> GetAllAsync(RestaurantParams restaurantParams)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(int id, UpdateRestaurantDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
