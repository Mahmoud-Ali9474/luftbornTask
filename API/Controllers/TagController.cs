using API.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Core;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;

        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetTags()
        {
            return Ok((await _tagRepository.GetTags()).Select(t => _mapper.Map<KeyValuePairDto>(t)));
        }

    }
}