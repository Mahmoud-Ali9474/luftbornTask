using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.Core.Models;
using API.Core;

using API.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IMapper mapper, ICategoryRepository repository, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._categoryRepository = repository;
            this._mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "manager,admin")]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryQueryDto categoryQueryDto)
        {
            var categoryQuery = _mapper.Map<CategoryQueryDto, CategoryQuery>(categoryQueryDto);
            var result = await _categoryRepository.GetCategories(categoryQuery);
            return Ok(_mapper.Map<QueryResult<Category>, QueryResultDto<CategoryDto>>(result));
        }
        [Authorize(Roles = "admin")]

        [HttpPost]
        public async Task<IActionResult> CreateCategory(SaveCategoryDto saveCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<SaveCategoryDto, Category>(saveCategoryDto);

            await _categoryRepository.Add(category);
            await _unitOfWork.CompleteAsync();

            category = await _categoryRepository.GetCategory(category.Id);

            var result = _mapper.Map<Category, CategoryDto>(category);

            return Ok(result);
        }
        [Authorize(Roles = "admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategor(int id, SaveCategoryDto saveCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryRepository.GetCategory(id);

            if (category == null)
                return NotFound();

            _mapper.Map<SaveCategoryDto, Category>(saveCategoryDto, category);
            await _categoryRepository.Update(category);
            await _unitOfWork.CompleteAsync();

            category = await _categoryRepository.GetCategory(category.Id);
            var result = _mapper.Map<Category, CategoryDto>(category);

            return Ok(result);
        }
        [Authorize(Roles = "admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var category = await _categoryRepository.GetCategory(id, includeRelated: false);

            if (category == null)
                return NotFound();

            _categoryRepository.Remove(category);
            await _unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepository.GetCategory(id);

            if (category == null)
                return NotFound();

            var categoryDto = _mapper.Map<Category, CategoryDto>(category);

            return Ok(categoryDto);
        }
    }
}