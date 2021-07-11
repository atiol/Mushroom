using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Core.Common.GenericSearch;
using Core.Common.Enums;
using Core.Common.Models.Grid;
using Core.Common.GenericSearch.SearchTypes;
using Core.Common.Helpers;
using Core.Common.Services;
using Core.Data;
using Core.Data.Extensions;
using Core.Entity.Abstracts;
using Core.Model.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Service.Abstracts
{
    public abstract class BaseService<TContext, TEntity, TDtoModel>
        where TEntity : BaseEntity where TDtoModel : BaseDtoModel where TContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly GenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        private readonly UnitOfWork<TContext> _unitOfWork;
        protected readonly ResponseMessageCommonService ResponseMessageCommonService;

        protected BaseService(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetService<UnitOfWork<TContext>>();

            _repository = _unitOfWork.GetRepository<TEntity>();

            _mapper = serviceProvider.GetService<IMapper>();
            
            _httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            
            _configuration = serviceProvider.GetService<IConfiguration>();
            
            ResponseMessageCommonService = serviceProvider.GetService<ResponseMessageCommonService>();
        }

        protected GenericRepository<TEntity> GetRepository()
        {
            return _repository;
        }

        protected IMapper GetMapper()
        {
            return _mapper;
        }

        protected UnitOfWork<TContext> GetUnitOfWork()
        {
            return _unitOfWork;
        }

        protected string GetConfigurationValue(string key)
        {
            return _configuration[key];
        }
        
        protected EnumLanguage GetLanguage()
        {
            var lang = _httpContextAccessor.HttpContext.Request.Headers["X-Lang"].ToString();

            return string.IsNullOrEmpty(lang)
                ? Enum.Parse<EnumLanguage>(_configuration["DefaultLanguage"])
                : Enum.Parse<EnumLanguage>(lang);
        }

        public async Task<IList<TDtoModel>> GetAll(Expression<Func<TEntity, TEntity>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var all = await _repository.GetAllAsQueryable(selector, predicate, orderBy, include).ToListAsync();

            return _mapper.Map<IList<TDtoModel>>(all);
        }

        public async Task<IList<TDtoModel>> GetAllByIds(IEnumerable<long> ids)
        {
            var all = await _repository.GetAllAsQueryable(ids).ToListAsync();

            return _mapper.Map<IList<TDtoModel>>(all);
        }

        public async Task<TDtoModel> GetOneOrNull(Expression<Func<TEntity, TEntity>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var one = await _repository.GetFirstOrDefaultAsQueryable(selector, predicate, include)
                .FirstOrDefaultAsync();

            return one == null ? null : _mapper.Map<TDtoModel>(one);
        }

        public async Task<TDtoModel> GetOneOrNullById(long id)
        {
            var one = await _repository.GetFirstOrDefaultAsQueryable(id).FirstOrDefaultAsync();

            return one == null ? null : _mapper.Map<TDtoModel>(one);
        }

        public async Task<IList<TDtoModel>> GetPagedList(Expression<Func<TEntity, TEntity>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 0,
            int pageSize = 10)
        {
            var all = await _repository.GetAllAsQueryable(selector, predicate, orderBy, include)
                .ToGridResult(pageIndex, pageSize);

            return _mapper.Map<IList<TDtoModel>>(all);
        }

        public async Task<IList<TDtoModel>> GetPagedList(GridSearch search,
            Expression<Func<TEntity, TEntity>> selector = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (search == null)
            {
                return null;
            }

            var query = _repository.GetAllAsQueryable(selector, null, null, include);

            if (search.Sort != null)
            {
                query = query.ApplySortFilter(search.Sort);
            }

            if (search.Filters != null && search.Filters.Any())
            {
                query = query.ApplyBaseSearchList(GenerateSearch(search.Filters.ToList()));
            }

            var all = await query.ToGridResult(search.PageIndex, search.PageSize);

            return _mapper.Map<IList<TDtoModel>>(all);
        }

        public async Task<int> GetCount(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _repository.Count(predicate);
        }

        public async Task<bool> GetExist(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.Exist(predicate);
        }

        public async Task<bool> GetExistById(long id)
        {
            return await _repository.ExistById(id);
        }

        public async Task<IList<TDtoModel>> FromSql(string sql, object[] parameters)
        {
            var all = await _repository.FromSqlAsQueryable(sql, parameters).ToListAsync();

            return _mapper.Map<IList<TDtoModel>>(all);
        }

        public async Task<TDtoModel> Save(TDtoModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            await _repository.Add(entity);
            await _unitOfWork.Commit();

            return _mapper.Map<TDtoModel>(entity);
        }

        public async Task SaveRange(IEnumerable<TDtoModel> models)
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(models);

            await _repository.Add(entities);
            await _unitOfWork.Commit();
        }

        public async Task SaveRange(TDtoModel[] models)
        {
            var entities = _mapper.Map<TEntity[]>(models);

            await _repository.Add(entities);
            await _unitOfWork.Commit();
        }

        public async Task UpdateById(long id, TDtoModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            entity.Id = id;

            _repository.Update(entity);
            await _unitOfWork.Commit();
        }

        public async Task HardDeleteById(long id)
        {
            _repository.Remove(id);
            await _unitOfWork.Commit();
        }
        
        public async Task SoftDeleteById(long id)
        {
            var viewModel = await GetOneOrNullById(id);

            if (viewModel != null)
            {
                viewModel.DeletedDate = DateTime.Now;
                viewModel.DeletedBy = HttpContextHelper.GetUserName();

                await UpdateById(id, viewModel);
            }
        }

        public async Task HardDeleteByIds(IEnumerable<long> ids)
        {
            _repository.Remove(ids);
            await _unitOfWork.Commit();
        }
        
        public async Task SoftDeleteByIds(IEnumerable<long> ids)
        {
            var viewModels = await GetAllByIds(ids);
            
            viewModels.ToList().ForEach(async viewModel =>
            {
                viewModel.DeletedDate = DateTime.Now;
                viewModel.DeletedBy = HttpContextHelper.GetUserName();
                
                await UpdateById(viewModel.Id, viewModel);
            });
        }

        public async Task HardDeleteByExpression(Expression<Func<TEntity, bool>> predicate)
        {
            _repository.Remove(predicate);
            await _unitOfWork.Commit();
        }
        
        public async Task SoftDeleteByExpression(Expression<Func<TEntity, bool>> predicate)
        {
            var viewModels = await GetAll(null, predicate);
            
            viewModels.ToList().ForEach(async viewModel =>
            {
                viewModel.DeletedDate = DateTime.Now;
                viewModel.DeletedBy = HttpContextHelper.GetUserName();
                
                await UpdateById(viewModel.Id, viewModel);
            });
        }

        protected IEnumerable<BaseSearch> GenerateSearch(IList<Filter> filters)
        {
            ICollection<BaseSearch> searches = new List<BaseSearch>();

            if (filters == null || !filters.Any()) return searches;
            
            foreach (var filter in  filters)
            {
                if (filter == null)
                {
                    throw new InvalidOperationException("Search filter is required.");
                }

                if (string.IsNullOrEmpty(filter.Property))
                {
                    throw new InvalidOperationException("Search filter property is required.");
                }

                switch (filter.SearchType)
                {
                        case EnumSearchType.String:
                            searches.Add(new StringSearch
                            {
                                SearchTerm = filter.Value,
                                Property = filter.Property,
                                Comparator = Enum.Parse<EnumStringComparator>(filter.Comparator)
                            });
                            continue;
                        case EnumSearchType.Boolean:
                            searches.Add(new BooleanSearch
                            {
                                SearchTerm = bool.Parse(filter.Value),
                                Property = filter.Property,
                                Comparator = Enum.Parse<EnumBooleanComparator>(filter.Comparator)
                            });
                            continue;
                        case EnumSearchType.Integer:
                            searches.Add(new IntegerSearch
                            {
                                SearchTerm = int.Parse(filter.Value),
                                Property = filter.Property,
                                Comparator = Enum.Parse<EnumNumericComparator>(filter.Comparator)
                            });
                            continue;
                        case EnumSearchType.Long:
                            searches.Add(new LongSearch
                            {
                                SearchTerm = long.Parse(filter.Value),
                                Property = filter.Property,
                                Comparator = Enum.Parse<EnumNumericComparator>(filter.Comparator)
                            });
                            continue;
                        case EnumSearchType.Date:
                            searches.Add(new DateSearch
                            {
                                SearchTerm = DateTime.Parse(filter.Value),
                                Property = filter.Property,
                                Comparator = Enum.Parse<EnumDateComparator>(filter.Comparator)
                            });
                            continue;
                        case EnumSearchType.Guid:
                            searches.Add(new GuidSearch
                            {
                                SearchTerm = Guid.Parse(filter.Value),
                                Property = filter.Property,
                                Comparator = Enum.Parse<EnumGuidComparator>(filter.Comparator)
                            });
                            continue;
                        default:
                            throw new InvalidOperationException("Search filter is not implemented.");
                }
            }

            return searches;
        }
    }
}