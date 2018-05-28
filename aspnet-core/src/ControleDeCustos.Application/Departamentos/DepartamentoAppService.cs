﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ControleDeCustos.Departamentos.Dto;
using ControleDeCustos.Funcionarios.Dto;
using ControleDeCustos.Managers;
using ControleDeCustos.Models;

namespace ControleDeCustos.Departamentos
{
    public class DepartamentoAppService : AsyncCrudAppService<Departamento, DepartamentoDto, int, PagedResultRequestDto, CreateDepartamentoDto, DepartamentoDto>, IDepartamentoAppService
    {
        private readonly IDepartamentoManager _departamentoManager;
        private readonly IFuncionarioManager _funcionarioManager;

        public DepartamentoAppService(
            IRepository<Departamento> repository,
            IDepartamentoManager departamentoManager,
            IFuncionarioManager funcionarioManager
            ) : base(repository)
        {
            _departamentoManager = departamentoManager;
            _funcionarioManager = funcionarioManager;
        }

        public async Task<PagedResultDto<DepartamentoDto>> GetAll()
        {
            var entity = await _departamentoManager.GetAllList();
            return new PagedResultDto<DepartamentoDto>(entity.Count, ObjectMapper.Map<List<DepartamentoDto>>(entity));
        }

        public async Task<ListResultDto<FuncionarioDto>> GetFuncionarios(EntityDto<int> input)
        {
            var entity = await _funcionarioManager.GetAllByDepartamento(input.Id);
            return new ListResultDto<FuncionarioDto>(ObjectMapper.Map<List<FuncionarioDto>>(entity));
        }

        public override async Task<DepartamentoDto> Create(CreateDepartamentoDto input)
        {
            var entity = ObjectMapper.Map<Departamento>(input);
            var output = await _departamentoManager.Create(entity);
            return MapToEntityDto(output);
        }

        public override async Task Delete(EntityDto<int> input)
        {
            await _departamentoManager.Delete(input.Id);
        }

        public override async Task<DepartamentoDto> Update(DepartamentoDto input)
        {
            var entity = ObjectMapper.Map<Departamento>(input);
            var output = await _departamentoManager.Update(entity);
            return MapToEntityDto(output);
        }

        protected override DepartamentoDto MapToEntityDto(Departamento entity)
        {
            return new DepartamentoDto
            {
                Id = entity.Id,
                Nome = entity.Nome,
                QuantidadeFuncionarios = entity.Funcionarios == null ? 0 : entity.Funcionarios.Count
            };
        }
    }
}